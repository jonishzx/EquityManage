using System;
using System.Text;
using System.Web;
using System.IO;

using Clover.Core.Configuration;


namespace Clover.Config.CPM
{

    /// <summary>
    /// 权限信息管理类
    /// </summary>
    public sealed class PermissionManager : DefaultConfigFileManager<PermissionConfigInfo>
    {

           /// <summary>
        /// 默认配置文件所在路径
        /// </summary>
        const string CONST_DEFAULTPATH = "~/config/permission.config";

         /// <summary>
        /// 初始化文件修改时间和对象实例
        /// </summary>
        public PermissionManager()
            : this(CONST_DEFAULTPATH)
        {            
        }

        /// <summary>
        /// 初始化文件修改时间和对象实例
        /// </summary>
        public PermissionManager(string configfilepath)
            : base(configfilepath)
        {
        
        }        
    }
}
