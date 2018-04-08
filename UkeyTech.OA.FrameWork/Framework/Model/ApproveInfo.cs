namespace UkeyTech.WebFW.Model
{
	using System;
	using System.Data;
    using Clover.Core.Domain;
    using Newtonsoft.Json;
    using Dapper.Contrib.Extensions;

    /// <summary>
    /// ApproveInfo 数据模型层
    /// Descn:
    /// Creator:laozijian
    /// AddDate:2012-08-12
    /// Creator:laozijian
    /// UpdateTime:2012-08-12    
    /// </summary>
    [Serializable]
    [Table("Biz_ApproveInfo")]
    public class ApproveInfo : IEntity
    {

        #region 私有属性
        /// <summary>
        /// (主键)
        /// </summary>
        private int _auditingID;
        /// <summary>
        /// 
        /// </summary>
        private string _staffID;
        /// <summary>
        /// 
        /// </summary>
        private string _auditingInfo;
        /// <summary>
        /// 
        /// </summary>
        private string _auditingState;
        /// <summary>
        /// 
        /// </summary>
        private DateTime? _auditingTime;
        /// <summary>
        /// 
        /// </summary>
        private string _remark;
        /// <summary>
        /// 
        /// </summary>
        private string _targetId;
        /// <summary>
        /// 
        /// </summary>
        private string _targetObject;
        /// <summary>
        /// 
        /// </summary>
        private string _targetDescn;
        #endregion

        public ApproveInfo()
        {
        }

        #region 公有属性
        /// <summary>
        /// (主键)
        /// </summary>
        [Key]
        public int AuditingID
        {
            get { return _auditingID; }
            set { _auditingID = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string StaffID
        {
            get { return _staffID; }
            set { _staffID = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AuditingInfo
        {
            get { return _auditingInfo; }
            set { _auditingInfo = value; }
        }
        /// <summary>
        /// 审核状态
        /// </summary>
        public string AuditingState
        {
            get { return _auditingState; }
            set { _auditingState = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? AuditingTime
        {
            get { return _auditingTime; }
            set { _auditingTime = value; }
        }

        /// <summary>
        /// 关联的工作任务ID
        /// </summary>
        public string WorkItemId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TargetId
        {
            get { return _targetId; }
            set { _targetId = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TargetObject
        {
            get { return _targetObject; }
            set { _targetObject = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TargetDescn
        {
            get { return _targetDescn; }
            set { _targetDescn = value; }
        }
        #endregion

        #region 方法
        //获取对象hashcode
        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_auditingID);
            sb.Append(_staffID);

            return sb.ToString().GetHashCode();
        }
        #endregion
    }
}