namespace Clover.Permission.Model
{
	using System;
	using System.Data;
    using Clover.Core.Domain;
    
	/// <summary>
	/// Log 数据模型层
    /// Descn:日志记录表
    /// Creator:laozijian
    /// AddDate:2012-02-08
    /// Creator:laozijian
    /// UpdateTime:2012-02-08    
	/// </summary>
	[Serializable]
	public class Log : IEntity
	{
       
        #region 私有属性
		/// <summary>
		/// (主键)
		/// </summary>
		private int  _logID;
		/// <summary>
		/// 登陆账号
		/// </summary>
		private string  _loginName;
		/// <summary>
		/// 用户名
		/// </summary>
		private string  _userName;
		/// <summary>
		/// 日志类型的标记
   
		/// </summary>
		private string  _logTag;
		/// <summary>
		/// 用户IP
		/// </summary>
		private string  _userIP;
		/// <summary>
		/// 消息
		/// </summary>
		private string  _message;
		/// <summary>
		/// 创建时间
		/// </summary>
		private DateTime?  _createTime;
		#endregion
		
		public Log()
		{
		}
				
		#region 公有属性		
		/// <summary>
		/// (主键)
		/// </summary>
		public int LogID
		{
            get {return  _logID;}
            set { _logID = value;}
		}
		/// <summary>
		/// 登陆账号
		/// </summary>
		public string LoginName
		{
            get {return  _loginName;}
            set { _loginName = value;}
		}
		/// <summary>
		/// 用户名
		/// </summary>
		public string UserName
		{
            get {return  _userName;}
            set { _userName = value;}
		}
		/// <summary>
		/// 日志类型的标记
   
		/// </summary>
		public string LogTag
		{
            get {return  _logTag;}
            set { _logTag = value;}
		}
		/// <summary>
		/// 用户IP
		/// </summary>
		public string UserIP
		{
            get {return  _userIP;}
            set { _userIP = value;}
		}
		/// <summary>
		/// 消息
		/// </summary>
		public string Message
		{
            get {return  _message;}
            set { _message = value;}
		}
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime? CreateTime
		{
            get {return  _createTime;}
            set { _createTime = value;}
		}
		#endregion
        
        #region 方法
        //获取对象hashcode
        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            sb.Append(this.GetType().FullName);
			sb.Append( _logID);
			sb.Append( _loginName);
			sb.Append( _userName);
			sb.Append( _logTag);
			sb.Append( _userIP);
			sb.Append( _message);
			sb.Append( _createTime);

            return sb.ToString().GetHashCode();
        }
		#endregion
	}
}