namespace Clover.Permission.Model
{
	using System;
	using System.Data;
    using Clover.Core.Domain;
    
	/// <summary>
	/// FuncPermission 数据模型层
    /// Descn:功能权限信息表，模块与功能是必须，具有以下三种搭配方式：
    ///1.角色->功能
    ///2.用户->功能(继承于组)
    ///3.用户组->功能(继承于角色)
    /// Creator:laozijian
    /// AddDate:2012-02-08
    /// Creator:laozijian
    /// UpdateTime:2012-02-08    
	/// </summary>
	[Serializable]
	public class FuncPermission : IEntity
	{
       
         #region 私有属性
		/// <summary>
		/// (主键)
		/// </summary>
		private int  _funcPermissionID;
		/// <summary>
		/// 用户ID
		/// </summary>
		private string  _userID;
		/// <summary>
		/// 角色ID
		/// </summary>
		private int?  _roleID;
		/// <summary>
		/// 模块ID
		/// </summary>
		private int  _moduleID;
		/// <summary>
		/// 功能ID
		/// </summary>
		private int  _functionID;
		/// <summary>
		/// 允许权限
		/// </summary>
		private bool?  _isAllow;
		/// <summary>
		/// 拒绝权限
		/// </summary>
		private bool?  _isDeny;
		/// <summary>
		/// 最后修改时间
		/// </summary>
		private DateTime?  _createDate;
		/// <summary>
		/// 
		/// </summary>
		private int?  _groupID;
		#endregion

        public FuncPermission()
		{
		}
				
		#region 公有属性		
		/// <summary>
		/// (主键)
		/// </summary>
		public int FuncPermissionID
		{
            get {return  _funcPermissionID;}
            set { _funcPermissionID = value;}
		}
		/// <summary>
		/// 用户ID
		/// </summary>
		public string UserID
		{
            get {return  _userID;}
            set { _userID = value;}
		}
		/// <summary>
		/// 角色ID
		/// </summary>
		public int? RoleID
		{
            get {return  _roleID;}
            set { _roleID = value;}
		}
		/// <summary>
		/// 模块ID
		/// </summary>
		public int ModuleID
		{
            get {return  _moduleID;}
            set { _moduleID = value;}
		}
		/// <summary>
		/// 功能ID
		/// </summary>
		public int FunctionID
		{
            get {return  _functionID;}
            set { _functionID = value;}
		}
		/// <summary>
		/// 允许权限
		/// </summary>
		public bool? IsAllow
		{
            get {return  _isAllow;}
            set { _isAllow = value;}
		}
		/// <summary>
		/// 拒绝权限
		/// </summary>
		public bool? IsDeny
		{
            get {return  _isDeny;}
            set { _isDeny = value;}
		}
		/// <summary>
		/// 最后修改时间
		/// </summary>
		public DateTime? CreateDate
		{
            get {return  _createDate;}
            set { _createDate = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? GroupID
		{
            get {return  _groupID;}
            set { _groupID = value;}
		}

        /// <summary>
        /// 数据权限ID
        /// </summary>
        public int? DataPermissionId
        {
            get;
            set;
        }
		#endregion
        
        #region 方法
        //获取对象hashcode
        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            sb.Append(this.GetType().FullName);
			sb.Append( _funcPermissionID);
			sb.Append( _userID);
			sb.Append( _roleID);
			sb.Append( _moduleID);
			sb.Append( _functionID);
			sb.Append( _isAllow);
			sb.Append( _isDeny);
			sb.Append( _createDate);
			sb.Append( _groupID);

            return sb.ToString().GetHashCode();
        }
		#endregion
	}
}