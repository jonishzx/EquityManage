﻿namespace Clover.Message.DAO
{
	using System;
    using System.Collections.Generic;
	using System.Data;
	using System.Data.Common;
	using System.Data.SqlClient;
    using System.Text;
	using Dapper;
    using Dapper.Contrib.Extensions;
    
	using Clover.Message.Model;
    
	/// <summary>
	/// BoxMessage 数据访问层
	/// </summary>
	public partial class BoxMessageDAO : Clover.Data.BaseDAO
	{
		#region 构造函数
		public BoxMessageDAO()
		{
		}
		#endregion		
		
		#region 根据主键创建 BoxMessage 数据模型实例
		/// <summary>
		/// 根据主键创建 BoxMessage 数据模型实例 
		/// </summary>
		public BoxMessage GetModel(long MessageBoxId,long MessageId,string Receiver)
		{           
            var conn = DbService();
            
            try{
                Dictionary<string,object> keylist = new Dictionary<string,object>();
                 keylist.Add("MessageBoxId",MessageBoxId); 
                 keylist.Add("MessageId",MessageId); 
                 keylist.Add("Receiver",Receiver); 
                var rst = conn.Get<BoxMessage>(keylist);
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
		public  bool Update(BoxMessage model)
		{
			return Update(null,model);
		}
		/// <summary>
		/// 更新记录到数据库(利用事务)
		/// </summary>
		public  bool Update(IDbTransaction tran, BoxMessage model)
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
		public  bool Insert(BoxMessage model)
		{
			return Insert(null,model);
		}
		
		/// <summary>
		/// 新增记录到数据库(事务)
		/// </summary>
		public bool Insert(IDbTransaction tran, BoxMessage model)
		{
         
            IDbConnection conn = tran == null ?  DbService() : tran.Connection;

            try{
                long id = conn.Insert(model, tran);
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
		public bool Delete(BoxMessage model)
		{
			return Delete(model, null);
		}
        
         /// <summary>
		/// 从数据库删除记录(事务)
		/// </summary>
		public bool Delete(BoxMessage model, IDbTransaction tran )
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
		public bool Delete(long MessageBoxId,long MessageId,string Receiver)
		{
			return Delete(null,MessageBoxId,MessageId,Receiver);
		}
		
		/// <summary>
		/// 从数据库删除记录(事务)
		/// </summary>
		public  bool Delete(IDbTransaction tran, long MessageBoxId,long MessageId,string Receiver)
		{
			int affectedrows = 0;
        
            var p = new DynamicParameters();           
		     //获取主键
            p.Add("MessageBoxId",MessageBoxId ,DbType.Int64,null,8);
            p.Add("MessageId",MessageId ,DbType.Int64,null,8);
            p.Add("Receiver",Receiver ,DbType.String,null,36);
            
            string sql = @"DELETE FROM [msg_BoxMessage]
                        WHERE 	[MessageBoxId] = @MessageBoxId
	AND [MessageId] = @MessageId
	AND [Receiver] = @Receiver
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
		public  List<BoxMessage> GetAll(string orderBy)
		{
			return GetList(null,null, string.Empty, orderBy);
		}
        
		/// <summary>
		/// 查询所有记录，并排序(事务)
		/// </summary>
		public List<BoxMessage> GetList(IDbConnection conn,int? top, string strWhere, string orderBy)
		{
            if(conn == null)
                conn = DbService();
            
            StringBuilder strSql=new StringBuilder();
			strSql.Append("Select ");
			if(top.HasValue)
			{
				strSql.Append(" top "+top.ToString());
			}
			strSql.Append(@" 	[MessageBoxId],
	[MessageId],
	[Receiver],
	[Status],
	[Result],
	[ReadTime],
	[ReadComment],
	[OpTime],
	[OpComment],
	[Mark]
 ");
			strSql.Append(" FROM [msg_BoxMessage] ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
           
            if (orderBy != string.Empty)
                strSql.Append(" order by " + orderBy);
			
            
            var rst = conn.Query<BoxMessage>(strSql.ToString(), null);
                
            
            
			return new List<BoxMessage>(rst);
		}
	
		/// <summary>
		/// 查询所有记录，并排序、分页
		/// </summary>
		public List<BoxMessage> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname)
		{
            int rstcount = 0;
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc, orderfldname,out rstcount);
		}
		
        /// <summary>
		/// 查询所有记录，并排序、分页,没有排序字段
		/// </summary>
		public List<BoxMessage> GetAllPaged(int PageSize,int PageIndex,string strWhere,bool desc)
		{
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc,"MessageBoxId,MessageId,Receiver" );
		}
        
		/// <summary>
		/// 查询所有记录，并排序、分页,没有排序字段
		/// </summary>
		public List<BoxMessage> GetAllPaged(int PageSize,int PageIndex,string strWhere,bool desc,  out int rstcount)
		{
			return GetAllPaged( null, PageSize, PageIndex, strWhere, desc,"MessageBoxId,MessageId,Receiver", out rstcount );
		}
		
		/// <summary>
		/// 查询所有记录，并排序、分页(事务)
		/// </summary>
		public List<BoxMessage> GetAllPaged(IDbConnection conn,int PageSize,int PageIndex,string strWhere,bool desc,string orderfldname, out int rstcount)
		{
		    return Clover.Data.BaseDAO.GetList<BoxMessage>("[msg_BoxMessage]","MessageBoxId,MessageId,Receiver",PageSize, PageIndex, strWhere ,desc, orderfldname, out rstcount);
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
            string strSQL = "select count(*) from [msg_BoxMessage]";
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