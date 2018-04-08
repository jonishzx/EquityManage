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
	/// Group_User 数据访问层
	/// </summary>
	partial class GroupUserDAO : Clover.Data.BaseDAO
	{
		#region 构造函数
		public GroupUserDAO()
		{
		}
		#endregion		
		
		#region 根据主键创建 Group_User 数据模型实例
		/// <summary>
		/// 根据主键创建 Group_User 数据模型实例 
		/// </summary>
		public GroupUser GetModel(int GroupID,string UserID)
		{           
            var p = new DynamicParameters();
            //获取主键
            p.Add("GroupID",GroupID ,DbType.Int32,null,4);
            p.Add("UserID",UserID ,DbType.String,null,50);
            
            var conn = DbService();
            
            try{
                var rst = conn.Query<GroupUser>(
                @"select * from CPM_Group_User where 	[GroupID] = @GroupID
	AND [UserID] = @UserID
", p);
                
                return new List<GroupUser>(rst)[0];
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
		public  bool Update(GroupUser model)
		{
			return Update(null,model);
		}
		/// <summary>
		/// 更新记录到数据库(利用事务)
		/// </summary>
		public  bool Update(IDbConnection conn, GroupUser model)
		{		
			int affectedrows = 0;
           
            var p = new DynamicParameters();           
            p.Add("GroupID",model.GroupID ,DbType.Int32,null,4);
            p.Add("UserID",model.UserID ,DbType.String,null,50);
            
                string sql = @"UPDATE [CPM_Group_User] SET
                    WHERE
                        	[GroupID] = @GroupID
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
		public  bool Insert(GroupUser model)
		{
			return Insert(null,model);
		}
		
		/// <summary>
		/// 新增记录到数据库(事务)
		/// </summary>
		public bool Insert(IDbTransaction tran,GroupUser model)
		{
            var p = new DynamicParameters();           
            p.Add("GroupID",model.GroupID ,DbType.Int32,null,4);
            p.Add("UserID",model.UserID ,DbType.String,null,50);
            
        
            string sql = @"INSERT INTO [CPM_Group_User] 
                          Select @GroupID,@UserID where NOT Exists(select 1 from [CPM_Group_User] Where GroupID=@GroupID and UserID = @UserID)                     
                      ";
       
            IDbConnection conn = tran == null ?  DbService() : tran.Connection;

            try{
                conn.Execute(sql, p);       
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
		public bool Delete(int GroupID,string UserID)
		{
			return Delete(null,GroupID,UserID);
		}
		
		
		/// <summary>
		/// 从数据库删除记录(事务)
		/// </summary>
		public  bool Delete(IDbConnection conn,int GroupID,string UserID)
		{
			  int affectedrows = 0;
        
            var p = new DynamicParameters();           
		     //获取主键
            p.Add("GroupID",GroupID ,DbType.Int32,null,4);
            p.Add("UserID",UserID ,DbType.String,null,50);
            
            string sql = @"DELETE FROM [CPM_Group_User]
                        WHERE 	[GroupID] = @GroupID
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
		
		#region 查询，返回自定义类			
		/// <summary>
		/// 查询所有记录，并排序
		/// </summary>
		public  List<GroupUser> GetAll(string orderBy)
		{
			return GetList(null,null, string.Empty, orderBy);
		}
        
		/// <summary>
		/// 查询所有记录，并排序(事务)
		/// </summary>
		public List<GroupUser> GetList(IDbConnection conn,int? top, string strWhere, string orderBy)
		{
            if(conn == null)
                conn = DbService();
            
            StringBuilder strSql=new StringBuilder();
			strSql.Append("Select ");
			if(top.HasValue)
			{
				strSql.Append(" top "+top.ToString());
			}
			strSql.Append(@" 	[GroupID],
	[UserID]
 ");
			strSql.Append(" FROM [CPM_Group_User] ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + orderBy);
            
            var rst = conn.Query<GroupUser>(strSql.ToString(), null);
                
            
            
			return new List<GroupUser>(rst);
		}
	
		/// <summary>
		/// 查询所有记录，并排序、分页
		/// </summary>
		public List<GroupUser> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname)
		{
            int rstcount = 0;
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc, orderfldname,out rstcount);
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页,没有排序字段
		/// </summary>
		public List<GroupUser> GetAllPaged(int PageSize,int PageIndex,string strWhere,bool desc)
		{
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc,"GroupID,UserID" );
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页(事务)
		/// </summary>
		public List<GroupUser> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname, out int rstcount)
		{
		    return Clover.Data.BaseDAO.GetList<GroupUser>("[CPM_Group_User]","GroupID,UserID",PageSize, PageIndex, strWhere ,desc, orderfldname, out rstcount);
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
            string strSQL = "select count(*) from [CPM_Group_User]";
            strSQL = string.IsNullOrEmpty(strWhere)? strSQL : strSQL + " where " + strWhere;
            var conn = DbService();
		    IEnumerable<int> rst = conn.Query<int>(strSQL, null);
            
            return new List<int>(rst)[0];            
            
		}
		#endregion
		
		#region 其他自定义方法

        /// <summary>
        /// 获取组的用户ID
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<string> GetGroupUserIds(int groupId)
        {
            var conn = DbService();
            return new List<string>(conn.Query<string>(@"
                Select UserID from [CPM_Group_User] a 
                Join CPM_Group b on a.GroupID = b.GroupID
                Where a.GroupId = @GroupId OR 
                '\\' + b.ParentPath  + '\\' like  '%\\" + groupId.ToString() + "\\%'",
            new { GroupId = groupId }));
           
        }

        /// <summary>
        /// 获取组的用户ID
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<string> GetGroupUserIds(string groupCode)
        {
            var conn = DbService();
            return new List<string>(conn.Query<string>(@"Select UserID 
            from [CPM_Group_User] cgu
            JOIN [CPM_Group] cg on cgu.GroupId = cg.GroupId
            Where cg.GroupCode = @GroupCode", new { GroupCode = groupCode }));

        }

        /// <summary>
        /// 获取组的用户
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<User> GetGroupUsers(string groupCode, bool filterValidUser)
        {
            var conn = DbService();
            string sqlcmd = string.Format(@"Select UserID,{0}.{2} UserName,cpu.GroupID,cg.GroupName 
            from [CPM_Group_User] cgu
            JOIN [CPM_Group] cg on cgu.GroupId = cg.GroupId
            JOIN {0} on {0}.{1} = cpu.UserID 
            Where cg.GroupCode = @GroupCode", 
            Clover.Config.CPM.PermissionConfig.Config.RelativeToUserTable,
            Clover.Config.CPM.PermissionConfig.Config.RelativeToUserKey,
            Clover.Config.CPM.PermissionConfig.Config.RelativeToUserName);

            Common.AddUserEnabledFilter(filterValidUser, sqlcmd);

            return conn.Query<User>(sqlcmd,new { GroupCode = groupCode }).ToList();
        }

        public virtual int DeleteGroupUsers(int GroupID, params string[] userids)
        {
            var conn = DbService();
            return conn.Execute(
                string.Format("DELETE [CPM_Group_User] Where UserID in ('{0}') AND GroupID = {1}"
                , Clover.Core.Common.StringHelper.Join("','", userids)
                , GroupID), null);
        }

        public virtual int DeleteUserGroups(string UserID, params string[] Groupids)
        {
            var conn = DbService();

            return conn.Execute(
                string.Format("DELETE [CPM_Group_User] Where UserID = '{1}'  AND GroupID  in ('{0}')"
                , Clover.Core.Common.StringHelper.Join("','", Groupids)
                , UserID), null);
        }


        public virtual int DeleteUserGroups(string UserID)
        {
            var conn = DbService();

            return conn.Execute(
                string.Format("DELETE [CPM_Group_User] Where UserID = '{0}'",UserID), null);
        }
		#endregion
	}
}