namespace Clover.Permission.BLL
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Transactions;

    using Clover.Config.CPM;
    using Clover.Permission.DAO;
    using Clover.Permission.Model;
using System.Text;

    /// <summary>
    /// 功能权限业务类
    /// </summary>
    public partial class FuncPermissionBLL
    {
        private readonly FuncPermissionDAO dao = new FuncPermissionDAO();

        public List<UserFuncPMResult> GetFuncPermission(PermissionOwner owner, string ownerValue, FilterScope scope, string scopeValue)
        {
            List<UserFuncPMResult> rst = null;
           
            if (!string.IsNullOrEmpty(ownerValue))
            {
                string str = string.Empty;
                if (!((scope == FilterScope.All) || string.IsNullOrEmpty(scopeValue)))
                {
                    str = String.Format("{0} = {1}", Clover.Permission.Util.GetScopeFieldName(scope), scopeValue);
                }
                switch (owner)
                {
                    case PermissionOwner.User:
                        rst = this.dao.GetUserFunctions(ownerValue, str
                            , PermissionConfig.Config.EnableUserPermission
                            , PermissionConfig.Config.EnableDenyPermission,
                            PermissionConfig.Config.EnableInheritPermission);
                        break;

                    case PermissionOwner.Role:
                        rst = this.dao.GetRoleFunc(int.Parse(ownerValue), str, PermissionConfig.Config.EnableInheritPermission);
                        break;

                    case PermissionOwner.Group:
                        rst = this.dao.GetGroupFunc(int.Parse(ownerValue), str, PermissionConfig.Config.EnableInheritPermission);
                        break;
                }
            }
            return Util.CombinePrivilege(rst);

        }

        /// <summary>
        /// 获取所有权限(系统管理员指定)
        /// </summary>
        /// <returns></returns>
        public List<UserFuncPMResult> GetAllFuncPermission()
        {
            return this.dao.GetAllModuleFunc();
        }

        /// <summary>
        /// 更新数据权限
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ownerTitle"></param>
        /// <param name="ownerValue"></param>
        /// <returns></returns>
        public bool UpdateFuncPermission(FuncPermission model, string ownerTitle, string ownerValue)
        {
              PermissionOwner owner = Clover.Permission.Common.TranPermissionOwner(ownerTitle);
              return dao.SaveOrUpdateFuncDataPermission(model, owner, ownerValue);
        }

        /// <summary>
        /// 克隆权限
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ownerTitle"></param>
        /// <param name="ownerValue"></param>
        /// <returns></returns>
        public void CloneFuncPermission(string ownerTitle, string ownerValue, string targetValue)
        {
            PermissionOwner owner = Clover.Permission.Common.TranPermissionOwner(ownerTitle);
            dao.CloneFuncPermission(owner, ownerValue, targetValue);
        }

        /// <summary>
        /// 获取权限管理员所有权限(权限管理员指定)
        /// </summary>
        /// <returns></returns>
        public FuncPermission GetFuncPermission(int FuncPermissionID)
        {
            return this.dao.GetModel(FuncPermissionID);
        }

        /// <summary>
        /// 获取权限管理员所有权限(权限管理员指定)
        /// </summary>
        /// <returns></returns>
        public List<UserFuncPMResult> GetPMFuncPermission(int roleId)
        {
            return this.dao.GetPMAllModuleFunc(roleId);
        }

        public List<UserFuncPMResult> GetModuleFuncPermission(string userid, string moduleCode)
        {
            Module moduleByCode = new ModuleDAO().GetModuleByCode(moduleCode.Trim());

            if (((userid != null) && (moduleByCode != null)) && (new PMSystemDAO().GetModel(moduleByCode.SystemID.Value) != null))
            {
                return this.dao.GetModuleFunc(userid, moduleByCode.ModuleID,
                    PermissionConfig.Config.EnableUserPermission,
                    PermissionConfig.Config.EnableGroupPermission,
                    PermissionConfig.Config.EnableDenyPermission,
                    PermissionConfig.Config.EnableInheritPermission);
            }
            return null;
        }

        public List<UserFuncPMResult> GetSystemFuncPermission(string userid, string systemCode)
        {

            PMSystem systemByCode = new PMSystemDAO().GetPMSystemByCode(systemCode.Trim());

            if ((userid != null) && (systemByCode != null))
            {
                return this.dao.GetSystemFunc(userid, systemByCode.SystemID,
                    PermissionConfig.Config.EnableUserPermission,
                    PermissionConfig.Config.EnableGroupPermission,
                    PermissionConfig.Config.EnableDenyPermission,
                    PermissionConfig.Config.EnableInheritPermission);
            }
            return null;
        }

        public List<UserFuncPMResult> GetSystemFuncPermission(string userid, string roleid, string systemCode)
        {

            PMSystem systemByCode = new PMSystemDAO().GetPMSystemByCode(systemCode.Trim());

            if ((userid != null) && (systemByCode != null))
            {
                return this.dao.GetSystemFunc(userid, systemByCode.SystemID,roleid,
                    PermissionConfig.Config.EnableUserPermission,
                    PermissionConfig.Config.EnableGroupPermission,
                    PermissionConfig.Config.EnableDenyPermission,
                    PermissionConfig.Config.EnableInheritPermission);
            }
            return null;
        }

        public int SetFuncPermission(int moduleId, PermissionOwner owner, string ownerValue, List<FuncPermission> list)
        {
            if (string.IsNullOrEmpty(ownerValue))
                return 0;

            string whereStrBase = string.Format(" {0} = {1} AND ModuleID = {2}",
                    Clover.Permission.Util.GetOwnerFieldName(owner), 
                    (owner == PermissionOwner.User) ? (String.Format("'{0}'", ownerValue)) : ownerValue,
                    moduleId);
            string whereStr = whereStrBase;
            //只清除不在列表中的项目
            if (list.Count > 0)
            {
                StringBuilder sb = new StringBuilder(100);
                foreach (var func in list) {
                    sb.Append(func.FunctionID + ",");
                }

                if (sb.Length > 0)
                    sb.Remove(sb.Length - 1, 1);

                whereStr += string.Format(" AND FunctionID NOT IN ({0})", sb.ToString());
            }

            using (var ts = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                using (var conn = Clover.Data.BaseDAO.ManualDbService())
                {
                    IDbTransaction tran = null;
                    try
                    {
                        tran = conn.BeginTransaction();

                        dao.Delete(tran, whereStr);
                        var oldlist = dao.GetListTran(tran, 65536, whereStrBase, "");
                        
                        foreach (FuncPermission m in list)
                        {
                          
                            FuncPermission oldItem = oldlist.Find(x => x.FunctionID == m.FunctionID && x.RoleID == m.RoleID
                                && x.ModuleID == m.ModuleID
                                && m.GroupID == x.GroupID
                                && m.UserID == x.UserID);
                            if (oldItem != null)
                            {
                                //更新原有权限项目
                                oldItem.IsAllow = m.IsAllow;
                                oldItem.IsDeny = m.IsDeny;
                                dao.Update(oldItem);
                            }
                            else
                            {
                                //插入项目
                                dao.Insert(tran, m);
                            }
                        }

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
            return 0;
        }

        #region 授权信息列表

        public DataSet FunctionAsColumnPermissionList(List<UserFuncPMResult> list, bool isView)
        {
            DataSet set = new DataSet();
            DataTable table = new DataTable();

            table.Columns.Add(new DataColumn("ModuleID"));
            table.Columns.Add(new DataColumn("ModuleName"));
         
            List<int> existslist = new List<int>();
            List<int> funcColumnlist = new List<int>();

            //动态列头
            foreach (UserFuncPMResult row in list)
            {
                if (!funcColumnlist.Contains(row.FunctionID))
                {
                    table.Columns.Add(new DataColumn(row.FunctionID.ToString()));
                    funcColumnlist.Add(row.FunctionID);
                }

                if (!(row.ModuleID == 0 || existslist.Contains(row.ModuleID)))
                {
                    DataRow newdr = table.NewRow();
                    newdr["ModuleID"] = row.ModuleID.ToString();
                    newdr["ModuleName"] = row.ModuleName;
                    table.Rows.Add(newdr);
                    existslist.Add(row.ModuleID);
                }
            }

            //按照列头赋值
            foreach(int fid in funcColumnlist){
                foreach (DataRow row in table.Rows)
                {
                    UserFuncPMResult last = list.FindLast(x=>{return x.ModuleID == int.Parse(row["ModuleID"].ToString()) 
                        && x.FunctionID == fid;});
                    
                    //查找指定模块以及功能
                    if(last != null)
                        row[fid.ToString()] = string.Format("{0};{1};{2}", last.IsAllow, last.IsDeny, last.IsSelf);
                }
            }

            set.Tables.Add(table);
            if (!isView)
            {
                return null;
            }
            return set;
        }

        private DataSet ChangePermissionListDt(DataSet ds)
        {
            DataSet set;
            DataTable table;
            DataTable table2;
            DataRow row2;
            string[] strArray;
            if (PermissionConfig.Config.EnableDenyPermission)
            {
                set = new DataSet();
                table = new DataTable();
                if (ds != null)
                {
                    table2 = ds.Tables[0];
                    table = table2.Copy();
                    table.Clear();
                    table.Columns.Add(new DataColumn("PermissionTitle"));
                    foreach (DataRow row in table2.Rows)
                    {
                        row2 = table.NewRow();
                        DataRow row3 = table.NewRow();
                        foreach (DataColumn column in table2.Columns)
                        {
                            if (!((!(column.ColumnName != "ModuleID") || !(column.ColumnName != "ModuleName")) || string.IsNullOrEmpty(row[column.ColumnName].ToString())))
                            {
                                strArray = row[column.ColumnName].ToString().Split(new char[] { ';' });
                                row2[column.ColumnName] = string.Concat(new object[] { row["ModuleID"], "|", column.ColumnName, "|", strArray[0], ";", strArray[2] });
                                row3[column.ColumnName] = string.Concat(new object[] { row["ModuleID"], "|", column.ColumnName, "|", strArray[1] });
                            }
                            else
                            {
                                row2[column.ColumnName] = row[column.ColumnName];
                                row3[column.ColumnName] = row[column.ColumnName];
                            }
                        }
                        row2["PermissionTitle"] = row2["ModuleID"] + "|Allow|允许";
                        row3["PermissionTitle"] = row2["ModuleID"] + "|Deny|拒绝";
                        table.Rows.Add(row2);
                        table.Rows.Add(row3);
                    }
                }
                set.Tables.Add(table);
                return set;
            }
            set = new DataSet();
            table = new DataTable();
            if (ds != null)
            {
                table2 = ds.Tables[0];
                table = table2.Copy();
                table.Clear();
                foreach (DataRow row in table2.Rows)
                {
                    row2 = table.NewRow();
                    foreach (DataColumn column in table2.Columns)
                    {
                        if (!((!(column.ColumnName != "ModuleID") || !(column.ColumnName != "ModuleName")) || string.IsNullOrEmpty(row[column.ColumnName].ToString())))
                        {
                            strArray = row[column.ColumnName].ToString().Split(new char[] { ';' });
                            row2[column.ColumnName] = string.Concat(new object[] { row["ModuleID"], "|", column.ColumnName, "|", strArray[0], ";", strArray[2] });
                        }
                        else
                        {
                            row2[column.ColumnName] = row[column.ColumnName];
                        }
                    }
                    row2["ModuleName"] = String.Format("{0}|Allow|{1}", row2["ModuleID"], row2["ModuleName"]);
                    table.Rows.Add(row2);
                }
            }
            set.Tables.Add(table);
            return set;
        }

        #endregion

    }
}