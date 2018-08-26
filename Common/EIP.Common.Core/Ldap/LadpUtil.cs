using System;
using System.DirectoryServices;

namespace EIP.Common.Core.Ldap
{
    /// <summary>
    /// Ladp帮助类
    /// </summary>
    public class LadpUtil
    {
        #region 属性
        /// <summary>
        /// AD的LDAP地址
        /// 如:
        /// </summary>
        private string _ldapPath;

        public string LdapPath
        {
            get { return _ldapPath; }
            set { _ldapPath = value; }
        }
        /// <summary>
        /// AD的域名
        /// 如:
        /// </summary>
        private string _domain;

        public string Domain
        {
            get { return _domain; }
            set { _domain = value; }
        }
        /// <summary>
        /// AD成员账户用户名
        /// 如:
        /// </summary>
        private string _adminName;

        public string AdminName
        {
            get { return _adminName; }
            set { _adminName = value; }
        }
        /// <summary>
        /// AD成员账户密码
        /// 如:
        /// </summary>
        private string _adminPassword;

        public string AdminPassword
        {
            get { return _adminPassword; }
            set { _adminPassword = value; }
        }
        #endregion

        #region 方法

        /// <summary>
        /// 作用：获得管理员的DirectoryEntry
        /// </summary>
        /// <returns>返回管理员的DirectoryEntry</returns>
        public DirectoryEntry GetDirectory()
        {
            DirectoryEntry de = null;
            try
            {
                de = new DirectoryEntry("LDAP://" + LdapPath + "/" + Domain + "");
                de.Username = @"" + AdminName + "";
                de.Password = @"" + AdminPassword + "";
                de.AuthenticationType = AuthenticationTypes.Secure;
                de.RefreshCache();
            }
            catch
            {
                de = null;
            }
            return de;
        }
        /// <summary>
        /// 作用：指定用户验证登录情况
        /// </summary>
        /// <returns>返回bool,true:登录成功。false:登录失败</returns>
        public bool CheckLogin()
        {
            bool isLogin = false;
            try
            {
                DirectoryEntry de = new DirectoryEntry("LDAP://" + LdapPath + "/" + Domain + "");
                de.Username = @"" + AdminName + "";
                de.Password = @"" + AdminPassword + "";
                de.AuthenticationType = AuthenticationTypes.Secure;
                de.RefreshCache();
                isLogin = true;
            }
            catch
            {
                isLogin = false;
            }
            return isLogin;
        }
        /// <summary>
        /// 作用：指定用户验证登录情况
        /// </summary>
        /// <param name="userName">指定的用户名</param>
        /// <param name="userPwd">指定的密码</param>
        /// <returns></returns>
        public bool CheckLogin(string userName, string userPwd)
        {
            bool isLogin = false;
            try
            {
                DirectoryEntry de = new DirectoryEntry("LDAP://" + LdapPath + "/" + Domain + "");
                de.Username = @"" + userName + "";
                de.Password = @"" + userPwd + "";
                de.AuthenticationType = AuthenticationTypes.Secure;
                de.RefreshCache();
                isLogin = true;
            }
            catch
            {
                isLogin = false;
            }
            return isLogin;
        }

        /// <summary>
        /// 作用：查询是否有指定路径下的OU存在
        /// </summary>
        /// <param name="ouname">被查询的OU名称:OU=123[123隶属于xx]</param>
        /// <returns></returns>
        public bool SelectOU(string ouname)
        {
            bool res = false;
            DirectoryEntry de = null;
            DirectoryEntry de_child = null;
            try
            {
                de = GetDirectory();
                string path = ouname;
                de_child = de.Children.Find(path, "organizationalUnit");
                if (de_child != null)
                {
                    res = true;
                }
                else
                {
                    res = false;
                }
                de_child.Close();
                de.Close();
            }
            catch (Exception)
            {
                res = false;
            }
            return res;
        }
        /// <summary>
        /// 作用：查询是否有指定路径下的OU存在
        /// </summary>
        /// <param name="parentPath">指定OU的上级路径:OU=xx,OU=虹信,OU=长虹</param>
        /// <param name="ouname">被查询的OU名称:OU=123[123隶属于xx]</param>
        /// <returns></returns>
        public bool SelectOU(string parentPath, string ouname)
        {
            bool res = false;
            DirectoryEntry de = null;
            DirectoryEntry de_child = null;
            try
            {
                de = GetDirectory();
                string path = ouname + "," + parentPath;
                de_child = de.Children.Find(path, "organizationalUnit");
                if (de_child != null)
                {
                    res = true;
                }
                else
                {
                    res = false;
                }
                de_child.Close();
                de.Close();
            }
            catch (Exception)
            {
                res = false;
            }
            return res;
        }
        /// <summary>
        /// 作用：创建OU
        /// 备注：路径需从子级到父级,如:OU=B,OU=A[其组织结构B是]
        /// </summary>
        /// <param name="newOuName">将OU新建的路径位置</param>
        public bool CreateOU(string newOuName)
        {
            bool res = false;
            DirectoryEntry de = null;
            DirectoryEntry ou = null;
            try
            {
                de = GetDirectory();
                string newOupath = newOuName;
                ou = de.Children.Add(newOupath, "organizationalUnit");
                ou.CommitChanges();
                res = true;
                ou.Close();
                de.Close();
            }
            catch (Exception)
            {
                res = false;
            }
            return res;
        }

        /// <summary>
        /// 作用：创建OU
        /// 备注：路径需从子级到父级,如:OU=B,OU=A[其组织结构B是]
        /// </summary>
        /// <param name="newOuName"></param>
        /// <param name="ouPath">将OU新建的路径位置</param>
        public bool CreateOU(string newOuName, string ouPath)
        {
            bool res = false;
            DirectoryEntry de = null;
            DirectoryEntry ou = null;
            try
            {
                de = GetDirectory();
                string newOupath = newOuName + "," + ouPath;
                ou = de.Children.Add(newOupath, "organizationalUnit");
                ou.CommitChanges();
                res = true;
                ou.Close();
                de.Close();
            }
            catch (Exception)
            {
                res = false;
            }
            return res;
        }

        /// <summary>
        /// 作用：OU重命名
        /// 备注：路径需从子级到父级,如:OU=B,OU=A[其组织结构B是]
        ///       经过在OU中创建用户测试，完成
        /// </summary>
        /// <param name="ouPath">需重名命的OU路径(不包含需修改OU的名称)</param>
        /// <param name="oldOuName">历史OU名称</param>
        /// <param name="newOuName">当前OU名称</param>
        /// <returns>返回是否成功</returns>
        public bool RenameOu(string ouPath, string oldOuName, string newOuName)
        {
            bool res = false;
            DirectoryEntry de = null;
            try
            {
                de = GetDirectory();
                string reNameOuPath = "OU=" + oldOuName + "," + ouPath;
                DirectoryEntry oude = de.Children.Find(reNameOuPath, "organizationalUnit");
                oude.Rename("OU=" + newOuName);
                oude.CommitChanges();
                oude.Dispose();
                res = true;
            }
            catch (Exception)
            {
                res = false;
            }
            return res;
        }

        /// <summary>
        /// 作用：移动指定对象所在位置
        /// 备注：包含用户、OU
        /// </summary>
        /// <param name="oldPath">需移动OU/员工的历史路径</param>
        /// <param name="newPath">需移动OU/员工的当前路径</param>
        /// <returns>返回是否移动成功</returns>
        public bool Move(string oldPath, string newPath)
        {
            DirectoryEntry de = null;
            DirectoryEntry oldDe = null;
            DirectoryEntry newDe = null;
            bool res = false;
            try
            {
                de = GetDirectory();
                oldDe = de.Children.Find(oldPath);
                newDe = de.Children.Find(newPath);
                oldDe.MoveTo(newDe);
                res = true;
            }
            catch (Exception)
            {
                res = false;
            }
            return res;
        }
        /// <summary>
        /// 作用：查询是否有指定路径下的OU存在
        /// </summary>
        /// <param name="username">指定OU的上级路径</param>
        /// <param name="msg">被查询的OU名称</param>
        /// <returns></returns>
        public bool SelectUser(string username, ref string msg)
        {
            bool res = false;
            DirectoryEntry de = null;
            try
            {
                de = GetDirectory();
                DirectorySearcher ds = new DirectorySearcher(de);
                ds.Filter = "(&(objectClass=user)(sAMAccountName=" + username + "))";
                SearchResult sr = ds.FindOne();
                if (sr != null)
                {
                    res = true;
                }
                else
                {
                    res = false;
                }
                de.Close();
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
                res = false;
            }
            return res;
        }

        /// <summary>
        /// 作用：添加用户到指定位置
        /// 备注：各个参数如有无需添加项，可赋值为空字符串
        /// </summary>
        /// <param name="path">指定位置[例如：OU=changhong]</param>
        /// <param name="logonName">登录名</param>
        /// <param name="password">密码</param>
        /// <param name="name">用户姓名</param>
        /// <param name="desc">说明</param>
        /// <param name="mail">邮箱地址</param>
        /// <param name="telphone">手机号码</param>
        /// <param name="depart">部门名称</param>
        /// <param name="post">职务</param>
        /// <returns>返回是否创建成功。成功：true，失败：false</returns>
        public bool CreateUserTo(string path, string logonName, string password, string name, string desc, string mail, string telphone, string depart, string post)
        {
            bool res = false;
            DirectoryEntry de = null;
            DirectoryEntry newUserDe = null;
            string oGUID = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(telphone))
                {
                    telphone = "15888888888";
                }
                de = GetDirectory();
                //新增用户资料
                //DirectoryEntry dirFir = de.Children.Add(path, "organizationalUnit");
                newUserDe = de.Children.Add("CN=" + name + "," + path, "user");
                newUserDe.Properties["sAMAccountName"].Value = logonName;//用户登录名称
                newUserDe.Properties["userPrincipalName"].Value = logonName;//用户登录名称
                newUserDe.Properties["displayName"].Value = name;//姓名
                newUserDe.Properties["description"].Add(desc);
                newUserDe.Properties["mail"].Value = mail;//邮件地址
                newUserDe.Properties["telephoneNumber"].Value = telphone;//电话
                newUserDe.Properties["department"].Value = depart;//部门
                newUserDe.Properties["title"].Value = post;//职务
                newUserDe.CommitChanges();
                //oGUID = newUserDe.Guid.ToString();//返回新建用户的编号                
                //设置密码
                newUserDe.Invoke("SetPassword", new object[] { password });
                newUserDe.CommitChanges();
                //启用账户
                int val = (int)newUserDe.Properties["userAccountControl"].Value;
                newUserDe.Properties["userAccountControl"].Value = val & ~0x2;
                newUserDe.CommitChanges();
                UpdateUserInfo(logonName, name, desc, mail, telphone, depart, post);
                newUserDe.Close();
                res = true;
                de.Close();
            }
            catch (Exception)
            {
                res = false;
            }
            return res;
        }

        /// <summary>
        /// 作用：修改指定用户密码
        /// 备注：用户本身及对应管理员可执行
        /// </summary>
        /// <param name="userName">被修改用户的登录账户</param>
        /// <param name="newPwd"></param>
        /// <returns></returns>
        public bool UpdateUserPassword(string userName, string newPwd)
        {
            bool res = false;
            DirectoryEntry de = null;
            try
            {
                de = GetDirectory();
                DirectorySearcher ds = new DirectorySearcher(de);
                ds.Filter = "(&(objectClass=user)(sAMAccountName=" + userName + "))";
                SearchResult sr = ds.FindOne();
                if (sr != null)
                {
                    DirectoryEntry userDe = sr.GetDirectoryEntry();
                    userDe.Invoke("SetPassword", new object[] { newPwd });
                    userDe.Properties["LockOutTime"].Value = 0;
                    userDe.CommitChanges();
                }
                res = true;
                de.Close();
            }
            catch (Exception)
            {
                res = false;
            }
            return res;
        }
        /// <summary>
        /// 作用：根据用户登录账户修改用户自身信息
        /// 备注：提供修改密码
        /// </summary>
        /// <param name="logonName">登录名</param>
        /// <param name="name">用户姓名</param>
        /// <param name="desc">说明</param>
        /// <param name="mail">邮箱地址</param>
        /// <param name="telphone">手机号码</param>
        /// <param name="depart">部门名称</param>
        /// <param name="post">岗位名称</param>
        /// <returns></returns>
        public bool UpdateUserInfo(string logonName, string name, string desc, string mail, string telphone, string depart, string post)
        {
            bool res = false;
            DirectoryEntry de = null;
            try
            {
                de = new DirectoryEntry("LDAP://" + LdapPath + "/" + Domain + "");
                de.Username = @"" + AdminName + "";
                de.Password = @"" + AdminPassword + "";
                de.AuthenticationType = AuthenticationTypes.Secure;
                DirectorySearcher search = new DirectorySearcher(de);
                search.Filter = "(&(objectClass=user)(sAMAccountName=" + logonName + "))";
                search.SearchScope = SearchScope.Subtree;
                SearchResult sr = search.FindOne();
                if (sr != null)
                {
                    DirectoryEntry newInfo = sr.GetDirectoryEntry();
                    newInfo.Properties["displayName"].Value = name.Trim();//姓名
                    newInfo.Properties["description"].Value = desc.Trim();
                    newInfo.Properties["mail"].Value = mail;//邮件地址
                    newInfo.Properties["telephoneNumber"].Value = telphone;//电话
                    newInfo.Properties["department"].Value = depart;//部门
                    newInfo.Properties["title"].Value = post;//职务
                    newInfo.CommitChanges();
                    res = true;
                    de.Close();
                }
            }
            catch (Exception)
            {
                res = false;
            }
            return res;
        }
        /// <summary>
        /// 作用：启用账户[Enable a User Account]
        /// 备注：仅需提供用户的账户名称
        /// </summary>
        /// <param name="userDn">用户登录名</param>
        public bool EnableUserAccount(string userDn)
        {
            bool res = false;
            DirectoryEntry de = null;
            DirectoryEntry user = null;
            try
            {
                de = GetDirectory();
                DirectorySearcher ds = new DirectorySearcher(de);
                ds.Filter = "(&(objectClass=user)(sAMAccountName=" + userDn + "))";
                SearchResult sr = ds.FindOne();
                if (sr != null)
                {
                    user = sr.GetDirectoryEntry();
                    int val = (int)user.Properties["userAccountControl"].Value;
                    user.Properties["userAccountControl"].Value = val & ~0x2;
                    user.CommitChanges();
                    res = true;
                    user.Close();
                    de.Close();
                }
            }
            catch (Exception)
            {
                res = false;
            }
            return res;
        }
        /// <summary>
        /// 作用：禁用用户账户[Disable a User Account]
        /// 备注：仅需提供用户的账户名称
        /// </summary>
        /// <param name="userDn"></param>
        public bool DisableUserAccount(string userDn)
        {
            bool res = false;
            DirectoryEntry de = null;
            DirectoryEntry user = null;
            try
            {
                de = GetDirectory();
                DirectorySearcher ds = new DirectorySearcher(de);
                ds.Filter = "(&(objectClass=user)(sAMAccountName=" + userDn + "))";
                SearchResult sr = ds.FindOne();
                if (sr != null)
                {
                    user = sr.GetDirectoryEntry();
                    int val = (int)user.Properties["userAccountControl"].Value;
                    user.Properties["userAccountControl"].Value = val | 0x2;
                    user.CommitChanges();
                    res = true;
                    user.Close();
                }
                else
                {
                    res = true;
                }
            }
            catch (Exception e)
            {
                res = false;
            }
            return res;
        }
    }
        #endregion

}