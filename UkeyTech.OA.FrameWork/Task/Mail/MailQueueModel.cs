using System;
namespace UkeyTech.WebFW.Model
{
	/// <summary>
	/// 实体类MailQueue_Working 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class MailQueue
	{
        public MailQueue()
		{}
		#region Model
		private int _mailqueueid;
		private string _ptype;
		private string _form;
		private string _sendto;
		private string _subject;
		private string _message;
		private byte[] _attachments;
		private DateTime _createdate;
		private DateTime? _senddate;
		/// <summary>
		/// 
		/// </summary>
		public int MailQueueId
		{
			set{ _mailqueueid=value;}
			get{return _mailqueueid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string PType
		{
			set{ _ptype=value;}
			get{return _ptype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Form
		{
			set{ _form=value;}
			get{return _form;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SendTo
		{
			set{ _sendto=value;}
			get{return _sendto;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Subject
		{
			set{ _subject=value;}
			get{return _subject;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Message
		{
			set{ _message=value;}
			get{return _message;}
		}
		/// <summary>
		/// 
		/// </summary>
		public byte[] Attachments
		{
			set{ _attachments=value;}
			get{return _attachments;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime CreateDate
		{
			set{ _createdate=value;}
			get{return _createdate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? SendDate
		{
			set{ _senddate=value;}
			get{return _senddate;}
		}
		#endregion Model

	}
}

