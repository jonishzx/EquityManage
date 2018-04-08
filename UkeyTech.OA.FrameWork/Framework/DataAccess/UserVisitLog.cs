namespace UkeyTech.WebFW.DAO
{
	using System;
    using System.Collections.Generic;
	using System.Data;
	using System.Data.Common;
	using System.Data.SqlClient;
    using System.Text;
	using Dapper;
 
	using UkeyTech.WebFW.Model;
    
	/// <summary>
	/// UserVisitLog 数据访问层
	/// </summary>
	public partial class UserVisitLogDAO : Clover.Data.BaseDAO
	{
		#region 构造函数
		public UserVisitLogDAO()
		{
		}
		#endregion		
		
		#region 根据主键创建 UserVisitLog 数据模型实例
		/// <summary>
		/// 根据主键创建 UserVisitLog 数据模型实例 
		/// </summary>
		public UserVisitLog GetModel(int AutoId)
		{           
            var p = new DynamicParameters();
            //获取主键
            p.Add("AutoId",AutoId ,DbType.Int32,null,4);
            
            var conn = DbService();
            
            try{
                var rst = conn.Query<UserVisitLog>(
                @"select * from sys_UserVisitLog where 	[AutoId] = @AutoId
", p);
                
                return new List<UserVisitLog>(rst)[0];
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
		public  bool Update(UserVisitLog model)
		{
			return Update(null,model);
		}
		/// <summary>
		/// 更新记录到数据库(利用事务)
		/// </summary>
		public  bool Update(IDbConnection conn, UserVisitLog model)
		{		
			int affectedrows = 0;
           
            var p = new DynamicParameters();           
            p.Add("AutoId",model.AutoId ,DbType.Int32,null,4);
            p.Add("IP",model.IP ,DbType.String,null,20);
            p.Add("Url",model.Url ,DbType.String,null,250);
            p.Add("Visit",model.Visit ,DbType.DateTime,null,8);
            
                string sql = @"UPDATE [sys_UserVisitLog] SET
	[IP] = @IP,
	[Url] = @Url,
	[Visit] = @Visit
                    WHERE
                        	[AutoId] = @AutoId
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
		public  bool Insert(UserVisitLog model)
		{
			return Insert(null,model);
		}
		
		/// <summary>
		/// 新增记录到数据库(事务)
		/// </summary>
		public bool Insert(IDbConnection conn,UserVisitLog model)
		{
            var p = new DynamicParameters();           
            p.Add("AutoId",model.AutoId ,DbType.Int32,null,4);
            p.Add("IP",model.IP ,DbType.String,null,20);
            p.Add("Url",model.Url ,DbType.String,null,250);
            p.Add("Visit",model.Visit ,DbType.DateTime,null,8);
            
        
            string sql = @"INSERT INTO [sys_UserVisitLog] (                          
	[IP],
	[Url],
	[Visit]
                        ) VALUES (
                            	@IP,
	@Url,
	@Visit
)";
                            
            sql += ";select @@IDENTITY";
            
            if(conn == null)
                conn = DbService();

            try{
                var keys = new List<decimal>(conn.Query<decimal>(sql, p));
                
                model.AutoId = Convert.ToInt32(keys[0]);                
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
		public bool Delete(int AutoId)
		{
			return Delete(null,AutoId);
		}
		
		
		/// <summary>
		/// 从数据库删除记录(事务)
		/// </summary>
		public  bool Delete(IDbConnection conn,int AutoId)
		{
			  int affectedrows = 0;
        
            var p = new DynamicParameters();           
		     //获取主键
            p.Add("AutoId",AutoId ,DbType.Int32,null,4);
            
            string sql = @"DELETE FROM [sys_UserVisitLog]
                        WHERE 	[AutoId] = @AutoId
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
		public  List<UserVisitLog> GetAll(string orderBy)
		{
			return GetList(null,null, string.Empty, orderBy);
		}
        
		/// <summary>
		/// 查询所有记录，并排序(事务)
		/// </summary>
		public List<UserVisitLog> GetList(IDbConnection conn,int? top, string strWhere, string orderBy)
		{
            if(conn == null)
                conn = DbService();
            
            StringBuilder strSql=new StringBuilder();
			strSql.Append("Select ");
			if(top.HasValue)
			{
				strSql.Append(" top "+top.ToString());
			}
			strSql.Append(@" 	[AutoId],
	[IP],
	[Url],
	[Visit]
 ");
			strSql.Append(" FROM [sys_UserVisitLog] ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + orderBy);
            
            var rst = conn.Query<UserVisitLog>(strSql.ToString(), null);
                
            
            
			return new List<UserVisitLog>(rst);
		}
	
		/// <summary>
		/// 查询所有记录，并排序、分页
		/// </summary>
		public List<UserVisitLog> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname)
		{
            int rstcount = 0;
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc, orderfldname,out rstcount);
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页,没有排序字段
		/// </summary>
		public List<UserVisitLog> GetAllPaged(int PageSize,int PageIndex,string strWhere,bool desc)
		{
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc,"AutoId" );
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页(事务)
		/// </summary>
		public List<UserVisitLog> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname, out int rstcount)
		{
		    return Clover.Data.BaseDAO.GetList<UserVisitLog>("[sys_UserVisitLog]","AutoId",PageSize, PageIndex, strWhere ,desc, orderfldname, out rstcount);
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
            string strSQL = "select count(*) from [sys_UserVisitLog]";
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