namespace Clover.Permission.BLL
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;    
    using System.Transactions;

    using Clover.Core.Collection;
    using Clover.Permission.DAO;
    using Clover.Permission.Model;
    
    /// <summary>
    /// 组-用户业务类
    /// </summary>
    public partial class GroupUserBLL
    {
        private readonly GroupUserDAO dao = new GroupUserDAO();

        public List<string> GetGroupUserIds(int groupId)
        {
            return this.dao.GetGroupUserIds(groupId);
        }

        public List<string> GetGroupUserIds(string groupCode)
        {
            return this.dao.GetGroupUserIds(groupCode);
        }

        public List<User> GetGroupUsers(string groupCode)
        {
            return this.dao.GetGroupUsers(groupCode, false);
        }

        public List<User> GetGroupUsers(string groupCode, bool filterValidUser)
        {
            return this.dao.GetGroupUsers(groupCode, filterValidUser);
        }

        /// <summary>
        /// 删除指定组的用户
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public int DeleteGroupUsers(int groupId, string[] userIds)
        {
            return this.dao.DeleteGroupUsers(groupId, userIds);
        }

        /// <summary>
        /// 删除指定用户的组
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public int DeleteUserGroups(string userId, string[] groupIds)
        {
            return this.dao.DeleteUserGroups(userId, groupIds);
        }


        /// <summary>
        /// 删除指定用户的所有部门
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public int DeleteUserGroups(string userId)
        {
            return this.dao.DeleteUserGroups(userId);
        }

        /// <summary>
        /// 插入指定组的所有用户
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="userIds"></param>
        public void InsertGroupUsers(int groupId, string[] userIds)
        {
            using (var ts = new TransactionScope(System.Transactions.TransactionScopeOption.RequiresNew))
            {
                using (var conn = Clover.Data.BaseDAO.ManualDbService())
                {
                    IDbTransaction tran = null;
                    try
                    {
                        tran = conn.BeginTransaction();

                        foreach (string userid in userIds)
                        {
                            GroupUser m = new GroupUser();
                            m.GroupID = groupId;
                            m.UserID = userid;
                            dao.Insert(tran, m);
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

        /// <summary>
        /// 插入指定用户的指定组
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public void InsertUserGroups(string userId, int[] groupIds)
        {

            using (var ts = new TransactionScope(System.Transactions.TransactionScopeOption.RequiresNew))
            {
                using (var conn = Clover.Data.BaseDAO.ManualDbService())
                {
                    IDbTransaction tran = null;
                    try
                    {
                        tran = conn.BeginTransaction();

                        foreach (int roleId in groupIds)
                        {
                            GroupUser m = new GroupUser();
                            m.GroupID = roleId;
                            m.UserID = userId;
                            dao.Insert(m);
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

    }
}