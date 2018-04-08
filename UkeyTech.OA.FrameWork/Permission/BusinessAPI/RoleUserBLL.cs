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
    /// 角色-用户业务类
    /// </summary>
    public partial class RoleUserBLL
    {
        private readonly RoleUserDAO dao = new RoleUserDAO();

        public List<string> GetRoleUserIds(int roleId)
        {
            return this.dao.GetRoleUserIds(roleId);
        }

        public List<string> GetRoleUserIds(string roleCode)
        {
            return this.dao.GetRoleUserIds(roleCode);
        }

        public List<User> GetRoleUsers(string roleCode)
        {
            return this.dao.GetRoleUsers(roleCode);
        }

        public List<User> GetRoleUsers(string roleCode, bool validiValidUser)
        {
            return this.dao.GetRoleUsers(roleCode, validiValidUser);
        }

        /// <summary>
        /// 删除指定角色的用户
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public int DeleteRoleUsers(int roleId, string[] userIds)
        {
            return this.dao.DeleteRoleUsers(roleId, userIds);
        }

        /// <summary>
        /// 删除指定用户的角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public int DeleteUserRoles(string userId)
        {
            return this.dao.DeleteUserRoles(userId);
        }

        /// <summary>
        /// 删除指定用户的角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public int DeleteUserRoles(string userId, string[] roleIds)
        {
            return this.dao.DeleteUserRoles(userId, roleIds);
        }

        /// <summary>
        /// 插入指定角色的所有用户
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="userIds"></param>
        public void InsertRoleUsers(int roleId, string[] userIds)
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
                            RoleUser m = new RoleUser();
                            m.RoleID = roleId;
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
        /// 插入指定用户的指定角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public void InsertUserRoles(string userId, int[] roleIds)
        {

            using (var ts = new TransactionScope(System.Transactions.TransactionScopeOption.RequiresNew))
            {
                using (var conn = Clover.Data.BaseDAO.ManualDbService())
                {
                    IDbTransaction tran = null;
                    try
                    {
                        tran = conn.BeginTransaction();

                        foreach (int roleId in roleIds)
                        {
                            RoleUser m = new RoleUser();
                            m.RoleID = roleId;
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