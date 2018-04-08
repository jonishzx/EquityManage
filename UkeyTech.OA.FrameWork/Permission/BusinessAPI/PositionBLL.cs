namespace Clover.Permission.BLL
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Transactions;

    using Clover.Core.Collection;
    using Clover.Permission.DAO;
    using Clover.Permission.Model;
    
    /// <summary>
    /// 岗位功能业务类
    /// </summary>
    public partial class PositionBLL
    {
        private readonly PositionDAO dao = new PositionDAO();


        public PositionBLL()
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

        #region 树

        private Tree<Position> _tree;
        private static object lk = new object();

        /// <summary>
        /// 获取节点所组成的树
        /// </summary>
        /// <returns></returns>
        public Tree<Position> GetPositionTree()
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
        public Tree<Position> InitTree()
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
            conn = conn ?? Clover.Data.BaseDAO.DbService();

            List<Position> copylist = dao.GetList(conn, null, "", "");

            _tree = new Tree<Position>(copylist);
        }  
        #endregion

        public int DeletePosition(int id)
        {
            Position model = dao.GetModel(id);
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

                            var newparentid = (model.ParentID ?? 0);

                            this.dao.UpChildPosition(model.PositionID, newparentid, tran);

                            int num = this.dao.DeletePosition(model.PositionID, tran);

                            //查找子节点
                            List<TreeNode<Position>> childlist = _tree.FindChildren(model.PositionID.ToString());

                            PInitTree(conn);

                            //生成关系
                            foreach (TreeNode<Position> m in childlist)
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

        public int DeletePositions(int[] PositionIdArray)
        {
            int num = 0;
            if (PositionIdArray != null)
            {
                foreach (int num2 in PositionIdArray)
                {
                    num += this.DeletePosition(num2);
                }
            }
            return num;
        }

        public Position Get(int id)
        {
            return this.dao.GetModel(id);
        }

        public Position GetPositionByCode(string code)
        {
            return this.dao.GetPositionByCode(code);
        }

        public int InsertPosition(Position model)
        {
            using (var ts = new TransactionScope(TransactionScopeOption.RequiresNew))
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
                            model.ParentPath = GetCurrentPath(model.ParentID.Value, model.PositionID.ToString());
                        }
                        else
                        {
                            model.ParentPath = model.Id;
                        }

                        this.dao.Update(tran, model);

                        PInitTree(conn);

                        tran.Commit();
                        ts.Complete();

                        return model.PositionID;

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
   
        public List<Position> SelectPositionList(string where)
        {
            return this.dao.GetList(null, null, where, "");
        }

        public List<Position> GetPositionByUser(string userid) {
            return this.dao.GetPositionByUser(userid);
        }

        public List<Position> SelectPositionListWithOrder(string where, string order)
        {
            return this.dao.GetList(null, null, where, order);
        }

        /// <summary>
        /// 获取可用的模块
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Position> SelectEnabledRoleList(string where)
        {
            return SelectEnabledRoleListWithOrder(where, "ViewOrd");
        }

        public List<Position> SelectPositionList(int page, int pageindex, string codeorname, string groupid, string Positionid, string exceptid, out int rscount)
        {
           
            List<string> wherecd = new List<string>();
           
            if (!string.IsNullOrEmpty(codeorname))
            {
                wherecd.Add(string.Format(" (PositionCode LIKE '%{0}%' OR " +
                                          "PositionName LIKE '%{0}%' OR " +
                                          "GroupCode LIKE '%{0}%') ", codeorname));
            }

            if (!string.IsNullOrEmpty(Positionid))
            {
                wherecd.Add(string.Format(" (ParentID = {0})", Positionid));
            }

            if (!string.IsNullOrEmpty(exceptid))
            {
                wherecd.Add(string.Format(" (PositionID <> {0} AND '\\' +  ParentPath + '\\' not like '%\\{0}\\%')", exceptid));

            }

            if (!string.IsNullOrEmpty(groupid))
            {
                wherecd.Add(string.Format(" (GroupParentPath like '%{0}%')", groupid));
            }

            return this.dao.GetAllPaged(null, page, pageindex, string.Join(" and ", wherecd.ToArray()), true, "PositionLevel desc,ParentPath,ViewOrd", out rscount);
        }


        /// <summary>
        /// 获取可用的模块
        /// </summary>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public List<Position> SelectEnabledRoleListWithOrder(string where, string order)
        {
            return this.dao.GetList(null, null, !string.IsNullOrEmpty(where) ? "Status > 0 And" + where : " Status > 0", order);
        }

        public int UpdatePosition(Position model)
        {
            if (model != null)
            {
                var oldmodel = dao.GetModel(model.PositionID);

                using (var ts = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    using (var conn = Clover.Data.BaseDAO.ManualDbService())
                    {
                        IDbTransaction tran = null;
                        try
                        {
                            tran = conn.BeginTransaction();


                            if (model.ParentID.HasValue)
                                model.ParentPath = GetCurrentPath(model.ParentID.Value, model.PositionID.ToString());
                            else
                                model.ParentPath = model.PositionID.ToString();

                            this.dao.Update(tran, model);

                            PInitTree(conn);

                            //查找子节点
                            TreeNode<Position> node = _tree.FindById(model.PositionID.ToString());
                            RecUpdateParentPath(_tree.FindById(model.PositionID.ToString()), tran);

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

        #region 父子岗位设置
        /// <summary>
        /// 获取指定职位的子岗位
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<Position> GetPositionByMaster(int positionid)
        {
            return dao.GetPositionByMaster(positionid);        
        }


        /// <summary>
        /// 添加指定职位的子岗位
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int GetPositionByMaster(int positionid, params string[] subids)
        {
            return dao.AddPositionByMaster(positionid, subids);
        }

        /// <summary>
        /// 删除指定职位的子岗位
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int DeletePositionByMaster(int positionid, params string[] subids)
        {
            return dao.DeletePositionByMaster(positionid, subids);
        }
        #endregion

        private void RecUpdateParentPath(TreeNode<Position> node, IDbTransaction tran)
        {
            //生成关系

            this.dao.RefreshPositionParentPath(node.getNode().PositionID, GetCurrentPath(node.getNode().ParentID, node.getNode().PositionID.ToString()), tran);

            var childlist = node.getChildren();
            if (childlist.Count > 0)
            {
                childlist.ForEach(x =>
                {
                    RecUpdateParentPath(x, tran);
                });

            }
        }


        private string GetCurrentPath(int? parentid, string currid)
        {
            if (!parentid.HasValue || parentid.Value == -1)
                return currid;

            string tmp = getCurrentPath(parentid.Value);
            if (currid != string.Empty)
                return String.Format("\\{0}{1}", Clover.Core.Common.StringHelper.Join("\\",
                                           Core.Common.StringJoinOption.CheckStringCombieChar,
                                           tmp.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries)), currid);
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
            TreeNode<Position> node = _tree.FindById(id.ToString());
            if (node != null && node.getParent() != null)
                return String.Format("{0}\\{1}\\", getCurrentPath(node.getParent().getNode().PositionID), node.getNode().PositionID);
            else if (node != null)
                return node.getNode().PositionID.ToString();
            else
                return id.ToString();
        }

        /// <summary>
        /// 检查是否存在相同的ID
        /// </summary>
        public bool CheckExistsSameID(string Positioncode, string id)
        {
            return RoleDAO.ExistsSameAttr("CPM_Position", "PositionCode", Positioncode, "Status>=0", "PositionID", id);
        }

        /// <summary>
        /// 根据用岗位用户表表获取可用的岗位信息
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="strWhere"></param>
        /// <param name="desc"></param>
        /// <param name="orderfldname"></param>
        /// <param name="rstcount"></param>
        /// <returns></returns>
        public List<Position> GetGroupPositionAllPaged(int PageSize, int PageIndex, string codeorname, string groupid, string Positionid, out int rstcount)
        {
            List<string> wherecd = new List<string>();

            if (!string.IsNullOrEmpty(codeorname))
            {
                wherecd.Add(string.Format(" (PositionCode LIKE '%{0}%' OR " +
                                          "PositionName LIKE '%{0}%' OR " +
                                          "GroupCode LIKE '%{0}%' OR " + 
                                          "GroupName LIKE '%{0}%') ", codeorname));
            }

            if (!string.IsNullOrEmpty(Positionid))
            {
                wherecd.Add(string.Format(" (ParentID = {0})", Positionid));
            }

            if (!string.IsNullOrEmpty(groupid))
            {
                wherecd.Add(string.Format(" (GroupParentPath like '%{0}%')", groupid));
            }

            return dao.GetGroupPositionAllPaged(PageSize, PageIndex, string.Join(" and ", wherecd.ToArray()), true, "ViewOrd", out rstcount);
        }
    }
}