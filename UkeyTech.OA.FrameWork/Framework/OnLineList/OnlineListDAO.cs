namespace UkeyTech.WebFW.DAO
{
	using System;
    using System.Collections.Generic;
    using System.Linq;
	using System.Data;
	using System.Data.Common;
	using System.Data.SqlClient;
    using System.Text;
	using Dapper;
 
	using UkeyTech.WebFW.Model;
    
	/// <summary>
	/// OnlineList 数据访问层
	/// </summary>
	public partial class OnlineListDAO : Clover.Data.BaseDAO
	{
		#region 构造函数
		public OnlineListDAO()
		{
		}
		#endregion		
		
		#region 根据主键创建 OnlineList 数据模型实例
		/// <summary>
		/// 根据主键创建 OnlineList 数据模型实例 
		/// </summary>
		public OnlineList GetModel(DateTime VisitDate)
		{           
            var p = new DynamicParameters();
            //获取主键
            p.Add("VisitDate",VisitDate ,DbType.DateTime,null,8);
            
            var conn = DbService();
            
            try{
                var rst = conn.Query<OnlineList>(
                @"select * from sys_OnlineList where 	[VisitDate] = @VisitDate
", p).ToList();
                if (rst.Count > 0)
                    return rst[0];
                else
                    return null;
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
		public  bool Update(OnlineList model)
		{
			return Update(null,model);
		}
		/// <summary>
		/// 更新记录到数据库(利用事务)
		/// </summary>
		public  bool Update(IDbConnection conn, OnlineList model)
		{		
			int affectedrows = 0;
           
            var p = new DynamicParameters();           
            p.Add("VisitDate",model.VisitDate ,DbType.DateTime,null,8);
            p.Add("IPs",model.IPs ,DbType.String,null,16);
            p.Add("VisitCount",model.VisitCount ,DbType.Int32,null,4);
            
                string sql = @"UPDATE [sys_OnlineList] SET	
	[IPs] = @IPs,
	[VisitCount] = @VisitCount
                    WHERE
                        	[VisitDate] = @VisitDate
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
		public  bool Insert(OnlineList model)
		{
			return Insert(null,model);
		}
		
		/// <summary>
		/// 新增记录到数据库(事务)
		/// </summary>
		public bool Insert(IDbConnection conn,OnlineList model)
		{
            var p = new DynamicParameters();           
            p.Add("AutoId",model.AutoId ,DbType.Int32,null,4);
            p.Add("VisitDate",model.VisitDate ,DbType.DateTime,null,8);
            p.Add("IPs",model.IPs ,DbType.String,null,16);
            p.Add("VisitCount",model.VisitCount ,DbType.Int32,null,4);
            
        
            string sql = @"INSERT INTO [sys_OnlineList] (                          
	[AutoId],
	[IPs],
	[VisitCount]
                        ) VALUES (
                            	@AutoId,
	@IPs,
	@VisitCount
)";
                                      
            
            if(conn == null)
                conn = DbService();

            try{
                conn.Execute(sql, p);
                                
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
		public bool Delete(DateTime VisitDate)
		{
			return Delete(null,VisitDate);
		}
		
		
		/// <summary>
		/// 从数据库删除记录(事务)
		/// </summary>
		public  bool Delete(IDbConnection conn,DateTime VisitDate)
		{
			  int affectedrows = 0;
        
            var p = new DynamicParameters();           
		     //获取主键
            p.Add("VisitDate",VisitDate ,DbType.DateTime,null,8);
            
            string sql = @"DELETE FROM [sys_OnlineList]
                        WHERE 	[VisitDate] = @VisitDate
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
		public  List<OnlineList> GetAll(string orderBy)
		{
			return GetList(null,null, string.Empty, orderBy);
		}
        
		/// <summary>
		/// 查询所有记录，并排序(事务)
		/// </summary>
		public List<OnlineList> GetList(IDbConnection conn,int? top, string strWhere, string orderBy)
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
	[VisitDate],
	[IPs],
	[VisitCount]
 ");
			strSql.Append(" FROM [sys_OnlineList] ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + orderBy);
            
            var rst = conn.Query<OnlineList>(strSql.ToString(), null);
                
            
            
			return new List<OnlineList>(rst);
		}
	
		/// <summary>
		/// 查询所有记录，并排序、分页
		/// </summary>
		public List<OnlineList> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname)
		{
            int rstcount = 0;
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc, orderfldname,out rstcount);
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页,没有排序字段
		/// </summary>
		public List<OnlineList> GetAllPaged(int PageSize,int PageIndex,string strWhere,bool desc)
		{
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc,"VisitDate" );
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页(事务)
		/// </summary>
		public List<OnlineList> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname, out int rstcount)
		{
		    return Clover.Data.BaseDAO.GetList<OnlineList>("[sys_OnlineList]","VisitDate",PageSize, PageIndex, strWhere ,desc, orderfldname, out rstcount);
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
            string strSQL = "select count(*) from [sys_OnlineList]";
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