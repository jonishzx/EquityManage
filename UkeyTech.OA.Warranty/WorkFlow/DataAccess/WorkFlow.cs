using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security;
using Clover.Core.Logging;
using Clover.Core.XCrypt;
using Clover.Data;
using Clover.Net.Mail;
using FireWorkflow.Net.Engine;
using FireWorkflow.Net.Model;
using FireWorkflow.Net.Model.Net;
using StructureMap;
using UkeyTech.WebFW.DAO;
using UkeyTech.WebFW.Model;

namespace UkeyTech.OA.Warranty.WorkFlow.DataAccess
{
    /// <summary>
    ///     WorkFlowDAO 数据访问层
    /// </summary>
    public class WorkFlowDao : BaseDAO
    {
        #region 构造函数

        #endregion

        /// <summary>
        ///     针对获取工作流程中涉及操作员的名称处理器
        /// </summary>
        /// <param name="actorId"></param>
        /// <returns></returns>
        public delegate string WfUserNameDealer(string actorId, IWorkItem wi);

        /// <summary>
        /// 根据业务ID获取当前流程步骤的名称
        /// </summary>
        /// <param name="bizId">业务ID</param>
        public static ITaskInstance GetBizCurrAcivityName(string bizId){

            var persvc = FireWorkflow.Net.Engine.RuntimeContextFactory.getRuntimeContext().PersistenceService;
            var tasklist = persvc.FindLastTaskInstancesForBizId(bizId);
            if (tasklist != null && tasklist.Any())
            {
                return tasklist.First();
            }
            return null;
        }

        /// <summary>
        /// 根据业务ID获取当前流程步骤的名称
        /// </summary>
        /// <param name="bizId">业务ID</param>
        public static List<IWorkItem> GetWorkItemsByBizId(string bizId, string creator){

            var persvc = FireWorkflow.Net.Engine.RuntimeContextFactory.getRuntimeContext().PersistenceService;
            var tasklist = persvc.FindLastTaskInstancesForBizId(bizId);
            if (tasklist != null && tasklist.Any())
            {
                return persvc.FindHaveDoneWorkItems(creator, tasklist.First().ProcessInstanceId);
            }
            return null;
        }
        /// <summary>
        ///     判断是否存在系长\科长
        /// </summary>
        /// <param name="currGroupId">当前用户所在部门(申请部门)的CurrGroupId</param>
        /// <returns></returns>
        public void IsExistManager(string currGroupId, ref Dictionary<string, object> wfdict)
        {
            string grSql = string.Format(@"select GroupID,ParentID,ParentPath from dbo.CPM_Group");
            DataTable dtGroup = GetDataTable(grSql);

            string posSql = string.Format(@"select distinct a.PositionName,b.GroupID,c.AdminName 
                                        from dbo.CPM_Position a
                                        inner join dbo.CPM_Position_User b on a.PositionID=b.PositionID
                                        inner join dbo.sys_Admin c on b.userID=c.AdminID");
            DataTable dtPosition = GetDataTable(posSql);


            //判断系长是否存在，加休假条件过滤
            string pClSql = string.Format(@"select distinct a.PositionName,b.GroupID,c.adminId,c.AdminName 
                from dbo.CPM_Position a
                inner join dbo.CPM_Position_User b on a.PositionID=b.PositionID and PositionName = '系长'
                inner join dbo.sys_Admin c on b.userID=c.AdminID
                where not exists (select 1 from dbo.T_FF_CAL_USERCALENDAR cu where state = 1 and cu.userid = c.adminId and (getdate() between cu.Begin_date and end_date))    
            ");

            DataTable clDtPosition = GetDataTable(pClSql);

            //系长判断
            var isManager = CheckSection(dtGroup, clDtPosition, currGroupId, "'系长'");
           
            //科长判断
            var isSection = CheckSection(dtGroup, dtPosition, currGroupId, "'科长','副科长','科长助理'");

            //副部长判断
            var isSubSecretary = CheckSection(dtGroup, dtPosition, currGroupId, "'副部长'");

            //部长判断
            var isSecretary = CheckSection(dtGroup, dtPosition, currGroupId, "'部长'");

            //次长判断
            var isMinister = CheckSection(dtGroup, dtPosition, currGroupId, "'次长'");

            if (wfdict.ContainsKey("IsSubSecretary"))
            {
                wfdict["IsSubSecretary"] = (isSubSecretary ? "true" : "false");
            }
            else
                wfdict.Add("IsSubSecretary", (isSubSecretary ? "true" : "false"));

            if (wfdict.ContainsKey("IsSecretary"))
            {
                wfdict["IsSecretary"] = (isSecretary ? "true" : "false");
            }
            else
                wfdict.Add("IsSecretary", (isSecretary ? "true" : "false"));

            if (wfdict.ContainsKey("IsExistManager"))
            {
                wfdict["IsExistManager"] = (isManager ? "true" : "false");
            }
            else
                wfdict.Add("IsExistManager", (isManager ? "true" : "false"));

            if (wfdict.ContainsKey("IsExistSection"))
            {
                wfdict["IsExistSection"] = (isSection ? "true" : "false");
            }
            else
                wfdict.Add("IsExistSection", (isSection ? "true" : "false"));

            if (wfdict.ContainsKey("IsMinister"))
            {
                wfdict["IsMinister"] = (isMinister ? "true" : "false");
            }
            else
                wfdict.Add("IsMinister", (isMinister ? "true" : "false"));
        }

        /// <summary>
        ///     判断是否存在科长，一级一级往上查询
        /// </summary>
        /// <param name="dtGroup"></param>
        /// <param name="dtPosition"></param>
        /// <param name="groupId"></param>
        /// <param name="groupNames"></param>
        /// <returns></returns>
        private bool CheckSection(DataTable dtGroup, DataTable dtPosition, string groupId, string groupNames)
        {
            if (string.IsNullOrEmpty(groupId))
                return false;

            bool flag = true;
            bool isSection =
                dtPosition.Select(string.Format(" (PositionName in ({1}))  AND GroupID='{0}'", groupId, groupNames)).Any();
            if (!isSection)
            {
                //flag = false;
                DataRow[] dr = dtGroup.Select(string.Format(" GroupID='{0}'", groupId));
                if (dr.Length > 0)
                {
                    string parentId = dr[0]["ParentID"].ToString();
                    flag = CheckSection(dtGroup, dtPosition, parentId, groupNames);
                }
                else
                    flag = false;
            }
            return flag;
        }

        /// <summary>
        /// 获取正部长；
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public static bool IsSecretary(string adminId)
        {
            bool isSecretary = false;

            string posSql = string.Format(@"select distinct a.PositionName,b.GroupID,c.AdminName,c.AdminID
                                        from dbo.CPM_Position a
                                        inner join dbo.CPM_Position_User b on a.PositionID=b.PositionID
                                        inner join dbo.sys_Admin c on b.userID=c.AdminID");
            DataTable dtPosition = GetDataTable(posSql);

            if (dtPosition.Select(string.Format(" AdminID='{0}' AND PositionName='部长'", adminId)).Any())
            {
                isSecretary = true;
            }

            return isSecretary;
        }


        /// <summary>
        /// 获取副部长；
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public static bool IsSubSecretary(string adminId)
        {
            bool isSubSecretary = false;

            string posSql = string.Format(@"select distinct a.PositionName,b.GroupID,c.AdminName,c.AdminID
                                        from dbo.CPM_Position a
                                        inner join dbo.CPM_Position_User b on a.PositionID=b.PositionID
                                        inner join dbo.sys_Admin c on b.userID=c.AdminID");
            DataTable dtPosition = GetDataTable(posSql);


            if (dtPosition.Select(string.Format(" AdminID='{0}' AND PositionName='副部长'", adminId)).Any())
            {
                isSubSecretary = true;
            }

            return isSubSecretary;
        }


        /// <summary>
        /// 判断是否对应的岗位；
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="positionName"></param>
        /// <returns></returns>
        public static bool CheckSection(string adminId,string positionName)
        {
            bool isFlag = false;

            string posSql = string.Format(@"select distinct a.PositionName,b.GroupID,c.AdminName,c.AdminID
                                        from dbo.CPM_Position a
                                        inner join dbo.CPM_Position_User b on a.PositionID=b.PositionID
                                        inner join dbo.sys_Admin c on b.userID=c.AdminID");
            DataTable dtPosition = GetDataTable(posSql);

            if (dtPosition.Select(string.Format(" AdminID='{0}' AND PositionName='{1}'", adminId, positionName)).Any())
            {
                isFlag = true;
            }

            return isFlag;
        }


        /// <summary>
        ///     获取流程实例ID
        /// </summary>
        /// <param name="billno"></param>
        /// <returns></returns>
        public DataTable GetInstanceId(string billno)
        {
            string sql = string.Format("select Processinstance_ID from T_FF_RT_PROCINST_VAR WHERE [Value]='{0}'", billno);
            return GetDataTable(sql);
        }

        /// <summary>
        ///     获取相关部门部长正副领导
        /// </summary>
        /// <param name="budgetDeptCode"></param>
        /// <returns></returns>
        public DataTable GetRelateDeptByCode(string budgetDeptCode)
        {
            string sql = string.Format(@"select gu.GroupID,ru.UserID
,cg.GroupCode 
from dbo.CPM_Role_User ru
inner join dbo.CPM_Role cr on ru.roleid=cr.roleid
inner join  dbo.CPM_Group_User gu on  ru.userID=gu.userID
inner join CPM_Group cg on gu.groupId=cg.GroupId 
inner join sys_admin sa on sa.AdminId=ru.UserID 
where cr.RoleName like '%正副部长' and sa.BudgetDeptCode='{0}'", budgetDeptCode);
            return GetDataTable(sql);
        }

        /// <summary>
        ///     获取相关部门部长正副领导
        /// </summary>
        /// <returns></returns>
        public DataTable GetRelateDeptByGroupCode(string groupCode)
        {
            string sql = string.Format(@"select gu.GroupID,ru.UserID
,cg.GroupCode 
from dbo.CPM_Role_User ru
inner join dbo.CPM_Role cr on ru.roleid=cr.roleid
inner join  dbo.CPM_Group_User gu on  ru.userID=gu.userID
inner join CPM_Group cg on gu.groupId=cg.GroupId 
inner join sys_admin sa on sa.AdminId=ru.UserID 
where cr.RoleName like '%正副部长' and cg.GroupCode='{0}'", groupCode);
            return GetDataTable(sql);
        }

        /// <summary>
        ///     获取相关部门副部长
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public DataTable GetRelateDeptSecManager(string GroupCode)
        {
            string sql = string.Format(@"select distinct a.PositionName,b.GroupID,c.AdminName,
c.AdminID,
cg.GroupCode
from dbo.CPM_Position a
inner join dbo.CPM_Position_User b on a.PositionID=b.PositionID
inner join dbo.sys_Admin c on b.userID=c.AdminID
inner join CPM_Group cg on b.groupId=cg.GroupId
where GroupCode='{0}' AND PositionName='副部长'", GroupCode);
            return GetDataTable(sql);
        }

        /// <summary>
        ///     获取相关部门正部长
        /// </summary>
        /// <returns></returns>
        public DataTable GetRelateDeptManager(string groupCode)
        {
            string sql = string.Format(@"select distinct a.PositionName,b.GroupID,c.AdminName,
c.AdminID,
cg.GroupCode
from dbo.CPM_Position a
inner join dbo.CPM_Position_User b on a.PositionID=b.PositionID
inner join dbo.sys_Admin c on b.userID=c.AdminID
inner join CPM_Group cg on b.groupId=cg.GroupId
where GroupCode='{0}' AND PositionName='部长'", groupCode);
            return GetDataTable(sql);
        }

        /// <summary>
        ///     获取流程环节的审批人
        /// </summary>
        /// <param name="wiss"></param>
        /// <param name="sortacts"></param>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public static string GetName(List<IWorkItem> wiss, List<Activity> sortacts, string displayName)
        {
            return GetName(wiss, sortacts, displayName, null);
        }

        /// <summary>
        ///     获取流程环节的审批人
        /// </summary>
        /// <param name="wiss">工作项</param>
        /// <param name="sortacts">排序的步骤</param>
        /// <param name="displayName">流程步骤名称</param>
        /// <param name="dealer">用户名获取二次加工委托</param>
        /// <returns></returns>
        public static string GetName(List<IWorkItem> wiss, List<Activity> sortacts, string displayName,
                                     WfUserNameDealer dealer)
        {
            var idlist = new Dictionary<string, string>();
            Activity act = sortacts.FirstOrDefault(o => o.DisplayName == displayName);

            if (act != null)
            {
                foreach (Task task in act.InlineTasks)
                {
                    List<IWorkItem> wis = wiss.FindAll(x => x.TaskInstance.ActivityId == act.Id
                                                            && x.State == WorkItemEnum.COMPLETED);
                    int c = wis.Count;

                    foreach (IWorkItem wi in wis)
                    {
                        c--;

                        string adminName = !string.IsNullOrEmpty(wi.CompleteActorId) ? 
                            AdminDAO.getAdminName(wi.CompleteActorId) : AdminDAO.getAdminName(wi.ActorId);

                        if ((string.IsNullOrEmpty(adminName) || adminName == "无")
                            && !idlist.ContainsKey("无"))
                        {
                            idlist.Add("无", "");
                        }
                        else
                        {
                            //处理人不为空或无
                            string actorid = string.IsNullOrEmpty(wi.CompleteActorId) ? wi.ActorId : wi.CompleteActorId;
                            string text = string.Empty;
                            if (dealer != null)
                                text = dealer(actorid, wi);
                            else
                                text = adminName + "(" + ((wi.CompleteActorId != wi.ActorId) ? "(代)" : "") + //"\n" +
                                       (wi.EndTime != null ? wi.EndTime.Value.ToString("yyyy/MM/dd") : "") + ")";

                            if (!idlist.ContainsKey(actorid)
                                && wi.Comments.IndexOf(EngineConstant.PassStatusName) >= 0
                                && wi.Comments.IndexOf(EngineConstant.RejectStatusName) < 0)
                            {
                                //未在字段中的人设置为“用户名（完成时间）”
                                idlist.Add(actorid, text);
                            }
                            else if (idlist.ContainsKey(actorid)
                                     //如果同一个步骤一人审核多次,取最后一次更新时间
                                     && wi.Comments.IndexOf(EngineConstant.PassStatusName) >= 0
                                     && wi.Comments.IndexOf(EngineConstant.RejectStatusName) < 0)
                            {
                                idlist[actorid] = text;
                            }
                        }
                    } //end foreach
                }
            }

            //return idlist.Count > 0 ? string.Join(",", idlist.Values.ToArray()) : "/";
            return string.Join("\n", idlist.Values.ToArray());
        }

        /// <summary>
        ///     获取流程环节的审批人(这个方法只用于：获取 预算调整单“财务担当审批”的审批人，专用)
        /// </summary>
        /// <param name="wiss">工作项</param>
        /// <param name="sortacts">排序的步骤</param>
        /// <param name="displayName">流程步骤名称</param>
        /// <param name="dealer">用户名获取二次加工委托</param>
        /// <returns></returns>
        public static string GetNameForAdjust(List<IWorkItem> wiss, List<Activity> sortacts, string displayName,
                                     WfUserNameDealer dealer)
        {
            var idlist = new Dictionary<string, string>();
            List<Activity> lstact = sortacts.Where(o => o.DisplayName == displayName).ToList<Activity>();
            foreach (Activity act in lstact)
            {
                if (act != null)
                {
                    foreach (Task task in act.InlineTasks)
                    {
                        List<IWorkItem> wis = wiss.FindAll(x => x.TaskInstance.ActivityId == act.Id
                                                                && x.State == WorkItemEnum.COMPLETED);
                        int c = wis.Count;

                        foreach (IWorkItem wi in wis)
                        {
                            c--;
                            string adminName = !string.IsNullOrEmpty(wi.CompleteActorId) ?
                             AdminDAO.getAdminName(wi.ActorId) : AdminDAO.getAdminName(wi.ActorId);

                            if ((string.IsNullOrEmpty(adminName) || adminName == "无")
                                && !idlist.ContainsKey("无"))
                            {
                                idlist.Add("无", "/");
                            }
                            else
                            {
                                //处理人不为空或无
                                string actorid = string.IsNullOrEmpty(wi.CompleteActorId) ? wi.ActorId : wi.CompleteActorId;
                                string text = string.Empty;
                                if (dealer != null)
                                    text = dealer(actorid, wi);
                                else
                                    text = adminName + ((wi.CompleteActorId != wi.ActorId) ? "(代)" : "") + //"\n" +
                                           (wi.EndTime != null ? wi.EndTime.Value.ToString("yyyy-MM-dd") : "");

                                if (!idlist.ContainsKey(actorid)
                                    && wi.Comments.IndexOf(EngineConstant.PassStatusName) >= 0
                                    && wi.Comments.IndexOf(EngineConstant.RejectStatusName) < 0)
                                {
                                    //未在字段中的人设置为“用户名（完成时间）”
                                    idlist.Add(actorid, text);
                                }
                                else if (idlist.ContainsKey(actorid)
                                    //如果同一个步骤一人审核多次,取最后一次更新时间
                                         && wi.Comments.IndexOf(EngineConstant.PassStatusName) >= 0
                                         && wi.Comments.IndexOf(EngineConstant.RejectStatusName) < 0)
                                {
                                    idlist[actorid] = text;
                                }
                            }
                        } //end foreach
                    }
                }
            }

            //return idlist.Count > 0 ? string.Join(",", idlist.Values.ToArray()) : "/";
            return string.Join(",", idlist.Values.ToArray());
        }

        /// <summary>
        ///     获取流程环节的审批人
        /// </summary>
        /// <param name="wiss">工作项</param>
        /// <param name="sortacts">排序的步骤</param>
        /// <param name="displayName">流程步骤名称</param>
        /// <param name="dealer">用户名获取二次加工委托</param>
        /// <returns></returns>
        public static string GetName(List<IWorkItem> wiss, List<Activity> sortacts, string[] displayName,
                                     WfUserNameDealer dealer)
        {
            
            var idlist = new Dictionary<string, string>();
         
            List<Activity> lstact = sortacts.Where(o => displayName.Contains(o.DisplayName)).ToList();
            foreach(Activity act in lstact)
            {
                if (act != null)
                {
                    foreach (Task task in act.InlineTasks)
                    {
                        List<IWorkItem> wis = wiss.FindAll(x => x.TaskInstance.ActivityId == act.Id
                                                                && x.State == WorkItemEnum.COMPLETED);
                        int c = wis.Count;

                        foreach (IWorkItem wi in wis)
                        {
                            c--;
                            string adminName = AdminDAO.getAdminName(wi.ActorId);

                            if ((string.IsNullOrEmpty(adminName) || adminName == "无")
                                && !idlist.ContainsKey("无"))
                            {
                                idlist.Add("无", "/");
                            }
                            else
                            {
                                //处理人不为空或无
                                string actorid = string.IsNullOrEmpty(wi.CompleteActorId) ? wi.ActorId : wi.CompleteActorId;
                                string text = string.Empty;
                                if (dealer != null)
                                    text = dealer(actorid, wi);
                                else
                                    text = adminName + ((wi.CompleteActorId != wi.ActorId) ? "(代)" : "") + //"\n" +
                                           (wi.EndTime != null ? wi.EndTime.Value.ToString("yyyy-MM-dd") : "");

                                if (!idlist.ContainsKey(actorid)
                                    && wi.Comments.IndexOf(EngineConstant.PassStatusName) >= 0 
                                    && wi.Comments.IndexOf(EngineConstant.RejectStatusName) < 0)
                                {
                                    //未在字段中的人设置为“用户名（完成时间）”
                                    idlist.Add(actorid, text);
                                }
                                else if (idlist.ContainsKey(actorid)
                                    //如果同一个步骤一人审核多次,取最后一次更新时间
                                         && wi.Comments.IndexOf(EngineConstant.PassStatusName) >= 0 
                                         && wi.Comments.IndexOf(EngineConstant.RejectStatusName) < 0)
                                {
                                    idlist[actorid] = text;
                                }
                            }
                        } //end foreach
                    }
                }
            }

            //return idlist.Count > 0 ? string.Join(",", idlist.Values.ToArray()) : "/";

            if (idlist.Where(o => o.Key == "无").Count() != idlist.Count) {
                idlist = idlist.Where(o => o.Key != "无").ToDictionary(o => o.Key, o => o.Value);
            }
            return idlist.Count > 0 ? string.Join("\n/", idlist.Values.ToArray()) : "/";
        }

        /// <summary>
        ///     获取流程环节的审批人
        /// </summary>
        /// <param name="wiss"></param>
        /// <param name="sortacts"></param>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public static void GetManager(List<IWorkItem> wiss, List<Activity> sortacts, string displayName,
                                      ref Dictionary<string, object> wfdict)
        {
            Activity act = sortacts.Where(o => o.DisplayName == displayName).FirstOrDefault();
            if (act != null)
            {
                var dept1 = new Dictionary<string, string>();
                var dept2 = new Dictionary<string, string>();
                foreach (Task task in act.InlineTasks)
                {
                    List<IWorkItem> wis = wiss.FindAll(x => x.TaskInstance.ActivityId == act.Id
                                                            && x.State == WorkItemEnum.COMPLETED);
                    int c = wis.Count;

                    foreach (IWorkItem wi in wis)
                    {
                        c--;
                        string adminName = AdminDAO.getAdminName(wi.ActorId);
                        string text = string.Empty;

                        if (string.IsNullOrEmpty(adminName) || adminName == "无")
                        {
                            text = "/";
                        }
                        else if (wi.Comments.IndexOf(EngineConstant.PassStatusName) >= 0)
                        {
                            text = adminName + "\n" +
                                   (wi.EndTime != null ? wi.EndTime.Value.ToString("yyyy-MM-dd") : "");
                        }

                        if (IsSubSecretary(wi.ActorId))
                        {
                            if (dept1.ContainsKey(wi.CompleteActorId)) //如果同一个步骤一人审核多次,取最后一次更新时间,所以要迭代
                                dept1.Remove(wi.CompleteActorId);
                            dept1.Add(wi.CompleteActorId, text);
                        }
                        if (IsSecretary(wi.ActorId))
                        {
                            if (dept2.ContainsKey(wi.CompleteActorId)) //如果同一个步骤一人审核多次,取最后一次更新时间,所以要迭代
                                dept2.Remove(wi.CompleteActorId);
                            dept2.Add(wi.CompleteActorId, text);
                        }
                    }
                }

                wfdict["dept1"] = dept1.Count > 0 ? string.Join(",", dept1.Values.ToArray()) : "/";
                wfdict["dept2"] = dept2.Count > 0 ? string.Join(",", dept2.Values.ToArray()) : "/";
            }
        }


        /// <summary>
        ///     获取流程环节的科长助理、副科长、科长；
        /// </summary>
        /// <param name="wiss"></param>
        /// <param name="sortacts"></param>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public static void GetSection(List<IWorkItem> wiss, List<Activity> sortacts, string displayName,
                                      ref Dictionary<string, object> wfdict)
        {
            Activity act = sortacts.Where(o => o.DisplayName == displayName).FirstOrDefault();
            if (act != null)
            {
                var dept1 = new Dictionary<string, string>();
                var dept2 = new Dictionary<string, string>();
                var dept3 = new Dictionary<string, string>();
                foreach (Task task in act.InlineTasks)
                {
                    List<IWorkItem> wis = wiss.FindAll(x => x.TaskInstance.ActivityId == act.Id
                                                            && x.State == WorkItemEnum.COMPLETED);
                    int c = wis.Count;

                    foreach (IWorkItem wi in wis)
                    {
                        c--;
                        string adminName = AdminDAO.getAdminName(wi.ActorId);
                        string text = string.Empty;

                        if (string.IsNullOrEmpty(adminName) || adminName == "无")
                        {
                            text = "/";
                        }
                        else if (wi.Comments.IndexOf(EngineConstant.PassStatusName) >= 0)
                        {
                            text = adminName + "\n" +
                                   (wi.EndTime != null ? wi.EndTime.Value.ToString("yyyy-MM-dd") : "");
                        }

                        if (CheckSection(wi.ActorId, "副科长"))//副科长
                        {
                            if (dept1.ContainsKey(wi.CompleteActorId)) //如果同一个步骤一人审核多次,取最后一次更新时间,所以要迭代
                                dept1.Remove(wi.CompleteActorId);
                            dept1.Add(wi.CompleteActorId, text);
                        }
                        if (CheckSection(wi.ActorId, "科长助理"))//科长助理
                        {
                            if (dept2.ContainsKey(wi.CompleteActorId)) //如果同一个步骤一人审核多次,取最后一次更新时间,所以要迭代
                                dept2.Remove(wi.CompleteActorId);
                            dept2.Add(wi.CompleteActorId, text);
                        }
                        if (CheckSection(wi.ActorId, "科长"))//科长
                        {
                            if (dept3.ContainsKey(wi.CompleteActorId)) //如果同一个步骤一人审核多次,取最后一次更新时间,所以要迭代
                                dept3.Remove(wi.CompleteActorId);
                            dept3.Add(wi.CompleteActorId, text);
                        }
                    }
                }

                wfdict["dept1"] = dept1.Count > 0 ? string.Join(",", dept1.Values.ToArray()) : "/";
                wfdict["dept2"] = dept2.Count > 0 ? string.Join(",", dept2.Values.ToArray()) : "/";
                wfdict["dept3"] = dept3.Count > 0 ? string.Join(",", dept3.Values.ToArray()) : "/";
            }
        }

        /// <summary>
        /// 获取流程实例的相关内容
        /// </summary>
        /// <param name="processInstanceId">流程实例ID</param>
        /// <param name="sender">发送人ID</param>
        /// <returns></returns>
        public string GetContactMailTitle(string processInstanceId, string sender)
        {
            RuntimeContext ctx = RuntimeContextFactory.getRuntimeContext();
            IProcessInstance proc = ctx.PersistenceService.FindProcessInstanceById(processInstanceId);

            if (proc == null)
                throw new NullReferenceException("processInstanceId对应的流程实例无效");

            Admin adminmodel = ObjectFactory.GetInstance<AdminDAO>().GetModel(sender);
            Admin porcCreator = ObjectFactory.GetInstance<AdminDAO>().GetModel(proc.CreatorId);

            string title = string.Format("{0}[{1}]联系通知邮件", proc.DisplayName, proc.BizId);
            string content = string.Format(@"{0}，你好：我是{3}，由于我对当前处理的{1}[单号：{2}]存在疑问，请当面向我说明具体情况，谢谢！",
                porcCreator.AdminName, 
                proc.DisplayName,
                proc.BizId,
                adminmodel.AdminName);
            return title + "#$#" + content;
        }

        /// <summary>
        /// 发送联系我邮件给流程发起人
        /// </summary>
        /// <param name="processInstanceId">流程实例ID</param>
        /// <param name="sender">发送人ID</param>
        /// <param name="title">标题</param>
        /// <param name="content">发送内容</param>
        /// <returns></returns>
        public void SendContactMail(string processInstanceId, string sender, string title, string content, string sendCCUser)
        {
            string smtpserver = ConfigurationManager.AppSettings["SMTPSvr"];
            string user = ConfigurationManager.AppSettings["SMTPSvrUserName"];
            string pwd = XCryptEngine.Current().Decrypt(ConfigurationManager.AppSettings["SMTPSvrPassword"]);
            string mailform = ConfigurationManager.AppSettings["SMTPSvrUserName"];

            RuntimeContext ctx = RuntimeContextFactory.getRuntimeContext();
            IProcessInstance proc = ctx.PersistenceService.FindProcessInstanceById(processInstanceId);

            if (proc == null)
                throw new NullReferenceException("processInstanceId对应的流程实例无效");

            ILogger logger = LogCentral.Current.GetLoggerByName("Default");

            //使用当前人地址发送
            Admin adminmodel = ObjectFactory.GetInstance<AdminDAO>().GetModel(sender);
            Admin porcCreator = ObjectFactory.GetInstance<AdminDAO>().GetModel(proc.CreatorId);
            Admin adminCCmodel = ObjectFactory.GetInstance<AdminDAO>().GetModel(sendCCUser);

            if (adminmodel != null && !string.IsNullOrEmpty(adminmodel.Email) &&
                !string.IsNullOrEmpty(adminmodel.EmailPwd))
            {
                mailform = adminmodel.Email;
                user = adminmodel.Email;
                pwd = adminmodel.EmailPwd;
            }
            var client = new MailSmtpClient(
                smtpserver, user, pwd);


            if (!client.Authenticate())
            {
                throw new SecurityException("你无权限访问邮箱：" + mailform + ",请检查配置文件中的邮箱登录用户名及密码");
            }

            string mailto = porcCreator.Email;

            if (string.IsNullOrEmpty(mailto))
            {
                throw new Exception("申请人未填写E-mail,无法发送邮件！");
            }

            string mailCC = "";

            if (adminCCmodel != null && !string.IsNullOrEmpty(adminCCmodel.Email))
            {
                mailCC = adminCCmodel.Email;
            }

            logger.Info("邮箱登录成功");
            client.IsBodyHtml = true;

            client.MailSubject = title;
            client.MailBody = content;
            client.MailFrom = mailform;
            client.MailTo = mailto.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
            client.MailCc = mailCC.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (string.IsNullOrEmpty(mailto))
            {
                throw new Exception("申请人未填写E-mail,无法发送邮件！");
            }

            try
            {
                bool mailrst = client.Send();

                if (mailrst)
                {
                    logger.Info(title + "通知邮件发送成功");
                }
                else
                {
                    logger.Info(title + "通知邮件发送s失败");
                }
            }
            catch (Exception ex)
            {
                logger.Error(title + " 发送邮件到" + client.MailTo + "失败!");
                logger.Error("邮件发送失败", ex);
                throw;
            }


            logger.Info("邮件发送完成...");
        }

        #region 财务担当环节

        /// <summary>
        ///     判断是否存在系长\科长
        /// </summary>
        /// <param name="currGroupId">当前用户所在部门(申请部门)的CurrGroupId</param>
        /// <returns></returns>
        public void IsManager(string currGroupId, ref Dictionary<string, object> wfdict)
        {
            bool IsManager = false;
            bool IsSection = false;

            string grSql = string.Format(@"select GroupID,ParentID,ParentPath from dbo.CPM_Group");
            DataTable DtGroup = GetDataTable(grSql);

            string posSql = string.Format(@"select distinct a.PositionName,b.GroupID,c.AdminName 
                                        from dbo.CPM_Position a
                                        inner join dbo.CPM_Position_User b on a.PositionID=b.PositionID
                                        inner join dbo.sys_Admin c on b.userID=c.AdminID");
            DataTable DtPosition = GetDataTable(posSql);

            IsManager = DtPosition.Select(string.Format(" PositionName='系长' AND GroupID='{0}'", currGroupId)).Count() >
                        0;

            IsSection = CheckSection(DtGroup, DtPosition, currGroupId, "'科长','副科长','科长助理'");

            wfdict.Add("IsManager", (IsManager ? "true" : "false"));
            wfdict.Add("IsSection", (IsSection ? "true" : "false"));
        }

        #endregion

    }
}