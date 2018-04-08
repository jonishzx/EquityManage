namespace Clover.Permission.DAO
{
	using System;
    using System.Collections.Generic;
	using System.Data;
	using System.Data.Common;
	using System.Data.SqlClient;
    using System.Text;
	using Dapper;
 
	using Clover.Permission.Model;
    
	/// <summary>
	/// Log 数据访问层
	/// </summary>
	partial class LogDAO : Clover.Data.BaseDAO
	{
		#region 构造函数
		public LogDAO()
		{
		}
		#endregion		
		
		#region 根据主键创建 Log 数据模型实例
		/// <summary>
		/// 根据主键创建 Log 数据模型实例 
		/// </summary>
		public Log GetModel(int LogID)
		{           
            var p = new DynamicParameters();
            //获取主键
            p.Add("LogID",LogID ,DbType.Int32,null,4);
            
            var conn = DbService();
            
            try{
                var rst = conn.Query<Log>(
                @"select * from CPM_Log where 	[LogID] = @LogID
", p);
                
                return new List<Log>(rst)[0];
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
		public  bool Update(Log model)
		{
			return Update(null,model);
		}
		/// <summary>
		/// 更新记录到数据库(利用事务)
		/// </summary>
		public  bool Update(IDbConnection conn, Log model)
		{		
			int affectedrows = 0;
           
            var p = new DynamicParameters();           
            p.Add("LogID",model.LogID ,DbType.Int32,null,4);
            p.Add("LoginName",model.LoginName ,DbType.String,null,50);
            p.Add("UserName",model.UserName ,DbType.String,null,50);
            p.Add("LogTag",model.LogTag ,DbType.String,null,150);
            p.Add("UserIP",model.UserIP ,DbType.String,null,20);
            p.Add("Message",model.Message ,DbType.String,null,4000);
            p.Add("CreateTime",model.CreateTime ,DbType.DateTime,null,8);
            
                string sql = @"UPDATE [CPM_Log] SET
	[LoginName] = @LoginName,
	[UserName] = @UserName,
	[LogTag] = @LogTag,
	[UserIP] = @UserIP,
	[Message] = @Message,
	[CreateTime] = @CreateTime
                    WHERE
                        	[LogID] = @LogID
";
              
            if(conn == null)
                conn = DbService();

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
		
		#region 新增记录
		/// <summary>
		/// 新增记录到数据库
		/// </summary>
		public  bool Insert(Log model)
		{
			return Insert(null,model);
		}
		
		/// <summary>
		/// 新增记录到数据库(事务)
		/// </summary>
		public bool Insert(IDbConnection conn,Log model)
		{
            var p = new DynamicParameters();           
            p.Add("LogID",model.LogID ,DbType.Int32,null,4);
            p.Add("LoginName",model.LoginName ,DbType.String,null,50);
            p.Add("UserName",model.UserName ,DbType.String,null,50);
            p.Add("LogTag",model.LogTag ,DbType.String,null,150);
            p.Add("UserIP",model.UserIP ,DbType.String,null,20);
            p.Add("Message",model.Message ,DbType.String,null,4000);
            p.Add("CreateTime",model.CreateTime ,DbType.DateTime,null,8);
            
        
            string sql = @"INSERT INTO [CPM_Log] (                          
	[LoginName],
	[UserName],
	[LogTag],
	[UserIP],
	[Message],
	[CreateTime]
                        ) VALUES (
                            	@LoginName,
	@UserName,
	@LogTag,
	@UserIP,
	@Message,
	@CreateTime
)";
                            
            sql += ";select @@IDENTITY";
            
            if(conn == null)
                conn = DbService();

            try{
                var keys = new List<decimal>(conn.Query<decimal>(sql, p));
                
                model.LogID = Convert.ToInt32(keys[0]);                
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
		public bool Delete(int LogID)
		{
			return Delete(null,LogID);
		}
		
		
		/// <summary>
		/// 从数据库删除记录(事务)
		/// </summary>
		public  bool Delete(IDbConnection conn,int LogID)
		{
			  int affectedrows = 0;
        
            var p = new DynamicParameters();           
		     //获取主键
            p.Add("LogID",LogID ,DbType.Int32,null,4);
            
            string sql = @"DELETE FROM [CPM_Log]
                        WHERE 	[LogID] = @LogID
";
            
            if(conn == null)
                conn = DbService();

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
		public  List<Log> GetAll(string orderBy)
		{
			return GetList(null,null, string.Empty, orderBy);
		}
        
		/// <summary>
		/// 查询所有记录，并排序(事务)
		/// </summary>
		public List<Log> GetList(IDbConnection conn,int? top, string strWhere, string orderBy)
		{
            if(conn == null)
                conn = DbService();
            
            StringBuilder strSql=new StringBuilder();
			strSql.Append("Select ");
			if(top.HasValue)
			{
				strSql.Append(" top "+top.ToString());
			}
			strSql.Append(@" 	[LogID],
	[LoginName],
	[UserName],
	[LogTag],
	[UserIP],
	[Message],
	[CreateTime]
 ");
			strSql.Append(" FROM [CPM_Log] ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + orderBy);
            
            var rst = conn.Query<Log>(strSql.ToString(), null);
                
            
            
			return new List<Log>(rst);
		}
	
		/// <summary>
		/// 查询所有记录，并排序、分页
		/// </summary>
		public List<Log> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname)
		{
            int rstcount = 0;
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc, orderfldname,out rstcount);
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页,没有排序字段
		/// </summary>
		public List<Log> GetAllPaged(int PageSize,int PageIndex,string strWhere,bool desc)
		{
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc,"LogID" );
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页(事务)
		/// </summary>
		public List<Log> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname, out int rstcount)
		{
		    return Clover.Data.BaseDAO.GetList<Log>("[CPM_Log]","LogID",PageSize, PageIndex, strWhere ,desc, orderfldname, out rstcount);
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
            string strSQL = "select count(*) from [CPM_Log]";
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