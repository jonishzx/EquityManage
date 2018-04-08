using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Clover.Core.Configuration;

namespace Clover.Config.CPM
{
	
	/// <summary>
    /// 权限配置信息
	/// </summary>
    public class PermissionConfigInfo : IConfigInfo
	{

        private List<PMConfigInfo> paramslist = new List<PMConfigInfo>();
        /// <summary>
        /// 运行日程
        /// </summary>
        [XmlArray("PMConfigInfos")]
        public PMConfigInfo[] ConfigInfos
        {
            get
            {
                return paramslist.ToArray();
            }
            set
            {
                if (value == null)
                    return;

                paramslist.AddRange(value);
            }
        }

        public void AddFieldItem(PMConfigInfo con)
        {
            paramslist.Add(con);
        }

        /// <summary>
        /// 更具url地址以及开始日期获取任务
        /// </summary>
        /// <param name="url"></param>
        /// <param name="startdate"></param>
        /// <returns></returns>
        public PMConfigInfo getFieldItem(string id)
        {

            foreach (PMConfigInfo param in paramslist)
            {
                if (param.Key == id)
                {
                    return param;
                }
            }
            return null;
        }

        /// <summary>
        /// 校验数据
        /// </summary>
        public bool Exist(string id)
        {
            foreach (PMConfigInfo param in paramslist)
            {
                if (param.Key == id)
                {
                    return true;
                }
            }
            return false;
        }

        [XmlIgnore]
        /// <summary>
        /// 通用权限模块系统代码
        /// </summary>
        public string CPMSystemCode
        {
            get
            {
                if (Exist("CPMSYSTEMCODE"))
                {
                    return getFieldItem("CPMSYSTEMCODE").Value;
                }
                else
                    return null;
            }
            set {
                if (Exist("CPMSYSTEMCODE"))
                {
                    PMConfigInfo newit = new PMConfigInfo();
                    newit.Key = "CPMSYSTEMCODE";
                    newit.Value = value.ToString();
                    newit.Name = "通用权限模块系统代码";
                    AddFieldItem(newit);
                }
                else {
                    getFieldItem("CPMSYSTEMCODE").Value = value;
                }
            }
        }

        [XmlIgnore]
        /// <summary>
        /// 是否启用拒绝权限
        /// </summary>
        public bool EnableDenyPermission
        {
            get
            {
                if (Exist("EnableDenyPermission"))
                {
                    return bool.Parse(getFieldItem("EnableDenyPermission").Value);
                }
                else
                    return true;
            }
            set
            {
                if (Exist("EnableDenyPermission"))
                {
                    PMConfigInfo newit = new PMConfigInfo();
                    newit.Key = "EnableDenyPermission";
                    newit.Value = value.ToString();
                    newit.Name = "是否启用拒绝权限";
                    AddFieldItem(newit);
                }
                else
                {
                    getFieldItem("EnableDenyPermission").Value = value.ToString();
                }
            }
        }

        [XmlIgnore]
        /// <summary>
        /// 是否启用组权限
        /// </summary>
        public bool EnableGroupPermission
        {
            get
            {
                if (Exist("EnableDenyPermission"))
                {
                    return bool.Parse(getFieldItem("EnableGroupPermission").Value);
                }
                else
                    return true;
            }
            set
            {
                if (Exist("EnableGroupPermission"))
                {
                    PMConfigInfo newit = new PMConfigInfo();
                    newit.Key = "EnableGroupPermission";
                    newit.Value = value.ToString();
                    newit.Name = "是否启用组权限";
                    AddFieldItem(newit);
                }
                else
                {
                    getFieldItem("EnableGroupPermission").Value = value.ToString();
                }
            }
        }

        [XmlIgnore]
        /// <summary>
        /// 是否启用岗位权限
        /// </summary>
        public bool EnablePositionPermission
        {
            get
            {
                if (Exist("EnableDenyPermission"))
                {
                    return bool.Parse(getFieldItem("EnablePositionPermission").Value);
                }
                else
                    return true;
            }
            set
            {
                if (Exist("EnablePositionPermission"))
                {
                    PMConfigInfo newit = new PMConfigInfo();
                    newit.Key = "EnablePositionPermission";
                    newit.Value = value.ToString();
                    newit.Name = "是否启用岗位权限";
                    AddFieldItem(newit);
                }
                else
                {
                    getFieldItem("EnablePositionPermission").Value = value.ToString();
                }
            }
        }

        [XmlIgnore]
        /// <summary>
        /// 是否启用权限继承(对岗位及组织由上至下的架构不适用)
        /// </summary>
        public bool EnableInheritPermission
        {
            get
            {
                if (Exist("EnableInheritPermission"))
                {
                    return bool.Parse(getFieldItem("EnableInheritPermission").Value);
                }
                else
                    return true;
            }
            set
            {
                if (Exist("EnableInheritPermission"))
                {
                    PMConfigInfo newit = new PMConfigInfo();
                    newit.Key = "EnableInheritPermission";
                    newit.Value = value.ToString();
                    newit.Name = "是否启用权限继承(对岗位及组织由上至下的架构不适用)";
                    AddFieldItem(newit);
                }
                else
                {
                    getFieldItem("EnableInheritPermission").Value = value.ToString();
                }
            }
        }

        [XmlIgnore]
        /// <summary>
        /// 是否启用用户权限
        /// </summary>
        public bool EnableUserPermission
        {
            get
            {
                if (Exist("EnableUserPermission"))
                {
                    return bool.Parse(getFieldItem("EnableUserPermission").Value);
                }
                else
                    return true;
            }
            set
            {
                if (Exist("EnableUserPermission"))
                {
                    PMConfigInfo newit = new PMConfigInfo();
                    newit.Key = "EnableUserPermission";
                    newit.Value = value.ToString();
                    newit.Name = "是否启用用户权限";
                    AddFieldItem(newit);
                }
                else
                {
                    getFieldItem("EnableUserPermission").Value = value.ToString();
                }
            }
        }

        [XmlIgnore]
        /// <summary>
        /// 是否允许切换组织岗位
        /// </summary>
        public bool EnableChangePosition
        {
            get
            {
                if (Exist("EnableChangePosition"))
                {
                    return bool.Parse(getFieldItem("EnableChangePosition").Value);
                }
                else
                    return true;
            }
            set
            {
                if (Exist("EnableChangePosition"))
                {
                    PMConfigInfo newit = new PMConfigInfo();
                    newit.Key = "EnableChangePosition";
                    newit.Value = value.ToString();
                    newit.Name = "是否允许切换组织岗位";
                    AddFieldItem(newit);
                }
                else
                {
                    getFieldItem("EnableChangePosition").Value = value.ToString();
                }
            }
        }

        [XmlIgnore]
        /// <summary>
        /// 关联业务系统的用户信息表
        /// </summary>
        public string RelativeToUserTable
        {
            get
            {
                if (Exist("RelativeToUserTable"))
                {
                    return getFieldItem("RelativeToUserTable").Value;
                }
                else
                    return string.Empty;
            }
            set
            {
                if (Exist("RelativeToUserTable"))
                {
                    PMConfigInfo newit = new PMConfigInfo();
                    newit.Key = "RelativeToUserTable";
                    newit.Value = value;
                    newit.Name = "关联业务系统的用户信息表";
                    AddFieldItem(newit);
                }
                else
                {
                    getFieldItem("RelativeToUserTable").Value = value;
                }
            }
        }


        [XmlIgnore]
        /// <summary>
        /// 关联业务系统的用户状态字段
        /// </summary>
        public string RelativeToUserStatus
        {
            get
            {
                if (Exist("RelativeToUserStatus"))
                {
                    return getFieldItem("RelativeToUserStatus").Value;
                }
                else
                    return "Status";
            }
            set
            {
                if (Exist("RelativeToUserStatus"))
                {
                    PMConfigInfo newit = new PMConfigInfo();
                    newit.Key = "RelativeToUserStatus";
                    newit.Value = value;
                    newit.Name = "关联业务系统的用户信息表";
                    AddFieldItem(newit);
                }
                else
                {
                    getFieldItem("RelativeToUserStatus").Value = value;
                }
            }
        }

        [XmlIgnore]
        /// <summary>
        /// 关联业务系统的用户状态为可用的状态字段值
        /// </summary>
        public string RelativeToUserIsEnabled
        {
            get
            {
                if (Exist("RelativeToUserIsEnabled"))
                {
                    return getFieldItem("RelativeToUserIsEnabled").Value;
                }
                else
                    return "1";
            }
            set
            {
                if (Exist("RelativeToUserIsEnabled"))
                {
                    PMConfigInfo newit = new PMConfigInfo();
                    newit.Key = "RelativeToUserIsEnabled";
                    newit.Value = value;
                    newit.Name = "用户状态为可用的状态字段值";
                    AddFieldItem(newit);
                }
                else
                {
                    getFieldItem("RelativeToUserIsEnabled").Value = value;
                }
            }
        }

        [XmlIgnore]
        /// <summary>
        /// 是否允许切换组织岗位
        /// </summary>
        public string RelativeToUserKey
        {
            get
            {
                if (Exist("RelativeToUserKey"))
                {
                    return getFieldItem("RelativeToUserKey").Value;
                }
                else
                    return string.Empty;
            }
            set
            {
                if (Exist("RelativeToUserKey"))
                {
                    PMConfigInfo newit = new PMConfigInfo();
                    newit.Key = "RelativeToUserKey";
                    newit.Value = value;
                    newit.Name = "关联业务系统的用户信息表主键";
                    AddFieldItem(newit);
                }
                else
                {
                    getFieldItem("RelativeToUserKey").Value = value;
                }
            }
        }

        [XmlIgnore]
        /// <summary>
        /// 关联业务系统的用户信息表的用户名称
        /// </summary>
        public string RelativeToUserName
        {
            get
            {
                if (Exist("RelativeToUserName"))
                {
                    return  getFieldItem("RelativeToUserName").Value;
                }
                else
                    return string.Empty;
            }
            set
            {
                if (Exist("RelativeToUserName"))
                {
                    PMConfigInfo newit = new PMConfigInfo();
                    newit.Key = "RelativeToUserName";
                    newit.Value = value;
                    newit.Name = "关联业务系统的用户信息表的用户名称";
                    AddFieldItem(newit);
                }
                else
                {
                    getFieldItem("RelativeToUserName").Value = value;
                }
            }
        }

        [XmlIgnore]
        /// <summary>
        /// 岗位的选择是否基于用户定义的角色岗位
        /// </summary>
        public bool PositionBaseOnUserGroup
        {
            get
            {
                if (Exist("PositionBaseOnUserGroup"))
                {
                    return bool.Parse(getFieldItem("PositionBaseOnUserGroup").Value);
                }
                else
                    return true;
            }
            set
            {
                if (Exist("PositionBaseOnUserGroup"))
                {
                    PMConfigInfo newit = new PMConfigInfo();
                    newit.Key = "PositionBaseOnUserGroup";
                    newit.Value = value.ToString();
                    newit.Name = "岗位的选择是否基于用户定义的角色岗位";
                    AddFieldItem(newit);
                }
                else
                {
                    getFieldItem("PositionBaseOnUserGroup").Value = value.ToString();
                }
            }
        }
    }
	
}
