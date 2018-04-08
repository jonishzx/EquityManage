namespace Clover.Permission.BLL
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;    
    using System.Transactions;

    using Clover.Core.Domain;
    using Clover.Config.CPM;
    using Clover.Core.Collection;
    using Clover.Permission.DAO;
    using Clover.Permission.Model;
    
    /// <summary>
    /// 数据权限业务类
    /// </summary>
    public partial class FunctionDataRuleBLL
    {
        private readonly FunctionDataRuleDAO dao = new FunctionDataRuleDAO();

        /// <summary>
        /// 获取所有数据权限
        /// </summary>
        /// <returns></returns>
        public List<FunctionDataRule> GetAllFunctionDataRule()
        {
            return this.dao.GetAll("Priority");
        }

        public FunctionDataRule Get(int id)
        {
            return this.dao.GetModel(id);
        }

        public bool Update(FunctionDataRule model)
        {
            return this.dao.Update(model);
        }

        public bool Delete(int id)
        {
            return this.dao.Delete(id);
        }


        public bool Insert(FunctionDataRule model)
        {
            return this.dao.Insert(model);
        }

        public List<FunctionDataRule> GetModuleFunctionDataRule(int PageSize,int PageIndex, string codeorname, int moduleid)
        {
            return this.dao.GetAllPaged(null,PageSize, PageIndex, string.Format("(Code like '%{0}%' Or Name like '%{0}%') AND ModuleID = '{1}' AND Status = 1", codeorname, moduleid));
        }

        public List<FunctionDataRule> GetAllFunctionDataRule(int PageSize, int PageIndex, string codeorname, out int rstcount)
        {
            return this.dao.GetAllPaged(null, PageSize, PageIndex, string.Format("(Code like '%{0}%' Or Name like '%{0}%')", codeorname), false, out rstcount);
        }

        public List<FunctionDataRule> GetFunctionDataRule(int PageSize, int PageIndex, string codeorname, out int rstcount)
        {
            return this.dao.GetAllPaged(null, PageSize, PageIndex, string.Format("(Code like '%{0}%' Or Name like '%{0}%') AND Status = 1", codeorname), false, out rstcount);
        }

        public List<FunctionDataRule> GetFunctionDataRule(int funcpermissionid)
        {
            return this.dao.GetUserDataRule(funcpermissionid);
        }


        /// <summary>
        /// 获取数据权限控制字符串
        /// </summary>
        /// <param name="datapermissionid"></param>
        /// <returns></returns>
        public string GetFDRuleStr(int dataruleid)
        {
            return this.dao.GetUserDataRuleStr(dataruleid);
        }

        /// <summary>
        /// 获取数据权限控制字符串
        /// </summary>
        /// <param name="datapermissionid"></param>
        /// <returns></returns>
        public string GetUserDataRuleStrings(string dataruleids)
        {
            return this.dao.GetUserDataRuleStrings(dataruleids);
        }

        /// <summary>
        /// 获取数据权限控制后查询结果
        /// </summary>
        /// <param name="datapermissionid"></param>
        /// <returns></returns>
        public List<string> GetSysAdminRuleOutput(Clover.Core.Domain.IAppContext context, int dataruleid)
        {
            return dao.GetUserDataRuleOutput(context, dataruleid, "sys_Admin", "AdminId", "Creator", "AdminId");
        }

        /// 获取数据权限控制字符串
        /// </summary>
        /// <param name="datapermissionid"></param>
        /// <returns></returns>
        public static string GetFDRuleStr(string dataruleCode)
        {
            return FunctionDataRuleDAO.GetUserDataRuleStrByCode(dataruleCode);
        }

     
        public List<FunctionDataRule> GetFunctionDataRule(int PageSize, int PageIndex)
        {
            return this.dao.GetAllPaged(null, PageSize, PageIndex, "");
        }
      
    }
}