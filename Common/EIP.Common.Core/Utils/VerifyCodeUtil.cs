using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using EIP.Common.Core.Extensions;
namespace EIP.Common.Core.Utils
{
    /// <summary>
    ///     验证码帮助类:加减法
    /// </summary>
    public class VerifyCodeUtil
    {

        private static readonly Random Random = new Random();

        #region 属性

        /// <summary>
        /// 获取或设置 字体名称集合
        /// </summary>
        public List<string> FontNames { get; set; }

        /// <summary>
        /// 获取或设置 汉字字体名称集合
        /// </summary>
        public List<string> FontNamesForHanzi { get; set; }

        /// <summary>
        /// 获取或设置 字体大小
        /// </summary>
        public int FontSize { get; set; }

        /// <summary>
        /// 获取或设置 字体宽度
        /// </summary>
        public int FontWidth { get; set; }

        /// <summary>
        /// 获取或设置 图片高度
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 获取或设置 背景颜色
        /// </summary>
        public Color BgColor { get; set; }

        /// <summary>
        /// 获取或设置 是否有边框
        /// </summary>
        public bool HasBorder { get; set; }

        /// <summary>
        /// 获取或设置 是否随机位置
        /// </summary>
        public bool RandomPosition { get; set; }

        /// <summary>
        /// 获取或设置 是否随机字体颜色
        /// </summary>
        public bool RandomColor { get; set; }

        /// <summary>
        /// 获取或设置 是否随机倾斜字体
        /// </summary>
        public bool RandomItalic { get; set; }

        /// <summary>
        /// 获取或设置 随机干扰点百分比（百分数形式）
        /// </summary>
        public double RandomPointPercent { get; set; }

        /// <summary>
        /// 获取或设置 随机干扰线数量
        /// </summary>
        public int RandomLineCount { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public static string VerifyCode { get; set; }
        #endregion

        /// <summary>
        ///     验证码在Session中存储的Key
        /// </summary>
        private const string VerifyCodeSessionKey = "VerifyCodeSessionKey";

        /// <summary>
        /// 获取验证码的图片
        /// </summary>
        /// <param name="length">长度</param>
        /// <param name="codeType">类型</param>
        /// <returns></returns>
        public static byte[] GetVerifyCodeImage(int length = 4, ValidateCodeType codeType = ValidateCodeType.Operation)
        {
            switch (codeType)
            {
                case ValidateCodeType.Hanzi:
                    VerifyCode = GetRandomHanzis(length);
                    break;
                case ValidateCodeType.Number:
                    VerifyCode = GetRandomNums(length);
                    break;
                case ValidateCodeType.NumberAndLetter:
                    VerifyCode = GetRandomNumsAndLetters(length);
                    break;
                default:
                    VerifyCode = GetRandomOperation();
                    break;
            }
            var image = CreateGraphic(VerifyCode);
            return image;
        }

        /// <summary>
        ///     获取Session中的验证码信息
        /// </summary>
        /// <returns></returns>
        public static string GetVerifyCode()
        {
            return HttpContext.Current.Session["VerifyCodeSessionKey"].ToString();
        }

        #region 私有方法

        /// <summary>
        ///     创建图片
        /// </summary>
        /// <param name="verifyCode"></param>
        /// <returns></returns>
        private static byte[] CreateGraphic(string verifyCode)
        {
            var image = new Bitmap((int)Math.Ceiling((verifyCode.Length * 15.0)), 25);
            var g = Graphics.FromImage(image);

            try
            {
                //生成随机生成器
                var random = new Random();

                //清空图片背景色
                g.Clear(Color.White);

                //画图片的背景噪音线
                for (var i = 0; i < 12; i++)
                {
                    var x1 = random.Next(image.Width);
                    var x2 = random.Next(image.Width);
                    var y1 = random.Next(image.Height);
                    var y2 = random.Next(image.Height);

                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }

                var font = new Font("Arial", 16,
                    (FontStyle.Bold | FontStyle.Italic));
                var brush =
                    new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                        Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(verifyCode, font, brush, 1, 1);
                
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

                var ms = new MemoryStream();
                image.Save(ms, ImageFormat.Png);
                //输出图片流
                return ms.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        /// <summary>
        /// 获取数字随机数
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private static string GetRandomNums(int length)
        {
            VerifyCode = string.Empty;
            int[] ints = new int[length];
            for (int i = 0; i < length; i++)
            {
                ints[i] = Random.Next(0, 9);
            }
            VerifyCode = ints.ExpandAndToString("");
            var session = HttpContext.Current.Session;
            session[VerifyCodeSessionKey] = VerifyCode;
            return VerifyCode;
        }

        /// <summary>
        /// 获取数值和字母的组合数
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private static string GetRandomNumsAndLetters(int length)
        {
            VerifyCode = string.Empty;
            const string allChar = "2,3,4,5,6,7,8,9," +
                "A,B,C,D,E,F,G,H,J,K,M,N,P,Q,R,S,T,U,V,W,X,Y,Z," +
                "a,b,c,d,e,f,g,h,k,m,n,p,q,r,s,t,u,v,w,x,y,z";
            string[] allChars = allChar.Split(',');
            List<string> result = new List<string>();
            while (result.Count < length)
            {
                int index = Random.Next(allChars.Length);
                string c = allChars[index];
                result.Add(c);
            }
            VerifyCode = result.ExpandAndToString("");
            var session = HttpContext.Current.Session;
            session[VerifyCodeSessionKey] = VerifyCode;
            return VerifyCode;
        }

        /// <summary>
        /// 获取汉字验证码
        /// </summary>
        /// <param name="length">验证码长度</param>
        /// <returns></returns>
        private static string GetRandomHanzis(int length)
        {
            VerifyCode = string.Empty;
            //汉字编码的组成元素，十六进制数
            string[] baseStrs = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f".Split(',');
            Encoding encoding = Encoding.GetEncoding("GB2312");
            //每循环一次产生一个含两个元素的十六进制字节数组，并放入bytes数组中
            //汉字由四个区位码组成，1、2位作为字节数组的第一个元素，3、4位作为第二个元素
            for (int i = 0; i < length; i++)
            {
                Random rnd = Random;
                int index1 = rnd.Next(11, 14);
                string str1 = baseStrs[index1];

                int index2 = index1 == 13 ? rnd.Next(0, 7) : rnd.Next(0, 16);
                string str2 = baseStrs[index2];

                int index3 = rnd.Next(10, 16);
                string str3 = baseStrs[index3];

                int index4 = index3 == 10 ? rnd.Next(1, 16) : (index3 == 15 ? rnd.Next(0, 15) : rnd.Next(0, 16));
                string str4 = baseStrs[index4];

                //定义两个字节变量存储产生的随机汉字区位码
                byte b1 = Convert.ToByte(str1 + str2, 16);
                byte b2 = Convert.ToByte(str3 + str4, 16);
                byte[] bs = { b1, b2 };

                VerifyCode += encoding.GetString(bs);
            }
            var session = HttpContext.Current.Session;
            session[VerifyCodeSessionKey] = VerifyCode;
            return VerifyCode;
        }

        /// <summary>
        /// 获取运算符验证码
        /// </summary>
        /// <returns></returns>
        private static string GetRandomOperation()
        {
            VerifyCode = string.Empty;
            var session = HttpContext.Current.Session;
            var random = new Random();
            var intFirst = random.Next(1, 10);
            var intSec = random.Next(1, 10);
            switch (random.Next(1, 3).ToString(CultureInfo.InvariantCulture))
            {
                case "2":
                    if (intFirst < intSec)
                    {
                        var intTemp = intFirst;
                        intFirst = intSec;
                        intSec = intTemp;
                    }
                    VerifyCode = intFirst + "-" + intSec + "= ?";
                    session[VerifyCodeSessionKey] = intFirst - intSec;
                    break;
                default:
                    VerifyCode = intFirst + "+" + intSec + "= ?";
                    session[VerifyCodeSessionKey] = intFirst + intSec;
                    break;
            }
            return VerifyCode;
        }
        #endregion
    }

    /// <summary>
    /// 验证码类型
    /// </summary>
    public enum ValidateCodeType
    {
        /// <summary>
        /// 纯数值
        /// </summary>
        Number,

        /// <summary>
        /// 数值与字母的组合
        /// </summary>
        NumberAndLetter,

        /// <summary>
        /// 汉字
        /// </summary>
        Hanzi,

        /// <summary>
        /// 运算
        /// </summary>
        Operation
    }
}