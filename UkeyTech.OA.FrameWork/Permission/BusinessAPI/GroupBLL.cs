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
    /// 组功能功能业务类
    /// </summary>
    public partial class GroupBLL
    {
        private readonly GroupDAO dao = new GroupDAO();


        public GroupBLL()
        {
            if (_tree == null)
            {
                lock (lk)
                {
                    if (_tree == null)
                        PInitTree("");
                }
            }
        }

        public int DeleteGroup(int id)
        {
            Group model = dao.GetModel(id);
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

                            this.dao.UpChildren(model.GroupID, newparentid, tran);

                            int num = this.dao.DeleteGroup(model.GroupID, tran);

                            //查找子节点
                            List<TreeNode<Group>> childlist = _tree.FindChildren(model.GroupID.ToString());

                            PInitTree(conn, "");

                            //生成关系
                            foreach (TreeNode<Group> m in childlist)
                            {
                                m.getNode().ParentID = newparentid;
                                RecUpdateParentPath(m, tran);
                            }

                            PInitTree(conn, "");

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

        public int DeleteGroups(int[] GroupIdArray)
        {
            int num = 0;
            if (GroupIdArray != null)
            {
                foreach (int num2 in GroupIdArray)
                {
                    num += this.DeleteGroup(num2);
                }
            }
            return num;
        }

        public Group Get(int id)
        {
            return this.dao.GetModel(id);
        }

        public Group GetGroupByCode(string code)
        {
            return this.dao.GetGroupByCode(code);
        }


        #region 树

        private Tree<Group> _tree;
        private static object lk = new object();

        /// <summary>
        /// 获取节点所组成的树
        /// </summary>
        /// <returns></returns>
        public Tree<Group> GetGroupTree(string where)
        {
            if (_tree == null)
            {
                lock (lk)
                {
                    if (_tree == null)
                        PInitTree(where);
                }
            }

            return _tree;
        }


        public Tree<Group> GetGroupTreeInstance(string where)
        {
            var conn = Clover.Data.BaseDAO.DbService();

            List<Group> copylist = dao.GetList(conn, null, where, " GroupID ");
         
            return new Tree<Group>(copylist);
        }

        /// <summary>
        /// 获取节点所组成的树
        /// </summary>
        /// <returns></returns>
        public Tree<Group> GetGroupTreeByWhere(string where)
        {
            PInitTree(where);
            return _tree;
        }

        /// <summary>
        /// 获取节点所组成的树
        /// </summary>
        /// <returns></returns>
        public Tree<Group> InitTree()
        {
            if (_tree == null)
            {
                lock (lk)
                {
                    if (_tree == null)
                        PInitTree("");
                }
            }

            return _tree;
        }

        private void PInitTree(string where)
        {
            PInitTree(null,where);
        }

        private void PInitTree(IDbConnection conn, string where)
        {
            conn = conn == null ? Clover.Data.BaseDAO.DbService() : conn;

            List<Group> copylist = dao.GetList(conn, null, where, " GroupID ");

            _tree = new Tree<Group>(copylist);
        }         
        #endregion

        public int InsertGroup(Group model)
        {
            using (var ts = new TransactionScope(System.Transactions.TransactionScopeOption.RequiresNew))
            {
                using(var conn = Clover.Data.BaseDAO.ManualDbService())
                {
                    IDbTransaction tran = null;
                    try
                    {
                        tran = conn.BeginTransaction();

                        this.dao.Insert(tran, model);

                        //计算当前权限路径
                        if (model.ParentID.HasValue)
                        {
                            model.ParentPath = GetCurrentPath(model.ParentID.Value, model.GroupID.ToString());
                        }
                        else
                        {
                            model.ParentPath = model.Id;
                        }

                        this.dao.Update(tran, model);

                        PInitTree(conn,"");

                        tran.Commit();
                        ts.Complete();

                        return model.GroupID;

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
            TreeNode<Group> node = _tree.FindById(id.ToString());
            if (node != null && node.getParent() != null)
                return getCurrentPath(node.getParent().getNode().GroupID) + "\\" + node.getNode().GroupID.ToString() + "\\";
            else if (node != null)
                return node.getNode().GroupID.ToString();
            else
                return id.ToString();
        }


        public List<Group> SelectGroupList(string where)
        {
            return this.dao.GetList(null, null, where, "");
        }

        public List<Group> SelectGroupListWithOrder(string where, string order)
        {
            return this.dao.GetList(null, null, where, order);
        }

        /// <summary>
        /// 获取可用的模块
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Group> SelectEnabledRoleList(string where)
        {
            return SelectEnabledRoleListWithOrder(where, "ViewOrd");
        }

        public List<Group> GetGroupByUser(string userid) {
            return this.dao.GetGroupByUser(userid);
        }

        public List<Group> SelectGroupList(int page, int pageindex, string codeorname,string status, string parentid, out int rscount)
        {
            string where = string.Empty;

            List<string> wherecd = new List<string>();
           
            if (!string.IsNullOrEmpty(codeorname))
            {
                wherecd.Add(string.Format(" (GroupCode LIKE '%{0}%' OR GroupName LIKE '%{0}%') ", codeorname));
            }

            if (!string.IsNullOrEmpty(status))
            {
                wherecd.Add(string.Format(" (status = '{0}') ", status));
            }

            if (!string.IsNullOrEmpty(parentid))
            {
                wherecd.Add(string.Format(" ((ParentID = {0} OR GroupID = {0}) or (ParentPath like '%\\{0}\\%'))", parentid));
            }

            return this.dao.GetAllPaged(null, page, pageindex, string.Join(" and ", wherecd.ToArray()), true, "ParentPath,ViewOrd", out rscount);
        }


        /// <summary>
        /// 获取可用的模块
        /// </summary>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public List<Group> SelectEnabledRoleListWithOrder(string where, string order)
        {
            return this.dao.GetList(null, null, !string.IsNullOrEmpty(where) ? "Status > 0 And" + where : " Status > 0", order);
        }

        public int UpdateGroup(Group model)
        {          
            if (model != null)
            {
                Group oldmodel = dao.GetModel(model.GroupID);
                
                using (var ts = new TransactionScope(System.Transactions.TransactionScopeOption.RequiresNew))
                {
                    using (var conn = Clover.Data.BaseDAO.ManualDbService())
                    {
                        IDbTransaction tran = null;
                        try
                        {
                            tran = conn.BeginTransaction();


                            if (model.ParentID.HasValue)
                                model.ParentPath = GetCurrentPath(model.ParentID.Value, model.GroupID.ToString());
                            else
                                model.ParentPath = model.GroupID.ToString();

                            this.dao.Update(tran, model);

                            PInitTree(conn,"");

                            //查找子节点
                            TreeNode<Group> node = _tree.FindById(model.GroupID.ToString());
                            RecUpdateParentPath(_tree.FindById(model.GroupID.ToString()), tran);

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


        private void RecUpdateParentPath(TreeNode<Group> node,IDbTransaction tran) { 
              //生成关系

            this.dao.RefreshParentPath(node.getNode().GroupID, GetCurrentPath(node.getNode().ParentID, node.getNode().GroupID.ToString()), tran);

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
        public bool CheckExistsSameID(string groupcode, string id)
        {
            return RoleDAO.ExistsSameAttr("CPM_Group", "GroupCode", groupcode, "Status>=0", "GroupID", id.ToString());
        }
    }
}