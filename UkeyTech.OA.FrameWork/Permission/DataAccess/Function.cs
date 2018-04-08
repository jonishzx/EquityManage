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
	/// Function 数据访问层
	/// </summary>
	public partial class FunctionDAO : Clover.Data.BaseDAO
	{
		#region 构造函数
		public FunctionDAO()
		{
		}
		#endregion		
		
		#region 根据主键创建 Function 数据模型实例
		/// <summary>
		/// 根据主键创建 Function 数据模型实例 
		/// </summary>
		public Function GetModel(int FunctionID)
		{           
            Function model = null;
            var p = new DynamicParameters();
            //获取主键
            p.Add("FunctionID",FunctionID ,DbType.Int32,null,4);
            
            var conn = DbService();
            
            try{
                var rst = conn.Query<Function>(
                @"select * from CPM_Function where 	[FunctionID] = @FunctionID
", p);
                
                List<Function> lrst 
                    = new  List<Function>(rst);
                
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
		public  bool Update(Function model)
		{
			return Update(null,model);
		}
		/// <summary>
		/// 更新记录到数据库(利用事务)
		/// </summary>
		public  bool Update(IDbTransaction tran, Function model)
		{		
			int affectedrows = 0;
           
            var p = new DynamicParameters();           
            p.Add("FunctionID",model.FunctionID ,DbType.Int32,null,4);
            p.Add("FunctionCode",model.FunctionCode ,DbType.String,null,50);
            p.Add("FunctionName",model.FunctionName ,DbType.String,null,50);
            p.Add("FunctionTag",model.FunctionTag ,DbType.String,null,50);
            p.Add("Descn",model.Descn ,DbType.String,null,200);
            p.Add("RelationFunctionID",model.RelationFunctionID ,DbType.String,null,200);
            p.Add("CreateTime",model.CreateTime ,DbType.DateTime,null,8);
            p.Add("UpdateTime",model.UpdateTime ,DbType.DateTime,null,8);
            p.Add("Creator",model.Creator ,DbType.String,null,20);
            p.Add("Modifitor",model.Modifitor ,DbType.String,null,20);
            p.Add("Status",model.Status ,DbType.Int32,null,4);
            p.Add("ViewOrd",model.ViewOrd ,DbType.Int32,null,4);
            
                string sql = @"UPDATE [CPM_Function] SET
	[FunctionCode] = @FunctionCode,
	[FunctionName] = @FunctionName,
	[FunctionTag] = @FunctionTag,
	[Descn] = @Descn,
	[RelationFunctionID] = @RelationFunctionID,
	[CreateTime] = @CreateTime,
	[UpdateTime] = @UpdateTime,
	[Creator] = @Creator,
	[Modifitor] = @Modifitor,
	[Status] = @Status,
	[ViewOrd] = @ViewOrd
                    WHERE
                        	[FunctionID] = @FunctionID
";
            IDbConnection conn = tran == null ?  DbService() : tran.Connection;
         
            try{
                if(tran == null)
                    affectedrows = conn.Execute(sql, p);
                else
                    conn.Execute(sql, p, tran, null, null);
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
		public  bool Insert(Function model)
		{
			return Insert(null,model);
		}
		
		/// <summary>
		/// 新增记录到数据库(事务)
		/// </summary>
		public bool Insert(IDbTransaction tran, Function model)
		{
            var p = new DynamicParameters();           
            p.Add("FunctionID",model.FunctionID ,DbType.Int32,null,4);
            p.Add("FunctionCode",model.FunctionCode ,DbType.String,null,50);
            p.Add("FunctionName",model.FunctionName ,DbType.String,null,50);
            p.Add("FunctionTag",model.FunctionTag ,DbType.String,null,50);
            p.Add("Descn",model.Descn ,DbType.String,null,200);
            p.Add("RelationFunctionID",model.RelationFunctionID ,DbType.String,null,200);
            p.Add("CreateTime",model.CreateTime ,DbType.DateTime,null,8);
            p.Add("UpdateTime",model.UpdateTime ,DbType.DateTime,null,8);
            p.Add("Creator",model.Creator ,DbType.String,null,20);
            p.Add("Modifitor",model.Modifitor ,DbType.String,null,20);
            p.Add("Status",model.Status ,DbType.Int32,null,4);
            p.Add("ViewOrd",model.ViewOrd ,DbType.Int32,null,4);
            
        
            string sql = @"INSERT INTO [CPM_Function] (                          
	[FunctionCode],
	[FunctionName],
	[FunctionTag],
	[Descn],
	[RelationFunctionID],
	[CreateTime],
	[UpdateTime],
	[Creator],
	[Modifitor],
	[Status],
	[ViewOrd]
                        ) VALUES (
                            	@FunctionCode,
	@FunctionName,
	@FunctionTag,
	@Descn,
	@RelationFunctionID,
	@CreateTime,
	@UpdateTime,
	@Creator,
	@Modifitor,
	@Status,
	@ViewOrd
)";
                            
            sql += ";select @@IDENTITY";
            
            IDbConnection conn = tran == null ?  DbService() : tran.Connection;

            try{
                var keys = new List<decimal>(conn.Query<decimal>(sql, p));
                
                model.FunctionID = Convert.ToInt32(keys[0]);                
            }
            catch(DataException ex){
                throw ex;
            }

			return true;
		}

		#endregion
		
		#region 删除记录
			/// <summary>
		/// 从数据库删除记录(事务)
		/// </summary>
		public bool Delete(int FunctionID)
		{
			return Delete(null,FunctionID);
		}
		
		
		/// <summary>
		/// 从数据库删除记录(事务)
		/// </summary>
		public  bool Delete(IDbTransaction tran, int FunctionID)
		{
			  int affectedrows = 0;
        
            var p = new DynamicParameters();           
		     //获取主键
            p.Add("FunctionID",FunctionID ,DbType.Int32,null,4);
            
            string sql = @"DELETE FROM [CPM_Function]
                        WHERE 	[FunctionID] = @FunctionID
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
		public  List<Function> GetAll(string orderBy)
		{
			return GetList(null,null, string.Empty, orderBy);
		}
        
		/// <summary>
		/// 查询所有记录，并排序(事务)
		/// </summary>
		public List<Function> GetList(IDbConnection conn,int? top, string strWhere, string orderBy)
		{
            if(conn == null)
                conn = DbService();
            
            StringBuilder strSql=new StringBuilder();
			strSql.Append("Select ");
			if(top.HasValue)
			{
				strSql.Append(" top "+top.ToString());
			}
			strSql.Append(@" 	[FunctionID],
	[FunctionCode],
	[FunctionName],
	[FunctionTag],
	[Descn],
	[RelationFunctionID],
	[CreateTime],
	[UpdateTime],
	[Creator],
	[Modifitor],
	[Status],
	[ViewOrd]
 ");
			strSql.Append(" FROM [CPM_Function] ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
           
            if (orderBy != string.Empty)
                strSql.Append(" order by " + orderBy);
			
            
            var rst = conn.Query<Function>(strSql.ToString(), null);
                
            
            
			return new List<Function>(rst);
		}
	
		/// <summary>
		/// 查询所有记录，并排序、分页
		/// </summary>
		public List<Function> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname)
		{
            int rstcount = 0;
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc, orderfldname,out rstcount);
		}
		
        /// <summary>
		/// 查询所有记录，并排序、分页,没有排序字段
		/// </summary>
		public List<Function> GetAllPaged(int PageSize,int PageIndex,string strWhere,bool desc)
		{
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc,"FunctionID" );
		}
        
		/// <summary>
		/// 查询所有记录，并排序、分页,没有排序字段
		/// </summary>
		public List<Function> GetAllPaged(int PageSize,int PageIndex,string strWhere,bool desc,  out int rstcount)
		{
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc,"FunctionID", out rstcount );
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页(事务)
		/// </summary>
		public List<Function> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname, out int rstcount)
		{
		    return Clover.Data.BaseDAO.GetList<Function>("[CPM_Function]","FunctionID",PageSize, PageIndex, strWhere ,desc, orderfldname, out rstcount);
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
            string strSQL = "select count(*) from [CPM_Function]";
            strSQL = string.IsNullOrEmpty(strWhere)? strSQL : strSQL + " where " + strWhere;
            var conn = DbService();
		    IEnumerable<int> rst = conn.Query<int>(strSQL, null);
            
            return new List<int>(rst)[0];            
            
		}
		#endregion
		
		#region 其他自定义方法
        public Function GetFunctionByCode(string funcCode)
        {
            Function model = null;
            var p = new DynamicParameters();
            //获取主键
            p.Add("FunctionCode", funcCode, DbType.String, null, 20);

            var conn = DbService();

            try
            {
                var rst = conn.Query<Function>(
                @"select * from CPM_Function where 	[FunctionCode] = @FunctionCode  And Status > 0
", p);

                model = new List<Function>(rst)[0];
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

        public List<Function> GetModuleFunctions(int ModuleID)
        {

            DynamicParameters p = new DynamicParameters();
            p.Add("ModuleID", ModuleID, DbType.Int16, null, null);

            string sql = "SELECT a.* FROM CPM_Function a INNER JOIN CPM_Module_Function b ON a.FunctionID = b.FunctionID WHERE b.ModuleID = @ModuleID order by viewOrd asc,FunctionName ";

            var conn = DbService();

            return new List<Function>(conn.Query<Function>(sql, p));
        }

        public List<Function> GetModuleFunctions(string ModuleCode)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("ModuleCode", ModuleCode, DbType.String, null, null);

            string sql = @"SELECT a.* FROM CPM_Function a 
            INNER JOIN CPM_Module_Function b ON a.FunctionID = b.FunctionID
            INNER JOIN CPM_Module c ON b.ModuleID = c.ModuleID 
            WHERE c.ModuleCode = @ModuleCode";

            var conn = DbService();

            return new List<Function>(conn.Query<Function>(sql, p));
        }

        public Module GetFunctionSpModule(int FunctionID)
        {

            DynamicParameters p = new DynamicParameters();
            p.Add("FunctionID", FunctionID, DbType.Int16, null, null);

            string sql = "SELECT top 1 a.* FROM [CPM_Module] a Where exists (select 1 from [CPM_Module_Function] b where a.ModuleID = b.ModuleID and b.FunctionID = @FunctionID)";

            var conn = DbService();

            var rst = new List<Module>(conn.Query<Module>(sql, p));

            if (rst.Count > 0)
                return rst[0];
            else
                return null;
        }

        public List<Function> GetFunctionByNoModuleRef(int moduleId, string where)
        {
            string str = @"Select * FROM [CPM_Function] 
                            Where FunctionID NOT IN (SELECT FunctionID FROM CPM_Module_Function WHERE ModuleID = @ModuleID) 
                            AND (FunctionTag <> 'Special')
                            ";

            if (!string.IsNullOrEmpty(where))
            {
                str = str + " AND " + where;
            }
            var conn = DbService();
            var rst = conn.Query<Function>(str, new { ModuleID = moduleId });
            
            return new List<Function>(rst);
        }
		#endregion
	}
}