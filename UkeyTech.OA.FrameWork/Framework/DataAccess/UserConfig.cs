namespace UkeyTech.WebFW.DAO
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Text;
    using Dapper;
    using System.Transactions;

    using UkeyTech.WebFW.Model;

    /// <summary>
    /// UserConfig 数据访问层
    /// </summary>
    public partial class UserConfigDAO : Clover.Data.BaseDAO
    {
        #region 构造函数
        public UserConfigDAO()
        {
        }
        #endregion

        #region 根据主键创建 UserConfig 数据模型实例
        /// <summary>
        /// 根据主键创建 UserConfig 数据模型实例 
        /// </summary>
        public UserConfig GetModel(int Id)
        {
            UserConfig model = null;
            var p = new DynamicParameters();
            //获取主键
            p.Add("Id", Id, DbType.Int32, null, 4);

            var conn = DbService();

            try
            {
                var rst = conn.Query<UserConfig>(
                @"select * from sys_UserConfig where 	[Id] = @Id
", p);

                List<UserConfig> lrst
                    = new List<UserConfig>(rst);

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
        public bool Update(UserConfig model)
        {
            return Update(null, model);
        }
        /// <summary>
        /// 更新记录到数据库(利用事务)
        /// </summary>
        public bool Update(IDbTransaction tran, UserConfig model)
        {
            int affectedrows = 0;

            var p = new DynamicParameters();
            p.Add("Id", model.Id, DbType.Int32, null, 4);
            p.Add("UserId", model.UserId, DbType.String, null, 36);
            p.Add("ConfigType", model.ConfigType, DbType.String, null, 50);
            p.Add("Title", model.Title, DbType.String, null, 150);
            p.Add("ConfigValue", model.ConfigValue, DbType.String, null, 500);
            p.Add("UpdateTime", model.UpdateTime, DbType.DateTime, null, 8);

            string sql = @"UPDATE [sys_UserConfig] SET
	[UserId] = @UserId,
	[ConfigType] = @ConfigType,
	[Title] = @Title,
	[ConfigValue] = @ConfigValue,
	[UpdateTime] = @UpdateTime
                    WHERE
                        	[Id] = @Id
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
        public bool Insert(UserConfig model)
        {
            return Insert(null, model);
        }

        /// <summary>
        /// 新增记录到数据库(事务)
        /// </summary>
        public bool Insert(IDbTransaction tran, UserConfig model)
        {
            var p = new DynamicParameters();
            p.Add("Id", model.Id, DbType.Int32, null, 4);
            p.Add("UserId", model.UserId, DbType.String, null, 36);
            p.Add("ConfigType", model.ConfigType, DbType.String, null, 50);
            p.Add("Title", model.Title, DbType.String, null, 150);
            p.Add("ConfigValue", model.ConfigValue, DbType.String, null, 500);
            p.Add("UpdateTime", model.UpdateTime, DbType.DateTime, null, 8);


            string sql = @"INSERT INTO [sys_UserConfig] (                          
	[UserId],
	[ConfigType],
	[Title],
	[ConfigValue],
	[UpdateTime]
                        ) VALUES (
                            	@UserId,
	@ConfigType,
	@Title,
	@ConfigValue,
	@UpdateTime
)";

            sql += ";select @@IDENTITY";

            IDbConnection conn = tran == null ? DbService() : tran.Connection;

            try
            {
                var keys = new List<decimal>(conn.Query<decimal>(sql, p,tran, false, null ,null));

                model.Id = Convert.ToInt32(keys[0]);
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
        public bool Delete(int Id)
        {
            return Delete(null, Id);
        }


        /// <summary>
        /// 从数据库删除记录(事务)
        /// </summary>
        public bool Delete(IDbTransaction tran, int Id)
        {
            int affectedrows = 0;

            var p = new DynamicParameters();
            //获取主键
            p.Add("Id", Id, DbType.Int32, null, 4);

            string sql = @"DELETE FROM [sys_UserConfig]
                        WHERE 	[Id] = @Id
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
        public List<UserConfig> GetAll(string orderBy)
        {
            return GetList(null, null, string.Empty, orderBy);
        }

        /// <summary>
        /// 查询所有记录，并排序(事务)
        /// </summary>
        public List<UserConfig> GetList(IDbConnection conn, int? top, string strWhere, string orderBy)
        {
            if (conn == null)
                conn = DbService();

            StringBuilder strSql = new StringBuilder();
            strSql.Append("Select ");
            if (top.HasValue)
            {
                strSql.Append(" top " + top.ToString());
            }
            strSql.Append(@" 	[Id],
	[UserId],
	[ConfigType],
	[Title],
	[ConfigValue],
	[UpdateTime]
 ");
            strSql.Append(" FROM [sys_UserConfig] ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            if (orderBy != string.Empty)
                strSql.Append(" order by " + orderBy);


            var rst = conn.Query<UserConfig>(strSql.ToString(), null);

            

            return new List<UserConfig>(rst);
        }

        /// <summary>
        /// 查询所有记录，并排序、分页
        /// </summary>
        public List<UserConfig> GetAllPaged(IDbConnection conn, int PageSize, int PageIndex, string strWhere, bool desc, string orderfldname)
        {
            int rstcount = 0;
            return GetAllPaged(null, PageSize, PageIndex, strWhere, desc, orderfldname, out rstcount);
        }

        /// <summary>
        /// 查询所有记录，并排序、分页,没有排序字段
        /// </summary>
        public List<UserConfig> GetAllPaged(int PageSize, int PageIndex, string strWhere, bool desc)
        {
            return GetAllPaged(null, PageSize, PageIndex, strWhere, desc, "Id");
        }

        /// <summary>
        /// 查询所有记录，并排序、分页,没有排序字段
        /// </summary>
        public List<UserConfig> GetAllPaged(int PageSize, int PageIndex, string strWhere, bool desc, out int rstcount)
        {
            return GetAllPaged(null, PageSize, PageIndex, strWhere, desc, "Id", out rstcount);
        }

        /// <summary>
        /// 查询所有记录，并排序、分页(事务)
        /// </summary>
        public List<UserConfig> GetAllPaged(IDbConnection conn, int PageSize, int PageIndex, string strWhere, bool desc, string orderfldname, out int rstcount)
        {
            return Clover.Data.BaseDAO.GetList<UserConfig>("[sys_UserConfig]", "Id", PageSize, PageIndex, strWhere, desc, orderfldname, out rstcount);
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
            string strSQL = "select count(*) from [sys_UserConfig]";
            strSQL = string.IsNullOrEmpty(strWhere) ? strSQL : strSQL + " where " + strWhere;
            var conn = DbService();
            IEnumerable<int> rst = conn.Query<int>(strSQL, null);
            
            return new List<int>(rst)[0];

        }
        #endregion

        #region 其他自定义方法
        /// <summary>
        /// 根据主键创建 UserConfig 数据模型实例 
        /// </summary>
        public List<UserConfig> GetUserConfigs(string userId, string configtype)
        {
            var p = new DynamicParameters();
            //获取主键
            p.Add("UserId", userId, DbType.String, null, null);
           
            var conn = DbService();

            try
            {
                var rst = conn.Query<UserConfig>(
                @"select * from sys_UserConfig where [UserId] = @userId and [ConfigType]
 in ('" + configtype + "')", p);

                List<UserConfig> lrst
                    = new List<UserConfig>(rst);

                return lrst;
            }
            catch (DataException ex)
            {
                throw ex;
            }
            finally
            {
                
            }
        }

        /// <summary>
        /// 从数据库删除记录(事务)/
        /// ///
        /// </summary>
        public bool DeleteAllUserConfig(IDbTransaction tran, string configtype,  string UserId)
        {
            int affectedrows = 0;

            var p = new DynamicParameters();
            //获取主键
            p.Add("UserId", UserId, DbType.String, null, null);
            p.Add("Configtype", configtype, DbType.String, null, null);

            string sql = @"DELETE FROM [sys_UserConfig]
                        WHERE 	[UserId] = @UserId AND [Configtype] = @Configtype
";

            IDbConnection conn = tran == null ? DbService() : tran.Connection;

            try
            {
                affectedrows = conn.Execute(sql, p, tran ,null, null);
            }
            catch (DataException ex)
            {
                throw ex;
            }
         
            return Convert.ToBoolean(affectedrows);
        }

           /// <summary>
        /// 根据主键创建 UserConfig 数据模型实例 
        /// </summary>
        public void UpdateUserConfigs(string userid, string configtype, string val)
        {
            var p = new DynamicParameters();

            p.Add("UserId", userid, DbType.String, null, 36);
            p.Add("ConfigType", configtype, DbType.String, null, 50);

            p.Add("ConfigValue", val, DbType.String, null, 500);

            string sql = @"if exists (select 1 from [sys_UserConfig] where [UserId] = @UserId and [ConfigType] = @ConfigType)
                                UPDATE [sys_UserConfig] SET
	                            [ConfigValue] = @ConfigValue,
	                            [UpdateTime] = getdate()
                                WHERE
                        	    [UserId] = @UserId and [ConfigType] = @ConfigType
                            else
                                INSERT INTO [sys_UserConfig] (                          
	                            [UserId],
	                            [ConfigType],
	                            [Title],
	                            [ConfigValue],
	                            [UpdateTime]
                                                    ) VALUES (
                            	                            @UserId,
	                            @ConfigType,
	                            '用户部件布局设置',
	                            @ConfigValue,
	                            getdate()
                            )
";

            DbService().Execute(sql, p);
        }

        

        /// <summary>
        /// 根据主键创建 UserConfig 数据模型实例 
        /// </summary>
        public void SaveUserConfigs(string userid, string configtype, string[] titles, string[] targets)
        {
            using (var ts = new TransactionScope(System.Transactions.TransactionScopeOption.RequiresNew))
            {

                using (var conn = ManualDbService())
                {
                    IDbTransaction tran = null;
                    try
                    {
                        tran = conn.BeginTransaction();

                        //删除所有
                        DeleteAllUserConfig(tran, configtype, userid);

                        //重新添加
                        for (int i = 0; i < titles.Length; i++)
                        {
                            UserConfig model = new UserConfig();
                            model.UserId = userid;
                            model.ConfigType = configtype;
                            model.Title = titles[i];
                            model.UpdateTime = DateTime.Now;
                            model.ConfigValue = targets[i];
                            Insert(tran, model);
                        }

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
        #endregion
    }
}