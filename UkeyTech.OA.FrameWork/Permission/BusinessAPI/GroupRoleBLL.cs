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
    /// 组-角色业务类
    /// </summary>
    public partial class GroupRoleBLL
    {
        private readonly GroupRoleDAO dao = new GroupRoleDAO();

        /// <summary>
        /// 删除指定组的角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public int DeleteGroupRoles(int groupid, string[] roleids)
        {
            return this.dao.DeleteGroupRoles(groupid, roleids);
        }

        /// <summary>
        /// 删除指定角色的组
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public int DeleteRoleGroups(int roleid, string[] groupids)
        {
            return this.dao.DeleteRoleGroups(roleid, groupids);
        }

        /// <summary>
        /// 插入指定组的所有角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="groupIds"></param>
        public void InsertGroupRoles(int roleId, int[] groupIds)
        {
            using (var ts = new TransactionScope(System.Transactions.TransactionScopeOption.RequiresNew))
            {
                using (var conn = Clover.Data.BaseDAO.ManualDbService())
                {
                    IDbTransaction tran = null;
                    try
                    {
                        tran = conn.BeginTransaction();

                        foreach (int groupId in groupIds)
                        {
                            GroupRole m = new GroupRole();
                            m.RoleID = roleId;
                            m.GroupID = groupId;
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
        /// 插入指定角色的指定组
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public void InsertUserRoles(int groupId, int[] roleIds)
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
                            GroupRole m = new GroupRole();
                            m.RoleID = roleId;
                            m.GroupID = groupId;
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