namespace UkeyTech.WebFW.Model
{
	using System;
	using System.Data;
    using Clover.Core.Domain;
    
	/// <summary>
	/// CF_AssignUserList 数据模型层
    /// Descn:
    /// Creator:laozijian
    /// AddDate:2012-09-02
    /// Creator:laozijian
    /// UpdateTime:2012-09-02
	/// </summary>
	[Serializable]
	public class AssignUser : IEntity
	{
       
        #region 私有属性
		/// <summary>
		/// (委托人)
		/// </summary>
		private int  _userID;
		/// <summary>
		/// 用户名
		/// </summary>
		private string  _assignToUserId;
		/// <summary>
		/// 业务ID
		/// </summary>
		private string  _formID;
	
		#endregion

        public AssignUser()
		{
		}
				
		#region 公有属性		
		/// <summary>
		/// (主键)
		/// </summary>
        public int UserId
		{
            get {return  _userID;}
            set { _userID = value;}
		}
		/// <summary>
		/// 被委托人
		/// </summary>
        public string AssignToUserId
		{
            get { return _assignToUserId; }
            set { _assignToUserId = value; }
		}
		/// <summary>
		/// 登录名
		/// </summary>
        public string FormID
		{
            get { return _formID; }
            set { _formID = value; }
		}
	
		#endregion
        
        #region 方法
        //获取对象hashcode
        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            sb.Append(this.GetType().FullName);
			sb.Append( _userID);
			sb.Append( _assignToUserId);
			sb.Append(_formID);
		

            return sb.ToString().GetHashCode();
        }
		#endregion
	}
}