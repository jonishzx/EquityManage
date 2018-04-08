namespace Clover.Permission.DAO
{
	using System;
    using System.Collections.Generic;
	using System.Data;
	using System.Linq;
	using System.Data.SqlClient;
    using System.Text;
	using Dapper;
 
	using Clover.Permission.Model;
    
	/// <summary>
	/// Role_User 数据访问层
	/// </summary>
	public partial class RoleUserDAO : Clover.Data.BaseDAO
	{
		#region 构造函数
		public RoleUserDAO()
		{
		}
		#endregion		
		
		#region 根据主键创建 Role_User 数据模型实例
		/// <summary>
		/// 根据主键创建 Role_User 数据模型实例 
		/// </summary>
		public RoleUser GetModel(int RoleID,string UserID)
		{           
            var p = new DynamicParameters();
            //获取主键
            p.Add("RoleID",RoleID ,DbType.Int32,null,4);
            p.Add("UserID",UserID ,DbType.String,null,50);
            
            var conn = DbService();
            
            try{
                var rst = conn.Query<RoleUser>(
                @"select * from CPM_Role_User where 	[RoleID] = @RoleID
	AND [UserID] = @UserID
", p);
                
                return new List<RoleUser>(rst)[0];
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
		public  bool Update(RoleUser model)
		{
			return Update(null,model);
		}
		/// <summary>
		/// 更新记录到数据库(利用事务)
		/// </summary>
		public  bool Update(IDbConnection conn, RoleUser model)
		{		
			int affectedrows = 0;
           
            var p = new DynamicParameters();           
            p.Add("RoleID",model.RoleID ,DbType.Int32,null,4);
            p.Add("UserID",model.UserID ,DbType.String,null,50);
            
                string sql = @"UPDATE [CPM_Role_User] SET
                    WHERE
                        	[RoleID] = @RoleID
	AND [UserID] = @UserID
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
		public  bool Insert(RoleUser model)
		{
			return Insert(null,model);
		}
		
		/// <summary>
		/// 新增记录到数据库(事务)
		/// </summary>
		public bool Insert(IDbTransaction tran,RoleUser model)
		{
            var p = new DynamicParameters();           
            p.Add("RoleID",model.RoleID ,DbType.Int32,null,4);
            p.Add("UserID",model.UserID ,DbType.String,null,50);
            
        
            string sql = @"INSERT INTO [CPM_Role_User]
                            Select @RoleID,@UserID where NOT Exists(select 1 from [CPM_Role_User] Where RoleID=@RoleID and UserID = @UserID)";


            IDbConnection conn = tran == null ? DbService() : tran.Connection;

            try{
               conn.Execute(sql, p, tran, null,CommandType.Text);                              
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
		public bool Delete(int RoleID,string UserID)
		{
			return Delete(null,RoleID,UserID);
		}
		
		
		/// <summary>
		/// 从数据库删除记录(事务)
		/// </summary>
		public  bool Delete(IDbTransaction tran,int RoleID,string UserID)
		{
			  int affectedrows = 0;
        
            var p = new DynamicParameters();           
		     //获取主键
            p.Add("RoleID",RoleID ,DbType.Int32,null,4);
            p.Add("UserID",UserID ,DbType.String,null,50);
            
            string sql = @"DELETE FROM [CPM_Role_User]
                        WHERE 	[RoleID] = @RoleID
	AND [UserID] = @UserID
";

            IDbConnection conn = tran == null ? DbService() : tran.Connection;


            try{
                affectedrows = conn.Execute(sql, p, tran, null,CommandType.Text);
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
		public  List<RoleUser> GetAll(string orderBy)
		{
			return GetList(null,null, string.Empty, orderBy);
		}
        
		/// <summary>
		/// 查询所有记录，并排序(事务)
		/// </summary>
		public List<RoleUser> GetList(IDbConnection conn,int? top, string strWhere, string orderBy)
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
	[UserID]
 ");
			strSql.Append(" FROM [CPM_Role_User] ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + orderBy);
            
            var rst = conn.Query<RoleUser>(strSql.ToString(), null);
                
            
            
			return new List<RoleUser>(rst);
		}
	
		/// <summary>
		/// 查询所有记录，并排序、分页
		/// </summary>
		public List<RoleUser> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname)
		{
            int rstcount = 0;
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc, orderfldname,out rstcount);
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页,没有排序字段
		/// </summary>
		public List<RoleUser> GetAllPaged(int PageSize,int PageIndex,string strWhere,bool desc)
		{
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc,"RoleID,UserID" );
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页(事务)
		/// </summary>
		public List<RoleUser> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname, out int rstcount)
		{
		    return Clover.Data.BaseDAO.GetList<RoleUser>("[CPM_Role_User]","RoleID,UserID",PageSize, PageIndex, strWhere ,desc, orderfldname, out rstcount);
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
            string strSQL = "select count(*) from [CPM_Role_User]";
            strSQL = string.IsNullOrEmpty(strWhere)? strSQL : strSQL + " where " + strWhere;
            var conn = DbService();
		    IEnumerable<int> rst = conn.Query<int>(strSQL, null);
            
            return new List<int>(rst)[0];            
            
		}
		#endregion
		
		#region 其他自定义方法
        public virtual int DeleteRoleUsers(int RoleID, params string[] userids)
        {
            var conn = DbService();
            return conn.Execute(
                string.Format("DELETE [CPM_Role_User] Where UserID in ('{0}') AND RoleID = {1}"
                ,Clover.Core.Common.StringHelper.Join("','",userids)
                ,RoleID), null);          
        }

        public virtual int DeleteUserRoles(string UserID, params string[] roleids)
        {
            var conn = DbService();

            return conn.Execute(
                string.Format("DELETE [CPM_Role_User] Where UserID = '{1}'  AND RoleID  in ('{0}')"
                , Clover.Core.Common.StringHelper.Join("','", roleids)
                , UserID), null);
        }

        public virtual int DeleteUserRoles(string UserID)
        {
            var conn = DbService();

            return conn.Execute(
                string.Format("DELETE [CPM_Role_User] Where UserID = '{0}'", UserID), null);
        }

          /// <summary>
        /// 获取组的用户ID
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<string> GetRoleUserIds(int roleId)
        {
            var conn = DbService();
            return new List<string>(conn.Query<string>("Select UserID from [CPM_Role_User] Where RoleID = @RoleID", new { RoleID = roleId }));
        
        }

        /// <summary>
        /// 获取组的用户ID
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<string> GetRoleUserIds(string roleCode)
        {
            var conn = DbService();
            return new List<string>(conn.Query<string>(@"Select UserID 
                from [CPM_Role_User] CRU 
                JOIN [CPM_Role] CR on CRU.RoleId = CR.RoleId
                Where CR.RoleCode = @RoleCode", new { RoleCode = roleCode }));

        }

        public List<User> GetRoleUsers(string roleCode)
        {
            return GetRoleUsers(roleCode, false);
        }

        /// <summary>
        /// 获取组的用户ID
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<User> GetRoleUsers(string roleCode, bool filterValidUser)
        {
            var conn = DbService();

            string sqlcmd = string.Format(@"Select UserID,{0}.{2} UserName 
                from [CPM_Role_User] CRU 
                JOIN [CPM_Role] CR on CRU.RoleId = CR.RoleId
                JOIN {0} on {0}.{1} = CRU.UserID
                Where CR.RoleCode = @RoleCode",
                  Clover.Config.CPM.PermissionConfig.Config.RelativeToUserTable,
                  Clover.Config.CPM.PermissionConfig.Config.RelativeToUserKey,
                  Clover.Config.CPM.PermissionConfig.Config.RelativeToUserName);

            //只允许可用用户才显示
            Common.AddUserEnabledFilter(filterValidUser, sqlcmd);

            return conn.Query<User>(sqlcmd, new { RoleCode = roleCode }).ToList();

        }
		#endregion
	}
}