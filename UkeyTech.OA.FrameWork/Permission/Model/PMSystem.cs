namespace Clover.Permission.Model
{
	using System;
	using System.Data;
    using Clover.Core.Domain;
    
	/// <summary>
	/// MainSystem 数据模型层
    /// Descn:为日后权限统一管理而设置的系统信息表
    /// Creator:laozijian
    /// AddDate:2012-02-09
    /// Creator:laozijian
    /// UpdateTime:2012-02-09    
	/// </summary>
	[Serializable]
	public class PMSystem : IEntity
	{
       
        #region 私有属性
		/// <summary>
		/// (主键)
		/// </summary>
		private int  _systemID;
		/// <summary>
		/// 系统代码
		/// </summary>
		private string  _systemCode;
		/// <summary>
		/// 系统名称
		/// </summary>
		private string  _systemName;
		/// <summary>
		/// 注释
		/// </summary>
		private string  _descn;
		/// <summary>
		/// 显示顺序
   
		/// </summary>
		private int?  _viewOrd = 1;
		/// <summary>
		/// 是否可见(1:可见 0: 隐藏)
		/// </summary>
		private int?  _visible = 1;
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
		
		public PMSystem()
		{
		}
				
		#region 公有属性		
		/// <summary>
		/// (主键)
		/// </summary>
		public int SystemID
		{
            get {return  _systemID;}
            set { _systemID = value;}
		}
		/// <summary>
		/// 系统代码
		/// </summary>
		public string SystemCode
		{
            get {return  _systemCode;}
            set { _systemCode = value;}
		}
		/// <summary>
		/// 系统名称
		/// </summary>
		public string SystemName
		{
            get {return  _systemName;}
            set { _systemName = value;}
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
			sb.Append( _systemID);
			sb.Append( _systemCode);
			sb.Append( _systemName);
			sb.Append( _descn);
			sb.Append( _viewOrd);
			sb.Append( _visible);
			sb.Append( _createTime);
			sb.Append( _updateTime);
			sb.Append( _creator);
			sb.Append( _modifitor);
			sb.Append( _status);

            return sb.ToString().GetHashCode();
        }
		#endregion
	}
}