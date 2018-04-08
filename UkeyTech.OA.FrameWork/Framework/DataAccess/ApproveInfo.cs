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
    /// ApproveInfo 数据访问层
    /// </summary>
    public partial class ApproveInfoDAO : Clover.Data.BaseDAO
    {
        #region 构造函数
        public ApproveInfoDAO()
        {
        }
        #endregion

        #region 根据主键创建 ApproveInfo 数据模型实例
        /// <summary>
        /// 根据主键创建 ApproveInfo 数据模型实例 
        /// </summary>
        public ApproveInfo GetModel(int AuditingID)
        {
            var conn = DbService();

            try
            {
                var rst = conn.Get<ApproveInfo>(AuditingID);
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
        public bool Update(ApproveInfo model)
        {
            return Update(null, model);
        }
        /// <summary>
        /// 更新记录到数据库(利用事务)
        /// </summary>
        public bool Update(IDbTransaction tran, ApproveInfo model)
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
        public bool Insert(ApproveInfo model)
        {
            return Insert(null, model);
        }

        /// <summary>
        /// 新增记录到数据库(事务)
        /// </summary>
        public bool Insert(IDbTransaction tran, ApproveInfo model)
        {

            IDbConnection conn = tran == null ? DbService() : tran.Connection;

            try
            {
                long id = conn.Insert(model, tran);
                model.AuditingID = (int)id;
            }
            catch (DataException ex)
            {
                throw ex;
            }
           
            return true;
        }

        /// <summary>
        /// 新增记录到数据库(事务)
        /// </summary>
        public void InsertTrans(IDbConnection conn, ApproveInfo model)
        {
            var p = new DynamicParameters();
            p.Add("AuditingID", model.AuditingID, DbType.Int32, null, 4);
            p.Add("StaffID", model.StaffID, DbType.String, null, 50);
            p.Add("AuditingInfo", model.AuditingInfo, DbType.String, null, 2000);
            p.Add("AuditingState", model.AuditingState, DbType.String, null, 20);
            p.Add("AuditingTime", model.AuditingTime, DbType.DateTime, null, 8);
            p.Add("Remark", model.Remark, DbType.String, null, 500);
            p.Add("TargetId", model.TargetId, DbType.String, null, 50);
            p.Add("TargetObject", model.TargetObject, DbType.String, null, 150);
            p.Add("TargetDescn", model.TargetDescn, DbType.String, null, 50);
            p.Add("WorkItemId", model.WorkItemId, DbType.String, null, 50);

            string sql = @"INSERT INTO [Biz_ApproveInfo] (                          
	[StaffID],
	[AuditingInfo],
	[AuditingState],
	[AuditingTime],
	[Remark],
	[TargetId],
	[TargetObject],
	[TargetDescn],
    [WorkItemId]
                        ) VALUES (
                            	@StaffID,
	@AuditingInfo,
	@AuditingState,
	@AuditingTime,
	@Remark,
	@TargetId,
	@TargetObject,
    @TargetDescn,
	@WorkItemId
)";


                conn.Execute(sql, p);

        }
        #endregion

        #region 删除记录

        /// <summary>
        /// 从数据库删除记录(事务)
        /// </summary>
        public bool Delete(ApproveInfo model)
        {
            return Delete(model, null);
        }

        /// <summary>
        /// 从数据库删除记录(事务)
        /// </summary>
        public bool Delete(ApproveInfo model, IDbTransaction tran)
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
        public bool Delete(int AuditingID)
        {
            return Delete(null, AuditingID);
        }

        /// <summary>
        /// 从数据库删除记录(事务)
        /// </summary>
        public bool Delete(IDbTransaction tran, int AuditingID)
        {
            int affectedrows = 0;

            var p = new DynamicParameters();
            //获取主键
            p.Add("AuditingID", AuditingID, DbType.Int32, null, 4);

            string sql = @"DELETE FROM [Biz_ApproveInfo]
                        WHERE 	[AuditingID] = @AuditingID
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
        public List<ApproveInfo> GetAll(string orderBy)
        {
            return GetList(null, null, string.Empty, orderBy);
        }

        /// <summary>
        /// 查询所有记录，并排序(事务)
        /// </summary>
        public List<ApproveInfo> GetList(IDbConnection conn, int? top, string strWhere, string orderBy)
        {
            if (conn == null)
                conn = DbService();

            StringBuilder strSql = new StringBuilder();
            strSql.Append("Select ");
            if (top.HasValue)
            {
                strSql.Append(" top " + top.ToString());
            }
            strSql.Append(@" 	[AuditingID],
	[StaffID],
	[AuditingInfo],
	[AuditingState],
	[AuditingTime],
	[Remark],
	[TargetId],
	[TargetObject],
	[TargetDescn],
[WORKITEMID]
 ");
            strSql.Append(" FROM [Biz_ApproveInfo] ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            if (orderBy != string.Empty)
                strSql.Append(" order by " + orderBy);


            var rst = conn.Query<ApproveInfo>(strSql.ToString(), null);

            

            return new List<ApproveInfo>(rst);
        }

        /// <summary>
        /// 查询所有记录，并排序、分页
        /// </summary>
        public List<ApproveInfo> GetAllPaged(IDbConnection conn, int PageSize, int PageIndex, string strWhere, bool desc, string orderfldname)
        {
            int rstcount = 0;
            return GetAllPaged(null, PageSize, PageIndex, strWhere, desc, orderfldname, out rstcount);
        }

        /// <summary>
        /// 查询所有记录，并排序、分页,没有排序字段
        /// </summary>
        public List<ApproveInfo> GetAllPaged(int PageSize, int PageIndex, string strWhere, bool desc)
        {
            return GetAllPaged(null, PageSize, PageIndex, strWhere, desc, "AuditingID");
        }

        /// <summary>
        /// 查询所有记录，并排序、分页,没有排序字段
        /// </summary>
        public List<ApproveInfo> GetAllPaged(int PageSize, int PageIndex, string strWhere, bool desc, out int rstcount)
        {
            return GetAllPaged(null, PageSize, PageIndex, strWhere, desc, "AuditingID", out rstcount);
        }

        /// <summary>
        /// 查询所有记录，并排序、分页(事务)
        /// </summary>
        public List<ApproveInfo> GetAllPaged(IDbConnection conn, int PageSize, int PageIndex, string strWhere, bool desc, string orderfldname, out int rstcount)
        {
            return Clover.Data.BaseDAO.GetList<ApproveInfo>("[Biz_ApproveInfo]", "AuditingID", PageSize, PageIndex, strWhere, desc, orderfldname, out rstcount);
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
            string strSQL = "select count(*) from [Biz_ApproveInfo]";
            strSQL = string.IsNullOrEmpty(strWhere) ? strSQL : strSQL + " where " + strWhere;
            var conn = DbService();
            IEnumerable<int> rst = conn.Query<int>(strSQL, null);
            
            return new List<int>(rst)[0];

        }
        #endregion

        #region 其他自定义方法
        /// <summary>
        /// 查询所有记录，并排序、分页,没有排序字段
        /// </summary>
        public List<ApproveInfo> GetAllPaged(int PageSize, int PageIndex, string targetid, string targetobject, bool desc, out int rstcount)
        {
            string where = "TargetId='" + targetid + "' AND targetobject='" + targetobject + "'";

            return GetAllPaged(null, PageSize, PageIndex, where, desc, "AuditingID", out rstcount);
        }

        /// <summary>
        /// 
        /// </summary>
        public void InsertWithCallback(ApproveInfo model, Action callback)
        {
            //Insert(model);
            //callback();

            using (var ts = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required))
            {
                using (IDbConnection conn = ManualDbService())
                {
                    InsertTrans(conn, model);
                }

                callback();
                ts.Complete();
            }
        }
        #endregion
    }
}