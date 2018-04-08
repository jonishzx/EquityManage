namespace Clover.Permission.DAO
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Text;
    using Dapper;
    using Dapper.Contrib.Extensions;

    using Clover.Permission.Model;

    /// <summary>
    /// Position 数据访问层
    /// </summary>
    public partial class PositionDAO : Clover.Data.BaseDAO
    {
        
        #region 构造函数
        public PositionDAO()
        {
        }
        #endregion

        #region 根据主键创建 Position 数据模型实例
        /// <summary>
        /// 根据主键创建 Position 数据模型实例 
        /// </summary>
        public Position GetModel(int PositionID)
        {
            var conn = DbService();

            try
            {
                var rst = conn.Get<Position>(PositionID);
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
        public bool Update(Position model)
        {
            return Update(null, model);
        }
        /// <summary>
        /// 更新记录到数据库(利用事务)
        /// </summary>
        public bool Update(IDbTransaction tran, Position model)
        {

            int affectedrows = 0;

            var p = new DynamicParameters();
            p.Add("PositionID", model.PositionID, DbType.Int32, null, 4);
            p.Add("PositionCode", model.PositionCode, DbType.String, null, 50);
            p.Add("PositionName", model.PositionName, DbType.String, null, 50);
            p.Add("Descn", model.Descn, DbType.String, null, 200);
            p.Add("Attribute", model.Attribute, DbType.String, null, 16);
            p.Add("ParentID", model.ParentID, DbType.Int32, null, 4);
            p.Add("ParentPath", model.ParentPath, DbType.String, null, 200);
            p.Add("ViewOrd", model.ViewOrd, DbType.Int32, null, 4);
            p.Add("CreateTime", model.CreateTime, DbType.DateTime, null, 8);
            p.Add("UpdateTime", model.UpdateTime, DbType.DateTime, null, 8);
            p.Add("Creator", model.Creator, DbType.String, null, 20);
            p.Add("Modifitor", model.Modifitor, DbType.String, null, 20);
            p.Add("Status", model.Status, DbType.Int32, null, 4);
            p.Add("GroupId", model.GroupId, DbType.Int32, null, 4);
            p.Add("PositionLevel", model.PositionLevel, DbType.Int32, null, 4);

            string sql = @"UPDATE [CPM_Position] SET
	            [PositionCode] = @PositionCode,
	            [PositionName] = @PositionName,
	            [Descn] = @Descn,
	            [Attribute] = @Attribute,
	            [ParentID] = @ParentID,
                [GroupId] = @GroupId,
	            [ParentPath] = @ParentPath,
	            [ViewOrd] = @ViewOrd,
	            [CreateTime] = @CreateTime,
	            [UpdateTime] = @UpdateTime,
	            [Creator] = @Creator,
	            [Modifitor] = @Modifitor,
	            [Status] = @Status,
                [PositionLevel] = @PositionLevel
                    WHERE
                        	[PositionID] = @PositionID";
            IDbConnection conn = tran == null ? DbService() : tran.Connection;

            try
            {
                affectedrows = conn.Execute(sql, p);
            }
            catch (DataException ex)
            {
                throw ex;
            }

            return Convert.ToBoolean(affectedrows);
        }
        #endregion

        #region 新增记录
        /// <summary>
        /// 新增记录到数据库
        /// </summary>
        public bool Insert(Position model)
        {
            return Insert(null, model);
        }

        /// <summary>
        /// 新增记录到数据库(事务)
        /// </summary>
        public bool Insert(IDbTransaction tran, Position model)
        {

            var p = new DynamicParameters();
            p.Add("PositionCode", model.PositionCode, DbType.String, null, 50);
            p.Add("PositionName", model.PositionName, DbType.String, null, 50);
            p.Add("Descn", model.Descn, DbType.String, null, 200);
            p.Add("Attribute", model.Attribute, DbType.String, null, 16);
            p.Add("ParentID", model.ParentID, DbType.Int32, null, 4);
            p.Add("ParentPath", model.ParentPath, DbType.String, null, 200);
            p.Add("ViewOrd", model.ViewOrd, DbType.Int32, null, 4);
            p.Add("CreateTime", model.CreateTime, DbType.DateTime, null, 8);
            p.Add("UpdateTime", model.UpdateTime, DbType.DateTime, null, 8);
            p.Add("Creator", model.Creator, DbType.String, null, 20);
            p.Add("Modifitor", model.Modifitor, DbType.String, null, 20);
            p.Add("Status", model.Status, DbType.Int32, null, 4);
            p.Add("GroupId", model.GroupId, DbType.Int32, null, 4);
            p.Add("PositionLevel", model.PositionLevel, DbType.Int32, null, 4);


            string sql = @"INSERT INTO [CPM_Position] (                          
	            [PositionCode],
	            [PositionName],
	            [Descn],
	            [Attribute],
	            [ParentID],
	            [ParentPath],
	            [ViewOrd],
	            [CreateTime],
	            [UpdateTime],
	            [Creator],
	            [Modifitor],
	            [Status],
                [GroupId],
                [PositionLevel]
                        ) VALUES (
                @PositionCode,
	            @PositionName,
	            @Descn,
	            @Attribute,
	            @ParentID,
	            @ParentPath,
	            @ViewOrd,
	            @CreateTime,
	            @UpdateTime,
	            @Creator,
	            @Modifitor,
	            @Status,
                @GroupId,
                @PositionLevel
            )";

            sql += ";select @@IDENTITY";

            IDbConnection conn = tran == null ? DbService() : tran.Connection;

            try
            {
                var keys = new List<decimal>(conn.Query<decimal>(sql, p));

                model.PositionID = Convert.ToInt32(keys[0]);
            }
            catch (DataException ex)
            {
                throw ex;
            }

            return true;
        }
        #endregion

        #region 删除记录

        /// <summary>
        /// 从数据库删除记录(事务)
        /// </summary>
        public bool Delete(Position model)
        {
            return Delete(model, null);
        }

        /// <summary>
        /// 从数据库删除记录(事务)
        /// </summary>
        public bool Delete(Position model, IDbTransaction tran)
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
          

            return rst;
        }

        /// <summary>
        /// 从数据库删除记录(事务)
        /// </summary>
        public bool Delete(int PositionID)
        {
            return Delete(null, PositionID);
        }

        /// <summary>
        /// 从数据库删除记录(事务)
        /// </summary>
        public bool Delete(IDbTransaction tran, int PositionID)
        {
            int affectedrows = 0;

            var p = new DynamicParameters();
            //获取主键
            p.Add("PositionID", PositionID, DbType.Int32, null, 4);

            string sql = @"DELETE FROM [CPM_Position]
                        WHERE 	[PositionID] = @PositionID
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
        public List<Position> GetAll(string orderBy)
        {
            return GetList(null, null, string.Empty, orderBy);
        }

        /// <summary>
        /// 查询所有记录，并排序(事务)
        /// </summary>
        public List<Position> GetList(IDbConnection conn, int? top, string strWhere, string orderBy)
        {
            if (conn == null)
                conn = DbService();

            StringBuilder strSql = new StringBuilder();
            strSql.Append("Select ");
            if (top.HasValue)
            {
                strSql.Append(" top " + top.ToString());
            }
            strSql.Append(@" 	[PositionID],
	[PositionCode],
	[PositionName],
	[PositionId],
	[Descn],
	[Attribute],
	[ViewOrd],
	[CreateTime],
	[UpdateTime],
	[Creator],
    [ParentID],
    [ParentPath],
	[Modifitor],
	[Status],
    [PositionLevel]
 ");
            strSql.Append(" FROM [CPM_Position] ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            if (orderBy != string.Empty)
                strSql.Append(" order by " + orderBy);


            var rst = conn.Query<Position>(strSql.ToString(), null);

            return new List<Position>(rst);
        }

        /// <summary>
        /// 查询所有记录，并排序、分页
        /// </summary>
        public List<Position> GetAllPaged(IDbConnection conn, int PageSize, int PageIndex, string strWhere, bool desc, string orderfldname)
        {
            int rstcount = 0;
            return GetAllPaged(null, PageSize, PageIndex, strWhere, desc, orderfldname, out rstcount);
        }

        /// <summary>
        /// 查询所有记录，并排序、分页,没有排序字段
        /// </summary>
        public List<Position> GetAllPaged(int PageSize, int PageIndex, string strWhere, bool desc)
        {
            return GetAllPaged(null, PageSize, PageIndex, strWhere, desc, "PositionID");
        }

        /// <summary>
        /// 查询所有记录，并排序、分页,没有排序字段
        /// </summary>
        public List<Position> GetAllPaged(int PageSize, int PageIndex, string strWhere, bool desc, out int rstcount)
        {
            return GetAllPaged(null, PageSize, PageIndex, strWhere, desc, "PositionID", out rstcount);
        }

        /// <summary>
        /// 查询所有记录，并排序、分页(事务)
        /// </summary>
        public List<Position> GetAllPaged(IDbConnection conn, int PageSize, int PageIndex, string strWhere, bool desc, string orderfldname, out int rstcount)
        {
            string selectsql = @"(select a.*,b.PositionName ParentPositionName,cg.GroupName ParentGroupName 
    ,cg2.ParentPath GroupParentPath,cg2.GroupName,cg2.GroupCode
  from CPM_Position a 
  left join CPM_Position b on a.ParentID = b.PositionID
  left join CPM_Group cg on cg.GroupID = b.GroupId
  left join CPM_Group cg2 on cg2.GroupID = a.GroupId)t";
            return Clover.Data.BaseDAO.GetList<Position>(selectsql, "PositionID", PageSize, PageIndex, strWhere, desc, orderfldname, out rstcount);
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
            string strSQL = "select count(*) from [CPM_Position]";
            strSQL = string.IsNullOrEmpty(strWhere) ? strSQL : strSQL + " where " + strWhere;
            var conn = DbService();
            IEnumerable<int> rst = conn.Query<int>(strSQL, null);
            
            return new List<int>(rst)[0];

        }
        #endregion

        #region 其他自定义方法
        /// <summary>
        /// 从数据库删除记录(事务)
        /// </summary>
        public int DeletePosition(int ID, IDbTransaction tran)
        {
            var p = new DynamicParameters();
            //获取主键
            p.Add("PositionID", ID, DbType.Int32, null, 4);

            StringBuilder builder = new StringBuilder();
            builder.Append("DELETE FROM CPM_Position_User WHERE PositionID = @PositionID ");
            builder.Append("DELETE FROM CPM_PositionMaster WHERE MasterPositionID = @PositionID ");
            builder.Append("DELETE FROM CPM_Position WHERE PositionID = @PositionID ");

            return tran.Connection.Execute(builder.ToString(), p, tran, null, null);
        }

        public Position GetPositionByCode(string PositionCode)
        {
            Position model = null;
            var p = new DynamicParameters();
            //获取主键
            p.Add("PositionCode", PositionCode, DbType.String, null, 20);
            var conn = DbService();
            try
            {
                var rst = conn.Query<Position>(
                @"select * from CPM_Position where [PositionCode] = @PositionCode  And Status > 0", p);

                model = new List<Position>(rst)[0];
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

        public List<Position> GetPositionByUser(string userId)
        {
            string str = "SELECT a.* FROM CPM_Position a INNER JOIN CPM_Position_User b ON a.PositionID = b.PositionID WHERE b.UserID = @UserID And Status > 0";
            var conn = DbService();
            var rst = conn.Query<Position>(str, new { UserID = userId });
            
            return new List<Position>(rst);
        }

        public List<Position> GetPositionByMaster(int MasterPositionId)
        {
            string str = @"SELECT * FROM CPM_Position cp
WHERE cp.PositionID IN (
SELECT cpm.SubPositionID FROM CPM_PositionMaster cpm
WHERE cpm.MasterPositionID = @PositionID) And Status > 0";
            var conn = DbService();
            var rst = conn.Query<Position>(str, new { PositionID = MasterPositionId });
            
            return new List<Position>(rst);
        }

       
        /// <summary>
        /// 查询所有记录，并排序、分页(事务)
        /// </summary>
        public List<Position> GetGroupPositionAllPaged(int PageSize, int PageIndex, string strWhere, bool desc, string orderfldname, out int rstcount)
        {
            string selectsql = @" (select * from (select distinct (cg.GroupCode+a.PositionCode) PMKey, a.PositionID, a.PositionCode, a.PositionName ,null ParentPositionName,null ParentGroupName 
    ,cg.ParentPath GroupParentPath,cg.GroupName,cg.GroupCode,a.ViewOrd, a.PositionLevel
  from CPM_Position_User cpu 
  left join CPM_Position a on cpu.PositionID = a.PositionID
  left join CPM_Group cg on cg.GroupID = cpu.GroupId
  )t  where pmkey is not null)x";
            //return Clover.Data.BaseDAO.GetList<Position>(selectsql, "PMKey", PageSize, PageIndex, strWhere, desc, orderfldname, out rstcount);
            return GetList<Position>(selectsql, "PMKey", PageSize, PageIndex, strWhere, desc, orderfldname, out rstcount);
        }

        public int AddPositionByMaster(int MasterPositionId, params string[] subPositionIds)
        {
            var conn = DbService();
            StringBuilder sb = new StringBuilder();
            foreach(var subId in subPositionIds){
                sb.AppendFormat(" Insert into [CPM_PositionMaster] SELECT '{0}','{1}' where not Exists (select 1 from [CPM_PositionMaster] where MasterPositionID='{0}' and SubPositionID='{1}');", MasterPositionId, subId);
            }
            return conn.Execute(
              sb.ToString(), null);
        }

        public int DeletePositionByMaster(int MasterPositionId, params string[] subPositionIds)
        {
            var conn = DbService();
            return conn.Execute(
                string.Format("DELETE [CPM_PositionMaster] Where SubPositionID in ('{0}') AND MasterPositionID = {1}"
                , Clover.Core.Common.StringHelper.Join("','", subPositionIds)
                , MasterPositionId), null);
        }

        /// <summary>
        /// 查询所有记录，并排序(事务)
        /// </summary>
        public List<Position> GetListWithOpen(IDbConnection conn, int? top, string strWhere, string orderBy)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("Select ");
            if (top.HasValue)
            {
                strSql.Append(" top " + top.ToString());
            }
            strSql.Append(@" * ");
            strSql.Append(" FROM [CPM_Position] ");

            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            if (orderBy != string.Empty)
                strSql.Append(" order by " + orderBy);

            var rst = conn.Query<Position>(strSql.ToString(), null);

            return new List<Position>(rst);
        }

        public void RefreshPositionParentPath(int Positionid, string path, IDbTransaction tran)
        {

            tran.Connection.Execute("UPDATE [CPM_Position] SET ParentPath = @ParentPath WHERE PositionID = @PositionID",
                  new { PositionID = Positionid, ParentPath = path }, tran, null, CommandType.Text);

        }


        public void UpChildPosition(int PositionID, int parentPositionID, IDbTransaction tran)
        {
            tran.Connection.Execute("UPDATE [CPM_Position] SET ParentID = @ParentPositionID WHERE ParentID = @PositionID",
                          new { PositionID = PositionID, ParentPositionID = parentPositionID }, tran, null, null);

        }
        #endregion
    }
}