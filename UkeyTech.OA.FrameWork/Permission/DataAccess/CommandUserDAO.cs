using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using Dapper;

namespace Clover.Permission.DAO
{
    /// <summary>
    /// 处理第三方用户表的功能
    /// </summary>
    public class CommandUserDAO : Clover.Data.BaseDAO
    {
        /// <summary>
        /// 删除指定用户的角色权限相关信息
        /// </summary>
        /// <param name="tran"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int DeleteUserPermission(IDbTransaction tran, string userId)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("DELETE FROM CPM_FuncPermission WHERE UserID = @UserID ");
            builder.Append("DELETE FROM CPM_Group_User WHERE UserID = @UserID ");
            builder.Append("DELETE FROM CPM_Role_User WHERE UserID = @UserID ");
     
            DynamicParameters p = new DynamicParameters();
            var conn = DbService();

            p.Add("UserID", userId, DbType.String, null, null);

            int num = conn.Execute(builder.ToString(), p, tran, null, CommandType.Text);
            
            return num;
        }
       
    }
}
