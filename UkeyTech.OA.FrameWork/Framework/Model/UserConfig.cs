using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UkeyTech.WebFW.Model
{
    using System;
    using System.Data;
    using Clover.Core.Domain;

    /// <summary>
    /// UserConfig 数据模型层
    /// Descn:
    /// Creator:laozijian
    /// AddDate:2012-02-24
    /// Creator:laozijian
    /// UpdateTime:2012-02-24    
    /// </summary>
    [Serializable]
    public class UserConfig : IEntity
    {

        #region 私有属性
        /// <summary>
        /// (主键)
        /// </summary>
        private int _id;
        /// <summary>
        /// 用户ID
        /// </summary>
        private string _userId;
        /// <summary>
        /// 配置类型(Memo,Portal)
        /// </summary>
        private string _configType;
        /// <summary>
        /// 标题
        /// </summary>
        private string _title;
        /// <summary>
        /// 配置值
        /// </summary>
        private string _configValue;
        /// <summary>
        /// 
        /// </summary>
        private DateTime _updateTime;
        #endregion

        public UserConfig()
        {
        }

        #region 公有属性
        /// <summary>
        /// (主键)
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }
        /// <summary>
        /// 配置类型(Memo,Portal)
        /// </summary>
        public string ConfigType
        {
            get { return _configType; }
            set { _configType = value; }
        }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        /// <summary>
        /// 配置值
        /// </summary>
        public string ConfigValue
        {
            get { return _configValue; }
            set { _configValue = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime UpdateTime
        {
            get { return _updateTime; }
            set { _updateTime = value; }
        }
        #endregion

        #region 方法
        //获取对象hashcode
        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_id);
            sb.Append(_userId);
            sb.Append(_configType);
            sb.Append(_title);
            sb.Append(_configValue);
            sb.Append(_updateTime);

            return sb.ToString().GetHashCode();
        }
        #endregion
    }
}
