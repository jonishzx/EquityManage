namespace Clover.Message.Model
{
	using System;
	using System.Data;
    using Dapper.Contrib.Extensions;
    using Clover.Core.Domain;
    
	/// <summary>
	/// BoxMessage 数据模型层
    /// Descn:
    /// Creator:laozijian
    /// AddDate:2013-10-03
    /// Creator:laozijian
    /// UpdateTime:2013-10-03    
	/// </summary>
	[Serializable]
    [Table("msg_BoxMessage")]
	public class BoxMessage : IEntity
	{
       
        #region 私有属性
		/// <summary>
		/// (主键)
		/// </summary>
		private long  _messageBoxId;
		/// <summary>
		/// (主键)
		/// </summary>
		private string  _messageId;
		/// <summary>
		/// (主键)
		/// </summary>
		private string  _receiver;
		/// <summary>
		/// 用户读取或操作消息的状态
		/// </summary>
		private string  _status;
		/// <summary>
		/// 如果消息的NeedAccept为1，则需要返回一个结果 
		/// </summary>
		private string  _result;
		/// <summary>
		/// 
		/// </summary>
		private DateTime?  _readTime;
		/// <summary>
		/// 已读备注
		/// </summary>
		private string  _readComment;
		/// <summary>
		/// 操作时间
		/// </summary>
		private DateTime?  _opTime;
		/// <summary>
		/// 操作备注
		/// </summary>
		private string  _opComment;
		/// <summary>
		/// 用户对消息的标记
		/// </summary>
		private string  _mark;
		#endregion
		
		public BoxMessage()
		{
		}
				
		#region 公有属性		
		/// <summary>
		/// (主键)
		/// </summary>
        [Key]
		public long MessageBoxId
		{
            get {return  _messageBoxId;}
            set { _messageBoxId = value;}
		}
		/// <summary>
		/// (主键)
		/// </summary>
        [Key]
		public string MessageId
		{
            get {return  _messageId;}
            set { _messageId = value;}
		}
		/// <summary>
		/// (主键)
		/// </summary>
        [Key]
		public string Receiver
		{
            get {return  _receiver;}
            set { _receiver = value;}
		}
		/// <summary>
		/// 用户读取或操作消息的状态
		/// </summary>
		public string Status
		{
            get {return  _status;}
            set { _status = value;}
		}
		/// <summary>
		/// 如果消息的NeedAccept为1，则需要返回一个结果 
		/// </summary>
		public string Result
		{
            get {return  _result;}
            set { _result = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? ReadTime
		{
            get {return  _readTime;}
            set { _readTime = value;}
		}
		/// <summary>
		/// 已读备注
		/// </summary>
		public string ReadComment
		{
            get {return  _readComment;}
            set { _readComment = value;}
		}
		/// <summary>
		/// 操作时间
		/// </summary>
		public DateTime? OpTime
		{
            get {return  _opTime;}
            set { _opTime = value;}
		}
		/// <summary>
		/// 操作备注
		/// </summary>
		public string OpComment
		{
            get {return  _opComment;}
            set { _opComment = value;}
		}
		/// <summary>
		/// 用户对消息的标记
		/// </summary>
		public string Mark
		{
            get {return  _mark;}
            set { _mark = value;}
		}

        /// <summary>
        /// 接收时间
        /// </summary>
        public DateTime ReceiveTime { get; set; }

        /// <summary>
        /// 是否已经回收
        /// </summary>
        public bool IsRecyle { get; set; }

        [Write(false)]
        public string ReceiverName { get; set; }

        [Write(false)]
        public string StatusName { get {
            if (string.IsNullOrEmpty(Status))
            {
                return "未读";
            }
            else {
                switch (Status) {
                    case "E":
                        return "已撤回";
                }
            }
            string rst = "";
            if (ReadTime.HasValue)
                rst = "已读,";

            if (OpTime.HasValue)
                rst += "已处理,";

            return rst.TrimEnd(',');
        } }
		#endregion
        
        #region 方法
        //获取对象hashcode
        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
			sb.Append( _messageBoxId);
			sb.Append( _messageId);
			sb.Append( _receiver);
		
            return sb.ToString().GetHashCode();
        }
		#endregion
	}
}