using System;
using System.ComponentModel;
using System.DirectoryServices;

namespace EIP.Common.Core.Ldap {

    /// <summary>
    /// 活动目录实体接口
    /// </summary>
    public interface IDirectoryEntity {
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// LDAP地址
        /// </summary>
        string Path { get; set; }
        /// <summary>
        /// GUID值
        /// </summary>
        Guid Guid { get; set; }
        /// <summary>
        /// SID值
        /// </summary>
        byte[] Sid { get; set; }
        /// <summary>
        /// DN值
        /// </summary>
        string DistinguishedName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime WhenCreated { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        DateTime WhenChanged { get; set; }
    }

    /// <summary>
    /// 活动目录基本实体
    /// 提供基本的通用的活动目录实体方法，属性以及事件
    /// </summary>
    [Serializable]
    public class DirectoryEntity :IDirectoryEntity, INotifyPropertyChanged,IDisposable {

        #region Constitutions

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public DirectoryEntity(){

        }

        #endregion

        #region Events

        /// <summary>
        /// 属性值改变事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 属性值即将改变事件
        /// </summary>
        //public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        /// 触发属性值改变事件
        /// </summary>
        /// <param name="propertyName">值发生改变的属性名</param>
        protected void OnPropertyChanged(string propertyName) {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 触发属性值即将改变事件
        /// </summary>
        /// <param name="propertyName">值即将发生改变的属性名</param>
        protected void OnPropertyChanging(string propertyName) {
            //if (PropertyChanging != null)
            //    PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
        }

        #endregion

        #region Members

        private string _name;
        private string _path;
        private string _distinguishedName;
        private Guid _guid;
        private byte[] _sid;
        private DateTime _whenCreated;
        private DateTime _whenChanged;
        private string _parentPath;
        private Guid _parentGuid;

        #endregion

        #region Properties

        /// <summary>
        /// 活动目录条目
        /// </summary>
        public DirectoryEntry DirectoryEntry { get; internal set; }


        /// <summary>
        /// 获得或设置活动目录实体路径
        /// </summary>
        public string Path {
            get { return _path; }
            set { _path = value; }
        }


        /// <summary>
        /// 获得活动目录实体名称
        /// </summary>
        [DirectoryAttribute("name",true)]
        public string Name {
            get { return _name ;}
            set{
                if (_name != value) {
                    OnPropertyChanging("Name");
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        /// <summary>
        /// 获得活动目录实体Guid值
        /// </summary>
        [DirectoryAttribute("objectGuid",true)]
        public Guid Guid {
            get { return _guid; }
            set {
                if (_guid != value)
                    _guid = value;
            }
        }

        /// <summary>
        /// 获得活动目录实体Sid值
        /// </summary>
        [DirectoryAttribute("objectSid", true)]
        public byte[] Sid {
            get { return _sid; }
            set { _sid = value; }
        }

        /// <summary>
        /// 获得活动目录实体的父实体LDAP
        /// </summary>
        public string ParentPath {
            get { return _parentPath; }
            set { _parentPath = value; }
        }

        /// <summary>
        /// 获得活动目录实体的父实体LDAP
        /// </summary>
        public Guid ParentGuid {
            get { return _parentGuid; }
            set { _parentGuid = value; }
        }

        /// <summary>
        /// 获得活动目录实体的DN值
        /// </summary>
        [DirectoryAttribute("distinguishedName",true)]
        public string DistinguishedName {
            get {
                return _distinguishedName;
            }
            set {
                if (_distinguishedName != value)
                    _distinguishedName = value;
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DirectoryAttribute("whenCreated",true)]
        public DateTime WhenCreated {
            get {
                return _whenCreated;
            }
            set {
                if (_whenCreated != value)
                    _whenCreated = value;
            }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        [DirectoryAttribute("whenChanged",true)]
        public DateTime WhenChanged {
            get {
                return _whenChanged;
            }
            set {
                if (_whenChanged != value)
                    _whenChanged = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 移动实体到指定实体下
        /// </summary>
        /// <param name="entity"></param>
        public void MoveTo(DirectoryEntity entity) {
            this.DirectoryEntry.MoveTo(entity.DirectoryEntry);
            this._path = this.DirectoryEntry.Path;
            this._distinguishedName = this.DirectoryEntry.Properties["distinguishedName"][0].ToString();
            this._whenChanged = DateTime.Parse(this.DirectoryEntry.Properties["whenChanged"][0].ToString());
        }

        /// <summary>
        /// 重命名实体
        /// </summary>
        /// <param name="newName"></param>
        public void Rename(string newName) {
            string schema = DirectoryContext.GetEntitySchemaClassType(this.GetType());
            this.DirectoryEntry.Rename(schema + "=" + newName);
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// 释放实体
        /// </summary>
        public void Dispose() {
            if (this.DirectoryEntry != null)
                this.DirectoryEntry.Close();
        }

        #endregion
    }
}