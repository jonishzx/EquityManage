namespace Clover.Permission.Model
{
    using System;
    using System.Data;
    using System.Collections.Generic;
    using System.Linq;
    using Dapper.Contrib.Extensions;
    using Clover.Core.Domain;

    /// <summary>
    /// FunctionDataRule 数据模型层
    /// Descn:
    /// Creator:laozijian
    /// AddDate:2013-07-21
    /// Creator:laozijian
    /// UpdateTime:2013-07-21    
    /// </summary>
    [Serializable]
    [Table("CPM_FunctionDataRule")]
    public class FunctionDataRule : IEntity
    {

        #region 私有属性
        /// <summary>
        /// (主键)
        /// </summary>
        private int _dataPermissionId;
        /// <summary>
        /// 如果不为空,代表专属于一个模块
        /// </summary>
        private int? _moduleID;
        /// <summary>
        /// 数据权限代码
        /// </summary>
        private string _code;
        /// <summary>
        /// 验证通过后的动作
        /// </summary>
        private string _allowAction;
        /// <summary>
        /// 验证失败后的动作
        /// </summary>
        private string _denyAction;
        /// <summary>
        /// 
        /// </summary>
        private string _name;
        /// <summary>
        /// 优先级
        /// </summary>
        private int? _priority;
        /// <summary>
        /// 权限控制
        /// </summary>
        private string _dataRule;
        /// <summary>
        /// 
        /// </summary>
        private string _descn;
        /// <summary>
        /// 状态(1:可用 0:不可用)
        /// </summary>
        private int? _status;
        #endregion

        public FunctionDataRule()
        {
        }

        #region 公有属性
        /// <summary>
        /// (主键)
        /// </summary>
        [Key]
        public int DataPermissionId
        {
            get { return _dataPermissionId; }
            set { _dataPermissionId = value; }
        }
        /// <summary>
        /// 如果不为空,代表专属于一个模块
        /// </summary>
        public int? ModuleID
        {
            get { return _moduleID; }
            set { _moduleID = value; }
        }
        /// <summary>
        /// 数据权限代码
        /// </summary>
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }
        /// <summary>
        /// 验证通过后的动作
        /// </summary>
        public string AllowAction
        {
            get { return _allowAction; }
            set { _allowAction = value; }
        }
        /// <summary>
        /// 验证失败后的动作
        /// </summary>
        public string DenyAction
        {
            get { return _denyAction; }
            set { _denyAction = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// 优先级
        /// </summary>
        public int? Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }
        /// <summary>
        /// 权限控制
        /// </summary>
        public string DataRule
        {
            get { return _dataRule; }
            set { _dataRule = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Descn
        {
            get { return _descn; }
            set { _descn = value; }
        }
        /// <summary>
        /// 状态(1:可用 0:不可用)
        /// </summary>
        public int? Status
        {
            get { return _status; }
            set { _status = value; }
        }

        /// <summary>
        /// 是否选中
        /// </summary>
        public int? IsSelected { get; set; }
        #endregion

        #region 方法
        //获取对象hashcode
        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_dataPermissionId);
            sb.Append(_moduleID);
         

            return sb.ToString().GetHashCode();
        }
        #endregion
    }
}