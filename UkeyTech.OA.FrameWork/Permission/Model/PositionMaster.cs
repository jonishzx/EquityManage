namespace Clover.Permission.Model
{
	using System;
	using System.Data;
    using Clover.Core.Domain;
    
	/// <summary>
	/// Group_User 数据模型层
    /// Descn:
    /// Creator:laozijian
    /// AddDate:2012-07-15
    /// Creator:laozijian
    /// UpdateTime:2012-07-15    
	/// </summary>
	[Serializable]
	public class PositionMaster : IEntity
	{
       
        #region 私有属性
		/// <summary>
		/// (主键)
		/// </summary>
        private int _MasterPositionID;
		/// <summary>
		/// (主键)
		/// </summary>
        private string _SubPositionID;
		#endregion
		
		public PositionMaster()
		{
		}
				
		#region 公有属性		
		/// <summary>
		/// (主键)主管级别岗位ID
		/// </summary>
		public int GroupID
		{
            get { return _MasterPositionID; }
            set { _MasterPositionID = value; }
		}
		/// <summary>
		/// (主键)下属级别岗位ID
		/// </summary>
		public string UserID
		{
            get { return _SubPositionID; }
            set { _SubPositionID = value; }
		}
		#endregion
        
        #region 方法
        //获取对象hashcode
        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            sb.Append(this.GetType().FullName);
            sb.Append(_SubPositionID);
            sb.Append(_SubPositionID);

            return sb.ToString().GetHashCode();
        }
		#endregion
	}
}