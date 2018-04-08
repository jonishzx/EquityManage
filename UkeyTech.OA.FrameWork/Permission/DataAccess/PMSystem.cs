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
	/// MainSystem 数据访问层
	/// </summary>
	public partial class PMSystemDAO : Clover.Data.BaseDAO
	{
		#region 构造函数
		public PMSystemDAO()
		{
		}
		#endregion		
		
		#region 根据主键创建 MainSystem 数据模型实例
		/// <summary>
		/// 根据主键创建 MainSystem 数据模型实例 
		/// </summary>
		public PMSystem GetModel(int SystemID)
		{           
            PMSystem model = null;
            var p = new DynamicParameters();
            //获取主键
            p.Add("SystemID",SystemID ,DbType.Int32,null,4);
            
            var conn = DbService();
            
            try{
                var rst = conn.Query<PMSystem>(
                @"select * from CPM_PMSystem where [SystemID] = @SystemID", p);
                
                List<PMSystem> lrst 
                    = new  List<PMSystem>(rst);
                
                if(lrst.Count > 0)
                    model = lrst[0];
            }
            catch(DataException ex){
                throw ex;
            }finally{
                
            }
		
			return model;
		}
		#endregion
			
		#region 更新记录
		/// <summary>
		/// 更新记录到数据库
		/// </summary>
		public  bool Update(PMSystem model)
		{
			return Update(null,model);
		}
		/// <summary>
		/// 更新记录到数据库(利用事务)
		/// </summary>
		public  bool Update(IDbTransaction tran, PMSystem model)
		{		
			int affectedrows = 0;
           
            var p = new DynamicParameters();           
            p.Add("SystemID",model.SystemID ,DbType.Int32,null,4);
            p.Add("SystemCode",model.SystemCode ,DbType.String,null,50);
            p.Add("SystemName",model.SystemName ,DbType.String,null,50);
            p.Add("Descn",model.Descn ,DbType.String,null,200);
            p.Add("ViewOrd",model.ViewOrd ,DbType.Int32,null,4);
            p.Add("Visible",model.Visible ,DbType.Int32,null,4);
            p.Add("CreateTime",model.CreateTime ,DbType.DateTime,null,8);
            p.Add("UpdateTime",model.UpdateTime ,DbType.DateTime,null,8);
            p.Add("Creator",model.Creator ,DbType.String,null,20);
            p.Add("Modifitor",model.Modifitor ,DbType.String,null,20);
            p.Add("Status",model.Status ,DbType.Int32,null,4);
            
                string sql = @"UPDATE [CPM_PMSystem] SET
	[SystemCode] = @SystemCode,
	[SystemName] = @SystemName,
	[Descn] = @Descn,
	[ViewOrd] = @ViewOrd,
	[Visible] = @Visible,
	[CreateTime] = @CreateTime,
	[UpdateTime] = @UpdateTime,
	[Creator] = @Creator,
	[Modifitor] = @Modifitor,
	[Status] = @Status
                    WHERE
                        	[SystemID] = @SystemID;
";           
            IDbConnection conn = tran == null ?  DbService() : tran.Connection;
         
            try{
                affectedrows = conn.Execute(sql, p);
            }
            catch(DataException ex){
                throw ex;
            }
			
			return Convert.ToBoolean(affectedrows);
		}
		#endregion
		
		#region 新增记录
		/// <summary>
		/// 新增记录到数据库
		/// </summary>
		public  bool Insert(PMSystem model)
		{
			return Insert(null,model);
		}
		
		/// <summary>
		/// 新增记录到数据库(事务)
		/// </summary>
		public bool Insert(IDbTransaction tran, PMSystem model)
		{
            var p = new DynamicParameters();           
            p.Add("SystemID",model.SystemID ,DbType.Int32,null,4);
            p.Add("SystemCode",model.SystemCode ,DbType.String,null,50);
            p.Add("SystemName",model.SystemName ,DbType.String,null,50);
            p.Add("Descn",model.Descn ,DbType.String,null,200);
            p.Add("ViewOrd",model.ViewOrd ,DbType.Int32,null,4);
            p.Add("Visible",model.Visible ,DbType.Int32,null,4);
            p.Add("CreateTime",model.CreateTime ,DbType.DateTime,null,8);
            p.Add("UpdateTime",model.UpdateTime ,DbType.DateTime,null,8);
            p.Add("Creator",model.Creator ,DbType.String,null,20);
            p.Add("Modifitor",model.Modifitor ,DbType.String,null,20);
            p.Add("Status",model.Status ,DbType.Int32,null,4);
            
        
            string sql = @"INSERT INTO [CPM_PMSystem] (                          
	[SystemCode],
	[SystemName],
	[Descn],
	[ViewOrd],
	[Visible],
	[CreateTime],
	[UpdateTime],
	[Creator],
	[Modifitor],
	[Status]
                        ) VALUES (
                            	@SystemCode,
	@SystemName,
	@Descn,
	@ViewOrd,
	@Visible,
	@CreateTime,
	@UpdateTime,
	@Creator,
	@Modifitor,
	@Status
)";
                            
            sql += ";select @@IDENTITY";
            
            IDbConnection conn = tran == null ?  DbService() : tran.Connection;

            try{
                var keys = new List<decimal>(conn.Query<decimal>(sql, p));
                
                model.SystemID = Convert.ToInt32(keys[0]);                
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
		public bool Delete(int SystemID)
		{
			return Delete(null,SystemID);
		}
		
		
		/// <summary>
		/// 从数据库删除记录(事务)
		/// </summary>
		public  bool Delete(IDbTransaction tran, int SystemID)
		{
			  int affectedrows = 0;
        
            var p = new DynamicParameters();           
		     //获取主键
            p.Add("SystemID",SystemID ,DbType.Int32,null,4);


            string sql = "DELETE FROM CPM_FuncPermission WHERE ModuleID IN (SELECT ModuleID FROM CPM_Module WHERE SystemID = @SystemID) ";
            sql += "DELETE FROM CPM_Module_Function WHERE ModuleID IN (SELECT ModuleID FROM CPM_Module WHERE SystemID = @SystemID) ";
            sql += "DELETE FROM CPM_Module WHERE SystemID = @SystemID ";
            sql += @"DELETE FROM [CPM_PMSystem] WHERE [SystemID] = @SystemID;"; 
            
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
		public  List<PMSystem> GetAll(string orderBy)
		{
			return GetList(null,null, string.Empty, orderBy);
		}
        
		/// <summary>
		/// 查询所有记录，并排序(事务)
		/// </summary>
		public List<PMSystem> GetList(IDbConnection conn,int? top, string strWhere, string orderBy)
		{
            if(conn == null)
                conn = DbService();
            
            StringBuilder strSql=new StringBuilder();
			strSql.Append("Select ");
			if(top.HasValue)
			{
				strSql.Append(" top "+top.ToString());
			}
			strSql.Append(@" 	[SystemID],
	[SystemCode],
	[SystemName],
	[Descn],
	[ViewOrd],
	[Visible],
	[CreateTime],
	[UpdateTime],
	[Creator],
	[Modifitor],
	[Status]
 ");
			strSql.Append(" FROM [CPM_PMSystem] ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
           
            if (orderBy != string.Empty)
                strSql.Append(" order by " + orderBy);
			
            
            var rst = conn.Query<PMSystem>(strSql.ToString(), null);
                
            
            
			return new List<PMSystem>(rst);
		}
	
		/// <summary>
		/// 查询所有记录，并排序、分页
		/// </summary>
		public List<PMSystem> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname)
		{
            int rstcount = 0;
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc, orderfldname,out rstcount);
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页,没有排序字段
		/// </summary>
		public List<PMSystem> GetAllPaged(int PageSize,int PageIndex,string strWhere,bool desc)
		{
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc,"SystemID" );
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页(事务)
		/// </summary>
		public List<PMSystem> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname, out int rstcount)
		{
		    return Clover.Data.BaseDAO.GetList<PMSystem>("[CPM_PMSystem]","SystemID",PageSize, PageIndex, strWhere ,desc, orderfldname, out rstcount);
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
            string strSQL = "select count(*) from [CPM_PMSystem]";
            strSQL = string.IsNullOrEmpty(strWhere)? strSQL : strSQL + " where " + strWhere;
            var conn = DbService();
		    IEnumerable<int> rst = conn.Query<int>(strSQL, null);
            
            return new List<int>(rst)[0];            
            
		}
		#endregion
		
		#region 其他自定义方法
        public PMSystem GetPMSystemByCode(string SystemCode)
        {
            PMSystem model = null;
            var p = new DynamicParameters();
            //获取主键
            p.Add("SystemCode", SystemCode, DbType.String, null, 20);

            var conn = DbService();

            try
            {
                var rst = conn.Query<PMSystem>(
                @"select * from CPM_PMSystem where 	[SystemCode] = @SystemCode", p);

                List<PMSystem> lrst
                    = new List<PMSystem>(rst);

                if (lrst.Count > 0)
                    model = new List<PMSystem>(rst)[0];
            }
            catch (DataException ex)
            {
                throw ex;
            }
            finally
            {
                
            }

            return model;
        }

		#endregion
	}
}