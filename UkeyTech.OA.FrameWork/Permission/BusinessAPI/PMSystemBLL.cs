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
    /// 权限系统功能业务类
    /// </summary>
    public partial class PMSystemBLL
    {
        private readonly PMSystemDAO dao = new PMSystemDAO();
        private readonly ModuleDAO moduledao = new ModuleDAO();

        public PMSystemBLL()
        {
           
        }

        public int DeletePMSystem(int id)
        {
            PMSystem model = dao.GetModel(id);
            if (model != null)
            {
                dao.Delete(id);
            }
            return 0;
        }

        public int DeletePMSystems(int[] PMSystemIdArray)
        {
            int num = 0;
            if (PMSystemIdArray != null)
            {
                foreach (int num2 in PMSystemIdArray)
                {
                    num += this.DeletePMSystem(num2);
                }
            }
            return num;
        }

        public PMSystem Get(int id)
        {
            return this.dao.GetModel(id);
        }

        public PMSystem GetPMSystemByCode(string code)
        {
            return this.dao.GetPMSystemByCode(code);
        }

        public void InsertPMSystem(PMSystem model)
        {
            this.dao.Insert(model);
        }

        public List<PMSystem> SelectPMSystemList(string where)
        {
            return this.dao.GetList(null, null, where, "");
        }

        public List<PMSystem> SelectPMSystemListWithOrder(string where, string order)
        {
            return this.dao.GetList(null, null, where, order);
        }

        public List<PMSystem> SelectPMSystemList(int page, int pageindex, string codeorname, out int rscount)
        {
            string where = string.Empty;
          
            if (!string.IsNullOrEmpty(codeorname))
            {
                where += string.Format("  (SystemName LIKE '%{0}%' OR SystemCode LIKE '%{0}%') ", codeorname);
            }         

            return this.dao.GetAllPaged(null, page, pageindex, where, true, "ViewOrd", out rscount);
        }


        /// <summary>
        /// 更新系统的信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdatePMSystem(PMSystem model)
        {          
            int num = 0;
            if (model != null)
            {
                using (var ts = new TransactionScope(System.Transactions.TransactionScopeOption.RequiresNew))
                {


                    using (var conn = Clover.Data.BaseDAO.ManualDbService())
                    {

                        IDbTransaction tran = null;
                        try
                        {
                            tran = conn.BeginTransaction();

                            num += dao.Update(tran, model) ? 1 : 0;
                            moduledao.UpdateStatus(tran, model.SystemID, model.Status.Value);

                            tran.Commit();
                            ts.Complete();
                            return num;
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
            return num;
        }

        /// <summary>
        /// 检查是否存在相同的ID
        /// </summary>
        public bool CheckExistsSameID(string systemcode, string id)
        {
            return RoleDAO.ExistsSameAttr("CPM_PMSystem", "SystemCode", systemcode, "Status>=0", "SystemID", id.ToString());
        }
    }
}