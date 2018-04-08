using System.Transactions;

namespace UkeyTech.WebFW.DAO
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Data.SqlClient;
    using Clover.Data;
    using UkeyTech.WebFW.Model;
    using Dapper;
    public class MailQueueDAO : Clover.Data.BaseDAO
    {

        public static string GetInsertSqlCommand
        {
            get
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into MailQueue_Working(");
                strSql.Append("PType,Form,SendTo,Subject,Message,Attachments,CreateDate,SendDate)");
                strSql.Append(" values (");
                strSql.Append("@PType,@Form,@SendTo,@Subject,@Message,@Attachments,@CreateDate,@SendDate)");
                return strSql.ToString();
            }
        }

        public static DynamicParameters GetInsertSqlParamters(MailQueue model)
        {
            var p = new DynamicParameters();
            p.Add("PType", model.PType);
            p.Add("Form", model.Form);
            p.Add("SendTo", model.SendTo);
            p.Add("Subject", model.Subject);
            p.Add("Message", model.Message);
            p.Add("Attachments", model.Attachments, DbType.Binary, null,null);
            p.Add("CreateDate", model.CreateDate);
            p.Add("SendDate", model.SendDate);

          
            return p;

        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public static void Add(MailQueue model)
        {
            using (IDbConnection conn = ManualDbService())
            {

                try
                {
                    conn.Execute(GetInsertSqlCommand, GetInsertSqlParamters(model));
                }
                catch (DataException ex)
                {
                    throw ex;
                }
                finally {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public static MailQueue GetModel(int MailQueueId, string table)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select MailQueueId,PType,Form,SendTo,Subject,Message,Attachments,CreateDate,SendDate from  " + table);
            strSql.Append(" where MailQueueId=@MailQueueId ");
            var p = new DynamicParameters();
            p.Add("MailQueueId", MailQueueId);

            IDbConnection conn = DbService();
            var list = new List<MailQueue>(conn.Query<MailQueue>(strSql.ToString(), p));

            if (list.Count > 0)
            {
                return list[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 删除指定记录
        /// </summary>
        public static void Delete(string table, int MailQueueId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from " + table);
            strSql.Append(" where MailQueueId=@MailQueueId");
            var p = new DynamicParameters();
            p.Add("MailQueueId", MailQueueId);

            IDbConnection conn = DbService();
            conn.Execute(strSql.ToString(), p);
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public static DataSet GetList(string table, int PageSize, int PageIndex, string strWhere, out int rowcount)
        {
            return Clover.Data.BaseDAO.GetList(table, "MailQueueId", PageSize, PageIndex, strWhere, true, "", out rowcount);
        }

        /// <summary>
        /// 分页获取数据列表-未发送
        /// </summary>
        public static DataSet GetWorkingList(int PageSize, int PageIndex, string strWhere, out int rowcount)
        {
            return Clover.Data.BaseDAO.GetList("MailQueue_Working", "MailQueueId", PageSize, PageIndex, strWhere, true, "", out rowcount);
        }

        /// <summary>
        /// 分页获取数据列表-发送中
        /// </summary>
        public static DataSet GetReceivingList(int PageSize, int PageIndex, string strWhere, out int rowcount)
        {
            return Clover.Data.BaseDAO.GetList("MailQueue_Receiving", "MailQueueId", PageSize, PageIndex, strWhere, true, "", out rowcount);
        }

        /// <summary>
        /// 分页获取数据列表-历史
        /// </summary>
        public static DataSet GetHisitoryList(int PageSize, int PageIndex, string strWhere, out int rowcount)
        {
            return Clover.Data.BaseDAO.GetList("MailQueue_Hisitory", "MailQueueId", PageSize, PageIndex, strWhere, true, "", out rowcount);
        }


        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public static void MoveToReceivingBox()
        {
            string inssertcmd = @"INSERT INTO MailQueue_Receiving
                                SELECT * FROM MailQueue_Working mqw";

            string delcmd = @"DELETE MailQueue_Working";

           
            using (var ts = new TransactionScope())
            {
                using (var conn = ManualDbService())
                {
                    var tran = conn.BeginTransaction();

                    conn.Execute(inssertcmd, null, null, tran);
                    conn.Execute(delcmd, null, null, tran);

                    tran.Commit();
                    ts.Complete();
                }
            }
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public static void MoveToHistoryBox(int MailQueueId)
        {
            string inssertcmd = @"INSERT INTO MailQueue_Hisitory(MailQueueId,PType,
	                                Form,
	                                SendTo,
	                                [Subject],
	                                [Message],
	                                Attachments,
	                                CreateDate,
	                                SendDate,
	                                FinishDate)
                                SELECT [MailQueueId]
                                      ,[PType]
                                      ,[Form]
                                      ,[SendTo]
                                      ,[Subject]
                                      ,[Message]
                                      ,[Attachments]
                                      ,[CreateDate]
                                      ,getdate(),getdate() 
                            FROM MailQueue_Receiving mqr
                                WHERE mqr.MailQueueId = " + MailQueueId + ";";

            string delcmd = @"DELETE MailQueue_Receiving
                              WHERE MailQueueId = " + MailQueueId;

            using (var ts = new TransactionScope())
            {
                using (var conn = ManualDbService())
                {
                    var tran = conn.BeginTransaction();

                    conn.Execute(inssertcmd, null, null, tran);
                    conn.Execute(delcmd, null, null, tran);

                    tran.Commit();
                    ts.Complete();
                }
            }
         
        }
    }
}