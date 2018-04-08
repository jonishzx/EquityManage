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
	/// Group 数据访问层
	/// </summary>
	public partial class GroupDAO : Clover.Data.BaseDAO
	{
		#region 构造函数
		public GroupDAO()
		{
		}
		#endregion		
		
		#region 根据主键创建 Group 数据模型实例
		/// <summary>
		/// 根据主键创建 Group 数据模型实例 
		/// </summary>
		public Group GetModel(int GroupID)
		{           
            Group model = null;
            var p = new DynamicParameters();
            //获取主键
            p.Add("GroupID",GroupID ,DbType.Int32,null,4);
            
            var conn = DbService();
            
            try{
                var rst = conn.Query<Group>(
                @"select * from CPM_Group where [GroupID] = @GroupID", p);
                
                List<Group> lrst 
                    = new  List<Group>(rst);
                
                if(lrst.Count > 0)
                    model = lrst[0];
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
		public  bool Update(Group model)
		{
			return Update(null,model);
		}
		/// <summary>
		/// 更新记录到数据库(利用事务)
		/// </summary>
		public  bool Update(IDbTransaction tran, Group model)
		{		
			int affectedrows = 0;
           
            var p = new DynamicParameters();           
            p.Add("GroupID",model.GroupID ,DbType.Int32,null,4);
            p.Add("GroupCode",model.GroupCode ,DbType.String,null,50);
            p.Add("GroupName",model.GroupName ,DbType.String,null,50);
            p.Add("Descn",model.Descn ,DbType.String,null,200);
            p.Add("Attribute",model.Attribute ,DbType.String,null,16);
            p.Add("ParentID",model.ParentID ,DbType.Int32,null,4);
            p.Add("ParentPath",model.ParentPath ,DbType.String,null,200);
            p.Add("ViewOrd",model.ViewOrd ,DbType.Int32,null,4);
            p.Add("CreateTime",model.CreateTime ,DbType.DateTime,null,8);
            p.Add("UpdateTime",model.UpdateTime ,DbType.DateTime,null,8);
            p.Add("Creator",model.Creator ,DbType.String,null,20);
            p.Add("Modifitor",model.Modifitor ,DbType.String,null,50);
            p.Add("Status",model.Status ,DbType.Int32,null,4);
            p.Add("FullName", model.FullName, DbType.String, null, 100);

                string sql = @"UPDATE [CPM_Group] SET
	[GroupCode] = @GroupCode,
	[GroupName] = @GroupName,
	[Descn] = @Descn,
	[Attribute] = @Attribute,
	[ParentID] = @ParentID,
	[ParentPath] = @ParentPath,
	[ViewOrd] = @ViewOrd,
	[CreateTime] = @CreateTime,
	[UpdateTime] = @UpdateTime,
	[Creator] = @Creator,
	[Modifitor] = @Modifitor,
	[Status] = @Status,
    [FullName] = @FullName
                    WHERE
                        	[GroupID] = @GroupID
";
            IDbConnection conn = tran == null ?  DbService() : tran.Connection;
         
            try{
                affectedrows = conn.Execute(sql, p);
            }
            catch(DataException ex){
                throw ex;
            }
			
			return Convert.ToBoolean(affectedrows);
		}
		#endregion
		
		#region 新增记录
		/// <summary>
		/// 新增记录到数据库
		/// </summary>
		public  bool Insert(Group model)
		{
			return Insert(null,model);
		}
		
		/// <summary>
		/// 新增记录到数据库(事务)
		/// </summary>
		public bool Insert(IDbTransaction tran, Group model)
		{
            var p = new DynamicParameters();           
            p.Add("GroupID",model.GroupID ,DbType.Int32,null,4);
            p.Add("GroupCode",model.GroupCode ,DbType.String,null,50);
            p.Add("GroupName",model.GroupName ,DbType.String,null,50);
            p.Add("Descn",model.Descn ,DbType.String,null,200);
            p.Add("Attribute",model.Attribute ,DbType.String,null,16);
            p.Add("ParentID",model.ParentID ,DbType.Int32,null,4);
            p.Add("ParentPath",model.ParentPath ,DbType.String,null,200);
            p.Add("ViewOrd",model.ViewOrd ,DbType.Int32,null,4);
            p.Add("CreateTime",model.CreateTime ,DbType.DateTime,null,8);
            p.Add("UpdateTime",model.UpdateTime ,DbType.DateTime,null,8);
            p.Add("Creator",model.Creator ,DbType.String,null,20);
            p.Add("Modifitor",model.Modifitor ,DbType.String,null,50);
            p.Add("Status",model.Status ,DbType.Int32,null,4);
            p.Add("FullName", model.FullName, DbType.String, null, 100);
        
            string sql = @"INSERT INTO [CPM_Group] (                          
	[GroupCode],
	[GroupName],
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
    [FullName]
                        ) VALUES (
                            	@GroupCode,
	@GroupName,
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
    @FullName
)";
                            
            sql += ";select @@IDENTITY";
            
            IDbConnection conn = tran == null ?  DbService() : tran.Connection;

            try{
                var keys = new List<decimal>(conn.Query<decimal>(sql, p));
                
                model.GroupID = Convert.ToInt32(keys[0]);                
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
		public bool Delete(int GroupID)
		{
			return Delete(null,GroupID);
		}
		
		
		/// <summary>
		/// 从数据库删除记录(事务)
		/// </summary>
		public  bool Delete(IDbTransaction tran, int GroupID)
		{
			  int affectedrows = 0;
        
            var p = new DynamicParameters();           
		     //获取主键
            p.Add("GroupID",GroupID ,DbType.Int32,null,4);
            
            string sql = @"DELETE FROM [CPM_Group]
                        WHERE 	[GroupID] = @GroupID
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
		public  List<Group> GetAll(string orderBy)
		{
			return GetList(null,null, string.Empty, orderBy);
		}
        
		/// <summary>
		/// 查询所有记录，并排序(事务)
		/// </summary>
		public List<Group> GetList(IDbConnection conn,int? top, string strWhere, string orderBy)
		{
            if(conn == null)
                conn = DbService();
            
            StringBuilder strSql=new StringBuilder();
			strSql.Append("Select ");
			if(top.HasValue)
			{
				strSql.Append(" top "+top.ToString());
			}
			strSql.Append(@" 	[GroupID],
	[GroupCode],
	[GroupName],
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
    [FullName]
 ");
			strSql.Append(" FROM [CPM_Group] ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
           
            if (orderBy != string.Empty)
                strSql.Append(" order by " + orderBy);
			
            
            var rst = conn.Query<Group>(strSql.ToString(), null);
                
			return new List<Group>(rst);
		}
	
		/// <summary>
		/// 查询所有记录，并排序、分页
		/// </summary>
		public List<Group> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname)
		{
            int rstcount = 0;
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc, orderfldname,out rstcount);
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页,没有排序字段
		/// </summary>
		public List<Group> GetAllPaged(int PageSize,int PageIndex,string strWhere,bool desc)
		{
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc,"GroupID" );
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页(事务)
		/// </summary>
		public List<Group> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname, out int rstcount)
		{
		    return Clover.Data.BaseDAO.GetList<Group>("[CPM_Group]","GroupID",PageSize, PageIndex, strWhere ,desc, orderfldname, out rstcount);
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
            string strSQL = "select count(*) from [CPM_Group]";
            strSQL = string.IsNullOrEmpty(strWhere)? strSQL : strSQL + " where " + strWhere;
            var conn = DbService();
		    IEnumerable<int> rst = conn.Query<int>(strSQL, null);
            
            return new List<int>(rst)[0];            
            
		}
		#endregion
		
		#region 其他自定义方法
        public void RefreshParentPath(int Groupid, string path, IDbTransaction tran)
        {

            tran.Connection.Execute("UPDATE [CPM_Group] SET ParentPath = @ParentPath WHERE GroupID = @GroupID",
                  new { GroupID = Groupid, ParentPath = path }, tran, null, CommandType.Text);

        }

        public void UpChildren(int GroupId, int parentGroupId, IDbTransaction tran)
        {
            tran.Connection.Execute("UPDATE [CPM_Group] SET ParentID = @ParentGroupID WHERE ParentID = @GroupID",
                          new { GroupID = GroupId, ParentGroupID = parentGroupId }, tran, null, null);

        }

        /// <summary>
        /// 从数据库删除记录(事务)
        /// </summary>
        public int DeleteGroup(int ID, IDbTransaction tran)
        {
            var p = new DynamicParameters();
            //获取主键
            p.Add("GroupID", ID, DbType.Int32, null, 4);

            StringBuilder builder = new StringBuilder();
            builder.Append("DELETE FROM CPM_Group_Role WHERE GroupID = @GroupID ");
            builder.Append("DELETE FROM CPM_Group_User WHERE GroupID = @GroupID ");
            builder.Append("DELETE FROM CPM_FuncPermission WHERE GroupID = @GroupID ");
            builder.Append("DELETE FROM CPM_Group WHERE GroupID = @GroupID ");

            return tran.Connection.Execute(builder.ToString(), p, tran, null, null);
        }

        public List<Group> GetGroupByNoUserRef(string userId, string where)
        {
            string str = "Select * FROM　[CPM_Group] Where GroupID NOT IN (SELECT GroupID From CPM_Group_User WHERE UserID = @UserID)";
            if (!string.IsNullOrEmpty(where))
            {
                str = str + " AND " + where;
            }

            var conn = DbService();
            var rst = conn.Query<Group>(str, new { UserID = userId });
            
            return new List<Group>(rst);
        }

        public Group GetGroupByCode(string GroupCode)
        {
            Group model = null;
            var p = new DynamicParameters();
            //获取主键
            p.Add("GroupCode", GroupCode, DbType.String, null, 20);
            var conn = DbService();
            try
            {
                var rst = conn.Query<Group>(
                @"select * from CPM_Group where [GroupCode] = @GroupCode  And Status > 0", p);

                model = new List<Group>(rst)[0];
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

        public List<Group> GetGroupByUser(string userId)
        {
            string str = "SELECT a.* FROM CPM_Group a INNER JOIN CPM_Group_User b ON a.GroupID = b.GroupID WHERE b.UserID = @UserID And Status > 0";
            var conn = DbService();
            var rst = conn.Query<Group>(str, new { UserID = userId });
            
            return new List<Group>(rst);
        }

        /// <summary>
        /// 查询所有记录，并排序(事务)
        /// </summary>
        public List<Group> GetListWithOpen(IDbConnection conn, int? top, string strWhere, string orderBy)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("Select ");
            if (top.HasValue)
            {
                strSql.Append(" top " + top.ToString());
            }
            strSql.Append(@" 		[GroupID],
	[GroupCode],
	[GroupName],
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
    [FullName]
 ");
            strSql.Append(" FROM [CPM_Group] ");

            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            if (orderBy != string.Empty)
                strSql.Append(" order by " + orderBy);

            var rst = conn.Query<Group>(strSql.ToString(), null);

            return new List<Group>(rst);
        }
		#endregion
	}
}