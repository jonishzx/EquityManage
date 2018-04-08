using System;
using System.Data;
using Clover.Core.Domain;

namespace UkeyTech.WebFW.Model
{
  
    /// Widget 数据模型层
    /// Descn:
    /// Creator:laozijian
    /// AddDate:2012-02-28
    /// Creator:laozijian
    /// UpdateTime:2012-02-28    
    /// </summary>
    [Serializable]
    public class Widget : IEntity
    {

        #region 私有属性
        /// <summary>
        /// (主键)
        /// </summary>
        private int _widgetID;
        /// <summary>
        /// 部件代码
        /// </summary>
        private string _widgetCode;
        /// <summary>
        /// 部件名称
        /// </summary>
        private string _widgetName;
        /// <summary>
        /// 部件标记
        /// </summary>
        private string _widgetTag;
        /// <summary>
        /// 描述
        /// </summary>
        private string _descn;
        /// <summary>
        /// 部件目标（如URL,winform对象）
        /// </summary>
        private string _target;
        /// <summary>
        /// 注入的参数
        /// </summary>
        private string _parameters;
        /// <summary>
        /// UI参数
        /// </summary>
        private string _uIParamters;
        /// <summary>
        /// 创建时间
        /// </summary>
        private DateTime? _createTime = DateTime.Now;
        /// <summary>
        /// 修改时间
        /// </summary>
        private DateTime? _updateTime = DateTime.Now;
        /// <summary>
        /// 修改人

        /// </summary>
        private string _creator;
        /// <summary>
        /// 修改人
        /// </summary>
        private string _modifitor;
        /// <summary>
        /// 状态(0:无效,1:有效)
        /// </summary>
        private int? _status;
        /// <summary>
        /// 
        /// </summary>
        private int? _viewOrd;
        #endregion

        public Widget()
        {
        }

        #region 公有属性
        /// <summary>
        /// (主键)
        /// </summary>
        public int WidgetID
        {
            get { return _widgetID; }
            set { _widgetID = value; }
        }
        /// <summary>
        /// 部件代码
        /// </summary>
        public string WidgetCode
        {
            get { return _widgetCode; }
            set { _widgetCode = value; }
        }
        /// <summary>
        /// 部件名称
        /// </summary>
        public string WidgetName
        {
            get { return _widgetName; }
            set { _widgetName = value; }
        }
        /// <summary>
        /// 部件标记
        /// </summary>
        public string WidgetTag
        {
            get { return _widgetTag; }
            set { _widgetTag = value; }
        }
        /// <summary>
        /// 描述
        /// </summary>
        public string Descn
        {
            get { return _descn; }
            set { _descn = value; }
        }
        /// <summary>
        /// 部件目标（如URL,winform对象）
        /// </summary>
        public string Target
        {
            get { return _target; }
            set { _target = value; }
        }
        /// <summary>
        /// 注入的参数
        /// </summary>
        public string Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }
        /// <summary>
        /// UI参数
        /// </summary>
        public string UIParamters
        {
            get { return _uIParamters; }
            set { _uIParamters = value; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateTime
        {
            get { return _updateTime; }
            set { _updateTime = value; }
        }
        /// <summary>
        /// 修改人

        /// </summary>
        public string Creator
        {
            get { return _creator; }
            set { _creator = value; }
        }
        /// <summary>
        /// 修改人
        /// </summary>
        public string Modifitor
        {
            get { return _modifitor; }
            set { _modifitor = value; }
        }
        /// <summary>
        /// 状态(0:无效,1:有效)
        /// </summary>
        public int? Status
        {
            get { return _status; }
            set { _status = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ViewOrd
        {
            get { return _viewOrd; }
            set { _viewOrd = value; }
        }
        #endregion

        #region 方法
        //获取对象hashcode
        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_widgetID);
            sb.Append(_widgetCode);
            sb.Append(_widgetName);
            sb.Append(_widgetTag);
            sb.Append(_descn);
            sb.Append(_target);
            sb.Append(_parameters);
            sb.Append(_uIParamters);
            sb.Append(_createTime);
            sb.Append(_updateTime);
            sb.Append(_creator);
            sb.Append(_modifitor);
            sb.Append(_status);
            sb.Append(_viewOrd);

            return sb.ToString().GetHashCode();
        }
        #endregion
    }
}
