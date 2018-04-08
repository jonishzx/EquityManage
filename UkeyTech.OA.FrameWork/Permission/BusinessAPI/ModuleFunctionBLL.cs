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
    public partial class ModuleFunctionBLL
    {
        private readonly ModuleFunctionDAO dao = new ModuleFunctionDAO();

        /// <summary>
        /// 删除指定模块的功能资源
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public int DeleteModuleFunctions(int moduleId, string[] functionIds)
        {
            return this.dao.DeleteModuleFunctions(moduleId, functionIds);
        }
      
        /// <summary>
        /// 插入指定模块的功能资源
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="userIds"></param>
        public void InsertModuleFunctions(int moduleId, string[] functionIds)
        {
            using (var ts = new TransactionScope(System.Transactions.TransactionScopeOption.RequiresNew))
            {
                using (var conn = Clover.Data.BaseDAO.ManualDbService())
                {
                    IDbTransaction tran = null;
                    try
                    {
                        tran = conn.BeginTransaction();

                        foreach (string funcId in functionIds)
                        {
                            ModuleFunction m = new ModuleFunction();
                            m.ModuleID = moduleId;
                            m.FunctionID = int.Parse(funcId);
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
    }
}