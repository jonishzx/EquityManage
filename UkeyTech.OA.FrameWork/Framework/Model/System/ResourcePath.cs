namespace UkeyTech.WebFW.Model
{
	using System;
	using System.Data;
    using Clover.Core.Domain;
    
	/// <summary>
	/// ResourcePath 数据模型层
    /// Descn:
    /// Creator:laozijian
    /// AddDate:2012-02-08
    /// Creator:laozijian
    /// UpdateTime:2012-02-08    
	/// </summary>
	[Serializable]
	public class ResourcePath : IEntity
	{
       
        #region 私有属性
		/// <summary>
		/// (主键)
		/// </summary>
		private int  _id;
		/// <summary>
		/// 
		/// </summary>
		private string  _name;
		/// <summary>
		/// 
		/// </summary>
		private string  _path;
		/// <summary>
		/// 
		/// </summary>
		private string  _downloadUrl;
		/// <summary>
		/// 
		/// </summary>
		private int?  _viewOrder;
		/// <summary>
		/// 
		/// </summary>
		private string  _remark;
		/// <summary>
		/// 
		/// </summary>
		private int?  _status;
		/// <summary>
		/// 所属类型
		/// </summary>
		private string  _pTypeId;
		#endregion
		
		public ResourcePath()
		{
		}
				
		#region 公有属性		
		/// <summary>
		/// (主键)
		/// </summary>
		public int id
		{
            get {return  _id;}
            set { _id = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Name
		{
            get {return  _name;}
            set { _name = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Path
		{
            get {return  _path;}
            set { _path = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string DownloadUrl
		{
            get {return  _downloadUrl;}
            set { _downloadUrl = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ViewOrder
		{
            get {return  _viewOrder;}
            set { _viewOrder = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Remark
		{
            get {return  _remark;}
            set { _remark = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Status
		{
            get {return  _status;}
            set { _status = value;}
		}
		/// <summary>
		/// 所属类型
		/// </summary>
		public string PTypeId
		{
            get {return  _pTypeId;}
            set { _pTypeId = value;}
		}
		#endregion
        
        #region 方法
        //获取对象hashcode
        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            sb.Append(this.GetType().FullName);
			sb.Append( _id);
			sb.Append( _name);
			sb.Append( _path);
			sb.Append( _downloadUrl);
			sb.Append( _viewOrder);
			sb.Append( _remark);
			sb.Append( _status);
			sb.Append( _pTypeId);

            return sb.ToString().GetHashCode();
        }
		#endregion
	}
}