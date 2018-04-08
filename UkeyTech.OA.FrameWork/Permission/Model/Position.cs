namespace Clover.Permission.Model
{
    using System;
    using System.Data;
    using Dapper.Contrib.Extensions;
    using Clover.Core.Domain;
    using Clover.Core.Collection;

    
    /// <summary>
    /// 岗位信息 数据模型层
    /// Descn:
    /// Creator:laozijian
    /// AddDate:2012-07-15
    /// Creator:laozijian
    /// UpdateTime:2012-07-15    
    /// </summary>
    [Serializable]
    [Table("CPM_Position")]
    public class Position : IEntity, ISNode, IComparable<Position>
    {

        #region 私有属性
        /// <summary>
        /// (主键)
        /// </summary>
        private int _positionID;
        /// <summary>
        /// 岗位代码
        /// </summary>
        private string _positionCode;
        /// <summary>
        /// 岗位名称
        /// </summary>
        private string _positionName;
        /// <summary>
        /// 所属组的ID
        /// </summary>
        private int? _groupId = -1;
        /// <summary>
        /// 注释
        /// </summary>
        private string _descn;
        /// <summary>
        /// 扩展
        /// </summary>
        private string _attribute;
        /// <summary>
        /// 显示顺序

        /// </summary>
        private int? _viewOrd;
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
        /// <summary>
        /// 状态(0:无效,1:有效)
        /// </summary>
        private int? _status = 1;

        /// <summary>
        /// 父组ID
        /// </summary>
        public int? _parentId = null;
        /// <summary>
        /// 层级父路径ID
        /// </summary>
        private string _parentPath;
        /// <summary>
        /// 职务等级，数字越大，等级越大
        /// </summary>
        private int? _positionLevel;
        #endregion

        public Position()
        {
        }

        #region 公有属性
        /// <summary>
        /// (主键)
        /// </summary>
        [Key]
        public int PositionID
        {
            get { return _positionID; }
            set { _positionID = value; }
        }
        /// <summary>
        /// 岗位代码
        /// </summary>
        public string PositionCode
        {
            get { return _positionCode; }
            set { _positionCode = value; }
        }
        /// <summary>
        /// 岗位名称
        /// </summary>
        public string PositionName
        {
            get { return _positionName; }
            set { _positionName = value; }
        }
        /// <summary>
        /// 所属组的ID
        /// </summary>
        public int? GroupId
        {
            get { return _groupId; }
            set { _groupId = value; }
        }
        /// <summary>
        /// 注释
        /// </summary>
        public string Descn
        {
            get { return _descn; }
            set { _descn = value; }
        }
        /// <summary>
        /// 扩展
        /// </summary>
        public string Attribute
        {
            get { return _attribute; }
            set { _attribute = value; }
        }
        /// <summary>
        /// 显示顺序

        /// </summary>
        public int? ViewOrd
        {
            get { return _viewOrd; }
            set { _viewOrd = value; }
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
        /// <summary>
        /// 修改人

        /// </summary>
        public string Creator
        {
            get { return _creator; }
            set { _creator = value; }
        }
        /// <summary>
        /// 修改人
        /// </summary>
        public string Modifitor
        {
            get { return _modifitor; }
            set { _modifitor = value; }
        }
        /// <summary>
        /// 状态(0:无效,1:有效)
        /// </summary>
        public int? Status
        {
            get { return _status; }
            set { _status = value; }
        }

        /// <summary>
        /// 父组ID
        /// </summary>
        public int? ParentID
        {
            get { return _parentId; }
            set { _parentId = value; }
        }
        /// <summary>
        /// 层级父路径ID
        /// </summary>
        public string ParentPath
        {
            get { return _parentPath; }
            set { _parentPath = value; }
        }
        /// <summary>
        /// 职务等级，数字越大，等级越大
        /// </summary>
        public int? PositionLevel
        {
            get { return _positionLevel; }
            set { _positionLevel = value; }
        }

        /// <summary>
        /// 上级岗位名称
        /// </summary>
        [Write(false)]
        public string ParentPositionName { get; set; }

        /// <summary>
        /// 上级岗位所属组织
        /// </summary>
        [Write(false)]
        public string ParentGroupName { get; set; }

        /// <summary>
        /// 所属部门
        /// </summary>
        [Write(false)]
        public string GroupName { get; set; }

        /// <summary>
        /// 所属部门代码
        /// </summary>
        [Write(false)]
        public string GroupCode { get; set; }
        #endregion

        #region 方法
        //获取对象hashcode
        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_positionID);
            sb.Append(_positionCode);
            sb.Append(_positionName);
            sb.Append(_groupId);
            sb.Append(_positionLevel);


            return sb.ToString().GetHashCode();
        }
        #endregion

        #region ISNode 成员
        [Write(false)]
        [Newtonsoft.Json.JsonIgnore]
        /// <summary>
        /// 项目ID
        /// </summary>
        public string Id
        {
            get
            {
                return _positionID.ToString();
            }
            set
            {
                _positionID = int.Parse(value);
            }
        }

        [Write(false)]
        [Newtonsoft.Json.JsonIgnore]
        /// <summary>
        /// 项目ID
        /// </summary>
        public string Name
        {
            get
            {
                return _positionName;
            }
            set
            {
                _positionName = value;
            }
        }

        [Write(false)]
        [Newtonsoft.Json.JsonIgnore]
        /// <summary>
        /// 菜单的父ID
        /// </summary>    
        public string ParentId
        {
            get
            {
                return _parentId.HasValue ? _parentId.ToString() : null;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _parentId = int.Parse(value);
                else
                    _parentId = null;
            }
        }

        [Write(false)]
        [Newtonsoft.Json.JsonIgnore]
        /// <summary>
        /// 显示顺序
        /// </summary>
        public int ViewOrder
        {
            get
            {
                if (_viewOrd.HasValue)
                    return _viewOrd.Value;
                else
                    return 0;
            }
            set
            {
                _viewOrd = value;
            }
        }

        #endregion

        #region IComparable<T> 成员

        public int CompareTo(Position other)
        {
            return this.ViewOrder - other.ViewOrder;
        }

        #endregion
    }
}
