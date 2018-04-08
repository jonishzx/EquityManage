namespace UkeyTech.WebFW.Model
{
	using System;
	using System.Data;
    using Clover.Core.Domain;
    
	/// <summary>
	/// OnlineList 数据模型层
    /// Descn:
    /// Creator:laozijian
    /// AddDate:2012-02-08
    /// Creator:laozijian
    /// UpdateTime:2012-02-08    
	/// </summary>
	[Serializable]
	public class OnlineList : IEntity
	{
       
        #region 私有属性
		/// <summary>
		/// 自动id
		/// </summary>
		private int  _autoId;
		/// <summary>
		/// (主键)
		/// </summary>
		private DateTime  _visitDate;
		/// <summary>
		/// 访问ip列表
		/// </summary>
		private string  _iPs;
		/// <summary>
		/// 访问人数
		/// </summary>
		private int  _visitCount;
		#endregion
		
		public OnlineList()
		{
		}
				
		#region 公有属性		
		/// <summary>
		/// 自动id
		/// </summary>
		public int AutoId
		{
            get {return  _autoId;}
            set { _autoId = value;}
		}
		/// <summary>
		/// (主键)
		/// </summary>
		public DateTime VisitDate
		{
            get {return  _visitDate;}
            set { _visitDate = value;}
		}
		/// <summary>
		/// 访问ip列表
		/// </summary>
		public string IPs
		{
            get {return  _iPs;}
            set { _iPs = value;}
		}
		/// <summary>
		/// 访问人数
		/// </summary>
		public int VisitCount
		{
            get {return  _visitCount;}
            set { _visitCount = value;}
		}
		#endregion
        
        #region 方法
        //获取对象hashcode
        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            sb.Append(this.GetType().FullName);
			sb.Append( _autoId);
			sb.Append( _visitDate);
			sb.Append( _iPs);
			sb.Append( _visitCount);

            return sb.ToString().GetHashCode();
        }
		#endregion
	}
}