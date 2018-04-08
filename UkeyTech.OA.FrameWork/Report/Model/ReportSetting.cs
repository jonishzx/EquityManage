namespace UkeyTech.WebFW.Model
{
	using System;
	using System.Data;
    using Dapper.Contrib.Extensions;
    using Clover.Core.Domain;
    
	/// <summary>
	/// ReportSetting 数据模型层
    /// Descn:
    /// Creator:laozijian
    /// AddDate:2013-03-23
    /// Creator:laozijian
    /// UpdateTime:2013-03-23    
	/// </summary>
	[Serializable]
    [Table("sys_ReportSetting")]
	public class ReportSetting : IEntity
	{
       
        #region 私有属性
		/// <summary>
		/// (主键)
		/// </summary>
		private int  _recId;
		/// <summary>
		/// 报表标题
		/// </summary>
		private string  _title;
		/// <summary>
		/// 所属系统
		/// </summary>
		private string  _systemName;
		/// <summary>
		/// 分类
		/// </summary>
		private string  _tag;
		/// <summary>
		/// 存储过程(proc)或语句(sqlstring)
		/// </summary>
		private string  _cmdType;
		/// <summary>
		/// 执行的sql语句
		/// </summary>
		private string  _sQLCmd;
		/// <summary>
		/// 搜索用的控件
		/// </summary>
		private string  _queryPartial;
		/// <summary>
		/// 输出列的json
		/// </summary>
		private string  _columns;
		/// <summary>
		/// 状态(1:可见 0:不可见)
		/// </summary>
		private string  _status;
		/// <summary>
		/// 指向动作(如果没有,则使用默认的action)
		/// </summary>
		private string  _viewAction;
		/// <summary>
		/// 导出动作(如果没有,则使用默认的导出action)
		/// </summary>
		private string  _exportAction;
		/// <summary>
		/// 双击后的指向活动
		/// </summary>
		private string  _clickAction;
		/// <summary>
		/// 显示顺序
		/// </summary>
		private int?  _viewOrder;
		#endregion
		
		public ReportSetting()
		{
		}
				
		#region 公有属性		
		/// <summary>
		/// (主键)
		/// </summary>
        [Key]
		public int RecId
		{
            get {return  _recId;}
            set { _recId = value;}
		}
		/// <summary>
		/// 报表标题
		/// </summary>
		public string Title
		{
            get {return  _title;}
            set { _title = value;}
		}
		/// <summary>
		/// 所属系统
		/// </summary>
		public string SystemName
		{
            get {return  _systemName;}
            set { _systemName = value;}
		}
		/// <summary>
		/// 分类
		/// </summary>
		public string Tag
		{
            get {return  _tag;}
            set { _tag = value;}
		}
		/// <summary>
		/// 存储过程(proc)或语句(sqlstring)
		/// </summary>
		public string CmdType
		{
            get {return  _cmdType;}
            set { _cmdType = value;}
		}
		/// <summary>
		/// 执行的sql语句
		/// </summary>
		public string SQLCmd
		{
            get {return  _sQLCmd;}
            set { _sQLCmd = value;}
		}
		/// <summary>
		/// 搜索用的控件
		/// </summary>
		public string QueryPartial
		{
            get {return  _queryPartial;}
            set { _queryPartial = value;}
		}
		/// <summary>
		/// 输出列的json
		/// </summary>
		public string Columns
		{
            get {return  _columns;}
            set { _columns = value;}
		}
		/// <summary>
		/// 状态(1:可见 0:不可见)
		/// </summary>
		public string Status
		{
            get {return  _status;}
            set { _status = value;}
		}
		/// <summary>
		/// 指向动作(如果没有,则使用默认的action)
		/// </summary>
		public string ViewAction
		{
            get {return  _viewAction;}
            set { _viewAction = value;}
		}
		/// <summary>
		/// 导出动作(如果没有,则使用默认的导出action)
		/// </summary>
		public string ExportAction
		{
            get {return  _exportAction;}
            set { _exportAction = value;}
		}
		/// <summary>
		/// 双击后的指向活动
		/// </summary>
		public string ClickAction
		{
            get {return  _clickAction;}
            set { _clickAction = value;}
		}
		/// <summary>
		/// 显示顺序
		/// </summary>
		public int? ViewOrder
		{
            get {return  _viewOrder;}
            set { _viewOrder = value;}
		}


        /// <summary>
        /// 初始化的字典ID
        /// </summary>
        public string InitDictIds { get; set; }

	    #endregion
        
        #region 方法
        //获取对象hashcode
        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(this.GetType().FullName);
        
                    sb.Append( _recId);                 
            
            return sb.ToString().GetHashCode();
        }
		#endregion
	}
}