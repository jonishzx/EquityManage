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
	/// Module 数据访问层
	/// </summary>
	public partial class ModuleDAO : Clover.Data.BaseDAO
	{
		#region 构造函数
		public ModuleDAO()
		{
		}
		#endregion		
		
		#region 根据主键创建 Module 数据模型实例
		/// <summary>
		/// 根据主键创建 Module 数据模型实例 
		/// </summary>
		public Module GetModel(int ModuleID)
		{           
            Module model = null;
            var p = new DynamicParameters();
            //获取主键
            p.Add("ModuleID",ModuleID ,DbType.Int32,null,4);
            
            var conn = DbService();
            
            try{
                var rst = conn.Query<Module>(
                @"select * from CPM_Module where 	[ModuleID] = @ModuleID
", p);
                
                List<Module> lrst 
                    = new List<Module>(rst);
                
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
		public  bool Update(Module model)
		{
			return Update(null,model);
		}
		/// <summary>
		/// 更新记录到数据库(利用事务)
		/// </summary>
		public  bool Update(IDbTransaction tran, Module model)
		{		
			int affectedrows = 0;
           
            var p = new DynamicParameters();           
            p.Add("ModuleID",model.ModuleID ,DbType.Int32,null,4);
            p.Add("ModuleCode",model.ModuleCode ,DbType.String,null,50);
            p.Add("ModuleName",model.ModuleName ,DbType.String,null,50);
            p.Add("ModuleTag",model.ModuleTag ,DbType.String,null,400);
            p.Add("SystemID",model.SystemID ,DbType.Int32,null,4);
            p.Add("Descn",model.Descn ,DbType.String,null,200);
            p.Add("Target",model.Target ,DbType.String,null,100);
            p.Add("TargetFrame",model.TargetFrame ,DbType.String,null,100);
            p.Add("ImageUrl",model.ImageUrl ,DbType.String,null,100);
            p.Add("Attribute",model.Attribute ,DbType.String,null,16);
            p.Add("ParentID",model.ParentID ,DbType.Int32,null,4);
            p.Add("ParentPath",model.ParentPath ,DbType.String,null,200);
            p.Add("ParentTitle",model.ParentTitle ,DbType.String,null,500);
            p.Add("ViewOrd",model.ViewOrd ,DbType.Int32,null,4);
            p.Add("Visible",model.Visible ,DbType.Int32,null,4);
            p.Add("CreateTime",model.CreateTime ,DbType.DateTime,null,8);
            p.Add("UpdateTime",model.UpdateTime ,DbType.DateTime,null,8);
            p.Add("Creator",model.Creator ,DbType.String,null,20);
            p.Add("Modifitor",model.Modifitor ,DbType.String,null,20);
            p.Add("Status",model.Status ,DbType.Int32,null,4);
            
                string sql = @"UPDATE [CPM_Module] SET
	[ModuleCode] = @ModuleCode,
	[ModuleName] = @ModuleName,
	[ModuleTag] = @ModuleTag,
	[SystemID] = @SystemID,
	[Descn] = @Descn,
	[Target] = @Target,
	[TargetFrame] = @TargetFrame,
	[ImageUrl] = @ImageUrl,
	[Attribute] = @Attribute,
	[ParentID] = @ParentID,
	[ParentPath] = @ParentPath,
	[ParentTitle] = @ParentTitle,
	[ViewOrd] = @ViewOrd,
	[Visible] = @Visible,
	[CreateTime] = @CreateTime,
	[UpdateTime] = @UpdateTime,
	[Creator] = @Creator,
	[Modifitor] = @Modifitor,
	[Status] = @Status
                    WHERE
                        	[ModuleID] = @ModuleID
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
		public  bool Insert(Module model)
		{
			return Insert(null,model);
		}
		
		/// <summary>
		/// 新增记录到数据库(事务)
		/// </summary>
		public bool Insert(IDbTransaction tran, Module model)
		{
            var p = new DynamicParameters();           
            p.Add("ModuleID",model.ModuleID ,DbType.Int32,null,4);
            p.Add("ModuleCode",model.ModuleCode ,DbType.String,null,50);
            p.Add("ModuleName",model.ModuleName ,DbType.String,null,50);
            p.Add("ModuleTag",model.ModuleTag ,DbType.String,null,400);
            p.Add("SystemID",model.SystemID ,DbType.Int32,null,4);
            p.Add("Descn",model.Descn ,DbType.String,null,200);
            p.Add("Target",model.Target ,DbType.String,null,100);
            p.Add("TargetFrame",model.TargetFrame ,DbType.String,null,100);
            p.Add("ImageUrl",model.ImageUrl ,DbType.String,null,100);
            p.Add("Attribute",model.Attribute ,DbType.String,null,16);
            p.Add("ParentID",model.ParentID ,DbType.Int32,null,4);
            p.Add("ParentPath",model.ParentPath ,DbType.String,null,200);
            p.Add("ParentTitle",model.ParentTitle ,DbType.String,null,500);
            p.Add("ViewOrd",model.ViewOrd ,DbType.Int32,null,4);
            p.Add("Visible",model.Visible ,DbType.Int32,null,4);
            p.Add("CreateTime",model.CreateTime ,DbType.DateTime,null,8);
            p.Add("UpdateTime",model.UpdateTime ,DbType.DateTime,null,8);
            p.Add("Creator",model.Creator ,DbType.String,null,20);
            p.Add("Modifitor",model.Modifitor ,DbType.String,null,20);
            p.Add("Status",model.Status ,DbType.Int32,null,4);
            
        
            string sql = @"INSERT INTO [CPM_Module] (                          
	[ModuleCode],
	[ModuleName],
	[ModuleTag],
	[SystemID],
	[Descn],
	[Target],
	[TargetFrame],
	[ImageUrl],
	[Attribute],
	[ParentID],
	[ParentPath],
	[ParentTitle],
	[ViewOrd],
	[Visible],
	[CreateTime],
	[UpdateTime],
	[Creator],
	[Modifitor],
	[Status]
                        ) VALUES (
                            	@ModuleCode,
	@ModuleName,
	@ModuleTag,
	@SystemID,
	@Descn,
	@Target,
	@TargetFrame,
	@ImageUrl,
	@Attribute,
	@ParentID,
	@ParentPath,
	@ParentTitle,
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
                
                model.ModuleID = Convert.ToInt32(keys[0]);                
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
		public bool Delete(int ModuleID)
		{
			return Delete(null,ModuleID);
		}
		
		
		/// <summary>
		/// 从数据库删除记录(事务)
		/// </summary>
		public  bool Delete(IDbTransaction tran, int ModuleID)
		{
			  int affectedrows = 0;
        
            var p = new DynamicParameters();           
		     //获取主键
            p.Add("ModuleID",ModuleID ,DbType.Int32,null,4);
            
            string sql = @"DELETE FROM [CPM_Module]
                        WHERE 	[ModuleID] = @ModuleID
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
		public  List<Module> GetAll(string orderBy)
		{
			return GetList(null,null, string.Empty, orderBy);
		}
        
		/// <summary>
		/// 查询所有记录，并排序(事务)
		/// </summary>
		public List<Module> GetList(IDbConnection conn,int? top, string strWhere, string orderBy)
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
	[ModuleCode],
	[ModuleName],
	[ModuleTag],
	[SystemID],
	[Descn],
	[Target],
	[TargetFrame],
	[ImageUrl],
	[Attribute],
	[ParentID],
	[ParentPath],
	[ParentTitle],
	[ViewOrd],
	[Visible],
	[CreateTime],
	[UpdateTime],
	[Creator],
	[Modifitor],
	[Status]
 ");
			strSql.Append(" FROM [CPM_Module] ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
           
            if (orderBy != string.Empty)
                strSql.Append(" order by " + orderBy);
			
            
            var rst = conn.Query<Module>(strSql.ToString(), null);
                
			return new List<Module>(rst);
		}
	
		/// <summary>
		/// 查询所有记录，并排序、分页
		/// </summary>
		public List<Module> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname)
		{
            int rstcount = 0;
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc, orderfldname,out rstcount);
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页,没有排序字段
		/// </summary>
		public List<Module> GetAllPaged(int PageSize,int PageIndex,string strWhere,bool desc)
		{
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc,"ModuleID" );
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页(事务)
		/// </summary>
		public List<Module> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname, out int rstcount)
		{
		    return Clover.Data.BaseDAO.GetList<Module>("[CPM_Module]","ModuleID",PageSize, PageIndex, strWhere ,desc, orderfldname, out rstcount);
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
            string strSQL = "select count(*) from [CPM_Module]";
            strSQL = string.IsNullOrEmpty(strWhere)? strSQL : strSQL + " where " + strWhere;
            var conn = DbService();
		    IEnumerable<int> rst = conn.Query<int>(strSQL, null);
            
            return new List<int>(rst)[0];            
            
		}
		#endregion

        #region 其他自定义方法
        /// <summary>
        /// 查询所有记录，并排序、分页(事务)
        /// </summary>
        public List<Module> GetAllWithFunctionPaged(IDbConnection conn, int PageSize, int PageIndex, string strWhere, bool desc, string orderfldname, out int rstcount)
        {
            var sql = "(select *,dbo.Get_FunctionByModuleID(ModuleID) Functions from CPM_Module)t";
            return Clover.Data.BaseDAO.GetList<Module>(sql, "ModuleID", PageSize, PageIndex, strWhere, desc, orderfldname, out rstcount);
        }

        public void UpdateStatus(IDbTransaction tran, int systemId, int Status)
        {

            tran.Connection.Execute("UPDATE [CPM_Module] SET Status = @Status WHERE SystemId = @SystemId",
                  new { SystemId = systemId, Status = Status }, tran, null, CommandType.Text);

           }
        

        public void RefreshModuleParentPath(int moduleid, string path, IDbTransaction tran)
        {

            tran.Connection.Execute("UPDATE [CPM_Module] SET ParentPath = @ParentPath WHERE ModuleID = @ModuleID",
                  new { ModuleID = moduleid, ParentPath = path }, tran, null, CommandType.Text);

        }

        public void UpChildModule(int ModuleId, int parentModuleId, IDbTransaction tran)
        {
            tran.Connection.Execute("UPDATE [CPM_Module] SET ParentID = @ParentModuleID WHERE ParentID = @ModuleID",
                          new { ModuleID = ModuleId, ParentModuleID = parentModuleId }, tran, null, null);

        }

        /// <summary>
        /// 从数据库删除记录(事务)
        /// </summary>
        public int DeleteModule(int ID, IDbTransaction tran)
        {
            var p = new DynamicParameters();
            //获取主键
            p.Add("ModuleID", ID, DbType.Int32, null, 4);

            string sql = @"DELETE [CPM_Module_Function] WHERE [ModuleID] = @ModuleID;";
            sql += @"DELETE [CPM_FuncPermission]  WHERE [ModuleID] = @ModuleID;";
            sql += @"DELETE [CPM_Module] WHERE [ModuleID] = @ModuleID;";

            return tran.Connection.Execute(sql, p, tran, null, null);
        }

        public List<Module> GetEnabledModules()
        {
            return new List<Module>(DbService().Query<Module>(
                @"select * from CPM_Module where Visible = 1 and Status = 1", null));         
        }

        public Module GetModuleByCode(string ModuleCode)
        {
            Module model = null;
            var p = new DynamicParameters();
            //获取主键
            p.Add("ModuleCode", ModuleCode, DbType.String, null, 20);

            var conn = DbService();

            try
            {
                var rst = conn.Query<Module>(
                @"select * from CPM_Module where [ModuleCode] = @ModuleCode", p);

                model = new List<Module>(rst)[0];
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

        public List<Module> GetSystemModules(int systemId)
        {
            return GetList(null, null, "SystemID=" + systemId.ToString(), "");            
        }

        /// <summary>
        /// 查询所有记录，并排序(事务)
        /// </summary>
        public List<Module> GetListWithOpen(IDbConnection conn, int? top, string strWhere, string orderBy)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("Select ");
            if (top.HasValue)
            {
                strSql.Append(" top " + top.ToString());
            }
            strSql.Append(@" 	*
 ");
            strSql.Append(" FROM [CPM_Module] ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            if (orderBy != string.Empty)
                strSql.Append(" order by " + orderBy);

            var rst = conn.Query<Module>(strSql.ToString(), null);

            return new List<Module>(rst);
        }

       
        #endregion
	}
}