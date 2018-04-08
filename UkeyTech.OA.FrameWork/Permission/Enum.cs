using System;

namespace Clover.Permission.BLL
{
    public enum PermissionOwner
    {
        User,
        Role,
        Group
    }

    /// <summary>
    /// 用以过滤查询权限的范围(针对系统模块)
    /// </summary>
    public enum FilterScope
    {
        All,
        System,
        Module
    }


}
