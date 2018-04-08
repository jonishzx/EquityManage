namespace Clover.Permission.DAO
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Text;
    using System.Linq;
    using Dapper;
    using Dapper.Contrib;
    using Dapper.Contrib.Extensions;
    using Clover.Permission.Model;
    using Clover.Permission.DAO;
    using Clover.Core.Caching;
   
    /// <summary>
    /// FuncPermission 数据访问层
    /// </summary>
    partial class FunctionDataRuleDAO : Clover.Data.BaseDAO
    {
       #region 构造函数
		public FunctionDataRuleDAO()
		{
		}
		#endregion		
		
		#region 根据主键创建 FunctionDataRule 数据模型实例
		/// <summary>
		/// 根据主键创建 FunctionDataRule 数据模型实例 
		/// </summary>
		public FunctionDataRule GetModel(int DataPermissionId)
		{           
            var conn = DbService();
            
            try{
                var rst = conn.Get<FunctionDataRule>(DataPermissionId);
                return rst;
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
		public  bool Update(FunctionDataRule model)
		{
			return Update(null,model);
		}
		/// <summary>
		/// 更新记录到数据库(利用事务)
		/// </summary>
		public  bool Update(IDbTransaction tran, FunctionDataRule model)
		{		
		
            IDbConnection conn = tran == null ?  DbService() : tran.Connection;
            bool rst = false;
            try{
                rst = conn.Update(model, tran);
            }
            catch(DataException ex){
                throw ex;
            }finally{
                
            }
			
			return rst;
		}
		#endregion
		
		#region 新增记录
		/// <summary>
		/// 新增记录到数据库
		/// </summary>
		public  bool Insert(FunctionDataRule model)
		{
			return Insert(null,model);
		}
		
		/// <summary>
		/// 新增记录到数据库(事务)
		/// </summary>
		public bool Insert(IDbTransaction tran, FunctionDataRule model)
		{
         
            IDbConnection conn = tran == null ?  DbService() : tran.Connection;

            try{
                long id = conn.Insert(model, tran);
                    model.DataPermissionId = (int)id;
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
		public bool Delete(FunctionDataRule model)
		{
			return Delete(model, null);
		}
        
         /// <summary>
		/// 从数据库删除记录(事务)
		/// </summary>
		public bool Delete(FunctionDataRule model, IDbTransaction tran )
		{
            IDbConnection conn = tran == null ?  DbService() : tran.Connection;
            bool rst = false;
            try{
                rst = conn.Delete(model, tran);
            }
            catch(DataException ex){
                throw ex;
            }finally{
                
            }

			return rst;
		}
        
		/// <summary>
		/// 从数据库删除记录(事务)
		/// </summary>
		public bool Delete(int DataPermissionId)
		{
			return Delete(null,DataPermissionId);
		}
		
		/// <summary>
		/// 从数据库删除记录(事务)
		/// </summary>
		public  bool Delete(IDbTransaction tran, int DataPermissionId)
		{
			int affectedrows = 0;
        
            var p = new DynamicParameters();           
		     //获取主键
            p.Add("DataPermissionId",DataPermissionId ,DbType.Int32,null,4);
            
            string sql = @"DELETE FROM [CPM_FunctionDataRule]
                        WHERE 	[DataPermissionId] = @DataPermissionId
";
            
            IDbConnection conn = tran == null ?  DbService() : tran.Connection;

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
		public  List<FunctionDataRule> GetAll(string orderBy)
		{
			return GetList(null,null, string.Empty, orderBy);
		}
        
		/// <summary>
		/// 查询所有记录，并排序(事务)
		/// </summary>
		public List<FunctionDataRule> GetList(IDbConnection conn,int? top, string strWhere, string orderBy)
		{
            if(conn == null)
                conn = DbService();
            
            StringBuilder strSql=new StringBuilder();
			strSql.Append("Select ");
			if(top.HasValue)
			{
				strSql.Append(" top "+top.ToString());
			}
			strSql.Append(@" 	[DataPermissionId],
	[ModuleID],
	[Code],
	[AllowAction],
	[DenyAction],
	[Name],
	[Priority],
	[DataRule],
	[Descn],
	[Status]
 ");
			strSql.Append(" FROM [CPM_FunctionDataRule] ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
           
            if (orderBy != string.Empty)
                strSql.Append(" order by " + orderBy);
			
            
            var rst = conn.Query<FunctionDataRule>(strSql.ToString(), null);
                
            
            
			return new List<FunctionDataRule>(rst);
		}
	
		/// <summary>
		/// 查询所有记录，并排序、分页
		/// </summary>
		public List<FunctionDataRule> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere)
		{
            int rstcount = 0;
			return GetAllPaged( null, PageSize, PageIndex, strWhere, false,out rstcount);
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页(事务)
		/// </summary>
		public List<FunctionDataRule> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc, out int rstcount)
		{
            return Clover.Data.BaseDAO.GetList<FunctionDataRule>("[CPM_FunctionDataRule]", "DataPermissionId", PageSize, PageIndex, strWhere, false, "Priority", out rstcount);
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
            string strSQL = "select count(*) from [CPM_FunctionDataRule]";
            strSQL = string.IsNullOrEmpty(strWhere)? strSQL : strSQL + " where " + strWhere;
            var conn = DbService();
		    IEnumerable<int> rst = conn.Query<int>(strSQL, null);
            
            return new List<int>(rst)[0];            
            
		}
		#endregion
		
		#region 其他自定义方法
        public List<FunctionDataRule> GetUserDataRule(int FuncPermissionID)
        {
            string strSQL = @"select *, (case when cfp.DataPermissionId is not null then 1 else 0 end)IsSelected   
                                from CPM_FunctionDataRule cfd
                                left join dbo.CPM_FuncPermission cfp on FuncPermissionID = @FuncPermissionID
                                and cfd.DataPermissionId = cfp.DataPermissionId";

            var conn = DbService();
            var p = new DynamicParameters();
            p.Add("FuncPermissionID", FuncPermissionID);
            var rst = conn.Query<FunctionDataRule>(strSQL, p).ToList() ;
            
            return rst;
        }
        
        public static string GetUserDataRuleStrByCode(string datarulecode) {

            Clover.Core.Caching.ICacheBacker backer = StructureMap.ObjectFactory.GetInstance<ICacheBacker>();
            string key = "DataRuleCode_" + datarulecode;

            if (backer.Contains(key))
                return backer.Get(key) as string;

            string strSQL = @"select DataRule
                                from CPM_FunctionDataRule where Code = @Code and Status = 1";

            var conn = DbService();
            var p = new DynamicParameters();
            p.Add("Code", datarulecode);

            var rst = conn.Query<string>(strSQL, p).First();

            if (!string.IsNullOrEmpty(rst))
                backer.Set(key, rst, DateTime.Now.AddHours(1));

            return rst;
        }



        public string GetUserDataRuleStr(int DataRuleID)
        {
            Clover.Core.Caching.ICacheBacker backer = StructureMap.ObjectFactory.GetInstance<ICacheBacker>();
            string key = "DataRule_" + DataRuleID;

            if (backer.Contains(key))
                return backer.Get(key) as string;

            string strSQL = @"select DataRule
                                from CPM_FunctionDataRule where DataPermissionId = @DataPermissionId and Status = 1";

            var conn = DbService();
            var p = new DynamicParameters();
            p.Add("DataPermissionId", DataRuleID);
            var rst = conn.Query<string>(strSQL, p).First();

            if(!string.IsNullOrEmpty(rst))
                backer.Set(key, rst, DateTime.Now.AddHours(1));

            return rst;
        }


        public string GetUserDataRuleStrings(string DataRuleIDs)
        {
            Clover.Core.Caching.ICacheBacker backer = StructureMap.ObjectFactory.GetInstance<ICacheBacker>();
            string key = "DataRule_" + DataRuleIDs;

            if (backer.Contains(key))
                return backer.Get(key) as string;

            string strSQL = string.Format(@"select top 1 DataRule
                                from CPM_FunctionDataRule where DataPermissionId in({0}) and Status = 1 order by Priority",DataRuleIDs);

            var conn = DbService();
            var p = new DynamicParameters();
            //p.Add("DataPermissionId", DataRuleIDs);
            var rst = conn.Query<string>(strSQL, p).First();

            if (!string.IsNullOrEmpty(rst))
                backer.Set(key, rst, DateTime.Now.AddHours(1));

            return rst;
        }

        /// <summary>
        /// 获取数据权限的输出
        /// 如数据权限为(Creator in(   select cpu.UserID from CPM_Position cp    join (     select cpu.PositionID,cp2.ParentPath from CPM_Position_User cpu     join CPM_Position cp2 on cpu.PositionID = cp2.PositionID     where cpu.UserID =  #env.CurrentUser.UserId#    )t on  '\' + cp.ParentPath + '\' like '%\' + t.ParentPath + '\%'       and LEN(cp.ParentPath) > LEN(t.ParentPath)    join CPM_Position_User cpu on cp.PositionID = cpu.PositionID   where cp.Status = 1   )  or Creator = #env.CurrentUser.UserId#)
        /// 该方法会先查询出该内容，然后将该内容转换成条件，如果我想获取应用了该条件的查询内容，如sys_admin表，则会替换语句为
        /// select AdminId from sys_admin where (AdminId in(   select cpu.UserID from CPM_Position cp    join (     select cpu.PositionID,cp2.ParentPath from CPM_Position_User cpu     join CPM_Position cp2 on cpu.PositionID = cp2.PositionID     where cpu.UserID =  #env.CurrentUser.UserId#    )t on  '\' + cp.ParentPath + '\' like '%\' + t.ParentPath + '\%'       and LEN(cp.ParentPath) > LEN(t.ParentPath)    join CPM_Position_User cpu on cp.PositionID = cpu.PositionID   where cp.Status = 1   )  or Creator = #env.CurrentUser.UserId#)
        /// </summary>
        /// <param name="DataRuleID">数据权限ID</param>
        /// <param name="targetTable">可用于该数据权限的数据来源表</param>
        /// <param name="selectField">选择的字段</param>
        /// <param name="targetField">目标替换的字段</param>
        /// <param name="replaceField">替换的字段</param>
        /// <returns></returns>
        public List<string> GetUserDataRuleOutput(Clover.Core.Domain.IAppContext context,int DataRuleID, string targetTable, string selectField, string targetField, string replaceField)
        {
            string strSQL = @"select DataRule
                                from CPM_FunctionDataRule where DataPermissionId = @DataPermissionId and Status = 1";

            var conn = DbService();
            var p = new DynamicParameters();
            p.Add("DataPermissionId", DataRuleID);
            var rst = conn.Query<string>(strSQL, p).First();

            //结果操控,将查找到的目标字符串替换为指定内容
            rst = rst.Replace(targetField, replaceField);
            var newsql = Clover.Data.BaseDAO.ParseSQLCommand(context, string.Format("select {0} from {1} where {2}",
                selectField, targetTable, rst));

            var rst2 = conn.Query<string>(newsql).ToList();
            
            return rst2;
        }
		#endregion
	}
}