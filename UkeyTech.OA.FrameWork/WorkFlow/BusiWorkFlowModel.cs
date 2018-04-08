using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FireWorkflow.Net.CustomExtension;
using StructureMap;
using Clover.Web.Core;
using FireWorkflow.Net.CustomExtension.Model;
using Clover.Core.Domain;

namespace UkeyTech.WebFW.WorkFlow.Model
{
    /// <summary>
    /// 业务工作模型
    /// </summary>
    public class BusiWorkFlowModel : BaseWorkFlowModel
    {
        /// <summary>
        /// 当前操作人
        /// </summary>
        public override IAccount Operator{
            get {
                IWebContext _context = ObjectFactory.GetInstance<IWebContext>();
                return _context.CurrentUser;
            }
            set {
                //throw new NotImplementedException("非实现内容");
            }
        }
        #region 业务属性？
        /// <summary>
        /// 是否为展期第一次进入
        /// </summary>
        public bool IsFirst { get; set; }

        /// <summary>
        /// 是否关闭重财单
        /// </summary>
        public bool AllowClose { get; set; }
        #endregion
    }
}