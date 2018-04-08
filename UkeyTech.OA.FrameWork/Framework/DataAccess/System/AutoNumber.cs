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
	/// AutoNumber 数据访问层
	/// </summary>
	public partial class AutoNumberDAO : Clover.Data.BaseDAO
	{
		#region 构造函数
		public AutoNumberDAO()
		{
		}
		#endregion		

		#region 根据主键创建 AutoNumber 数据模型实例
		/// <summary>
		/// 根据主键创建 AutoNumber 数据模型实例 
		/// </summary>
		public AutoNumber GetModel(int ID)
		{           
            var p = new DynamicParameters();
            //获取主键
            p.Add("ID",ID ,DbType.Int32,null,4);
            
            var conn = DbService();
            
            try{
                var rst = conn.Query<AutoNumber>(
                @"select * from sys_AutoNumber where 	[ID] = @ID
", p);
                
                return new List<AutoNumber>(rst)[0];
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
		public  bool Update(AutoNumber model)
		{
			return Update(null,model);
		}
		/// <summary>
		/// 更新记录到数据库(利用事务)
		/// </summary>
		public  bool Update(IDbConnection conn, AutoNumber model)
		{		
			int affectedrows = 0;
           
            var p = new DynamicParameters();           
            p.Add("ID",model.ID ,DbType.Int32,null,4);
            p.Add("Target",model.Target ,DbType.String,null,50);
            p.Add("Number",model.Number ,DbType.String,null,10);
            p.Add("Date",model.Date ,DbType.DateTime,null,8);
            
                string sql = @"UPDATE [sys_AutoNumber] SET
	[Target] = @Target,
	[Number] = @Number,
	[Date] = @Date
                    WHERE
                        	[ID] = @ID
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
		public  bool Insert(AutoNumber model)
		{
			return Insert(null,model);
		}
		
		/// <summary>
		/// 新增记录到数据库(事务)
		/// </summary>
		public bool Insert(IDbConnection conn,AutoNumber model)
		{
            var p = new DynamicParameters();           
            p.Add("ID",model.ID ,DbType.Int32,null,4);
            p.Add("Target",model.Target ,DbType.String,null,50);
            p.Add("Number",model.Number ,DbType.String,null,10);
            p.Add("Date",model.Date ,DbType.DateTime,null,8);
            
        
            string sql = @"INSERT INTO [sys_AutoNumber] (                          
	[Target],
	[Number],
	[Date]
                        ) VALUES (
                            	@Target,
	@Number,
	@Date
)";
                            
            sql += ";select @@IDENTITY";
            
            if(conn == null)
                conn = DbService();

            try{
                var keys = new List<decimal>(conn.Query<decimal>(sql, p));
                
                model.ID = Convert.ToInt32(keys[0]);                
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
		public bool Delete(int ID)
		{
			return Delete(null,ID);
		}
		
		
		/// <summary>
		/// 从数据库删除记录(事务)
		/// </summary>
		public  bool Delete(IDbConnection conn,int ID)
		{
			  int affectedrows = 0;
        
            var p = new DynamicParameters();           
		     //获取主键
            p.Add("ID",ID ,DbType.Int32,null,4);
            
            string sql = @"DELETE FROM [sys_AutoNumber]
                        WHERE 	[ID] = @ID
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
		public  List<AutoNumber> GetAll(string orderBy)
		{
			return GetList(null,null, string.Empty, orderBy);
		}
        
		/// <summary>
		/// 查询所有记录，并排序(事务)
		/// </summary>
		public List<AutoNumber> GetList(IDbConnection conn,int? top, string strWhere, string orderBy)
		{
            if(conn == null)
                conn = DbService();
            
            StringBuilder strSql=new StringBuilder();
			strSql.Append("Select ");
			if(top.HasValue)
			{
				strSql.Append(" top "+top.ToString());
			}
			strSql.Append(@" 	[ID],
	[Target],
	[Number],
	[Date]
 ");
			strSql.Append(" FROM [sys_AutoNumber] ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}

            if (orderBy != string.Empty)
                strSql.Append(" order by " + orderBy);
            
            var rst = conn.Query<AutoNumber>(strSql.ToString(), null);
                
            
            
			return new List<AutoNumber>(rst);
		}
	
		/// <summary>
		/// 查询所有记录，并排序、分页
		/// </summary>
		public List<AutoNumber> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname)
		{
            int rstcount = 0;
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc, orderfldname,out rstcount);
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页,没有排序字段
		/// </summary>
		public List<AutoNumber> GetAllPaged(int PageSize,int PageIndex,string strWhere,bool desc)
		{
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc,"ID" );
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页(事务)
		/// </summary>
		public List<AutoNumber> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname, out int rstcount)
		{
		    return Clover.Data.BaseDAO.GetList<AutoNumber>("[sys_AutoNumber]","ID",PageSize, PageIndex, strWhere ,desc, orderfldname, out rstcount);
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
            string strSQL = "select count(*) from [sys_AutoNumber]";
            strSQL = string.IsNullOrEmpty(strWhere)? strSQL : strSQL + " where " + strWhere;
            var conn = DbService();
		    IEnumerable<int> rst = conn.Query<int>(strSQL, null);
            
            return new List<int>(rst)[0];            
            
		}
		#endregion
		
		#region 其他自定义方法

        /// <summary>
        /// 读取信息
        /// </summary>
        public static string GetBillNo(string document)
        {
            return GetBillNo("{document}-{datetime}-{Number}", "001", "0", "yyMM", document);
        }

        /// <summary>
        /// 读取信息
        /// </summary>
        public static string GetBillNo(string document, string startNumber)
        {
            return GetBillNo("{Number}", startNumber, "0", "yyMM", document);
        }

        /// <summary>
        /// 读取信息
        /// </summary>
        public static string GetBillNoSequence(string document)
        {
            return GetBillNo("{Number}", "10000000", "", "yyMM", document);
        }

        /// <summary>
        /// 读取信息
        /// </summary>
        public static string GetBillNo(string format, string startNumber, string padding, string datetimeformat, string document)
        {
            string Number = "";
            DateTime date = DateTime.Now;
            string DateCode = "";


            var conn = DbService();

            StringBuilder strSql = new StringBuilder();
            strSql.Append("Select ");
            strSql.Append(@" 	[ID],
	                        [Target],
	                        [Number],
	                        [Date]
                         ");
            strSql.Append(" FROM [sys_AutoNumber] ");

            var rst = conn.Query<AutoNumber>(strSql.ToString(), null);

            

            List<AutoNumber> list = new List<AutoNumber>(rst);

            bool bo = false;
            DateTime today = DateTime.Today;

            foreach (AutoNumber m in list)
            {
                if (m.Target == document)
                {
                    bo = true;
                    date = m.Date;
                    DateCode = today.ToString(datetimeformat);

                    if (m.Date.AddMonths(1) <= DateTime.Now)//隔天序号从001起
                    {
                        m.Date = today;
                        m.Number = startNumber;

                        SetBillNo(document, startNumber);//修改数据库

                        return GetFormatValue(format, DateCode, startNumber, document);
                    }

                    Number = (!string.IsNullOrEmpty(m.Number)) ? m.Number : "0";
                }
            }

            //找不到标识，新建
            if (!bo)
            {
                Number = startNumber;
                date = today;
                insert(document, Number, date + "-01");//修改数据库

                DateCode = today.ToString(datetimeformat);

                return GetFormatValue(format, DateCode, Number, document);
            }

            Number = (Convert.ToInt32(Number) + 1).ToString();

            while (Number.Length < startNumber.Length)
            {
                Number.Insert(0, padding);
            }

            SetBillNo(document, Number);//修改数据库

            return GetFormatValue(format, DateCode, Number, document);
        }

        private static string GetFormatValue(string format, string datecode, string number, string document)
        {
            return format.Replace("{datetime}", datecode).Replace("{Number}", number).Replace("{document}", document);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        private static void SetBillNo(string document, string Number)
        {
            Update(document, Number, DateTime.Today.ToString("yyyy-MM-") + "01");
        }


        /// <summary>
        /// 新增记录到数据库(事务)
        /// </summary>
        public static bool insert(string document, string number, string Date)
        {
            var p = new DynamicParameters();

            p.Add("Target", document, DbType.String, null, 50);
            p.Add("Number", number, DbType.String, null, 10);
            p.Add("Date", Date, DbType.DateTime, null, 8);


            string sql = @"INSERT INTO [sys_AutoNumber] (                          
	                        [Target],
	                        [Number],
	                        [Date]
                              ) VALUES ( @Target,
	                        @Number,
	                        @Date
                        )";


            var conn = DbService();
            conn.Execute(sql, p);
            

            return true;
        }

        /// <summary>
        /// 保存配置信息
        /// </summary>
        /// <param name="document">单号前缀</param>
        /// <param name="number">3位序号</param>
        /// <param name="Date">日期</param>
        private static void Update(string document, string number, string Date)
        {
            var p = new DynamicParameters();
            p.Add("Target", document, DbType.String, null, 50);
            p.Add("Number", number, DbType.String, null, 10);
            p.Add("Date", Date, DbType.DateTime, null, 8);

            string sql = @"UPDATE [sys_AutoNumber] SET
                [Number] = @Number,
                [Date] = @Date
                                WHERE [Target] = @Target
            ";

            var conn = DbService();
            conn.Execute(sql, p);
            

        }

		#endregion
	}
}