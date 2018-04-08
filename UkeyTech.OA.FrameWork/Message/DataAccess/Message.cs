using System.Linq;

namespace Clover.Message.DAO
{
	using System;
    using System.Collections.Generic;
	using System.Data;
	using System.Data.Common;
	using System.Data.SqlClient;
    using System.Text;
	using Dapper;
    using Dapper.Contrib.Extensions;
    
	using Clover.Message.Model;
    using System.Transactions;
    
	/// <summary>
	/// Message 数据访问层
	/// </summary>
	public partial class MessageDAO : Clover.Data.BaseDAO
	{
		#region 构造函数
		public MessageDAO()
		{
		}
		#endregion		
		
		#region 根据主键创建 Message 数据模型实例
		/// <summary>
		/// 根据主键创建 Message 数据模型实例 
		/// </summary>
		public Message GetModel(string messageid)
		{           
            var conn = DbService();
            
            try{
                var rst = conn.Get<Message>(messageid);
                return rst;
            }
            catch(DataException ex){
                throw ex;
            }
		}
		#endregion
			
		#region 更新记录
		/// <summary>
		/// 更新记录到数据库
		/// </summary>
		public  bool Update(Message model)
		{
			return Update(null,model);
		}
		/// <summary>
		/// 更新记录到数据库(利用事务)
		/// </summary>
		public  bool Update(IDbTransaction tran, Message model)
		{		
		
            IDbConnection conn = tran == null ?  DbService() : tran.Connection;
            bool rst = false;
            try{
                rst = conn.Update(model, tran);
            }
            catch(DataException ex){
                throw ex;
            }
			
			return rst;
		}
		#endregion
		
		#region 新增记录
		/// <summary>
		/// 新增记录到数据库
		/// </summary>
		public  bool Insert(Message model)
		{
			return Insert(null,model);
		}
		
		/// <summary>
		/// 新增记录到数据库(事务)
		/// </summary>
		public bool Insert(IDbTransaction tran, Message model)
		{
         
            IDbConnection conn = tran == null ?  DbService() : tran.Connection;

            var p = new DynamicParameters();
            p.Add("MessageId", model.MessageId, DbType.String, null, 36);
            p.Add("Title", model.Title, DbType.String, null, 50);
            p.Add("MessageBody", model.MessageBody, DbType.String, null, -1);
            p.Add("Status", model.Status, DbType.String, null, 8);
            p.Add("AutoAlert", model.AutoAlert, DbType.String, null, 1);
            p.Add("TargetId", model.TargetId, DbType.String, null, 50);
            p.Add("TargetObject", model.TargetObject, DbType.String, null, 150);
            p.Add("ReferMessageId", model.ReferMessageId, DbType.String, null, 36);
            p.Add("Sender", model.Sender, DbType.String, null, 36);
            p.Add("Receivers", model.Receivers, DbType.String, null, 1000);
            p.Add("Creator", model.Creator, DbType.String, null, 36);
            p.Add("CreateTime", model.CreateTime, DbType.DateTime, null, 8);
            p.Add("MessageAction", model.MessageAction, DbType.String, null, 250);
            p.Add("OperationAction", model.OperationAction, DbType.String, null, 250);
            p.Add("ExtendAction1", model.ExtendAction1, DbType.String, null, 250);
            p.Add("ExtendAction2", model.ExtendAction2, DbType.String, null, 250);
            p.Add("NeedAccept", model.NeedAccept, DbType.Boolean, null, 1);
            p.Add("NeedRead", model.NeedRead, DbType.Boolean, null, 1);
            p.Add("TemplateCode", model.TemplateCode, DbType.String, null, 50);
            p.Add("CanReplay", model.CanReplay, DbType.Boolean, null, 1);
            p.Add("CreatorName", model.CreatorName, DbType.String, null, 50);
            p.Add("ReceiversName", model.ReceiversName, DbType.String, null, 250);

            string sql = @"INSERT INTO [msg_Message] (  
    [MessageId],                        
	[Title],
	[MessageBody],
	[Status],
	[AutoAlert],
	[TargetId],
	[TargetObject],
	[ReferMessageId],
	[Sender],
	[Receivers],
	[Creator],
	[CreateTime],
	[MessageAction],
	[OperationAction],
	[ExtendAction1],
	[ExtendAction2],
	[NeedAccept],
	[NeedRead],
	[TemplateCode],
	[CanReplay],
    [CreatorName],
    [ReceiversName]
                        ) 
select 
@MessageId,
                            	@Title,
	@MessageBody,
	@Status,
	@AutoAlert,
	@TargetId,
	@TargetObject,
	@ReferMessageId,
	@Sender,
	@Receivers,
	@Creator,
	@CreateTime,
	@MessageAction,
	@OperationAction,
	@ExtendAction1,
	@ExtendAction2,
	@NeedAccept,
	@NeedRead,
	@TemplateCode,
	@CanReplay,
    @CreatorName,
    @ReceiversName
 where not exists (select 1 from msg_Message where MessageId = @MessageId)";

            try
            {
                conn.Execute(sql, p);
            }
            catch (DataException ex)
            {
                throw ex;
            }
            finally {
                
            }
            
           
			return true;
		}
		#endregion
		
		#region 删除记录
        
        /// <summary>
		/// 从数据库删除记录(事务)
		/// </summary>
		public bool Delete(Message model)
		{
			return Delete(model, null);
		}
        
         /// <summary>
		/// 从数据库删除记录(事务)
		/// </summary>
		public bool Delete(Message model, IDbTransaction tran )
		{
            IDbConnection conn = tran == null ?  DbService() : tran.Connection;
            bool rst = false;
            try{
                rst = conn.Delete(model, tran);
            }
            catch(DataException ex){
                throw ex;
            }finally{
                
            }

			return rst;
		}
        
		/// <summary>
		/// 从数据库删除记录(事务)
		/// </summary>
		public bool Delete(long MessageId)
		{
			return Delete(null,MessageId);
		}
		
		/// <summary>
		/// 从数据库删除记录(事务)
		/// </summary>
		public  bool Delete(IDbTransaction tran, long MessageId)
		{
			int affectedrows = 0;
        
            var p = new DynamicParameters();           
		     //获取主键
            p.Add("MessageId",MessageId ,DbType.Int64,null,8);
            
            string sql = @"DELETE FROM [msg_Message]
                        WHERE 	[MessageId] = @MessageId
";
            
            IDbConnection conn = tran == null ?  DbService() : tran.Connection;

            try{
                affectedrows = conn.Execute(sql, p);
            }
            catch(DataException ex){
                throw ex;
            }finally{
                
            }

			return Convert.ToBoolean(affectedrows);
		}
		
		#endregion
		
		#region 查询，返回自定义类			
		/// <summary>
		/// 查询所有记录，并排序
		/// </summary>
		public  List<Message> GetAll(string orderBy)
		{
			return GetList(null,null, string.Empty, orderBy);
		}
        
		/// <summary>
		/// 查询所有记录，并排序(事务)
		/// </summary>
		public List<Message> GetList(IDbConnection conn,int? top, string strWhere, string orderBy)
		{
            if(conn == null)
                conn = DbService();
            
            StringBuilder strSql=new StringBuilder();
			strSql.Append("Select ");
			if(top.HasValue)
			{
				strSql.Append(" top "+top.ToString());
			}
			strSql.Append(@" *");
			strSql.Append(" FROM [msg_Message] ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
           
            if (orderBy != string.Empty)
                strSql.Append(" order by " + orderBy);
			
            
            var rst = conn.Query<Message>(strSql.ToString(), null);
                
            
            
			return new List<Message>(rst);
		}
	
		/// <summary>
		/// 查询所有记录，并排序、分页
		/// </summary>
		public List<Message> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname)
		{
            int rstcount = 0;
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc, orderfldname,out rstcount);
		}
		
        /// <summary>
		/// 查询所有记录，并排序、分页,没有排序字段
		/// </summary>
		public List<Message> GetAllPaged(int PageSize,int PageIndex,string strWhere,bool desc)
		{
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc,"MessageId" );
		}
        
		/// <summary>
		/// 查询所有记录，并排序、分页,没有排序字段
		/// </summary>
		public List<Message> GetAllPaged(int PageSize,int PageIndex,string strWhere,bool desc,  out int rstcount)
		{
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc,"MessageId", out rstcount );
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页(事务)
		/// </summary>
		public List<Message> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname, out int rstcount)
		{
		    return Clover.Data.BaseDAO.GetList<Message>("[msg_Message]","MessageId",PageSize, PageIndex, strWhere ,desc, orderfldname, out rstcount);
		}
		
		#endregion
				
		#region 计算查询结果记录数
		/// <summary>
		/// 对所有记录进行记录数计算
		/// </summary>
		public  int SumAllCount()
		{           
		    return SumDynamicCount("");
		}
		/// <summary>
		/// 对所有符合条件的记录进行记录数计算
		/// </summary>
		public  int SumDynamicCount(string strWhere)
		{
            string strSQL = "select count(*) from [msg_Message]";
            strSQL = string.IsNullOrEmpty(strWhere)? strSQL : strSQL + " where " + strWhere;
            var conn = DbService();
		    IEnumerable<int> rst = conn.Query<int>(strSQL, null);
            
            return new List<int>(rst)[0];            
            
		}
		#endregion
		
		#region 其他自定义方法
        public void Save(Message model) {
            if (model.ObjectStatus == DataRowState.Modified && model.Status == null)
            {
                Update(model);
            }
            else {
                Insert(model);
            }
        }
        public void SaveAndSend(Message model, string userid)
        {
            if (model.ObjectStatus == DataRowState.Modified && model.Status == null)
            {
                Update(model);
               
            }
            else
            {
                Insert(model);
            }

            SendMessage(model.MessageId.ToString(), userid);
        }

        /// <summary>
        /// 获取收件箱信息
        /// </summary>
        /// <param name="userid"></param>
        public List<Message> GetInBoxMessage(int boxid,int PageSize, int PageIndex, string strWhere, bool desc, string orderfldname, out int rstcount)
        {
            if (!string.IsNullOrEmpty(strWhere))
                strWhere += " AND ";// F代表已经删除到回收站

            strWhere += "IsRecyle = 0 AND MessageBoxId = " + boxid.ToString();

            return Clover.Data.BaseDAO.GetList<Message>(inboxsql, "MessageId", PageSize, PageIndex, strWhere, desc, orderfldname, out rstcount);
        }

        /// <summary>
        /// 获取关联的信息
        /// </summary>
        /// <param name="refMessageId">关联消息id</param>
        public List<Message> GetRefMessage(string refMessageId)
        {
            string strWhere = string.Format("(MessageId = '{0}'  or ReferMessageId = '{0}' " +
                                            "or MessageId = (select ReferMessageId from msg_Message where MessageId = '{0}')" +
                                            "or ReferMessageId = (select ReferMessageId from msg_Message where MessageId = '{0}'))", refMessageId);
            var conn = DbService();
            string strSql = "select * from msg_Message where Status not in ('E') and " + strWhere + " order by CreateTime desc";
            return conn.Query<Message>(strSql).ToList(); 
        }

        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="userid"></param>
        public void SendMessage(string messageids, string userid)
        {
            string createInBoxMessageSql = @"if(not exists(select 1 from msg_MessageBox
where [Owner] = @1 and BoxType in ('InBox','OutBox','Recyle')))
begin
	insert into msg_MessageBox
	select * from (
	select '收件箱' Name,'A' Status,@1 Owener,GETDATE() CreateTime,'InBox' BoxType,null Parent,1 ViewOrder
	union
	select '发件箱','A',@1,GETDATE(),'OutBox',null,2
	union
	select '草稿箱','A',@1,GETDATE(),'Draft',null,5
	union
	select '回收站','A',@1,GETDATE(),'Recyle',null,10
	)t
	
end	
declare @messageboxid int
set @messageboxid = (select top 1 MessageBoxId from msg_MessageBox where BoxType = 'InBox' AND Name = '收件箱' AND Owner = @1)
insert into msg_BoxMessage
select  @messageboxid,
@MessageId, @1, null [Status], null Result, getdate(), null, null, null, null, null, 0, 0
where not exists(select 1 from msg_BoxMessage where MessageBoxId = @messageboxid AND MessageId =@MessageId AND Receiver = @1)
";

            //更改邮件状态为'A'
            string[] ids = messageids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string sql = @"update msg_Message set Status = @2, SendTime=getdate() where Status is null AND Creator = @3 AND MessageId =@1;";

            //插入收信箱
            //按收件人生成发送记录
         
            using (var ts = new TransactionScope(System.Transactions.TransactionScopeOption.RequiresNew))
            {
                using (var conn = ManualDbService())
                {
                    var tran = conn.BeginTransaction();
                    try
                    {
                        foreach (var messageid in ids)
                        {
                            //更改消息状态

                            DynamicParameters p = new DynamicParameters();
                            p.Add("@1", messageid);
                            p.Add("@2", "B"); //B代表已发送
                            p.Add("@3", userid);
                            conn.Execute(sql, p, null, tran);

                            var msgmodel = conn.Get<Message>(messageid);

                            foreach (var receiver in msgmodel.Receivers.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                //生成入箱记录
                                DynamicParameters p2 = new DynamicParameters();
                                p2.Add("@1", receiver);
                                p2.Add("@MessageId", messageid);
                                conn.Execute(createInBoxMessageSql, p2, null, tran);
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
                    finally {
                        CloseWithDispose(conn);
                    }
                }
            }
        }

        /// <summary>
        /// 撤销未读消息
        /// </summary>
        /// <param name="messageid"></param>
        /// <returns></returns>
        public int WithDrawMessage(string messageid, string userid)
        {
            //撤回未读邮件
            string sql = @"update msg_BoxMessage set Status = @2 where MessageId = @1 and MessageId in (select MessageId from  msg_Message where Creator = @3) and Status is null";

            DynamicParameters p = new DynamicParameters();
            p.Add("@1", messageid);
            p.Add("@2", "E"); //E代表已撤回
            p.Add("@3", userid);

            var conn = DbService();
            return conn.Execute(sql, p);
        }

        /// <summary>
        /// 获取已发信息的相关情况(收件人的情况)
        /// </summary>
        /// <param name="userid"></param>
        public List<BoxMessage> GetMessageStatus(string messageid, int PageSize, int PageIndex, string strWhere, bool desc, string orderfldname, out int rstcount)
        {
                  //加入草稿状态
            if (!string.IsNullOrEmpty(strWhere))
                strWhere += " AND ";

            strWhere += string.Format(" MessageId = '{0}'", messageid);


            return Clover.Data.BaseDAO.GetList<BoxMessage>("( select mb.*,sa.AdminName ReceiverName from msg_BoxMessage mb join sys_admin sa on mb.Receiver = sa.AdminId ) t", "MessageId", PageSize, PageIndex, strWhere, desc, orderfldname, out rstcount);

        }

        /// <summary>
        /// 标记收件箱地址
        /// </summary>
        /// <param name="userid"></param>
        public int MarkBoxMessage(int messageboxid, string messageid, string receiver, string mark)
        {
            string sql = @"update msg_BoxMessage set Mark = @4
where MessageBoxId = @1 and MessageId = @2 and Receiver = @3";

            DynamicParameters p = new DynamicParameters();
            p.Add("@1", messageboxid);
            p.Add("@2", messageid);
            p.Add("@3", receiver);
            p.Add("@4", mark); 

            var conn = DbService();
            return conn.Execute(sql, p);
        }

        /// <summary>
        /// 获取发件箱信息
        /// </summary>
        /// <param name="userid"></param>
        public List<Message> GetOutBoxMessage(string userid, int PageSize, int PageIndex, string strWhere, bool desc, string orderfldname, out int rstcount)
        {
            //加入草稿状态
            if (!string.IsNullOrEmpty(strWhere))
                strWhere += " AND ";

            strWhere += string.Format("Creator = '{0}' AND SendTime is not null", userid);

            return Clover.Data.BaseDAO.GetList<Message>("[msg_Message]", "MessageId", PageSize, PageIndex, strWhere, desc, orderfldname, out rstcount);

        }

        /// <summary>
        /// 获取草稿箱信息
        /// </summary>
        /// <param name="userid"></param>
        public List<Message> GetDraftMessage(string userid, int PageSize, int PageIndex, string strWhere, bool desc, string orderfldname, out int rstcount)
        {
            //加入草稿状态
            if (!string.IsNullOrEmpty(strWhere))
                strWhere += " AND ";

            strWhere += string.Format("Status is null AND Creator = '{0}'" , userid);
            return Clover.Data.BaseDAO.GetList<Message>("[msg_Message]", "MessageId", PageSize, PageIndex, strWhere, desc, orderfldname, out rstcount);

        }

        string inboxsql = @"(select bm.MessageId, bm.Status, bm.ReadTime, bm.OpTime, 
bm.Mark,bm.MessageBoxId,bm.ReceiveTime,bm.IsRecyle,bm.IsDelete,bm.Receiver,mm.CreatorName SenderName,
mm.Title,mm.SendTime,mm.Creator Sender,
mb.[Name] MessageBoxName,mb.BoxType
from dbo.msg_BoxMessage bm 
join msg_Message mm on bm.MessageId = mm.MessageId

join msg_MessageBox mb on bm.MessageBoxId = mb.MessageBoxId)t";
        /// <summary>
        /// 获取垃圾箱信息
        /// </summary>
        /// <param name="userid"></param>
        public List<Message> GetRecyleMessage(string userid, int PageSize, int PageIndex, string strWhere, bool desc, string orderfldname, out int rstcount)
        {
            if (!string.IsNullOrEmpty(strWhere))
                strWhere += " AND ";

            strWhere += string.Format("IsRecyle = 1 AND IsDelete = 0 AND Receiver = '{0}'", userid); // F代表已经删除到回收站


            return Clover.Data.BaseDAO.GetList<Message>(inboxsql, "MessageId", PageSize, PageIndex, strWhere, desc, orderfldname, out rstcount);
        }

        /// <summary>
        /// 回收垃圾箱信息
        /// </summary>
        /// <param name="userid"></param>
        public int RecyleMessage(string messageIds, string userid)
        {
            string[] ids = messageIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string sql = @"update msg_BoxMessage set IsRecyle = 1
where Receiver=@3 AND MessageId in ('" + string.Join("','", ids) + "')";
            var p = new DynamicParameters();
            p.Add("@3", userid);

            var conn = DbService();
            return conn.Execute(sql, p);
        }
        /// <summary>
        /// 回收垃圾箱信息
        /// </summary>
        /// <param name="userid"></param>
        public int DeleteALL(string messageIds, string userid)
        {
            string[] ids = messageIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //删除回收站消息
            string sql = @"update msg_BoxMessage set IsDelete = 1 where Receiver=@3 AND IsRecyle = 1 AND MessageId in ('" + string.Join("','", ids) + "');";
            //删除属于草稿箱的信息
            sql += @"delete msg_Message where Creator=@3 AND Status is null AND MessageId in ('" + string.Join("','", ids) + "');";
            var p = new DynamicParameters();
            p.Add("@3", userid);
            var conn = DbService();
            return conn.Execute(sql, p);
        }

        /// <summary>
        /// 回收垃圾箱信息
        /// </summary>
        /// <param name="userid"></param>
        public int Clean(int boxid, string userid)
        {
            string sql = @"update msg_BoxMessage set IsDelete = 1 where Receiver=@3 AND IsRecyle = 1";
            var p = new DynamicParameters();
            p.Add("@3", userid);
            p.Add("@4", boxid);
            var conn = DbService();
            return conn.Execute(sql, p);
        }
        /// <summary>
        /// 还原回收垃圾箱信息
        /// </summary>
        /// <param name="userid"></param>
        public int RestoreRecycleMessage(string messageids, string userid)
        {
            string[] ids = messageids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string sql = @"update msg_BoxMessage set IsRecyle = 0
where  Receiver=@3 AND MessageId in ('" + string.Join("','", ids) + "')";
            var p = new DynamicParameters();
            p.Add("@3", userid);
            var conn = DbService();
            return conn.Execute(sql, p);
        }


		#endregion
	}
}