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
    /// DictItem 数据访问层
    /// </summary>
    public partial class DictItemDAO : Clover.Data.BaseDAO
    {
        #region 构造函数
        public DictItemDAO()
        {
        }
        #endregion

        #region 根据主键创建 DictItem 数据模型实例
        /// <summary>
        /// 根据主键创建 DictItem 数据模型实例 
        /// </summary>
        public DictItem GetModel(string DictID, string Code)
        {
            var conn = DbService();

            try
            {
                var keylist = new DynamicParameters();
                keylist.Add("DictID", DictID, DbType.String, null, 30);
                keylist.Add("Code", Code, DbType.String, null, 30);
                var rst = new List<DictItem>(conn.Query<DictItem>(
                    "select * from sys_DictItems where DictID = @DictID and Code = @Code",
                    keylist));
              
                return rst.Count > 0 ? rst[0]:null;
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
        public bool Update(DictItem model)
        {
            return Update(null, model);
        }
        /// <summary>
        /// 更新记录到数据库(利用事务)
        /// </summary>
        public bool Update(IDbTransaction tran, DictItem model)
        {

            IDbConnection conn = tran == null ? DbService() : tran.Connection;
            bool rst = false;
            try
            {
                var sql = @"UPDATE [sys_DictItems]
   SET 
      [Name] = @Name
      ,[Value] = @Value 
      ,[Descn] = @Descn    
      ,[Status] = @Status
      ,[ViewOrder] = @ViewOrder
      ,[UpdateTime] = @UpdateTime
 WHERE [Code] = @Code AND [DictID] = @DictID";
                rst = conn.Execute(sql, model, null, tran) > 0;
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
        public bool Insert(DictItem model)
        {
            return Insert(null, model);
        }

        /// <summary>
        /// 新增记录到数据库(事务)
        /// </summary>
        public bool Insert(IDbTransaction tran, DictItem model)
        {

            IDbConnection conn = tran == null ? DbService() : tran.Connection;

            try
            {
                conn.Insert(model, tran);
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
        public bool Delete(DictItem model)
        {
            return Delete(model, null);
        }

        /// <summary>
        /// 从数据库删除记录(事务)
        /// </summary>
        public bool Delete(DictItem model, IDbTransaction tran)
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
        public bool Delete(string DictID, string Code)
        {
            return Delete(null, DictID, Code);
        }

        /// <summary>
        /// 从数据库删除记录(事务)
        /// </summary>
        public bool Delete(IDbTransaction tran, string DictID, string Code)
        {
            int affectedrows = 0;

            var p = new DynamicParameters();
            //获取主键
            p.Add("DictID", DictID, DbType.String, null, 30);
            p.Add("Code", Code, DbType.String, null, 30);

            string sql = @"DELETE FROM [sys_DictItems]
                        WHERE 	[DictID] = @DictID
	AND [Code] = @Code
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
        public List<DictItem> GetAll(string orderBy)
        {
            return GetList(null, null, string.Empty, orderBy);
        }

        /// <summary>
        /// 查询所有记录，并排序(事务)
        /// </summary>
        public List<DictItem> GetList(IDbConnection conn, int? top, string strWhere, string orderBy)
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
	[Code],
	[Name],
	[Value],
	[IsGroup],
	[Descn],
	[ExtAttr],
	[Status],
	[ViewOrder],
    [UpdateTime]
 ");
            strSql.Append(" FROM [sys_DictItems] ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            if (orderBy != string.Empty)
                strSql.Append(" order by " + orderBy);


            var rst = conn.Query<DictItem>(strSql.ToString(), null);

            

            return new List<DictItem>(rst);
        }

        /// <summary>
        /// 查询所有记录，并排序、分页
        /// </summary>
        public List<DictItem> GetAllPaged(IDbConnection conn, int PageSize, int PageIndex, string strWhere, bool desc, string orderfldname)
        {
            int rstcount = 0;
            return GetAllPaged(null, PageSize, PageIndex, strWhere, desc, orderfldname, out rstcount);
        }

        /// <summary>
        /// 查询所有记录，并排序、分页,没有排序字段
        /// </summary>
        public List<DictItem> GetAllPaged(int PageSize, int PageIndex, string strWhere, bool desc)
        {
            return GetAllPaged(null, PageSize, PageIndex, strWhere, desc, "DictID,Code");
        }

        /// <summary>
        /// 查询所有记录，并排序、分页,没有排序字段
        /// </summary>
        public List<DictItem> GetAllPaged(int PageSize, int PageIndex, string strWhere, bool desc, out int rstcount)
        {
            return GetAllPaged(null, PageSize, PageIndex, strWhere, desc, "DictID,Code", out rstcount);
        }

        /// <summary>
        /// 查询所有记录，并排序、分页(事务)
        /// </summary>
        public List<DictItem> GetAllPaged(IDbConnection conn, int PageSize, int PageIndex, string strWhere, bool desc, string orderfldname, out int rstcount)
        {
            return Clover.Data.BaseDAO.GetList<DictItem>("[sys_DictItems]", "Code", PageSize, PageIndex, strWhere, desc, orderfldname, out rstcount);
        }


        /// <summary>
        /// 查询所有记录，并排序、分页(事务)
        /// </summary>
        public DataSet GetAllPaged(int PageSize, int PageIndex, string strWhere, bool desc, string orderfldname, out int rstcount)
        {
            DataSet ds = Clover.Data.BaseDAO.GetList("[sys_DictItems]", "Code", PageSize, PageIndex, strWhere, desc, orderfldname, out rstcount);

            return ds;
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
            string strSQL = "select count(*) from [sys_DictItems]";
            strSQL = string.IsNullOrEmpty(strWhere) ? strSQL : strSQL + " where " + strWhere;
            var conn = DbService();
            IEnumerable<int> rst = conn.Query<int>(strSQL, null);
            
            return new List<int>(rst)[0];

        }
        #endregion

        #region 其他自定义方法
        /// <summary>
        /// 检查是否存在相同的ID
        /// </summary>
        public bool CheckExistsSameName(string name,string dictid, string code)
        {
            return ExistsSameAttr("sys_DictItems", "Name", name, "DictID='" + dictid + "'", "Code", code);
        }

        public bool CheckExistsSameCode(string dictid, string code)
        {
            return ExistsSameAttr("sys_DictItems", "Code", code, "DictID='" + dictid + "'", "", string.Empty);
        }

        public int GetDictItemsCacheCount() {
            return dictitem == null ? 0 : dictitem.Count;
        }
        public void InitDictItems() {
            var conn = DbService();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("Select ");
            strSql.Append(@" 	[DictID],
	            [Code],
	            [Name],
	            [Value],
	            [IsGroup],
	            [Descn],
	            [ExtAttr],
	            [Status],
	            [ViewOrder],
                [UpdateTime]
             ");
            strSql.Append(" FROM [sys_DictItems] ");
            strSql.Append(" where Status = 1");
            strSql.Append(" order by DictID,ViewOrder");

            var rst = conn.Query<DictItem>(strSql.ToString(), null);

            dictitem = new List<DictItem>(rst);
        }


        static List<DictItem> dictitem = null;
        private static object lk = new object();
        public void ClearCacheItems()
        {
            lock (lk)
            {
                dictitem = null;
            }
        }
        /// <summary>
        /// 查询所有记录，并排序(事务)
        /// </summary>
        public List<DictItem> GetListByDictID(string dictid)
        {
            if (dictitem == null)
            {
                lock (lk)
                {
                    if (dictitem == null)
                        InitDictItems();
                }
            }
            if (dictitem != null)
                return dictitem.FindAll(x => x.DictID == dictid);
            else
                return null;
        }
        /// <summary>
        /// 查询所有记录，并排序(事务)
        /// </summary>
        public string GetItemNameByDictID(string dictid, string code)
        {
            if (dictitem == null)
            {
                lock (lk)
                {
                    if (dictitem == null)
                        InitDictItems();
                }
            }
            if (dictitem != null){
            
                var exit = dictitem.Find(x => x.DictID.Equals(dictid, StringComparison.CurrentCultureIgnoreCase) 
                            && x.Code.Equals(code, StringComparison.CurrentCultureIgnoreCase));
                if (exit != null)
                    return exit.Name;
                else
                {
                    return code;
                    
                }
            }
            else
                return null;
        }
        /// <summary>
        /// 查询所有记录，并排序(事务)
        /// </summary>
        public List<DictItem> GetListByDictIDs(string[] dictids)
        {
            List<string> list = new List<string>(dictids);
            if (dictitem == null)
            {
                lock (lk)
                {
                    if (dictitem == null)
                        InitDictItems();
                }
            }
            if (dictitem != null)
                return dictitem.FindAll(x => list.Contains(x.DictID));
            else
                return null;
        }


        public DataTable GetListByEmpIDs()
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("Select ");
            strSql.Append(@" 	[Code],
	[Name] 
 ");
            strSql.Append(" FROM [V_PayStrust] ");

            return  Clover.Data.BaseDAO.GetDataTable(strSql.ToString());
        }
        #endregion
    }
}