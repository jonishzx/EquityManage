namespace Clover.Permission.DAO
{
	using System;
    using System.Collections.Generic;
	using System.Data;
	using System.Data.Common;
	using System.Data.SqlClient;
    using System.Text;
	using Dapper;
 
	using Clover.Permission.Model;
    
	/// <summary>
	/// Group_Role 数据访问层
	/// </summary>
	partial class GroupRoleDAO : Clover.Data.BaseDAO
	{
		#region 构造函数
		public GroupRoleDAO()
		{
		}
		#endregion		
		
		#region 根据主键创建 Group_Role 数据模型实例
		/// <summary>
		/// 根据主键创建 Group_Role 数据模型实例 
		/// </summary>
		public GroupRole GetModel(int RoleID,int GroupID)
		{           
            var p = new DynamicParameters();
            //获取主键
            p.Add("RoleID",RoleID ,DbType.Int32,null,4);
            p.Add("GroupID",GroupID ,DbType.Int32,null,4);
            
            var conn = DbService();
            
            try{
                var rst = conn.Query<GroupRole>(
                @"select * from CPM_Group_Role where 	[RoleID] = @RoleID
	AND [GroupID] = @GroupID
", p);
                
                return new List<GroupRole>(rst)[0];
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
		public  bool Update(GroupRole model)
		{
			return Update(null,model);
		}
		/// <summary>
		/// 更新记录到数据库(利用事务)
		/// </summary>
		public  bool Update(IDbConnection conn, GroupRole model)
		{		
			int affectedrows = 0;
           
            var p = new DynamicParameters();           
            p.Add("RoleID",model.RoleID ,DbType.Int32,null,4);
            p.Add("GroupID",model.GroupID ,DbType.Int32,null,4);
            
                string sql = @"UPDATE [CPM_Group_Role] SET
                    WHERE
                        	[RoleID] = @RoleID
	AND [GroupID] = @GroupID
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
		public  bool Insert(GroupRole model)
		{
			return Insert(null,model);
		}
		
		/// <summary>
		/// 新增记录到数据库(事务)
		/// </summary>
        public bool Insert(IDbTransaction tran, GroupRole model)
		{
            var p = new DynamicParameters();           
            p.Add("RoleID",model.RoleID ,DbType.Int32,null,4);
            p.Add("GroupID",model.GroupID ,DbType.Int32,null,4);
            
        
            string sql = @"INSERT INTO [CPM_Group_Role]                        
                         Select @GroupID,@RoleID where NOT Exists(select 1 from [CPM_Group_Role] Where RoleID=@RoleID and GroupID = @GroupID)";


            IDbConnection conn = tran == null ? DbService() : tran.Connection;

            try{
                conn.Execute(sql, p, tran, null, null);                      
            }
            catch(DataException ex){
                throw ex;
            }

			return true;
		}
		#endregion
		
		#region 删除记录
			/// <summary>
		/// 从数据库删除记录(事务)
		/// </summary>
		public bool Delete(int RoleID,int GroupID)
		{
			return Delete(null,RoleID,GroupID);
		}
		
		
		/// <summary>
		/// 从数据库删除记录(事务)
		/// </summary>
		public  bool Delete(IDbConnection conn,int RoleID,int GroupID)
		{
			  int affectedrows = 0;
        
            var p = new DynamicParameters();           
		     //获取主键
            p.Add("RoleID",RoleID ,DbType.Int32,null,4);
            p.Add("GroupID",GroupID ,DbType.Int32,null,4);
            
            string sql = @"DELETE FROM [CPM_Group_Role]
                        WHERE 	[RoleID] = @RoleID
	AND [GroupID] = @GroupID
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
		public  List<GroupRole> GetAll(string orderBy)
		{
			return GetList(null,null, string.Empty, orderBy);
		}
        
		/// <summary>
		/// 查询所有记录，并排序(事务)
		/// </summary>
		public List<GroupRole> GetList(IDbConnection conn,int? top, string strWhere, string orderBy)
		{
            if(conn == null)
                conn = DbService();
            
            StringBuilder strSql=new StringBuilder();
			strSql.Append("Select ");
			if(top.HasValue)
			{
				strSql.Append(" top "+top.ToString());
			}
			strSql.Append(@" 	[RoleID],
	[GroupID]
 ");
			strSql.Append(" FROM [CPM_Group_Role] ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + orderBy);
            
            var rst = conn.Query<GroupRole>(strSql.ToString(), null);
                
            
            
			return new List<GroupRole>(rst);
		}
	
		/// <summary>
		/// 查询所有记录，并排序、分页
		/// </summary>
		public List<GroupRole> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname)
		{
            int rstcount = 0;
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc, orderfldname,out rstcount);
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页,没有排序字段
		/// </summary>
		public List<GroupRole> GetAllPaged(int PageSize,int PageIndex,string strWhere,bool desc)
		{
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc,"RoleID,GroupID" );
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页(事务)
		/// </summary>
		public List<GroupRole> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname, out int rstcount)
		{
		    return Clover.Data.BaseDAO.GetList<GroupRole>("[CPM_Group_Role]","RoleID,GroupID",PageSize, PageIndex, strWhere ,desc, orderfldname, out rstcount);
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
            string strSQL = "select count(*) from [CPM_Group_Role]";
            strSQL = string.IsNullOrEmpty(strWhere)? strSQL : strSQL + " where " + strWhere;
            var conn = DbService();
		    IEnumerable<int> rst = conn.Query<int>(strSQL, null);
            
            return new List<int>(rst)[0];            
            
		}
		#endregion
		
		#region 其他自定义方法
        public virtual int DeleteGroupRoles(int GroupID, params string[] Roleids)
        {
            var conn = DbService();
            return conn.Execute(
                string.Format("DELETE [CPM_Group_Role] Where RoleID in ({0},0) AND GroupID = {1}"
                , Clover.Core.Common.StringHelper.Join(",", Roleids)
                , GroupID), null);
        }

        public virtual int DeleteRoleGroups(int RoleID, params string[] Groupids)
        {
            var conn = DbService();

            return conn.Execute(
                string.Format("DELETE [CPM_Group_Role] Where RoleID = {1}  AND GroupID  in ({0},0)"
                , Clover.Core.Common.StringHelper.Join("','", Groupids)
                , RoleID), null);
        }
		#endregion
	}
}