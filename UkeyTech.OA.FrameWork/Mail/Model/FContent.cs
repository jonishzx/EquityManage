namespace UkeyTech.WebFW.Mail.Model
{
    using System;
    using System.Data;
    using Dapper.Contrib.Extensions;
    using Clover.Core.Domain;

    /// <summary>
    /// FContent 数据模型层
    /// Descn:
    /// Creator:laozijian
    /// AddDate:2013-09-24
    /// Creator:laozijian
    /// UpdateTime:2013-09-24    
    /// </summary>
    [Serializable]
    [Table("Mail_FContent")]
    public class FContent : IEntity
    {

        #region 私有属性
        /// <summary>
        /// (主键)
        /// </summary>
        private string _mailId;
        /// <summary>
        /// 
        /// </summary>
        private string _title;
        /// <summary>
        /// 
        /// </summary>
        private string _mContent;
        /// <summary>
        /// 
        /// </summary>
        private string _status;
        /// <summary>
        /// 
        /// </summary>
        private string _sender;
        /// <summary>
        /// 
        /// </summary>
        private DateTime? _createDate;
        #endregion

        public FContent()
        {
        }

        #region 公有属性
        /// <summary>
        /// (主键)
        /// </summary>
        [Key]
        public string MailId
        {
            get { return _mailId; }
            set { _mailId = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MContent
        {
            get { return _mContent; }
            set { _mContent = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Sender
        {
            get { return _sender; }
            set { _sender = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateDate
        {
            get { return _createDate; }
            set { _createDate = value; }
        }
        #endregion

        #region 方法
        //获取对象hashcode
        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_mailId);
            sb.Append(_title);
            sb.Append(_mContent);
            sb.Append(_status);
            sb.Append(_sender);
            sb.Append(_createDate);

            return sb.ToString().GetHashCode();
        }
        #endregion
    }
}