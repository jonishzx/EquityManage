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
    /// 功能业务类
    /// </summary>
    public partial class FunctionBLL
    {
        private readonly FunctionDAO dao = new FunctionDAO();

        public FunctionBLL() {           
        }

        public int DeleteFunction(int id)
        {
            Function model = dao.GetModel(id);
            if (model != null)
            {
                return dao.Delete(id) ? 1 : 0;
            }
            return 0;
        }

        public int DeleteFunctions(int[] FunctionIds)
        {
            int num = 0;
            if (FunctionIds != null)
            {
                foreach (int num2 in FunctionIds)
                {
                    num += this.DeleteFunction(num2);
                }
            }
            return num;
        }

        public Function Get(int id)
        {
            return this.dao.GetModel(id);
        }

        public List<Function> GetModuleFunctions(int moduleId)
        {
            return this.dao.GetModuleFunctions(moduleId);
        }

        public List<Function> GetModuleFunctions(string moduleCode)
        {
            return this.dao.GetModuleFunctions(moduleCode);
        }

        public Function GetFunctionByCode(string code)
        {
            return this.dao.GetFunctionByCode(code);
        }

        /// <summary>
        /// 获取功能的专属模块
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Module GetFunctionSpModule(int FunctionID)
        {
            return dao.GetFunctionSpModule(FunctionID);
        }

        public List<Function> GetFunctionByNoModuleRef(int moduleId, string where)
        {
            return this.dao.GetFunctionByNoModuleRef(moduleId, where);
        }

        public int InsertFunction(Function model)
        {
            return dao.Insert(model) ? 1 : 0;
        }

        public void InsertFunction(Function model, int moduleId)
        {
            ModuleFunctionDAO mfdao = new ModuleFunctionDAO();
           
            using (var ts = new TransactionScope(System.Transactions.TransactionScopeOption.RequiresNew))
            {
                using (var conn = ModuleDAO.ManualDbService())
                {
                    IDbTransaction tran = null;
                    try
                    {
                        tran = conn.BeginTransaction();

                        this.dao.Insert(tran, model);

                        //获取已经的专用权限设置
                        if (moduleId != 0)
                        {
                            ModuleFunction m = new ModuleFunction();
                            m.FunctionID = model.FunctionID;
                            m.ModuleID = moduleId;
                            mfdao.Insert(tran, m);
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

        public void UpdateFunction(Function model, int moduleId)
        {
            ModuleFunctionDAO mfdao = new ModuleFunctionDAO();
            Module oldmodule = dao.GetFunctionSpModule(model.FunctionID);
            model.UpdateTime = DateTime.Now;
            using (var ts = new TransactionScope(System.Transactions.TransactionScopeOption.RequiresNew))
            {
                using (var conn = Clover.Data.BaseDAO.ManualDbService())
                {
                    IDbTransaction tran = null;
                    try
                    {
                        tran = conn.BeginTransaction();


                        this.dao.Update(tran, model);

                        //获取已经的专用权限设置
                        if (moduleId != 0)
                        {
                            //删除已有关系
                            if (oldmodule != null)
                                mfdao.DeleteModuleFunctions(tran, oldmodule.ModuleID, new string[] { model.FunctionID.ToString() });

                            //重新插入
                            ModuleFunction m = new ModuleFunction();
                            m.FunctionID = model.FunctionID;
                            m.ModuleID = moduleId;
                            mfdao.Insert(tran, m);
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

        public List<Function> SelectFunctionList(string where)
        {
            return this.dao.GetList(null, null, where, "");
        }
      
        public List<Function> SelectFunctionListWithOrder(string where, string order)
        {
            return this.dao.GetList(null, null, where, order);
        }


        /// <summary>
        /// 获取可用的功能
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Function> SelectEnabledFunctionList(string where)
        {
            return SelectEnabledFunctionListWithOrder(where, "ViewOrd");
        }



       /// <summary>
        /// 获取可用的功能
       /// </summary>
       /// <param name="where"></param>
       /// <param name="order"></param>
       /// <returns></returns>
        public List<Function> SelectEnabledFunctionListWithOrder(string where, string order)
        {
            return this.dao.GetList(null, null, !string.IsNullOrEmpty(where) ? "Status > 0 And" + where : " Status > 0", order);
        }

        public void UpdateFunctionWithOutSPModel(Function model)
        {
            ModuleFunctionDAO mfdao = new ModuleFunctionDAO();
            Module oldmodule = dao.GetFunctionSpModule(model.FunctionID);
            model.UpdateTime = DateTime.Now;
            using (var ts = new TransactionScope(System.Transactions.TransactionScopeOption.RequiresNew))
            {
                using (var conn = Clover.Data.BaseDAO.ManualDbService())
                {
                    IDbTransaction tran = null;
                    try
                    {
                        tran = conn.BeginTransaction();

                        this.dao.Update(tran, model);

                        //获取已经的专用权限设置
                        if (oldmodule != null)
                        {
                            //删除已有关系
                            mfdao.DeleteModuleFunctions(tran, oldmodule.ModuleID, new string[] { model.FunctionID.ToString() });
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

        public List<Function> SelectFunctionList(int page, int pageindex, string codeorname, out int rscount)
        {
            string where = string.Empty;

            if (!string.IsNullOrEmpty(codeorname))
            {
                where += string.Format("  (FunctionName LIKE '%{0}%' OR FunctionCode LIKE '%{0}%') ", codeorname);
            }

            return this.dao.GetAllPaged(null, page, pageindex, where, true, "ViewOrd", out rscount);
        }


        /// <summary>
        /// 检查是否存在相同的ID
        /// </summary>
        public bool CheckExistsSameID(string functioncode, string id)
        {
            return RoleDAO.ExistsSameAttr("CPM_Function", "FunctionCode", functioncode, "Status>=0", "FunctionID", id.ToString());
        }
    }
}