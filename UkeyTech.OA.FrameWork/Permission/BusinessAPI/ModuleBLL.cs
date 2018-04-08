namespace Clover.Permission.BLL
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;    
    using System.Transactions;
    using System.Linq;
    using Clover.Core.Collection;
    using Clover.Permission.DAO;
    using Clover.Permission.Model;
    
    /// <summary>
    /// 模块功能业务类
    /// </summary>
    public partial class ModuleBLL
    {
        private readonly ModuleDAO dao = new ModuleDAO();


        public ModuleBLL()
        {
            if (_tree == null)
            {
                lock (lk)
                {
                    if (_tree == null)
                        PInitTree();
                }
            }
        }

        public int DeleteModule(int id)
        {
            Module model = dao.GetModel(id);
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

                            var newparentid = (model.ParentID.HasValue ? model.ParentID.Value : 0);

                            this.dao.UpChildModule(model.ModuleID, newparentid, tran);

                            int num = this.dao.DeleteModule(model.ModuleID, tran);

                            //查找子节点
                            List<TreeNode<Module>> childlist = _tree.FindChildren(model.ModuleID.ToString());

                            PInitTree(conn);

                            //生成关系
                            foreach (TreeNode<Module> m in childlist)
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
            return 0;
        }

        public int DeleteModules(int[] ModuleIdArray)
        {
            int num = 0;
            if (ModuleIdArray != null)
            {
                foreach (int num2 in ModuleIdArray)
                {
                    num += this.DeleteModule(num2);
                }
            }
            return num;
        }

        public Module Get(int id)
        {
            return this.dao.GetModel(id);
        }

        public Module GetModuleByCode(string code)
        {
            return this.dao.GetModuleByCode(code);
        }

        public List<Module> GetSystemModules(int systemId)
        {
            return this.dao.GetSystemModules(systemId);
        }

        #region 树

        private Tree<Module> _tree;
        private static object lk = new object();

        /// <summary>
        /// 获取节点所组成的树
        /// </summary>
        /// <returns></returns>
        public Tree<Module> GetModuleTree()
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
        public Tree<Module> InitTree()
        {
            lock (lk)
            {
                PInitTree();
            }

            return _tree;
        }

        private void PInitTree()
        {
            PInitTree(null);
        }

        private void PInitTree(IDbConnection conn)
        {
            conn = conn == null ? Clover.Data.BaseDAO.DbService() : conn;

            List<Module> copylist = dao.GetList(conn, null, "", "");

            _tree = new Tree<Module>(copylist);
        }      
        #endregion

        public int InsertModule(Module model)
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
                            model.ParentPath = GetCurrentPath(model.ParentID.Value, model.ModuleID.ToString());
                        }
                        else
                        {
                            model.ParentPath = model.Id;
                        }

                        this.dao.Update(tran, model);

                        PInitTree(conn);

                        tran.Commit();
                        ts.Complete();

                        return model.ModuleID;

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
        private string getCurrentPath(int id)
        {
            TreeNode<Module> node = _tree.FindById(id.ToString());
            if (node != null && node.getParent() != null)
                return getCurrentPath(node.getParent().getNode().ModuleID) + "\\" + node.getNode().ModuleID.ToString() + "\\";
            else if (node != null)
                return node.getNode().ModuleID.ToString();
            else
                return id.ToString();
        }

        /// <summary>
        /// 获取当前节点的内容
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string getCurrentModulePath(int Moduleid)
        {
            TreeNode<Module> node = _tree.FindById(Moduleid.ToString());
            if (node!= null && node.getParent() != null)
                return node.getNode().ModuleID.ToString() + "\\" + getCurrentModulePath(node.getParent().getNode().ModuleID);
            else
                return node.getNode().ModuleID.ToString();
        }

        public List<Module> SelectModuleList(string where)
        {
            return this.dao.GetList(null, null, where, "");
        }

        public List<Module> SelectModuleList(int page, int pageindex, int? systemid, string codeorname, string parentid, out int rscount)
        {
            string where = string.Empty;

            List<string> wherecd = new List<string>();


            if (systemid.HasValue)
            {
                wherecd.Add(" SystemID = {0}" + systemid.Value);
            }

            if (!string.IsNullOrEmpty(codeorname))
            {
                wherecd.Add(string.Format("  (ModuleCode LIKE '%{0}%' OR ModuleName LIKE '%{0}%') ", codeorname));
            }

            if (!string.IsNullOrEmpty(parentid))
            {
                wherecd.Add(string.Format("((ParentID = {0} OR ModuleID = {0}) or (ParentPath like '%\\{0}\\%'))", parentid));
            }

            return this.dao.GetAllPaged(null, page, pageindex, string.Join(" and ", wherecd.ToArray()), true, "ParentPath,ParentID", out rscount);
        }

        public List<Module> SelectModuleListWithFunction(int page, int pageindex, int? systemid, string codeorname, string parentid, out int rscount)
        {

            string where = string.Empty;

            List<string> wherecd = new List<string>();


            if (systemid.HasValue)
            {
                wherecd.Add(" SystemID = {0}" + systemid.Value);
            }

            if (!string.IsNullOrEmpty(codeorname))
            {
                wherecd.Add(string.Format("  (ModuleCode LIKE '%{0}%' OR ModuleName LIKE '%{0}%') ", codeorname));
            }

            if (!string.IsNullOrEmpty(parentid))
            {
                wherecd.Add(string.Format("((ParentID = {0} OR ModuleID = {0}) or (ParentPath like '%\\{0}\\%'))", parentid));
            }

            return this.dao.GetAllWithFunctionPaged(null, page, pageindex, string.Join(" and ", wherecd.ToArray()), true, "ParentPath,ParentID", out rscount);
        }

        public List<Module> SelectModuleListWithOrder(string where, string order)
        {
            return this.dao.GetList(null, null, where, order);
        }

        /// <summary>
        /// 获取可用的模块
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Module> SelectEnabledRoleList(string where)
        {
            return SelectEnabledRoleListWithOrder(where, "ViewOrd");
        }

        /// <summary>
        /// 获取可用的模块
        /// </summary>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public List<Module> SelectEnabledRoleListWithOrder(string where, string order)
        {
            return this.dao.GetList(null, null, !string.IsNullOrEmpty(where) ? "Status > 0 And" + where : " Status > 0", order);
        }

        public int UpdateModule(Module model)
        {          
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
                                model.ParentPath = GetCurrentPath(model.ParentID.Value, model.ModuleID.ToString());
                            else
                                model.ParentPath = model.ModuleID.ToString();

                            this.dao.Update(tran, model);

                            PInitTree(conn);


                            //查找子节点
                            TreeNode<Module> node = _tree.FindById(model.ModuleID.ToString());
                            RecUpdateParentPath(_tree.FindById(model.ModuleID.ToString()), tran);

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

        private void RecUpdateParentPath(TreeNode<Module> node,IDbTransaction tran) { 
              //生成关系

            this.dao.RefreshModuleParentPath(node.getNode().ModuleID, GetCurrentPath(node.getNode().ParentID.Value, node.getNode().ModuleID.ToString()), tran);

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
        public bool CheckExistsSameID(string modulecode, string id)
        {
            return RoleDAO.ExistsSameAttr("CPM_Module", "ModuleCode", modulecode, "Status>=0", "ModuleID", id.ToString());
        }

        public List<Module> GetEnabledModules()
        {
            if (_tree == null)
            {
                lock (lk)
                {
                    if (_tree == null)
                        PInitTree();
                }
            }

            var tmplist = _tree.GetAllOrdered().Where<Module>(x => x.Visible == 1);

            return new List<Module>(tmplist);
        }

        public Tree<Module> GetEnabledModuleTree()
        {
            if (_tree == null)
            {
                lock (lk)
                {
                    if (_tree == null)
                        PInitTree();
                }
            }

            var tmplist = _tree.GetAllOrdered().Where<Module>(x => x.Visible == 1);
            var copylist = new List<Module>(tmplist);
            //查找不显示的父节点
            List<string> hiddenroots = new List<string>(10);
            copylist.ForEach(delegate(Module it)
            {
                if ((it.ParentID.HasValue || (it.ParentID == -1) || (it.ParentID == 0)) && it.Status <= 0)
                    hiddenroots.Add(it.Id);
            });

            //删除不可见列表
            copylist.RemoveAll(delegate(Module it)
            {
                return it.Status <= 0 || hiddenroots.Contains(it.ParentId);
            });

            _tree = new Tree<Module>(copylist);

            return _tree;
        }
    
    }
}