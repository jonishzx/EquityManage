namespace UkeyTech.WebFW.Model
{
    using System;
    using System.Data;
    using Dapper.Contrib.Extensions;
    using Clover.Core.Domain;


    /// <summary>
    /// FormColumn 数据模型层
    /// Descn:
    /// Creator:laozijian
    /// AddDate:2012-07-05
    /// Creator:laozijian
    /// UpdateTime:2012-07-05    
    /// </summary>
    [Serializable]
    [Table("CF_FormColumn")]
    public class FormColumn : IEntity
    {
        public static readonly string Const = "Const";
        public static readonly string Single = "Single";
        public static readonly string Mutil = "Mutil";
        public static readonly string CurrentUser = "CurrentUserName";
        public static readonly string Null = "-1";

        #region 私有属性
        /// <summary>
        /// (主键)
        /// </summary>
        private int _iD;
        /// <summary>
        /// 
        /// </summary>
        private int _formID;
        /// <summary>
        /// 
        /// </summary>
        private string _colName;
        /// <summary>
        /// 
        /// </summary>
        private string _colCaption;
        /// <summary>
        /// 
        /// </summary>
        private string _colType;
        /// <summary>
        /// 
        /// </summary>
        private int? _viewOrd;
        /// <summary>
        /// 
        /// </summary>
        private string _colHtml;
        /// <summary>
        /// 
        /// </summary>
        private int? _status = 1;
        /// <summary>
        /// 
        /// </summary>
        private int? _size = 50;
        /// <summary>
        /// 
        /// </summary>
        private int? _fPSize = 1;
        /// <summary>
        /// 
        /// </summary>
        private string _isSelectCol = "-1";
        /// <summary>
        /// 
        /// </summary>
        private string _selectType = "-1";
        /// <summary>
        /// 
        /// </summary>
        private int? _required;
        #endregion

        public FormColumn()
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
        /// <summary>
        /// 
        /// </summary>
        public int FormID
        {
            get { return _formID; }
            set { _formID = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ColName
        {
            get { return _colName; }
            set { _colName = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ColCaption
        {
            get { return _colCaption; }
            set { _colCaption = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ColType
        {
            get { return _colType; }
            set { _colType = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ViewOrd
        {
            get { return _viewOrd; }
            set { _viewOrd = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ColHtml
        {
            get { return _colHtml; }
            set { _colHtml = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Status
        {
            get { return _status; }
            set { _status = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Size
        {
            get { return _size; }
            set { _size = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? FPSize
        {
            get { return _fPSize; }
            set { _fPSize = value; }
        }
        /// <summary>
        /// 选择的类型(Const:固定下拉,Single:引用数据表下拉,Mutil:弹出选择,CurrenntUser:当前用户信息)
        /// </summary>
        public string SelectColType
        {
            get { return _isSelectCol; }
            set { _isSelectCol = value; }
        }
        /// <summary>
        /// 目标iD，与字典表sys_Dictionary中parentid为1000的数据进行关联
        /// </summary>
        public string SelectTypeId
        {
            get { return _selectType; }
            set { _selectType = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Required
        {
            get { return _required; }
            set { _required = value; }
        }

        /// <summary>
        /// 是否自动生成字段
        /// </summary>
        public int AutoCreateTableName { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Descn { get; set; }

        /// <summary>
        /// 可操作性（可见:1,可编辑：2, 发起人可编辑）
        /// </summary>
        public int OPStatus { get; set; }

        /// <summary>
        /// 属于流程变量
        /// </summary>
        public int IsProcessVar { get; set; }
        #endregion

        #region 方法
        //获取对象hashcode
        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_iD);
            sb.Append(_formID);
            sb.Append(_colName);
            sb.Append(_colCaption);

            return sb.ToString().GetHashCode();
        }
        #endregion
    }
}