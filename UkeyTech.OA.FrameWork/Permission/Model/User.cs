using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clover.Permission.Model
{
    /// <summary>
    /// 单个用户模型
    /// </summary>
    public class User
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public int GroupID { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        ///  岗位ID
        /// </summary>
        public int PositionID { get; set; }

        /// <summary>
        /// 岗位名称
        /// </summary>
        public string PositionName { get; set; }

    }
}
