namespace UkeyTech.WebFW.Model
{
    using System;
    using System.Data;
    using Dapper.Contrib.Extensions;
    using Clover.Core.Domain;

    /// <summary>
    /// Const 数据模型层
    /// Descn:
    /// Creator:laozijian
    /// AddDate:2014-03-19
    /// Creator:laozijian
    /// UpdateTime:2014-03-19    
    /// </summary>
    [Serializable]
    [Table("sys_Const")]
    public class Const : IEntity
    {

        #region 私有属性
        /// <summary>
        /// (主键)
        /// </summary>
        private string _constId;
        /// <summary>
        /// 常量代码
        /// </summary>
        private string _cCode;
        /// <summary>
        /// 检查的值
        /// </summary>
        private string _cValue;
        /// <summary>
        /// 名称
        /// </summary>
        private string _cName;
        /// <summary>
        /// 状态
        /// </summary>
        private string _status;
        /// <summary>
        /// 备注
        /// </summary>
        private string _descn;
        #endregion

        public Const()
        {
        }

        #region 公有属性
        /// <summary>
        /// (主键)
        /// </summary>
        [Key]
        public string ConstId
        {
            get { return _constId; }
            set { _constId = value; }
        }
        /// <summary>
        /// 常量代码
        /// </summary>
        public string cCode
        {
            get { return _cCode; }
            set { _cCode = value; }
        }
        /// <summary>
        /// 检查的值
        /// </summary>
        public string cValue
        {
            get { return _cValue; }
            set { _cValue = value; }
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string cName
        {
            get { return _cName; }
            set { _cName = value; }
        }
        /// <summary>
        /// 状态
        /// </summary>
        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Descn
        {
            get { return _descn; }
            set { _descn = value; }
        }
        #endregion

        #region 方法
        //获取对象hashcode
        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_constId);
            sb.Append(_cCode);
            sb.Append(_cValue);
            sb.Append(_cName);
            sb.Append(_status);
            sb.Append(_descn);

            return sb.ToString().GetHashCode();
        }
        #endregion
    }
}