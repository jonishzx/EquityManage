using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Clover.Permission.BLL;
using Clover.Permission.Model;

namespace Clover.Permission
{
    /// <summary>
    /// 帮助类
    /// </summary>
    public class Util
    {
        public static string GetOwnerFieldName(PermissionOwner owner)
        {
            string str = string.Empty;
            switch (owner)
            {
                case PermissionOwner.User:
                    return "UserID";

                case PermissionOwner.Role:
                    return "RoleID";

                case PermissionOwner.Group:
                    return "GroupID";
            }
            return str;
        }

        public static string GetScopeFieldName(FilterScope scope)
        {
            string str = string.Empty;
            switch (scope)
            {
                case FilterScope.System:
                    return "SystemID";

                case FilterScope.Module:
                    return "ModuleID";
            }
            return str;
        }

        /// <summary>
        /// 合并相同的模块及功能
        /// </summary>
        /// <param name="dsSource"></param>
        /// <param name="scopeField"></param>
        /// <param name="typeField"></param>
        /// <param name="mergeField"></param>
        /// <returns></returns>
        public static List<UserFuncPMResult> CombinePrivilege(List<UserFuncPMResult> source)
        {
            if (source!= null && source.Count > 0)
            {
                List<UserFuncPMResult> set = new List<UserFuncPMResult>();
                foreach (var m in source)
                {
                 
                    UserFuncPMResult oldModel = set.Find(x => {
                        return x.FunctionID == m.FunctionID && x.ModuleID == m.ModuleID;
                    });

                    if(oldModel!=null) //已经存在数据
                    {
                        oldModel.IsAllow = oldModel.IsAllow || m.IsAllow; //合并规则：集合中任意一个为允许
                        oldModel.IsDeny = oldModel.IsDeny || (m.IsDeny && m.IsSelf == 1); //合并规则：集合中任意一个为拒绝&& 集合中得元素为目标ID
                        oldModel.IsSelf = (oldModel.IsSelf == 1 || ((m.IsSelf == 1) && m.IsAllow)) ? 1 : 0; //合并规则：集合中任意一个为允许 && 集合中得元素为自己                 
                        if (oldModel.IsSelf == 1) {
                            oldModel.FuncPermissionID = m.FuncPermissionID;
                        }
                    }
                    else{
                       UserFuncPMResult newmodel = m.Clone() as UserFuncPMResult;
                       newmodel.IsDeny = m.IsDeny && m.IsSelf == 1;
                       newmodel.IsSelf = (m.IsAllow && m.IsSelf == 1) ? 1: 0;
                       newmodel.FuncPermissionID = m.FuncPermissionID;
                       newmodel.DataPermissionId = m.DataPermissionId;
                       set.Add(newmodel);
                    }
                }
                return set;
            }
            return new List<UserFuncPMResult>();
        }
    }
}
