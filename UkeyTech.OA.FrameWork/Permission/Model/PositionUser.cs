namespace Clover.Permission.Model
{
	using System;
	using System.Data;
    using Clover.Core.Domain;
    
	/// <summary>
	/// Position_User 数据模型层
    /// Descn:
    /// Creator:laozijian
    /// AddDate:2012-07-15
    /// Creator:laozijian
    /// UpdateTime:2012-07-15    
	/// </summary>
	[Serializable]
	public class PositionUser : IEntity
	{
       
        #region 私有属性
		/// <summary>
		/// (主键)
		/// </summary>
		private int  _PositionID;
		/// <summary>
		/// (主键)
		/// </summary>
		private string  _userID;
		#endregion
		
		public PositionUser()
		{
		}
				
		#region 公有属性		
		/// <summary>
		/// (主键)岗位ID
		/// </summary>
		public int PositionID
		{
            get {return  _PositionID;}
            set { _PositionID = value;}
		}
		/// <summary>
		/// (主键)用户ID
		/// </summary>
		public string UserID
		{
            get {return  _userID;}
            set { _userID = value;}
		}

        /// <summary>
        /// 用户组织ID
        /// </summary>
	    public int? GroupID { get; set; }

        /// <summary>
        /// 用户角色ID
        /// </summary>
        public int? RoleID { get; set; }
	    #endregion
        
        #region 方法
        //获取对象hashcode
        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            sb.Append(this.GetType().FullName);
            sb.Append(GroupID);
			sb.Append( _PositionID);
			sb.Append( _userID);

            return sb.ToString().GetHashCode();
        }
		#endregion
	}
}