using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using Dapper;

using UkeyTech.WebFW.Model;

namespace UkeyTech.WebFW.DAO
{

    /// <summary>
    /// Widget 数据访问层
    /// </summary>
    public partial class WidgetDAO : Clover.Data.BaseDAO
    {
        #region 构造函数
        public WidgetDAO()
        {
        }
        #endregion

        #region 根据主键创建 Widget 数据模型实例
        /// <summary>
        /// 根据主键创建 Widget 数据模型实例 
        /// </summary>
        public Widget GetModel(int WidgetID)
        {
            Widget model = null;
            var p = new DynamicParameters();
            //获取主键
            p.Add("WidgetID", WidgetID, DbType.Int32, null, 4);

            var conn = DbService();

            try
            {
                var rst = conn.Query<Widget>(
                @"select * from sys_Widget where 	[WidgetID] = @WidgetID
", p);

                List<Widget> lrst
                    = new List<Widget>(rst);

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
        public bool Update(Widget model)
        {
            return Update(null, model);
        }
        /// <summary>
        /// 更新记录到数据库(利用事务)
        /// </summary>
        public bool Update(IDbTransaction tran, Widget model)
        {
            int affectedrows = 0;

            var p = new DynamicParameters();
            p.Add("WidgetID", model.WidgetID, DbType.Int32, null, 4);
            p.Add("WidgetCode", model.WidgetCode, DbType.String, null, 20);
            p.Add("WidgetName", model.WidgetName, DbType.String, null, 50);
            p.Add("WidgetTag", model.WidgetTag, DbType.String, null, 50);
            p.Add("Descn", model.Descn, DbType.String, null, 100);
            p.Add("Target", model.Target, DbType.String, null, 100);
            p.Add("Parameters", model.Parameters, DbType.String, null, 4000);
            p.Add("UIParamters", model.UIParamters, DbType.String, null, 2000);
            p.Add("CreateTime", model.CreateTime, DbType.DateTime, null, 8);
            p.Add("UpdateTime", model.UpdateTime, DbType.DateTime, null, 8);
            p.Add("Creator", model.Creator, DbType.String, null, 20);
            p.Add("Modifitor", model.Modifitor, DbType.String, null, 20);
            p.Add("Status", model.Status, DbType.Int32, null, 4);
            p.Add("ViewOrd", model.ViewOrd, DbType.Int32, null, 4);

            string sql = @"UPDATE [sys_Widget] SET
	[WidgetCode] = @WidgetCode,
	[WidgetName] = @WidgetName,
	[WidgetTag] = @WidgetTag,
	[Descn] = @Descn,
	[Target] = @Target,
	[Parameters] = @Parameters,
	[UIParamters] = @UIParamters,
	[CreateTime] = @CreateTime,
	[UpdateTime] = @UpdateTime,
	[Creator] = @Creator,
	[Modifitor] = @Modifitor,
	[Status] = @Status,
	[ViewOrd] = @ViewOrd
                    WHERE
                        	[WidgetID] = @WidgetID
";
            IDbConnection conn = tran == null ? DbService() : tran.Connection;

            try
            {
                if (tran == null)
                    affectedrows = conn.Execute(sql, p);
                else
                    conn.Execute(sql, p, tran, null, null);
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
        public bool Insert(Widget model)
        {
            return Insert(null, model);
        }

        /// <summary>
        /// 新增记录到数据库(事务)
        /// </summary>
        public bool Insert(IDbTransaction tran, Widget model)
        {
            var p = new DynamicParameters();
            p.Add("WidgetID", model.WidgetID, DbType.Int32, null, 4);
            p.Add("WidgetCode", model.WidgetCode, DbType.String, null, 20);
            p.Add("WidgetName", model.WidgetName, DbType.String, null, 50);
            p.Add("WidgetTag", model.WidgetTag, DbType.String, null, 50);
            p.Add("Descn", model.Descn, DbType.String, null, 100);
            p.Add("Target", model.Target, DbType.String, null, 100);
            p.Add("Parameters", model.Parameters, DbType.String, null, 4000);
            p.Add("UIParamters", model.UIParamters, DbType.String, null, 2000);
            p.Add("CreateTime", model.CreateTime, DbType.DateTime, null, 8);
            p.Add("UpdateTime", model.UpdateTime, DbType.DateTime, null, 8);
            p.Add("Creator", model.Creator, DbType.String, null, 20);
            p.Add("Modifitor", model.Modifitor, DbType.String, null, 20);
            p.Add("Status", model.Status, DbType.Int32, null, 4);
            p.Add("ViewOrd", model.ViewOrd, DbType.Int32, null, 4);


            string sql = @"INSERT INTO [sys_Widget] (                          
	[WidgetCode],
	[WidgetName],
	[WidgetTag],
	[Descn],
	[Target],
	[Parameters],
	[UIParamters],
	[CreateTime],
	[UpdateTime],
	[Creator],
	[Modifitor],
	[Status],
	[ViewOrd]
                        ) VALUES (
                            	@WidgetCode,
	@WidgetName,
	@WidgetTag,
	@Descn,
	@Target,
	@Parameters,
	@UIParamters,
	@CreateTime,
	@UpdateTime,
	@Creator,
	@Modifitor,
	@Status,
	@ViewOrd
)";

            sql += ";select @@IDENTITY";

            IDbConnection conn = tran == null ? DbService() : tran.Connection;

            try
            {
                var keys = new List<decimal>(conn.Query<decimal>(sql, p));

                model.WidgetID = Convert.ToInt32(keys[0]);
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
        public bool Delete(int WidgetID)
        {
            return Delete(null, WidgetID);
        }


        /// <summary>
        /// 从数据库删除记录(事务)
        /// </summary>
        public bool Delete(IDbTransaction tran, int WidgetID)
        {
            int affectedrows = 0;

            var p = new DynamicParameters();
            //获取主键
            p.Add("WidgetID", WidgetID, DbType.Int32, null, 4);

            string sql = @"DELETE FROM [sys_Widget]
                        WHERE 	[WidgetID] = @WidgetID
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
        public List<Widget> GetAll(string orderBy)
        {
            return GetList(null, null, " Status = 1 ", orderBy);
        }

        /// <summary>
        /// 查询所有记录，并排序(事务)
        /// </summary>
        public List<Widget> GetList(IDbConnection conn, int? top, string strWhere, string orderBy)
        {
            if (conn == null)
                conn = DbService();

            StringBuilder strSql = new StringBuilder();
            strSql.Append("Select ");
            if (top.HasValue)
            {
                strSql.Append(" top " + top.ToString());
            }
            strSql.Append(@" 	[WidgetID],
	[WidgetCode],
	[WidgetName],
	[WidgetTag],
	[Descn],
	[Target],
	[Parameters],
	[UIParamters],
	[CreateTime],
	[UpdateTime],
	[Creator],
	[Modifitor],
	[Status],
	[ViewOrd]
 ");
            strSql.Append(" FROM [sys_Widget] ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            if (orderBy != string.Empty)
                strSql.Append(" order by " + orderBy);


            var rst = conn.Query<Widget>(strSql.ToString(), null);

            

            return new List<Widget>(rst);
        }

        /// <summary>
        /// 查询所有记录，并排序、分页
        /// </summary>
        public List<Widget> GetAllPaged(IDbConnection conn, int PageSize, int PageIndex, string strWhere, bool desc, string orderfldname)
        {
            int rstcount = 0;
            return GetAllPaged(null, PageSize, PageIndex, strWhere, desc, orderfldname, out rstcount);
        }

        /// <summary>
        /// 查询所有记录，并排序、分页,没有排序字段
        /// </summary>
        public List<Widget> GetAllPaged(int PageSize, int PageIndex, string strWhere, bool desc)
        {
            return GetAllPaged(null, PageSize, PageIndex, strWhere, desc, "WidgetID");
        }

        /// <summary>
        /// 查询所有记录，并排序、分页,没有排序字段
        /// </summary>
        public List<Widget> GetAllPaged(int PageSize, int PageIndex, string strWhere, bool desc, out int rstcount)
        {
            return GetAllPaged(null, PageSize, PageIndex, strWhere, desc, "WidgetID", out rstcount);
        }

        /// <summary>
        /// 查询所有记录，并排序、分页(事务)
        /// </summary>
        public List<Widget> GetAllPaged(IDbConnection conn, int PageSize, int PageIndex, string strWhere, bool desc, string orderfldname, out int rstcount)
        {
            return Clover.Data.BaseDAO.GetList<Widget>("[sys_Widget]", "WidgetID", PageSize, PageIndex, strWhere, desc, orderfldname, out rstcount);
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
            string strSQL = "select count(*) from [sys_Widget]";
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
        public bool CheckExistsSameID(string widgetcode, string id)
        {
            return ExistsSameAttr("[sys_Widget]", "WidgetCode", widgetcode, "Status>=0", "WidgetID", id);
        }

        #endregion
    }
}
