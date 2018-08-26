using System;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace EIP.Common.Core.Utils
{
    /// <summary>
    ///     邮件工具
    /// </summary>
    public class EmailUtil
    {
        private static readonly MailMessage MailMessage = new MailMessage(); //实例化一个邮件类  

        #region 构造函数
        public EmailUtil()
        {

        }
        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="toAddresses">收件人地址（多个以,号分开）</param>
        /// <param name="fromAddress">发件人地址</param>
        /// <param name="title">主题</param>
        /// <param name="body">正文</param>
        public EmailUtil(string toAddresses, string fromAddress, string title, string body)
            : this(toAddresses, fromAddress, "", "", title, body, false)
        {
        }

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="toAddress">收件人地址</param>
        /// <param name="fromAddress">发件人地址</param>
        /// <param name="toName">收件人名字</param>
        /// <param name="fromName">发件人姓名</param>
        /// <param name="title">主题</param>
        /// <param name="body">正文</param>
        /// <param name="isBodyHtml">正文是否为html格式</param>
        public EmailUtil(string toAddress, string fromAddress, string toName, string fromName, string title, string body,
            bool isBodyHtml)
        {
            MailMessage.From = new MailAddress(fromAddress, fromName, Encoding.GetEncoding(936));
            if (toName.Equals(""))
                MailMessage.To.Add(toAddress);
            else
                MailMessage.To.Add(new MailAddress(toAddress, toName, Encoding.GetEncoding(936)));

            MailMessage.Subject = title;
            MailMessage.SubjectEncoding = Encoding.GetEncoding(936);

            MailMessage.Body = body;
            MailMessage.IsBodyHtml = isBodyHtml;
            MailMessage.BodyEncoding = Encoding.GetEncoding(936);
        }

        #endregion

        #region 通过Smtp发送邮件

        /// <summary>
        ///     设置SMTP，并且将邮件发送出去
        ///     所有参数都设置完成后再调用该方法
        /// </summary>
        /// <param name="password">发件人密码</param>
        /// <param name="smtpHost">SMTP服务器地址</param>
        public void SetSmtp(string password, string smtpHost)
        {
            SetSmtp(MailMessage.From.Address, password, smtpHost, 25, false, MailPriority.Normal);
        }

        /// <summary>
        ///     设置SMTP，并且将邮件发送出去
        ///     所有参数都设置完成后再调用该方法
        /// </summary>
        /// <param name="password">发件人密码</param>
        /// <param name="smtpHost">SMTP服务器地址</param>
        /// <param name="isEnableSsl"></param>
        public void SetSmtp(string password, string smtpHost, bool isEnableSsl)
        {
            SetSmtp(MailMessage.From.Address, password, smtpHost, 25, isEnableSsl, MailPriority.Normal);
        }

        /// <summary>
        ///     设置SMTP，并且将邮件发送出去
        ///     所有参数都设置完成后再调用该方法
        /// </summary>
        /// <param name="address">发件人地址（必须为真实有效的email地址）</param>
        /// <param name="password">发件人密码</param>
        /// <param name="smtpHost">SMTP服务器地址</param>
        /// <param name="smtpPort">SMTP服务器的端口</param>
        /// <param name="isEnableSsl">SMTP服务器是否启用SSL加密</param>
        /// <param name="priority">邮件的优先级</param>
        public void SetSmtp(string address, string password, string smtpHost, int smtpPort, bool isEnableSsl,
            MailPriority priority)
        {
            var smtp = new SmtpClient();
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new NetworkCredential(address, password);
            smtp.Host = smtpHost;
            smtp.Port = smtpPort;
            smtp.EnableSsl = isEnableSsl;
            MailMessage.Priority = priority;
            smtp.Send(MailMessage); //发送邮件  
        }

        #endregion

        #region 通过Pop3获取指定邮件

        public string _mAddress = "pop.qq.com";
        public int _mPort = 995;

        /// <summary>
        ///     获取Mail列表
        /// </summary>
        /// <param name="pName">用户名</param>
        /// <param name="pPassWord">密码</param>
        /// <returns>Mail信息</returns>
        public DataTable GetMailTable(string pName, string pPassWord)
        {
            var client = new Pop3Client();
            client.UserName = pName;
            client.PassWord = pPassWord;
            client.Client = new TcpClient();
            client.Client.BeginConnect(_mAddress, _mPort, OnConnectRequest, client);
            if (client.Error.Length != 0) throw new Exception("错误信息!" + client.Error);
            return client.MailDataTable;
        }

        /// <summary>
        ///     获取邮件内容
        /// </summary>
        /// <param name="pName">名称</param>
        /// <param name="pPassWord">密码</param>
        /// <param name="pMailIndex">邮件编号</param>
        /// <returns>数据集</returns>
        public DataTable GetMail(string pName, string pPassWord, int pMailIndex)
        {
            var client = new Pop3Client();
            client.UserName = pName;
            client.PassWord = pPassWord;
            client.Client = new TcpClient();
            client.ReadIndex = pMailIndex;
            client.Client.BeginConnect(_mAddress, _mPort, OnConnectRequest, client);
            if (client.Error.Length != 0) throw new Exception("错误信息!" + client.Error);
            return client.MailTable;
        }

        /// <summary>
        ///     连接事件
        /// </summary>
        /// <param name="ar"></param>
        private void OnConnectRequest(IAsyncResult ar)
        {
            var client = (Pop3Client)ar.AsyncState;
            var readBytes = new byte[0];
            client.Client.Client.BeginReceive(readBytes, 0, 0, SocketFlags.None, OnWrite, client);
        }

        /// <summary>
        ///     连接事件
        /// </summary>
        /// <param name="ar"></param>
        private void OnSend(IAsyncResult ar)
        {
            var client = (Pop3Client)ar.AsyncState;
            var readBytes = new byte[0];
            client.Client.Client.BeginReceive(readBytes, 0, 0, SocketFlags.None, OnWrite, client);
        }

        /// <summary>
        ///     连接事件
        /// </summary>
        /// <param name="ar"></param>
        private void OnWrite(IAsyncResult ar)
        {
            var client = (Pop3Client)ar.AsyncState;
            var writeBytes = new byte[client.Client.Client.ReceiveBufferSize];
            client.Client.Client.Receive(writeBytes);
            if (client.ReadEnd) writeBytes = ReadEnd(writeBytes, client);
            var sendBytes = client.GetSendBytes(writeBytes);
            if (sendBytes.Length == 0) return;
            client.Client.Client.BeginSend(sendBytes, 0, sendBytes.Length, SocketFlags.None, OnSend, client);
        }

        /// <summary>
        ///     获取知道获取到. 否则一直获取数据
        /// </summary>
        /// <param name="pValue"></param>
        /// <param name="pClient"></param>
        /// <returns></returns>
        private byte[] ReadEnd(byte[] pValue, Pop3Client pClient)
        {
            if (Encoding.ASCII.GetString(pValue).IndexOf("\r\n.\r\n") != -1) return pValue;
            var stream = new MemoryStream();
            stream.Write(pValue, 0, pValue.Length);
            while (true)
            {
                var writeBytes = new byte[pClient.Client.ReceiveBufferSize];
                pClient.Client.Client.Receive(writeBytes);
                stream.Write(writeBytes, 0, writeBytes.Length);
                Thread.Sleep(100);
                if (Encoding.ASCII.GetString(writeBytes).IndexOf("\r\n.\r\n") != -1) return stream.ToArray();
            }
        }

        private class Pop3Client
        {
            private int _mSendMessage;
            private int _mTopIndex = 1;
            public TcpClient Client;
            public string Error = "";
            public string PassWord = "";
            public bool ReadEnd;
            public int ReadIndex = -1;
            public bool ReturnEnd;
            public string UserName = "";
            public readonly DataTable MailDataTable = new DataTable();
            public readonly DataTable MailTable = new DataTable();

            public Pop3Client()
            {
                MailDataTable.Columns.Add("NUM");
                MailDataTable.Columns.Add("Size");
                MailDataTable.Columns.Add("Form");
                MailDataTable.Columns.Add("To");
                MailDataTable.Columns.Add("Subject");
                MailDataTable.Columns.Add("Date");

                MailTable.Columns.Add("Type", typeof(string));
                MailTable.Columns.Add("Text", typeof(object));
                MailTable.Columns.Add("Name", typeof(string));
            }

            /// <summary>
            ///     获取下一个登陆到获取列表需要的命令
            /// </summary>
            /// <param name="pValue"></param>
            /// <returns></returns>
            public byte[] GetSendBytes(byte[] pValue)
            {
                ReadEnd = false;
                var value = Encoding.Default.GetString(pValue).Replace("\0", "");
                if (value.IndexOf("+OK") == 0)
                {
                    _mSendMessage++;
                    switch (_mSendMessage)
                    {
                        case 1:
                            return Encoding.ASCII.GetBytes("USER " + UserName + "\r\n");
                        case 2:
                            return Encoding.ASCII.GetBytes("PASS " + PassWord + "\r\n");
                        case 3:
                            ReadEnd = true;
                            if (ReadIndex != -1)
                            {
                                _mSendMessage = 5;
                                return Encoding.ASCII.GetBytes("RETR " + ReadIndex + "\r\n");
                            }
                            return Encoding.ASCII.GetBytes("LIST\r\n");
                        case 4:
                            var _List = value.Split(new[] { '\r', '\n', '.' }, StringSplitOptions.RemoveEmptyEntries);
                            for (var i = 1; i != _List.Length; i++)
                            {
                                var _MaliSize = _List[i].Split(' ');
                                MailDataTable.Rows.Add(_MaliSize[0], _MaliSize[1]);
                            }
                            if (MailDataTable.Rows.Count == 0)
                            {
                                ReturnEnd = true;
                                return new byte[0];
                            }
                            ReadEnd = true;
                            _mTopIndex = 1;
                            return Encoding.ASCII.GetBytes("TOP 1\r\n");
                        case 5:
                            var _Regex = new Regex(@"(?<=Date: ).*?(\r\n)+");
                            var _Collection = _Regex.Matches(value);
                            if (_Collection.Count != 0)
                                MailDataTable.Rows[_mTopIndex - 1]["Date"] = GetReadText(_Collection[0].Value);

                            var _RegexFrom = new Regex(@"(?<=From: ).*?(\r\n)+");
                            var _CollectionForm = _RegexFrom.Matches(value);
                            if (_CollectionForm.Count != 0)
                                MailDataTable.Rows[_mTopIndex - 1]["Form"] = GetReadText(_CollectionForm[0].Value);


                            var _RegexTo = new Regex(@"(?<=To: ).*?(\r\n)+");
                            var _CollectionTo = _RegexTo.Matches(value);
                            if (_CollectionTo.Count != 0)
                                MailDataTable.Rows[_mTopIndex - 1]["To"] = GetReadText(_CollectionTo[0].Value);

                            var _RegexSubject = new Regex(@"(?<=Subject: ).*?(\r\n)+");
                            var _CollectionSubject = _RegexSubject.Matches(value);
                            if (_CollectionSubject.Count != 0)
                                MailDataTable.Rows[_mTopIndex - 1]["Subject"] = GetReadText(_CollectionSubject[0].Value);

                            _mTopIndex++;
                            _mSendMessage--;
                            ReadEnd = true;
                            if (_mTopIndex > MailDataTable.Rows.Count)
                            {
                                ReturnEnd = true;
                                return Encoding.ASCII.GetBytes("QUIT");
                            }
                            return Encoding.ASCII.GetBytes("TOP " + _mTopIndex + "\r\n");
                        case 6:
                            GetMailText(value);
                            ReturnEnd = true;
                            return Encoding.ASCII.GetBytes("QUIT");
                    }
                }
                Error = value;
                ReturnEnd = true;
                return new byte[0];
            }

            /// <summary>
            ///     转换文字里的字符集
            /// </summary>
            /// <param name="p_Text"></param>
            /// <returns></returns>
            public string GetReadText(string p_Text)
            {
                var _Regex = new Regex(@"(?<=\=\?).*?(\?\=)+");
                var _Collection = _Regex.Matches(p_Text);
                var _Text = p_Text;
                foreach (Match _Match in _Collection)
                {
                    var _Value = "=?" + _Match.Value;
                    if (_Value[0] == '=')
                    {
                        var _BaseData = _Value.Split('?');
                        if (_BaseData.Length == 5)
                        {
                            var _Coding = Encoding.GetEncoding(_BaseData[1]);
                            _Text = _Text.Replace(_Value, _Coding.GetString(Convert.FromBase64String(_BaseData[3])));
                        }
                    }
                }
                return _Text;
            }

            #region 获取邮件正文 和 附件

            /// <summary>
            ///     获取文字主体
            /// </summary>
            /// <param name="p_Mail"></param>
            /// <returns></returns>
            public void GetMailText(string p_Mail)
            {
                var _ConvertType = GetTextType(p_Mail, "\r\nContent-Type: ", ";");
                if (_ConvertType.Length == 0)
                {
                    _ConvertType = GetTextType(p_Mail, "\r\nContent-Type: ", "\r");
                }
                var _StarIndex = -1;
                var _EndIndex = -1;
                var _ReturnText = "";
                var _Transfer = "";
                var _Boundary = "";
                var _EncodingName = GetTextType(p_Mail, "charset=\"", "\"").Replace("\"", "");
                var _Encoding = Encoding.Default;
                if (_EncodingName != "") _Encoding = Encoding.GetEncoding(_EncodingName);
                switch (_ConvertType)
                {
                    case "text/html;":
                        _Transfer = GetTextType(p_Mail, "\r\nContent-Transfer-Encoding: ", "\r\n").Trim();
                        _StarIndex = p_Mail.IndexOf("\r\n\r\n");
                        if (_StarIndex != -1) _ReturnText = p_Mail.Substring(_StarIndex, p_Mail.Length - _StarIndex);
                        switch (_Transfer)
                        {
                            case "8bit":

                                break;
                            case "quoted-printable":
                                _ReturnText = DecodeQuotedPrintable(_ReturnText, _Encoding);
                                break;
                            case "base64":
                                _ReturnText = DecodeBase64(_ReturnText, _Encoding);
                                break;
                        }
                        MailTable.Rows.Add("text/html", _ReturnText);
                        break;
                    case "text/plain;":
                        _Transfer = GetTextType(p_Mail, "\r\nContent-Transfer-Encoding: ", "\r\n").Trim();
                        _StarIndex = p_Mail.IndexOf("\r\n\r\n");
                        if (_StarIndex != -1) _ReturnText = p_Mail.Substring(_StarIndex, p_Mail.Length - _StarIndex);
                        switch (_Transfer)
                        {
                            case "8bit":

                                break;
                            case "quoted-printable":
                                _ReturnText = DecodeQuotedPrintable(_ReturnText, _Encoding);
                                break;
                            case "base64":
                                _ReturnText = DecodeBase64(_ReturnText, _Encoding);
                                break;
                        }
                        MailTable.Rows.Add("text/plain", _ReturnText);
                        break;
                    case "multipart/alternative;":
                        _Boundary = GetTextType(p_Mail, "boundary=\"", "\"").Replace("\"", "");
                        _StarIndex = p_Mail.IndexOf("--" + _Boundary + "\r\n");
                        if (_StarIndex == -1) return;
                        while (true)
                        {
                            _EndIndex = p_Mail.IndexOf("--" + _Boundary, _StarIndex + _Boundary.Length);
                            if (_EndIndex == -1) break;
                            GetMailText(p_Mail.Substring(_StarIndex, _EndIndex - _StarIndex));
                            _StarIndex = _EndIndex;
                        }
                        break;
                    case "multipart/mixed;":
                        _Boundary = GetTextType(p_Mail, "boundary=\"", "\"").Replace("\"", "");
                        _StarIndex = p_Mail.IndexOf("--" + _Boundary + "\r\n");
                        if (_StarIndex == -1) return;
                        while (true)
                        {
                            _EndIndex = p_Mail.IndexOf("--" + _Boundary, _StarIndex + _Boundary.Length);
                            if (_EndIndex == -1) break;
                            GetMailText(p_Mail.Substring(_StarIndex, _EndIndex - _StarIndex));
                            _StarIndex = _EndIndex;
                        }
                        break;
                    default:
                        if (_ConvertType.IndexOf("application/") == 0)
                        {
                            _StarIndex = p_Mail.IndexOf("\r\n\r\n");
                            if (_StarIndex != -1)
                                _ReturnText = p_Mail.Substring(_StarIndex, p_Mail.Length - _StarIndex);
                            _Transfer = GetTextType(p_Mail, "\r\nContent-Transfer-Encoding: ", "\r\n").Trim();
                            var _Name = GetTextType(p_Mail, "filename=\"", "\"").Replace("\"", "");
                            _Name = GetReadText(_Name);
                            var _FileBytes = new byte[0];
                            switch (_Transfer)
                            {
                                case "base64":
                                    _FileBytes = Convert.FromBase64String(_ReturnText);
                                    break;
                            }
                            MailTable.Rows.Add("application/octet-stream", _FileBytes, _Name);
                        }
                        break;
                }
            }

            /// <summary>
            ///     获取类型（正则）
            /// </summary>
            /// <param name="p_Mail">原始文字</param>
            /// <param name="p_TypeText">前文字</param>
            /// <param name="p_End">结束文字</param>
            /// <returns>符合的记录</returns>
            public string GetTextType(string p_Mail, string p_TypeText, string p_End)
            {
                var _Regex = new Regex(@"(?<=" + p_TypeText + ").*?(" + p_End + ")+");
                var _Collection = _Regex.Matches(p_Mail);
                if (_Collection.Count == 0) return "";
                return _Collection[0].Value;
            }

            /// <summary>
            ///     QuotedPrintable编码接码
            /// </summary>
            /// <param name="p_Text">原始文字</param>
            /// <param name="p_Encoding">编码方式</param>
            /// <returns>接码后信息</returns>
            public string DecodeQuotedPrintable(string p_Text, Encoding p_Encoding)
            {
                var _Stream = new MemoryStream();
                var _CharValue = p_Text.ToCharArray();
                for (var i = 0; i != _CharValue.Length; i++)
                {
                    switch (_CharValue[i])
                    {
                        case '=':
                            if (_CharValue[i + 1] == '\r' || _CharValue[i + 1] == '\n')
                            {
                                i += 2;
                            }
                            else
                            {
                                try
                                {
                                    _Stream.WriteByte(Convert.ToByte(_CharValue[i + 1] + _CharValue[i + 2].ToString(),
                                        16));
                                    i += 2;
                                }
                                catch
                                {
                                    _Stream.WriteByte(Convert.ToByte(_CharValue[i]));
                                }
                            }
                            break;
                        default:
                            _Stream.WriteByte(Convert.ToByte(_CharValue[i]));
                            break;
                    }
                }
                return p_Encoding.GetString(_Stream.ToArray());
            }

            /// <summary>
            ///     解码BASE64
            /// </summary>
            /// <param name="p_Text"></param>
            /// <param name="p_Encoding"></param>
            /// <returns></returns>
            public string DecodeBase64(string p_Text, Encoding p_Encoding)
            {
                if (p_Text.Trim().Length == 0) return "";
                var _ValueBytes = Convert.FromBase64String(p_Text);
                return p_Encoding.GetString(_ValueBytes);
            }

            #endregion

        #endregion

            #region 设置邮件地址

            /// <summary>
            ///     设置更多收件人
            /// </summary>
            /// <param name="toAddresses">收件人地址</param>
            public void SetMoreToAddress(string toAddresses)
            {
                MailMessage.To.Add(toAddresses);
            }

            /// <summary>
            ///     设置更多收件人
            /// </summary>
            /// <param name="toAddress">收件人地址</param>
            /// <param name="toName">收件人名字</param>
            public void SetMoreToAddress(string toAddress, string toName)
            {
                MailMessage.To.Add(new MailAddress(toAddress, toName, Encoding.GetEncoding(936)));
            }

            /// <summary>
            ///     设置抄送者（多个以,号分开）
            /// </summary>
            /// <param name="ccAddresses">抄送者地址</param>
            public void SetCarbonCopyFor(string ccAddresses)
            {
                MailMessage.CC.Add(ccAddresses);
            }

            /// <summary>
            ///     设置抄送者
            /// </summary>
            /// <param name="ccAddress">抄送者地址</param>
            /// <param name="ccName">抄送者名字</param>
            public void SetCarbonCopyFor(string ccAddress, string ccName)
            {
                MailMessage.Bcc.Add(new MailAddress(ccAddress, ccName, Encoding.GetEncoding(936)));
            }

            /// <summary>
            ///     设置密送者（多个以,号分开）
            /// </summary>
            /// <param name="bccAddresses">密送者</param>
            public void SetBlindCarbonCopyFor(string bccAddresses)
            {
                MailMessage.Bcc.Add(bccAddresses);
            }

            /// <summary>
            ///     设置密送者
            /// </summary>
            /// <param name="bccAddress">密送者</param>
            /// <param name="bccName">密送者名字</param>
            public void SetBlindCarbonCopyFor(string bccAddress, string bccName)
            {
                MailMessage.Bcc.Add(new MailAddress(bccAddress, bccName, Encoding.GetEncoding(936)));
            }

            #endregion

            #region 添加附件

            /// <summary>
            ///     添加附件（自动识别文件类型）
            /// </summary>
            /// <param name="fileName">单个文件的路径</param>
            public void Attachments(string fileName)
            {
                MailMessage.Attachments.Add(new Attachment(fileName));
            }

            /// <summary>
            ///     添加附件（默认为富文本RTF格式）
            /// </summary>
            /// <param name="fileName">单个文件的路径</param>
            public void AttachmentsForRtf(string fileName)
            {
                MailMessage.Attachments.Add(new Attachment(fileName, MediaTypeNames.Application.Rtf));
            }

            #endregion
        }
    }


}