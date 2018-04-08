namespace UkeyTech.WebFW.Model
{
    using System;
    using System.Data;
    using Clover.Core.Domain;
    using Newtonsoft.Json;
    using Dapper.Contrib.Extensions;
using System.Collections.Generic;
    /// <summary>
    /// Admin 数据模型层
    /// Descn:
    /// Creator:laozijian
    /// AddDate:2012-02-08
    /// Creator:laozijian
    /// UpdateTime:2012-02-08    
    /// </summary>
    [Serializable]
    public class Admin : IEntity, IAccount
    {

        #region 私有属性
        /// <summary>
        /// (主键)
        /// </summary>
        private string _adminId;
        /// <summary>
        /// 用户名
        /// </summary>
        private string _adminName;
        /// <summary>
        /// 登录名
        /// </summary>
        private string _loginName;
        /// <summary>
        /// 密码
        /// </summary>
        private string _password;
        /// <summary>
        /// 邮件地址
        /// </summary>
        private string _email;
        /// <summary>
        /// 加入时间
        /// </summary>
        private DateTime _joined;
        /// <summary>
        /// 最后访问时间
        /// </summary>
        private DateTime _lastVisit;
        /// <summary>
        /// 注册ip
        /// </summary>
        private string _iP;
        /// <summary>
        /// 语言
        /// </summary>
        private string _languageFile;
        /// <summary>
        /// 用户状态(-1:禁用  0:可用)
        /// </summary>
        private int _status = 1;
        /// <summary>
        /// 
        /// </summary>
        private bool? _isActived;
        /// <summary>
        /// 
        /// </summary>
        private string _descn;

        /// <summary>
        /// 邮箱登陆密码
        /// </summary>
        private string _emailPwd;

     
        #endregion

        public Admin()
        {
        }

        #region 公有属性

        /// <summary>
        /// (主键)
        /// </summary>
        public string AdminId
        {
            get { return _adminId; }
            set { _adminId = value; }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string AdminName
        {
            get { return _adminName; }
            set { _adminName = value; }
        }
        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName
        {
            get { return _loginName; }
            set { _loginName = value; }
        }

        
        /// <summary>
        /// 密码
        /// </summary>
        [JsonIgnore]
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        /// <summary>
        /// 邮件地址
        /// </summary>
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        
        /// <summary>
        /// 加入时间
        /// </summary>
        [JsonIgnore]
        public DateTime Joined
        {
            get { return _joined; }
            set { _joined = value; }
        }

        
        /// <summary>
        /// 最后访问时间
        /// </summary>
        [JsonIgnore]
        public DateTime LastVisit
        {
            get { return _lastVisit; }
            set { _lastVisit = value; }
        }

        
        /// <summary>
        /// 注册ip
        /// </summary>
        [JsonIgnore]
        public string IP
        {
            get { return _iP; }
            set { _iP = value; }
        }

        
        /// <summary>
        /// 语言
        /// </summary>
        [JsonIgnore]
        public string LanguageFile
        {
            get { return _languageFile; }
            set { _languageFile = value; }
        }

        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }

        
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public bool? IsActived
        {
            get { return _isActived; }
            set { _isActived = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public string Descn
        {
            get { return _descn; }
            set { _descn = value; }
        }

        
        /// <summary>
        /// 所属组织
        /// </summary>
        public string GroupName { get; set; }

        
        /// <summary>
        /// 所属组织ID
        /// </summary>
        [JsonIgnore]
        public int? GroupID { get; set; }

        /// <summary>
        /// 所属组织(复数)
        /// </summary>
        [Write(false)]
        public string GroupNames { get; set; }

        /// <summary>
        /// 所属岗位(复数)
        /// </summary>
        [Write(false)]
        public string PositionNames { get; set; }

        /// <summary>
        /// 当前部门ID
        /// </summary>
        [Write(false)]
        public string CurrGroupId { get; set; }

        /// <summary>
        /// 当前岗位
        /// </summary>
        [Write(false)]
        public string CurrPositionId { get; set; }

        /// <summary>
        /// 当前岗位名称
        /// </summary>
        [Write(false)]
        public string CurrGroupName { get; set; }

        /// <summary>
        /// 当前岗位名称
        /// </summary>
        [Write(false)]
        public string CurrPositionName { get; set; }

        /// <summary>
        /// 使用部门
        /// </summary>
        [JsonIgnore]
        public string UsedDeptCode { get; set; }
        
        /// <summary>
        /// 使用部门名称
        /// </summary>
        [JsonIgnore]
        public string UsedDeptName { get; set; }
        
        /// <summary>
        /// 预算部门
        /// </summary>
        [JsonIgnore]
        public string BudgetDeptCode { get; set; }
        
        /// <summary>
        /// 预算部门名称
        /// </summary>
        [JsonIgnore]
        public string BudgetDeptName { get; set; }
        /// <summary>
        /// 员工类别
        /// </summary>
        [JsonIgnore]
        public string EmpType
        {
            get;
            set;
        }

        /// <summary>
        /// 员工类别名称
        /// </summary>
        [JsonIgnore]
        public string EmpTypeName
        {
            get;
            set;
        }
        /// <summary>
        /// 员工电话
        /// </summary>
        [JsonIgnore]
        public string MobilePhone { get; set; }
        /// <summary>
        /// 所属岗位
        /// </summary>
        public string Postion { get; set; }
        /// <summary>
        /// 所属岗位
        /// </summary>
        public string DeptCode { get; set; }

        /// <summary>
        /// 国籍
        /// </summary>
        public string Nation { get; set; }
        /// <summary>
        /// 邮箱密码
        /// </summary>
        [JsonIgnore]
        public string EmailPwd
        {
            get { return _emailPwd; }
            set { _emailPwd = value; }
        }

        List<string> accountlist = new List<string>(); 
        string _MappingAccount;
        /// <summary>
        /// 映射账号信息（域行号或第三方账号,可用,号分隔）
        /// </summary>       
        public string MappingAccount { 
            get{
            return _MappingAccount;
            } 
            set{
                _MappingAccount = value;
                //if(!string.IsNullOrEmpty(_MappingAccount)){
                //    accountlist.Clear();
                //    accountlist.AddRange(_MappingAccount.Split(new char[]{','},  StringSplitOptions.RemoveEmptyEntries));
                //}
            }
        }
        #endregion

        #region 方法
        //获取对象hashcode
        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(_adminId);
            sb.Append(_loginName);
            sb.Append(_email);

            return sb.ToString().GetHashCode();
        }
        /*
        /// <summary>
        /// 检查是否存在相应的映射账号
        /// </summary>
        /// <param name="account">映射账号</param>
        /// <returns></returns>
        public bool CheckExistsMappingAccount(string account) {
            return accountlist.Count > 0 && accountlist.Contains(account);
        }
        */
        #endregion

        #region IAccount 成员


        public string UniqueId
        {
            get
            {
                return _adminId;
            }
            set
            {

                _adminId = value;
            }
        }

        [JsonIgnore]
        public string AccountCode
        {
            get
            {
                return LoginName;
            }
            set
            {
                LoginName = value;
            }
        }

        [JsonIgnore]
        public string UserName
        {
            get
            {
                return _adminName;
            }
            set
            {
                _adminName = value;
            }
        }

        string _permission;
        [JsonIgnore]
        public string Permission
        {
            get
            {
                return _permission;
            }
            set
            {
                _permission = value;
            }
        }


        /// <summary>
        /// 当前角色名称
        /// </summary>
        [JsonIgnore]
        [Write(false)]
        public string CurrRoleName
        {
            get;
            set;
        }

        /// <summary>
        /// 当前角色ID
        /// </summary>
        [JsonIgnore]
        [Write(false)]
        public string CurrRoleId
        {
            get;
            set;
        }

        /// <summary>
        /// 拥有的角色
        /// </summary>
        [JsonIgnore]
        [Write(false)]
        public IRole[] Roles
        {
            get;
            set;
        }

        /// <summary>
        /// 具有的组织ID
        /// </summary>
        [JsonIgnore]
        [Write(false)]
        public IList<string> GroupIds
        {
            get;
            set;
        }

        /// <summary>
        /// 具有的组织岗位
        /// </summary>
        [JsonIgnore]
        [Write(false)]
        public IList<IGroupPosition> GroupPositions
        {
            get
            {
                return null;
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        /// <summary>
        /// 密码修改时间
        /// </summary>
        public DateTime? PasswordLastUpdateTime { get; set; }
    }
}