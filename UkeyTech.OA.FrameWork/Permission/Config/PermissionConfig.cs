using System;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;

using Clover.Core.Configuration;
using Clover.Config.Sys;

namespace Clover.Config.CPM
{
    /// <summary>
    /// 权限配置信息
    /// </summary>
    public class PermissionConfig : ConfigCenter<PermissionManager, PermissionConfigInfo>
    {
        /// <summary>
        /// 返回配置信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static PMConfigInfo GetConnecting(string key)
        {
            return Config.getFieldItem(key);
        }


    }
}
