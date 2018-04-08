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
	/// Position_User 数据访问层
	/// </summary>
	partial class PositionUserDAO : Clover.Data.BaseDAO
	{
		#region 构造函数
		public PositionUserDAO()
		{
		}
		#endregion		
		
		#region 根据主键创建 Position_User 数据模型实例
		/// <summary>
		/// 根据主键创建 Position_User 数据模型实例 
		/// </summary>
		public PositionUser GetModel(int GroupID,int PositionID,string UserID)
		{           
            var p = new DynamicParameters();
            //获取主键
            p.Add("PositionID",PositionID ,DbType.Int32,null,4);
            p.Add("GroupID", GroupID, DbType.Int32, null, 4);
            p.Add("UserID",UserID ,DbType.String,null,50);
            var conn = DbService();
            
            try{
                var rst = conn.Query<PositionUser>(
                @"select * from CPM_Position_User where GroupID = @GroupID AND	[PositionID] = @PositionID
	AND [UserID] = @UserID
", p);
                
                return new List<PositionUser>(rst)[0];
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
		public  bool Update(PositionUser model)
		{
			return Update(null,model);
		}
		/// <summary>
		/// 更新记录到数据库(利用事务)
		/// </summary>
		public  bool Update(IDbConnection conn, PositionUser model)
		{		
			int affectedrows = 0;
           
            var p = new DynamicParameters();           
            p.Add("GroupID",model.GroupID ,DbType.Int32,null,4);
            p.Add("PositionID", model.PositionID, DbType.Int32, null, 4);
            p.Add("RoleID", model.RoleID, DbType.Int32, null, 4);
            p.Add("UserID",model.UserID ,DbType.String,null,50);

            string sql = @"UPDATE [CPM_Position_User] SET RoleID = @RoleID
                    WHERE
                        	[GroupID] = @GroupID
    AND [PositionID] = @PositionID
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
		public  bool Insert(PositionUser model)
		{
			return Insert(null,model);
		}
		
		/// <summary>
		/// 新增记录到数据库(事务)
		/// </summary>
		public bool Insert(IDbConnection conn,PositionUser model)
		{
            var p = new DynamicParameters();           
            p.Add("PositionID",model.PositionID ,DbType.Int32,null,4);
            p.Add("UserID",model.UserID ,DbType.String,null,50);
            p.Add("GroupID", model.GroupID, DbType.Int32, null, 4);
            p.Add("RoleID", model.RoleID, DbType.Int32, null, 4);

            string sql = @"INSERT INTO [CPM_Position_User] (PositionID,UserID,GroupID,RoleID)
                          Select @PositionID,@UserID,@GroupID,@RoleID where NOT Exists(select 1 from [CPM_Position_User] Where PositionID=@PositionID and UserID = @UserID and GroupID = @GroupID);
                      ";
            //自动补加组织
            if (model.GroupID.HasValue) {
                sql +=  @"INSERT INTO [CPM_Group_User] 
                          Select @GroupID,@UserID where NOT Exists(select 1 from [CPM_Group_User] Where GroupID=@GroupID and UserID = @UserID);                     
                      ";
            }
       
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
		public bool Delete(int PositionID,string UserID)
		{
			return Delete(null,PositionID,UserID);
		}
		
		
		/// <summary>
		/// 从数据库删除记录(事务)
		/// </summary>
		public  bool Delete(IDbConnection conn,int PositionID,string UserID)
		{
			  int affectedrows = 0;
        
            var p = new DynamicParameters();           
		     //获取主键
            p.Add("PositionID",PositionID ,DbType.Int32,null,4);
            p.Add("UserID",UserID ,DbType.String,null,50);
            
            string sql = @"DELETE FROM [CPM_Position_User]
                        WHERE 	[PositionID] = @PositionID
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
		public  List<PositionUser> GetAll(string orderBy)
		{
			return GetList(null,null, string.Empty, orderBy);
		}
        
		/// <summary>
		/// 查询所有记录，并排序(事务)
		/// </summary>
		public List<PositionUser> GetList(IDbConnection conn,int? top, string strWhere, string orderBy)
		{
            if(conn == null)
                conn = DbService();
            
            StringBuilder strSql=new StringBuilder();
			strSql.Append("Select ");
			if(top.HasValue)
			{
				strSql.Append(" top "+top.ToString());
			}
			strSql.Append(@" 	[PositionID],
	[UserID]
 ");
			strSql.Append(" FROM [CPM_Position_User] ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + orderBy);
            
            var rst = conn.Query<PositionUser>(strSql.ToString(), null);
                
            
            
			return new List<PositionUser>(rst);
		}
	
		/// <summary>
		/// 查询所有记录，并排序、分页
		/// </summary>
		public List<PositionUser> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname)
		{
            int rstcount = 0;
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc, orderfldname,out rstcount);
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页,没有排序字段
		/// </summary>
		public List<PositionUser> GetAllPaged(int PageSize,int PageIndex,string strWhere,bool desc)
		{
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc,"PositionID,UserID" );
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页(事务)
		/// </summary>
		public List<PositionUser> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname, out int rstcount)
		{
		    return Clover.Data.BaseDAO.GetList<PositionUser>("[CPM_Position_User]","PositionID,UserID",PageSize, PageIndex, strWhere ,desc, orderfldname, out rstcount);
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
            string strSQL = "select count(*) from [CPM_Position_User]";
            strSQL = string.IsNullOrEmpty(strWhere)? strSQL : strSQL + " where " + strWhere;
            var conn = DbService();
		    IEnumerable<int> rst = conn.Query<int>(strSQL, null);
            
            return new List<int>(rst)[0];            
            
		}
		#endregion
		
		#region 其他自定义方法

        /// <summary>
        /// 获取岗位的用户ID
        /// </summary>
        /// <param name="PositionId"></param>
        /// <returns></returns>
        public List<string> GetPositionUserIds(int PositionId)
        {
            var conn = DbService();
            return new List<string>(conn.Query<string>(
                @"Select UserID from [CPM_Position_User] cpu with(nolock)
                JOIN [CPM_Position] cp with(nolock) ON cpu.PositionID = cp.PositionID
                Where cpu.PositionID = @PositionId", new { PositionId = PositionId }));
           
        }

        /// <summary>
        /// 获取岗位的用户ID
        /// </summary>
        /// <param name="PositionId"></param>
        /// <returns></returns>
        public List<string> GetPositionUserIds(string PositionCode)
        {
            var conn = DbService();
            return new List<string>(conn.Query<string>(
                @"Select distinct UserID from [CPM_Position_User] cpu  with(nolock)
                JOIN [CPM_Position] cp  with(nolock) ON cpu.PositionID = cp.PositionID
                Where cp.PositionCode = @PositionCode", new { PositionCode = PositionCode }));

        }

        /// <summary>
        /// 获取岗位的用户
        /// </summary>
        /// <param name="PositionId"></param>
        /// <returns></returns>
        public List<User> GetPositionUsers(string PositionCode, bool filterValidUser)
        {
            var conn = DbService();
            string sqlcmd = string.Format(@"Select distinct UserID,{0}.{2} UserName,cp.PositionID,cp.PositionName
                from [CPM_Position_User] cpu  with(nolock)
                JOIN [CPM_Position] cp  with(nolock) ON cpu.PositionID = cp.PositionID
                JOIN {0} on {0}.{1} = cpu.UserID  
                Where cp.PositionCode = @PositionCode", 
                  Clover.Config.CPM.PermissionConfig.Config.RelativeToUserTable,
                  Clover.Config.CPM.PermissionConfig.Config.RelativeToUserKey,
                  Clover.Config.CPM.PermissionConfig.Config.RelativeToUserName);

            Common.AddUserEnabledFilter(filterValidUser, sqlcmd);

            return conn.Query<User>(sqlcmd, new { PositionCode = PositionCode }).ToList();

        }

        /// <summary>
        /// 获取部门岗位的用户ID
        /// </summary>
        /// <param name="PositionId"></param>
        /// <returns></returns>
        public List<string> GetGroupPositionUserIds(string PositionCode, string GroupID)
        {
            var conn = DbService();
            return new List<string>(conn.Query<string>(
                @"Select UserID from [CPM_Position_User] cpu with(nolock)
                JOIN [CPM_Position] cp with(nolock) ON cpu.PositionID = cp.PositionID
                JOIN [CPM_Group] cg with(nolock) ON cpu.GroupID = cg.GroupID
                Where cp.PositionCode = @PositionCode AND cpu.GroupID = @GroupID", new { PositionCode = PositionCode, GroupID = GroupID }));
        }

        public List<User> GetGroupPositionUsers(string PositionCode, string GroupID)
        {
            return GetGroupPositionUsers(PositionCode, GroupID, false);
        }

        /// <summary>
        /// 获取部门多个岗位的用户ID
        /// </summary>
        /// <param name="PositionId"></param>
        /// <returns></returns>
        public List<User> GetGroupMultiPositionUsers(string PositionCodes, string GroupID, bool filterValidUser)
        {
            var conn = DbService();

            var group = conn.Query<Group>(@"SELECT  GroupID ,
        GroupCode ,
        GroupName ,
        Descn ,
        Attribute ,
        ParentID ,
        ParentPath ,
        ViewOrd ,
        CreateTime ,
        UpdateTime ,
        Creator ,
        Modifitor ,
        Status ,
        FullName ,
        ExtString1 ,
        ExtString2 ,
        ExtString3 ,
        ExtString4 
        from CPM_Group with(nolock) where GroupID=@GroupID", new { GroupID = GroupID }).ToList();
            string grouppath = "";
            if (group.Count > 0)
                grouppath = group[0].ParentPath;
            var groups = new List<string>(grouppath.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries));
            var users = new List<User>();
            while (groups.Count > 0 && users.Count == 0)
            {
                string sqlcmd = string.Format(
                        @"Select UserID,{0}.{2} UserName,cpu.GroupID,cg.GroupName,cp.PositionID,cp.PositionName 
                from [CPM_Position_User] cpu with(nolock)
                JOIN [CPM_Position] cp with(nolock) ON cpu.PositionID = cp.PositionID
                JOIN [CPM_Group] cg with(nolock) ON cpu.GroupID = cg.GroupID
                JOIN {0} on {0}.{1} = cpu.UserID  
                Where cp.PositionCode in ('{3}') AND cpu.GroupID = @GroupID",
                        Clover.Config.CPM.PermissionConfig.Config.RelativeToUserTable,
                        Clover.Config.CPM.PermissionConfig.Config.RelativeToUserKey,
                        Clover.Config.CPM.PermissionConfig.Config.RelativeToUserName,
                        PositionCodes);

                //只允许可用用户才显示
                sqlcmd = Common.AddUserEnabledFilter(filterValidUser, sqlcmd);

                users = conn.Query<User>(sqlcmd, new {GroupID = groups[groups.Count - 1] }).ToList();

                if (groups.Count > 0)
                    groups.RemoveAt(groups.Count - 1);
            }
            return users;
        }

        /// <summary>
        /// 获取部门岗位的用户ID
        /// </summary>
        /// <param name="PositionId"></param>
        /// <returns></returns>
        public List<User> GetGroupPositionUsers(string PositionCode, string GroupID, bool filterValidUser)
        {
            var conn = DbService();

            var group = conn.Query<Group>(@"SELECT  GroupID ,
        GroupCode ,
        GroupName ,
        Descn ,
        Attribute ,
        ParentID ,
        ParentPath ,
        ViewOrd ,
        CreateTime ,
        UpdateTime ,
        Creator ,
        Modifitor ,
        Status ,
        FullName ,
        ExtString1 ,
        ExtString2 ,
        ExtString3 ,
        ExtString4 
        from CPM_Group with(nolock) where GroupID=@GroupID", new {GroupID = GroupID}).ToList();
            string grouppath = "";
            if (group.Count > 0)
                grouppath = group[0].ParentPath;
            var groups = new List<string>(grouppath.Split(new char[] {'\\'}, StringSplitOptions.RemoveEmptyEntries));
            var users = new List<User>();
            while (groups.Count > 0 && users.Count == 0)
            {
                string sqlcmd =  string.Format(
                        @"Select UserID,{0}.{2} UserName,cpu.GroupID,cg.GroupName,cp.PositionID,cp.PositionName 
                from [CPM_Position_User] cpu with(nolock)
                JOIN [CPM_Position] cp with(nolock) ON cpu.PositionID = cp.PositionID
                JOIN [CPM_Group] cg with(nolock) ON cpu.GroupID = cg.GroupID
                JOIN {0} on {0}.{1} = cpu.UserID  
                Where cp.PositionCode = @PositionCode AND cpu.GroupID = @GroupID",
                        Clover.Config.CPM.PermissionConfig.Config.RelativeToUserTable,
                        Clover.Config.CPM.PermissionConfig.Config.RelativeToUserKey,
                        Clover.Config.CPM.PermissionConfig.Config.RelativeToUserName);
                
                //只允许可用用户才显示
                sqlcmd = Common.AddUserEnabledFilter(filterValidUser, sqlcmd);

                users = conn.Query<User>(sqlcmd, new { PositionCode = PositionCode, GroupID = groups[groups.Count -1] }).ToList();

                if(groups.Count > 0)
                    groups.RemoveAt(groups.Count -1);
            }
            return users;
        }

      

        /// <summary>
        /// 检查用户是否已经存在指定的部门岗位
        /// </summary>
        /// <param name="PositionCode"></param>
        /// <param name="GroupCode"></param>
        /// <returns></returns>
        public bool CheckUserHasGroupPosition(IDbConnection conn, string UserID, string PositionId, string GroupId)
        {
            conn = conn == null ? DbService() : conn;
            return conn.Query<int>(
                @"Select top 1 1 from [CPM_Position_User] cpu
                Where cpu.UserID = @UserID AND cpu.PositionID = @PositionId AND cpu.GroupId = @GroupId", 
                new { UserID = UserID, PositionId = PositionId, GroupId = GroupId}).Any();
        }

        /// <summary>
        /// 获取部门岗位组合的用户ID
        /// </summary>
        /// <param name="PositionId"></param>
        /// <returns></returns>
        public List<string> GetCompGroupPositionUserIds(string PositionCode, string GroupCode, bool filterValidUser)
        {
            var conn = DbService();
            string sqlcmd =
                @"Select UserID from [CPM_Position_User] cpu with(nolock)
                JOIN [CPM_Position] cp with(nolock) ON cpu.PositionID = cp.PositionID
                JOIN [CPM_Group] cg with(nolock) ON cpu.GroupID = cg.GroupID
                Where cp.PositionCode = @PositionCode AND cg.GroupCode = @GroupCode";

            Common.AddUserEnabledFilter(filterValidUser, sqlcmd);

            return new List<string>(conn.Query<string>(sqlcmd,
                new { PositionCode = PositionCode, GroupCode = GroupCode }));
        }

        /// <summary>
        /// 获取部门岗位组合的用户ID
        /// </summary>
        /// <param name="PositionId"></param>
        /// <returns></returns>
        public List<User> GetCompGroupPositionUsers(string PositionCode, string GroupCode, bool filterValidUser)
        {
            var conn = DbService();
            string sqlcmd =
                String.Format(@"Select {0}.{2} UserName,cpu.UserID,cg.GroupID,cp.PositionID from [CPM_Position_User] cpu with(nolock)
                JOIN {0} with(nolock) on {0}.{1} = cpu.UserID
                JOIN [CPM_Position] cp with(nolock) ON cpu.PositionID = cp.PositionID
                JOIN [CPM_Group] cg with(nolock) ON cpu.GroupID = cg.GroupID
                Where cp.PositionCode = @PositionCode AND cg.GroupCode = @GroupCode",
                  Clover.Config.CPM.PermissionConfig.Config.RelativeToUserTable,
                  Clover.Config.CPM.PermissionConfig.Config.RelativeToUserKey,
                  Clover.Config.CPM.PermissionConfig.Config.RelativeToUserName );

            Common.AddUserEnabledFilter(filterValidUser, sqlcmd);

            return conn.Query<User>(sqlcmd,
                new { PositionCode = PositionCode, GroupCode = GroupCode }).ToList();
        }

        /// <summary>
        /// /获取直属岗位的用户
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public List<string> GetDirectPositionUserIds(string userid, string posCodeOrNameFilter)
        {
            var conn = DbService();
            return new List<string>(conn.Query<string>(
                @"   Select distinct UserID,t.PositionID,t.PositionName,t.PositionCode from [CPM_Position_User] cpu
                    join (select parentcp.PositionID,parentcp.PositionName,parentcp.PositionCode 
                    from CPM_Position cp 
					join CPM_Position parentcp on cp.ParentID = parentcp.PositionID
	                join CPM_Position_User cpu 
						on cpu.PositionID = cp.PositionID and cpu.UserId = @UserId) t
                 on cpu.PositionID = t.PositionID
                 where (PositionName like '%' + @CodeOrName + '%' or PositionCode like '%' + @CodeOrName + '%')"
                , new { UserId = userid, CodeOrName = posCodeOrNameFilter}));
        }

        /// <summary>
        /// /获取直属岗位的用户
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public List<User> GetDirectPositionUsers(string userid, string posCodeOrNameFilter, bool filterValidUser)
        {
            var conn = DbService();

            string sqlcmd = string.Format(
                @"Select distinct UserID, {0}.{2} UserName, t.PositionID,t.PositionName,t.PositionCode 
                    from [CPM_Position_User] cpu
                    join (select parentcp.PositionID,parentcp.PositionName,parentcp.PositionCode 
                    from CPM_Position cp 
					join CPM_Position parentcp on cp.ParentID = parentcp.PositionID
	                join CPM_Position_User cpu 
						on cpu.PositionID = cp.PositionID and cpu.UserId = @UserId) t
                 on cpu.PositionID = t.PositionID
                 JOIN {0} on {0}.{1} = cpu.UserID
                 where (PositionName like '%' + @CodeOrName + '%' or PositionCode like '%' + @CodeOrName + '%')"
                , Clover.Config.CPM.PermissionConfig.Config.RelativeToUserTable,
                  Clover.Config.CPM.PermissionConfig.Config.RelativeToUserKey,
                  Clover.Config.CPM.PermissionConfig.Config.RelativeToUserName);

            Common.AddUserEnabledFilter(filterValidUser, sqlcmd);

            return conn.Query<User>(sqlcmd , new { UserId = userid, CodeOrName = posCodeOrNameFilter }).ToList();
        }
        /// <summary>
        /// 获取用户的上级岗位的用户(如果没有指定岗位，那么对于身兼多岗的人可能全部发送)
        /// </summary>
        /// <param name="userid">用户的ID</param>
        /// <param name="PositionCode">指定岗位的代码</param>
        /// <returns></returns>
        public List<string> GetPositionMasterUsers(string userid, string PositionCode)
        {
            var conn = DbService();

            List<string> posuserlist = new List<string>(conn.Query<string>(
              @"SELECT UserID FROM [CPM_Position_User] a
                JOIN [CPM_Position] b ON a.PositionID = b.PositionID
            WHERE a.PositionID IN (
	            SELECT b.ParentID FROM [CPM_Position_User] a
	            JOIN [CPM_Position] b ON a.PositionID = b.PositionID
	            AND a.UserID = @UserID
             )
            AND UserID <> @UserID" + (!string.IsNullOrEmpty(PositionCode) ? " AND PositionCode='" + PositionCode + "'":"")            
            , new { UserID = userid }));

            return posuserlist;
        }

        public virtual int DeletePositionUsers(int PositionID, int? GroupId,  params string[] userids)
        {
            var conn = DbService();
            StringBuilder sb = new StringBuilder(50);
            string useridstr =  Clover.Core.Common.StringHelper.Join("','", userids);
            sb.AppendFormat("DELETE [CPM_Position_User] Where UserID in ('{0}') AND PositionID = {1} ", useridstr, PositionID);
            //如果具有部门信息ID,则连带删除
            if(GroupId.HasValue){
                sb.AppendFormat("AND GroupId={0};", GroupId);
                sb.AppendFormat(@"DELETE [CPM_Group_User] Where UserID in ('{0}') 
                    AND NOT Exists (SELECT 1 FROM [CPM_Position_User] cpu WHERE UserID in ('{0}') AND [CPM_Group_User].GroupId = cpu.GroupId);", useridstr, GroupId);
            }

            return conn.Execute(sb.ToString() , null);
        }

        public virtual int DeleteUserPositions(string UserID, params string[] Positionids)
        {
            var conn = DbService();

            return conn.Execute(
                string.Format("DELETE [CPM_Position_User] Where UserID = '{1}'  AND PositionID  in ('{0}')"
                , Clover.Core.Common.StringHelper.Join("','", Positionids)
                , UserID), null);
        }
		#endregion
	}
}