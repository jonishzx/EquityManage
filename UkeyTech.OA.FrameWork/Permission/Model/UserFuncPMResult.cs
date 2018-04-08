using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clover.Permission.Model
{
    /// <summary>
    /// 查询出的用户功能权限
    /// </summary>
    [Serializable]
    public class UserFuncPMResult : ICloneable
    {
        public int ModuleID;
        public string ModuleName;
        public string ModuleTag;
        public int FunctionID;
        public string FunctionCode;
        public string FunctionName;
        public bool IsAllow = false;
        public bool IsDeny = false;
        public int IsSelf = 1;
        public string ModuleCode;
        public string PermissionTitle;
        /// <summary>
        /// 数据权限ID
        /// </summary>
        public int? DataPermissionId;

        /// <summary>
        /// 数据权限ID
        /// </summary>
        public int? DataPermissionPriority;

        /// <summary>
        /// 权限ID
        /// </summary>
        public int? FuncPermissionID;

        #region ICloneable 成员

        public object Clone()
        {
            UserFuncPMResult newmodel = new UserFuncPMResult();
            newmodel.ModuleID = this.ModuleID;
            newmodel.ModuleName = this.ModuleName;
            newmodel.ModuleTag = this.ModuleTag;
            newmodel.FunctionID = this.FunctionID;
            newmodel.FunctionCode = this.FunctionCode;
            newmodel.FunctionName = this.FunctionName;
            newmodel.IsAllow = this.IsAllow;
            newmodel.IsDeny = this.IsDeny;
            newmodel.IsSelf = this.IsSelf;
            newmodel.ModuleCode = this.ModuleCode;
            newmodel.DataPermissionId = this.DataPermissionId;
            newmodel.DataPermissionPriority = this.DataPermissionPriority;
            return newmodel;
        }

        public override string ToString()
        {
            return string.Format("{0}({1})-IsDeny:{2},IsAllow:{3},IsSelf{4}",
                this.ModuleCode, this.ModuleID, this.IsDeny, this.IsAllow, this.IsSelf);
        }
        #endregion
    }
}
