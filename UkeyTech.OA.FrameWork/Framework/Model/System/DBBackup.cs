namespace UkeyTech.WebFW.Model
{
	using System;
	using System.Data;
    using Clover.Core.Domain;
    
	/// <summary>
	/// DBBackup 数据模型层
    /// Descn:
    /// Creator:laozijian
    /// AddDate:2012-02-08
    /// Creator:laozijian
    /// UpdateTime:2012-02-08    
	/// </summary>
	[Serializable]
	public class DBBackup : IEntity
	{
       
        #region 私有属性
		/// <summary>
		/// (主键)
		/// </summary>
		private int  _id;
		/// <summary>
		/// 备份时间
		/// </summary>
		private DateTime  _updatetime;
		/// <summary>
		/// 
		/// </summary>
		private string  _fileName;
		/// <summary>
		/// 备份的服务器路径
		/// </summary>
		private string  _serverPath;
		/// <summary>
		/// 操作员ID
		/// </summary>
		private int  _operatorId;
		#endregion
		
		public DBBackup()
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
		/// 备份时间
		/// </summary>
		public DateTime Updatetime
		{
            get {return  _updatetime;}
            set { _updatetime = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string FileName
		{
            get {return  _fileName;}
            set { _fileName = value;}
		}
		/// <summary>
		/// 备份的服务器路径
		/// </summary>
		public string ServerPath
		{
            get {return  _serverPath;}
            set { _serverPath = value;}
		}
		/// <summary>
		/// 操作员ID
		/// </summary>
		public int OperatorId
		{
            get {return  _operatorId;}
            set { _operatorId = value;}
		}
		#endregion
        
        #region 方法
        //获取对象hashcode
        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            sb.Append(this.GetType().FullName);
			sb.Append( _id);
			sb.Append( _updatetime);
			sb.Append( _fileName);
			sb.Append( _serverPath);
			sb.Append( _operatorId);

            return sb.ToString().GetHashCode();
        }
		#endregion
	}
}