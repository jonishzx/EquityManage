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
	/// User 数据访问层
	/// </summary>
	public partial class UserDAO : Clover.Data.BaseDAO
	{
		#region 构造函数
		public UserDAO()
		{
		}
		#endregion		
		
		#region 根据主键创建 User 数据模型实例
		/// <summary>
		/// 根据主键创建 User 数据模型实例 
		/// </summary>
		public User GetModel(int UserID)
		{           
            var p = new DynamicParameters();
            //获取主键
            p.Add("UserID",UserID ,DbType.Int32,null,4);
            
            var conn = DbService();
            
            try{
                var rst = conn.Query<User>(
                @"select * from sys_User where 	[UserID] = @UserID
", p);
                
                return new List<User>(rst)[0];
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
		public  bool Update(User model)
		{
			return Update(null,model);
		}
		/// <summary>
		/// 更新记录到数据库(利用事务)
		/// </summary>
		public  bool Update(IDbConnection conn, User model)
		{		
			int affectedrows = 0;
           
            var p = new DynamicParameters();           
            p.Add("UserID",model.UserID ,DbType.Int32,null,4);
            p.Add("UserName",model.UserName ,DbType.String,null,50);
            p.Add("LoginName",model.LoginName ,DbType.String,null,50);
            p.Add("Password",model.Password ,DbType.String,null,32);
            p.Add("Email",model.Email ,DbType.String,null,50);
            p.Add("Joined",model.Joined ,DbType.DateTime,null,8);
            p.Add("LastVisit",model.LastVisit ,DbType.DateTime,null,8);
            p.Add("IP",model.IP ,DbType.String,null,15);
            p.Add("LanguageFile",model.LanguageFile ,DbType.String,null,50);
            p.Add("Phone",model.Phone ,DbType.String,null,20);
            p.Add("QQ",model.QQ ,DbType.String,null,20);
            p.Add("Position",model.Position ,DbType.String,null,50);
            p.Add("Sex",model.Sex ,DbType.String,null,5);
            p.Add("Flags",model.Flags ,DbType.Int32,null,4);
            p.Add("Descn",model.Descn ,DbType.String,null,250);
            p.Add("IsActived",model.IsActived ,DbType.Boolean,null,1);
            p.Add("FPWDQuestion",model.FPWDQuestion ,DbType.String,null,250);
            p.Add("FPWDAnswer",model.FPWDAnswer ,DbType.String,null,100);
            
                string sql = @"UPDATE [sys_User] SET
	[UserName] = @UserName,
	[LoginName] = @LoginName,
	[Password] = @Password,
	[Email] = @Email,
	[Joined] = @Joined,
	[LastVisit] = @LastVisit,
	[IP] = @IP,
	[LanguageFile] = @LanguageFile,
	[Phone] = @Phone,
	[QQ] = @QQ,
	[Position] = @Position,
	[Sex] = @Sex,
	[Flags] = @Flags,
	[Descn] = @Descn,
	[IsActived] = @IsActived,
	[FPWDQuestion] = @FPWDQuestion,
	[FPWDAnswer] = @FPWDAnswer
                    WHERE
                        	[UserID] = @UserID
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
		public  bool Insert(User model)
		{
			return Insert(null,model);
		}
		
		/// <summary>
		/// 新增记录到数据库(事务)
		/// </summary>
		public bool Insert(IDbConnection conn,User model)
		{
            var p = new DynamicParameters();           
            p.Add("UserID",model.UserID ,DbType.Int32,null,4);
            p.Add("UserName",model.UserName ,DbType.String,null,50);
            p.Add("LoginName",model.LoginName ,DbType.String,null,50);
            p.Add("Password",model.Password ,DbType.String,null,32);
            p.Add("Email",model.Email ,DbType.String,null,50);
            p.Add("Joined",model.Joined ,DbType.DateTime,null,8);
            p.Add("LastVisit",model.LastVisit ,DbType.DateTime,null,8);
            p.Add("IP",model.IP ,DbType.String,null,15);
            p.Add("LanguageFile",model.LanguageFile ,DbType.String,null,50);
            p.Add("Phone",model.Phone ,DbType.String,null,20);
            p.Add("QQ",model.QQ ,DbType.String,null,20);
            p.Add("Position",model.Position ,DbType.String,null,50);
            p.Add("Sex",model.Sex ,DbType.String,null,5);
            p.Add("Flags",model.Flags ,DbType.Int32,null,4);
            p.Add("Descn",model.Descn ,DbType.String,null,250);
            p.Add("IsActived",model.IsActived ,DbType.Boolean,null,1);
            p.Add("FPWDQuestion",model.FPWDQuestion ,DbType.String,null,250);
            p.Add("FPWDAnswer",model.FPWDAnswer ,DbType.String,null,100);
            
        
            string sql = @"INSERT INTO [sys_User] (                          
	[UserName],
	[LoginName],
	[Password],
	[Email],
	[Joined],
	[LastVisit],
	[IP],
	[LanguageFile],
	[Phone],
	[QQ],
	[Position],
	[Sex],
	[Flags],
	[Descn],
	[IsActived],
	[FPWDQuestion],
	[FPWDAnswer]
                        ) VALUES (
                            	@UserName,
	@LoginName,
	@Password,
	@Email,
	@Joined,
	@LastVisit,
	@IP,
	@LanguageFile,
	@Phone,
	@QQ,
	@Position,
	@Sex,
	@Flags,
	@Descn,
	@IsActived,
	@FPWDQuestion,
	@FPWDAnswer
)";
                            
            sql += ";select @@IDENTITY";
            
            if(conn == null)
                conn = DbService();

            try{
                var keys = new List<decimal>(conn.Query<decimal>(sql, p));
                
                model.UserID = Convert.ToInt32(keys[0]);                
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
		public bool Delete(int UserID)
		{
			return Delete(null,UserID);
		}
		
		
		/// <summary>
		/// 从数据库删除记录(事务)
		/// </summary>
		public  bool Delete(IDbConnection conn,int UserID)
		{
			  int affectedrows = 0;
        
            var p = new DynamicParameters();           
		     //获取主键
            p.Add("UserID",UserID ,DbType.Int32,null,4);
            
            string sql = @"DELETE FROM [sys_User]
                        WHERE 	[UserID] = @UserID
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
		public  List<User> GetAll(string orderBy)
		{
			return GetList(null,null, string.Empty, orderBy);
		}
        
		/// <summary>
		/// 查询所有记录，并排序(事务)
		/// </summary>
		public List<User> GetList(IDbConnection conn,int? top, string strWhere, string orderBy)
		{
            if(conn == null)
                conn = DbService();
            
            StringBuilder strSql=new StringBuilder();
			strSql.Append("Select ");
			if(top.HasValue)
			{
				strSql.Append(" top "+top.ToString());
			}
			strSql.Append(@" 	[UserID],
	[UserName],
	[LoginName],
	[Password],
	[Email],
	[Joined],
	[LastVisit],
	[IP],
	[LanguageFile],
	[Phone],
	[QQ],
	[Position],
	[Sex],
	[Flags],
	[Descn],
	[IsActived],
	[FPWDQuestion],
	[FPWDAnswer]
 ");
			strSql.Append(" FROM [sys_User] ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + orderBy);
            
            var rst = conn.Query<User>(strSql.ToString(), null);
                
            
            
			return new List<User>(rst);
		}
	
		/// <summary>
		/// 查询所有记录，并排序、分页
		/// </summary>
		public List<User> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname)
		{
            int rstcount = 0;
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc, orderfldname,out rstcount);
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页,没有排序字段
		/// </summary>
		public List<User> GetAllPaged(int PageSize,int PageIndex,string strWhere,bool desc)
		{
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc,"UserID" );
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页(事务)
		/// </summary>
		public List<User> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname, out int rstcount)
		{
		    return Clover.Data.BaseDAO.GetList<User>("[sys_User]","UserID",PageSize, PageIndex, strWhere ,desc, orderfldname, out rstcount);
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
            string strSQL = "select count(*) from [sys_User]";
            strSQL = string.IsNullOrEmpty(strWhere)? strSQL : strSQL + " where " + strWhere;
            var conn = DbService();
		    IEnumerable<int> rst = conn.Query<int>(strSQL, null);
            
            return new List<int>(rst)[0];            
            
		}
		#endregion
		
		#region 其他自定义方法
		
		#endregion
	}
}