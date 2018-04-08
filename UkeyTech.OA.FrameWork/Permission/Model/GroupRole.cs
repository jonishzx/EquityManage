namespace Clover.Permission.Model
{
	using System;
	using System.Data;
    using Clover.Core.Domain;
    
	/// <summary>
	/// Group_Role 数据模型层
    /// Descn:
    /// Creator:laozijian
    /// AddDate:2012-02-08
    /// Creator:laozijian
    /// UpdateTime:2012-02-08    
	/// </summary>
	[Serializable]
	public class GroupRole : IEntity
	{
       
        #region 私有属性
		/// <summary>
		/// (主键)
		/// </summary>
		private int  _roleID;
		/// <summary>
		/// (主键)
		/// </summary>
		private int  _groupID;
		#endregion
		
		public GroupRole()
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
		public int GroupID
		{
            get {return  _groupID;}
            set { _groupID = value;}
		}
		#endregion
        
        #region 方法
        //获取对象hashcode
        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            sb.Append(this.GetType().FullName);
			sb.Append( _roleID);
			sb.Append( _groupID);

            return sb.ToString().GetHashCode();
        }
		#endregion
	}
}