namespace Clover.Permission.Model
{
	using System;
	using System.Data;
    using Clover.Core.Domain;
    using Clover.Core.Collection;
    using Dapper;
    using Dapper.Contrib;
	/// <summary>
	/// Module 数据模型层
    /// Descn:功能模块信息表
    /// Creator:laozijian
    /// AddDate:2012-02-09
    /// Creator:laozijian
    /// UpdateTime:2012-02-09    
	/// </summary>
	[Serializable]
    public class Module : IEntity, ISNode, IComparable<Module>
	{
       
        #region 私有属性
		/// <summary>
		/// (主键)
		/// </summary>
		private int  _moduleID;
		/// <summary>
		/// 模块代码
		/// </summary>
		private string  _moduleCode;
		/// <summary>
		/// 模块名称
		/// </summary>
		private string  _moduleName;
		/// <summary>
		/// 模块应用规则
		/// </summary>
		private string  _moduleTag;
		/// <summary>
		/// 系统ID
		/// </summary>
		private int?  _systemID;
		/// <summary>
		/// 注释
		/// </summary>
		private string  _descn;
		/// <summary>
		/// 该模块导向的目标
		/// </summary>
		private string  _target;
		/// <summary>
		/// 目标框架
		/// </summary>
		private string  _targetFrame;
		/// <summary>
		/// 
		/// </summary>
		private string  _imageUrl;
		/// <summary>
		/// 扩展属性
		/// </summary>
		private string  _attribute;
		/// <summary>
		/// 
		/// </summary>
		public int?  _parentId = -1;
		/// <summary>
		/// 层级父路径ID
		/// </summary>
		private string  _parentPath;
		/// <summary>
		/// 菜单路径
		/// </summary>
		private string  _parentTitle;
		/// <summary>
		/// 显示顺序
   
		/// </summary>
		private int?  _viewOrd = 1;
		/// <summary>
		/// 是否可见(1:可见 0: 隐藏)
		/// </summary>
		private int?  _visible =1 ;
		/// <summary>
		/// 创建时间
		/// </summary>
		private DateTime?  _createTime = DateTime.Now;
		/// <summary>
		/// 修改时间
		/// </summary>
        private DateTime? _updateTime = DateTime.Now;
		/// <summary>
		/// 修改人
   
		/// </summary>
		private string  _creator;
		/// <summary>
		/// 修改人
		/// </summary>
		private string  _modifitor;
		/// <summary>
		/// 状态(0:无效,1:有效)
		/// </summary>
		private int?  _status = 1;
		#endregion
		
		public Module()
		{
		}
				
		#region 公有属性		
		/// <summary>
		/// (主键)
		/// </summary>
		public int ModuleID
		{
            get {return  _moduleID;}
            set { _moduleID = value;}
		}
		/// <summary>
		/// 模块代码
		/// </summary>
		public string ModuleCode
		{
            get {return  _moduleCode;}
            set { _moduleCode = value;}
		}
		/// <summary>
		/// 模块名称
		/// </summary>
		public string ModuleName
		{
            get {return  _moduleName;}
            set { _moduleName = value;}
		}
		/// <summary>
		/// 模块应用规则
		/// </summary>
		public string ModuleTag
		{
            get {return  _moduleTag;}
            set { _moduleTag = value;}
		}
		/// <summary>
		/// 系统ID
		/// </summary>
		public int? SystemID
		{
            get {return  _systemID;}
            set { _systemID = value;}
		}
		/// <summary>
		/// 注释
		/// </summary>
		public string Descn
		{
            get {return  _descn;}
            set { _descn = value;}
		}
		/// <summary>
		/// 该模块导向的目标
		/// </summary>
		public string Target
		{
            get {return  _target;}
            set { _target = value;}
		}
		/// <summary>
		/// 目标框架
		/// </summary>
		public string TargetFrame
		{
            get {return  _targetFrame;}
            set { _targetFrame = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ImageUrl
		{
            get {return  _imageUrl;}
            set { _imageUrl = value;}
		}
		/// <summary>
		/// 扩展属性
		/// </summary>
		public string Attribute
		{
            get {return  _attribute;}
            set { _attribute = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ParentID
		{
            get {return  _parentId;}
            set { _parentId = value;}
		}
		/// <summary>
		/// 层级父路径ID
		/// </summary>
		public string ParentPath
		{
            get {return  _parentPath;}
            set { _parentPath = value;}
		}
		/// <summary>
		/// 菜单路径
		/// </summary>
		public string ParentTitle
		{
            get {return  _parentTitle;}
            set { _parentTitle = value;}
		}
		/// <summary>
		/// 显示顺序
   
		/// </summary>
		public int? ViewOrd
		{
            get {return  _viewOrd;}
            set { _viewOrd = value;}
		}
		/// <summary>
		/// 是否可见(1:可见 0: 隐藏)
		/// </summary>
		public int? Visible
		{
            get {return  _visible;}
            set { _visible = value;}
		}
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime? CreateTime
		{
            get {return  _createTime;}
            set { _createTime = value;}
		}
		/// <summary>
		/// 修改时间
		/// </summary>
		public DateTime? UpdateTime
		{
            get {return  _updateTime;}
            set { _updateTime = value;}
		}
		/// <summary>
		/// 修改人
   
		/// </summary>
		public string Creator
		{
            get {return  _creator;}
            set { _creator = value;}
		}
		/// <summary>
		/// 修改人
		/// </summary>
		public string Modifitor
		{
            get {return  _modifitor;}
            set { _modifitor = value;}
		}
		/// <summary>
		/// 状态(0:无效,1:有效)
		/// </summary>
		public int? Status
		{
            get {return  _status;}
            set { _status = value;}
		}
        
		#endregion
        
        #region 方法
        //获取对象hashcode
        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            sb.Append(this.GetType().FullName);
			sb.Append( _moduleID);

            return sb.ToString().GetHashCode();
        }
		#endregion

        #region ISNode 成员
        /// <summary>
        /// 项目ID
        /// </summary>
        public string Id
        {
            get
            {
                return _moduleID.ToString();
            }
            set
            {
                _moduleID = int.Parse(value);
            }
        }

        /// <summary>
        /// 项目ID
        /// </summary>
        public string Name
        {
            get
            {
                return _moduleName;
            }
            set
            {
                _moduleName = value;
            }
        }

        /// <summary>
        /// 菜单的父ID
        /// </summary>    
        public string ParentId
        {
            get
            {
                return _parentId.HasValue ? _parentId.ToString() : null;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _parentId = int.Parse(value);
                else
                    _parentId = null;
            }
        }

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

        public int CompareTo(Module other)
        {
            return this.ViewOrder - other.ViewOrder;
        }

        #endregion
	}
}