using System;
using Clover.Permission.BLL;

namespace Clover.Permission
{
    public class Common {
        /// <summary>
        /// 获取可用用户的过滤方法
        /// </summary>
        /// <param name="filterValidUser">是否过滤可用用户</param>
        /// <param name="sqlcmd">sql脚本</param>
        /// <returns></returns>
        public static string AddUserEnabledFilter(bool filterValidUser, string sqlcmd)
        {
            if (filterValidUser)
            {
                sqlcmd += string.Format(" AND {0}.{1} = '{2}'",
                    Clover.Config.CPM.PermissionConfig.Config.RelativeToUserTable,
                    Clover.Config.CPM.PermissionConfig.Config.RelativeToUserStatus,
                    Clover.Config.CPM.PermissionConfig.Config.RelativeToUserIsEnabled);
            }
            return sqlcmd;
        }

        public static PermissionOwner TranPermissionOwner(string ownerStr)
        {
            switch (ownerStr)
            {
                case "角色":
                case "Role":
                    return PermissionOwner.Role;

                case "工作组":
                case "Group":
                    return PermissionOwner.Group;
            }
            return PermissionOwner.User;
        }

        public static FilterScope TranFilterScope(string scopeStr)
        {

            switch (scopeStr)
            {
                case "System":
                    return FilterScope.System;

                case "Module":
                    return FilterScope.Module;
            }
            return FilterScope.All;
        }

    }
    public class Consts {
        /// <summary>
        /// 新增
        /// </summary>
        public const string Create = "Create";
        /// <summary>
        /// 修改
        /// </summary>
        public const string Edit = "Edit";
        /// <summary>
        /// 删除/废止
        /// </summary>
        public const string Delete = "Delete";
        /// <summary>
        /// 浏览
        /// </summary>
        public const string Browse = "Browse";
        /// <summary>
        /// 复核/审批
        /// </summary>
        public const string Audit = "Audit";
        /// <summary>
        /// 领导复核/审批
        /// </summary>
        public const string LeaderAudit = "LeaderAudit";
        /// <summary>
        /// 批量审批
        /// </summary>
        public const string BatchAudit = "BatchAudit";
        /// <summary>
        /// 反删除，反废止
        /// </summary>
        public const string AntiDelete = "AntiDelete";
        /// <summary>
        /// 反审核，反复核
        /// </summary>
        public const string AntiAudit = "AntiAudit";
        /// <summary>
        /// 导入
        /// </summary>
        public const string Import = "Import";
        /// <summary>
        /// 导出
        /// </summary>
        public const string Export = "Export";

    }
}
