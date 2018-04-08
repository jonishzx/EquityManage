namespace Clover.Permission.DAO
{
	using System;
    using System.Collections.Generic;
	using System.Data;
	using System.Data.Common;
    using System.Transactions;
    using System.Text;
	using Dapper;

    using Clover.Core.Collection;
	using Clover.Permission.Model;
    
	/// <summary>
	/// Role 数据访问层
	/// </summary>
	public partial class RoleDAO : Clover.Data.BaseDAO
	{
		#region 构造函数
		public RoleDAO()
		{
		}
		#endregion		
		
		#region 根据主键创建 Role 数据模型实例
		/// <summary>
		/// 根据主键创建 Role 数据模型实例 
		/// </summary>
		public Role GetModel(int RoleID)
		{           
            Role model = null;
            var p = new DynamicParameters();
            //获取主键
            p.Add("RoleID",RoleID ,DbType.Int32,null,4);
            
            var conn = DbService();
            
            try{
                var rst = conn.Query<Role>(
                @"select * from CPM_Role where 	[RoleID] = @RoleID
", p);

                List<Role> lrst = new List<Role>(rst);
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
		public  bool Update(Role model)
		{
			return Update(null,model);
		}
		/// <summary>
		/// 更新记录到数据库(利用事务)
		/// </summary>
        public bool Update(IDbTransaction tran, Role model)
		{		
			int affectedrows = 0;
           
            var p = new DynamicParameters();           
            p.Add("RoleID",model.RoleID ,DbType.Int32,null,4);
            p.Add("RoleCode",model.RoleCode ,DbType.String,null,20);
            p.Add("RoleName",model.RoleName ,DbType.String,null,50);
            p.Add("RoleTag",model.RoleTag ,DbType.String,null,20);
            p.Add("Descn",model.Descn ,DbType.String,null,200);
            p.Add("ParentID",model.ParentID ,DbType.Int32,null,4);
            p.Add("ParentPath",model.ParentPath ,DbType.String,null,200);
            p.Add("ViewOrd",model.ViewOrd ,DbType.Int32,null,4);
            p.Add("CreateTime",model.CreateTime ,DbType.DateTime,null,8);
            p.Add("UpdateTime",DateTime.Now ,DbType.DateTime,null,8);
            p.Add("Creator",model.Creator ,DbType.String,null,20);
            p.Add("Modifitor",model.Modifitor ,DbType.String,null,20);
            p.Add("Status",model.Status ,DbType.Int32,null,4);
            
                string sql = @"UPDATE [CPM_Role] SET
	[RoleCode] = @RoleCode,
	[RoleName] = @RoleName,
	[RoleTag] = @RoleTag,
	[Descn] = @Descn,
	[ParentID] = @ParentID,
	[ParentPath] = @ParentPath,
	[ViewOrd] = @ViewOrd,
	[CreateTime] = @CreateTime,
	[UpdateTime] = @UpdateTime,
	[Creator] = @Creator,
	[Modifitor] = @Modifitor,
	[Status] = @Status
                    WHERE
                        	[RoleID] = @RoleID
";

            IDbConnection conn = tran == null ? DbService() : tran.Connection;

            try{
                affectedrows = conn.Execute(sql, p, tran, null, CommandType.Text);
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
		public  bool Insert(Role model)
		{
			return Insert(null,model);
		}
		
		/// <summary>
		/// 新增记录到数据库(事务)
		/// </summary>
		public bool Insert(IDbTransaction tran,Role model)
		{
            var p = new DynamicParameters();           
            p.Add("RoleID",model.RoleID ,DbType.Int32,null,4);
            p.Add("RoleCode",model.RoleCode ,DbType.String,null,20);
            p.Add("RoleName",model.RoleName ,DbType.String,null,50);
            p.Add("RoleTag",model.RoleTag ,DbType.String,null,20);
            p.Add("Descn",model.Descn ,DbType.String,null,200);
            p.Add("ParentID",model.ParentID ,DbType.Int32,null,4);
            p.Add("ParentPath",model.ParentPath ,DbType.String,null,200);
            p.Add("ViewOrd",model.ViewOrd ,DbType.Int32,null,4);
            p.Add("CreateTime",model.CreateTime ,DbType.DateTime,null,8);
            p.Add("UpdateTime",model.UpdateTime ,DbType.DateTime,null,8);
            p.Add("Creator",model.Creator ,DbType.String,null,20);
            p.Add("Modifitor",model.Modifitor ,DbType.String,null,20);
            p.Add("Status",model.Status ,DbType.Int32,null,4);
            
        
            string sql = @"INSERT INTO [CPM_Role] (                          
	[RoleCode],
	[RoleName],
	[RoleTag],
	[Descn],
	[ParentID],
	[ParentPath],
	[ViewOrd],
	[CreateTime],
	[UpdateTime],
	[Creator],
	[Modifitor],
	[Status]
                        ) VALUES (
                            	@RoleCode,
	@RoleName,
	@RoleTag,
	@Descn,
	@ParentID,
	@ParentPath,
	@ViewOrd,
	@CreateTime,
	@UpdateTime,
	@Creator,
	@Modifitor,
	@Status
)";
                            
            sql += ";select @@IDENTITY";
            
            IDbConnection conn = tran == null ?  DbService() : tran.Connection;

            try{
                var keys = new List<decimal>(conn.Query<decimal>(sql, p, tran, true, null, CommandType.Text));
                
                model.RoleID = Convert.ToInt32(keys[0]);                
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
		public bool Delete(int RoleID)
		{
			return Delete(null,RoleID);
		}
		
		
		/// <summary>
		/// 从数据库删除记录(事务)
		/// </summary>
		public  bool Delete(IDbTransaction tran,int RoleID)
		{
			  int affectedrows = 0;
        
            var p = new DynamicParameters();           
		     //获取主键
            p.Add("RoleID",RoleID ,DbType.Int32,null,4);
            
            string sql = @"DELETE FROM [CPM_Role]
                        WHERE 	[RoleID] = @RoleID
";

            IDbConnection conn = tran == null ? DbService() : tran.Connection;

            try{
                affectedrows = conn.Execute(sql, p, tran, null, CommandType.Text);
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
		public  List<Role> GetAll(string orderBy)
		{
			return GetList(null,null, string.Empty, orderBy);
		}
        
		/// <summary>
		/// 查询所有记录，并排序(事务)
		/// </summary>
		public List<Role> GetList(IDbConnection conn,int? top, string strWhere, string orderBy)
		{
            if(conn == null)
                conn = DbService();
            
            StringBuilder strSql=new StringBuilder();
			strSql.Append("Select ");
			if(top.HasValue)
			{
				strSql.Append(" top "+top.ToString());
			}
			strSql.Append(@" 	[RoleID],
	[RoleCode],
	[RoleName],
	[RoleTag],
	[Descn],
	[ParentID],
	[ParentPath],
	[ViewOrd],
	[CreateTime],
	[UpdateTime],
	[Creator],
	[Modifitor],
	[Status]
 ");
			strSql.Append(" FROM [CPM_Role] ");
            
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}

            if (orderBy!=string.Empty)
			    strSql.Append(" order by " + orderBy);
            
            var rst = conn.Query<Role>(strSql.ToString(), null);
                
			return new List<Role>(rst);
		}
	
		/// <summary>
		/// 查询所有记录，并排序、分页
		/// </summary>
		public List<Role> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname)
		{
            int rstcount = 0;
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc, orderfldname,out rstcount);
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页,没有排序字段
		/// </summary>
		public List<Role> GetAllPaged(int PageSize,int PageIndex,string strWhere,bool desc)
		{
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc,"RoleID" );
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页(事务)
		/// </summary>
		public List<Role> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname, out int rstcount)
		{
		    return Clover.Data.BaseDAO.GetList<Role>("[CPM_Role]","RoleID",PageSize, PageIndex, strWhere ,desc, orderfldname, out rstcount);
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
            string strSQL = "select count(*) from [CPM_Role]";
            strSQL = string.IsNullOrEmpty(strWhere)? strSQL : strSQL + " where " + strWhere;
            var conn = DbService();
		    IEnumerable<int> rst = conn.Query<int>(strSQL, null);
            
            return new List<int>(rst)[0];            
            
		}
		#endregion
		
		#region 其他自定义方法
        public void RefreshRoleParentPath(int roleid,string path, IDbTransaction tran)
        {

            tran.Connection.Execute("UPDATE [CPM_Role] SET ParentPath = @ParentPath WHERE RoleID = @RoleID",
                  new { RoleID = roleid, ParentPath = path }, tran, null, CommandType.Text);                
      
        }

        public void UpChildRole(int roleId, int parentRoleId,  IDbTransaction tran)
        {
            tran.Connection.Execute("UPDATE [CPM_Role] SET ParentID = @ParentRoleID WHERE ParentID = @RoleID",
                          new { RoleID = roleId, ParentRoleID = parentRoleId },tran, null, null);
            
        }

        /// <summary>
        /// 从数据库删除记录(事务)
        /// </summary>
        public int DeleteRole(int ID, IDbTransaction tran)
        {           
            var p = new DynamicParameters();
            //获取主键
            p.Add("RoleID", ID, DbType.Int32, null, 4);

            string sql = @"DELETE [CPM_Group_Role] WHERE [RoleID] = @RoleID;";
            sql += @"DELETE [CPM_Role_User] WHERE [RoleID] = @RoleID;";
            sql += @"DELETE [CPM_FuncPermission]  WHERE [RoleID] = @RoleID;";
            sql += @"DELETE [CPM_Role] WHERE [RoleID] = @RoleID;";
     
            return tran.Connection.Execute(sql, p, tran, null, null);         
        }

        public List<Role> GetRoleByNoUserRef(string userId, string where)
        {
            string str = "Select * From [CPM_Role] Where RoleID NOT IN (SELECT RoleID From CPM_Role_User WHERE UserID = @UserID)";
            if (!string.IsNullOrEmpty(where))
            {
                str = str + " AND " + where;
            }

            var conn = DbService();
            var rst = conn.Query<Role>(str, new { UserID = userId });
            
            return new List<Role>(rst);
        }

        public Role GetRoleByCode(string roleCode)
        {
            Role model = null;
            var p = new DynamicParameters();
            //获取主键
            p.Add("RoleCode", roleCode, DbType.String, null, 20);

            var conn = DbService();

            try
            {
                var rst = conn.Query<Role>(
                @"select * from CPM_Role where 	[RoleCode] = @RoleCode  And Status > 0
", p);

                model = new List<Role>(rst)[0];
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

        public List<Role> GetRoleByUser(string userId, string roleId)
        {
            string str = "select * from CPM_Role "+
                @"Where (Exists (SELECT 1 From CPM_Role_User WHERE CPM_Role_User.RoleID = CPM_Role.RoleId And CPM_Role_User.UserID = @UserID) or 
                           Exists (SELECT 1 From CPM_Position_User WHERE CPM_Position_User.RoleID = CPM_Role.RoleId And CPM_Position_User.UserID = @UserID) )" +
                "And Status > 0";
            
            if (!string.IsNullOrEmpty(roleId)) { 
                str += " AND RoleId in ('" + string.Join("','", roleId.Split(new char[]{','},  StringSplitOptions.RemoveEmptyEntries)) + "')";
            }
            var conn = DbService();
            var rst = conn.Query<Role>(str, new { UserID = userId });
            
            return new List<Role>(rst);
        }

        /// <summary>
        /// 查询所有记录，并排序(事务)
        /// </summary>
        public List<Role> GetListWithOpen(IDbConnection conn, int? top, string strWhere, string orderBy)
        {
            
            StringBuilder strSql = new StringBuilder();
            strSql.Append("Select ");
            if (top.HasValue)
            {
                strSql.Append(" top " + top.ToString());
            }
            strSql.Append(@" 	[RoleID],
	[RoleCode],
	[RoleName],
	[RoleTag],
	[Descn],
	[ParentID],
	[ParentPath],
	[ViewOrd],
	[CreateTime],
	[UpdateTime],
	[Creator],
	[Modifitor],
	[Status]
 ");
            strSql.Append(" FROM [CPM_Role] ");
           
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            if (orderBy != string.Empty)
                strSql.Append(" order by " + orderBy);

            var rst = conn.Query<Role>(strSql.ToString(), null);
         
            return new List<Role>(rst);
        }
		#endregion
	}
}