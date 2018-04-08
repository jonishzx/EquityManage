namespace Clover.Message.DAO
{
	using System;
    using System.Linq;
    using System.Collections.Generic;
	using System.Data;
	using System.Data.Common;
	using System.Data.SqlClient;
    using System.Text;
	using Dapper;
    using Dapper.Contrib.Extensions;
    
	using Clover.Message.Model;
    
	/// <summary>
	/// MessageBox 数据访问层
	/// </summary>
	public partial class MessageBoxDAO : Clover.Data.BaseDAO
	{
		#region 构造函数
		public MessageBoxDAO()
		{
		}
		#endregion		
		
		#region 根据主键创建 MessageBox 数据模型实例
		/// <summary>
		/// 根据主键创建 MessageBox 数据模型实例 
		/// </summary>
		public MessageBox GetModel(int MessageBoxId)
		{           
            var conn = DbService();
            
            try{
                var rst = conn.Get<MessageBox>(MessageBoxId);
                return rst;
            }
            catch(DataException ex){
                throw ex;
            }finally{
                
            }
		}
		#endregion
			
		#region 更新记录
		/// <summary>
		/// 更新记录到数据库
		/// </summary>
		public  bool Update(MessageBox model)
		{
			return Update(null,model);
		}
		/// <summary>
		/// 更新记录到数据库(利用事务)
		/// </summary>
		public  bool Update(IDbTransaction tran, MessageBox model)
		{		
		
            IDbConnection conn = tran == null ?  DbService() : tran.Connection;
            bool rst = false;
            try{
                rst = conn.Update(model, tran);
            }
            catch(DataException ex){
                throw ex;
            }finally{
                
            }
			
			return rst;
		}
		#endregion
		
		#region 新增记录
		/// <summary>
		/// 新增记录到数据库
		/// </summary>
		public  bool Insert(MessageBox model)
		{
			return Insert(null,model);
		}
		
		/// <summary>
		/// 新增记录到数据库(事务)
		/// </summary>
		public bool Insert(IDbTransaction tran, MessageBox model)
		{
         
            IDbConnection conn = tran == null ?  DbService() : tran.Connection;

            try{
                long id = conn.Insert(model, tran);
                    model.MessageBoxId = (int)id;
            }
            catch(DataException ex){
                throw ex;
            }finally{
                
            }

			return true;
		}
		#endregion
		
		#region 删除记录
        
        /// <summary>
		/// 从数据库删除记录(事务)
		/// </summary>
		public bool Delete(MessageBox model)
		{
			return Delete(model, null);
		}
        
         /// <summary>
		/// 从数据库删除记录(事务)
		/// </summary>
		public bool Delete(MessageBox model, IDbTransaction tran )
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
		public bool Delete(int MessageBoxId)
		{
			return Delete(null,MessageBoxId);
		}
		
		/// <summary>
		/// 从数据库删除记录(事务)
		/// </summary>
		public  bool Delete(IDbTransaction tran, int MessageBoxId)
		{
			int affectedrows = 0;
        
            var p = new DynamicParameters();           
		     //获取主键
            p.Add("MessageBoxId",MessageBoxId ,DbType.Int32,null,4);
            
            string sql = @"DELETE FROM [msg_MessageBox]
                        WHERE 	[MessageBoxId] = @MessageBoxId
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
		public  List<MessageBox> GetAll(string orderBy)
		{
			return GetList(null,null, string.Empty, orderBy);
		}
        
		/// <summary>
		/// 查询所有记录，并排序(事务)
		/// </summary>
		public List<MessageBox> GetList(IDbConnection conn,int? top, string strWhere, string orderBy)
		{
            if(conn == null)
                conn = DbService();
            
            StringBuilder strSql=new StringBuilder();
			strSql.Append("Select ");
			if(top.HasValue)
			{
				strSql.Append(" top "+top.ToString());
			}
			strSql.Append(@" 	[MessageBoxId],
	[Name],
	[Status],
	[Owner],
	[CreateTime],
	[BoxType],
	[Parent]
 ");
			strSql.Append(" FROM [msg_MessageBox] ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
           
            if (orderBy != string.Empty)
                strSql.Append(" order by " + orderBy);
			
            
            var rst = conn.Query<MessageBox>(strSql.ToString(), null);
                
            
            
			return new List<MessageBox>(rst);
		}
	
		/// <summary>
		/// 查询所有记录，并排序、分页
		/// </summary>
		public List<MessageBox> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname)
		{
            int rstcount = 0;
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc, orderfldname,out rstcount);
		}
		
        /// <summary>
		/// 查询所有记录，并排序、分页,没有排序字段
		/// </summary>
		public List<MessageBox> GetAllPaged(int PageSize,int PageIndex,string strWhere,bool desc)
		{
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc,"MessageBoxId" );
		}
        
		/// <summary>
		/// 查询所有记录，并排序、分页,没有排序字段
		/// </summary>
		public List<MessageBox> GetAllPaged(int PageSize,int PageIndex,string strWhere,bool desc,  out int rstcount)
		{
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc,"MessageBoxId", out rstcount );
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页(事务)
		/// </summary>
		public List<MessageBox> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname, out int rstcount)
		{
		    return Clover.Data.BaseDAO.GetList<MessageBox>("[msg_MessageBox]","MessageBoxId",PageSize, PageIndex, strWhere ,desc, orderfldname, out rstcount);
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
            string strSQL = "select count(*) from [msg_MessageBox]";
            strSQL = string.IsNullOrEmpty(strWhere)? strSQL : strSQL + " where " + strWhere;
            var conn = DbService();
		    IEnumerable<int> rst = conn.Query<int>(strSQL, null);
            
            return new List<int>(rst)[0];            
            
		}
		#endregion
		
		#region 其他自定义方法
        /// <summary>
        /// 自动创建用户的消息箱
        /// </summary>
        /// <param name="userid"></param>
        public List<MessageBox> AutoCreateMessageBoxAndGetBox(string userid) {
            string sql = @"if(not exists(select 1 from msg_MessageBox
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

select *, 
(case when BoxType = 'InBox'
            then (select count(*) from msg_BoxMessage where Receiver = @1 and ReadTime is null)
            else 0 end) InBoxCount
from msg_MessageBox where Owner = @1;";
            var p = new DynamicParameters();
            p.Add("@1", userid);

          
            return DbService().Query<MessageBox>(sql, p).ToList();
          
        }
		#endregion
	}
}