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
    
	/// <summary>
	/// MessageTemplate 数据访问层
	/// </summary>
	public partial class MessageTemplateDAO : Clover.Data.BaseDAO
	{
		#region 构造函数
		public MessageTemplateDAO()
		{
		}
		#endregion		
		
		#region 根据主键创建 MessageTemplate 数据模型实例
		/// <summary>
		/// 根据主键创建 MessageTemplate 数据模型实例 
		/// </summary>
		public MessageTemplate GetModel(string Code)
		{           
            var conn = DbService();
            
            try{
                var rst = conn.Get<MessageTemplate>(Code);
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
		public  bool Update(MessageTemplate model)
		{
			return Update(null,model);
		}
		/// <summary>
		/// 更新记录到数据库(利用事务)
		/// </summary>
		public  bool Update(IDbTransaction tran, MessageTemplate model)
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
		public  bool Insert(MessageTemplate model)
		{
			return Insert(null,model);
		}
		
		/// <summary>
		/// 新增记录到数据库(事务)
		/// </summary>
		public bool Insert(IDbTransaction tran, MessageTemplate model)
		{
         
            IDbConnection conn = tran == null ?  DbService() : tran.Connection;

            try{
                conn.Insert(model, tran);
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
		public bool Delete(MessageTemplate model)
		{
			return Delete(model, null);
		}
        
         /// <summary>
		/// 从数据库删除记录(事务)
		/// </summary>
		public bool Delete(MessageTemplate model, IDbTransaction tran )
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
		public bool Delete(string Code)
		{
			return Delete(null,Code);
		}
		
		/// <summary>
		/// 从数据库删除记录(事务)
		/// </summary>
		public  bool Delete(IDbTransaction tran, string Code)
		{
			int affectedrows = 0;
        
            var p = new DynamicParameters();           
		     //获取主键
            p.Add("Code",Code ,DbType.String,null,50);
            
            string sql = @"DELETE FROM [msg_MessageTemplate]
                        WHERE 	[Code] = @Code
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
		public  List<MessageTemplate> GetAll(string orderBy)
		{
			return GetList(null,null, string.Empty, orderBy);
		}
        
		/// <summary>
		/// 查询所有记录，并排序(事务)
		/// </summary>
		public List<MessageTemplate> GetList(IDbConnection conn,int? top, string strWhere, string orderBy)
		{
            if(conn == null)
                conn = DbService();
            
            StringBuilder strSql=new StringBuilder();
			strSql.Append("Select ");
			if(top.HasValue)
			{
				strSql.Append(" top "+top.ToString());
			}
			strSql.Append(@" 	[Code],
	[Name],
	[MessageAction],
	[OperationAction],
	[ExtendAction1],
	[ExtendAction2],
	[Descn],
	[Creator],
	[CreateTime],
	[UpdateTime],
	[Status],
	[NeedAccept],
	[NeedRead]
 ");
			strSql.Append(" FROM [msg_MessageTemplate] ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
           
            if (orderBy != string.Empty)
                strSql.Append(" order by " + orderBy);
			
            
            var rst = conn.Query<MessageTemplate>(strSql.ToString(), null);
                
            
            
			return new List<MessageTemplate>(rst);
		}
	
		/// <summary>
		/// 查询所有记录，并排序、分页
		/// </summary>
		public List<MessageTemplate> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname)
		{
            int rstcount = 0;
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc, orderfldname,out rstcount);
		}
		
        /// <summary>
		/// 查询所有记录，并排序、分页,没有排序字段
		/// </summary>
		public List<MessageTemplate> GetAllPaged(int PageSize,int PageIndex,string strWhere,bool desc)
		{
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc,"Code" );
		}
        
		/// <summary>
		/// 查询所有记录，并排序、分页,没有排序字段
		/// </summary>
		public List<MessageTemplate> GetAllPaged(int PageSize,int PageIndex,string strWhere,bool desc,  out int rstcount)
		{
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc,"Code", out rstcount );
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页(事务)
		/// </summary>
		public List<MessageTemplate> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname, out int rstcount)
		{
		    return Clover.Data.BaseDAO.GetList<MessageTemplate>("[msg_MessageTemplate]","Code",PageSize, PageIndex, strWhere ,desc, orderfldname, out rstcount);
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
            string strSQL = "select count(*) from [msg_MessageTemplate]";
            strSQL = string.IsNullOrEmpty(strWhere)? strSQL : strSQL + " where " + strWhere;
            var conn = DbService();
		    IEnumerable<int> rst = conn.Query<int>(strSQL, null);
            
            return new List<int>(rst)[0];            
            
		}
		#endregion
		
		#region 其他自定义方法
		
		#endregion
	}
}