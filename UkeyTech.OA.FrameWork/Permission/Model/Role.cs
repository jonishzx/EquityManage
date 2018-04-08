namespace Clover.Permission.Model
{
	using System;
	using System.Data;
    using System.Xml;

    using Clover.Core.Domain;
    using Clover.Core.Collection;

	/// <summary>
	/// Role 数据模型层
    /// Descn:角色信息表，由于角色的权限可以继承于父级，请注意
    /// Creator:laozijian
    /// AddDate:2012-02-08
    /// Creator:laozijian
    /// UpdateTime:2012-02-08    
	/// </summary>
	[Serializable]
    public class Role : IEntity, ISNode, IRole, IComparable<Role>
	{
       
        #region 私有属性
		/// <summary>
		/// (主键)
		/// </summary>
		private int  _roleID;
		/// <summary>
		/// 角色代码
		/// </summary>
		private string  _roleCode;
		/// <summary>
		/// 角色名称
		/// </summary>
		private string  _roleName;
		/// <summary>
		/// 角色类型(1.系统角色,系统管理员才能修改 2.普通角色，任何人可以修改)
		/// </summary>
		private string  _roleTag;
		/// <summary>
		/// 描述
		/// </summary>
		private string  _descn;
		/// <summary>
		/// 父角色ID
		/// </summary>
		private int?  _parentID = null;
		/// <summary>
		/// 层级父路径ID
		/// </summary>
		private string  _parentPath;
		/// <summary>
		/// 显示顺序
   
		/// </summary>
		private int?  _viewOrd =1;
		/// <summary>
		/// 创建时间
		/// </summary>
		private DateTime?  _createTime = DateTime.Now;
		/// <summary>
		/// 修改时间
		/// </summary>
		private DateTime?  _updateTime = DateTime.Now;
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
		
		public Role()
		{
		}
				
		#region 公有属性		
		/// <summary>
		/// (主键)
		/// </summary>
		public int RoleID
		{
            get {return  _roleID;}
            set { _roleID = value;}
		}
		/// <summary>
		/// 角色代码
		/// </summary>
		public string RoleCode
		{
            get {return  _roleCode;}
            set { _roleCode = value;}
		}
		/// <summary>
		/// 角色名称
		/// </summary>
		public string RoleName
		{
            get {return  _roleName;}
            set { _roleName = value;}
		}
		/// <summary>
		/// 角色类型(1.系统角色,系统管理员才能修改 2.普通角色，任何人可以修改)
		/// </summary>
		public string RoleTag
		{
            get {return  _roleTag;}
            set { _roleTag = value;}
		}
		/// <summary>
		/// 描述
		/// </summary>
		public string Descn
		{
            get {return  _descn;}
            set { _descn = value;}
		}
		/// <summary>
		/// 父角色ID
		/// </summary>
		public int? ParentID
		{
            get {return  _parentID;}
            set { _parentID = value;}
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
		/// 显示顺序
   
		/// </summary>
		public int? ViewOrd
		{
            get {return  _viewOrd;}
            set { _viewOrd = value;}
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
			sb.Append( _roleID);
			sb.Append( _roleCode);
			sb.Append( _roleName);
			sb.Append( _roleTag);
			sb.Append( _descn);
			sb.Append( _parentID);
			sb.Append( _parentPath);
			sb.Append( _viewOrd);
			sb.Append( _createTime);
			sb.Append( _updateTime);
			sb.Append( _creator);
			sb.Append( _modifitor);
			sb.Append( _status);

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
                return _roleID.ToString();
            }
            set
            {
                _roleID = int.Parse(value);
            }
        }
     
        /// <summary>
        /// 项目ID
        /// </summary>
        public string Name
        {
            get
            {
                return _roleName;
            }
            set
            {
                _roleName = value;
            }
        }

        /// <summary>
        /// 菜单的父ID
        /// </summary>    
        public string ParentId
        {
            get
            {
                return _parentID.HasValue ? _parentID.ToString() : null;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _parentID = int.Parse(value);
                else
                    _parentID = null;
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

        public int CompareTo(Role other)
        {
            return this.ViewOrder - other.ViewOrder;
        }

        #endregion

     
    }
}