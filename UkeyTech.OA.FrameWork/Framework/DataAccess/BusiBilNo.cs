using System.Collections.Generic;
using System.Data;
using System.Linq;
using Clover.Data;
using Dapper;

namespace UkeyTech.WebFW.DAO
{
    /// <summary>
    ///     User 数据访问层
    /// </summary>
    public partial class BillNoDAO : BaseDAO
    {
        #region 业务方法


        /// <summary>
        ///     获取请假业务的新单号
        /// </summary>
        /// <param name="billno"></param>
        /// <returns></returns>
        public string ShowNewQingJiaBillNo()
        {
            return ShowNewBillNo("QJ", "{document}{datetime}{Number}", "yyyyMM", "HR_EmpLeave", "BillNo");
        }

        /// <summary>
        ///     获取差旅费报销的新单号
        /// </summary>
        /// <param name="billno"></param>
        /// <returns></returns>
        public string ShowNewTravelChargeBillNo()
        {
            return ShowNewBillNo("CLBX", "{document}{datetime}{Number}", "yyyyMM", "Biz_TravelRbsm", "BillNo");
        }

        /// <summary>
        ///     获取交际费报销的新单号
        /// </summary>
        /// <param name="billno"></param>
        /// <returns></returns>
        public string ShowSOPaymentBillNo()
        {
            return ShowNewBillNo("SOP", "{document}{datetime}{Number}", "yyyyMM", "Biz_BuildSOPayment", "BillNo");
        }


        /// <summary>
        ///     获取出差申请业务的新单号
        /// </summary>
        /// <param name="billno"></param>
        /// <returns></returns>
        public string ShowNewTravelAppBillNo()
        {
            return ShowNewBillNo("CJ", "{document}{datetime}{Number}", "yyyyMM", "HR_EmpEvection", "BillNo");
        }

        /// <summary>
        ///     获取办报销单的新单号
        /// </summary>
        /// <param name="billno"></param>
        /// <returns></returns>
        public string ShowNewRbsmBillNo()
        {
            return ShowNewBillNo("BX", "{document}{datetime}{Number}", "yyyyMM", "Biz_FincRbsm", "BillNo");
        }

        /// <summary>
        ///     获取办公室采购业务的新单号
        /// </summary>
        /// <param name="billno"></param>
        /// <returns></returns>
        public string ShowNewOfficePurchaseBillNo()
        {
            return ShowNewBillNo("OP", "{document}{datetime}{Number}", "yyyyMM", "Biz_OfficePurMaster", "BillNo");
        }

        /// <summary>
        ///     获取短期投资业务的新单号
        /// </summary>
        /// <param name="billno"></param>
        /// <returns></returns>
        public string ShowNewSTCBillNo(string busitype)
        {
           return ShowNewBillNo(busitype, "{document}{datetime}{Number}", "yyyy", "Biz_FincLoanBussiness",
                "BillNo");
        }

        /// <summary>
        ///  工资录入信息编码
        /// </summary>
        /// <param name="billno"></param>
        /// <returns></returns>
        public string ShowNewWageVoucherBillNo()
        {
            return ShowNewBillNo("WV", "{document}{datetime}{Number}", "yyyyMM", "Wage_Voucher",
                 "VoucherNo");
        }
        #endregion
    }
}