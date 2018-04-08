using System;
using System.Data;
using System.Text;
using Clover.Core.Domain;
using Dapper.Contrib.Extensions;
using Newtonsoft.Json;
using Clover.Core.Collection;
namespace UkeyTech.WebFW.Model
{
    /// <summary>
    /// Dictionary 数据模型层
    /// Descn:
    /// Creator:laozijian
    /// AddDate:2012-07-03
    /// Creator:laozijian
    /// UpdateTime:2012-07-03    
    /// </summary>
    /// 
    [Table("sys_Dictionary")]
    [Serializable]
    public class Dictionary : IEntity, ISNode, IComparable<Dictionary>
    {

        #region 私有属性
        /// <summary>
        /// (主键)
        /// </summary>
        private string _iD;
        /// <summary>
        /// 
        /// </summary>
        private string _tag;
        /// <summary>
        /// 配置代码
        /// </summary>
        private string _code;
        /// <summary>
        /// 配置名称
        /// </summary>
        private string _name;
        /// <summary>
        /// 配置值
        /// </summary>
        private string _value;
        /// <summary>
        /// 父项目
        /// </summary>
        private string _parentId;
        /// <summary>
        /// 创建时间
        /// </summary>
        private DateTime? _updateTime;
        /// <summary>
        /// 
        /// </summary>
        private int? _visible = 1;
        #endregion

        public Dictionary()
        {
        }

        #region 公有属性
        /// <summary>
        /// (主键)
        /// </summary>
        [Key]
        public string DictID
        {
            get { return _iD; }
            set { _iD = value; }
        }
       
        /// <summary>
        /// 
        /// </summary>
        public string Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }
        /// <summary>
        /// 配置代码
        /// </summary>
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }
       
        /// <summary>
        /// 配置值
        /// </summary>
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

    
        /// <summary>
        /// 创建时间
        /// </summary>
        [JsonIgnore]     
        public DateTime? UpdateTime
        {
            get { return _updateTime; }
            set { _updateTime = value; }
        }

        /// <summary>
        /// 扩展属性
        /// </summary>
        [JsonIgnore]
        public string ExtAttr { get; set; }

     
        /// <summary>
        /// 
        /// </summary>
        public int? Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        /// <summary>
        /// 脚本代码
        /// </summary>
        public string SqlCmd { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string ParentPath { get; set; }
        #endregion

        #region 方法
        //获取对象hashcode
        public override int GetHashCode()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_iD);
            sb.Append(_tag);
            sb.Append(_code);
            sb.Append(_name);
            sb.Append(_value);
            sb.Append(_parentId);
            sb.Append(_updateTime);
            sb.Append(_visible);

            return sb.ToString().GetHashCode();
        }
        #endregion

        #region ISNode 成员
        /// <summary>
        /// 项目ID
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]  
        [Write(false)]
        public string Id
        {
            get
            {
                return _iD;
            }
            set
            {
                _iD = value;
            }
        }

        /// <summary>
        /// 项目ID
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// 菜单的父ID
        /// </summary>    
        [Newtonsoft.Json.JsonIgnore]     
        public string ParentId
        {
            get
            {
                return !string.IsNullOrEmpty(_parentId) ? _parentId.ToString() : null;
            }
            set
            {
                _parentId = value;
            }
        }
        int? _viewOrd = 0;
        [Newtonsoft.Json.JsonIgnore]
        /// <summary>
        /// 显示顺序
        /// </summary>
        public int ViewOrder
        {
            get
            {
                if (_viewOrd.HasValue)
                    return _viewOrd.Value;
                else
                    return 0;
            }
            set
            {
                _viewOrd = value;
            }
        }

        #endregion

        #region IComparable<T> 成员

        public int CompareTo(Dictionary other)
        {
            return this.ViewOrder - other.ViewOrder;
        }

        #endregion
    }
}
