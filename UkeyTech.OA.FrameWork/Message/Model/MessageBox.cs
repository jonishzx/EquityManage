namespace Clover.Message.Model
{
	using System;
	using System.Data;
    using Dapper.Contrib.Extensions;
    using Clover.Core.Domain;
    
	/// <summary>
	/// MessageBox 数据模型层
    /// Descn:
    /// Creator:laozijian
    /// AddDate:2013-10-03
    /// Creator:laozijian
    /// UpdateTime:2013-10-03    
	/// </summary>
	[Serializable]
    [Table("msg_MessageBox")]
	public class MessageBox : IEntity, Clover.Core.Collection.ISNode
	{
       
        #region 私有属性
		/// <summary>
		/// (主键)
		/// </summary>
		private int  _messageBoxId;
		/// <summary>
		/// 消息箱名称
		/// </summary>
		private string  _name;
		/// <summary>
		/// 状态(enabled:可用,delete:删除)
		/// </summary>
		private string  _status;
		/// <summary>
		/// 
		/// </summary>
		private string  _Owner;
		/// <summary>
		/// 
		/// </summary>
		private DateTime?  _createTime;
		/// <summary>
		/// 消息箱类型(inbox:收件,outbox:发件,recyle:垃圾箱)
		/// </summary>
		private string  _boxType;
		/// <summary>
		/// 父节点
		/// </summary>
		private int?  _parent;
		#endregion
		
		public MessageBox()
		{
		}
				
		#region 公有属性		
		/// <summary>
		/// (主键)
		/// </summary>
        [Key]
		public int MessageBoxId
		{
            get {return  _messageBoxId;}
            set { _messageBoxId = value;}
		}
		/// <summary>
		/// 消息箱名称
		/// </summary>
		public string Name
		{
            get {return  _name;}
            set { _name = value;}
		}
		/// <summary>
		/// 状态(enabled:可用,delete:删除)
		/// </summary>
		public string Status
		{
            get {return  _status;}
            set { _status = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Owner
		{
            get {return  _Owner;}
            set { _Owner = value;}
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
		/// 消息箱类型(inbox:收件,outbox:发件,recyle:垃圾箱)
		/// </summary>
		public string BoxType
		{
            get {return  _boxType;}
            set { _boxType = value;}
		}
		/// <summary>
		/// 父节点
		/// </summary>
		public int? Parent
		{
            get {return  _parent;}
            set { _parent = value;}
		}

        public int ViewOrder { get; set; }

        /// <summary>
        /// 消息数
        /// </summary>
        [Write(false)]       
        public int InBoxCount { get; set; }
		#endregion
        
        #region 方法
        //获取对象hashcode
        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            sb.Append(this.GetType().FullName);
			sb.Append( _messageBoxId);
			sb.Append( _name);
			sb.Append( _status);
			sb.Append( _Owner);
			sb.Append( _createTime);
			sb.Append( _boxType);
			sb.Append( _parent);

            return sb.ToString().GetHashCode();
        }
		#endregion



        public string Id
        {
            get
            {
                return MessageBoxId.ToString();
            }
            set
            {
                MessageBoxId = int.Parse(value);
            }
        }

        public string ParentId
        {
            get
            {
                return Parent.HasValue ? Parent.ToString() : null;
            }
            set
            {
                Parent = int.Parse(value);
            }
        }
    }
}