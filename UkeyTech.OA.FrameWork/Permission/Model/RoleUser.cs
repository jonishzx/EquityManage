namespace Clover.Permission.Model
{
	using System;
	using System.Data;
    using Clover.Core.Domain;
    
	/// <summary>
	/// Role_User 数据模型层
    /// Descn:
    /// Creator:laozijian
    /// AddDate:2012-02-08
    /// Creator:laozijian
    /// UpdateTime:2012-02-08    
	/// </summary>
	[Serializable]
	public class RoleUser : IEntity
	{
       
        #region 私有属性
		/// <summary>
		/// (主键)
		/// </summary>
		private int  _roleID;
		/// <summary>
		/// (主键)
		/// </summary>
		private string  _userID;
		#endregion
		
		public RoleUser()
		{
		}
				
		#region 公有属性		
		/// <summary>
		/// (主键)
		/// </summary>
		public int RoleID
		{
            get {return  _roleID;}
            set { _roleID = value;}
		}
		/// <summary>
		/// (主键)
		/// </summary>
		public string UserID
		{
            get {return  _userID;}
            set { _userID = value;}
		}
		#endregion
        
        #region 方法
        //获取对象hashcode
        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            sb.Append(this.GetType().FullName);
			sb.Append( _roleID);
			sb.Append( _userID);

            return sb.ToString().GetHashCode();
        }
		#endregion
	}
}