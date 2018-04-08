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
	/// DBBackup 数据访问层
	/// </summary>
	public partial class DBBackupDAO : Clover.Data.BaseDAO
	{
		#region 构造函数
		public DBBackupDAO()
		{
		}
		#endregion		
		
		#region 根据主键创建 DBBackup 数据模型实例
		/// <summary>
		/// 根据主键创建 DBBackup 数据模型实例 
		/// </summary>
		public DBBackup GetModel(int id)
		{           
            DBBackup model = null;
            var p = new DynamicParameters();
            //获取主键
            p.Add("id",id ,DbType.Int32,null,4);
            
            var conn = DbService();
            
            try{
                var rst = conn.Query<DBBackup>(
                @"select * from sys_DBBackup where 	[id] = @id
", p);
                
                model = new List<DBBackup>(rst)[0];
            }
            catch(DataException ex){
                throw ex;
            }finally{
                
            }
		
			return model;
		}
		#endregion
			
		#region 更新记录
		/// <summary>
		/// 更新记录到数据库
		/// </summary>
		public  bool Update(DBBackup model)
		{
			return Update(null,model);
		}
		/// <summary>
		/// 更新记录到数据库(利用事务)
		/// </summary>
		public  bool Update(IDbConnection conn, DBBackup model)
		{		
			int affectedrows = 0;
           
            var p = new DynamicParameters();           
            p.Add("id",model.id ,DbType.Int32,null,4);
            p.Add("Updatetime",model.Updatetime ,DbType.DateTime,null,8);
            p.Add("FileName",model.FileName ,DbType.String,null,100);
            p.Add("ServerPath",model.ServerPath ,DbType.String,null,250);
            p.Add("OperatorId",model.OperatorId ,DbType.Int32,null,4);
            
                string sql = @"UPDATE [sys_DBBackup] SET
	[Updatetime] = @Updatetime,
	[FileName] = @FileName,
	[ServerPath] = @ServerPath,
	[OperatorId] = @OperatorId
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
		public  bool Insert(DBBackup model)
		{
			return Insert(null,model);
		}
		
		/// <summary>
		/// 新增记录到数据库(事务)
		/// </summary>
		public bool Insert(IDbConnection conn,DBBackup model)
		{
            var p = new DynamicParameters();           
            p.Add("id",model.id ,DbType.Int32,null,4);
            p.Add("Updatetime",model.Updatetime ,DbType.DateTime,null,8);
            p.Add("FileName",model.FileName ,DbType.String,null,100);
            p.Add("ServerPath",model.ServerPath ,DbType.String,null,250);
            p.Add("OperatorId",model.OperatorId ,DbType.Int32,null,4);
            
        
            string sql = @"INSERT INTO [sys_DBBackup] (                          
	[Updatetime],
	[FileName],
	[ServerPath],
	[OperatorId]
                        ) VALUES (
                            	@Updatetime,
	@FileName,
	@ServerPath,
	@OperatorId
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
            
            string sql = @"DELETE FROM [sys_DBBackup]
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
		public  List<DBBackup> GetAll(string orderBy)
		{
			return GetList(null,null, string.Empty, orderBy);
		}
        
		/// <summary>
		/// 查询所有记录，并排序(事务)
		/// </summary>
		public List<DBBackup> GetList(IDbConnection conn,int? top, string strWhere, string orderBy)
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
	[Updatetime],
	[FileName],
	[ServerPath],
	[OperatorId]
 ");
			strSql.Append(" FROM [sys_DBBackup] ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}

            if (orderBy != string.Empty)
                strSql.Append(" order by " + orderBy);
            
            var rst = conn.Query<DBBackup>(strSql.ToString(), null);
                
            
            
			return new List<DBBackup>(rst);
		}
	
		/// <summary>
		/// 查询所有记录，并排序、分页
		/// </summary>
		public List<DBBackup> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname)
		{
            int rstcount = 0;
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc, orderfldname,out rstcount);
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页,没有排序字段
		/// </summary>
		public List<DBBackup> GetAllPaged(int PageSize,int PageIndex,string strWhere,bool desc)
		{
            return GetAllPaged(null, PageSize, PageIndex, strWhere, desc, "Updatetime");
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页(事务)
		/// </summary>
		public List<DBBackup> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname, out int rstcount)
		{
            return Clover.Data.BaseDAO.GetList<DBBackup>("[sys_DBBackup]", "Updatetime", PageSize, PageIndex, strWhere, desc, orderfldname, out rstcount);
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
            string strSQL = "select count(*) from [sys_DBBackup]";
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
        public int Backup(DBBackup model, string databasename)
        {
            var p = new DynamicParameters();
            p.Add("@id", model.id, DbType.Int32, ParameterDirection.Output, 4);
            p.Add("@FileName", model.FileName, DbType.String, null, 100);
            p.Add("@ServerPath", model.ServerPath, DbType.String, null, 250);
            p.Add("@databasename", databasename, DbType.String, null, 250);
            p.Add("@OperatorId", model.OperatorId, DbType.Int32, null, 4);

            var conn = DbService();
            conn.Execute("UP_DBBackup", p,null,null,CommandType.StoredProcedure);
            
            return p.Get<int>("id");
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Restore(string servicename,string databasename, string filepath)
        {

            var p = new DynamicParameters();
            p.Add("DBName", databasename, DbType.String, null, 1000);
            p.Add("BackUpPath", filepath, DbType.String, null, 1000);

            var conn = DbService(servicename);
            int count = conn.Execute("UP_DBRestore", p);
            
            return count;     
        }

        public DataTable CreateList(string dbbackuppath)
        {
            DataTable filedt = new DataTable();
            filedt.Columns.Add("FileName");
            filedt.Columns.Add("DirName");
            filedt.Columns.Add("UpdateTime", typeof(System.DateTime));
            filedt.Columns.Add("FullFileName");

            string[] files = System.IO.Directory.GetFiles(dbbackuppath);
            foreach (string file in files)
            {
                DataRow dr = filedt.NewRow();
                System.IO.FileInfo finfo = new System.IO.FileInfo(file);
                dr["FileName"] = finfo.Name;
                dr["DirName"] = finfo.DirectoryName;
                dr["UpdateTime"] = finfo.LastAccessTime;
                dr["FullFileName"] = finfo.FullName;

                filedt.Rows.Add(dr);
            }

            return filedt;
        }
		#endregion
	}
}