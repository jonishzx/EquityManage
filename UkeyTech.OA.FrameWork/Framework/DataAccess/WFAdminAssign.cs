namespace UkeyTech.WebFW.DAO
{
	using System;
    using System.Collections.Generic;
	using System.Data;
	using System.Data.Common;
	using System.Data.SqlClient;
    using System.Text;
    using System.Transactions;

	using Dapper;
 
	using UkeyTech.WebFW.Model;    
    
	/// <summary>
	/// 操作员业务委托配置 数据访问层
	/// </summary>
    public partial class WFAdminAssignDAO : Clover.Data.BaseDAO
    {
        #region 构造函数
        public WFAdminAssignDAO()
        {
        }
        #endregion

        #region 用户委托信息
        /// <summary>
        /// 判断委托人是否可以接受任务
        /// </summary>
        /// <param name="assignuserid"></param>
        /// <returns></returns>
        public bool CheckUserAssign(string formid, string assignuserid)
        {
            var conn = DbService();
            string sql = "SELECT count(*) FROM CF_AssignUserList WHERE AssignToUserId = @1 AND FormID = @2 AND convert(varchar(16),GETDATE(),121) between convert(varchar(16),AssginBeginDate,121) and convert(varchar(16),AssginEndDate,121)";
            DynamicParameters p = new DynamicParameters();
            p.Add("1", assignuserid);
            p.Add("2", formid);

            return new List<int>(conn.Query<int>(sql, p))[0] > 0;
        }

        /// <summary>
        /// 根据被委托人获取委托信息
        /// </summary>
        /// <param name="assignuserid"></param>
        /// <returns></returns>
        public List<AssignUser> GetUsersByAssignUserId(string assignuserid)
        {
            var conn = DbService();
            string sql = "SELECT * FROM CF_AssignUserList WHERE AssignToUserId = @1";
            DynamicParameters p = new DynamicParameters();
            p.Add("1", assignuserid);
            return new List<AssignUser>(conn.Query<AssignUser>(sql, p));
        }


        /// <summary>
        /// 获取未被处理的委托记录信息
        /// Table 1:为唯一的业务委托人及时间
        /// Table 2：包含业务名称、日期、委托人、时间
        /// </summary>
        /// <returns>数据表</returns>
        public DataSet GetNuNotifyAssignUsers()
        {
            string sql = @"SELECT DISTINCT TOP 50 caull.LogID, caull.UserId, caull.AssignToUserId, cf.FormName,
	sa.AdminName UserName, sa2.AdminName AssignToUserName,
       (STUFF(STUFF(CONVERT(char(8),caul.AssginBeginDate,112),5,0,N'年'),8,0,N'月')+N'日') AssginBeginDate,
       (STUFF(STUFF(CONVERT(char(8),caul.AssginEndDate,112),5,0,N'年'),8,0,N'月')+N'日') AssginEndDate
INTo #tmp
FROM CF_AssignUserListLog AS caull
JOIN CF_AssignUserList AS caul ON 
	caul.UserId = caull.UserId 
	AND caul.AssignToUserId = caull.AssignToUserId
	AND caul.FormID = caull.FormID
JOIN CF_Form AS cf ON cf.ID = caull.FormID
JOIN sys_Admin sa ON sa.AdminId = caull.UserId
JOIN sys_Admin sa2 ON sa2.AdminId = caull.AssignToUserId
WHERE caull.[Status] = 1
ORDER BY (STUFF(STUFF(CONVERT(char(8),caul.AssginBeginDate,112),5,0,N'年'),8,0,N'月')+N'日') DESC;
SELECT DISTINCT UserId,t.AssignToUserId, t.UserName,
       t.AssignToUserName, t.AssginBeginDate, t.AssginEndDate FROM #tmp t;
SELECT * FROM #tmp;
DROP TABLE #tmp;";
            return QueryData(sql, null);
        }

        /// <summary>
        /// 更新日志的状态
        /// </summary>
        /// <param name="logid">委托日志id</param>
        public void UpdateNotifyLogStatus(string logid)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("@1", logid);

            using (var conn = ManualDbService())
            {
                try
                {

                    var rst = conn.Execute(
                        string.Format(@"UPDATE CF_AssignUserListLog
                                        SET	 
	                                        [Status] = 2
                                        WHERE LogID = @1"
                        , logid), p);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    Clover.Data.BaseDAO.CloseWithDispose(conn);
                }
            }
        }

        /// <summary>
        /// 更新日志的状态
        /// </summary>
        /// <param name="logid">委托日志id</param>
        public void UpdateNotifyLogStatus(string userid, string assigntouserid, string begindate, string enddate)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("@1", userid);
            p.Add("@2", begindate);
            p.Add("@3", enddate);
            p.Add("@4", assigntouserid);
            using (var conn = ManualDbService())
            {
                try
                {

                    var rst = conn.Execute(
                        @"UPDATE CF_AssignUserListLog SET [Status] = 2
                          FROM CF_AssignUserListLog AS caull
                        JOIN CF_AssignUserList AS caul ON 
	                        caul.UserId = caull.UserId 
	                        AND caul.AssignToUserId = caull.AssignToUserId
	                        AND caul.FormID = caull.FormID
                            WHERE caull.[Status] = 1 AND caull.UserId = @1 AND caull.AssignToUserId = @4
AND (STUFF(STUFF(CONVERT(char(8),caul.AssginBeginDate,112),5,0,N'年'),8,0,N'月')+N'日') = @2 
AND (STUFF(STUFF(CONVERT(char(8),caul.AssginEndDate,112),5,0,N'年'),8,0,N'月')+N'日') = @3"
                        , p);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    Clover.Data.BaseDAO.CloseWithDispose(conn);
                }
            }
        }

        public virtual void DeleteAssignUsers(string userid, int formid, params string[] assignUserIds)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("@1", userid);
            p.Add("@2", formid);
            using (var ts = new TransactionScope(System.Transactions.TransactionScopeOption.RequiresNew))
            {
                using (var conn = ManualDbService())
                {
                    IDbTransaction tran = null;
                    try
                    {
                        tran = conn.BeginTransaction();

                        var rst = conn.Execute(
                            string.Format("DELETE [CF_AssignUserList] Where UserID = @1 AND FormID = @2 AND AssignToUserId IN ('{0}')"
                            , Clover.Core.Common.StringHelper.Join("','", assignUserIds)), p, null, tran);

                        conn.Execute(
                                 string.Format("update [CF_AssignUserListLog] set Status='-1',EndDate=getdate()  Where UserID = @1 AND FormID = @2 and Status in ('1','2')  "), p, null, tran);

                        tran.Commit();
                        ts.Complete();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        Clover.Data.BaseDAO.CloseWithDispose(conn);
                    }
                }
            }
        }

        public virtual int DeleteAssignUsersAll(string userid, string[] formids)
        {

            using (var ts = new TransactionScope(System.Transactions.TransactionScopeOption.RequiresNew))
            {
                using (var conn = ManualDbService())
                {
                    IDbTransaction tran = null;
                    try
                    {
                        tran = conn.BeginTransaction();

                        foreach (string id in formids)
                        {
                            if (string.IsNullOrEmpty(id))

                                continue;
                            DynamicParameters p = new DynamicParameters();
                            p.Add("1", userid);
                            p.Add("2", id);

                            conn.Execute(
                            string.Format("DELETE [CF_AssignUserList] Where UserID = @1 AND FormID = @2 "), p, null, tran);

                            conn.Execute(
                           string.Format("update [CF_AssignUserListLog] set Status='-1',EndDate=getdate()  Where UserID = @1 AND FormID = @2 and Status='1'  "), p, null, tran);


                        }

                        tran.Commit();
                        ts.Complete();

                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        Clover.Data.BaseDAO.CloseWithDispose(conn);
                    }
                }
            }
            return 1;

        }

        /// <summary>
        /// 设置委托代办日期
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="formids"></param>
        /// <returns></returns>
        public virtual int SetAssignDate(string userid, string[] assignActorIds, string formid, string[] beginDates, string[] endDates )
        {
            int i = 0;
            
            if (assignActorIds.Length != beginDates.Length || assignActorIds.Length != endDates.Length)
                throw new ArgumentException("参数个数不统一,参数异常");

            using (var ts = new TransactionScope(System.Transactions.TransactionScopeOption.RequiresNew))
            {
                using (var conn = ManualDbService())
                {
                    IDbTransaction tran = null;
                    try
                    {
                        tran = conn.BeginTransaction();

                        foreach (string actorid in assignActorIds)
                        {
                            if (string.IsNullOrEmpty(userid))
                                continue;

                            DynamicParameters p = new DynamicParameters();
                            p.Add("1", userid);
                            p.Add("2", actorid);
                            p.Add("3", formid);
                            p.Add("4", beginDates[i]);
                            p.Add("5", endDates[i]);

                            conn.Execute(@"UPDATE [CF_AssignUserList] 
                                SET AssginBeginDate = @4, AssginEndDate = @5
                                Where UserID = @1 AND AssignToUserId = @2 AND FormID = @3 ", p, null, tran);
                            
                            i++;
                        }
                        tran.Commit();
                        ts.Complete();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        Clover.Data.BaseDAO.CloseWithDispose(conn);
                    }
                }
            }
            return 1;

        }

        /// <summary>
        /// 清空委托代办日期
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="formids"></param>
        /// <returns></returns>
        public virtual int CleanAssignDate(string userid, string[] assignActorIds, string formid)
        {

            using (var ts = new TransactionScope(System.Transactions.TransactionScopeOption.RequiresNew))
            {
                using (var conn = ManualDbService())
                {
                    IDbTransaction tran = null;
                    try
                    {
                        tran = conn.BeginTransaction();

                        foreach (string actorid in assignActorIds)
                        {
                            if(string.IsNullOrEmpty(userid))

                                continue;
                            DynamicParameters p = new DynamicParameters();
                            p.Add("1", userid);
                            p.Add("2", actorid);
                            p.Add("3", formid);

                           conn.Execute(@"UPDATE [CF_AssignUserList] SET 
                            AssginBeginDate = null, AssginEndDate = null 
                            Where UserId = @1 AND AssignToUserId = @2 AND FormID = @3", p, null, tran);
                        }

                        tran.Commit();
                        ts.Complete();

                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        Clover.Data.BaseDAO.CloseWithDispose(conn);
                    }
                }
            }
            return 1;

        }

        /// <summary>
        /// 获取委托的用户
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="formid"></param>
        /// <returns></returns>
        public DataTable GetAssignUsers(string userid, int formid)
        {
            var conn = DbService();
            var p = new Dictionary<string, object>();
            p.Add("FormID", formid);
            p.Add("UserId", userid);
            return Clover.Data.BaseDAO.QueryData(@"SELECT A.AdminId, A.AdminName,A.LoginName,B.AssignToUserId,
B.UserId,B.FormID,convert(varchar(16),B.AssginBeginDate,121) AssginBeginDate,
convert(varchar(16),B.AssginEndDate,121) AssginEndDate
FROM sys_Admin A
JOIN CF_AssignUserList B
ON A.AdminId = B.AssignToUserId AND B.FormID = @FormID
WHERE B.UserId = @UserId", p).Tables[0];

        }


        /// 获取委托的用户
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="formid"></param>
        /// <returns></returns>
        public List<Admin> GetAssignAdmins(string userid, int formid)
        {
            var conn = DbService();
            return new List<Admin>(conn.Query<Admin>(@"SELECT * FROM sys_Admin A
JOIN CF_AssignUserList B
ON A.AdminId = B.AssignToUserId AND B.FormID = @FormID
WHERE B.UserId = @UserId", new { UserId = userid, FormID = formid }));

        }

        /// <summary>
        /// 获取未委托的用户
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="formid"></param>
        /// <returns></returns>
        public List<Admin> GetNoAssignUsers(string userid, int formid, string curruserid, int pagesize, int pageindex, string codeOrName, out int rowscount)
        {
            string whereStr =  string.Format(@"AdminId NOT IN (
 SELECT AssignToUserId FROM CF_AssignUserList
 WHERE UserId = '{0}' AND FormID = {1}) And AdminId <> '{2}'", userid, formid,curruserid);
            if(!string.IsNullOrEmpty(codeOrName))
                whereStr += string.Format(" AND (LoginName like '%{0}%' or AdminName like '%{0}%')", codeOrName);
            UkeyTech.WebFW.DAO.AdminDAO admindao = new AdminDAO();
            return admindao.GetAllPaged(pagesize, pageindex, whereStr, true, out rowscount);        
        }

        /// <summary>
        /// 获取未委托的用户
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="formid"></param>
        /// <returns></returns>
        public List<Admin> GetNoAssignUsersSet(string CodeOrName, string groupId, string userid, int formid, string curruserid, int pagesize, int pageindex, string level, out int rowscount)
        {
            UkeyTech.WebFW.DAO.AdminDAO admindao = new AdminDAO();

            if (level == "Less")
            {
                //过滤条件：非本人，不在本任务的已委托列表，向下一级或同级或上级部门
                return admindao.GetAllPaged(pagesize, pageindex, string.Format(@"AdminId NOT IN (
                 SELECT AssignToUserId FROM CF_AssignUserList
                 WHERE UserId = '{0}' AND FormID = {1}) 
                  AND AdminId <> '{2}' and (loginname like '%{3}%' or adminname like '%{3}%')
                  AND adminId in(
	                  SELECT cpu.UserID
                       FROM   CPM_Group cp
                              JOIN CPM_Group_User cpu
                                   ON  cp.GroupID = cpu.GroupID
                       WHERE  cp.Status = 1
							  AND ((charindex(cp.ParentPath + '\', (SELECT ParentPath FROM CPM_Group WHERE GroupID = '{4}') + '\')>0
							  AND len(cp.ParentPath) <= (SELECT len(ParentPath) FROM CPM_Group WHERE GroupID = '{4}') 
                               OR CP.ParentID = '{4}')))
                    ", userid, formid, curruserid, CodeOrName, groupId), true, out rowscount);

            }
            else
            {
               //过滤条件：非本人，不在本任务的已委托列表，同级或上级部门
             return admindao.GetAllPaged(pagesize, pageindex, string.Format(@"AdminId NOT IN (
                SELECT AssignToUserId FROM CF_AssignUserList
                WHERE UserId = '{0}' AND FormID = {1}) 
                 AND AdminId <> '{2}' and (loginname like '%{3}%' or adminname like '%{3}%')
                 AND adminId in(
                     SELECT cpu.UserID
                      FROM   CPM_Group cp
                             JOIN CPM_Group_User cpu
                                  ON  cp.GroupID = cpu.GroupID
                      WHERE  cp.Status = 1
                             AND charindex(cp.ParentPath + '\', (SELECT ParentPath FROM CPM_Group WHERE GroupID = '{4}') + '\')>0
                             AND len(cp.ParentPath) <= (SELECT len(ParentPath) FROM CPM_Group WHERE GroupID = '{4}'))	
               ", userid, formid, curruserid, CodeOrName,groupId), true, out rowscount);
            
            }
        }
             

        /// <summary>
        /// 获取未委托的用户
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="formid"></param>
        /// <returns></returns>
        public void SetAssignUsers(string userid, int formid, string[] assignUserIds, string begindate, string enddate)
        {
            string sql = @"INSERT INTO [CF_AssignUserList]
           ([UserId]
           ,[AssignToUserId]
           ,[FormID]
           ,[AssginBeginDate]
           ,[AssginEndDate])
SELECT @1, @2, @3, @4, @5
WHERE NOT EXISTS (SELECT 1 FROM [CF_AssignUserList] 
WHERE UserId=@1 AND AssignToUserId=@2 AND FormID=@3)";

            string sqllog = @"INSERT INTO [CF_AssignUserListLog]
           ([UserId]
           ,[AssignToUserId]
           ,[FormID]
           ,[BeginDate]
           ,[Status])
SELECT @1, @2, @3,getdate(),'1'
WHERE NOT EXISTS (SELECT 1 FROM [CF_AssignUserListLog] 
WHERE UserId=@1 AND AssignToUserId=@2 AND FormID=@3 AND Status in ('1', '2'))";

            using (var ts = new TransactionScope(System.Transactions.TransactionScopeOption.RequiresNew))
            {
                using (var conn = ManualDbService())
                {
                    IDbTransaction tran = null;
                    try
                    {
                        tran = conn.BeginTransaction();

                        foreach (string id in assignUserIds)
                        {
                            DynamicParameters p = new DynamicParameters();
                            p.Add("1", userid);
                            p.Add("2", id);
                            p.Add("3", formid);
                            p.Add("4", begindate);
                            p.Add("5", enddate);

                            conn.Execute(sql, p, null, tran);

                            conn.Execute(sqllog, p, null, tran);
                        }

                        tran.Commit();
                        ts.Complete();

                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        Clover.Data.BaseDAO.CloseWithDispose(conn);
                    }
                }
            }
        }


        /// <summary>
        /// 获取委托的用户的委托时间
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="formid"></param>
        /// <returns></returns>
        public void AlterDate(string userid, int formid, string begindate, string enddate)
        {
            AllAlterDate(userid, new string[] { formid.ToString() }, begindate, enddate);
        }

        /// <summary>
        /// 获取委托的用户的委托时间
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="formid"></param>
        /// <returns></returns>
        public void AllAlterDate(string userid, string[] formids, string begindate, string enddate)
        {
            string sql = @"Update [CF_AssignUserList] set
            [AssginBeginDate] = @3 ,
            [AssginEndDate] = @4
            WHERE UserId=@1 AND FormID=@2";

            string sqllog = @"Update [CF_AssignUserListLog] set
            [BeginDate] = @3,
            [Status] = '1'
            WHERE UserId=@1 AND FormID=@2 AND Status in ('1', '2')";

            using (var ts = new TransactionScope(System.Transactions.TransactionScopeOption.RequiresNew))
            {
                using (var conn = ManualDbService())
                {
                    IDbTransaction tran = null;
                    try
                    {
                        tran = conn.BeginTransaction();

                        foreach (var formid in formids)
                        {
                            DynamicParameters p = new DynamicParameters();
                            p.Add("1", userid);
                            p.Add("2", formid);
                            p.Add("3", begindate);
                            p.Add("4", enddate);

                            conn.Execute(sql, p, null, tran);

                            conn.Execute(sqllog, p, null, tran);
                        }
                        tran.Commit();
                        ts.Complete();

                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        Clover.Data.BaseDAO.CloseWithDispose(conn);
                    }
                }
            }
        }

        /// <summary>
        /// 获取未委托的用户
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="formid"></param>
        /// <returns></returns>
        public void AllSetAssignUsers(string userid, string[] formid, string[] assignUserIds, string begindate, string enddate)
        {
            string sql = @"INSERT INTO [CF_AssignUserList]
           ([UserId]
           ,[AssignToUserId]
           ,[FormID]   
           ,[AssginBeginDate]
           ,[AssginEndDate])
SELECT @1, @2, @3, @4 ,@5
WHERE NOT EXISTS (SELECT 1 FROM [CF_AssignUserList] 
WHERE UserId=@1 AND AssignToUserId=@2 AND FormID=@3)";

            string sqllog = @"INSERT INTO [CF_AssignUserListLog]
           ([UserId]
           ,[AssignToUserId]
           ,[FormID]
           ,[BeginDate]
           ,[Status])
SELECT @1, @2, @3,getdate(),'1'
WHERE NOT EXISTS (SELECT 1 FROM [CF_AssignUserListLog] 
WHERE UserId=@1 AND AssignToUserId=@2 AND FormID=@3 AND Status in ('1','2'))";


            using (var ts = new TransactionScope(System.Transactions.TransactionScopeOption.RequiresNew))
            {
                using (var conn = ManualDbService())
                {
                    IDbTransaction tran = null;
                    try
                    {
                        tran = conn.BeginTransaction();

                        foreach (var fid in formid)
                        {

                            if(string.IsNullOrEmpty(fid))
                                continue;
                            foreach (string id in assignUserIds)
                            {
                                DynamicParameters p = new DynamicParameters();
                                p.Add("1", userid);
                                p.Add("2", id);
                                p.Add("3", fid);
                                p.Add("4", begindate);
                                p.Add("5", enddate);

                                conn.Execute(sql, p, null, tran);

                                conn.Execute(sqllog, p, null, tran);
                            }
                        }

                        tran.Commit();
                        ts.Complete();

                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        Clover.Data.BaseDAO.CloseWithDispose(conn);
                    }
                }
            }
        }

        #endregion

    }
}