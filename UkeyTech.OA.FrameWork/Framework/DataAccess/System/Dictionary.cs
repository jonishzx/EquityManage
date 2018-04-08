using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using Dapper;
using Dapper.Contrib.Extensions;
using UkeyTech.WebFW.Model;
using Clover.Core.Collection;
using Clover.Core.Caching;

namespace UkeyTech.WebFW.DAO
{

    /// <summary>
    /// Dictionary 数据访问层
    /// </summary>
    public partial class DictionaryDAO : Clover.Data.BaseDAO
    {
        #region 构造函数
        public DictionaryDAO()
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
        #endregion

        readonly Clover.Core.Caching.ICacheBacker backer = StructureMap.ObjectFactory.GetInstance<ICacheBacker>();
        private static Tree<Dictionary> _tree;
        private static object lk = new object();

        #region 根据主键创建 Dictionary 数据模型实例
        /// <summary>
        /// 根据主键创建 Dictionary 数据模型实例 
        /// </summary>
        public Dictionary GetModel(string DictID)
        {
            try
            {
                if (backer.Contains(DictID))
                    return backer.Get(DictID) as Dictionary;

                var conn = DbService();
                var rst = conn.Get<Dictionary>(DictID);
                backer.Set(DictID, rst, DateTime.Now.AddHours(1));
                
                return rst;
            }
            catch (DataException ex)
            {
                throw ex;
            }
          
        }
        #endregion

        #region 更新记录

        public bool Update( Dictionary model)
        {
            return Update(null, model);
        }


        /// <summary>
        /// 更新记录到数据库(利用事务)
        /// </summary>
        public bool Update(IDbTransaction tran, Dictionary model)
        {
            bool ismanual = true;

            var conn = tran != null ? tran.Connection : ManualDbService();
            try
            {
                if (tran != null)
                {
                    ismanual = false;
                }
                else
                {
                    tran = conn.BeginTransaction();
                }

                if (!string.IsNullOrEmpty(model.ParentId))
                    model.ParentPath = getCurrentPath(model.ParentId, model.DictID);
                else
                    model.ParentPath = model.DictID.ToString();

                conn.Update(model, tran);

                PInitTreeWithTran(tran, string.Empty);


                //查找子节点
                TreeNode<Dictionary> node = _tree.FindById(model.DictID);
                RecUpdateParentPath(_tree.FindById(model.DictID), tran);

                if (ismanual)
                    tran.Commit();

                return true;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
            finally
            {
                if(ismanual)
                   CloseWithDispose(conn);
            }
         
        }


        #endregion

        #region 新增记录
      
        /// <summary>
        /// 新增记录到数据库(事务)
        /// </summary>
        public bool Insert( Dictionary model)
        {
            using (var ts = new TransactionScope(System.Transactions.TransactionScopeOption.RequiresNew))
            {
                using (var conn = Clover.Data.BaseDAO.ManualDbService())
                {
                    IDbTransaction tran = null;
                    try
                    {
                        tran = conn.BeginTransaction();

                        conn.Insert( model, true, tran);

                        //计算当前权限路径
                        if (!string.IsNullOrEmpty(model.ParentId))
                        {
                            model.ParentPath = getCurrentPath(model.ParentId, model.DictID);
                        }
                        else
                        {
                            model.ParentPath = model.Id;
                        }

                        Update(tran, model);

                        PInitTreeWithTran(tran, string.Empty);

                        tran.Commit();
                        ts.Complete();

                        return true;
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
        #endregion

        #region 删除记录

        /// <summary>
        /// 从数据库删除记录(事务)
        /// </summary>
        public bool Delete(Dictionary model)
        {
            return Delete(model, null);
        }

        /// <summary>
        /// 从数据库删除记录(事务)
        /// </summary>
        public bool Delete(Dictionary model, IDbTransaction tran)
        {
            IDbConnection conn = tran == null ? DbService() : tran.Connection;
            bool rst = false;
            try
            {
                rst = conn.Delete(model, tran);
            }
            catch (DataException ex)
            {
                throw ex;
            }
            finally
            {
                
            }

            return rst;
        }

        /// <summary>
        /// 从数据库删除记录(事务)
        /// </summary>
        public bool Delete(string DictID)
        {
            return Delete(null, DictID);
        }

        /// <summary>
        /// 从数据库删除记录(事务)
        /// </summary>
        public bool Delete(IDbTransaction tran, string DictID)
        {
            int affectedrows = 0;

            var p = new DynamicParameters();
            //获取主键
            p.Add("DictID", DictID);

            string sql = @"DELETE FROM [sys_Dictionary]
                        WHERE 	[DictID] = @DictID
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

        public void UpChildDict(string dictId, string parentDictId, IDbTransaction tran)
        {
            tran.Connection.Execute("UPDATE [sys_Dictionary] SET ParentId = @ParentDictId WHERE ParentId = @DictID",
                          new { DictID = dictId, ParentDictId = parentDictId }, tran, null, null);

        }

        private void PInitTree()
        {
            PInitTree(null, string.Empty);
        }

        private void PInitTree(string filter)
        {
            PInitTree(null, filter);
        }

        private void PInitTree(IDbConnection conn)
        {
            PInitTree(conn, string.Empty);
        }

        private void PInitTreeWithTran(IDbTransaction tran, string filter)
        {
            var copylist = GetListWithTran(tran, null, filter, "", false);

            _tree = new Tree<Dictionary>(copylist);
        }      

        private void PInitTree(IDbConnection conn, string filter)
        {
            conn = conn == null ? Clover.Data.BaseDAO.DbService() : conn;

            var copylist = GetList(conn, null, filter, "", false);

            _tree = new Tree<Dictionary>(copylist);
        }      

        public void DeleteDictionary(string dictId)
        {
            var model = GetModel(dictId);
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

                            var newparentid = (!string.IsNullOrEmpty(model.ParentId) ? model.ParentId : "");

                            Delete(tran,model.DictID);

                            //查找子节点
                            List<TreeNode<Dictionary>> childlist = _tree.FindChildren(model.DictID);

                            PInitTreeWithTran(tran, string.Empty);

                            //生成关系
                            foreach (TreeNode<Dictionary> m in childlist)
                            {
                                UpChildDict(m.getNode().DictID, newparentid, tran);

                                m.getNode().ParentId = newparentid;
                                RecUpdateParentPath(m, tran);
                            }

                            PInitTreeWithTran(tran, string.Empty);

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
        }

        private string getCurrentPath(string parentid, string currid)
        {
            if (string.IsNullOrEmpty(parentid))
                return currid;

            string tmp = getCurrentPath(parentid);
            if (currid != string.Empty)
                return "\\" + Clover.Core.Common.StringHelper.Join("\\",
                    Clover.Core.Common.StringJoinOption.CheckStringCombieChar,
                    tmp.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries)) + currid;
            else
                return "\\" + Clover.Core.Common.StringHelper.ReverseString(tmp);
        }

        /// <summary>
        /// 获取当前节点的内容
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string getCurrentPath(string id)
        {
            TreeNode<Dictionary> node = _tree.FindById(id.ToString());
            if (node != null && node.getParent() != null)
                return getCurrentPath(node.getParent().getNode().DictID) + "\\" + node.getNode().DictID.ToString() + "\\";
            else
                return node.getNode().DictID.ToString();
        }

        public void RefreshDictParentPath(string dictID, string path, IDbTransaction tran)
        {
            tran.Connection.Execute("UPDATE [sys_Dictionary] SET ParentPath = @ParentPath WHERE DictID = @DictID",
                  new { DictID = dictID, ParentPath = path }, tran, null, CommandType.Text);
        }

        private void RecUpdateParentPath(TreeNode<Dictionary> node, IDbTransaction tran)
        {
            //生成关系

            RefreshDictParentPath(node.getNode().DictID, getCurrentPath(node.getNode().ParentId, node.getNode().DictID), tran);

            var childlist = node.getChildren();
            if (childlist.Count > 0)
            {
                childlist.ForEach(x =>
                {
                    RecUpdateParentPath(x, tran);
                });

            }
        }

       
        #endregion

        #region 查询，返回自定义类
        /// <summary>
        /// 查询所有记录，并排序
        /// </summary>
        public List<Dictionary> GetAll(string orderBy)
        {
            return GetList(null, null, string.Empty, orderBy, true);
        }

        /// <summary>
        /// 查询所有记录，并排序(事务)
        /// </summary>
        public List<Dictionary> GetList(IDbConnection conn, int? top, string strWhere, string orderBy, bool composeName)
        {
            if (conn == null)
                conn = DbService();

            StringBuilder strSql = new StringBuilder();
            strSql.Append("Select ");
            if (top.HasValue)
            {
                strSql.Append(" top " + top.ToString());
            }
            strSql.Append(@" 	[DictID],
	[Tag],
	[Code],
	" + (composeName ? "([Name] + '(' + isnull([DictID],'') + ')') as [Name]," : "[Name],") + 
	@"[Value],
    [ExtAttr],
	[ParentId],
	[UpdateTime],
    [ViewOrder],
	[Visible]
 ");
            strSql.Append(" FROM [sys_Dictionary] with(nolock) ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            if (orderBy != string.Empty)
                strSql.Append(" order by " + orderBy);


            var rst = conn.Query<Dictionary>(strSql.ToString(), null);

            

            return new List<Dictionary>(rst);
        }

        /// <summary>
        /// 查询所有记录，并排序(事务)
        /// </summary>
        public List<Dictionary> GetListWithTran(IDbTransaction tran, int? top, string strWhere, string orderBy, bool composeName)
        {
            var conn = tran.Connection;
            
            StringBuilder strSql = new StringBuilder();
            strSql.Append("Select ");
            if (top.HasValue)
            {
                strSql.Append(" top " + top.ToString());
            }
            strSql.Append(@" 	[DictID],
	[Tag],
	[Code],
	" + (composeName ? "([Name] + '(' + isnull([DictID],'') + ')') as [Name]," : "[Name],") +
    @"[Value],
    [ExtAttr],
	[ParentId],
	[UpdateTime],
    [ViewOrder],
	[Visible]
 ");
            strSql.Append(" FROM [sys_Dictionary] ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            if (orderBy != string.Empty)
                strSql.Append(" order by " + orderBy);


            var rst = conn.Query<Dictionary>(strSql.ToString(),tran, null);

            return new List<Dictionary>(rst);
        }

        /// <summary>
        /// 查询所有记录，并排序、分页
        /// </summary>
        public List<Dictionary> GetAllPaged(IDbConnection conn, int PageSize, int PageIndex, string strWhere, bool desc, string orderfldname)
        {
            int rstcount = 0;
            return GetAllPaged(null, PageSize, PageIndex, strWhere, desc, orderfldname, out rstcount);
        }

        /// <summary>
        /// 查询所有记录，并排序、分页,没有排序字段
        /// </summary>
        public List<Dictionary> GetAllPaged(int PageSize, int PageIndex, string strWhere, bool desc)
        {
            return GetAllPaged(null, PageSize, PageIndex, strWhere, desc, "DictID");
        }

        /// <summary>
        /// 查询所有记录，并排序、分页,没有排序字段
        /// </summary>
        public List<Dictionary> GetAllPaged(int PageSize, int PageIndex, string strWhere, bool desc, out int rstcount)
        {
            return GetAllPaged(null, PageSize, PageIndex, strWhere, desc, "DictID", out rstcount);
        }

        /// <summary>
        /// 查询所有记录，并排序、分页(事务)
        /// </summary>
        public List<Dictionary> GetAllPaged(IDbConnection conn, int PageSize, int PageIndex, string strWhere, bool desc, string orderfldname, out int rstcount)
        {
            return Clover.Data.BaseDAO.GetList<Dictionary>("[sys_Dictionary]", "DictID", PageSize, PageIndex, strWhere, desc, orderfldname, out rstcount);
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
            string strSQL = "select count(*) from [sys_Dictionary]";
            strSQL = string.IsNullOrEmpty(strWhere) ? strSQL : strSQL + " where " + strWhere;
            var conn = DbService();
            IEnumerable<int> rst = conn.Query<int>(strSQL, null);
            
            return new List<int>(rst)[0];

        }
        #endregion

        #region 其他自定义方法

        public void InitTree()
        {
            lock (lk)
            {
                _tree = null;
            }
            PInitTree();
        }

        /// <summary>
        /// 检查是否存在相同的DictID
        /// </summary>
        public bool CheckExistsSameID(string name, string DictID)
        {
            return ExistsSameAttr("sys_Dictionary", "Name", name, "", "DictID", DictID);
        }


        /// <summary>
        /// 查询所有记录，并排序
        /// </summary>
        public List<Dictionary> GetListByTag(string tag, string where)
        {
            string tagwhere = "Tag in ('" + string.Join("','", tag.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) + "')";

            if(string.IsNullOrEmpty(where))
                where = tagwhere;
            else
                where = where + " AND " + tagwhere;
            int rstcount = 0;
            return GetAllPaged(int.MaxValue, 1, where, true, out rstcount);
        }

        /// <summary>
        /// 查询所有记录，并排序
        /// </summary>
        public List<Dictionary> GetList(string where)
        {
            int rstcount = 0;
            return GetAllPaged(int.MaxValue, 1, where, true, out rstcount);
        }

        
        /// <summary>
        /// 查询所有记录，并排序
        /// </summary>
        public List<Dictionary> GetListByParent(string parent, string where)
        {
            if (string.IsNullOrEmpty(where))
                where = "ParentID = '" + parent + "'";
            else
                where = where + " AND ParentID = '" + parent + "'";
            
            int rstcount = 0;
            return GetAllPaged(int.MaxValue, 1, where, true, out rstcount);
        }

        private Tree<Dictionary> GetValidDictTree(string filter)
        {
            List<Dictionary> copylist = GetList(null, null, filter, "", false);

            //查找不显示的父节点
            List<string> hiddenroots = new List<string>(10);
            copylist.ForEach(delegate(Dictionary it)
            {
                if (!string.IsNullOrEmpty(it.ParentId))
                    hiddenroots.Add(it.DictID.ToString());
            });

            //删除不可见列表
            copylist.RemoveAll(delegate(Dictionary it)
            {
                return it.Visible <= 0 || hiddenroots.Contains((it.ParentId).ToString());
            });

            return new Tree<Dictionary>(copylist);
        }

        public Tree<Dictionary> GetDictTreeWithItems(string filter)
        {
            List<Dictionary> copylist = GetList(null, null, filter, "", false);
            var itemdao = new DictItemDAO();
            
            var dictids = (from x in copylist 
                select x.DictID).ToList();

            
            foreach (var dictid in dictids.ToList()) {
                var items = itemdao.GetListByDictID(dictid);
                foreach (var it in items) {
                    copylist.Add(new Dictionary() {
                        DictID = it.DictID + "_" + it.Code,
                        ExtAttr = it.Code,
                        Name = it.Name, 
                        ParentId = it.DictID
                    });
                }
            }

            return new Tree<Dictionary>(copylist);
        }

        public Tree<Dictionary> GetDictTreeWithItems()
        {
            return GetDictTreeWithItems(string.Empty);
        }

        public Tree<Dictionary> GetDictTree(string filter)
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

        public Tree<Dictionary> GetDictTree()
        {
            return GetDictTree(string.Empty);
        }

        /// <summary>
        /// 获取具有SQL名称的字典信息
        /// </summary>
        /// <param name="dictid"></param>
        /// <returns></returns>
        //public List<SYSDataColumn> GetSQLDictColumns(string dictid)
        //{
        //    string rexg1 = "(\\[|\\()(\\w|\\W)+(\\[|\\))(\\W)+((as|AS])(\\W)+)?(\\w)+(?=,)|(\\[|\\()(\\w|\\W)+(\\[|\\))(\\W)+((as|AS])(\\W)+)?(\\w)+(?=from)";
        //    string rexg2 = "(\\[|\\()(\\w|\\W)+(\\[|\\))+|((as|AS])(\\W)+)?";
        //    string rexg3 = "(\\[|\\()(\\w|\\W)+(\\[|\\))+";
        //    List<SYSDataColumn> rst = new List<SYSDataColumn>(5);

        //    var dict = GetModel(dictid);
        //    if (dict != null && !string.IsNullOrEmpty(dict.SqlCmd))
        //    {
        //        MatchCollection ms = Regex.Matches(dict.SqlCmd, rexg1);
        //        foreach (Match m in ms)
        //        {
        //            if (m.Value.IndexOf("=") >= 0 || m.Value.IndexOf(">") >= 0 || m.Value.IndexOf("<") >= 0)
        //                continue;

        //            SYSDataColumn col = new SYSDataColumn();
        //            col.Caption = Regex.Replace(m.Value, rexg2, "").TrimStart().TrimEnd();
        //            string colfield = Regex.Replace(Regex.Match(m.Value, rexg3).Value, "\\[|\\]|\\(|\\)", "");
        //            if (colfield.IndexOf(" ") > 0)
        //            {
        //                col.ColumnName = col.Caption;
        //            }
        //            else
        //                col.ColumnName = Regex.Match(m.Value, rexg3).Value.Replace("[", "").Replace("]", "");
        //            rst.Add(col);
        //        }
        //    }

        //    return rst;
        //}

        public string GetSQLDictTextByValue(string dictid, string value)
        {
            return GetSQLDictTextByValue(null, dictid, null, value);
        }
		
		public string GetSQLDictTextByValue(Clover.Web.Core.IWebContext context, string dictid, string value)
        {
            return GetSQLDictTextByValue(context, dictid, null, value);
        }
		
        public string GetSQLDictTextByValue(Clover.Web.Core.IWebContext context, string dictid, Dictionary<string, string> param , string value)
        {
            string rst = string.Empty;

            if (string.IsNullOrEmpty(value))
                return rst;

            var dict = GetModel(dictid);

            if(dict == null)
                throw new Exception("无效的字典代码，请检查字段代码:" + dictid + " 是否存在数据库中");
            
            string rexg1 = "\\[(\\w)+\\](\\W)+((as|AS])(\\W)+)?(\\w)+";
            string rexg3 = "(\\[(\\w)+\\])+";
            string sqlcmd = dict.SqlCmd;

            if (dict != null && !string.IsNullOrEmpty(dict.SqlCmd))
            {
                MatchCollection ms = Regex.Matches(dict.SqlCmd, rexg1);
                string idfld = "ID";

                for (var i = 0; i < ms.Count; i++ )
                {
                    //if (i == 0) {
                    //    idfld = Regex.Match(ms[0].Value, rexg3).Value;
                    //}
                    if (ms[i].Value.IndexOf("=") >= 0 || ms[i].Value.IndexOf(">") >= 0 || ms[i].Value.IndexOf("<") >= 0)
                        continue;

                    sqlcmd = sqlcmd.Replace(ms[i].Value, Regex.Match(ms[i].Value, rexg3).Value);

                }

                var vals = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string addwhere = " ID in ('" + string.Join("','", vals) + "')";

                int rowscount = 0;
                string cmd = string.Empty;
                if (param == null || param.Count == 0)
                {
                    cmd = "(" +
                       Clover.Data.BaseDAO.ParseSQLCommand(context, dict.SqlCmd, Clover.Permission.BLL.FunctionDataRuleBLL.GetFDRuleStr) + ") t";
                }
                else
                {
                    cmd = "(" +
                    Clover.Data.BaseDAO.ParseSQLCommand(context, dict.SqlCmd, param, Clover.Permission.BLL.FunctionDataRuleBLL.GetFDRuleStr, null) + ") t";
                }
                var data = Clover.Data.BaseDAO.GetList(cmd, idfld, int.MaxValue, 1, addwhere, false, "", out rowscount);

             
                if (data != null && data.Tables.Count > 0)
                {
                    //获取真实为名称的字段
                    var textcolumn = "名称";
                    foreach(DataColumn dc in data.Tables[0].Columns)
                    {
                        if (dc.ColumnName.Trim().CompareTo(textcolumn) == 0) {
                            textcolumn = dc.ColumnName;
                            break;
                        }
                    }
                    //获取真实的ID对应的名称
                    var sb = new StringBuilder();
                    foreach (DataRow dr in data.Tables[0].Rows)
                    {
                        sb.Append(dr[textcolumn].ToString() + ",");
                    }
                    if(sb.Length > 0)
                        sb.Remove(sb.Length-1 ,1);

                    rst = sb.ToString();
                }
            }
            return rst;
        }

        public DataTable GetSQLDictData(string dictid, int pagesize, int pageindex, out int rowscount)
        {
            return GetSQLDictData(null, dictid, pagesize, pageindex, out rowscount);
        }

        /// <summary>
        /// 获取预览数据
        /// </summary>
        /// <param name="selecttypeid"></param>
        /// <returns></returns>
        public DataTable GetSQLDictData(Clover.Web.Core.IWebContext context, string dictid, int pagesize, int pageindex, out int rowscount)
        {
            var dict = GetModel(dictid);
            if (dict == null)
            {
                rowscount = 0;
                
                return null;
            }
            rowscount = 0;
            string rexg1 = "\\[(\\w)+\\](\\W)+((as|AS])(\\W)+)?(\\w)+";
            string rexg3 = "(\\[(\\w)+\\])+";
            string sqlcmd = dict.SqlCmd;

            if (dict != null && !string.IsNullOrEmpty(dict.SqlCmd))
            {
                MatchCollection ms = Regex.Matches(dict.SqlCmd, rexg1);
                string idfld = "ID";

                for (var i = 0; i < ms.Count; i++ )
                {
                    if (i == 0) {
                        idfld = Regex.Match(ms[0].Value, rexg3).Value;
                    }
                    if (ms[i].Value.IndexOf("=") >= 0 || ms[i].Value.IndexOf(">") >= 0 || ms[i].Value.IndexOf("<") >= 0)
                        continue;

                    sqlcmd = sqlcmd.Replace(ms[i].Value, Regex.Match(ms[i].Value, rexg3).Value);

                }

                var data = Clover.Data.BaseDAO.GetList("(" + Clover.Data.BaseDAO.ParseSQLCommand(context, sqlcmd) + ") t", idfld, pagesize, pageindex, "", false, "", out rowscount);

                if (data != null && data.Tables.Count > 0)
                    return data.Tables[0];
                else
                    return null;
            }
            return null;         
        }

        #endregion
    }
}
