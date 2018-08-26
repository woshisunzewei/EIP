using System;

namespace EIP.Common.Pay.WeiXin.lib
{
    public class WxPayException : Exception 
    {
        public WxPayException(string msg) : base(msg) 
        {

        }
     }
}