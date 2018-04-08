namespace UkeyTech.WebFW.Model
{
    using System;
    using System.Data;
    using Dapper.Contrib.Extensions;
    using Clover.Core.Domain;
    using Newtonsoft.Json;

    /// <summary>
    /// Form 数据模型层
    /// Descn:
    /// Creator:laozijian
    /// AddDate:2012-07-05
    /// Creator:laozijian
    /// UpdateTime:2012-07-05    
    /// </summary>
    [Serializable]
    [Table("CF_Form")]
    public class Form : IEntity
    {

        #region 私有属性
        /// <summary>
        /// (主键)
        /// </summary>
        private int _iD;
        /// <summary>
        /// 
        /// </summary>
        private string _formCode;
        /// <summary>
        /// 
        /// </summary>
        private string _formName;
        /// <summary>
        /// 
        /// </summary>
        private string _formType;
        /// <summary>
        /// 
        /// </summary>
        private string _forUserList;
        /// <summary>
        /// 
        /// </summary>
        private string _forUserListId;
        /// <summary>
        /// 
        /// </summary>
        private string _condition;
        /// <summary>
        /// 
        /// </summary>
        private string _formDesignContent;
        /// <summary>
        /// 创建时间
        /// </summary>
        private DateTime? _createTime;
        /// <summary>
        /// 修改时间
        /// </summary>
        private DateTime? _updateTime;
        /// <summary>
        /// 修改人

        /// </summary>
        private string _creator;
        /// <summary>
        /// 修改人
        /// </summary>
        private string _modifitor;
      
        #endregion

        public Form()
        {
        }

        #region 公有属性
        /// <summary>
        /// (主键)
        /// </summary>
        [Key]
        public int ID
        {
            get { return _iD; }
            set { _iD = value; }
        }
        [JsonIgnore]
        /// <summary>
        /// 
        /// </summary>
        public string FormCode
        {
            get { return _formCode; }
            set { _formCode = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FormName
        {
            get { return _formName; }
            set { _formName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string FormType
        {
            get { return _formType; }
            set { _formType = value; }
        }

        [JsonIgnore]
        /// <summary>
        /// 
        /// </summary>
        public string ForUserList
        {
            get { return _forUserList; }
            set { _forUserList = value; }
        }

        [JsonIgnore]
        /// <summary>
        /// 
        /// </summary>
        public string ForUserListId
        {
            get { return _forUserListId; }
            set { _forUserListId = value; }
        }

        [JsonIgnore]
        /// <summary>
        /// 
        /// </summary>
        public string Condition
        {
            get { return _condition; }
            set { _condition = value; }
        }

        [JsonIgnore]
        /// <summary>
        /// 
        /// </summary>
        public string FormDesignContent
        {
            get { return _formDesignContent; }
            set { _formDesignContent = value; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateTime
        {
            get { return _updateTime; }
            set { _updateTime = value; }
        }

         [JsonIgnore]
        /// <summary>
        /// 修改人
        /// </summary>
        public string Creator
        {
            get { return _creator; }
            set { _creator = value; }
        }
         [JsonIgnore]
        /// <summary>
        /// 修改人
        /// </summary>
        public string Modifitor
        {
            get { return _modifitor; }
            set { _modifitor = value; }
        }

        int _status = 1;
        public int Status { get{return _status;} set{_status = value;}}

        [JsonIgnore]
        public string Descn { get; set; }


        [JsonIgnore]
        /// <summary>
        /// 自动生成表名
        /// </summary>
        public int AutoCreateTableName { get; set; }

        /// <summary>
        /// 物理表名
        /// </summary>
        public string PsyTableName { get; set; }

        /// <summary>
        /// 业务显示的图片
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 工作流ID
        /// </summary>
        public string PROCESS_ID { get; set; }

        /// <summary>
        ///  非自动生成窗体,使用特定的窗体
        /// </summary>
        public string ExternalFormUrl { get; set; }

        /// <summary>
        ///   非自动生成列表,使用特定的列表
        /// </summary>
        public string ExternalListUrl { get; set; }

        /// <summary>
        /// 可访问的角色id
        /// </summary>
        public string LmtRoleIds { get; set; }

        /// <summary>
        /// 可访问的岗位id
        /// </summary>
        public string LmtPosIds { get; set; }

        /// <summary>
        /// 可访问的组织id
        /// </summary>
        public string LmtGroupIds { get; set; } 


        /// <summary>
        /// 该表已经存在
        /// </summary>
        [Write(false)]
        public int HasExits { get; set; }
        /// <summary>
        /// 排列顺序
        /// </summary>
        public int ViewOrd { get; set; } 
        #endregion

        #region 方法
        //获取对象hashcode
        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_iD);
            sb.Append(_formCode);
            sb.Append(_formName);
            sb.Append(_formType);
            return sb.ToString().GetHashCode();
        }
        #endregion
    }
}