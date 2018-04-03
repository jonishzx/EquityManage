using System;
using System.Data;
using Clover.Data;
using Dapper;
namespace UkeyTech.OA.Warranty.Pub
{
    /// <summary>
    /// AccountTitle 数据访问层
    /// </summary>
    public partial class BaseFuntion : BaseDAO
    {
        #region 构造函数

        /// <summary>
        /// 单据类型：
        /* 
        PC 采购合同
        PG 采购合同数量单
        PB 采购大订单
        PO 采购小订单
        GE 入库单
        PP 预付款单
        PM 付款单
        IR 收工厂发票
         
        SC 销售合同
        SG 销售合同数量单
        SB 销售大订单
        SO 销售小订单     
        OS 出库单
        CD 报关单
        PR 预收汇单
        RC 收汇单
        ES 结汇单
        AC 销售发票申请
        ID 开销售发票        
        */
        /// </summary>
        /// <param name="billtype"></param>
        /// <returns></returns>
        public string GetBillId(string billtype)
        {
            string pBillId = "NO_BILL";

            using (var conn = Clover.Data.BaseDAO.ManualDbService())
            {
                try
                {
                    var p = new DynamicParameters();
                    p.Add("billtype", billtype, DbType.String, ParameterDirection.Input, 10);
                    p.Add("pBillID", pBillId, DbType.String, ParameterDirection.Output, 30);
                    conn.Execute("PROC_getBillId", p, null, CommandTimeout, CommandType.StoredProcedure);
                    pBillId = p.Get<string>("pBillID");
                }
                catch (DataException ex)
                {
                    pBillId = "NO_BILL";
                    throw ex;
                }
            }

            return pBillId;
        }

   
        #endregion


        #region todo 统计列表
        /// <summary>
        /// 获取待办统计信息
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>
        public DataTable GetTodoStat(string userid)
        {
            string sql = string.Format(@"
                select * from (      
                    SELECT   tfrp.Process_ID ,
                                        tfrp.DISPLAY_NAME WorkName ,
                                        wi.DISPLAY_NAME TaskName,
                                        COUNT(*) AllWork ,
                                        0 NormalWork ,
                                        0 OverWork ,
                                        'A' WorkType ,
                                        cf.ExternalAuditListUrl
                               FROM     dbo.T_FF_RT_WORKITEM wi                        
                                        JOIN dbo.T_FF_RT_PROCESSINSTANCE tfrp ON wi.PROCESSINSTANCE_ID = tfrp.ID
                                        LEFT JOIN dbo.CF_Form cf ON tfrp.PROCESS_ID = cf.PROCESS_ID
                                        JOIN v_Base_AllWorkFlowBill ab ON ab.BillNo = tfrp.BIZ_ID 
                               WHERE    wi.STATE>=0 and  wi.STATE<=1 
                                        AND ( wi.ACTOR_ID = '{0}'
                                              OR EXISTS ( SELECT    1
                                                          FROM      CF_AssignUserList caul
                                                          WHERE     caul.UserId = wi.ACTOR_ID
                                                                   and caul.FORMID=tfrp.Biz_TYPE_id
                                                                   AND convert(varchar(16),getdate(),121)  between convert(varchar(16),caul.AssginBeginDate,121)  
                                                                   and convert(varchar(16),caul.AssginEndDate,121)
                                    AND caul.AssignToUserId = '{0}' )
                                            )
                               GROUP BY tfrp.Process_ID ,
                                        tfrp.DISPLAY_NAME ,
                                        cf.ExternalAuditListUrl,
                                        wi.DISPLAY_NAME) xx where allwork > 0 ", userid);

            var data = Clover.Data.BaseDAO.GetDataTable(sql);

            return data;
        }







        /// <summary>
        ///     查询所有记录，并排序、分页(事务)
        /// </summary>
        public DataTable GetAduitAllData(string actor,
            string billNo, string tableName, string keyName, out int rstcount)
        {

            string sql = @"(SELECT hep." + keyName + @" as billNo, wi.CREATED_TIME, wi.ID WorkItemId, wi.State,
                            wi.STATE as WorkItemStatus, tfp.ID PROCESSINSTANCE_ID,wi.ACTOR_ID,wi.Complete_ACTOR_ID,wi.TASKINSTANCE_ID,
                            tfp.DISPLAY_NAME PorcessName, tfp.BIZ_TYPE_ID
                            FROM  " + tableName + " hep " +
                         string.Format(Const.WORKFLOW_QUERY_RUNNING_QUERY,
                                       "hep." + keyName) +
                         ") t";
            string strWhere = " billNo='" + billNo + "' AND " +
                       string.Format(Const.WORKFLOW_QUERY_WHEREACTOR, actor);
            return BaseDAO.GetPagedDataTable(sql, "billNo", 1, 1, strWhere, false, " CREATED_TIME desc ",
                                             out rstcount);
        }



        /// <summary>
        /// 得到委托信息
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>
        public DataTable AssignUserInfo(string userid)
        {
            string sql = string.Format(@"     
                SELECT a.[LogID]
                      ,a.[UserId]
                      ,a.[AssignToUserId]
                      ,a.[FormID]
                      ,a.[BeginDate]
                      ,a.[EndDate]
                      ,a.[Status]
                ,b.FormName
                ,c.adminname UserName
                ,d.adminname AssignToUserName
                  FROM [dbo].[CF_AssignUserListLog] as a
                left join CF_Form as b on a.[FormID]=b.id
                left join dbo.sys_Admin as c on a.[UserId]=c.Adminid
                left join dbo.sys_Admin as d on a.[AssignToUserId]=d.Adminid
                where a.Status='1' and 
                ([UserId]='{0}'
                or [AssignToUserId]='{0}')", userid);

            var data = Clover.Data.BaseDAO.GetDataTable(sql);

            return data;
        }
        #endregion

        #region 定时器用的数据
        /// <summary>
        ///     获取导出数据内容:退回的单在系统上默认保留30个自然日，30个自然日后提单人未签收退回的单，系统发邮件通知申请人废止退回的单。
        /// </summary>
        /// <returns></returns>
        public DataTable GetCancelNoticeDataTable()
        {
            return GetDataTable(@"select ord.BillNo,ord.BillType,sa.Email from dbo.v_Base_AllWorkFlowBillDetail ord                
                join dbo.sys_Admin sa on ord.Applicant = sa.adminId
                where ord.status = 'B' AND datediff(day,appdate,getdate())>=30 ");
        }


        /// <summary>
        ///     每天要定时发邮件给所有系统中有待办任务未签收的员工，提醒他们有任务需要签收。
        /// </summary>
        /// <returns></returns>
        public DataTable GetWorkItemDataTable()
        {
            return GetDataTable(@"select allWi.*,sa.AdminId,sa.AdminName,Email from (SELECT tfrp.DISPLAY_NAME, wi.ID, wi.STATE, tfrt.PROCESSINSTANCE_ID, wi.ACTOR_ID,wi.Complete_ACTOR_ID,tfrp.BIZ_ID  
                FROM T_FF_RT_WORKITEM wi 
                JOIN dbo.T_FF_RT_TASKINSTANCE tfrt  ON wi.TASKINSTANCE_ID = tfrt.ID
                JOIN dbo.T_FF_RT_PROCESSINSTANCE tfrp ON  tfrt.PROCESSINSTANCE_ID = tfrp.ID
                WHERE  wi.STATE IN (0,1)
            union
            SELECT  tfrp.DISPLAY_NAME, wi.ID, wi.STATE, tfrt.PROCESSINSTANCE_ID,caul.AssignToUserId as ACTOR_ID,wi.Complete_ACTOR_ID, tfrp.BIZ_ID  
                FROM T_FF_RT_WORKITEM wi 
                JOIN dbo.T_FF_RT_TASKINSTANCE tfrt ON wi.TASKINSTANCE_ID = tfrt.ID
                JOIN dbo.T_FF_RT_PROCESSINSTANCE tfrp ON  tfrt.PROCESSINSTANCE_ID = tfrp.ID
                JOIN CF_AssignUserList caul ON caul.UserId = wi.ACTOR_ID AND convert(varchar(10),getdate(),121) between convert(varchar(10),caul.AssginBeginDate,121)  and convert(varchar(10),caul.AssginEndDate,121)
	            JOIN T_FF_RT_PROCINST_VAR tfrpv ON caul.FormID = tfrpv.[VALUE] AND tfrpv.NAME = 'FormId'  AND tfrpv.PROCESSINSTANCE_ID = tfrp.ID
                WHERE  wi.STATE IN (0,1)
            ) allWi
            join dbo.sys_Admin sa on allWi.ACTOR_ID = sa.adminId ");
        }

        /// <summary>
        ///     获取导出数据内容:每天要定时发邮件给所有系统中有待办任务未签收的员工，提醒他们有任务需要签收。
        /// And tfrt.DISPLAY_NAME not like '%扫描%' and tfrt.DISPLAY_NAME not like '%财务出纳付款完毕%'
        /// </summary>
        /// <returns></returns>
        public DataTable GetWorkItemCountDataTable()
        {
            return GetDataTable(@"select count(1) Ecount,sa.AdminId,sa.AdminName,Email from (SELECT tfrp.DISPLAY_NAME, wi.ID, wi.STATE, tfrt.PROCESSINSTANCE_ID, wi.ACTOR_ID,wi.Complete_ACTOR_ID,tfrp.BIZ_ID  
                    FROM T_FF_RT_WORKITEM wi 
                    JOIN dbo.T_FF_RT_TASKINSTANCE tfrt  ON wi.TASKINSTANCE_ID = tfrt.ID
                    JOIN dbo.T_FF_RT_PROCESSINSTANCE tfrp ON  tfrt.PROCESSINSTANCE_ID = tfrp.ID
                    join dbo.v_Base_AllWorkFlowBill al ON al.BillNo = tfrp.BIZ_ID  AND al.Status <> 'B' 
                    WHERE  wi.STATE IN (0,1) 
                union
                SELECT  tfrp.DISPLAY_NAME, wi.ID, wi.STATE, tfrt.PROCESSINSTANCE_ID,caul.AssignToUserId as ACTOR_ID,wi.Complete_ACTOR_ID, tfrp.BIZ_ID  
                    FROM T_FF_RT_WORKITEM wi 
                    JOIN dbo.T_FF_RT_TASKINSTANCE tfrt ON wi.TASKINSTANCE_ID = tfrt.ID
                    JOIN dbo.T_FF_RT_PROCESSINSTANCE tfrp ON  tfrt.PROCESSINSTANCE_ID = tfrp.ID
                    JOIN CF_AssignUserList caul ON caul.UserId = wi.ACTOR_ID AND convert(varchar(10),getdate(),121) between convert(varchar(10),caul.AssginBeginDate,121)  and convert(varchar(10),caul.AssginEndDate,121)
	                JOIN T_FF_RT_PROCINST_VAR tfrpv ON caul.FormID = tfrpv.[VALUE] AND tfrpv.NAME = 'FormId'  AND tfrpv.PROCESSINSTANCE_ID = tfrp.ID
                    WHERE  wi.STATE IN (0,1) 
                ) allWi
                join dbo.sys_Admin sa on allWi.ACTOR_ID = sa.adminId
                group by sa.AdminId,sa.AdminName,Email");
        }

        /// <summary>
        ///     检查超过14天没报销的出差申请单
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetDelayTravelDataTable()
        {
            return GetDataTable(@"select TravelNo,DATEDIFF(DD,EndTime,GETDATE()) as DelayDas,dbo.Get_EmpEmaiForTravel(t.TravelNo) as Email
                from dbo.Exp_TravelApply t
                where TravelNo not in (select TravelNo from Exp_TravelExpense where Status > 'A' and Status < 'Z') 
                and Status = 'D' and DATEDIFF(DD,EndTime,GETDATE())>=14");
        }
        #endregion
        #region 其它
        //判断是否存在相同的Id
        public string GetIsExistsSameId(string billNo, string billType)
        {
            string flag = "0";
            using (var conn = Clover.Data.BaseDAO.ManualDbService())
            {
                try
                {
                    var p = new DynamicParameters();
                    p.Add("BillNo", billNo, DbType.String, ParameterDirection.Input, 10);
                    p.Add("BillType", billType, DbType.String, ParameterDirection.Input, 10);
                    p.Add("Result", flag, DbType.String, ParameterDirection.Output, 4);
                    conn.Execute("PROC_IsExistsSameId", p, null, CommandTimeout, CommandType.StoredProcedure);
                    flag = p.Get<string>("Result");
                }
                catch (DataException ex)
                {
                    throw ex;
                }
            }
            return flag;
        }   
        #endregion
    }
}
