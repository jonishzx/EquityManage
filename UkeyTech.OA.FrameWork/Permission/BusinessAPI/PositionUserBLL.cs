using System.Linq;

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
    using StructureMap;
    
    /// <summary>
    /// 岗位-用户业务类
    /// </summary>
    public partial class PositionUserBLL
    {
        private readonly PositionUserDAO dao = new PositionUserDAO();

        public PositionUser GetModel(int GroupID, int PositionID, string UserID)
        {
            return this.dao.GetModel(GroupID, PositionID, UserID);
        }
        public List<string> GetPositionUserIds(int posId)
        {
            return this.dao.GetPositionUserIds(posId);
        }

        public List<string> GetPositionUserIds(string posCode)
        {
            return this.dao.GetPositionUserIds(posCode);
        }

        public List<User> GetPositionUsers(string posCode)
        {
            return this.dao.GetPositionUsers(posCode, false);
        }

        public List<User> GetPositionUsers(string posCode, bool filterValidUser)
        {
            return this.dao.GetPositionUsers(posCode, filterValidUser);
        }

        public List<string> GetGroupPositionUserIds(string posCode, string groupId)
        {
            return this.dao.GetGroupPositionUserIds(posCode, groupId);
        }

        public List<User> GetGroupPositionUsers(string posCode, string groupId)
        {
            return this.dao.GetGroupPositionUsers(posCode, groupId);
        }

        public List<User> GetGroupMultiPositionUsers(String[] posCodes, string groupId, bool filterValidUser)
        {
            String posCodesStr = String.Join("','", posCodes);
            return this.dao.GetGroupMultiPositionUsers(posCodesStr, groupId, filterValidUser);
        }

        public List<User> GetGroupPositionUsers(string posCode, string groupId, bool filterValidUser)
        {
            return this.dao.GetGroupPositionUsers(posCode, groupId, filterValidUser);
        }

        public bool CheckUserHasGroupPosition(string userId, string positionId, string groupId)
        {
            return this.dao.CheckUserHasGroupPosition(null, userId, positionId, groupId);
        }


        public List<User> GetCompGroupPositionUsers(string posCode, string groupId, bool filterValidUser)
        {
            return this.dao.GetCompGroupPositionUsers(posCode, groupId, filterValidUser);
        }

         public List<string> GetCompGroupPositionUserIds(string posCode, string groupCode, bool filterValidUser)
        {
            return this.dao.GetCompGroupPositionUserIds(posCode, groupCode, filterValidUser);
        }

        public List<string> GetCompGroupPositionUserIds(string posCode, string groupCode)
        {
            return this.dao.GetCompGroupPositionUserIds(posCode, groupCode, false);
        }

        public List<string> GetDirectPositionUserIds(string userid, string posCodeOrNameFilter)
        {
            return this.dao.GetDirectPositionUserIds(userid, posCodeOrNameFilter);
        }

        public List<User> GetDirectPositionUsers(string userid, string posCodeOrNameFilter, bool filterValidUser)
        {
            return this.dao.GetDirectPositionUsers(userid, posCodeOrNameFilter, filterValidUser);
        }

        public List<User> GetDirectPositionUsers(string userid, string posCodeOrNameFilter)
        {
            return this.dao.GetDirectPositionUsers(userid, posCodeOrNameFilter, false);
        }

        /// <summary>
        /// 删除指定组的用户
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public int DeletePositionUsers(int PositionId, int? GroupId, string[] userIds)
        {
            return this.dao.DeletePositionUsers(PositionId, GroupId, userIds);
        }

        /// <summary>
        /// 删除指定用户的组
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public int DeleteUserPositions(string userId, string[] PositionIds)
        {
            return this.dao.DeleteUserPositions(userId, PositionIds);
        }


        public void InsertPositionUsers(int positionId, int? groupId, string[] userIds)
        {
            InsertPositionUsers(positionId, groupId, null, userIds);
        }

        /// <summary>
        /// 插入指定组的所有用户
        /// </summary>
        /// <param name="positionId"></param>
        /// <param name="groupId"></param>
        /// <param name="userIds"></param>
        public void InsertPositionUsers(int positionId, int? groupId, int? roleId, string[] userIds)
        {
            var posUserBLL = ObjectFactory.GetInstance<PositionUserBLL>();
            using (var ts = new TransactionScope(System.Transactions.TransactionScopeOption.Required))
            {

                using (var conn = Clover.Data.BaseDAO.ManualDbService())
                {
                    try
                    {
                        foreach (string userid in userIds)
                        {
                            var m = new PositionUser { PositionID = positionId, UserID = userid, RoleID = roleId, GroupID = groupId };

                            if (dao.CheckUserHasGroupPosition(conn, userid, positionId.ToString(), groupId.ToString()))
                            {
                                dao.Update(conn, m);
                            }
                            else {
                                dao.Insert(conn, m);
                            }
                        }

                        ts.Complete();

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// 插入指定用户的指定组
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="positionIds"></param>
        /// <returns></returns>
        public void InsertUserPosition(string userId, int[] positionIds)
        {

            using (var ts = new TransactionScope(System.Transactions.TransactionScopeOption.RequiresNew))
            {
                using (var conn = Clover.Data.BaseDAO.ManualDbService())
                {
                    IDbTransaction tran = null;
                    try
                    {
                        tran = conn.BeginTransaction();

                        foreach (int posid in positionIds)
                        {
                            var m = new PositionUser { PositionID = posid, UserID = userId};

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

        /// <summary>
        /// 获取指定用户的上级领导(所有)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<string> GetPositionMasterUsers(string userId)
        {
            return this.dao.GetPositionMasterUsers(userId, string.Empty);
        }

        /// <summary>
        /// 获取用户的上级岗位的用户(如果没有指定岗位，那么对于身兼多岗的人可能全部发送)
        /// </summary>
        /// <param name="userid">用户的ID</param>
        /// <param name="PositionCode">指定岗位的代码</param>
        /// <returns></returns>
        public List<string> GetPositionMasterUsers(string userId, string positionCode)
        {
            return this.dao.GetPositionMasterUsers(userId, positionCode);
        }
    }
}