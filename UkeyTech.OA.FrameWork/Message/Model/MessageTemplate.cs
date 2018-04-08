namespace Clover.Message.Model
{
	using System;
	using System.Data;
    using Dapper.Contrib.Extensions;
    using Clover.Core.Domain;
    
	/// <summary>
	/// MessageTemplate 数据模型层
    /// Descn:
    /// Creator:laozijian
    /// AddDate:2013-10-03
    /// Creator:laozijian
    /// UpdateTime:2013-10-03    
	/// </summary>
	[Serializable]
    [Table("msg_MessageTemplate")]
	public class MessageTemplate : IEntity
	{
       
        #region 私有属性
		/// <summary>
		/// (主键)
		/// </summary>
		private string  _code;
		/// <summary>
		/// 消息类型名称
		/// </summary>
		private string  _name;
		/// <summary>
		/// 消息本体Action的Uri
		/// </summary>
		private string  _messageAction;
		/// <summary>
		/// 消息动作Action的Uri
		/// </summary>
		private string  _operationAction;
		/// <summary>
		/// 动作Action的Uri(预留)
		/// </summary>
		private string  _extendAction1;
		/// <summary>
		/// 消息动作Action的Uri(预留)
		/// </summary>
		private string  _extendAction2;
		/// <summary>
		/// 消息类型名称
		/// </summary>
		private string  _descn;
		/// <summary>
		/// 创建人
		/// </summary>
		private string  _creator;
		/// <summary>
		/// 创建时间
		/// </summary>
		private DateTime  _createTime;
		/// <summary>
		/// 修改时间
		/// </summary>
		private DateTime  _updateTime;
		/// <summary>
		/// 状态：1为正常，0为删除
		/// </summary>
		private string  _status;
		/// <summary>
		/// 
		/// </summary>
		private bool?  _needAccept;
		/// <summary>
		/// 可以被标记已读
		/// </summary>
		private bool?  _needRead;
		#endregion
		
		public MessageTemplate()
		{
		}
				
		#region 公有属性		
		/// <summary>
		/// (主键)
		/// </summary>
        [Key]
		public string Code
		{
            get {return  _code;}
            set { _code = value;}
		}
		/// <summary>
		/// 消息类型名称
		/// </summary>
		public string Name
		{
            get {return  _name;}
            set { _name = value;}
		}
		/// <summary>
		/// 消息本体Action的Uri
		/// </summary>
		public string MessageAction
		{
            get {return  _messageAction;}
            set { _messageAction = value;}
		}
		/// <summary>
		/// 消息动作Action的Uri
		/// </summary>
		public string OperationAction
		{
            get {return  _operationAction;}
            set { _operationAction = value;}
		}
		/// <summary>
		/// 动作Action的Uri(预留)
		/// </summary>
		public string ExtendAction1
		{
            get {return  _extendAction1;}
            set { _extendAction1 = value;}
		}
		/// <summary>
		/// 消息动作Action的Uri(预留)
		/// </summary>
		public string ExtendAction2
		{
            get {return  _extendAction2;}
            set { _extendAction2 = value;}
		}
		/// <summary>
		/// 消息类型名称
		/// </summary>
		public string Descn
		{
            get {return  _descn;}
            set { _descn = value;}
		}
		/// <summary>
		/// 创建人
		/// </summary>
		public string Creator
		{
            get {return  _creator;}
            set { _creator = value;}
		}
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime CreateTime
		{
            get {return  _createTime;}
            set { _createTime = value;}
		}
		/// <summary>
		/// 修改时间
		/// </summary>
		public DateTime UpdateTime
		{
            get {return  _updateTime;}
            set { _updateTime = value;}
		}
		/// <summary>
		/// 状态：1为正常，0为删除
		/// </summary>
		public string Status
		{
            get {return  _status;}
            set { _status = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool? NeedAccept
		{
            get {return  _needAccept;}
            set { _needAccept = value;}
		}
		/// <summary>
		/// 可以被标记已读
		/// </summary>
		public bool? NeedRead
		{
            get {return  _needRead;}
            set { _needRead = value;}
		}

        /// <summary>
        /// 是否可回复
        /// </summary>
        public bool? CanReplay { get; set; }
		#endregion
        
        #region 方法
        //获取对象hashcode
        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            sb.Append(this.GetType().FullName);
			sb.Append( _code);
			sb.Append( _name);
			sb.Append( _messageAction);
			sb.Append( _operationAction);
			sb.Append( _extendAction1);
			sb.Append( _extendAction2);
			sb.Append( _descn);
			sb.Append( _creator);
			sb.Append( _createTime);
			sb.Append( _updateTime);
			sb.Append( _status);
			sb.Append( _needAccept);
			sb.Append( _needRead);

            return sb.ToString().GetHashCode();
        }
		#endregion
	}
}