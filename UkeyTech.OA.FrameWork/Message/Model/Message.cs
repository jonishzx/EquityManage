namespace Clover.Message.Model
{
	using System;
	using System.Data;
    using Dapper.Contrib.Extensions;
    using Clover.Core.Domain;
    using Newtonsoft.Json;
	/// <summary>
	/// Message 数据模型层
    /// Descn:消息盒
    /// Creator:laozijian
    /// AddDate:2013-10-03
    /// Creator:laozijian
    /// UpdateTime:2013-10-03    
	/// </summary>
	[Serializable]
    [Table("msg_Message")]
	public class Message : IEntity
	{
       
        #region 私有属性
		/// <summary>
		/// (主键)
		/// </summary>
		private string  _messageId;
		/// <summary>
		/// 
		/// </summary>
		private string  _title;
		/// <summary>
		/// 信息内容
		/// </summary>
		private string  _message;
		/// <summary>
		/// 
		/// </summary>
		private DateTime?  _beginDateTime;
		/// <summary>
		/// 
		/// </summary>
		private DateTime?  _endDateTime;
		/// <summary>
		/// 状态(A:新增,B:已送,C:已读,D:已关闭,E:已删除)
		/// </summary>
		private string  _status;
		/// <summary>
		/// 是否自动提醒
		/// </summary>
		private string  _autoAlert;
		/// <summary>
		/// 目标业务ID
		/// </summary>
		private string  _targetId;
		/// <summary>
		/// 目标业务对象
		/// </summary>
		private string  _targetObject;
		/// <summary>
		/// 关联信息ID
		/// </summary>
		private string  _referMessageId;
		/// <summary>
		/// 发送者
		/// </summary>
		private string  _sender;
		/// <summary>
		/// 接收者(多人)
		/// </summary>
		private string  _receivers;
		/// <summary>
		/// 发送时间
		/// </summary>
		private DateTime?  _sendTime;
	
		/// <summary>
		/// 
		/// </summary>
		private string  _creator;
		/// <summary>
		/// 
		/// </summary>
		private DateTime?  _createTime;
		/// <summary>
		/// 关联的视图
		/// </summary>
		private string  _messageAction;
		/// <summary>
		/// 
		/// </summary>
		private string  _operationAction;
		/// <summary>
		/// 
		/// </summary>
		private string  _extendAction1;
		/// <summary>
		/// 
		/// </summary>
		private string  _extendAction2;
		/// <summary>
		/// 
		/// </summary>
		private bool?  _needAccept;
		/// <summary>
		/// 需要人为标记为已读，否则打开时默认标记已读
		/// </summary>
		private bool?  _needRead;
		/// <summary>
		/// 来源模版代码
		/// </summary>
		private string  _templateCode;
		#endregion
		
		public Message()
		{
		}
				
		#region 公有属性		
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
		/// 
		/// </summary>
		public string Title
		{
            get {return  _title;}
            set { _title = value;}
		}
		/// <summary>
		/// 信息内容
		/// </summary>
		public string MessageBody
		{
            get {return  _message;}
            set { _message = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? BeginDateTime
		{
            get {return  _beginDateTime;}
            set { _beginDateTime = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? EndDateTime
		{
            get {return  _endDateTime;}
            set { _endDateTime = value;}
		}
		/// <summary>
		/// 状态(A:新增,B:已送,C:已读,D:已关闭,E:已删除,G:已回复)
		/// </summary>
		public string Status
		{
            get {return  _status;}
            set { _status = value;}
		}
		/// <summary>
		/// 是否自动提醒
		/// </summary>
		public string AutoAlert
		{
            get {return  _autoAlert;}
            set { _autoAlert = value;}
		}
		/// <summary>
		/// 目标业务ID
		/// </summary>
		public string TargetId
		{
            get {return  _targetId;}
            set { _targetId = value;}
		}
		/// <summary>
		/// 目标业务对象
		/// </summary>
		public string TargetObject
		{
            get {return  _targetObject;}
            set { _targetObject = value;}
		}
		/// <summary>
		/// 关联信息ID
		/// </summary>
		public string ReferMessageId
		{
            get {return  _referMessageId;}
            set { _referMessageId = value;}
		}
		/// <summary>
		/// 发送者
		/// </summary>
		public string Sender
		{
            get {return  _sender;}
            set { _sender = value;}
		}
		/// <summary>
		/// 接收者(多人)
		/// </summary>
		public string Receivers
		{
            get {return  _receivers;}
            set { _receivers = value;}
		}
		/// <summary>
		/// 发送时间
		/// </summary>
		public DateTime? SendTime
		{
            get {return  _sendTime;}
            set { _sendTime = value;}
		}
		
		/// <summary>
		/// 
		/// </summary>
		public string Creator
		{
            get {return  _creator;}
            set { _creator = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? CreateTime
		{
            get {return  _createTime;}
            set { _createTime = value;}
		}
		/// <summary>
		/// 关联的视图
		/// </summary>
		public string MessageAction
		{
            get {return  _messageAction;}
            set { _messageAction = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string OperationAction
		{
            get {return  _operationAction;}
            set { _operationAction = value;}
		}
        [JsonIgnore]
		/// <summary>
		/// 
		/// </summary>
		public string ExtendAction1
		{
            get {return  _extendAction1;}
            set { _extendAction1 = value;}
		}

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
	
		public string ExtendAction2
		{
            get {return  _extendAction2;}
            set { _extendAction2 = value;}
		}

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
		public bool? NeedAccept
		{
            get {return  _needAccept;}
            set { _needAccept = value;}
		}

        /// <summary>
        /// 需要人为标记为已读，否则打开时默认标记已读
        /// </summary>
        [JsonIgnore]
		public bool? NeedRead
		{
            get {return  _needRead;}
            set { _needRead = value;}
		}
		/// <summary>
		/// 来源模版代码
		/// </summary>
		public string TemplateCode
		{
            get {return  _templateCode;}
            set { _templateCode = value;}
		}

        /// <summary>
        /// 阅读时间
        /// </summary>
        [Write(false)]

        public DateTime ReadTime { get; set; }

        /// <summary>
        /// 收取时间
        /// </summary>
        [Write(false)]
        public DateTime ReceiveTime { get; set; }

        /// <summary>
        /// 标记
        /// </summary>
        [Write(false)]
        public string Mark { get; set; }

        /// <summary>
        /// 是否已经回收
        /// </summary>
        [Write(false)]
        [JsonIgnore]
        public bool IsRecyle { get; set; }

        /// <summary>
        /// 是否可回复
        /// </summary>
        [JsonIgnore]
        public bool? CanReplay { get; set; }

        /// <summary>
        /// 收件人
        /// </summary>
        public string ReceiversName { get; set; }

        [JsonIgnore]
        [Write(false)]
        public DataRowState ObjectStatus { get; set; }
        /// <summary>
        /// 当前状态
        /// </summary>
        [Write(false)]
        public string StatusName { 
            get {
                if (string.IsNullOrEmpty(Status))
                    return "草稿";

                switch (Status) { 
                    case "A":
                        return "已发送";
                    case "B":
                        return "已读";
                    case "C":
                        return "已接受";
                    case "D":
                        return "已处理";
                    case "E":
                        return "已撤回";
                    case "F":
                        return "已回收";
                    case "G":
                        return "已回复";
                    default:
                        return "未知";
                }
            } 
        }

   
		#endregion
        
        #region 方法
        //获取对象hashcode
        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            sb.Append(this.GetType().FullName);
			sb.Append( _messageId);
		
            return sb.ToString().GetHashCode();
        }
		#endregion
        /// <summary>
        /// 发件人名称
        /// </summary>
        [Write(false)]
        public string SenderName { get; set; }

        public string CreatorName { get; set; }
    }
}