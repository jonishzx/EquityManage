namespace Clover.Permission.DAO
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Text;
    using Dapper;

    using Clover.Permission.Model;
    using Clover.Permission.BLL;

    /// <summary>
    /// FuncPermission 数据访问层
    /// </summary>
    partial class FuncPermissionDAO : Clover.Data.BaseDAO
    {
        #region 构造函数
        public FuncPermissionDAO()
        {
        }
        #endregion

        #region 根据主键创建 FuncPermission 数据模型实例
        /// <summary>
        /// 根据主键创建 FuncPermission 数据模型实例 
        /// </summary>
        public FuncPermission GetModel(int FuncPermissionID)
        {
            FuncPermission model = null;
            var p = new DynamicParameters();
            //获取主键
            p.Add("FuncPermissionID", FuncPermissionID, DbType.Int32, null, 4);

            var conn = DbService();

            try
            {
                var rst = conn.Query<FuncPermission>(
                @"select * from CPM_FuncPermission where 	[FuncPermissionID] = @FuncPermissionID
", p);

                List<FuncPermission> lrst
                    = new List<FuncPermission>(rst);

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
        #endregion

        #region 更新记录
        /// <summary>
        /// 更新记录到数据库
        /// </summary>
        public bool Update(FuncPermission model)
        {
            return Update(null, model);
        }
        /// <summary>
        /// 更新记录到数据库(利用事务)
        /// </summary>
        public bool Update(IDbTransaction tran, FuncPermission model)
        {
            int affectedrows = 0;

            var p = new DynamicParameters();
            p.Add("FuncPermissionID", model.FuncPermissionID, DbType.Int32, null, 4);
            p.Add("UserID", model.UserID, DbType.String, null, 50);
            p.Add("RoleID", model.RoleID, DbType.Int32, null, 4);
            p.Add("ModuleID", model.ModuleID, DbType.Int32, null, 4);
            p.Add("FunctionID", model.FunctionID, DbType.Int32, null, 4);
            p.Add("IsAllow", model.IsAllow, DbType.Boolean, null, 1);
            p.Add("IsDeny", model.IsDeny, DbType.Boolean, null, 1);
            p.Add("CreateDate", model.CreateDate, DbType.DateTime, null, 8);
            p.Add("GroupID", model.GroupID, DbType.Int32, null, 4);
            p.Add("DataPermissionId", model.DataPermissionId, DbType.Int32, null, 4);

            string sql = @"UPDATE [CPM_FuncPermission] SET
	[UserID] = @UserID,
	[RoleID] = @RoleID,
	[ModuleID] = @ModuleID,
	[FunctionID] = @FunctionID,
	[IsAllow] = @IsAllow,
	[IsDeny] = @IsDeny,
	[CreateDate] = @CreateDate,
	[GroupID] = @GroupID,
    [DataPermissionId] = @DataPermissionId
                    WHERE
                        	[FuncPermissionID] = @FuncPermissionID
";
            IDbConnection conn = tran == null ? DbService() : tran.Connection;

            try
            {
                affectedrows = conn.Execute(sql, p);
            }
            catch (DataException ex)
            {
                throw ex;
            }
            finally
            {
                
            }

            return Convert.ToBoolean(affectedrows);
        }
        #endregion

        #region 新增记录
        /// <summary>
        /// 新增记录到数据库
        /// </summary>
        public bool Insert(FuncPermission model)
        {
            return Insert(null, model);
        }

        /// <summary>
        /// 新增记录到数据库(事务)
        /// </summary>
        public bool Insert(IDbTransaction tran, FuncPermission model)
        {
            var p = new DynamicParameters();
            p.Add("FuncPermissionID", model.FuncPermissionID, DbType.Int32, null, 4);
            p.Add("UserID", model.UserID, DbType.String, null, 50);
            p.Add("RoleID", model.RoleID, DbType.Int32, null, 4);
            p.Add("ModuleID", model.ModuleID, DbType.Int32, null, 4);
            p.Add("FunctionID", model.FunctionID, DbType.Int32, null, 4);
            p.Add("IsAllow", model.IsAllow, DbType.Boolean, null, 1);
            p.Add("IsDeny", model.IsDeny, DbType.Boolean, null, 1);
            p.Add("CreateDate", model.CreateDate, DbType.DateTime, null, 8);
            p.Add("GroupID", model.GroupID, DbType.Int32, null, 4);
            p.Add("DataPermissionId", model.DataPermissionId, DbType.Int32, null, 4);

            string sql = @"INSERT INTO [CPM_FuncPermission] (                          
	[UserID],
	[RoleID],
	[ModuleID],
	[FunctionID],
	[IsAllow],
	[IsDeny],
	[CreateDate],
	[GroupID],
    [DataPermissionId]
                        ) VALUES (
                            	@UserID,
	@RoleID,
	@ModuleID,
	@FunctionID,
	@IsAllow,
	@IsDeny,
	@CreateDate,
	@GroupID,
    @DataPermissionId
)";

            sql += ";select @@IDENTITY";

            IDbConnection conn = tran == null ? DbService() : tran.Connection;

            try
            {
                var keys = new List<decimal>(conn.Query<decimal>(sql, p));

                model.FuncPermissionID = Convert.ToInt32(keys[0]);
            }
            catch (DataException ex)
            {
                throw ex;
            }
           
            return true;
        }
        #endregion

        #region 删除记录
        /// <summary>
        /// 从数据库删除记录(事务)
        /// </summary>
        public bool Delete(int FuncPermissionID)
        {
            return Delete(null, FuncPermissionID);
        }


        /// <summary>
        /// 从数据库删除记录(事务)
        /// </summary>
        public bool Delete(IDbTransaction tran, int FuncPermissionID)
        {
            int affectedrows = 0;

            var p = new DynamicParameters();
            //获取主键
            p.Add("FuncPermissionID", FuncPermissionID, DbType.Int32, null, 4);

            string sql = @"DELETE FROM [CPM_FuncPermission]
                        WHERE 	[FuncPermissionID] = @FuncPermissionID
";

            IDbConnection conn = tran == null ? DbService() : tran.Connection;

            try
            {
                affectedrows = conn.Execute(sql, p);
            }
            catch (DataException ex)
            {
                throw ex;
            }
            finally
            {
                
            }

            return Convert.ToBoolean(affectedrows);
        }

        #endregion

        #region 查询，返回自定义类
        /// <summary>
        /// 查询所有记录，并排序
        /// </summary>
        public List<FuncPermission> GetAll(string orderBy)
        {
            return GetList(null, null, string.Empty, orderBy);
        }

        public List<FuncPermission> GetListTran(IDbTransaction tran ,int? top, string strWhere, string orderBy)
        {
            IDbConnection conn = tran != null ? tran.Connection : DbService();

            return GetList(conn, top, strWhere, orderBy);
        }
        /// <summary>
        /// 查询所有记录，并排序(事务)
        /// </summary>
        public List<FuncPermission> GetList(IDbConnection conn, int? top, string strWhere, string orderBy)
        {
          
            StringBuilder strSql = new StringBuilder();
            strSql.Append("Select ");
            if (top.HasValue)
            {
                strSql.Append(" top " + top.ToString());
            }
            strSql.Append(@" 	[FuncPermissionID],
	[UserID],
	[RoleID],
	[ModuleID],
	[FunctionID],
	[IsAllow],
	[IsDeny],
	[CreateDate],
	[GroupID],
    [DataPermissionId]
 ");
            strSql.Append(" FROM [CPM_FuncPermission] ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            if (orderBy != string.Empty)
                strSql.Append(" order by " + orderBy);


            var rst = conn.Query<FuncPermission>(strSql.ToString(), null);

            

            return new List<FuncPermission>(rst);
        }

        /// <summary>
        /// 查询所有记录，并排序、分页
        /// </summary>
        public List<FuncPermission> GetAllPaged(IDbConnection conn, int PageSize, int PageIndex, string strWhere, bool desc, string orderfldname)
        {
            int rstcount = 0;
            return GetAllPaged(null, PageSize, PageIndex, strWhere, desc, orderfldname, out rstcount);
        }

        /// <summary>
        /// 查询所有记录，并排序、分页,没有排序字段
        /// </summary>
        public List<FuncPermission> GetAllPaged(int PageSize, int PageIndex, string strWhere, bool desc)
        {
            return GetAllPaged(null, PageSize, PageIndex, strWhere, desc, "FuncPermissionID");
        }

        /// <summary>
        /// 查询所有记录，并排序、分页(事务)
        /// </summary>
        public List<FuncPermission> GetAllPaged(IDbConnection conn, int PageSize, int PageIndex, string strWhere, bool desc, string orderfldname, out int rstcount)
        {
            return Clover.Data.BaseDAO.GetList<FuncPermission>("[CPM_FuncPermission]", "FuncPermissionID", PageSize, PageIndex, strWhere, desc, orderfldname, out rstcount);
        }

        #endregion

        #region 计算查询结果记录数
        /// <summary>
        /// 对所有记录进行记录数计算
        /// </summary>
        public int SumAllCount()
        {
            return SumDynamicCount("");
        }
        /// <summary>
        /// 对所有符合条件的记录进行记录数计算
        /// </summary>
        public int SumDynamicCount(string strWhere)
        {
            string strSQL = "select count(*) from [CPM_FuncPermission]";
            strSQL = string.IsNullOrEmpty(strWhere) ? strSQL : strSQL + " where " + strWhere;
            var conn = DbService();
            IEnumerable<int> rst = conn.Query<int>(strSQL, null);
            
            return new List<int>(rst)[0];

        }
        #endregion


        #region 其他自定义方法

        /// <summary>
        /// 获取所有权限
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="scopeWhere"></param>
        /// <returns></returns>
        public List<UserFuncPMResult> GetAllModuleFunc()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(@"SELECT distinct d.ModuleID,d.ModuleName,a.FunctionID,a.FunctionCode,a.FunctionName,CONVERT(BIT,1) IsAllow ,CONVERT(BIT,0)  IsDeny,
                                IsSelf = 1
                                FROM CPM_Function a                                
                                CROSS JOIN CPM_Module d");
               
            var rst = DbService().Query<UserFuncPMResult>(builder.ToString(), null);
            return new List<UserFuncPMResult>(rst);          
        }

        /// <summary>
        /// 检查是否已经存在相应的权限设置
        /// </summary>
        /// <param name="model"></param>
        /// <param name="owner"></param>
        /// <param name="owenValue"></param>
        /// <returns></returns>
        public int? CheckOwnerPermissionExists(FuncPermission model, PermissionOwner owner, string owenValue)
        {
            string sql = string.Format("select FuncPermissionID from CPM_FuncPermission where {0} = '{1}' and ModuleID = '{2}' and FunctionID='{3}'",
                Util.GetOwnerFieldName(owner), owenValue, model.ModuleID, model.FunctionID);

            var rst = DbService().Query<int>(sql).ToList();
            if (rst.Count() > 0)
                return rst[0];
            else
                return null;
            
        }

        /// <summary>
        /// 更新数据权限
        /// </summary>
        /// <param name="model"></param>
        /// <param name="owner"></param>
        /// <param name="owenValue"></param>
        /// <returns></returns>
        public bool SaveOrUpdateFuncDataPermission(FuncPermission model, PermissionOwner owner, string owenValue) {

            var chkrst = CheckOwnerPermissionExists(model, owner, owenValue);
            if (!chkrst.HasValue)
            {
                switch (owner)
                {
                    case PermissionOwner.User:
                        model.UserID = owenValue;
                        break;
                    case PermissionOwner.Role:
                        model.RoleID = int.Parse(owenValue);
                        break;
                    case PermissionOwner.Group:
                        model.GroupID = int.Parse(owenValue);
                        break;
                }
                //插入
                return Insert(model);
            }
            else {
                model.FuncPermissionID = chkrst.Value;
                return Update(model);
            }
        }

        /// <summary>
        /// 克隆权限信息权限
        /// </summary>
        /// <param name="owner">所属权限范围</param>
        /// <param name="srcOwnerValue">所属数据来源范围</param>
        /// <param name="targetOwnerValue">克隆到的目标</param>
        public void CloneFuncPermission(PermissionOwner owner, string srcOwnerValue, string targetOwnerValue)
        {
            var ownerTitle = string.Empty;
            string sql = @"delete CPM_FuncPermission where {0} = '{1}';";
            switch (owner)
            {
                case PermissionOwner.User:
                    ownerTitle = "UserID";
                    sql += @"insert into CPM_FuncPermission(UserID, RoleID, ModuleID, FunctionID, IsAllow, IsDeny, CreateDate, GroupID, DataPermissionId)
                    select '{1}', RoleID, ModuleID, FunctionID, IsAllow, IsDeny, CreateDate, GroupID, DataPermissionId 
                    from CPM_FuncPermission
                    where {0} = '{2}';";
                    break;
                case PermissionOwner.Role:
                    ownerTitle = "RoleID";
                    sql += @"insert into CPM_FuncPermission(UserID, RoleID, ModuleID, FunctionID, IsAllow, IsDeny, CreateDate, GroupID, DataPermissionId)
                    select UserID, '{1}', ModuleID, FunctionID, IsAllow, IsDeny, CreateDate, GroupID, DataPermissionId 
                    from CPM_FuncPermission
                    where {0} = '{2}';";
                    break;
                case PermissionOwner.Group:
                    ownerTitle = "GroupID";
                    sql += @"insert into CPM_FuncPermission(UserID, RoleID, ModuleID, FunctionID, IsAllow, IsDeny, CreateDate, GroupID, DataPermissionId)
                    select UserID, RoleID, ModuleID, FunctionID, IsAllow, IsDeny, CreateDate, '{1}', DataPermissionId 
                    from CPM_FuncPermission
                    where {0} = '{2}';";
                    break;
            }

            sql = string.Format(sql, ownerTitle, targetOwnerValue, srcOwnerValue);

            DbService().Execute(sql);
        }

        /// <summary>
        /// 获取组的权限信息(只获取状态)
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="scopeWhere"></param>
        /// <returns></returns>
        public List<UserFuncPMResult> GetPMAllModuleFunc(int roleId)
        {
            StringBuilder builder = new StringBuilder();
            DynamicParameters p = new DynamicParameters();

            builder.Append(@"SELECT distinct d.ModuleID,d.ModuleName,a.FunctionID,a.FunctionCode,a.FunctionName,CONVERT(BIT,1) IsAllow ,CONVERT(BIT,0)  IsDeny,
                                IsSelf = 1
                                FROM CPM_Function a
                                CROSS JOIN CPM_Module d
                                Where d.ModuleCode in ('Permission','Module','Function','Account','Role','Group','FuncPermission','PMSystem')     
            ");

            Role model = new RoleDAO().GetModel(roleId);
            if (model != null)
            {
                builder.Append(@"   union
                            SELECT d.ModuleID,d.ModuleName,a.FunctionID,a.FunctionCode,a.FunctionName,isnull(c.IsAllow,0) IsAllow ,isnull(c.IsDeny,0) IsDeny
                                ,IsSelf = CASE c.RoleID WHEN @RoleID THEN 1 ELSE 0 END 
                                FROM CPM_Function a
                                INNER JOIN CPM_Module_Function b ON a.FunctionID = b.FunctionID 
                                INNER JOIN dbo.CPM_Module d on b.ModuleID=d.ModuleID                                 
                                LEFT JOIN CPM_FuncPermission c ON b.FunctionID = c.FunctionID AND b.ModuleID = c.ModuleID
                                AND CHARINDEX('\'+CAST(c.RoleID AS NVARCHAR(50))+'\',@ParentPath) > 0 AND (c.IsAllow = 1 OR c.RoleId = @RoleID) ");

                p.Add("RoleID", roleId, DbType.Int16, null, null);
                p.Add("ParentPath", "\\" + model.ParentPath + "\\", DbType.String, null, null);
            }

            var rst = DbService().Query<UserFuncPMResult>(builder.ToString(), p);
            return new List<UserFuncPMResult>(rst);
        }

        private string GetFuncSql(string userId, string scopeFieldName, bool enableuserpermission, bool enablegrouppermission, bool enabledenyPermission, bool enableinheritpermission)
        {
            return GetFuncSql(userId, scopeFieldName, string.Empty, enableuserpermission, enablegrouppermission, enabledenyPermission, enableinheritpermission);
        }
        /// <summary>
        ///  用户所具有权限查询语句
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId">角色ID，可为空</param>
        /// <param name="scopeFieldName"></param>
        /// <param name="enableuserpermission"></param>
        /// <param name="enablegrouppermission"></param>
        /// <param name="enabledenyPermission"></param>
        /// <returns></returns>
        private string GetFuncSql(string userId, string scopeFieldName, string roleId, bool enableuserpermission, bool enablegrouppermission, bool enabledenyPermission, bool enableinheritpermission)
        {
            string str = enableuserpermission ? " = 0 " : string.Empty;
            StringBuilder builder = new StringBuilder();
            StringBuilder builder2 = new StringBuilder();

            builder.Append("SELECT c.ModuleID,c.ModuleCode,c.ModuleTag,b.FunctionCode,min(a.DataPermissionId) DataPermissionId FROM ");
            builder.Append("( ");
            
            //启用用户权限
            if (enableuserpermission)
            {
                builder2.AppendLine("SELECT ModuleID,FunctionID,DataPermissionId,IsAllow,IsDeny ");
                builder2.AppendLine("FROM CPM_FuncPermission ");
                builder2.AppendLine("WHERE UserID = @UserID ");
            }

            //角色权限
            List<Role> roleByUser = new RoleDAO().GetRoleByUser(userId, roleId);
            if (roleByUser.Count > 0)
            {
                string rolePath = "\\";
                List<string> roleIds = new List<string>();
                roleByUser.ForEach(delegate(Role r)
                {
                    roleIds.Add(r.RoleID.ToString());
                    rolePath = rolePath + r.ParentPath + "\\";
                });
                string rolecondtion = (!enableuserpermission && (roleIds.Count > 0)) ? (" OR RoleID IN (" + string.Join(",", roleIds.ToArray()) + ") ") : string.Empty;
                if (builder2.Length > 0)
                {
                    builder2.Append(" UNION ALL ");
                }
                builder2.AppendLine(" SELECT ModuleID,FunctionID,");
                if (!enableuserpermission && (roleIds.Count > 0))
                {
                    //for 数据权限(如果自己没有权限则尝试获取父级的数据权限)
                    builder2.AppendLine(@" isnull((select top 1 cfp.DataPermissionId from CPM_FuncPermission cfp join CPM_FunctionDataRule cfdr on cfp.DataPermissionId = cfdr.DataPermissionId
where cfp.ModuleID = b.ModuleID and cfp.FunctionID = b.FunctionID 
 " + rolecondtion.Replace("OR", "AND") + " AND cfp.DataPermissionId is not null Order By cfdr.Priority),"
                    + (enableinheritpermission ? @"(select top 1 cfp.DataPermissionId from CPM_FuncPermission cfp join CPM_FunctionDataRule cfdr on cfp.DataPermissionId = cfdr.DataPermissionId
 where cfp.ModuleID = b.ModuleID and cfp.FunctionID = b.FunctionID 
 AND CHARINDEX('\'+ CAST(RoleID AS NVARCHAR(50)) + '\','" + rolePath.Replace("\\\\", "\\") + "') > 0 AND IsAllow = 1 and cfp.DataPermissionId is not null Order By cfdr.Priority)" : "null")
                    + ") DataPermissionId, ");
                    //end for 数据权限
                }
                builder2.AppendLine(" IsAllow,IsDeny " + str);
                builder2.AppendLine(" FROM CPM_FuncPermission b");

                string[] strArray = null;
                if (enableinheritpermission)
                {
                    strArray = new string[] { " WHERE CHARINDEX('\\'+ CAST(RoleID AS NVARCHAR(50)) + '\\','", 
                    rolePath.Replace("\\\\", "\\"), "') > 0 AND (IsAllow = 1 ",
                    (!enableuserpermission && (roleIds.Count > 0)) ? (" OR RoleID IN (" + string.Join(",", roleIds.ToArray()) + ") ") : string.Empty,
                    ") " };
                }
                else {
                    strArray = new string[] { 
                    (!enableuserpermission && (roleIds.Count > 0)) ? (" WHERE RoleID IN (" + string.Join(",", roleIds.ToArray()) + ") ") : string.Empty,
                    };
                }
                builder2.AppendLine(string.Concat(strArray));
            }

            //组权限
            if (enablegrouppermission)
            {
                List<Group> groupByUser = new GroupDAO().GetGroupByUser(userId);
                if (groupByUser.Count > 0)
                {
                    string groupPath = "\\";
                    List<string> groupIds = new List<string>();
                    groupByUser.ForEach(delegate(Group g)
                    {
                        groupIds.Add(g.GroupID.ToString());
                        groupPath = groupPath + g.ParentPath + "\\";
                    });

                  
                    if (builder2.Length > 0)
                    {
                        builder2.Append(" UNION ALL ");
                    }

                    string groupcondtion = (!enableuserpermission && (groupIds.Count > 0)) ? (" OR GroupID IN (" + string.Join(",", groupIds.ToArray()) + ") ") : string.Empty;
                    builder2.AppendLine(" SELECT ModuleID,FunctionID,");
                    if (!enableuserpermission && (groupIds.Count > 0))
                    {
                        //for 数据权限(如果自己没有权限则尝试获取父级的数据权限)
                        builder2.AppendLine(@" isnull((select top 1 cfp.DataPermissionId from CPM_FuncPermission cfp join CPM_FunctionDataRule cfdr on cfp.DataPermissionId = cfdr.DataPermissionId
where cfp.ModuleID = b.ModuleID and cfp.FunctionID = b.FunctionID " + groupcondtion.Replace("OR", "AND") + " AND cfp.DataPermissionId is not null Order By cfdr.Priority),"
                        + (enableinheritpermission ? @"(select top 1 cfp.DataPermissionId from CPM_FuncPermission cfp join CPM_FunctionDataRule cfdr on cfp.DataPermissionId = cfdr.DataPermissionId
 where cfp.ModuleID = b.ModuleID and cfp.FunctionID = b.FunctionID 
 AND CHARINDEX('\'+ CAST(GroupID AS NVARCHAR(50)) + '\','" + groupPath.Replace("\\\\", "\\") + "') > 0 AND IsAllow = 1 and cfp.DataPermissionId is not null Order By cfdr.Priority)" : "null")
                        + ") DataPermissionId, ");
                        //end for 数据权限
                    }
                    builder2.AppendLine(" IsAllow,IsDeny " + str);
                    builder2.AppendLine(" FROM CPM_FuncPermission b");
                    builder2.AppendLine(" FROM CPM_FuncPermission ");
                    if (enableinheritpermission)
                    {
                        builder2.AppendLine(" WHERE CHARINDEX('\\'+ CAST(GroupID AS NVARCHAR(50)) + '\\','" + groupPath.Replace("\\\\", "\\") + "') > 0");
                        builder2.AppendLine(" AND (IsAllow = 1 " + groupcondtion + ") ");
                    }
                    else {
                        builder2.AppendLine((!enableuserpermission && (groupIds.Count > 0)) ? (" WHERE GroupID IN (" + string.Join(",", groupIds.ToArray()) + ") ") : string.Empty);
                    }
                    
                }
            }
            if (builder2.Length == 0)
            {
                return string.Empty;
            }
            builder.Append(builder2.ToString());
            builder.AppendLine(") a ");
            builder.AppendLine(" INNER JOIN CPM_Function b ON a.FunctionID = b.FunctionID ");
            builder.AppendLine(" INNER JOIN CPM_Module c ON a.ModuleID = c.ModuleID ");
            builder.Append(" AND c." + scopeFieldName + " = @FieldValue ");
            if (!enabledenyPermission)
            {
                builder.Append(" AND a.IsAllow = 1 ");
            }
            builder.Append(" GROUP BY c.ModuleID,c.ModuleCode,c.ModuleTag,b.FunctionCode,a.DataPermissionId ");

            //使用拒绝权限设置
            if (enabledenyPermission)
            {
                builder.Append(" HAVING COUNT(1) =  SUM(Case WHEN a.IsDeny =0 THEN 1 ELSE 0 END) ");
            }

            return builder.ToString();
        }


        /// <summary>
        /// 获取组的权限信息(只获取状态)
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="scopeWhere"></param>
        /// <returns></returns>
        public List<UserFuncPMResult> GetGroupFunc(int groupId, string scopeWhere, bool enableinheritpermission)
        {

            Group model = new GroupDAO().GetModel(groupId);
            if (model != null)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(@"SELECT d.ModuleID,d.ModuleName,a.FunctionID,a.FunctionCode,a.FunctionName,isnull(c.IsAllow,0) IsAllow ,isnull(c.IsDeny,0) IsDeny
                                 IsSelf = CASE c.GroupID WHEN @GroupID THEN 1 ELSE 0 END
                                 ,FuncPermissionID, c.DataPermissionId, d.Priority DataPermissionPriority
                                 FROM CPM_Function a
                                 INNER JOIN CPM_Module_Function b ON a.FunctionID = b.FunctionID 
                                 INNER JOIN CPM_Module d on b.ModuleID=d.ModuleID 
                                 LEFT JOIN CPM_FuncPermission c ON b.FunctionID = c.FunctionID AND b.ModuleID = c.ModuleID ");

                if (enableinheritpermission)
                {
                    builder.Append(" AND CHARINDEX('\'+CAST(c.GroupID AS NVARCHAR(50))+'\',@ParentPath) > 0 AND (c.IsAllow = 1 OR c.GroupID = @GroupID)");
                }
                else
                {
                    builder.Append(" AND c.GroupID = @GroupID ");
                }
                builder.Append(" LEFT JOIN CPM_FunctionDataRule e on c.DataPermissionId = e.DataPermissionId ");
                //过滤模块的查询条件
                builder.AppendLine(!string.IsNullOrEmpty(scopeWhere) ? (" Where d." + scopeWhere + " ") : string.Empty);

                //排序
                builder.AppendLine(" order by viewOrd asc,FunctionName ");

                DynamicParameters p = new DynamicParameters();
                var conn = DbService();

                if (model.ParentPath.IndexOf("\\") < 0)
                    model.ParentPath = "\\" + model.ParentPath;

                p.Add("GroupID", groupId, DbType.Int16, null, null);
                p.Add("ParentPath", model.ParentPath.Replace("\\\\", "\\") + "\\", DbType.String, null, null);

                var rst = conn.Query<UserFuncPMResult>(builder.ToString(), p);
                
                return new List<UserFuncPMResult>(rst);

            }
            return null;
        }


        public List<UserFuncPMResult> GetRoleFunc(int roleId, string scopeWhere, bool enableinheritpermission)
        {
            Role model = new RoleDAO().GetModel(roleId);
            if (model != null)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(@"SELECT d.ModuleID,d.ModuleName,a.FunctionID,a.FunctionCode,a.FunctionName,isnull(c.IsAllow,0) IsAllow ,isnull(c.IsDeny,0) IsDeny
                ,IsSelf = CASE c.RoleID WHEN @RoleID THEN 1 ELSE 0 END 
                ,FuncPermissionID, c.DataPermissionId, e.Priority DataPermissionPriority,a.ViewOrd
                FROM CPM_Function a
                INNER JOIN CPM_Module_Function b ON a.FunctionID = b.FunctionID 
                INNER JOIN dbo.CPM_Module d on b.ModuleID=d.ModuleID 
                LEFT JOIN CPM_FuncPermission c ON b.FunctionID = c.FunctionID AND b.ModuleID = c.ModuleID");
                if(enableinheritpermission){
                    builder.Append(" AND CHARINDEX('\'+CAST(c.RoleID AS NVARCHAR(50))+'\',@ParentPath) > 0 AND (c.IsAllow = 1 OR c.RoleID = @RoleID) ");
                }
                else{
                    builder.Append(" AND c.RoleID = @RoleID ");
                }
                builder.Append(" LEFT JOIN CPM_FunctionDataRule e on c.DataPermissionId = e.DataPermissionId ");

                //过滤模块的查询条件
                builder.AppendLine(!string.IsNullOrEmpty(scopeWhere) ? ("Where d." + scopeWhere + " ") : string.Empty);

                //排序
                builder.AppendLine(" order by viewOrd asc,FunctionName ");

                var conn = DbService();
                DynamicParameters p = new DynamicParameters();

                if(model.ParentPath.IndexOf("\\")<0)
                    model.ParentPath = "\\" + model.ParentPath;
                
                p.Add("RoleID", roleId, DbType.Int16, null, null);
                p.Add("ParentPath", model.ParentPath.Replace("\\\\","\\") + "\\", DbType.String, null, null);

                var rst = conn.Query<UserFuncPMResult>(builder.ToString(), p);
                
                return new List<UserFuncPMResult>(rst);

            }
            return null;
        }

        public List<UserFuncPMResult> GetModuleFunc(string userId, int moduleId, bool enableuserpermission, bool enablegrouppermission, bool enabledenyPermission, bool enableinheritpermission)
        {           
            string funcSql = this.GetFuncSql(userId, Util.GetScopeFieldName(Clover.Permission.BLL.FilterScope.System),
                enableuserpermission,
                enablegrouppermission,
                enabledenyPermission,
                enableinheritpermission);

            var conn = DbService();
            DynamicParameters p = new DynamicParameters();
            if (!string.IsNullOrEmpty(funcSql))
            {
                p.Add("UserID", userId, DbType.String, null, null);
                p.Add("FieldValue", moduleId, DbType.String, null, null);

            }

            var rst = conn.Query<UserFuncPMResult>(funcSql, p);
            

            return new List<UserFuncPMResult>(rst);
        }


        public List<UserFuncPMResult> GetSystemFunc(string userId, int systemId, bool enableuserpermission, bool enablegrouppermission, bool enabledenyPermission, bool enableinheritpermission)
        {
            return GetSystemFunc(userId, systemId, string.Empty, enableuserpermission, enablegrouppermission, enabledenyPermission, enableinheritpermission);
        }

        public List<UserFuncPMResult> GetSystemFunc(string userId, int systemId, string roleId, bool enableuserpermission, bool enablegrouppermission, bool enabledenyPermission, bool enableinheritpermission)
        {
            string funcSql = this.GetFuncSql(userId,
                Util.GetScopeFieldName(Clover.Permission.BLL.FilterScope.System),
                roleId,
                enableuserpermission,
                enablegrouppermission,
                enabledenyPermission,
                enableinheritpermission);


            var conn = DbService();
            DynamicParameters p = new DynamicParameters();
            if (!string.IsNullOrEmpty(funcSql))
            {
                p.Add("UserID", userId, DbType.String, null, null);
                p.Add("FieldValue", systemId, DbType.String, null, null);

                var rst = conn.Query<UserFuncPMResult>(funcSql.ToString(), p);
                
                return new List<UserFuncPMResult>(rst);

            }
            else
                return new List<UserFuncPMResult>();         
        }


        /// <summary>
        /// 获取用户的权限信息（具有的以及未具有的）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="scopeWhere"></param>
        /// <param name="enableuserpermission"></param>
        /// <param name="enablegrouppermission"></param>
        /// <returns></returns>
        public List<UserFuncPMResult> GetUserFunctions(string userId, string scopeWhere, bool enableuserpermission, bool enablegrouppermission, bool enableinheritpermission)
        {
            StringBuilder builder = new StringBuilder();
            StringBuilder builder2 = new StringBuilder();
            builder.Append(@"SELECT c.ModuleID,c.ModuleName,a.FunctionID,a.FunctionCode,a.FunctionName,isnull(d.IsAllow,0) IsAllow,isnull(d.IsDeny,0) IsDeny,isnull(d.IsSelf,0) IsSelf,d.DataPermissionId,d.FuncPermissionID
                             ,e.Priority DataPermissionPriority
                             FROM CPM_Function a INNER JOIN  CPM_Module_Function  b ON a.FunctionID = b.FunctionID  
                             INNER JOIN CPM_Module c on b.ModuleID=c.ModuleID ");
            builder.Append(!string.IsNullOrEmpty(scopeWhere) ? ("AND c." + scopeWhere + " ") : string.Empty);
            builder.Append(" LEFT JOIN  (");

            //使用用户权限
            if (enableuserpermission) 
            {
                builder2.Append(@" SELECT ModuleID,FunctionID,IsAllow,IsDeny,IsSelf=1,DataPermissionId,FuncPermissionID
                                    FROM CPM_FuncPermission
                                    WHERE UserID = @UserID ");
            }
            List<Role> roleByUser = new RoleDAO().GetRoleByUser(userId, string.Empty);
            
            if (roleByUser.Count > 0)
            {
                //获取用户角色的查询字符串
                string roleThen = " CASE RoleID ";
                string rolePath = "\\";
                List<string> roleIds = new List<string>();
                roleByUser.ForEach(delegate(Role r)
                {
                    roleIds.Add(r.RoleID.ToString());
                    rolePath = rolePath + r.ParentPath + "\\";
                    object obj1 = roleThen;
                    roleThen = string.Concat(new object[] { obj1, " WHEN ", r.RoleID, " THEN 1 " });
                });
                roleThen = roleThen + "ELSE 0 END ";
              
                if (builder2.Length > 0)
                {
                    builder2.Append(" UNION ALL ");
                }
                //如果不是启动了用户权限,则 ifself 为0,否则如果是本角色,则使用1代替
                builder2.AppendLine(" SELECT ModuleID,FunctionID,IsAllow,IsDeny,IsSelf = " + (!enableuserpermission ? roleThen : " 0 ") + ",DataPermissionId,FuncPermissionID"); 
                builder2.AppendLine(" FROM CPM_FuncPermission ");
                if (enableinheritpermission)
                {
                    builder2.AppendLine(" WHERE CHARINDEX('\'+ CAST(RoleID AS NVARCHAR(50)) + '\','" + rolePath + "') > 0");
                    //如果不是启动用户权限，则使用角色权限
                    builder2.AppendLine(" AND (IsAllow = 1 " + (!enableuserpermission ? (" OR RoleID IN (" + string.Join(",", roleIds.ToArray()) + ") ") : string.Empty) + ") ");
                }
                else
                {
                    builder2.AppendLine((!enableuserpermission ? (" WHERE RoleID IN (" + string.Join(",", roleIds.ToArray()) + ") ") : string.Empty) + ") "); 
                }
              
            }

            //启用组的权限
            if (enablegrouppermission)
            {
                List<Group> groupByUser = new GroupDAO().GetGroupByUser(userId);
                if (groupByUser.Count > 0)
                {
                    string groupThen = " CASE GroupID ";
                    string groupPath = "\\";
                    List<string> groupIds = new List<string>();
                    groupByUser.ForEach(delegate(Group g)
                    {
                        groupIds.Add(g.GroupID.ToString());
                        groupPath = groupPath + g.ParentPath + "\\";
                        object obj1 = groupThen;
                        groupThen = string.Concat(new object[] { obj1, " WHEN ", g.GroupID, " THEN 1 " });
                    });
                    groupThen = groupThen + "ELSE 0 END ";
                   
                    if (builder2.Length > 0)
                    {
                        builder2.Append(" UNION ALL ");
                    }
                    //如果不是启动了用户权限,则 ifself 为0,否则如果是本组,则使用1代替
                    builder2.AppendLine(" SELECT ModuleID,FunctionID,IsAllow,IsDeny,IsSelf = " + (!enableuserpermission ? groupThen : " 0 ") + ",DataPermissionId,FuncPermissionID");
                    builder2.AppendLine(" FROM CPM_FuncPermission ");

                    if (enableinheritpermission)
                    {
                        builder2.AppendLine(" WHERE CHARINDEX('\'+ CAST(GroupID AS NVARCHAR(50)) + '\','" + groupPath + "') > 0 ");
                        //如果不是启动用户权限，则使用组权限
                        builder2.AppendLine(" AND (IsAllow = 1 " + (!enableuserpermission ? (" OR GroupID IN (" + string.Join(",", groupIds.ToArray()) + ") ") : string.Empty) + ") ");
                    }
                    else
                    {
                        builder2.AppendLine((!enableuserpermission ? (" WHERE GroupID IN (" + string.Join(",", groupIds.ToArray()) + ") ") : string.Empty) + ") ");
                    }
                }
            }

            //如果没有任何权限
            if (builder2.Length == 0)
            {
                builder.Append(" SELECT top 0 ModuleID,FunctionID,IsAllow,IsDeny,IsSelf=0,null DataPermissionId, null FuncPermissionID FROM CPM_FuncPermission ");
            }
            else
            {
                builder.Append(builder2.ToString());
            }

            //结尾left join
            builder.Append(") d ON b.FunctionID = d.FunctionID AND b.ModuleID = d.ModuleID ");
            builder.Append("left join CPM_FunctionDataRule e ON d.DataPermissionId = e.DataPermissionId");

            var conn = DbService();
            DynamicParameters p = new DynamicParameters();
            p.Add("UserID", userId, DbType.String, null, null);

            var rst = conn.Query<UserFuncPMResult>(builder.ToString(), p);
            
            return new List<UserFuncPMResult>(rst);
        }

        /// <summary>
        /// 从数据库删除记录(事务)
        /// </summary>
        public bool Delete(IDbTransaction tran, string where)
        {
            int affectedrows = 0;

            string sql = @"DELETE FROM [CPM_FuncPermission] Where" + where;

            IDbConnection conn = tran == null ? DbService() : tran.Connection;

            try
            {
                affectedrows = conn.Execute(sql, null);
            }
            catch (DataException ex)
            {
                throw ex;
            }
           
            return Convert.ToBoolean(affectedrows);
        }
        #endregion
    }
}