using System;
using System.Globalization;
using System.Threading;

namespace EIP.Common.Core.Extensions
{
    /// <summary>
    /// 浮点类型扩展
    /// </summary>
    public static class DecimalExtension
    {
        #region 转换为小数点后四位
        /// <summary>
        /// 转换为小数点后四位
        /// </summary>
        /// <param name="price">价格</param>
        /// <param name="currency">当前值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string PriceString4(this decimal? price, 
            string currency,
            string defaultValue = "0")
        {
            currency = currency ?? string.Empty;
            return price.GetValueOrDefault(0) != 0 ? string.Format("{0:0.0000} {1}", decimal.Round(price.Value, 4), currency) : defaultValue;
        }
        /// <summary>
        /// 转换为小数点后四位
        /// </summary>
        /// <param name="price"></param>
        /// <param name="currency"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string PriceString4(this decimal price,
            string currency, 
            string defaultValue = "0")
        {
            return price != 0 ? string.Format("{0:0.0000} {1}", decimal.Round(price, 4), currency) : defaultValue;
        }
        #endregion

        #region 转换为小数点后两位
        /// <summary>
        /// 转换为小数点后两位
        /// </summary>
        /// <param name="price">价格</param>
        /// <param name="currency">当前值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string PriceString2(this decimal? price,
            string currency,
            string defaultValue = "0")
        {
            currency = currency ?? string.Empty;
            return price.GetValueOrDefault(0) != 0 ? string.Format("{0:0.00} {1}", decimal.Round(price.Value, 2), currency) : defaultValue;
        }

        public static string PriceString2(this decimal price,
            string currency,
            string defaultValue = "0")
        {
            return price != 0 ? string.Format("{0:0.00} {1}", decimal.Round(price, 2), currency) : defaultValue;
        }
        #endregion

        #region 格式化货币
        /// <summary>
        /// 格式化货币
        /// </summary>
        /// <param name="target"></param>
        /// <param name="currencyCode"></param>
        /// <returns></returns>
        public static string FormatCurrency(this decimal target, 
            string currencyCode)
        {
            var cultureInfo = CultureInfo.CreateSpecificCulture("EN-US");
            if (new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).ISOCurrencySymbol.Equals(currencyCode))
            {
                cultureInfo = CultureInfo.CurrentCulture;
            }
            if (currencyCode != "USD")
            {
                foreach (var info in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
                {
                    if (new RegionInfo(info.LCID).ISOCurrencySymbol.Equals(currencyCode))
                    {
                        cultureInfo = info;
                    }
                }
            }

            var myCIclone = (CultureInfo)cultureInfo.Clone();
            const int decimalDigits = 2;

            myCIclone.NumberFormat.CurrencyDecimalDigits = decimalDigits;
            return string.Format(myCIclone, "{0:c}", new object[] { target });
        }

        #endregion

        #region 空转换为0
        /// <summary>
        /// 空转换为0
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static decimal ConvertNullToZero(this decimal? instance)
        {
            if (instance == null)
                return 0;

            return instance.Value;
        }
        #endregion

        #region 转换人民币大小金额
        /// <summary>
        ///     转换人民币大小金额
        /// </summary>
        /// <param name="num">金额</param>
        /// <returns>返回大写形式</returns>
        public static string CmycurD(this decimal num)
        {
            const string str1 = "零壹贰叁肆伍陆柒捌玖"; //0-9所对应的汉字 
            string str2 = "万仟佰拾亿仟佰拾万仟佰拾元角分"; //数字位所对应的汉字 
            string str5 = ""; //人民币大写金额形式 
            int i; //循环变量 
            string ch2 = ""; //数字位的汉字读法 
            int nzero = 0; //用来计算连续的零值是几个 

            num = Math.Round(Math.Abs(num), 2); //将num取绝对值并四舍五入取2位小数 
            var str4 = ((long)(num * 100)).ToString();
            var j = str4.Length;
            if (j > 15)
            {
                return "溢出";
            }
            str2 = str2.Substring(15 - j); //取出对应位数的str2的值。如：200.55,j为5所以str2=佰拾元角分 

            //循环取出每一位需要转换的值 
            for (i = 0; i < j; i++)
            {
                var str3 = str4.Substring(i, 1); //从原num值中取出的值 
                var temp = Convert.ToInt32(str3); //从原num值中取出的值 
                string ch1; //数字的汉语读法 
                if (i != (j - 3) && i != (j - 7) && i != (j - 11) && i != (j - 15))
                {
                    //当所取位数不为元、万、亿、万亿上的数字时 
                    if (str3 == "0")
                    {
                        ch1 = "";
                        ch2 = "";
                        nzero = nzero + 1;
                    }
                    else
                    {
                        if (str3 != "0" && nzero != 0)
                        {
                            ch1 = "零" + str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                    }
                }
                else
                {
                    //该位是万亿，亿，万，元位等关键位 
                    if (str3 != "0" && nzero != 0)
                    {
                        ch1 = "零" + str1.Substring(temp * 1, 1);
                        ch2 = str2.Substring(i, 1);
                        nzero = 0;
                    }
                    else
                    {
                        if (str3 != "0" && nzero == 0)
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            if (str3 == "0" && nzero >= 3)
                            {
                                ch1 = "";
                                ch2 = "";
                                nzero = nzero + 1;
                            }
                            else
                            {
                                if (j >= 11)
                                {
                                    ch1 = "";
                                    nzero = nzero + 1;
                                }
                                else
                                {
                                    ch1 = "";
                                    ch2 = str2.Substring(i, 1);
                                    nzero = nzero + 1;
                                }
                            }
                        }
                    }
                }
                if (i == (j - 11) || i == (j - 3))
                {
                    //如果该位是亿位或元位，则必须写上 
                    ch2 = str2.Substring(i, 1);
                }
                str5 = str5 + ch1 + ch2;

                if (i == j - 1 && str3 == "0")
                {
                    //最后一位（分）为0时，加上“整” 
                    str5 = str5 + '整';
                }
            }
            if (num == 0)
            {
                str5 = "零元整";
            }
            return str5;
        }
        #endregion
    }
}