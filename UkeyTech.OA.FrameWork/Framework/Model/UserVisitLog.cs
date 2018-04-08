namespace UkeyTech.WebFW.Model
{
	using System;
	using System.Data;
    using Clover.Core.Domain;
    
	/// <summary>
	/// UserVisitLog 数据模型层
    /// Descn:
    /// Creator:laozijian
    /// AddDate:2012-02-08
    /// Creator:laozijian
    /// UpdateTime:2012-02-08    
	/// </summary>
	[Serializable]
	public class UserVisitLog : IEntity
	{
       
        #region 私有属性
		/// <summary>
		/// (主键)
		/// </summary>
		private int  _autoId;
		/// <summary>
		/// 
		/// </summary>
		private string  _iP;
		/// <summary>
		/// 
		/// </summary>
		private string  _url;
		/// <summary>
		/// 
		/// </summary>
		private DateTime  _visit;
		#endregion
		
		public UserVisitLog()
		{
		}
				
		#region 公有属性		
		/// <summary>
		/// (主键)
		/// </summary>
		public int AutoId
		{
            get {return  _autoId;}
            set { _autoId = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string IP
		{
            get {return  _iP;}
            set { _iP = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Url
		{
            get {return  _url;}
            set { _url = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime Visit
		{
            get {return  _visit;}
            set { _visit = value;}
		}
		#endregion
        
        #region 方法
        //获取对象hashcode
        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            sb.Append(this.GetType().FullName);
			sb.Append( _autoId);
			sb.Append( _iP);
			sb.Append( _url);
			sb.Append( _visit);

            return sb.ToString().GetHashCode();
        }
		#endregion
	}
}