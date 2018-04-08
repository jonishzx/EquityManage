namespace UkeyTech.WebFW.DAO
{
	using System;
    using System.Collections.Generic;
	using System.Data;
	using System.Data.Common;
	using System.Data.SqlClient;
    using System.Text;
	using Dapper;
 
	using UkeyTech.WebFW.Model;
    
	/// <summary>
	/// OPLog 数据访问层
	/// </summary>
	public partial class OPLogDAO : Clover.Data.BaseDAO
	{
		#region 构造函数
		public OPLogDAO()
		{
		}
		#endregion		
		
		#region 根据主键创建 OPLog 数据模型实例
		/// <summary>
		/// 根据主键创建 OPLog 数据模型实例 
		/// </summary>
		public OPLog GetModel(int id)
		{           
            var p = new DynamicParameters();
            //获取主键
            p.Add("id",id ,DbType.Int32,null,4);
            
            var conn = DbService();
            
            try{
                var rst = conn.Query<OPLog>(
                @"select * from sys_OPLog where 	[id] = @id
", p);
                
                return new List<OPLog>(rst)[0];
            }
            catch(DataException ex){
                throw ex;
            }finally{
                
            }
         
		}
		#endregion
			
		#region 更新记录
		/// <summary>
		/// 更新记录到数据库
		/// </summary>
		public  bool Update(OPLog model)
		{
			return Update(null,model);
		}
		/// <summary>
		/// 更新记录到数据库(利用事务)
		/// </summary>
		public  bool Update(IDbConnection conn, OPLog model)
		{		
			int affectedrows = 0;
           
            var p = new DynamicParameters();           
            p.Add("id",model.id ,DbType.Int32,null,4);
            p.Add("UserId", model.UserId, DbType.String, null, 36);
            p.Add("LoginName",model.LoginName ,DbType.String,null,50);
            p.Add("UserName",model.UserName ,DbType.String,null,50);
            p.Add("UserIP",model.UserIP ,DbType.String,null,20);
            p.Add("LogMessage",model.LogMessage ,DbType.String,null,4000);
            p.Add("UpdateTime",model.UpdateTime ,DbType.DateTime,null,8);
            p.Add("LogOPName",model.LogOPName ,DbType.String,null,150);
            
                string sql = @"UPDATE [sys_OPLog] SET
    [UserId]=@UserId,
	[LoginName] = @LoginName,
	[UserName] = @UserName,
	[UserIP] = @UserIP,
	[LogMessage] = @LogMessage,
	[UpdateTime] = @UpdateTime,
	[LogOPName] = @LogOPName
                    WHERE
                        	[id] = @id
";
              
            if(conn == null)
                conn = DbService();

            try{
                affectedrows = conn.Execute(sql, p);
            }
            catch(DataException ex){
                throw ex;
            }finally{
                
            }
			
			return Convert.ToBoolean(affectedrows);
		}
		#endregion
		
		#region 新增记录
		/// <summary>
		/// 新增记录到数据库
		/// </summary>
		public  bool Insert(OPLog model)
		{
			return Insert(null,model);
		}
		
		/// <summary>
		/// 新增记录到数据库(事务)
		/// </summary>
		public bool Insert(IDbConnection conn,OPLog model)
		{
            var p = new DynamicParameters();
            p.Add("UserId", model.UserId, DbType.String, null, 50);
            p.Add("LoginName",model.LoginName ,DbType.String,null,50);
            p.Add("UserName",model.UserName ,DbType.String,null,50);
            p.Add("UserIP",model.UserIP ,DbType.String,null,20);
            p.Add("LogMessage",model.LogMessage ,DbType.String,null,4000);
            p.Add("UpdateTime",model.UpdateTime ,DbType.DateTime,null,8);
            p.Add("LogOPName",model.LogOPName ,DbType.String,null,150);
            
        
            string sql = @"INSERT INTO [sys_OPLog] ( 
    [UserId],                         
	[LoginName],
	[UserName],
	[UserIP],
	[LogMessage],
	[UpdateTime],
	[LogOPName]
                        ) VALUES (
                            	@LoginName,
	@UserName,
	@UserIP,
	@LogMessage,
	@UpdateTime,
	@LogOPName
)";
                            
            sql += ";select @@IDENTITY";
            
            if(conn == null)
                conn = DbService();

            try{
                var keys = new List<decimal>(conn.Query<decimal>(sql, p));
                
                model.id = Convert.ToInt32(keys[0]);                
            }
            catch(DataException ex){
                throw ex;
            }finally{
                
            }

			return true;
		}
		#endregion
		
		#region 删除记录
			/// <summary>
		/// 从数据库删除记录(事务)
		/// </summary>
		public bool Delete(int id)
		{
			return Delete(null,id);
		}
		
		
		/// <summary>
		/// 从数据库删除记录(事务)
		/// </summary>
		public  bool Delete(IDbConnection conn,int id)
		{
			  int affectedrows = 0;
        
            var p = new DynamicParameters();           
		     //获取主键
            p.Add("id",id ,DbType.Int32,null,4);
            
            string sql = @"DELETE FROM [sys_OPLog]
                        WHERE 	[id] = @id
";
            
            if(conn == null)
                conn = DbService();

            try{
                affectedrows = conn.Execute(sql, p);
            }
            catch(DataException ex){
                throw ex;
            }finally{
                
            }

			return Convert.ToBoolean(affectedrows);
		}
		
		#endregion
		
		#region 查询，返回自定义类			
		/// <summary>
		/// 查询所有记录，并排序
		/// </summary>
		public  List<OPLog> GetAll(string orderBy)
		{
			return GetList(null,null, string.Empty, orderBy);
		}
        
		/// <summary>
		/// 查询所有记录，并排序(事务)
		/// </summary>
		public List<OPLog> GetList(IDbConnection conn,int? top, string strWhere, string orderBy)
		{
            if(conn == null)
                conn = DbService();
            
            StringBuilder strSql=new StringBuilder();
			strSql.Append("Select ");
			if(top.HasValue)
			{
				strSql.Append(" top "+top.ToString());
			}
			strSql.Append(@" 	[id],
    [UserId]
	[LoginName],
	[UserName],
	[UserIP],
	[LogMessage],
	[UpdateTime],
	[LogOPName]
 ");
			strSql.Append(" FROM [sys_OPLog] ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
            if (orderBy != string.Empty)
                strSql.Append(" order by " + orderBy);
            
            var rst = conn.Query<OPLog>(strSql.ToString(), null);
                
            
            
			return new List<OPLog>(rst);
		}
	
		/// <summary>
		/// 查询所有记录，并排序、分页
		/// </summary>
		public List<OPLog> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname)
		{
            int rstcount = 0;
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc, orderfldname,out rstcount);
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页,没有排序字段
		/// </summary>
		public List<OPLog> GetAllPaged(int PageSize,int PageIndex,string strWhere,bool desc)
		{
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc,"id" );
		}

        /// <summary>
        /// 查询所有记录，并排序、分页,没有排序字段
        /// </summary>
        public List<OPLog> GetAllPaged(int PageSize, int PageIndex, string strWhere, bool desc, out int rstcount)
        {
            return GetAllPaged(null, PageSize, PageIndex, strWhere, desc, "id desc", out rstcount);
        }
		
		/// <summary>
		/// 查询所有记录，并排序、分页(事务)
		/// </summary>
		public List<OPLog> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname, out int rstcount)
		{
		    return Clover.Data.BaseDAO.GetList<OPLog>("[sys_OPLog]","id",PageSize, PageIndex, strWhere ,desc, orderfldname, out rstcount);
		}
		
		#endregion
				
		#region 计算查询结果记录数
		/// <summary>
		/// 对所有记录进行记录数计算
		/// </summary>
		public  int SumAllCount()
		{           
		    return SumDynamicCount("");
		}
		/// <summary>
		/// 对所有符合条件的记录进行记录数计算
		/// </summary>
		public  int SumDynamicCount(string strWhere)
		{
            string strSQL = "select count(*) from [sys_OPLog]";
            strSQL = string.IsNullOrEmpty(strWhere)? strSQL : strSQL + " where " + strWhere;
            var conn = DbService();
		    IEnumerable<int> rst = conn.Query<int>(strSQL, null);
            
            return new List<int>(rst)[0];            
            
		}
		#endregion
		
		#region 其他自定义方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public static int Log(Clover.Core.Domain.IAccount user, string ip, string message, object reflectobj)
        {
            if (user != null)
            {
                return Log(user.UniqueId,user.AccountCode, user.UserName, ip, message, reflectobj);
            }
            else
                return Log("登出操作","未知用户", "未知用户", ip, message, reflectobj);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public static int Log(string userId, string loginname, string username, string ip, string message, object reflectobj)
        {
            StringBuilder sb = new StringBuilder(1024);


            string msg = string.Empty;

            if (reflectobj != null)
            {
                try
                {
                    msg = Clover.Core.Reflection.DumpBuilder.Dump(reflectobj);
                }
                catch { }
            }

            var p = new DynamicParameters();
            p.Add("UserId", userId, DbType.String, null, 36);
            p.Add("LoginName", loginname, DbType.String, null, 50);
            p.Add("UserName", username, DbType.String, null, 50);
            p.Add("UserIP", ip, DbType.String, null, 20);
            p.Add("LogMessage", msg, DbType.String, null, 4000);        
            p.Add("LogOPName", message, DbType.String, null, 150);


            string sql = @"INSERT INTO [sys_OPLog] (  
                        [UserId],                        
	                    [LoginName],
	                    [UserName],
	                    [UserIP],
	                    [LogMessage],
	                    [LogOPName] ) VALUES (
                        @userId,
                        @LoginName,
	                    @UserName,
	                    @UserIP,
	                    @LogMessage,
	                    @LogOPName
                    )";
            var rst = 0;
            using (var conn = ManualDbService())
            {
                rst = conn.Execute(sql, p);
                conn.Close();
            }
            return rst;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public List<OPLog> Query(int PageSize, int PageIndex, string startdate, string enddate, string searchtext, out int rstcount)
        {
            StringBuilder sb = new StringBuilder(1024);

            string strWhere = string.Format(" convert(varchar(10),UpdateTime,121) between '{0}' and '{1}' and (UserName like '%{2}%' or LogMessage like '%{2}%')"
                , FilterErrChar(startdate)
                , FilterErrChar(enddate)
                , FilterErrChar(searchtext));

            List<OPLog> list = GetAllPaged(PageSize, PageIndex, strWhere, true, out rstcount);
            return list;
        }
		#endregion
	}
}