using System.Linq;
using Clover.Core.Domain;

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

    using Clover.Core.Collection;
    using Clover.Permission.DAO;
    using Clover.Permission.Model;
    using System.Transactions;
    
	/// <summary>
	/// Admin 数据访问层
	/// </summary>
	public partial class AdminDAO : Clover.Data.BaseDAO
	{
        public static readonly string DefaultGroupTag = "DefaultGroup";
        public static readonly string DefaultGroupPositionTag = "DefaultPosition";
        public static readonly string DefaultRoleTag = "DefaultRole";

		#region 构造函数
		public AdminDAO()
		{
		}
		#endregion		
		
		#region 根据主键创建 Admin 数据模型实例
		/// <summary>
		/// 根据主键创建 Admin 数据模型实例 
		/// </summary>
		public Admin GetModel(string AdminId)
		{           
            //Admin model = null;
            //var p = new DynamicParameters();
            ////获取主键
            //p.Add("AdminId",AdminId ,DbType.String,null,50);
            
            //var conn = DbService();
            
            //try{
            //    var rst = conn.Query<Admin>(
            //    @"select * from sys_Admin where 	[AdminId] = @AdminId", p);
                
            //    List<Admin> lrst 
            //        = new  List<Admin>(rst);
                
            //    if(lrst.Count > 0)
            //        model = lrst[0];
            //}
            //catch(DataException ex){
            //    throw ex;
            //}finally{
                
            //}
		
            //return model;

            return GetViewModel(AdminId);
		}
		#endregion
			
		#region 更新记录
		/// <summary>
		/// 更新记录到数据库
		/// </summary>
		public  bool Update(Admin model)
		{
			return Update(null,model);
		}
		/// <summary>
		/// 更新记录到数据库(利用事务)
		/// </summary>
		public  bool Update(IDbTransaction tran, Admin model)
		{		
			int affectedrows = 0;
           
            var p = new DynamicParameters();           
            p.Add("AdminId",model.AdminId ,DbType.String,null,50);
            p.Add("AdminName",model.AdminName ,DbType.String,null,50);
            p.Add("LoginName",model.LoginName ,DbType.String,null,50);
            p.Add("Password",model.Password ,DbType.String,null,32);
            p.Add("Email",model.Email ,DbType.String,null,50);
            p.Add("IP",model.IP ,DbType.String,null,15);
            p.Add("LanguageFile",model.LanguageFile ,DbType.String,null,50);
            p.Add("Status",model.Status ,DbType.Int32,null,4);
            p.Add("Descn",model.Descn ,DbType.String,null,100);
            p.Add("DeptCode", model.DeptCode, DbType.String, null, 36);
            p.Add("EmpType", model.EmpType, DbType.String, null, 15);
            p.Add("Postion", model.Postion, DbType.String, null, 36);
            p.Add("BudgetDeptCode", model.BudgetDeptCode, DbType.String, null, 36);
            p.Add("UsedDeptCode", model.UsedDeptCode, DbType.String, null, 36);
            p.Add("MobilePhone", model.MobilePhone, DbType.String, null, 50);
            p.Add("EmailPwd", model.EmailPwd, DbType.String, null, 64);
            p.Add("MappingAccount", model.MappingAccount, DbType.String, null, 100);
            p.Add("Nation", model.Nation, DbType.String, null, 36);
                string sql = @"UPDATE [sys_Admin] SET
	                [AdminName] = @AdminName,
	                [LoginName] = @LoginName,
	                [Password] = @Password,
	                [Email] = @Email,
	                [IP] = @IP,
	                [LanguageFile] = @LanguageFile,
	                [Status] = @Status,
	                [Descn] = @Descn,
                    [DeptCode] = @DeptCode,
	                [EmpType] = @EmpType,
	                [Postion] = @Postion,
	                [BudgetDeptCode] = @BudgetDeptCode,
	                [UsedDeptCode] = @UsedDeptCode,
	                [MobilePhone] = @MobilePhone,
                    [EmailPwd] = @EmailPwd,
                    [MappingAccount] = @MappingAccount,[Nation]=@Nation
                                    WHERE
                        	                [AdminId] = @AdminId";
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
		
		#region 新增记录
		/// <summary>
		/// 新增记录到数据库
		/// </summary>
		public  bool Insert(Admin model)
		{
			return Insert(null,model);
		}
		
		/// <summary>
		/// 新增记录到数据库(事务)
		/// </summary>
		public bool Insert(IDbTransaction tran, Admin model)
		{
            var p = new DynamicParameters();           
            p.Add("AdminId",model.AdminId ,DbType.String,null,50);
            p.Add("AdminName",model.AdminName ,DbType.String,null,50);
            p.Add("LoginName",model.LoginName ,DbType.String,null,50);
            p.Add("Password",model.Password ,DbType.String,null,32);
            p.Add("Email",model.Email ,DbType.String,null,50);
            p.Add("IP",model.IP ,DbType.String,null,15);
            p.Add("LanguageFile",model.LanguageFile ,DbType.String,null,50);
            p.Add("Status",model.Status ,DbType.Int32,null,4);
            p.Add("IsActived",model.IsActived ,DbType.Boolean,null,1);
            p.Add("Descn",model.Descn ,DbType.String,null,100);
            p.Add("DeptCode", model.DeptCode, DbType.String, null, 36);
            p.Add("EmpType", model.EmpType, DbType.String, null, 15);
            p.Add("Postion", model.Postion, DbType.String, null, 36);
            p.Add("BudgetDeptCode", model.BudgetDeptCode, DbType.String, null, 36);
            p.Add("UsedDeptCode", model.UsedDeptCode, DbType.String, null, 36);
            p.Add("MobilePhone", model.MobilePhone, DbType.String, null, 50);
            p.Add("EmailPwd", model.EmailPwd, DbType.String, null, 64);
            p.Add("MappingAccount", model.MappingAccount, DbType.String, null, 100);
            p.Add("PasswordLastUpdateTime", model.PasswordLastUpdateTime, DbType.DateTime, null, 8);
            p.Add("Nation", model.Nation, DbType.String, null, 36);
        
            string sql = @"INSERT INTO [sys_Admin] (
               [AdminId],          
	            [AdminName],
	            [LoginName],
	            [Password],
	            [Email],
	            [IP],
	            [LanguageFile],
	            [Status],
	            [Descn],
                [DeptCode],
	            [EmpType],
	            [Postion],
	            [BudgetDeptCode],
	            [UsedDeptCode],
	            [MobilePhone],
                [EmailPwd],
                [MappingAccount],
                [PasswordLastUpdateTime],[Nation]
                                    ) VALUES (
                @AdminId,
                @AdminName,
	            @LoginName,
	            @Password,
	            @Email,
	            @IP,
	            @LanguageFile,
	            @Status,
	            @Descn,
                @DeptCode,
	            @EmpType,
	            @Postion,
	            @BudgetDeptCode,
	            @UsedDeptCode,
	            @MobilePhone,
                @EmailPwd,
                @MappingAccount,
                getdate(),@Nation)";
        
            
            IDbConnection conn = tran == null ?  DbService() : tran.Connection;

            try{
                if (tran == null)
                    conn.Execute(sql, p);
                else
                    conn.Execute(sql, p, tran, null, null);
                             
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
		public bool Delete(string AdminId)
		{
			return Delete(null,AdminId);
		}
		
		
		/// <summary>
		/// 从数据库删除记录(事务)
		/// </summary>
		public  bool Delete(IDbTransaction tran, string AdminId)
		{
			int affectedrows = 0;
        
            var p = new DynamicParameters();           
		     //获取主键
            p.Add("AdminId",AdminId ,DbType.String,null,50);
            
            string sql = @"DELETE FROM [sys_Admin]
                        WHERE 	[AdminId] = @AdminId
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
		
		#region 查询，返回自定义类			
		/// <summary>
		/// 查询所有记录，并排序
		/// </summary>
		public  List<Admin> GetAll(string orderBy)
		{
			return GetList(null,null, string.Empty, orderBy);
		}
        
		/// <summary>
		/// 查询所有记录，并排序(事务)
		/// </summary>
		public List<Admin> GetList(IDbConnection conn,int? top, string strWhere, string orderBy)
		{
            if(conn == null)
                conn = DbService();
            
            StringBuilder strSql=new StringBuilder();
			strSql.Append("Select ");
			if(top.HasValue)
			{
				strSql.Append(" top "+top.ToString());
			}
			strSql.Append(@" 	[AdminId],
	            [AdminName],
	            [LoginName],
	            [Password],
	            [Email],
	            [Joined],
	            [LastVisit],
	            [IP],
	            [LanguageFile],
	            [Status],
	            [IsActived],
	            [Descn],
                [DeptCode],
	            [EmpType],
	            [Postion],
	            [BudgetDeptCode],
	            [UsedDeptCode],
	            [MobilePhone],
                [EmailPwd],
                [MappingAccount],
                [PasswordLastUpdateTime]
             ");
			strSql.Append(" FROM [sys_Admin] ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
           
            if (orderBy != string.Empty)
                strSql.Append(" order by " + orderBy);
			
            
            var rst = conn.Query<Admin>(strSql.ToString(), null);
                
            
            
			return new List<Admin>(rst);
		}
	
		/// <summary>
		/// 查询所有记录，并排序、分页
		/// </summary>
		public List<Admin> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname)
		{
            int rstcount = 0;
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc, orderfldname,out rstcount);
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页,没有排序字段
		/// </summary>
		public List<Admin> GetAllPaged(int PageSize,int PageIndex,string strWhere,bool desc)
		{
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc,"" );
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页(事务)
		/// </summary>
		public List<Admin> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname, out int rstcount)
		{
            return Clover.Data.BaseDAO.GetList<Admin>("[v_sys_admin]", "AdminId", PageSize, PageIndex, strWhere, desc, orderfldname, out rstcount);
		}
       
        /// <summary>
        /// 查询所有记录，并排序、分页,没有排序字段
        /// </summary>
        public List<Admin> GetAllPaged(int PageSize, int PageIndex, string strWhere, bool desc, out int rstcount)
        {
            return GetAllPaged(null, PageSize, PageIndex, strWhere, desc, "", out rstcount);
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
            string strSQL = "select count(*) from [sys_Admin]";
            strSQL = string.IsNullOrEmpty(strWhere)? strSQL : strSQL + " where " + strWhere;
            var conn = DbService();
		    IEnumerable<int> rst = conn.Query<int>(strSQL, null);
            
            return new List<int>(rst)[0];            
            
		}
		#endregion
		
		#region 其他自定义方法
       
        /// <summary>
        /// 删除操作员信息
        /// </summary>
        /// <param name="userId"></param>
        public void DeleteAdmin(string userId)
        {
            CommandUserDAO permissiondao = new CommandUserDAO();
            using (var ts = new TransactionScope(System.Transactions.TransactionScopeOption.RequiresNew))
            {
                using (var conn = ManualDbService())
                {
                    IDbTransaction tran = null;
                    try
                    {
                        tran = conn.BeginTransaction();

                        //删除用户权限角色相关信息
                        permissiondao.DeleteUserPermission(tran, userId);

                        //删除用户信息
                        Delete(tran, userId);

                        tran.Commit();
                        ts.Complete();

                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        Clover.Data.BaseDAO.CloseWithDispose(conn);
                    }
                }
            }
        }

        /// <summary>
        /// 获取没有被组引用的操作员
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Admin> GetAdminNoGroupRef(int page, int pageindex, int groupId, string where, out int resultcount)
        {
            GroupUserDAO groupuser = new GroupUserDAO();
        
            string strWhere = string.Format("AdminId NOT IN ('{0}') ",Clover.Core.Common.StringHelper.Join("','",groupuser.GetGroupUserIds(groupId).ToArray()));

            if (!string.IsNullOrEmpty(where))
            {
                strWhere += " And " + where;
            }

            return GetAllPaged(page, pageindex, strWhere, true, out resultcount);
        }

        /// <summary>
        /// 获取没有被组引用的操作员
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Admin> GetAdminNoPositionRef(int page, int pageindex, int PositionId, string where, out int resultcount)
        {
            PositionUserDAO Positionuser = new PositionUserDAO();

            string strWhere = string.Format("AdminId NOT IN ('{0}') ", Clover.Core.Common.StringHelper.Join("','", Positionuser.GetPositionUserIds(PositionId).ToArray()));

            if (!string.IsNullOrEmpty(where))
            {
                strWhere += " And " + where;
            }

            return GetAllPaged(page, pageindex, strWhere, true, out resultcount);
        }


        /// <summary>
        /// 获取没有被角色引用的操作员
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Admin> GetNotJoinUserByRole(int page, int pageindex, int roleId, string where, out int resultcount)
        {
            RoleUserDAO roleuser = new RoleUserDAO();
        
            string strWhere = string.Format("AdminId NOT IN ('{0}') ",Clover.Core.Common.StringHelper.Join("','",roleuser.GetRoleUserIds(roleId).ToArray()));

            if(!string.IsNullOrEmpty(where))
            {
                strWhere += " And " + where;
            }

            return GetAllPaged(page, pageindex, strWhere, true, out resultcount);
        }

        /// <summary>
        /// 通过映射账号查找用户信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public Admin GetAdminByMappingAccount(string account)
        {
            var p = new DynamicParameters();
            //获取主键
            p.Add("Account", account, DbType.String, null, 50);

            string sql = @"Select LoginName FROM [sys_Admin]
                        WHERE ([MappingAccount] = @Account)";

            IDbConnection conn = DbService();

            try
            {
                var rst = conn.Query<string>(sql, p).ToList();

                if (rst.Count > 0)
                    return GetFullInfoByLoginName(rst[0]);
            }
            catch (DataException ex)
            {
                throw ex;
            }

            return null;
        }

        public Admin GetAdminByLoginName(string logname)
        {
            var p = new DynamicParameters();           
		     //获取主键
            p.Add("LoginName",logname ,DbType.String,null,50);
            
            string sql = @"Select * FROM [sys_Admin]
                        WHERE 	[LoginName] = @LoginName";

            IDbConnection conn = DbService();

            try{
                var rst = conn.Query<Admin>(sql, p);
                
                List<Admin> lrst 
                    = new List<Admin>(rst);
                
                if(lrst.Count > 0)
                    return lrst[0];
            }
            catch(DataException ex){
                throw ex;
            }

			return null;
        }

        public List<Admin> GetAdminByGroup(int page, int pageindex, int groupId, out int resultcount)
        {
            if (groupId > 0)
            {
                string subwhere = string.Format(@"
                Select UserID from [CPM_Group_User] a 
                Join CPM_Group b on a.GroupID = b.GroupID
                Where a.GroupId = {0} OR 
                '\\' + b.ParentPath  + '\\' like  '%\\{0}\\%'", groupId);

                string strWhere = string.Format("AdminID IN ({0}) ", subwhere);
                return GetAllPaged(page, pageindex, strWhere, true, out resultcount);
            }
            else
            {
                resultcount = 0;
                return null;
            }
        }

        public List<Admin> GetAdminByPosition(int page, int pageindex, int PositionId, out int resultcount)
        {
            if (PositionId > 0)
            {
                string subwhere = @"Select UserID from [CPM_Position_User] cpu  with(nolock)
                JOIN [CPM_Position] cp  with(nolock) ON cpu.PositionID = cp.PositionID
                Where cpu.PositionID = " + PositionId.ToString();

                string strWhere = string.Format("AdminID IN ({0}) ", subwhere);
                return GetAllPaged(page, pageindex, strWhere, true, out resultcount);
            }
            else
            {
                resultcount = 0;
                return null;
            }
        }

        public List<Admin> GetAdminByRole(int page, int pageindex, int roleId, out int resultcount)
        {
            if (roleId > 0)
            {
                string strWhere = string.Format("AdminID IN (Select UserID from [CPM_Role_User] Where RoleID = '{0}')",
                    roleId);
                return GetAllPaged(page, pageindex, strWhere, true, out resultcount);
            }
            else
            {
                resultcount = 0;
                return null;
            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdatePassword(string adminid, string password)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update sys_Admin set ");
            strSql.Append("Password=@Password ");
            strSql.Append(",PasswordLastUpdateTime=getdate()");
            strSql.Append(" where AdminID=@AdminID ");

            DynamicParameters p = new DynamicParameters();
            p.Add("AdminID" , adminid,null, null, null);
            p.Add("Password", password, null, null, null);

            var conn = DbService();

            conn.Execute(strSql.ToString(), p);
            
        }


        /// <summary>
        /// 检查是否存在相同的ID
        /// </summary>
        public bool CheckExistsSameID(string loginname, string id)
        {
            return ExistsSameAttr("sys_Admin", "LoginName", loginname, "Status>=0", "Adminid", id);            
        }


        /// <summary>
        /// 检查是否存在相同的ID
        /// </summary>
        public bool CheckIsExistsSameMappingAccount(string mappingAccount, string adminid, out Admin admin)
        {
            admin = null;

            if (string.IsNullOrEmpty(mappingAccount))
                return false;

            string strWhere = "'" + string.Join("','", 
                mappingAccount.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) + "'";

            List<Admin> list = null;
            var conn = DbService();
            var p = new DynamicParameters();
            //获取主键
            p.Add("MappingAccount", mappingAccount, DbType.String, null, 100);
            if (!string.IsNullOrEmpty(adminid))
            {
                p.Add("adminid", adminid, DbType.String, null, 50);

                list = conn.Query<Admin>(
                 @"select AdminName,AdminId,LoginName from sys_Admin where [MappingAccount] = @MappingAccount and AdminId <> @adminid ;", p).ToList();
               
            }
            else {
                list = conn.Query<Admin>(
                 @"select AdminName,AdminId,LoginName from sys_Admin where [MappingAccount] = @MappingAccount;", p).ToList();
            }

            if (list.Count > 0)
                admin = list[0];

            return list.Count > 0;
        }

        /// <summary>
        /// 检查是否存在相同的ID
        /// </summary>
        public List<Admin> GetEnabledAdmin()
        {
            int resultcount = 0;
            string strWhere = string.Format("Status = 1");
            return GetAllPaged(int.MaxValue, 1, strWhere, true, out resultcount);
        }

        public Admin GetSimpleByLoginName(string LoginName)
        {
            Admin model = null;
            var p = new DynamicParameters();
            //获取主键
            p.Add("LoginName", LoginName, DbType.String, null, 50);

            var conn = DbService();

            try
            {
                var rst = conn.Query<Admin>(
                @"select AdminId,Password,AdminName from sys_Admin where [LoginName] = @LoginName and Status = 1 ;", p);

                List<Admin> lrst
                    = new List<Admin>(rst);

                if (lrst.Count > 0)
                    model = lrst[0];
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
        /// <summary>
        /// 获取完整的用户信息
        /// </summary>
        /// <param name="LoginName"></param>
        /// <returns></returns>
        public Admin GetFullInfoByLoginName(string LoginName)
        {
            Admin model = null;
            var p = new DynamicParameters();
            //获取主键
            p.Add("LoginName", LoginName, DbType.String, null, 50);
            var sql = @"SELECT  t.AdminId ,
        t.LoginName ,
        t.AdminName ,
        t.Password ,
        t.Email ,
        t.Joined ,
        t.LastVisit ,
        t.IP ,
        t.LanguageFile ,
        t.Status ,
        t.IsActived ,
        t.Descn ,
        t.DeptCode ,
        t.EmpType ,
        t.Postion ,
        t.MobilePhone ,
        t.BudgetDeptCode ,
        t.UsedDeptCode ,
        t.EmailPwd ,
        t.MappingAccount ,
        t.PasswordLastUpdateTime ,
        t.IsSeniorManager ,
        t.CurrGroupId ,
        t.Nation ,
        t.CurrPositionId ,
        cg.GroupName CurrGroupName ,
        cp.PositionName CurrPositionName,
        t.CurrRoleId,
        cr.RoleName CurrRoleName
FROM    ( SELECT TOP 1
                    sa.AdminId ,
                    sa.AdminName ,
                    sa.LoginName ,
                    sa.Password ,
                    sa.Email ,
                    sa.Joined ,
                    sa.LastVisit ,
                    sa.IP ,
                    sa.LanguageFile ,
                    sa.Status ,
                    sa.IsActived ,
                    sa.Descn ,
                    sa.DeptCode ,
                    sa.EmpType ,
                    sa.Postion ,
                    sa.MobilePhone ,
                    sa.BudgetDeptCode ,
                    sa.UsedDeptCode ,
                    sa.EmailPwd ,
                    sa.MappingAccount ,
                    sa.PasswordLastUpdateTime ,
                    sa.IsSeniorManager ,
                    sa.Nation ,
                   ( SELECT TOP 1  ConfigValue
                             FROM   sys_UserConfig suc WITH ( NOLOCK )
                             WHERE  suc.UserId = sa.AdminId
                                    AND ConfigType = 'DefaultRole'
                           ) CurrRoleId ,
                    ISNULL(( SELECT TOP 1
                                    ConfigValue
                             FROM   sys_UserConfig suc WITH ( NOLOCK )
                             WHERE  suc.UserId = sa.AdminId
                                    AND ConfigType = 'DefaultGroup'
                           ), ( SELECT TOP 1
                                        GroupID
                                FROM    CPM_Group_User cgu
                                WHERE   sa.AdminId = cgu.UserID
                              )) CurrGroupId ,
                    ISNULL(( SELECT TOP 1
                                    ConfigValue
                             FROM   sys_UserConfig suc WITH ( NOLOCK )
                             WHERE  suc.UserId = sa.AdminId
                                    AND ConfigType = 'DefaultPosition'
                           ), ( SELECT TOP 1
                                        PositionID
                                FROM    CPM_Position_User cgu WITH ( NOLOCK )
                                WHERE   sa.AdminId = cgu.UserID
                              )) CurrPositionId
          FROM      sys_Admin sa WITH ( NOLOCK )
          WHERE     [LoginName] = @LoginName
        ) t
        LEFT JOIN CPM_Group cg WITH ( NOLOCK ) ON t.CurrGroupId = cg.GroupID
        LEFT JOIN CPM_Position cp WITH ( NOLOCK ) ON t.CurrPositionId = cp.PositionID
        LEFT JOIN CPM_Role cr  WITH ( NOLOCK ) ON t.CurrRoleId = cr.RoleID;         
SELECT  cr.RoleId ,
        cr.RoleName
FROM    CPM_Role cr WITH ( NOLOCK )
        JOIN CPM_Role_User cru WITH ( NOLOCK ) ON cru.RoleId = cr.RoleId
        JOIN sys_Admin sa ON cru.UserID = sa.AdminId
WHERE   cr.Status = 1
        AND sa.LoginName = @LoginName";

            var conn = DbService();

            try
            {
                using (var multi = conn.QueryMultiple(sql, p, null, null, CommandType.Text))
                {
                    var rdobj = multi.Read<Admin>().ToList();                    
                    if (rdobj.Any())
                    {
                        model = rdobj.First();
                        model.Roles = multi.Read<Role>().ToArray();
                    }
                    else {
                        throw new NullReferenceException("无法通过该账号查找出用户信息");
                    }
                }
            }
            catch (DataException ex)
            {
                throw ex;
            }

            return model;
        }


        public Admin GetFullInfoByAdminId(string AdminId)
        {
            return GetFullInfoByAdminId(AdminId, true);
        }
        /// <summary>
        /// 获取完整的用户信息
        /// </summary>
        /// <param name="LoginName"></param>
        /// <returns></returns>
        public Admin GetFullInfoByAdminId(string AdminId, bool withRoles)
        {
            Admin model = null;
            var p = new DynamicParameters();
            //获取主键
            p.Add("AdminId", AdminId, DbType.String, null, 50);

            var conn = DbService();

            string sql = @"select t.*,cg.GroupName CurrGroupName,cp.PositionName CurrPositionName from (
                    select top 1 sa.*,
                    isnull((select top 1 ConfigValue from sys_UserConfig suc with(nolock)
                    where suc.UserId = sa.AdminId and ConfigType ='DefaultGroup'), 
                    (select top 1 GroupID from CPM_Group_User cgu with(nolock) where sa.AdminId = cgu.UserID))
                     CurrGroupId,
                    isnull((select top 1 ConfigValue from sys_UserConfig suc with(nolock)
                    where suc.UserId = sa.AdminId and ConfigType ='DefaultPosition'), 
                    (select top 1 PositionID from CPM_Position_User cgu with(nolock) where sa.AdminId = cgu.UserID))
                     CurrPositionId
                    from sys_Admin sa 
                    where sa.[AdminId] = @AdminId) t
                    left join CPM_Group cg with(nolock) on t.CurrGroupId = cg.GroupID
                    left join CPM_Position cp with(nolock) on t.CurrPositionId = cp.PositionID;";

            if (withRoles)
                sql += @"select cr.RoleId,cr.RoleName from CPM_Role cr with(nolock) join CPM_Role_User cru with(nolock) on cru.RoleId = cr.RoleId join sys_Admin sa with(nolock) on cru.UserID = sa.AdminId
                    where cr.Status = 1 and sa.AdminId = @AdminId";

            try
            {
                var multi = conn.QueryMultiple(sql, p, null, null, CommandType.Text);

                model = multi.Read<Admin>().First();
                if(withRoles){
                    model.Roles = multi.Read<Role>().ToArray();
                }
            }
            catch (DataException ex)
            {
                throw ex;
            }

            return model;
        }

        public Admin GetByLoginName(string LoginName)
        {
            Admin model = null;
            var p = new DynamicParameters();
            //获取主键
            p.Add("LoginName", LoginName, DbType.String, null, 50);

            var conn = DbService();

            try
            {
                var rst = conn.Query<Admin>(
                @"select * from sys_Admin where [LoginName] = @LoginName;", p);

                List<Admin> lrst
                    = new List<Admin>(rst);

                if (lrst.Count > 0)
                    model = lrst[0];
            }
            catch (DataException ex)
            {
                throw ex;
            }
           
            return model;
        }

        public string GetAdminName(string adminId)
        {
            var p = new DynamicParameters();
            //获取主键
            p.Add("AdminId", adminId, DbType.String, null, 50);
            
            var conn = DbService();
            var rst = conn.Query<string>(
            @"select * from sys_Admin where [AdminId] = @AdminId", p);

            List<string> lrst
                = new List<string>(rst);

            if (lrst.Count > 0)
                return lrst[0];
            else
                return "";
        }

        /// <summary>
        /// 查询所有记录，并排序、分页(事务)
        /// </summary>
        public List<Admin> GetUserByLogin(string Adminid, int PageSize, int PageIndex, string sWhere,out int rstcount)
        {

            var p = new DynamicParameters();
            p.Add("Adminid", Adminid, null, null, null);
            p.Add("PageSize", PageSize, null, null, null);
            p.Add("PageIndex", PageIndex, null, null, null);
            p.Add("sWhere", sWhere, null, null, null);
            p.Add("@ResultCount", 0, DbType.Int32, ParameterDirection.Output, null);

            List<Admin> rst = null;
            IDbConnection cnn = null;

            try
            {

                cnn = DbService();
                rst = new List<Admin>(cnn.Query<Admin>("PROC_GetPriUser", p, null, true, CommandTimeout, CommandType.StoredProcedure));
                rstcount = p.Get<int>("ResultCount");
            }
            catch (DataException dex)
            {
                throw dex;
            }
            finally
            {
                Close();
            }

            return rst;

        }

        /// <summary>
        /// 获取用户所具有的兼职
        /// </summary>
        /// <param name="adminid"></param>
        /// <returns></returns>
        public DataTable GetUserGroupPosition(string adminid)
        {
            /*
            string sql = @"
SELECT DISTINCT cg.GroupId,cg.GroupCode,cg.GroupName,
cp.PositionId,cp.PositionCode,cp.PositionName,
(select top 1 ConfigValue from sys_UserConfig suc 
where suc.UserId = sa.AdminId and ConfigType ='DefaultGroup') CurrGroupId,
(select top 1 ConfigValue from sys_UserConfig suc 
where suc.UserId = sa.AdminId and ConfigType ='DefaultPosition') CurrPositionId
from dbo.sys_Admin sa
LEFT JOIN CPM_Group_User cgu ON cgu.UserID = sa.AdminId
LEFT join CPM_Group cg on cgu.GroupID = cg.GroupID
LEFT join CPM_Position cp on cp.GroupId = cg.GroupId
LEFT join CPM_Position_User cpu on cpu.PositionID = cp.PositionID
where cgu.GroupID IS NOT NULL
AND sa.AdminId = @AdminId";
             */
            string sql = @"SELECT DISTINCT GroupId,GroupCode,GroupName,PositionId,PositionCode,PositionName,
CurrGroupId,CurrPositionId,PositionLevel,
CurrRoleId,RoleId,RoleName
FROM(
SELECT cg.GroupId,cg.GroupCode,cg.GroupName,
cp.PositionId,cp.PositionCode,cp.PositionName,cp.PositionLevel,
(select top 1 ConfigValue from sys_UserConfig suc 
where suc.UserId = sa.AdminId and ConfigType ='DefaultGroup')
 CurrGroupId,
(select top 1 ConfigValue from sys_UserConfig suc 
where suc.UserId = sa.AdminId and ConfigType ='DefaultPosition')
 CurrPositionId,
(select top 1 ConfigValue from sys_UserConfig suc 
where suc.UserId = sa.AdminId and ConfigType ='DefaultRole')
 CurrRoleId,
cr.RoleId,
cr.RoleName
from CPM_Position_User cpu
join dbo.sys_Admin sa ON sa.AdminId  = cpu.UserID 
join CPM_Position cp on cpu.PositionID = cp.PositionID
LEFT join CPM_Group cg on cpu.GroupID = cg.GroupID
left join CPM_Role cr on cpu.RoleId = cr.RoleId
where cg.GroupID IS NOT NULL
AND sa.AdminId = @AdminId
/*
UNION ALL 
SELECT DISTINCT cg.GroupId,cg.GroupCode,cg.GroupName,
null, null,null,null,null,null,null,null
from dbo.sys_Admin sa
LEFT JOIN CPM_Group_User cgu ON cgu.UserID = sa.AdminId
LEFT join CPM_Group cg on cgu.GroupID = cg.GroupID
LEFT join CPM_Position_User cpu 
	on cpu.GroupID = cgu.GroupID AND sa.AdminId = cpu.UserID
where cgu.GroupID IS NOT NULL AND cpu.PositionID IS NULL
AND sa.AdminId = @AdminId
*/
)t";
            var p = new Dictionary<string, object>();
            //获取主键
            p.Add("AdminId", adminid);

            var ds = QueryData(sql, p);
            return ds.Tables[0];
        }


		#endregion

        static Dictionary<string, string> adminidnamelist = null;
        static object lckobj = new object();

        /// <summary>
        /// 获取用户ID 与名称的键值对
        /// </summary>
        public static Dictionary<string, string> AdminIdNameList
        {
            get
            {
                lock (lckobj)
                {
                    if (adminidnamelist == null || adminidnamelist.Count == 0)
                    {
                        lock (lckobj)
                        {

                            if (adminidnamelist == null)
                                adminidnamelist = new Dictionary<string, string>();

                            var dao = new UkeyTech.WebFW.DAO.AdminDAO();
                            var list = dao.GetAll("");
                            foreach (var m in list)
                            {
                                //if (m.Status == 1) do not filter anything
                                    adminidnamelist.Add(m.AdminId, m.AdminName);
                            }
                        }
                    }
                }

                return adminidnamelist;
            }
        }
        /// <summary>
        /// 获取操作员的名字
        /// </summary>
        /// <param name="adminid"></param>
        /// <returns></returns>
        public static string getAdminName(string adminid)
        {
            if (string.IsNullOrEmpty(adminid))
                return string.Empty;

            if (AdminIdNameList.ContainsKey(adminid))
                return AdminIdNameList[adminid];
            else
            {
                //尝试清空再加
                AdminIdNameList.Clear();
                return AdminIdNameList.ContainsKey(adminid) ? AdminIdNameList[adminid] : adminid;
            }
        }

        /// <summary>
        /// 获取操作员的名字
        /// </summary>
        /// <param name="adminid"></param>
        /// <returns></returns>
        public List<string> GetEnabledAdminIds(string[] adminids)
        {
            StringBuilder sb = new StringBuilder(100);
            sb.Append("'");
            foreach (string adminid in adminids) {
                sb.Append(adminid.Replace("'", ""));
                sb.Append("','");
            }
            sb.Append("'");

            var idvals = sb.ToString();
            var conn = DbService();
            return conn.Query<string>(string.Format("select AdminId from sys_Admin with(nolock) where Status = 1 AND AdminId in ({0})", idvals)).ToList();
        }

        /// <summary>
        /// 获取操作员的名字
        /// </summary>
        /// <param name="adminid"></param>
        /// <returns></returns>
        public List<string> GetEnabledAdminIds(string adminids)
        {
            StringBuilder sb = new StringBuilder(50);
            var ids = adminids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var idvals = "'" + string.Join("','", ids) + "'";
            var conn = DbService();
            return conn.Query<string>(string.Format("select AdminId from sys_Admin with(nolock) where Status = 1 AND AdminId in ({0})", idvals)).ToList();
        }


        public static string getAdminNames(string adminids)
        {
            if (string.IsNullOrEmpty(adminids))
                return string.Empty;

            StringBuilder sb = new StringBuilder(50);

            var ids = adminids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var adminid in ids)
            {
                if (string.IsNullOrEmpty(adminid))
                    return string.Empty;

                if (AdminIdNameList.ContainsKey(adminid))
                    sb.Append(AdminIdNameList[adminid] + ",");
                else
                {
                    //尝试清空再加
                    AdminIdNameList.Clear();
                    if (AdminIdNameList.ContainsKey(adminid))
                    {
                        sb.Append(AdminIdNameList[adminid] + ",");
                    }
                    else
                    {
                        sb.Append("[" + adminid + "],");
                    }
                }
            }

            return sb.ToString().Trim(',');
        }


        public DataTable GetGroup(string adminId) {

            string sql = "select GroupID from dbo.CPM_Group_User where userid=@userid order by GroupID desc";
            var p = new Dictionary<string, object>();
            //获取主键
            p.Add("userid", adminId);

            var ds = QueryData(sql, p);
            return ds.Tables[0];
        }

        /// <summary>
        /// 根据主键创建 Admin 数据模型实例 
        /// </summary>
        public Admin GetViewModel(string AdminId)
        {
            Admin model = null;
            var p = new DynamicParameters();
            //获取主键
            p.Add("AdminId", AdminId, DbType.String, null, 50);
            var conn = DbService();
            try
            {
                var rst = conn.Query<Admin>(
                @"SELECT sa.[AdminId]
                  ,sa.[AdminName]
                  ,sa.[LoginName]
                  ,sa.[Password]
                  ,sa.[Email]
                  ,sa.[Joined]
                  ,sa.[LastVisit]
                  ,sa.[IP]
                  ,sa.[LanguageFile]                  
                  ,sa.[IsActived]
                  ,sa.[Descn]
                  ,sa.[DeptCode]                  
                  ,sa.[Postion]                 
                  ,sa.[UsedDeptCode],
                    MobilePhone,
                    EmpType,
                    sdo.[Name] as EmpTypeName,
                    BudgetDeptCode,
                    sds.[Name] as BudgetDeptName,
                    sd.GroupName as UsedDeptName,
                    sa.EmailPwd,
                    sa.[MappingAccount],
                    sa.[PasswordLastUpdateTime],
                    sa.Status,sa.Nation
              FROM [sys_admin] sa 
                left join Base_UsedDept  sd on Convert(varchar(50),sd.GroupID) = sa.UsedDeptCode
                left join sys_DictItems sdo on sdo.Code = sa.EmpType and sdo.DictID = 'EmployeeType'
                left join sys_DictItems sds on sds.Code = sa.BudgetDeptCode and sds.DictID = 'BaseBudgetDept'
			    where sa.[AdminId] = @AdminId", p);

                List<Admin> lrst = new List<Admin>(rst);

                if (lrst.Count > 0)
                    model = lrst[0];
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
	}
}