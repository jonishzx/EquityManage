namespace UkeyTech.WebFW.Model
{
	using System;
	using System.Data;
    using Clover.Core.Domain;
    
	/// <summary>
	/// User 数据模型层
    /// Descn:
    /// Creator:laozijian
    /// AddDate:2012-02-08
    /// Creator:laozijian
    /// UpdateTime:2012-02-08    
	/// </summary>
	[Serializable]
	public class User : IEntity
	{
       
        #region 私有属性
		/// <summary>
		/// (主键)
		/// </summary>
		private int  _userID;
		/// <summary>
		/// 用户名
		/// </summary>
		private string  _userName;
		/// <summary>
		/// 登录名
		/// </summary>
		private string  _loginName;
		/// <summary>
		/// 密码
		/// </summary>
		private string  _password;
		/// <summary>
		/// 邮件地址
		/// </summary>
		private string  _email;
		/// <summary>
		/// 加入时间
		/// </summary>
		private DateTime  _joined;
		/// <summary>
		/// 最后访问时间
		/// </summary>
		private DateTime?  _lastVisit;
		/// <summary>
		/// 注册ip
		/// </summary>
		private string  _iP;
		/// <summary>
		/// 语言
		/// </summary>
		private string  _languageFile;
		/// <summary>
		/// 
		/// </summary>
		private string  _phone;
		/// <summary>
		/// 
		/// </summary>
		private string  _qQ;
		/// <summary>
		/// 
		/// </summary>
		private string  _position;
		/// <summary>
		/// 
		/// </summary>
		private string  _sex;
		/// <summary>
		/// 用户状态(-1:禁用  0:注册  2激活)
		/// </summary>
		private int  _flags;
		/// <summary>
		/// 
		/// </summary>
		private string  _descn;
		/// <summary>
		/// 
		/// </summary>
		private bool?  _isActived;
		/// <summary>
		/// 安全问题
		/// </summary>
		private string  _fPWDQuestion;
		/// <summary>
		/// 安全答案
		/// </summary>
		private string  _fPWDAnswer;
		#endregion
		
		public User()
		{
		}
				
		#region 公有属性		
		/// <summary>
		/// (主键)
		/// </summary>
		public int UserID
		{
            get {return  _userID;}
            set { _userID = value;}
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
		/// 登录名
		/// </summary>
		public string LoginName
		{
            get {return  _loginName;}
            set { _loginName = value;}
		}
		/// <summary>
		/// 密码
		/// </summary>
		public string Password
		{
            get {return  _password;}
            set { _password = value;}
		}
		/// <summary>
		/// 邮件地址
		/// </summary>
		public string Email
		{
            get {return  _email;}
            set { _email = value;}
		}
		/// <summary>
		/// 加入时间
		/// </summary>
		public DateTime Joined
		{
            get {return  _joined;}
            set { _joined = value;}
		}
		/// <summary>
		/// 最后访问时间
		/// </summary>
		public DateTime? LastVisit
		{
            get {return  _lastVisit;}
            set { _lastVisit = value;}
		}
		/// <summary>
		/// 注册ip
		/// </summary>
		public string IP
		{
            get {return  _iP;}
            set { _iP = value;}
		}
		/// <summary>
		/// 语言
		/// </summary>
		public string LanguageFile
		{
            get {return  _languageFile;}
            set { _languageFile = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Phone
		{
            get {return  _phone;}
            set { _phone = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string QQ
		{
            get {return  _qQ;}
            set { _qQ = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Position
		{
            get {return  _position;}
            set { _position = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Sex
		{
            get {return  _sex;}
            set { _sex = value;}
		}
		/// <summary>
		/// 用户状态(-1:禁用  0:注册  2激活)
		/// </summary>
		public int Flags
		{
            get {return  _flags;}
            set { _flags = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Descn
		{
            get {return  _descn;}
            set { _descn = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool? IsActived
		{
            get {return  _isActived;}
            set { _isActived = value;}
		}
		/// <summary>
		/// 安全问题
		/// </summary>
		public string FPWDQuestion
		{
            get {return  _fPWDQuestion;}
            set { _fPWDQuestion = value;}
		}
		/// <summary>
		/// 安全答案
		/// </summary>
		public string FPWDAnswer
		{
            get {return  _fPWDAnswer;}
            set { _fPWDAnswer = value;}
		}
		#endregion
        
        #region 方法
        //获取对象hashcode
        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            sb.Append(this.GetType().FullName);
			sb.Append( _userID);
			sb.Append( _userName);
			sb.Append( _loginName);
			sb.Append( _password);
			sb.Append( _email);
			sb.Append( _joined);
			sb.Append( _lastVisit);
			sb.Append( _iP);
			sb.Append( _languageFile);
			sb.Append( _phone);
			sb.Append( _qQ);
			sb.Append( _position);
			sb.Append( _sex);
			sb.Append( _flags);
			sb.Append( _descn);
			sb.Append( _isActived);
			sb.Append( _fPWDQuestion);
			sb.Append( _fPWDAnswer);

            return sb.ToString().GetHashCode();
        }
		#endregion
	}
}