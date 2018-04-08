namespace UkeyTech.WebFW.Mail.Model
{
    using System;
    using System.Data;
    using Dapper.Contrib.Extensions;
    using Clover.Core.Domain;

    /// <summary>
    /// CReceiver 数据模型层
    /// Descn:
    /// Creator:laozijian
    /// AddDate:2013-09-24
    /// Creator:laozijian
    /// UpdateTime:2013-09-24    
    /// </summary>
    [Serializable]
    [Table("Mail_CReceiver")]
    public class CReceiver : IEntity
    {

        #region 私有属性
        /// <summary>
        /// (主键)
        /// </summary>
        private string _receId;
        /// <summary>
        /// 
        /// </summary>
        private string _receiver;
        /// <summary>
        /// 
        /// </summary>
        private DateTime? _recDate;
        /// <summary>
        /// 
        /// </summary>
        private string _status;
        /// <summary>
        /// 
        /// </summary>
        private string _iSRead;
        /// <summary>
        /// 
        /// </summary>
        private string _remark;
        #endregion

        public CReceiver()
        {
        }

        #region 公有属性
        /// <summary>
        /// (主键)
        /// </summary>
        [Key]
        public string ReceId
        {
            get { return _receId; }
            set { _receId = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Receiver
        {
            get { return _receiver; }
            set { _receiver = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? RecDate
        {
            get { return _recDate; }
            set { _recDate = value; }
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
        public string ISRead
        {
            get { return _iSRead; }
            set { _iSRead = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        #endregion

        #region 方法
        //获取对象hashcode
        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_receId);
            sb.Append(_receiver);
            sb.Append(_recDate);
            sb.Append(_status);
            sb.Append(_iSRead);
            sb.Append(_remark);

            return sb.ToString().GetHashCode();
        }
        #endregion
    }
}