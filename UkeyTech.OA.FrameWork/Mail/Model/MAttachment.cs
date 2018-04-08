namespace UkeyTech.WebFW.Mail.Model
{
    using System;
    using System.Data;
    using Dapper.Contrib.Extensions;
    using Clover.Core.Domain;

    /// <summary>
    /// MAttachment 数据模型层
    /// Descn:
    /// Creator:laozijian
    /// AddDate:2013-09-24
    /// Creator:laozijian
    /// UpdateTime:2013-09-24    
    /// </summary>
    [Serializable]
    [Table("Mail_MAttachment")]
    public class MAttachment : IEntity
    {

        #region 私有属性
        /// <summary>
        /// (主键)
        /// </summary>
        private int _attachmentID;
        /// <summary>
        /// 
        /// </summary>
        private string _title;
        /// <summary>
        /// 
        /// </summary>
        private string _targetID;
        /// <summary>
        /// 
        /// </summary>
        private string _targetType;
        /// <summary>
        /// 
        /// </summary>
        private string _tag;
        /// <summary>
        /// 
        /// </summary>
        private string _filePath;
        /// <summary>
        /// 
        /// </summary>
        private string _fileName;
        /// <summary>
        /// 
        /// </summary>
        private string _previewFilePath;
        /// <summary>
        /// 
        /// </summary>
        private long _bytes;
        /// <summary>
        /// 
        /// </summary>
        private string _descn;
        /// <summary>
        /// 
        /// </summary>
        private int? _viewOrder;
        /// <summary>
        /// 
        /// </summary>
        private int? _needConvert;
        /// <summary>
        /// 
        /// </summary>
        private decimal? _status;
        /// <summary>
        /// 
        /// </summary>
        private int? _downloadCount;
        /// <summary>
        /// 
        /// </summary>
        private string _creator;
        /// <summary>
        /// 
        /// </summary>
        private DateTime? _updateTime;
        #endregion

        public MAttachment()
        {
        }

        #region 公有属性
        /// <summary>
        /// (主键)
        /// </summary>
        [Key]
        public int AttachmentID
        {
            get { return _attachmentID; }
            set { _attachmentID = value; }
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
        public string TargetID
        {
            get { return _targetID; }
            set { _targetID = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TargetType
        {
            get { return _targetType; }
            set { _targetType = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PreviewFilePath
        {
            get { return _previewFilePath; }
            set { _previewFilePath = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long Bytes
        {
            get { return _bytes; }
            set { _bytes = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Descn
        {
            get { return _descn; }
            set { _descn = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ViewOrder
        {
            get { return _viewOrder; }
            set { _viewOrder = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? NeedConvert
        {
            get { return _needConvert; }
            set { _needConvert = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? Status
        {
            get { return _status; }
            set { _status = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DownloadCount
        {
            get { return _downloadCount; }
            set { _downloadCount = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Creator
        {
            get { return _creator; }
            set { _creator = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? UpdateTime
        {
            get { return _updateTime; }
            set { _updateTime = value; }
        }
        #endregion

        #region 方法
        //获取对象hashcode
        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_attachmentID);
            sb.Append(_title);
            sb.Append(_targetID);
            sb.Append(_targetType);
            sb.Append(_tag);
            sb.Append(_filePath);
            sb.Append(_fileName);
            sb.Append(_previewFilePath);
            sb.Append(_bytes);
            sb.Append(_descn);
            sb.Append(_viewOrder);
            sb.Append(_needConvert);
            sb.Append(_status);
            sb.Append(_downloadCount);
            sb.Append(_creator);
            sb.Append(_updateTime);

            return sb.ToString().GetHashCode();
        }
        #endregion
    }
}