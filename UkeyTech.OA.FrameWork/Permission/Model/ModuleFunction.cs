namespace Clover.Permission.Model
{
	using System;
	using System.Data;
    using Clover.Core.Domain;
    
	/// <summary>
	/// Module_Function 数据模型层
    /// Descn:
    /// Creator:laozijian
    /// AddDate:2012-02-08
    /// Creator:laozijian
    /// UpdateTime:2012-02-08    
	/// </summary>
	[Serializable]
	public class ModuleFunction : IEntity
	{
       
        #region 私有属性
		/// <summary>
		/// (主键)
		/// </summary>
		private int  _moduleID;
		/// <summary>
		/// (主键)
		/// </summary>
		private int  _functionID;
		#endregion
		
		public ModuleFunction()
		{
		}
				
		#region 公有属性		
		/// <summary>
		/// (主键)
		/// </summary>
		public int ModuleID
		{
            get {return  _moduleID;}
            set { _moduleID = value;}
		}
		/// <summary>
		/// (主键)
		/// </summary>
		public int FunctionID
		{
            get {return  _functionID;}
            set { _functionID = value;}
		}
		#endregion
        
        #region 方法
        //获取对象hashcode
        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            sb.Append(this.GetType().FullName);
			sb.Append( _moduleID);
			sb.Append( _functionID);

            return sb.ToString().GetHashCode();
        }
		#endregion
	}
}