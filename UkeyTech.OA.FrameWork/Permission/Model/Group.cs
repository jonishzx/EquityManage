namespace Clover.Permission.Model
{
	using System;
	using System.Data;
    using Clover.Core.Domain;
    using Clover.Core.Collection;
    using Dapper.Contrib.Extensions;

	/// <summary>
	/// Group 数据模型层
    /// Descn:工作组：同一个组内的用户拥有所有角色的权限的并集 1.用户可归属于工作组 2.工作组的功能来源于角色
    /// Creator:laozijian
    /// AddDate:2012-02-11
    /// Creator:laozijian
    /// UpdateTime:2012-02-11    
	/// </summary>
	[Serializable]
	public class Group : IEntity, ISNode, IComparable<Group>
	{
       
        #region 私有属性
		/// <summary>
		/// (主键)
		/// </summary>
		private int  _groupID;
		/// <summary>
		/// 组代码
		/// </summary>
		private string  _groupCode;
		/// <summary>
		/// 组名称
		/// </summary>
		private string  _groupName;
		/// <summary>
		/// 注释
		/// </summary>
		private string  _descn;
		/// <summary>
		/// 扩展
		/// </summary>
		private string  _attribute;
		/// <summary>
		/// 父组ID
		/// </summary>
		public int?  _parentId = null;
		/// <summary>
		/// 层级父路径ID
		/// </summary>
		private string  _parentPath;
		/// <summary>
		/// 显示顺序
   
		/// </summary>
		private int?  _viewOrd = 1;
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
		
		public Group()
		{
		}
				
		#region 公有属性		
		/// <summary>
		/// (主键)
		/// </summary>
		public int GroupID
		{
            get {return  _groupID;}
            set { _groupID = value;}
		}
		/// <summary>
		/// 组代码
		/// </summary>
		public string GroupCode
		{
            get {return  _groupCode;}
            set { _groupCode = value;}
		}
		/// <summary>
		/// 组名称
		/// </summary>
		public string GroupName
		{
            get {return  _groupName;}
            set { _groupName = value;}
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
		/// 扩展
		/// </summary>
		public string Attribute
		{
            get {return  _attribute;}
            set { _attribute = value;}
		}
		/// <summary>
		/// 父组ID
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
			sb.Append( _groupID);
			sb.Append( _groupCode);
		
            return sb.ToString().GetHashCode();
        }
		#endregion

        #region ISNode 成员
        [Newtonsoft.Json.JsonIgnore]
        /// <summary>
        /// 项目ID
        /// </summary>
        public string Id
        {
            get
            {
                return _groupID.ToString();
            }
            set
            {
                _groupID = int.Parse(value);
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        /// <summary>
        /// 项目ID
        /// </summary>
        public string Name
        {
            get
            {
                return _groupName;
            }
            set
            {
                _groupName = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Write(false)]
        [Newtonsoft.Json.JsonIgnore]
        public string EmpName
        {
            get
            {
                try
                {
                    string sql = " select top 1 name from V_PayStrust where code='" + _modifitor + "'";
                    DataTable dt = Clover.Data.BaseDAO.GetDataTable(sql);
                    if (dt != null && dt.Rows.Count > 0)
                        return dt.Rows[0][0].ToString();
                    else
                        return _modifitor;
                }
                catch {
                    //Clover.Core.Logging.LogCentral.Current.Error("获取V_PayStrust失败:" + ex.Message );
                    return string.Empty;
                }
            }
        }

        [Newtonsoft.Json.JsonIgnore]
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

        /// <summary>
        /// 全名
        /// </summary>
        public string FullName { get; set; }
        #endregion

        #region IComparable<T> 成员

        public int CompareTo(Group other)
        {
            return this.ViewOrder - other.ViewOrder;
        }

        #endregion
	}
}