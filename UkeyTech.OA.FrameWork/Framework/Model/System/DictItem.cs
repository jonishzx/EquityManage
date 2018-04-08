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
    [Table("sys_DictItems")]
    [Serializable]
    public class DictItem : IEntity, IComparable<DictItem>
    {

        #region 私有属性
        /// <summary>
        /// (主键)
        /// </summary>
        private string _dictID;
   
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
        /// 创建时间
        /// </summary>
        private DateTime? _updateTime;
     
        #endregion

        public DictItem()
        {
        }

        #region 公有属性
        /// <summary>
        /// (主键)
        /// </summary>

        public string DictID
        {
            get { return _dictID; }
            set { _dictID = value; }
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

        string _status = "1";

        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get{return _status;} set{_status = value;} }

        /// <summary>
        /// 项目名称
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
        /// 创建时间
        /// </summary>
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
       /// 备注
       /// </summary>
        public string Descn { get; set; }
     
     
        private int _viewOrd = 0;
        /// <summary>
        /// 显示顺序
        /// </summary>
        public int ViewOrder
        {
            get { return _viewOrd; }
            set { _viewOrd = value; }
        }
        #endregion

        #region 方法
        //获取对象hashcode
        public override int GetHashCode()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_dictID);
            sb.Append(_code);
       
            return sb.ToString().GetHashCode();
        }
        #endregion

   
        #region IComparable<T> 成员

        public int CompareTo(DictItem other)
        {
            return this.ViewOrder - other.ViewOrder;
        }

        #endregion
    }
}
