namespace UkeyTech.WebFW.Model
{
    using System;
    using System.Data;
    using Dapper.Contrib.Extensions;
    using Clover.Core.Domain;
    using Newtonsoft.Json;
    /// <summary>
    /// Attachment 数据模型层
    /// Descn:
    /// Creator:laozijian
    /// AddDate:2013-03-06
    /// Creator:laozijian
    /// UpdateTime:2013-03-06    
    /// </summary>
    [Serializable]
    [Table("sys_Attachment")]
    public class Attachment : IEntity
    {

        #region 私有属性
        /// <summary>
        /// (主键)
        /// </summary>
        private int _attachmentID;
        /// <summary>
        /// 标题
        /// </summary>
        private string _title;
        /// <summary>
        /// 目标ID
        /// </summary>
        private string _targetID;
        /// <summary>
        /// 目标类型(表名)
        /// </summary>
        private string _targetType;
        /// <summary>
        /// 文件路径 
        /// </summary>
        private string _filePath;
        /// <summary>
        /// 文件名称
        /// </summary>
        private string _fileName;
        /// <summary>
        /// 预览文件路径
        /// </summary>
        private string _previewFilePath;
        /// <summary>
        /// 文件大小
        /// </summary>
        private long _bytes;
        /// <summary>
        /// 描述
        /// </summary>
        private string _descn;
        /// <summary>
        /// 显示顺序
        /// </summary>
        private int _viewOrder;
        /// <summary>
        /// 该文件是否需要进行自动转换
        /// </summary>
        private int? _needConvert;
        /// <summary>
        /// 0:临时状态 1:正常状态 -1:删除状态
        /// </summary>
        private decimal? _status;
        /// <summary>
        /// 下载次数
        /// </summary>
        private int? _downloadCount;
        /// <summary>
        /// 创建人
        /// </summary>
        private string _creator;
        /// <summary>
        /// 更新时间
        /// </summary>
        private DateTime? _updateTime;
        #endregion

        public Attachment()
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
        /// 标题
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        /// <summary>
        /// 目标ID
        /// </summary>
        public string TargetID
        {
            get { return _targetID; }
            set { _targetID = value; }
        }
        /// <summary>
        /// 目标类型(表名)
        /// </summary>
        public string TargetType
        {
            get { return _targetType; }
            set { _targetType = value; }
        }
        /// <summary>
        /// 文件路径 
        /// </summary>
        [JsonIgnore]
        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }


        /// <summary>
        /// 预览文件路径
        /// </summary>
        [JsonIgnore]
        public string PreviewFilePath
        {
            get { return _previewFilePath; }
            set { _previewFilePath = value; }
        }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long Bytes
        {
            get { return _bytes; }
            set { _bytes = value; }
        }
        /// <summary>
        /// 描述
        /// </summary>
        public string Descn
        {
            get { return _descn; }
            set { _descn = value; }
        }
        /// <summary>
        /// 显示顺序
        /// </summary>
        public int ViewOrder
        {
            get { return _viewOrder; }
            set { _viewOrder = value; }
        }
        /// <summary>
        /// 该文件是否需要进行自动转换
        /// </summary>
        public int? NeedConvert
        {
            get { return _needConvert; }
            set { _needConvert = value; }
        }
        /// <summary>
        /// 0:临时状态 1:正常状态 -1:删除状态
        /// </summary>
        public decimal? Status
        {
            get { return _status; }
            set { _status = value; }
        }
        /// <summary>
        /// 下载次数
        /// </summary>
        public int? DownloadCount
        {
            get { return _downloadCount; }
            set { _downloadCount = value; }
        }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator
        {
            get { return _creator; }
            set { _creator = value; }
        }

        [Write(false)]
        public string CreatorName
        {
            get;
            set;
        }

        public string Tag
        {
            get;
            set;
        }
        /// <summary>
        /// 更新时间
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

            return sb.ToString().GetHashCode();
        }
        #endregion
    }
}