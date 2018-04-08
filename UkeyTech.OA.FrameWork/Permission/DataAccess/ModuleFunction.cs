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
	/// Module_Function 数据访问层
	/// </summary>
	partial class ModuleFunctionDAO : Clover.Data.BaseDAO
	{
		#region 构造函数
		public ModuleFunctionDAO()
		{
		}
		#endregion		
		
		#region 根据主键创建 Module_Function 数据模型实例
		/// <summary>
		/// 根据主键创建 Module_Function 数据模型实例 
		/// </summary>
		public ModuleFunction GetModel(int ModuleID,int FunctionID)
		{           
            var p = new DynamicParameters();
            //获取主键
            p.Add("ModuleID",ModuleID ,DbType.Int32,null,4);
            p.Add("FunctionID",FunctionID ,DbType.Int32,null,4);
            
            var conn = DbService();
            
            try{
                var rst = conn.Query<ModuleFunction>(
                @"select * from CPM_Module_Function where 	[ModuleID] = @ModuleID
	AND [FunctionID] = @FunctionID
", p);
                
                return new List<ModuleFunction>(rst)[0];
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
		public  bool Update(ModuleFunction model)
		{
			return Update(null,model);
		}
		/// <summary>
		/// 更新记录到数据库(利用事务)
		/// </summary>
		public  bool Update(IDbConnection conn, ModuleFunction model)
		{		
			int affectedrows = 0;
           
            var p = new DynamicParameters();           
            p.Add("ModuleID",model.ModuleID ,DbType.Int32,null,4);
            p.Add("FunctionID",model.FunctionID ,DbType.Int32,null,4);
            
                string sql = @"UPDATE [CPM_Module_Function] SET
                    WHERE
                        	[ModuleID] = @ModuleID
	AND [FunctionID] = @FunctionID
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
		public  bool Insert(ModuleFunction model)
		{
			return Insert(null,model);
		}
		
		/// <summary>
		/// 新增记录到数据库(事务)
		/// </summary>
		public bool Insert(IDbTransaction tran,ModuleFunction model)
		{
            var p = new DynamicParameters();
            p.Add("ModuleID", model.ModuleID, DbType.Int32, null, 4);
            p.Add("FunctionID",model.FunctionID ,DbType.Int32,null,4);


            string sql = @"INSERT INTO [CPM_Module_Function] 
                        Select @ModuleID,@FunctionID where NOT Exists(select 1 from [CPM_Module_Function] Where ModuleID=@ModuleID and FunctionID = @FunctionID)";   
         
            IDbConnection conn = tran == null ? DbService() : tran.Connection;

            try{
               conn.Execute(sql, p, tran, null, null);
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
		public bool Delete(int ModuleID,int FunctionID)
		{
			return Delete(null,ModuleID,FunctionID);
		}
		
		
		/// <summary>
		/// 从数据库删除记录(事务)
		/// </summary>
		public  bool Delete(IDbConnection conn,int ModuleID,int FunctionID)
		{
			  int affectedrows = 0;
        
            var p = new DynamicParameters();           
		     //获取主键
            p.Add("ModuleID",ModuleID ,DbType.Int32,null,4);
            p.Add("FunctionID",FunctionID ,DbType.Int32,null,4);
            
            string sql = @"DELETE FROM [CPM_Module_Function]
                        WHERE 	[ModuleID] = @ModuleID
	AND [FunctionID] = @FunctionID
";
            
            if(conn == null)
                conn = DbService();

            try{
                affectedrows = conn.Execute(sql, p);
            }
            catch(DataException ex){
                throw ex;
            }

			return Convert.ToBoolean(affectedrows);
		}
		
		#endregion
		
		#region 查询，返回自定义类			
		/// <summary>
		/// 查询所有记录，并排序
		/// </summary>
		public  List<ModuleFunction> GetAll(string orderBy)
		{
			return GetList(null,null, string.Empty, orderBy);
		}
        
		/// <summary>
		/// 查询所有记录，并排序(事务)
		/// </summary>
		public List<ModuleFunction> GetList(IDbConnection conn,int? top, string strWhere, string orderBy)
		{
            if(conn == null)
                conn = DbService();
            
            StringBuilder strSql=new StringBuilder();
			strSql.Append("Select ");
			if(top.HasValue)
			{
				strSql.Append(" top "+top.ToString());
			}
			strSql.Append(@" 	[ModuleID],
	[FunctionID]
 ");
			strSql.Append(" FROM [CPM_Module_Function] ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + orderBy);
            
            var rst = conn.Query<ModuleFunction>(strSql.ToString(), null);
                
            
            
			return new List<ModuleFunction>(rst);
		}
	
		/// <summary>
		/// 查询所有记录，并排序、分页
		/// </summary>
		public List<ModuleFunction> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname)
		{
            int rstcount = 0;
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc, orderfldname,out rstcount);
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页,没有排序字段
		/// </summary>
		public List<ModuleFunction> GetAllPaged(int PageSize,int PageIndex,string strWhere,bool desc)
		{
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc,"ModuleID,FunctionID" );
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页(事务)
		/// </summary>
		public List<ModuleFunction> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname, out int rstcount)
		{
		    return Clover.Data.BaseDAO.GetList<ModuleFunction>("[CPM_Module_Function]","ModuleID,FunctionID",PageSize, PageIndex, strWhere ,desc, orderfldname, out rstcount);
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
            string strSQL = "select count(*) from [CPM_Module_Function]";
            strSQL = string.IsNullOrEmpty(strWhere)? strSQL : strSQL + " where " + strWhere;
            var conn = DbService();
		    IEnumerable<int> rst = conn.Query<int>(strSQL, null);
            
            return new List<int>(rst)[0];            
            
		}
		#endregion
		
		#region 其他自定义方法
        public virtual int DeleteModuleFunctions(int ModuleID, params string[] functionIds)
        {

            return DeleteModuleFunctions(null, ModuleID, functionIds);
        }

        public virtual int DeleteModuleFunctions(IDbTransaction tran, int ModuleID, params string[] functionIds)
        {
            var conn = tran == null ?  DbService() : tran.Connection;

            string str = string.Format("ModuleID = {0} AND FunctionID IN ({1}) ", ModuleID,
                Clover.Core.Common.StringHelper.Join(",", Clover.Core.Common.StringJoinOption.NoLastJoinFlag, functionIds));

            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("DELETE FROM CPM_FuncPermission WHERE {0};", str);
            builder.AppendFormat("DELETE FROM CPM_Module_Function WHERE {0};", str);

            return conn.Execute(
                builder.ToString(),null, tran, null, null);

        }

      
        
		#endregion
	}
}