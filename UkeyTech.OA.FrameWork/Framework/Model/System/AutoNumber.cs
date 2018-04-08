namespace UkeyTech.WebFW.Model
{
	using System;
	using System.Data;
    using Clover.Core.Domain;
    
	/// <summary>
	/// AutoNumber 数据模型层
    /// Descn:
    /// Creator:laozijian
    /// AddDate:2012-02-08
    /// Creator:laozijian
    /// UpdateTime:2012-02-08    
	/// </summary>
	[Serializable]
	public class AutoNumber : IEntity
	{
       
        #region 私有属性
		/// <summary>
		/// (主键)
		/// </summary>
		private int  _iD;
		/// <summary>
		/// 目标
		/// </summary>
		private string  _target;
		/// <summary>
		/// 
		/// </summary>
		private string  _number;
		/// <summary>
		/// 
		/// </summary>
		private DateTime  _date;
		#endregion
		
		public AutoNumber()
		{
		}
				
		#region 公有属性		
		/// <summary>
		/// (主键)
		/// </summary>
		public int ID
		{
            get {return  _iD;}
            set { _iD = value;}
		}
		/// <summary>
		/// 目标
		/// </summary>
		public string Target
		{
            get {return  _target;}
            set { _target = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Number
		{
            get {return  _number;}
            set { _number = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime Date
		{
            get {return  _date;}
            set { _date = value;}
		}
		#endregion
        
        #region 方法
        //获取对象hashcode
        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            sb.Append(this.GetType().FullName);
			sb.Append( _iD);
			sb.Append( _target);
			sb.Append( _number);
			sb.Append( _date);

            return sb.ToString().GetHashCode();
        }
		#endregion
	}
}