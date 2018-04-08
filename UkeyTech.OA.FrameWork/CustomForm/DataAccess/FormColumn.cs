namespace UkeyTech.WebFW.DAO
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Text;
    using Dapper;
    using Dapper.Contrib.Extensions;

    using UkeyTech.WebFW.Model;

    /// <summary>
    /// FormColumn 数据访问层
    /// </summary>
    public partial class FormColumnDAO : Clover.Data.BaseDAO
    {
        #region 构造函数
        public FormColumnDAO()
        {
        }
        #endregion

        #region 根据主键创建 FormColumn 数据模型实例
        /// <summary>
        /// 根据主键创建 FormColumn 数据模型实例 
        /// </summary>
        public FormColumn GetModel(int ID)
        {
            var conn = DbService();

            try
            {
                var rst = conn.Get<FormColumn>(ID);
                return rst;
            }
            catch (DataException ex)
            {
                throw ex;
            }
            finally
            {
                
            }
        }
        #endregion

        #region 更新记录
        /// <summary>
        /// 更新记录到数据库
        /// </summary>
        public bool Update(FormColumn model)
        {
            return Update(null, model);
        }
        /// <summary>
        /// 更新记录到数据库(利用事务)
        /// </summary>
        public bool Update(IDbTransaction tran, FormColumn model)
        {

            IDbConnection conn = tran == null ? DbService() : tran.Connection;
            bool rst = false;
            try
            {
                rst = conn.Update(model, tran);
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
        #endregion

        #region 新增记录
        /// <summary>
        /// 新增记录到数据库
        /// </summary>
        public bool Insert(FormColumn model)
        {
            return Insert(null, model);
        }

        /// <summary>
        /// 新增记录到数据库(事务)
        /// </summary>
        public bool Insert(IDbTransaction tran, FormColumn model)
        {

            IDbConnection conn = tran == null ? DbService() : tran.Connection;

            try
            {
                long id = conn.Insert(model, tran);
                model.ID = (int)id;
            }
            catch (DataException ex)
            {
                throw ex;
            }
            finally
            {
                
            }

            return true;
        }
        #endregion

        #region 删除记录

        /// <summary>
        /// 从数据库删除记录(事务)
        /// </summary>
        public bool Delete(FormColumn model)
        {
            return Delete(model, null);
        }

        /// <summary>
        /// 从数据库删除记录(事务)
        /// </summary>
        public bool Delete(FormColumn model, IDbTransaction tran)
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
        public bool Delete(int ID)
        {
            return Delete(null, ID);
        }

        /// <summary>
        /// 从数据库删除记录(事务)
        /// </summary>
        public bool Delete(IDbTransaction tran, int ID)
        {
            int affectedrows = 0;

            var p = new DynamicParameters();
            //获取主键
            p.Add("ID", ID, DbType.Int32, null, 4);

            string sql = @"DELETE FROM [CF_FormColumn]
                        WHERE 	[ID] = @ID
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
        public List<FormColumn> GetAll(string orderBy)
        {
            return GetList(null, null, string.Empty, orderBy);
        }

        /// <summary>
        /// 查询所有记录，并排序(事务)
        /// </summary>
        public List<FormColumn> GetList(IDbConnection conn, int? top, string strWhere, string orderBy)
        {
            if (conn == null)
                conn = DbService();

            StringBuilder strSql = new StringBuilder();
            strSql.Append("Select ");
            if (top.HasValue)
            {
                strSql.Append(" top " + top.ToString());
            }
            strSql.Append(@" 	[ID],
	[FormID],
	[ColName],
	[ColCaption],
	[ColType],
	[ViewOrd],
	[ColHtml],
	[Status],
	[Size],
	[FPSize],
	[IsSelectCol],
	[SelectType],
	[Required]
 ");
            strSql.Append(" FROM [CF_FormColumn] ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            if (orderBy != string.Empty)
                strSql.Append(" order by " + orderBy);


            var rst = conn.Query<FormColumn>(strSql.ToString(), null);

            

            return new List<FormColumn>(rst);
        }

        /// <summary>
        /// 查询所有记录，并排序、分页
        /// </summary>
        public List<FormColumn> GetAllPaged(IDbConnection conn, int PageSize, int PageIndex, string strWhere, bool desc, string orderfldname)
        {
            int rstcount = 0;
            return GetAllPaged(null, PageSize, PageIndex, strWhere, desc, orderfldname, out rstcount);
        }

        /// <summary>
        /// 查询所有记录，并排序、分页,没有排序字段
        /// </summary>
        public List<FormColumn> GetAllPaged(int PageSize, int PageIndex, string strWhere, bool desc)
        {
            return GetAllPaged(null, PageSize, PageIndex, strWhere, desc, "ID");
        }

        /// <summary>
        /// 查询所有记录，并排序、分页,没有排序字段
        /// </summary>
        public List<FormColumn> GetAllPaged(int PageSize, int PageIndex, string strWhere, bool desc, out int rstcount)
        {
            return GetAllPaged(null, PageSize, PageIndex, strWhere, desc, "ID", out rstcount);
        }

        /// <summary>
        /// 查询所有记录，并排序、分页(事务)
        /// </summary>
        public List<FormColumn> GetAllPaged(IDbConnection conn, int PageSize, int PageIndex, string strWhere, bool desc, string orderfldname, out int rstcount)
        {
            return Clover.Data.BaseDAO.GetList<FormColumn>("[CF_FormColumn]", "ID", PageSize, PageIndex, strWhere, desc, orderfldname, out rstcount);
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
            string strSQL = "select count(*) from [CF_FormColumn]";
            strSQL = string.IsNullOrEmpty(strWhere) ? strSQL : strSQL + " where " + strWhere;
            var conn = DbService();
            IEnumerable<int> rst = conn.Query<int>(strSQL, null);
            
            return new List<int>(rst)[0];

        }
        #endregion

        #region 其他自定义方法

          /// <summary>
        /// 查询所有记录，并排序(事务)
        /// </summary>
        public List<FormColumn> GetListByFormID(int FormID){
            return GetListByFormID(null, FormID);
        }

        /// <summary>
        /// 查询所有记录，并排序(事务)
        /// </summary>
        public List<FormColumn> GetListByFormID(IDbConnection conn, int FormID)
        {
            if (conn == null)
                conn = DbService();

            string sql = @"select * from [CF_FormColumn] where Status =1 AND FormID = " + FormID.ToString();
            var rst = conn.Query<FormColumn>(sql, null);

            

            return new List<FormColumn>(rst);
        }

        /// <summary>
        /// 检查是否存在相同的ID
        /// </summary>
        public bool CheckExistsSameID(int formID, string name, string id)
        {
            return ExistsSameAttr("[CF_FormColumn]", "ColCaption", name, " FormId=" + formID.ToString(), "ID", id);
        }

        /// <summary>
        /// 检查是否存在相同的ID
        /// </summary>
        public bool CheckExistsSameColName(int formID, string name, string id)
        {
            return ExistsSameAttr("[CF_FormColumn]", "ColName", name, " FormId=" + formID.ToString(), "ID", id);
        }

        /// <summary>
        /// 对所有符合条件的记录进行记录数计算
        /// </summary>
        public int GetMaxID(int fomrId)
        {
            string strSQL = "select max(ID) from [CF_FormColumn] WHERE FormID = @FormID";

            DynamicParameters p = new DynamicParameters();
            p.Add("FormID", fomrId);

            var conn = DbService();
            IEnumerable<int> rst = conn.Query<int>(strSQL, p);
            
            return (new List<int>(rst)[0] + 1);

        }

        /// <summary>
        /// 获取数据列
        /// </summary>
        /// <param name="selecttypeid"></param>
        /// <returns></returns>
        public List<SYSDataColumn> GetPreViewDataColumnsByCode(string selecttypecode)
        {
            DictionaryDAO dictDAO = StructureMap.ObjectFactory.GetInstance<DictionaryDAO>();
            var m = dictDAO.GetModel(selecttypecode);
            List<SYSDataColumn> colList = new List<SYSDataColumn>();

            if (m.Code == "Const")
            { //固定单选类型
                var childList = dictDAO.GetListByParent(m.DictID.ToString(), "");

                colList.Add(new SYSDataColumn("ID", "标记"));
                colList.Add(new SYSDataColumn("Name", "可选项"));

            }
            else if (m.Code == "Mutil" || m.Code == "Single")
            {
                //下拉或弹出类型
                DataTable dt = Clover.Data.BaseDAO.GetDataTable(parseSchemaSQL(m.Value));
                foreach (DataColumn dc in dt.Columns)
                {
                    colList.Add(new SYSDataColumn(dc.ColumnName, dc.ColumnName));
                }
            }

            return colList;
        }

        /// <summary>
        /// 获取数据列
        /// </summary>
        /// <param name="selecttypeid"></param>
        /// <returns></returns>
        public List<SYSDataColumn> GetPreViewDataColumns(string selecttypeid)
        {
            DictionaryDAO dictDAO = StructureMap.ObjectFactory.GetInstance<DictionaryDAO>();
            var m = dictDAO.GetModel(selecttypeid);
            List<SYSDataColumn> colList = new List<SYSDataColumn>();

            if (m.Tag == "Const")
            { //固定单选类型
               
                colList.Add(new SYSDataColumn("ID","标记"));
                colList.Add(new SYSDataColumn("Name","可选项"));
             
            }
            else if (m.Tag == "Mutil" || m.Tag == "Single")
            {
                //下拉或弹出类型

                DataTable dt = Clover.Data.BaseDAO.GetDataTable(parseSchemaSQL(m.SqlCmd));
                foreach (DataColumn dc in dt.Columns) {
                    colList.Add(new SYSDataColumn(dc.ColumnName, dc.ColumnName));
                }
            }

            return colList;
        }

        private string parseSchemaSQL(string sqlcmd) {
            string sql = sqlcmd;
            int sidx = sqlcmd.IndexOf("select ", StringComparison.OrdinalIgnoreCase);
            int widx = sqlcmd.IndexOf("where ", StringComparison.OrdinalIgnoreCase);
            int didx = sqlcmd.IndexOf("distinct ", StringComparison.OrdinalIgnoreCase);
            if (widx > 1)
            {
                sql = sql.Substring(0, widx - 1);
            }
            if (didx > 1)
            {
                sql = sql.Insert(didx + 9, " TOP 0 ");
            }
            else
            {
                sql = sql.Insert(sidx + 7, " TOP 0 ");
            }
            return sql;
        }
            /// <summary>
        /// 获取预览数据
        /// </summary>
        /// <param name="selecttypeid"></param>
        /// <returns></returns>
        public DataTable GetPreViewData(string selecttypeid){
            int rowscount = 0;

            return GetPreViewData(selecttypeid, int.MaxValue, 1, string.Empty, out rowscount);
        }

        public DataTable GetPreViewData(string selecttypeid, int pagesize, int pageindex, string where,out int rowscount)
        {
            return GetPreViewData(null,selecttypeid, pagesize,  pageindex, where, "", "", out rowscount);
        }
        /// <summary>
        /// 获取预览数据
        /// </summary>
        /// <param name="selecttypeid"></param>
        /// <returns></returns>
        public DataTable GetPreViewData(Clover.Web.Core.IWebContext context, string selecttypeid, int pagesize, int pageindex, string where, string datarule, string sort, out int rowscount)
        {
            DictionaryDAO dictDAO = StructureMap.ObjectFactory.GetInstance<DictionaryDAO>();
            var m = dictDAO.GetModel(selecttypeid);
            DataTable data = null;
            rowscount = 0;

            if (m.Tag == FormColumn.Const)
            { //固定单选类型
                var childList = dictDAO.GetListByParent(m.DictID.ToString(), "");
                data =
                    Clover.Data.BaseDAO.GetDataTable("SELECT Code ID, Name FROM sys_DictItems WHERE Status = 1 AND DictID = '" + m.DictID.ToString() + "'");

                data = Clover.Core.Extension.DataTableHelper.GetPagedTable(data, pageindex, pagesize, "", "", out rowscount);
            }
            else if (m.Tag == FormColumn.Mutil || m.Tag == FormColumn.Single)
            {
                //下拉或弹出类型
                if (!string.IsNullOrEmpty(datarule))
                {
                    
                    data = Clover.Data.BaseDAO.GetList("("
                        + Clover.Data.BaseDAO.ParseSQLCommand(context, m.SqlCmd, null, null, datarule) + ") t"
                            , "ID", pagesize, pageindex, where, false, sort, out rowscount).Tables[0];
                }
                else {
                  
                    data = Clover.Data.BaseDAO.GetList("("
                        + Clover.Data.BaseDAO.ParseSQLCommand(context, m.SqlCmd
                            , Clover.Permission.BLL.FunctionDataRuleBLL.GetFDRuleStr) + ") t"
                            , "ID", pagesize, pageindex, where, false, sort, out rowscount).Tables[0];
                }
            }
            return data;
        }

     
        /// <summary>
        /// 获取预览数据
        /// </summary>
        /// <param name="selecttypeid"></param>
        /// <returns></returns>
        public DataTable GetPreViewDataWhere(string selecttypeid, string where, int pagesize, int pageindex, out int rowscount)
        {
            DictionaryDAO dictDAO = StructureMap.ObjectFactory.GetInstance<DictionaryDAO>();
            var m = dictDAO.GetModel(selecttypeid);
            DataTable data = null;
            rowscount = 0;

            if (m.Tag == "Const")
            { //固定单选类型
                var childList = dictDAO.GetListByParent(m.DictID.ToString(), "");
                data =
                    Clover.Data.BaseDAO.GetDataTable("SELECT ID, Name FROM sys_Dictionary WHERE ParentId = " + m.DictID.ToString());

                data = Clover.Core.Extension.DataTableHelper.GetPagedTable(data, pageindex, pagesize, "", "", out rowscount);
            }
            else if (m.Tag == "Mutil" || m.Tag == "Single")
            {
                //下拉或弹出类型
                data = Clover.Data.BaseDAO.GetList("(" + m.Value + (where!=string.Empty ? (" where " + where) : "") + ") t", "ID", pagesize, pageindex, "", true, "", out rowscount).Tables[0];
            }
            return data;
        }
        #endregion
    }

    public class SYSDataColumn
    {
        public string ColumnName;
        public string Caption;
        public SYSDataColumn() { }
        public SYSDataColumn(string columnName, string Caption)
        {
            this.ColumnName = columnName;
            this.Caption = Caption;
        }
    }
}