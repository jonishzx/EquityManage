namespace UkeyTech.WebFW.DAO
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;
    using Dapper;
    using Dapper.Contrib.Extensions;
    using UkeyTech.WebFW.Model;

    /// <summary>
    /// Form 数据访问层
    /// </summary>
    public partial class FormDAO : Clover.Data.BaseDAO
    {
        #region 构造函数
        public FormDAO()
        {
        }
        #endregion

        #region 根据主键创建 Form 数据模型实例
        /// <summary>
        /// 根据主键创建 Form 数据模型实例 
        /// </summary>
        public Form GetModel(int ID)
        {
            var conn = DbService();

            try
            {
                var rst = conn.Get<Form>(ID);
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
        public bool Update(Form model)
        {
            return Update(null, model);
        }
        /// <summary>
        /// 更新记录到数据库(利用事务)
        /// </summary>
        public bool Update(IDbTransaction tran, Form model)
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
        public bool Insert(Form model)
        {
            return Insert(null, model);
        }

        /// <summary>
        /// 新增记录到数据库(事务)
        /// </summary>
        public bool Insert(IDbTransaction tran, Form model)
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
        public bool Delete(Form model)
        {
            return Delete(model, null);
        }

        /// <summary>
        /// 从数据库删除记录(事务)
        /// </summary>
        public bool Delete(Form model, IDbTransaction tran)
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

            string sql = @"
                        DELETE FROM [CF_FormColumn] Where FormID = @ID;
                        DELETE FROM [CF_Form]
                        WHERE 	[ID] = @ID AND Status <> 2;
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
        public List<Form> GetAll(string orderBy)
        {
            return GetList(null, null, string.Empty, orderBy);
        }

        /// <summary>
        /// 查询所有记录，并排序(事务)
        /// </summary>
        public List<Form> GetList(IDbConnection conn, int? top, string strWhere, string orderBy)
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
	[FormCode],
	[FormName],
	[FormType],
	[ForUserList],
	[ForUserListId],
	[Condition],
	[FormDesignContent],
	[CreateTime],
	[UpdateTime],
	[Creator],
	[Modifitor]
 ");
            strSql.Append(" FROM [CF_Form] ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            if (orderBy != string.Empty)
                strSql.Append(" order by " + orderBy);


            var rst = conn.Query<Form>(strSql.ToString(), null);

            

            return new List<Form>(rst);
        }

        /// <summary>
        /// 查询所有记录，并排序、分页
        /// </summary>
        public List<Form> GetAllPaged(IDbConnection conn, int PageSize, int PageIndex, string strWhere, bool desc, string orderfldname)
        {
            int rstcount = 0;
            return GetAllPaged(null, PageSize, PageIndex, strWhere, desc, orderfldname, out rstcount);
        }

        /// <summary>
        /// 查询所有记录，并排序、分页,没有排序字段
        /// </summary>
        public List<Form> GetAllPaged(int PageSize, int PageIndex, string strWhere, bool desc)
        {
            return GetAllPaged(null, PageSize, PageIndex, strWhere, desc, "ID");
        }

        /// <summary>
        /// 查询所有记录，并排序、分页,没有排序字段
        /// </summary>
        public List<Form> GetAllPaged(int PageSize, int PageIndex, string strWhere, bool desc, out int rstcount)
        {
            return GetAllPaged(null, PageSize, PageIndex, strWhere, desc, "ID", out rstcount);
        }

       
        /// <summary>
        /// 查询所有记录，并排序、分页(事务)
        /// </summary>
        public List<Form> GetAllPaged(IDbConnection conn, int PageSize, int PageIndex, string strWhere, bool desc, string orderfldname, out int rstcount)
        {
            List<Form> list =  Clover.Data.BaseDAO.GetList<Form>("[CF_Form]", "ID", PageSize, PageIndex, strWhere, desc, orderfldname, out rstcount);

            foreach (var m in list) {
                m.HasExits = CheckTableExits(m.PsyTableName);
            }

            return list;
        }


        /// <summary>
        ///     获取导出数据内容
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetAllPagedFrom(string CodeOrName,string UserID)
        {
            string sql = string.Format(@"select A.*,dbo.Get_FormName(A.ID,'{0}') UserName from CF_Form A where {1} ", UserID, CodeOrName);
            return GetDataTable(sql);
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
            string strSQL = "select count(*) from [CF_Form]";
            strSQL = string.IsNullOrEmpty(strWhere) ? strSQL : strSQL + " where " + strWhere;
            var conn = DbService();
            IEnumerable<int> rst = conn.Query<int>(strSQL, null);
            
            return new List<int>(rst)[0];

        }
        #endregion

        #region 其他自定义方法
        /// <summary>
        /// 对所有符合条件的记录进行记录数计算
        /// </summary>
        public int GetMaxID()
        {
            string strSQL = "select max(ID) from [CF_Form]";

            var conn = DbService();
            IEnumerable<int> rst = conn.Query<int>(strSQL, null);
            
            return (new List<int>(rst)[0] + 1);

        }

        private int CheckTableExits(string tablename) {
            return Clover.Data.BaseDAO.CheckTableExists("dbo",tablename) ? 1 : 0;   
        }
        
          /// <summary>
        /// 检查是否存在相同的ID
        /// </summary>
        public bool DeleteTable(int FormID)
        {
            var model = GetModel(FormID);
            string sql = string.Format("Drop Table {0}", model.PsyTableName);
            var conn = DbService();

            if (CheckTableExits(model.PsyTableName) == 1)
                conn.Execute(sql);

            return true;
        }

        /// <summary>
        ///
        /// </summary>
        public string GetFormIdByName(string formName)
        {
            string sql = string.Format("select top 1 ID from CF_Form where FormName ='{0}'", formName);
         
            var conn = DbService();
            var rst = conn.Query<int>(sql, null);
            if (rst.Count() > 0)
                return rst.First().ToString();
            else
                return "";
        }

        public string GetFormIdByCode(string code)
        {
            string sql = string.Format("select top 1 ID from CF_Form where FormCode ='{0}'", code);

            var conn = DbService();
            var rst = conn.Query<int>(sql, null);
            if (rst.Count() > 0)
                return rst.First().ToString();
            else
                return "";
        }

        /// <summary>
        /// 检查是否存在相同的ID
        /// </summary>
        //public bool CreateTable(int FormID)
        //{
        //    FormColumnDAO col = new FormColumnDAO();
        //    DictionaryDAO dictdal = StructureMap.ObjectFactory.GetInstance<DictionaryDAO>();

        //    var collist = col.GetListByFormID(null, FormID);
        //    if (collist.Count == 0)
        //        return false;

        //    //建立语句
        //    var model = GetModel(FormID);
        //    StringBuilder sql = new StringBuilder();

        //    //获取数据类型映射
        //    var dicts = dictdal.GetListByTag("FormColumnDbType", "");

        //    string fmDatatableName = "[" + model.PsyTableName + "]";


        //    //检查表是否已经存在
        //    if (CheckTableExits(model.PsyTableName) == 0)
        //    {
        //        //需要新建表

        //        sql.AppendFormat("CREATE TABLE {0} (", fmDatatableName);
        //        //自动增长列
        //        sql.AppendLine("[ID] [int] IDENTITY (1, 1) NOT NULL ,[WorkFlowID] [varchar](50) ,");

        //        foreach (var m in collist)
        //        {
        //            sql.Append(string.Format("[{0}] [{1}]{2} {3},"
        //                , m.ColName
        //                , dicts.Find(x => m.ColType == x.Code).Value
        //                , getDBTypeSize(m)
        //                , m.Required == 1 ? "" : "NULL"));
        //        }
        //        sql.Remove(sql.Length - 1, 1);
        //        sql.Append(");");

        //        //建立主键
        //        sql.AppendFormat("ALTER TABLE {0} ", model.PsyTableName);
        //        sql.AppendFormat("ADD CONSTRAINT {0}_PK_ID ", model.PsyTableName);
        //        sql.AppendFormat("PRIMARY KEY ({0})", "ID");
        //    }
        //    else { 
        //        //更改列，只可以追加列

        //        //1.获取列
        //        DataTable dt = Clover.Data.BaseDAO.GetDataTable("select top 0 * from " + fmDatatableName);

        //        foreach (var m in collist)
        //        {
        //            if (!dt.Columns.Contains(m.ColName))
        //            {
        //                sql.AppendFormat("ALTER TABLE {0} ", model.PsyTableName);
        //                sql.Append(string.Format(" ADD [{0}] [{1}]{2} {3};"
        //                    , m.ColName
        //                    , dicts.Find(x => m.ColType == x.Code).Value
        //                    , getDBTypeSize(m)
        //                    , m.Required == 1 ? "" : "NULL"));
        //            }
        //        }
        //    }

        //    //执行
        //    if (sql.Length > 0)
        //    {
        //        var conn = DbService();
        //        int affectedrows = conn.Execute(sql.ToString());
        //    }

        //    return true;
        //}

        //private string getDBTypeSize(Model.FormColumn col) {
        //    if (col.ColType == FireWorkflow.Net.Model.DataTypeEnum.FLOAT.ToString())
        //    {
        //        return string.Format("({0},{1})", col.Size.Value.ToString(), col.FPSize.Value.ToString());
        //    }
        //    else if (col.ColType == FireWorkflow.Net.Model.DataTypeEnum.STRING.ToString())
        //    {
        //        return string.Format("({0})", col.Size.Value.ToString());
        //    }
        //    else
        //        return string.Empty;
        //}
     
        /// <summary>
        /// 检查是否存在相同的ID
        /// </summary>
        public bool CheckExistsSameID(string name, string id)
        {
            return ExistsSameAttr("[CF_Form]", "FormName", name, "", "ID", id);
        }

        /// <summary>
        /// 检查是否存在相同的ID
        /// </summary>
        public bool CheckPsyTableExistsSameID(string name, string id)
        {
            return ExistsSameAttr("[CF_Form]", "PsyTableName", name, "", "ID", id);
        }

           /// <summary>
        /// 插入指定数据
        /// </summary>
        public int InsertData(string psyTableName, Dictionary<string, object> dict)
        {
            return InsertData(psyTableName, dict, null);
        }

        /// <summary>
        /// 插入指定数据
        /// </summary>
        public int InsertData(string psyTableName, Dictionary<string,object> dict, Func<int, int> callback)
        {
           
            StringBuilder insertdata = new StringBuilder();

            insertdata.AppendFormat(" INSERT INTO {0} (", psyTableName);

            foreach (string key in dict.Keys)
            {
                insertdata.AppendFormat(" [{0}],", key);
            }

            insertdata.Remove(insertdata.Length - 1, 1);
            insertdata.Append(") Values (");

            foreach (string key in dict.Keys)
            {
                insertdata.AppendFormat("'{0}',", dict[key]);
            }
            insertdata.Remove(insertdata.Length - 1, 1);
            insertdata.Append(");select SCOPE_IDENTITY();");

            using (var ts = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.RequiresNew))
            {
                var conn = DbService();
                try
                {
                    var list = new List<decimal>(conn.Query<decimal>(insertdata.ToString()));

                    if (list.Count > 0)
                    {
                        if (callback != null)
                            callback(int.Parse(list[0].ToString()));
                    }
                   
                    ts.Complete();

                    if (list.Count > 0)
                    {
                         return (int)list[0];
                    }
                    else
                        return -1;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
          
        }


        /// <summary>
        /// 获取指定数据
        /// </summary>
        public Dictionary<string,object> GetData(string psyTableName, KeyValuePair<string,object> pmkey)
        {
            StringBuilder seldata = new StringBuilder();

            seldata.AppendFormat(" select top 1 * from {0}", psyTableName);
            seldata.AppendFormat(" Where {0} = '{1}'", pmkey.Key, pmkey.Value);

        
            var conn = DbService();

            var table = Clover.Data.BaseDAO.GetDataTable(seldata.ToString());
            Dictionary<string, object> dict = new Dictionary<string, object>();

            if (table.Rows.Count > 0)
            {
                foreach (DataColumn dc in table.Columns) {
                    dict.Add(dc.ColumnName, table.Rows[0][dc.ColumnName]);
                }

                return dict;
            }
            else
                return null;
        }


              /// <summary>
        /// 更新指定数据
        /// </summary>
        public int UpdateData(string psyTableName, KeyValuePair<string, object> pmkey, Dictionary<string, object> dict)
        {
            return UpdateData(psyTableName, pmkey, dict, null);
        }

        /// <summary>
        /// 更新指定数据
        /// </summary>
        public int UpdateData(string psyTableName, KeyValuePair<string, object> pmkey, Dictionary<string, object> dict, Action callback)
        {
            StringBuilder updateData = new StringBuilder();

            updateData.AppendFormat(" Update {0} Set ", psyTableName);


            foreach (string key in dict.Keys)
            {   
                updateData.AppendFormat("[{0}] = '{1}',", key, dict[key]);
            }

            updateData.Remove(updateData.Length - 1, 1);
            updateData.AppendFormat(" Where {0} = '{1}'", pmkey.Key, pmkey.Value);
            int rst = 0;
            using (var ts = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.RequiresNew))
            {
                var conn = DbService();
                try
                {
                    rst =  conn.Execute(updateData.ToString());

                    if (callback!=null)
                        callback();

                    ts.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return rst;
        }

        /// <summary>
        /// 查询所有记录，并排序、分页(事务)
        /// </summary>
        public DataSet GetAllPaged(int formid, int PageSize, int PageIndex, string strWhere, bool desc, string orderfldname, out int rstcount)
        {
          
            var model = GetModel(formid);
            var coldal = new DAO.FormColumnDAO();
            var dcitdal = StructureMap.ObjectFactory.GetInstance<DictionaryDAO>();

            StringBuilder sb = new StringBuilder();
            sb.Append("( select a.ID,a.WorkFlowID, ");
            
            var list = coldal.GetListByFormID(formid);

            foreach (var m in list) {                
                switch (m.SelectColType) { 
                    case "Const":
                        sb.AppendFormat("(select Name from sys_Dictionary where ID = {0}) {0}", m.ColName);
                        break;
                    case "Mutil":
                        sb.AppendFormat("( select top 1 '名称' from ({1}) t where ID = {0}) {0}", m.ColName, dcitdal.GetModel(m.SelectTypeId).Value);
                        break;
                    case "Single":
                        sb.AppendFormat("( select top 1 '名称' from ({1}) t where ID = {0}) {0}", m.ColName, dcitdal.GetModel(m.SelectTypeId).Value);
                        break;
                    default:
                        sb.Append(m.ColName);
                        break;
                }

                sb.Append(",");
            }
            
     
            sb.Append(@" c.ID as WorkFlowProcessId from [" + model.PsyTableName + @"] a 
			    left join T_FF_DF_WORKFLOWDEF d on a.WorkFlowID = d.ID
				join T_FF_RT_PROCESSINSTANCE c on d.PROCESS_ID = c.PROCESS_ID AND d.VERSION = c.VERSION
				join T_FF_RT_PROCINST_VAR b on (b.NAME = 'sn' AND a.ID = b.VALUE) 		  
				AND c.ID = b.PROCESSINSTANCE_ID
                ) t ");

            DataSet ds = Clover.Data.BaseDAO.GetList(sb.ToString(), "ID", PageSize, PageIndex, strWhere, desc, orderfldname, out rstcount);

            return ds;
        }
        public bool CheckProcessIsAssociatedWithWorkFlow(string formId, string busikey, out string processinstanceid)
        {
            string processid = string.Empty;
            return CheckProcessIsAssociatedWithWorkFlow(formId, busikey, out processinstanceid, out processid);
        }
        /// <summary>
        /// 检查特定的业务实体已经存在于工作流程表中
        /// </summary>
        /// <param name="formId"></param>
        /// <param name="busikey"></param>
        /// <returns></returns>
        public bool CheckProcessIsAssociatedWithWorkFlow(string formId, string busikey, out string processinstanceid, out string processid)
        {
            processinstanceid = string.Empty;
            processid = string.Empty;
            string sql = @"SELECT top 1 (tfrp.ID + ',' + (select top 1 tfdw.ID from T_FF_DF_WORKFLOWDEF 
       tfdw where tfrp.version = tfdw.version
       and  tfrp.PROCESS_ID = tfdw.PROCESS_ID)) ProcessID
FROM T_FF_RT_PROCESSINSTANCE AS tfrp
WHERE tfrp.BIZ_ID = @BusiId AND tfrp.BIZ_TYPE_ID = @FormId
ORDER BY tfrp.CREATED_TIME DESC";

            var p = new DynamicParameters();
            p.Add("BusiId", busikey);
            p.Add("FormId", formId);
            var conn = DbService();
            var rst = conn.Query<string>(sql, p);
            if (rst.Count() > 0)
            {
                var vals = rst.First().Split(new char[] { ',' });
                processinstanceid = vals[0];
                processid = vals[1];
                return true;
            }
            else
                return false;
        }
        #endregion
    }
}