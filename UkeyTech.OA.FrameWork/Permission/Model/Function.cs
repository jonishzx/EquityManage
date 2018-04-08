namespace Clover.Permission.Model
{
	using System;
	using System.Data;
    using Clover.Core.Domain;

    /// <summary>
    /// Function 数据模型层
    /// Descn:功能信息表
    /// Creator:laozijian
    /// AddDate:2012-02-16
    /// Creator:laozijian
    /// UpdateTime:2012-02-16    
    /// </summary>
    [Serializable]
    public class Function : IEntity
    {

        #region 私有属性
        /// <summary>
        /// (主键)
        /// </summary>
        private int _functionID;
        /// <summary>
        /// 功能代码
        /// </summary>
        private string _functionCode;
        /// <summary>
        /// 功能名称
        /// </summary>
        private string _functionName;
        /// <summary>
        /// 功能类型（管理功能，报表功能等）,当为特定模块的权限类型时,可选择其他模块，而不准其他模块选择(默认为SepcialModule)
        /// </summary>
        private string _functionTag;
        /// <summary>
        /// 描述
        /// </summary>
        private string _descn;
        /// <summary>
        /// 关联功能ID(关联后访问该权限也有访问其他权限的功能)
        /// </summary>
        private string _relationFunctionID;
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
        private int? _status =1;
        /// <summary>
        /// 
        /// </summary>
        private int? _viewOrd = 1;
        #endregion

        public Function()
        {
        }

        #region 公有属性
        /// <summary>
        /// (主键)
        /// </summary>
        public int FunctionID
        {
            get { return _functionID; }
            set { _functionID = value; }
        }
        /// <summary>
        /// 功能代码
        /// </summary>
        public string FunctionCode
        {
            get { return _functionCode; }
            set { _functionCode = value; }
        }
        /// <summary>
        /// 功能名称
        /// </summary>
        public string FunctionName
        {
            get { return _functionName; }
            set { _functionName = value; }
        }
        /// <summary>
        /// 功能类型（管理功能，报表功能等）,当为特定模块的权限类型时,可选择其他模块，而不准其他模块选择(默认为SepcialModule)
        /// </summary>
        public string FunctionTag
        {
            get { return _functionTag; }
            set { _functionTag = value; }
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
        /// 关联功能ID(关联后访问该权限也有访问其他权限的功能)
        /// </summary>
        public string RelationFunctionID
        {
            get { return _relationFunctionID; }
            set { _relationFunctionID = value; }
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
            sb.Append(_functionID);
            sb.Append(_functionCode);
            sb.Append(_functionName);
            sb.Append(_functionTag);
            sb.Append(_descn);
            sb.Append(_relationFunctionID);
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