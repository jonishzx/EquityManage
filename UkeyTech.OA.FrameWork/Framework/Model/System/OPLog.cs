namespace UkeyTech.WebFW.Model
{
	using System;
	using System.Data;
    using Clover.Core.Domain;
    
	/// <summary>
	/// OPLog 数据模型层
    /// Descn:
    /// Creator:laozijian
    /// AddDate:2012-02-08
    /// Creator:laozijian
    /// UpdateTime:2012-02-08    
	/// </summary>
	[Serializable]
	public class OPLog : IEntity
	{
       
        #region 私有属性
		/// <summary>
		/// (主键)
		/// </summary>
		private int  _id;
        /// <summary>
        /// 
        /// </summary>
        private string _userId;
		/// <summary>
		/// 
		/// </summary>
		private string  _loginName;
		/// <summary>
		/// 
		/// </summary>
		private string  _userName;
		/// <summary>
		/// 
		/// </summary>
		private string  _userIP;
		/// <summary>
		/// 
		/// </summary>
		private string  _logMessage;
		/// <summary>
		/// 
		/// </summary>
		private DateTime?  _updateTime;
		/// <summary>
		/// 
		/// </summary>
		private string  _logOPName;
		#endregion
		
		public OPLog()
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
        public string UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }
		/// <summary>
		/// 
		/// </summary>
		public string LoginName
		{
            get {return  _loginName;}
            set { _loginName = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UserName
		{
            get {return  _userName;}
            set { _userName = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UserIP
		{
            get {return  _userIP;}
            set { _userIP = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string LogMessage
		{
            get {return  _logMessage;}
            set { _logMessage = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? UpdateTime
		{
            get {return  _updateTime;}
            set { _updateTime = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string LogOPName
		{
            get {return  _logOPName;}
            set { _logOPName = value;}
		}
		#endregion
        
        #region 方法
        //获取对象hashcode
        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            sb.Append(this.GetType().FullName);
			sb.Append( _id);
            sb.Append(_userId);
			sb.Append( _loginName);
			sb.Append( _userName);
			sb.Append( _userIP);
			sb.Append( _logMessage);
			sb.Append( _updateTime);
			sb.Append( _logOPName);

            return sb.ToString().GetHashCode();
        }
		#endregion
	}
}