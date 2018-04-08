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
	/// 权限设置, 加[Serializable]标记为可序列化
	/// </summary>
	[Serializable]
    public class PMConfigInfo : IConfigInfo
    {
        #region 私有字段

        private string tckey = "Key";
        private string tcname = "Name";
        private string tcvalue = "Value";
        #endregion

        #region 属性
        [XmlAttribute("Key")]
        /// <summary>
        /// 键
        /// </summary>
        public string Key
        {
            get { return tckey; }
            set
            {
                tckey = value;
            }
        }

        [XmlAttribute("Name")]
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return tcname; }
            set
            {
                tcname = value;
            }
        }

        [XmlAttribute("Value")]
        /// <summary>
        /// 值
        /// </summary>
        public string Value
        {
            get { return tcvalue; }
            set
            {
                tcvalue = value;
            }
        }

        #endregion
    }
}
