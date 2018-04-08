namespace Clover.Permission.BLL
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;    
    using System.Transactions;

    using Clover.Core.Collection;
    using Clover.Permission.DAO;
    using Clover.Permission.Model;
    
    /// <summary>
    /// 角色业务类
    /// </summary>
    public partial class RoleBLL
    {
        private readonly RoleDAO dao = new RoleDAO();

        public RoleBLL() {
            if (_tree == null)
            {
                lock (lk)
                {
                    if (_tree == null)
                        PInitTree();
                }
            }
        }

        public int DeleteRole(int id)
        {
            Role model = dao.GetModel(id);
            if (model != null)
            {
                using (var ts = new TransactionScope(System.Transactions.TransactionScopeOption.RequiresNew))
                {

                    using (var conn = Clover.Data.BaseDAO.ManualDbService())
                    {
                        {
                            IDbTransaction tran = null;
                            try
                            {
                                tran = conn.BeginTransaction();

                                var newparentid = (model.ParentID.HasValue ? model.ParentID.Value : 0);

                                this.dao.UpChildRole(model.RoleID, newparentid, tran);

                                int num = this.dao.DeleteRole(model.RoleID, tran);

                                //查找子节点
                                List<TreeNode<Role>> childlist = _tree.FindChildren(model.RoleID.ToString());

                                PInitTree(conn);

                                //生成关系
                                foreach (TreeNode<Role> m in childlist)
                                {
                                    m.getNode().ParentID = newparentid;
                                    RecUpdateParentPath(m, tran);
                                }

                                PInitTree(conn);

                                tran.Commit();
                                ts.Complete();
                                return num;
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
            }
            return 0;
        }

        public int DeleteRoles(int[] roleIds)
        {
            int num = 0;
            if (roleIds != null)
            {
                foreach (int num2 in roleIds)
                {
                    num += this.DeleteRole(num2);
                }
            }
            return num;
        }

        public Role Get(int id)
        {
            return this.dao.GetModel(id);
        }

        public List<Role> GetRoleByNoUserRef(string userId, string where)
        {
            return this.dao.GetRoleByNoUserRef(userId, where);
        }

        public Role GetRoleByCode(string code)
        {
            return this.dao.GetRoleByCode(code);
        }

        public List<Role> GetRoleByUser(string userId)
        {
            return this.dao.GetRoleByUser(userId, string.Empty);
        }

        #region 树

        private Tree<Role> _tree;
        private static object lk = new object();

        /// <summary>
        /// 获取节点所组成的树
        /// </summary>
        /// <returns></returns>
        public Tree<Role> GetRoleTree()
        {
            if (_tree == null)
            {
                lock (lk)
                {
                    if (_tree == null)
                        PInitTree();
                }
            }

            return _tree;
        }

        /// <summary>
        /// 获取节点所组成的树
        /// </summary>
        /// <returns></returns>
        public Tree<Role> InitTree()
        {
            lock (lk)
            {
                PInitTree();
            }

            return _tree;
        }

        private void PInitTree(IDbConnection conn)
        {
            conn = conn == null ? Clover.Data.BaseDAO.DbService() : conn;
            List<Role> copylist = dao.GetList(conn, null, "", "");

            _tree = new Tree<Role>(copylist);
        }     

        private void PInitTree()
        {
            PInitTree(null);
        }         
        #endregion

        public int InsertRole(Role model)
        {
            using (var ts = new TransactionScope(System.Transactions.TransactionScopeOption.RequiresNew))
            {
                using (var conn = Clover.Data.BaseDAO.ManualDbService())
                {
                    IDbTransaction tran = null;
                    try
                    {
                        tran = conn.BeginTransaction();

                        this.dao.Insert(tran, model);

                        //计算当前权限路径
                        if (model.ParentID.HasValue)
                        {
                            model.ParentPath = GetCurrentPath(model.ParentID.Value, model.RoleID.ToString());
                        }
                        else
                        {
                            model.ParentPath = model.Id;
                        }

                        this.dao.Update(tran, model);

                        PInitTree(conn);

                        tran.Commit();
                        ts.Complete();

                        return model.RoleID;

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

        private string GetCurrentPath(int? parentid, string currid)
        {
            if (!parentid.HasValue || parentid.Value == -1)
                return currid;

            string tmp = getCurrentPath(parentid.Value);
            if (currid != string.Empty)
                return "\\" + Clover.Core.Common.StringHelper.Join("\\",
                    Core.Common.StringJoinOption.CheckStringCombieChar,
                    tmp.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries)) + currid;
            else
                return "\\" + Clover.Core.Common.StringHelper.ReverseString(tmp);
        }

        /// <summary>
        /// 获取当前节点的内容
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string getCurrentPath(int roleid)
        {
            TreeNode<Role> node = _tree.FindById(roleid.ToString());
            if (node!= null)
            {
                if (node.getParent() != null)
                    return getCurrentPath(node.getParent().getNode().RoleID)+ "\\" +  node.getNode().RoleID.ToString() ;
                else
                    return node.getNode().RoleID.ToString();
            }
            return string.Empty;
        }

        public List<Role> SelectRoleList(string where)
        {
            return this.dao.GetList(null, null, where, "");
        }
      
        public List<Role> SelectRoleListWithOrder(string where, string order)
        {
            return this.dao.GetList(null, null, where, order);
        }

        public List<Role> SelectRoleList(string codeorname, string parentid)
        {
            string where = string.Empty;


            List<string> wherecd = new List<string>();

            if (!string.IsNullOrEmpty(codeorname))
            {
                wherecd.Add(string.Format(" (RoleCode LIKE '%{0}%' OR RoleName LIKE '%{0}%') ", codeorname));
            }

            if (!string.IsNullOrEmpty(parentid))
            {
                wherecd.Add(string.Format(" ((ParentID = {0} OR RoleID = {0}) or (ParentPath like '%\\{0}\\%'))", parentid));
            }

            return this.dao.GetList(null, null, string.Join(" and ", wherecd.ToArray()), "");
        }

        public List<Role> SelectRoleList(string code, string name, string parentid)
        {
            string where = string.Empty;


            List<string> wherecd = new List<string>();

          
            if (!string.IsNullOrEmpty(code))
            {
                wherecd.Add(string.Format(" (RoleCode LIKE '%{0}%') ", code));
            }

            if (!string.IsNullOrEmpty(name))
            {
                wherecd.Add(string.Format(" (RoleName LIKE '%{0}%') ", name));
            }

            if (!string.IsNullOrEmpty(parentid))
            {
                wherecd.Add(string.Format(" ((ParentID = {0} OR RoleID = {0}) or (ParentPath like '%\\{0}\\%'))", parentid));
            }

            return this.dao.GetList(null, null, string.Join(" and ", wherecd.ToArray()), "");
        }

        public List<Role> SelectRoleList(int pagesize, int pageindex ,string codeorname, string parentid, out int rscount)
        {
            string where = string.Empty;

            if (!string.IsNullOrEmpty(codeorname))
            {
                where = string.Format(" (RoleCode LIKE '%{0}%' OR RoleName LIKE '%{0}%') ", codeorname);
            }

            if (!string.IsNullOrEmpty(parentid))
            {
                if (where != string.Empty)
                    where += " and ";

                where += string.Format("(ParentID = {0} OR RoleID = {0})", parentid);
            }

            return this.dao.GetAllPaged(null, pagesize, pageindex, where, true, "", out rscount);
        }


        /// <summary>
        /// 获取可用的角色
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Role> SelectEnabledRoleList(string where)
        {
            return SelectEnabledRoleListWithOrder(where, "ViewOrd");
        }

       /// <summary>
        /// 获取可用的角色
       /// </summary>
       /// <param name="where"></param>
       /// <param name="order"></param>
       /// <returns></returns>
        public List<Role> SelectEnabledRoleListWithOrder(string where, string order)
        {
            return this.dao.GetList(null, null, !string.IsNullOrEmpty(where) ? "Status > 0 And" + where : " Status > 0", order);
        }


        public int UpdateRole(Role model)
        {
            model.UpdateTime = DateTime.Now;
            if (model != null)
            {             
                using (var ts = new TransactionScope(System.Transactions.TransactionScopeOption.RequiresNew))
                {

                    using (var conn = Clover.Data.BaseDAO.ManualDbService())
                    {
                        IDbTransaction tran = null;
                        try
                        {
                            tran = conn.BeginTransaction();


                            if (model.ParentID.HasValue)
                                model.ParentPath = GetCurrentPath(model.ParentID.Value, model.RoleID.ToString());
                            else
                                model.ParentPath = model.RoleID.ToString();

                            this.dao.Update(tran, model);

                            PInitTree(conn);

                            //查找子节点
                            TreeNode<Role> node = _tree.FindById(model.RoleID.ToString());
                            RecUpdateParentPath(_tree.FindById(model.RoleID.ToString()), tran);

                            tran.Commit();
                            ts.Complete();

                            return 1;
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
            return 0;
        }

        private void RecUpdateParentPath(TreeNode<Role> node,IDbTransaction tran) { 
              //生成关系

            this.dao.RefreshRoleParentPath(node.getNode().RoleID, GetCurrentPath(node.getNode().ParentID.Value, node.getNode().RoleID.ToString()), tran);

            var childlist = node.getChildren();
            if (childlist.Count > 0)
            {
                childlist.ForEach(x=>{
                    RecUpdateParentPath(x, tran);
                });

            }
      
                        
        }


        /// <summary>
        /// 检查是否存在相同的ID
        /// </summary>
        public bool CheckExistsSameID(string rolecode, string id)
        {
            return RoleDAO.ExistsSameAttr("CPM_Role", "RoleCode", rolecode, "Status>=0", "RoleID", id.ToString());
        }

    }
}