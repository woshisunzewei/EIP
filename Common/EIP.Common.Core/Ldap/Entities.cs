using System;
using System.Collections.Generic;
using System.DirectoryServices;

namespace EIP.Common.Core.Ldap {

    /// <summary>
    /// 活动目录用户实体
    /// </summary>
    [DirectorySchema("user","cn")]
    public class DirectoryUser:DirectoryEntity {

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        public DirectoryUser() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="logonName">登录名</param>
        public DirectoryUser(string name, string logonName) {
            this.Name = name;
            this.LogonName = logonName;
        }

        #endregion

        #region Members
        private string _c;
        private string _city;
        private string _cn;
        private string _company;
        private string _country;
        private string _department;
        private string _description;
        private string _displayName;
        private string _duty;
        private string _fax;
        private string _firstName;
        private string _givenName;
        private string _homePage;
        private string _homePhone;
        private string _initials;
        private string _ipPhone;
        private string _mail;
        private string[] _manageObjects;
        private string _manager;
        private string[] _memberOf;
        private string _mobile;
        private string _note;
        private string _office;
        private string[] _otherFax;
        private string[] _otherHomePage;
        private string[] _otherHomePhone;
        private string[] _otherIpPhone;
        private string[] _otherMobile;
        private string[] _otherPager;
        private string[] _otherTelephone;
        private string _pager;
        private string _postalCode;
        private string _postOfficeBox;
        private string _province;
        private string _sAMAccountName;
        private string _streetAddress;
        private string _telephone;
        private string _userPrincipalName;
        private int _userAccountControl;
        private bool _msRTCSIP_UserEnabled;
        #endregion

        #region Properties

        /// <summary>
        /// Cn
        /// </summary>
        [DirectoryAttribute("cn")]
        public string Cn {
            get {
                return _cn;
            }
            set {
                if (_cn != value) {
                    OnPropertyChanging("Cn");
                    _cn = value;
                    OnPropertyChanged("Cn");
                }
            }
        }

        /// <summary>
        /// 姓
        /// </summary>
        [DirectoryAttribute("sn")]
        public string FirstName {
            get {
                return _firstName;
            }
            set {
                if (_firstName != value) {
                    OnPropertyChanging("FirstName");
                    _firstName = value;
                    OnPropertyChanged("FirstName");
                }
            }
        }

        /// <summary>
        /// CN
        /// </summary>
        [DirectoryAttribute("c")]
        public string C {
            get {
                return _c;
            }
            set {
                if (_c != value) {
                    OnPropertyChanging("CN");
                    _c = value;
                    OnPropertyChanged("CN");
                }
            }
        }

        /// <summary>
        /// 市/县
        /// </summary>
        [DirectoryAttribute("l")]
        public string City {
            get {
                return _city;
            }
            set {
                if (_city != value) {
                    OnPropertyChanging("City");
                    _city = value;
                    OnPropertyChanged("City");
                }
            }
        }

        /// <summary>
        /// 省/自治区
        /// </summary>
        [DirectoryAttribute("st")]
        public string Province {
            get {
                return _province;
            }
            set {
                if (_province != value) {
                    OnPropertyChanging("Province");
                    _province = value;
                    OnPropertyChanged("Province");
                }
            }
        }

        /// <summary>
        /// 职务
        /// </summary>
        [DirectoryAttribute("title")]
        public string Duty {
            get {
                return _duty;
            }
            set {
                if (_duty != value) {
                    OnPropertyChanging("Duty");
                    _duty = value;
                    OnPropertyChanged("Duty");
                }
            }
        }

        /// <summary>
        /// 描述
        /// </summary>
        [DirectoryAttribute("description")]
        public string Description {
            get {
                return _description;
            }
            set {
                if (_description != value) {
                    OnPropertyChanging("Description");
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        /// <summary>
        /// 邮政编码
        /// </summary
        [DirectoryAttribute("postalCode")]
        public string PostalCode {
            get {
                return _postalCode;
            }
            set {
                if (_postalCode != value) {
                    OnPropertyChanging("PostalCode");
                    _postalCode = value;
                    OnPropertyChanged("PostalCode");
                }
            }
        }

        /// <summary>
        /// 邮政信箱
        /// </summary>
        [DirectoryAttribute("postOfficeBox")]
        public string PostOfficeBox {
            get {
                return _postOfficeBox;
            }
            set {
                if (_postOfficeBox != value) {
                    OnPropertyChanging("PostOfficeBox");
                    _postOfficeBox = value;
                    OnPropertyChanged("PostOfficeBox");
                }
            }
        }

        /// <summary>
        /// 办公室
        /// </summary>
        [DirectoryAttribute("physicalDeliveryOfficeName")]
        public string Office {
            get {
                return _office;
            }
            set {
                if (_office != value) {
                    OnPropertyChanging("Office");
                    _office = value;
                    OnPropertyChanged("Office");
                }
            }
        }

        /// <summary>
        /// 电话号码
        /// </summary>
        [DirectoryAttribute("telephoneNumber")]
        public string Telephone {
            get {
                return _telephone;
            }
            set {
                if (_telephone != value) {
                    OnPropertyChanging("Telephone");
                    _telephone = value;
                    OnPropertyChanged("Telephone");
                }
            }
        }

        /// <summary>
        /// 传真
        /// </summary>
        [DirectoryAttribute("facsimileTelephoneNumber")]
        public string Fax {
            get {
                return _fax;
            }
            set {
                if (_fax != value) {
                    OnPropertyChanging("Fax");
                    _fax = value;
                    OnPropertyChanged("Fax");
                }
            }
        }

        /// <summary>
        /// 名
        /// </summary>
        [DirectoryAttribute("givenName")]
        public string GivenName {
            get {
                return _givenName;
            }
            set {
                if (_givenName != value) {
                    OnPropertyChanging("GivenName");
                    _givenName = value;
                    OnPropertyChanged("GivenName");
                }
            }
        }

        /// <summary>
        /// 英文缩写
        /// </summary>
        [DirectoryAttribute("initials")]
        public string Initials {
            get {
                return _initials;
            }
            set {
                if (_initials != value) {
                    OnPropertyChanging("Initials");
                    _initials = value;
                    OnPropertyChanged("Initials");
                }
            }
        }

        /// <summary>
        /// 显示名称
        /// </summary>
        [DirectoryAttribute("displayName")]
        public string DisplayName {
            get {
                return _displayName;
            }
            set {
                if (_displayName != value) {
                    OnPropertyChanging("DisplayName");
                    _displayName = value;
                    OnPropertyChanged("DisplayName");
                }
            }
        }

        /// <summary>
        /// 注释
        /// </summary>
        [DirectoryAttribute("info")]
        public string Note {
            get {
                return _note;
            }
            set {
                if (_note != value) {
                    OnPropertyChanging("Note");
                    _note = value;
                    OnPropertyChanged("Note");
                }
            }
        }

        /// <summary>
        /// 国家
        /// </summary>
        [DirectoryAttribute("co")]
        public string Country {
            get {
                return _country;
            }
            set {
                if (_country != value) {
                    OnPropertyChanging("Country");
                    _country = value;
                    OnPropertyChanged("Country");
                }
            }
        }

        /// <summary>
        /// 部门
        /// </summary>
        [DirectoryAttribute("department")]
        public string Department {
            get {
                return _department;
            }
            set {
                if (_department != value) {
                    OnPropertyChanging("Department");
                    _department = value;
                    OnPropertyChanged("Department");
                }
            }
        }

        /// <summary>
        /// 公司
        /// </summary>
        [DirectoryAttribute("company")]
        public string Company {
            get {
                return _company;
            }
            set {
                if (_company != value) {
                    OnPropertyChanging("Company");
                    _company = value;
                    OnPropertyChanged("Company");
                }
            }
        }

        /// <summary>
        /// 街道
        /// </summary>
        [DirectoryAttribute("streetAddress")]
        public string StreetAddress {
            get {
                return _streetAddress;
            }
            set {
                if (_streetAddress != value) {
                    OnPropertyChanging("StreetAddress");
                    _streetAddress = value;
                    OnPropertyChanged("StreetAddress");
                }
            }
        }

        /// <summary>
        /// 用户登陆名(Windows 2000)
        /// </summary>
        [DirectoryAttribute("sAMAccountName")]
        public string LogonName {
            get {
                return _sAMAccountName;
            }
            set {
                if (_sAMAccountName != value) {
                    OnPropertyChanging("LogonName");
                    _sAMAccountName = value;
                    OnPropertyChanged("LogonName");
                }
            }
        }

        /// <summary>
        /// 用户登陆名
        /// </summary>
        [DirectoryAttribute("userPrincipalName")]
        public string UserPrincipalName {
            get {
                return _userPrincipalName;
            }
            set {
                if (_userPrincipalName != value) {
                    OnPropertyChanging("UserPrincipalName");
                    _userPrincipalName = value;
                    OnPropertyChanged("UserPrincipalName");
                }
            }
        }

        /// <summary>
        /// 家庭电话
        /// </summary>
        [DirectoryAttribute("homePhone")]
        public string HomePhone {
            get {
                return _homePhone;
            }
            set {
                if (_homePhone != value) {
                    OnPropertyChanging("HomePhone");
                    _homePhone = value;
                    OnPropertyChanged("HomePhone");
                }
            }
        }

        /// <summary>
        /// 移动电话
        /// </summary>
        [DirectoryAttribute("mobile")]
        public string Mobile {
            get {
                return _mobile;
            }
            set {
                if (_mobile != value) {
                    OnPropertyChanging("Mobile");
                    _mobile = value;
                    OnPropertyChanged("Mobile");
                }
            }
        }

        /// <summary>
        /// 寻呼机
        /// </summary>
        [DirectoryAttribute("pager")]
        public string Pager {
            get {
                return _pager;
            }
            set {
                if (_pager != value) {
                    OnPropertyChanging("Pager");
                    _pager = value;
                    OnPropertyChanged("Pager");
                }
            }
        }

        /// <summary>
        /// IP电话
        /// </summary>
        [DirectoryAttribute("ipPhone")]
        public string IpPhone {
            get {
                return _ipPhone;
            }
            set {
                if (_ipPhone != value) {
                    OnPropertyChanging("IpPhone");
                    _ipPhone = value;
                    OnPropertyChanged("IpPhone");
                }
            }
        }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        [DirectoryAttribute("mail")]
        public string Mail {
            get {
                return _mail;
            }
            set {
                if (_mail != value) {
                    OnPropertyChanging("Mail");
                    _mail = value;
                    OnPropertyChanged("Mail");
                }
            }
        }

        /// <summary>
        /// 网页
        /// </summary>
        [DirectoryAttribute("wWWHomePage")]
        public string HomePage {
            get {
                return _homePage;
            }
            set {
                if (_homePage != value) {
                    OnPropertyChanging("HomePage");
                    _homePage = value;
                    OnPropertyChanged("HomePage");
                }
            }
        }

        /// <summary>
        /// 其他电话号码
        /// </summary>
        [DirectoryAttribute("otherTelephone")]
        public string[] OtherTelephone {
            get {
                return _otherTelephone;
            }
            set {
                if (_otherTelephone != value) {
                    OnPropertyChanging("OtherTelephone");
                    _otherTelephone = value;
                    OnPropertyChanged("OtherTelephone");
                }
            }
        }

        /// <summary>
        /// 其他寻呼机
        /// </summary>
        [DirectoryAttribute("otherPager")]
        public string[] OtherPager {
            get {
                return _otherPager;
            }
            set {
                if (_otherPager != value) {
                    OnPropertyChanging("OtherPager");
                    _otherPager = value;
                    OnPropertyChanged("OtherPager");
                }
            }
        }

        /// <summary>
        /// 其他家庭电话
        /// </summary>
        [DirectoryAttribute("otherHomePhone")]
        public string[] OtherHomePhone {
            get {
                return _otherHomePhone;
            }
            set {
                if (_otherHomePhone != value) {
                    OnPropertyChanging("OtherHomePhone");
                    _otherHomePhone = value;
                    OnPropertyChanged("OtherHomePhone");
                }
            }
        }

        /// <summary>
        /// 其他传真
        /// </summary>
        [DirectoryAttribute("otherFacsimileTelephoneNumber")]
        public string[] OtherFax {
            get {
                return _otherFax;
            }
            set {
                if (_otherFax != value) {
                    OnPropertyChanging("OtherFax");
                    _otherFax = value;
                    OnPropertyChanged("OtherFax");
                }
            }
        }

        /// <summary>
        /// 其他移动电话
        /// </summary>
        [DirectoryAttribute("otherMobile")]
        public string[] OtherMobile {
            get {
                return _otherMobile;
            }
            set {
                if (_otherMobile != value) {
                    OnPropertyChanging("OtherMobile");
                    _otherMobile = value;
                    OnPropertyChanged("OtherMobile");
                }
            }
        }

        /// <summary>
        /// 其他IP电话
        /// </summary>
        [DirectoryAttribute("otherIpPhone")]
        public string[] OtherIpPhone {
            get {
                return _otherIpPhone;
            }
            set {
                if (_otherIpPhone != value) {
                    OnPropertyChanging("OtherIpPhone");
                    _otherIpPhone = value;
                    OnPropertyChanged("OtherIpPhone");
                }
            }
        }

        /// <summary>
        /// 其他个人网页
        /// </summary>
        [DirectoryAttribute("url")]
        public string[] OtherHomePage {
            get {
                return _otherHomePage;
            }
            set {
                if (_otherHomePage != value) {
                    OnPropertyChanging("OtherHomePage");
                    _otherHomePage = value;
                    OnPropertyChanged("OtherHomePage");
                }
            }
        }

        /// <summary>
        /// 经理（DN）
        /// </summary>
        [DirectoryAttribute("manager")]
        public string Manager {
            get {
                return _manager;
            }
            set {
                if (_manager != value) {
                    OnPropertyChanging("Manager");
                    _manager = value;
                    OnPropertyChanged("Manager");
                }
            }
        }

        /// <summary>
        /// 隶属于（DNs）
        /// </summary>
        [DirectoryAttribute("memberOf")]
        public string[] MemberOf {
            get {
                return _memberOf;
            }
            set {
                if (_memberOf != value) {
                    OnPropertyChanging("MemberOf");
                    _memberOf = value;
                    OnPropertyChanged("MemberOf");
                }
            }
        }

        /// <summary>
        /// 所管理的用户组
        /// </summary>
        [DirectoryAttribute("managedObjects")]
        public string[] ManagedObjects {
            get {
                return _manageObjects;
            }
            set {
                if (_manageObjects != value) {
                    OnPropertyChanging("ManagedObjects");
                    _manageObjects = value;
                    OnPropertyChanged("ManagedObjects");
                }
            }
        }

        /// <summary>
        /// 用户状态代码
        /// </summary>
        [DirectoryAttribute("userAccountControl",false)]
        public int UserAccountControl {
            get {
                return _userAccountControl;
            }
            set {
                if (_userAccountControl != value) {
                    OnPropertyChanging("UserAccountControl");
                    _userAccountControl = value;
                    OnPropertyChanged("UserAccountControl");
                }
            }
        }


        [DirectoryAttribute("msRTCSIP-UserEnabled")]
        public bool isOcUser
        {
            get { return _msRTCSIP_UserEnabled; }
            set
            {
                if (_msRTCSIP_UserEnabled != value)
                {
                    OnPropertyChanging("isOcUser");
                    _msRTCSIP_UserEnabled = value;
                    OnPropertyChanged("isOcUser");
                }
            }
        }

        /// <summary>
        /// 用户启用状态
        /// </summary>
        public bool Enabled {
            get { return (_userAccountControl == 512 || _userAccountControl == 66048); }
        }


        #endregion

        #region Methods

        public bool SetPassword(string password) {
            return this.DirectoryEntry.Invoke("SetPassword", new object[] { password }) == null;
        }

        public bool ChangePassword(string oldPassword, string newPassword) {
            return this.DirectoryEntry.Invoke("ChangePassword", oldPassword, newPassword) == null;
        }

        public void Enable() {
            this.UserAccountControl = _userAccountControl & ~2;
        }

        public void Disable() {
            this.UserAccountControl = _userAccountControl | 2;
        }

        #endregion
    }

    /// <summary>
    /// 活动目录组实体
    /// </summary>
    [DirectorySchema("group", "CN")]
    public class DirectoryGroup : DirectoryEntity {

        #region Constructors

        public DirectoryGroup() { }

        #endregion

        #region Members

        private string _cn;
        private string _description;
        private string _mail;
        private string _managedBy;
        private string[] _member;
        private string[] _memberOf;
        private string _note;
        private string _sAMAccountName;

        #endregion

        #region Properties

        /// <summary>
        /// Cn
        /// </summary>
        [DirectoryAttribute("cn")]
        public string Cn {
            get {
                return _cn;
            }
            set {
                if (_cn != value) {
                    OnPropertyChanging("Cn");
                    _cn = value;
                    OnPropertyChanged("Cn");
                }
            }
        }

        /// <summary>
        /// 登录名(Windows 2000 以前版本)
        /// </summary>
        [DirectoryAttribute("sAMAccountName")]
        public string LogonName {
            get {
                return _sAMAccountName;
            }
            set {
                if (_sAMAccountName != value) {
                    OnPropertyChanging("LogonName");
                    _sAMAccountName = value;
                    OnPropertyChanged("LogonName");
                }
            }
        }


        /// <summary>
        /// 描述
        /// </summary>
        [DirectoryAttribute("description")]
        public String Description {
            get {
                return _description;
            }
            set {
                if (_description != value) {
                    OnPropertyChanging("Description");
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        [DirectoryAttribute("mail")]
        public string Mail {
            get {
                return _mail;
            }
            set {
                if (_mail != value) {
                    OnPropertyChanging("Mail");
                    _mail = value;
                    OnPropertyChanged("Mail");
                }
            }
        }

        /// <summary>
        /// 注释
        /// </summary>
        [DirectoryAttribute("info")]
        public string Note {
            get {
                return _note;
            }
            set {
                if (_note != value) {
                    OnPropertyChanging("Note");
                    _note = value;
                    OnPropertyChanged("Note");
                }
            }
        }

        /// <summary>
        /// 隶属于（DNs）
        /// </summary>
        [DirectoryAttribute("memberOf")]
        public string[] MemberOf {
            get {
                return _memberOf;
            }
            set {
                if (_memberOf != value) {
                    OnPropertyChanging("MemberOf");
                    _memberOf = value;
                    OnPropertyChanged("MemberOf");
                }
            }
        }

        /// <summary>
        /// 管理员（DN）
        /// </summary>
        [DirectoryAttribute("managedBy")]
        public string ManagedBy {
            get {
                return _managedBy;
            }
            set {
                if (_managedBy != value) {
                    OnPropertyChanging("ManagedBy");
                    _managedBy = value;
                    OnPropertyChanged("ManagedBy");
                }
            }
        }

        /// <summary>
        /// 成员(DN)
        /// </summary>
        [DirectoryAttribute("member")]
        public string[] Member {
            get {
                return _member;
            }
            set {
                if (_member != value) {
                    OnPropertyChanging("Member");
                    _member = value;
                    OnPropertyChanged("Member");
                }
            }
        }

        #endregion

    }

    /// <summary>
    /// 活动目录组织单位实体
    /// </summary>
    [DirectorySchema("organizationalUnit", "OU")]
    public class DirectoryOrganizationalUnit : DirectoryEntity {

        #region Constructors

        public DirectoryOrganizationalUnit() { }
        public DirectoryOrganizationalUnit(string name) { this.Name = name; }

        #endregion

        #region Members

        private string _c;
        private string _city;
        private string _country;
        private string _description;
        private string _managedBy;
        private string _postalCode;
        private string _province;
        private string _streetAddress;
        private string _cn;

        #endregion

        #region Properties

        /// <summary>
        /// C
        /// </summary>
        [DirectoryAttribute("c")]
        public string C {
            get {
                return _c;
            }
            set {
                if (_c != value) {
                    OnPropertyChanging("C");
                    _c = value;
                    OnPropertyChanged("C");
                }
            }
        }

        /// <summary>
        /// 城市
        /// </summary>
        [DirectoryAttribute("l")]
        public string City {
            get {
                return _city;
            }
            set {
                if (_city != value) {
                    OnPropertyChanging("City");
                    _city = value;
                    OnPropertyChanged("City");
                }
            }
        }

        /// <summary>
        /// 省/自治区
        /// </summary>
        [DirectoryAttribute("st")]
        public string Province {
            get {
                return _province;
            }
            set {
                if (_province != value) {
                    OnPropertyChanging("Province");
                    _province = value;
                    OnPropertyChanged("Province");
                }
            }
        }

        /// <summary>
        /// 街道地址
        /// </summary>
        [DirectoryAttribute("street")]
        public string StreetAddress {
            get {
                return _streetAddress;
            }
            set {
                if (_streetAddress != value) {
                    OnPropertyChanging("StreetAddress");
                    _streetAddress = value;
                    OnPropertyChanged("StreetAddress");
                }
            }
        }

        /// <summary>
        /// 描述
        /// </summary>
        [DirectoryAttribute("description")]
        public string Description {
            get {
                return _description;
            }
            set {
                if (_description != value) {
                    OnPropertyChanging("Description");
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        /// <summary>
        /// 邮政编码
        /// </summary>
        [DirectoryAttribute("postalCode")]
        public string PostalCode {
            get {
                return _postalCode;
            }
            set {
                if (_postalCode != value) {
                    OnPropertyChanging("PostalCode");
                    _postalCode = value;
                    OnPropertyChanged("PostalCode");
                }
            }
        }

        /// <summary>
        /// 国家
        /// </summary>
        [DirectoryAttribute("co")]
        public string Country {
            get {
                return _country;
            }
            set {
                if (_country != value) {
                    OnPropertyChanging("Country");
                    _country = value;
                    OnPropertyChanged("Country");
                }
            }
        }
        [DirectoryAttribute("cn")]
        public string CN
        {
            get
            {
                return _cn; ;
            }
            set
            {
                if (_cn != value)
                {
                    OnPropertyChanging("CN");
                    _cn = value;
                    OnPropertyChanged("CN");
                }
            }
        }

        /// <summary>
        /// 管理员（DN）
        /// </summary>
        [DirectoryAttribute("managedBy")]
        public string ManagedBy {
            get {
                return _managedBy;
            }
            set {
                if (_managedBy != value) {
                    OnPropertyChanging("ManagedBy");
                    _managedBy = value;
                    OnPropertyChanged("ManagedBy");
                }
            }
        }

        #endregion

        #region Methods

        public List<T> GetEntities<T>() where T : IDirectoryEntity {
            return DirectoryContext.GetEntities<T>(this.DirectoryEntry, SearchScope.OneLevel);
        }

        /// <summary>
        /// 用户列表
        /// </summary>
        public List<DirectoryUser> GetUsers() {
            return this.GetEntities<DirectoryUser>();
        }

        /// <summary>
        /// 组列表
        /// </summary>
        public List<DirectoryGroup> GetGroups() {
            return this.GetEntities<DirectoryGroup>();
        }

        /// <summary>
        /// 组织单位列表
        /// </summary>
        public List<DirectoryOrganizationalUnit> GetOrganizationalUnits() {
            return this.GetEntities<DirectoryOrganizationalUnit>();
        }

        /// <summary>
        /// 计算机列表
        /// </summary>
        public List<DirectoryComputer> GetComputers() {
            return this.GetEntities<DirectoryComputer>();
        }


        #endregion
    }

    /// <summary>
    /// 活动目录计算机实体
    /// </summary>
    [DirectorySchema("computer", "cn")]
    public class DirectoryComputer : DirectoryEntity {

        #region Constructors

        public DirectoryComputer() { }

        #endregion

        #region Members

        private string _cn;
        private string _description;
        private string _dNSHostName;
        private int _logonCount;
        private string _operatingSystem;
        private string _operatingSystemServicePack;
        private string _operatingSystemVersion;
        private string _sAMAccountName;


        #endregion

        #region Properties

        /// <summary>
        /// Cn
        /// </summary>
        [DirectoryAttribute("cn")]
        public string Cn {
            get {
                return _cn;
            }
            set {
                if (_cn != value) {
                    OnPropertyChanging("Cn");
                    _cn = value;
                    OnPropertyChanged("Cn");
                }
            }
        }

        /// <summary>
        /// 登录名(Windows 2000 以前版本)
        /// </summary>
        [DirectoryAttribute("sAMAccountName")]
        public string LogonName {
            get {
                return _sAMAccountName;
            }
            set {
                if (_sAMAccountName != value) {
                    OnPropertyChanging("LogonName");
                    _sAMAccountName = value;
                    OnPropertyChanged("LogonName");
                }
            }
        }


        /// <summary>
        /// 描述
        /// </summary>
        [DirectoryAttribute("description")]
        public string Description {
            get {
                return _description;
            }
            set {
                if (_description != value) {
                    OnPropertyChanging("Description");
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        /// <summary>
        /// 登录次数
        /// </summary>
        [DirectoryAttribute("logonCount")]
        public int LogonCount {
            get {
                return _logonCount;
            }
            set {
                if (_logonCount != value) {
                    OnPropertyChanging("LogonCount");
                    _logonCount = value;
                    OnPropertyChanged("LogonCount");
                }
            }
        }

        /// <summary>
        /// 操作系统
        /// </summary>
        [DirectoryAttribute("operatingSystem")]
        public string OperatingSystem {
            get {
                return _operatingSystem;
            }
            set {
                if (_operatingSystem != value) {
                    OnPropertyChanging("OperatingSystem");
                    _operatingSystem = value;
                    OnPropertyChanged("OperatingSystem");
                }
            }
        }

        /// <summary>
        /// 操作系统版本
        /// </summary>
        [DirectoryAttribute("operatingSystemVersion")]
        public string OperatingSystemVersion {
            get {
                return _operatingSystemVersion;
            }
            set {
                if (_operatingSystemVersion != value) {
                    OnPropertyChanging("OperatingSystemVersion");
                    _operatingSystemVersion = value;
                    OnPropertyChanged("OperatingSystemVersion");
                }
            }
        }

        /// <summary>
        /// 操作系统补丁
        /// </summary>
        [DirectoryAttribute("operatingSystemServicePack")]
        public string OperatingSystemServicePack {
            get {
                return _operatingSystemServicePack;
            }
            set {
                if (_operatingSystemServicePack != value) {
                    OnPropertyChanging("OperatingSystemServicePack");
                    _operatingSystemServicePack = value;
                    OnPropertyChanged("OperatingSystemServicePack");
                }
            }
        }

        /// <summary>
        /// Dns主机名
        /// </summary>
        [DirectoryAttribute("dNSHostName")]
        public string DNSHostName {
            get {
                return _dNSHostName;
            }
            set {
                if (_dNSHostName != value) {
                    OnPropertyChanging("DNSHostName");
                    _dNSHostName = value;
                    OnPropertyChanged("DNSHostName");
                }
            }
        }

        #endregion

    }

    /// <summary>
    /// Builtin
    /// </summary>
    [DirectorySchema("builtinDomain","cn")]
    public class DirectoryBuiltin : DirectoryEntity {

        #region Members & Properties

        private System.String _cn;
        /// <summary>
        /// CN
        /// </summary>
        [DirectoryAttribute("cn")]
        public System.String Cn {
            get { return _cn; }
            internal set {
                if (_cn != value) {
                    OnPropertyChanging("Cn");
                    _cn = value;
                    OnPropertyChanged("Cn");
                }
            }
        }

        private System.String _objectCategory;
        /// <summary>
        /// ObjectCategory
        /// </summary>
        [DirectoryAttribute("objectCategory")]
        public System.String objectCategory {
            get { return _objectCategory; }
            internal set {
                if (objectCategory != value) {
                    _objectCategory = value;
                    OnPropertyChanged("objectCategory");
                }
            }
        }

        private System.Boolean _showInAdvancedViewOnly;
        /// <summary>
        /// ShowInAdvancedViewOnly
        /// </summary>
        [DirectoryAttribute("showInAdvancedViewOnly")]
        public System.Boolean ShowInAdvancedViewOnly {
            get { return _showInAdvancedViewOnly; }
            internal set {
                if (_showInAdvancedViewOnly != value) {
                    OnPropertyChanging("ShowInAdvancedViewOnly");
                    _showInAdvancedViewOnly = value;
                    OnPropertyChanged("ShowInAdvancedViewOnly");
                }
            }
        }

        private System.Boolean _isCriticalSystemObject;
        /// <summary>
        /// IsCriticalSystemObject
        /// </summary>
        [DirectoryAttribute("isCriticalSystemObject")]
        public System.Boolean IsCriticalSystemObject {
            get { return _isCriticalSystemObject; }
            internal set {
                if (_isCriticalSystemObject != value) {
                    OnPropertyChanging("IsCriticalSystemObject");
                    _isCriticalSystemObject = value;
                    OnPropertyChanged("IsCriticalSystemObject");
                }
            }
        }

        private System.Int32 _systemFlags;
        /// <summary>
        /// systemFlags
        /// </summary>
        [DirectoryAttribute("systemFlags")]
        public System.Int32 SystemFlags {
            get { return _systemFlags; }
            internal set {
                if (_systemFlags != value) {
                    OnPropertyChanging("SystemFlags");
                    _systemFlags = value;
                    OnPropertyChanged("SystemFlags");
                }
            }
        }

        private System.Int32 _lockoutThreshold;
        /// <summary>
        /// lockoutThreshold
        /// </summary>
        [DirectoryAttribute("lockoutThreshold")]
        public System.Int32 LockoutThreshold {
            get { return _lockoutThreshold; }
            internal set {
                if (_lockoutThreshold != value) {
                    OnPropertyChanging("LockoutThreshold");
                    _lockoutThreshold = value;
                    OnPropertyChanged("LockoutThreshold");
                }
            }
        }

        private System.Int32 _minPwdLength;
        /// <summary>
        /// 最小密码长度
        /// </summary>
        [DirectoryAttribute("minPwdLength")]
        public System.Int32 MinPwdLength {
            get { return _minPwdLength; }
            internal set {
                if (_minPwdLength != value) {
                    OnPropertyChanging("MinPwdLength");
                    _minPwdLength = value;
                    OnPropertyChanged("MinPwdLength");
                }
            }
        }

        private System.Int32 _nextRid;
        /// <summary>
        /// nextRid
        /// </summary>
        [DirectoryAttribute("nextRid")]
        public System.Int32 NextRid {
            get { return _nextRid; }
            internal set {
                if (_nextRid != value) {
                    OnPropertyChanging("NextRid");
                    _nextRid = value;
                    OnPropertyChanged("NextRid");
                }
            }
        }

        private System.Int32 _pwdProperties;
        /// <summary>
        /// pwdProperties
        /// </summary>
        [DirectoryAttribute("pwdProperties")]
        public System.Int32 PwdProperties {
            get { return _pwdProperties; }
            internal set {
                if (_pwdProperties != value) {
                    OnPropertyChanging("PwdProperties");
                    _pwdProperties = value;
                    OnPropertyChanged("PwdProperties");
                }
            }
        }

        private System.Int32 _pwdHistoryLength;
        /// <summary>
        /// pwdHistoryLength
        /// </summary>
        [DirectoryAttribute("pwdHistoryLength")]
        public System.Int32 PwdHistoryLength {
            get { return _pwdHistoryLength; }
            internal set {
                if (_pwdHistoryLength != value) {
                    OnPropertyChanging("PwdHistoryLength");
                    _pwdHistoryLength = value;
                    OnPropertyChanged("PwdHistoryLength");
                }
            }
        }

        private System.Int32 _uASCompat;
        /// <summary>
        /// uASCompat
        /// </summary>
        [DirectoryAttribute("uASCompat")]
        public System.Int32 UASCompat {
            get { return _uASCompat; }
            internal set {
                if (_uASCompat != value) {
                    OnPropertyChanging("UASCompat");
                    _uASCompat = value;
                    OnPropertyChanged("UASCompat");
                }
            }
        }


        private System.String _description;
        /// <summary>
        /// 描述
        /// </summary>
        [DirectoryAttribute("description")]
        public System.String Description {
            get { return _description; }
            set {
                if (_description != value) {
                    OnPropertyChanging("Description");
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// Container
    /// </summary>
    [DirectorySchema("container")]
    public class DirectoryContainer : DirectoryEntity {

        #region Members & Properties

        private System.String _cn;
        /// <summary>
        /// CN
        /// </summary>
        [DirectoryAttribute("cn")]
        public System.String Cn {
            get { return _cn; }
            internal set {
                if (_cn != value) {
                    OnPropertyChanging("Cn");
                    _cn = value;
                    OnPropertyChanged("Cn");
                }
            }
        }

        private System.Boolean _showInAdvancedViewOnly;
        /// <summary>
        /// ShowInAdvancedViewOnly
        /// </summary>
        [DirectoryAttribute("showInAdvancedViewOnly")]
        public System.Boolean ShowInAdvancedViewOnly {
            get { return _showInAdvancedViewOnly; }
            set {
                if (_showInAdvancedViewOnly != value) {
                    OnPropertyChanging("ShowInAdvancedViewOnly");
                    _showInAdvancedViewOnly = value;
                    OnPropertyChanged("ShowInAdvancedViewOnly");
                }
            }
        }

        private System.Boolean _isCriticalSystemObject;
        /// <summary>
        /// IsCriticalSystemObject
        /// </summary>
        [DirectoryAttribute("isCriticalSystemObject")]
        public System.Boolean IsCriticalSystemObject {
            get { return _isCriticalSystemObject; }
            internal set {
                if (_isCriticalSystemObject != value) {
                    OnPropertyChanging("IsCriticalSystemObject");
                    _isCriticalSystemObject = value;
                    OnPropertyChanged("IsCriticalSystemObject");
                }
            }
        }

        private System.Int32 _systemFlags;
        /// <summary>
        /// systemFlags
        /// </summary>
        [DirectoryAttribute("systemFlags")]
        public System.Int32 SystemFlags {
            get { return _systemFlags; }
            internal set {
                if (_systemFlags != value) {
                    OnPropertyChanging("SystemFlags");
                    _systemFlags = value;
                    OnPropertyChanged("SystemFlags");
                }
            }
        }

        private System.String _description;
        /// <summary>
        /// 描述
        /// </summary>
        [DirectoryAttribute("description")]
        public System.String Description {
            get { return _description; }
            set {
                if (_description != value) {
                    OnPropertyChanging("Description");
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        #endregion

    }
}
