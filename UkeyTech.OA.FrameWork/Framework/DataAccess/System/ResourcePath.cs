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
	/// ResourcePath 数据访问层
	/// </summary>
	public partial class ResourcePathDAO : Clover.Data.BaseDAO
	{
		#region 构造函数
		public ResourcePathDAO()
		{
		}
		#endregion		
		
		#region 根据主键创建 ResourcePath 数据模型实例
		/// <summary>
		/// 根据主键创建 ResourcePath 数据模型实例 
		/// </summary>
		public ResourcePath GetModel(int id)
		{           
            var p = new DynamicParameters();
            //获取主键
            p.Add("id",id ,DbType.Int32,null,4);
            
            var conn = DbService();
            
            try{
                var rst = conn.Query<ResourcePath>(
                @"select * from sys_ResourcePath where 	[id] = @id
", p);
                
                return new List<ResourcePath>(rst)[0];
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
		public  bool Update(ResourcePath model)
		{
			return Update(null,model);
		}
		/// <summary>
		/// 更新记录到数据库(利用事务)
		/// </summary>
		public  bool Update(IDbConnection conn, ResourcePath model)
		{		
			int affectedrows = 0;
           
            var p = new DynamicParameters();           
            p.Add("id",model.id ,DbType.Int32,null,4);
            p.Add("Name",model.Name ,DbType.String,null,50);
            p.Add("Path",model.Path ,DbType.String,null,500);
            p.Add("DownloadUrl",model.DownloadUrl ,DbType.String,null,500);
            p.Add("ViewOrder",model.ViewOrder ,DbType.Int32,null,4);
            p.Add("Remark",model.Remark ,DbType.String,null,100);
            p.Add("Status",model.Status ,DbType.Int32,null,4);
            p.Add("PTypeId",model.PTypeId ,DbType.String,null,20);
            
                string sql = @"UPDATE [sys_ResourcePath] SET
	[Name] = @Name,
	[Path] = @Path,
	[DownloadUrl] = @DownloadUrl,
	[ViewOrder] = @ViewOrder,
	[Remark] = @Remark,
	[Status] = @Status,
	[PTypeId] = @PTypeId
                    WHERE
                        	[id] = @id
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
		public  bool Insert(ResourcePath model)
		{
			return Insert(null,model);
		}
		
		/// <summary>
		/// 新增记录到数据库(事务)
		/// </summary>
		public bool Insert(IDbConnection conn,ResourcePath model)
		{
            var p = new DynamicParameters();           
            p.Add("id",model.id ,DbType.Int32,null,4);
            p.Add("Name",model.Name ,DbType.String,null,50);
            p.Add("Path",model.Path ,DbType.String,null,500);
            p.Add("DownloadUrl",model.DownloadUrl ,DbType.String,null,500);
            p.Add("ViewOrder",model.ViewOrder ,DbType.Int32,null,4);
            p.Add("Remark",model.Remark ,DbType.String,null,100);
            p.Add("Status",model.Status ,DbType.Int32,null,4);
            p.Add("PTypeId",model.PTypeId ,DbType.String,null,20);
            
        
            string sql = @"INSERT INTO [sys_ResourcePath] (                          
	[Name],
	[Path],
	[DownloadUrl],
	[ViewOrder],
	[Remark],
	[Status],
	[PTypeId]
                        ) VALUES (
                            	@Name,
	@Path,
	@DownloadUrl,
	@ViewOrder,
	@Remark,
	@Status,
	@PTypeId
)";
                            
            sql += ";select @@IDENTITY";
            
            if(conn == null)
                conn = DbService();

            try{
                var keys = new List<decimal>(conn.Query<decimal>(sql, p));
                
                model.id = Convert.ToInt32(keys[0]);                
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
		public bool Delete(int id)
		{
			return Delete(null,id);
		}
		
		
		/// <summary>
		/// 从数据库删除记录(事务)
		/// </summary>
		public  bool Delete(IDbConnection conn,int id)
		{
			  int affectedrows = 0;
        
            var p = new DynamicParameters();           
		     //获取主键
            p.Add("id",id ,DbType.Int32,null,4);
            
            string sql = @"DELETE FROM [sys_ResourcePath]
                        WHERE 	[id] = @id
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
		public  List<ResourcePath> GetAll(string orderBy)
		{
			return GetList(null,null, string.Empty, orderBy);
		}
        
		/// <summary>
		/// 查询所有记录，并排序(事务)
		/// </summary>
		public List<ResourcePath> GetList(IDbConnection conn,int? top, string strWhere, string orderBy)
		{
            if(conn == null)
                conn = DbService();
            
            StringBuilder strSql=new StringBuilder();
			strSql.Append("Select ");
			if(top.HasValue)
			{
				strSql.Append(" top "+top.ToString());
			}
			strSql.Append(@" 	[id],
	[Name],
	[Path],
	[DownloadUrl],
	[ViewOrder],
	[Remark],
	[Status],
	[PTypeId]
 ");
			strSql.Append(" FROM [sys_ResourcePath] ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
            if (orderBy != string.Empty)
                strSql.Append(" order by " + orderBy);
            
            var rst = conn.Query<ResourcePath>(strSql.ToString(), null);
                
            
            
			return new List<ResourcePath>(rst);
		}
	
		/// <summary>
		/// 查询所有记录，并排序、分页
		/// </summary>
		public List<ResourcePath> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname)
		{
            int rstcount = 0;
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc, orderfldname,out rstcount);
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页,没有排序字段
		/// </summary>
		public List<ResourcePath> GetAllPaged(int PageSize,int PageIndex,string strWhere,bool desc)
		{
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc,"id" );
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页(事务)
		/// </summary>
		public List<ResourcePath> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname, out int rstcount)
		{
		    return Clover.Data.BaseDAO.GetList<ResourcePath>("[sys_ResourcePath]","id",PageSize, PageIndex, strWhere ,desc, orderfldname, out rstcount);
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
            string strSQL = "select count(*) from [sys_ResourcePath]";
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