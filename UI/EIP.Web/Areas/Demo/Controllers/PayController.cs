using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using EIP.Common.Pay.Alipay;
using EIP.Common.Pay.Unionpay;
using EIP.Common.Pay.WeiXin.business;
using EIP.Common.Pay.WeiXin.lib;
using ThoughtWorks.QRCode.Codec;
using Notify = EIP.Common.Pay.Alipay.Notify;

namespace EIP.Web.Areas.Demo.Controllers
{
    /// <summary>
    ///     支付控制器
    /// </summary>
    public class PayController : Controller
    {
        /// <summary>
        ///     主页面
        /// </summary>
        /// <returns></returns>
        public ViewResultBase Main()
        {
            return View();
        }

        #region 网银

        /// <summary>
        ///     网银
        /// </summary>
        /// <returns></returns>
        public ActionResult UnionpayPcPay()
        {
            /**
            * 重要：联调测试时请仔细阅读注释！
            * 
            * 产品：跳转网关支付产品<br>
            * 交易：消费：前台跳转，有前台通知应答和后台通知应答<br>
            * 日期： 2015-09<br>
            * 版本： 1.0.0
            * 版权： 中国银联<br>
            * 说明：以下代码只是为了方便商户测试而提供的样例代码，商户可以根据自己需要，按照技术文档编写。该代码仅供参考，不提供编码性能规范性等方面的保障<br>
            * 提示：该接口参考文档位置：open.unionpay.com帮助中心 下载  产品接口规范  《网关支付产品接口规范》，<br>
            *              《平台接入接口规范-第5部分-附录》（内包含应答码接口规范，全渠道平台银行名称-简码对照表)<br>
            *              《全渠道平台接入接口规范 第3部分 文件接口》（对账文件格式说明）<br>
            * 测试过程中的如果遇到疑问或问题您可以：1）优先在open平台中查找答案：
            * 							        调试过程中的问题或其他问题请在 https://open.unionpay.com/ajweb/help/faq/list 帮助中心 FAQ 搜索解决方案
            *                             测试过程中产生的6位应答码问题疑问请在https://open.unionpay.com/ajweb/help/respCode/respCodeList 输入应答码搜索解决方案
            *                          2） 咨询在线人工支持： open.unionpay.com注册一个用户并登陆在右上角点击“在线客服”，咨询人工QQ测试支持。
            * 交易说明:1）以后台通知或交易状态查询交易确定交易成功,前台通知不能作为判断成功的标准.
            *       2）交易状态查询交易（Form_6_5_Query）建议调用机制：前台类交易建议间隔（5分、10分、30分、60分、120分）发起交易查询，如果查询到结果成功，则不用再查询。（失败，处理中，查询不到订单均可能为中间状态）。也可以建议商户使用payTimeout（支付超时时间），过了这个时间点查询，得到的结果为最终结果。
            */

            Dictionary<string, string> param = new Dictionary<string, string>();

            //以下信息非特殊情况不需要改动
            param["version"] = "5.0.0";//版本号
            param["encoding"] = "UTF-8";//编码方式
            param["txnType"] = "01";//交易类型
            param["txnSubType"] = "01";//交易子类
            param["bizType"] = "000201";//业务类型
            param["signMethod"] = "01";//签名方法
            param["channelType"] = "08";//渠道类型
            param["accessType"] = "0";//接入类型
            param["frontUrl"] = SDKConfig.FrontUrl;  //前台通知地址      
            param["backUrl"] = SDKConfig.BackUrl;  //后台通知地址
            param["currencyCode"] = "156";//交易币种
            //获取订单信息
            //var order = await _cmsOrderLogic.GetByIdAsync(Request.Form["hiddenOrderId"]);
            //TODO 以下信息需要填写
            param["merId"] = Request.Form["merId"];//商户号，请改自己的测试商户号，此处默认取demo演示页面传递的参数
            //param["hiddenOrderId"] = Request.Form["hiddenOrderId"];
            param["orderId"] = Request.Form["orderId"];//商户订单号，8-32位数字字母，不能含“-”或“_”，此处默认取demo演示页面传递的参数，可以自行定制规则
            param["txnTime"] = Request.Form["txnTime"];//订单发送时间，格式为YYYYMMDDhhmmss，取北京时间，此处默认取demo演示页面传递的参数，参考取法： DateTime.Now.ToString("yyyyMMddHHmmss")
            param["txnAmt"] = (Convert.ToDecimal(0.01 * 100).ToString("#.##"));//交易金额，单位分，此处默认取demo演示页面传递的参数
            if (param["txnAmt"] == "")  // 如果为空或者小于0.01那么默认为0.01
            {
                param["txnAmt"] = "1";
            }
            //param["reqReserved"] = "透传信息";//请求方保留域，透传字段，查询、通知、对账文件中均会原样出现，如有需要请启用并修改自己希望透传的数据

            //TODO 其他特殊用法请查看 pages/api_01_gateway/special_use_purchase.htm

            AcpService.Sign(param, Encoding.UTF8);
            string html = AcpService.CreateAutoFormHtml(SDKConfig.FrontTransUrl, param, Encoding.UTF8);// 将SDKUtil产生的Html文档写入页面，从而引导用户浏览器重定向   
            Response.ContentEncoding = Encoding.UTF8; // 指定输出编码
            return Content(html);
        }

        /// <summary>
        /// 回传界面
        /// </summary>
        /// <returns></returns>
        public ViewResultBase UnionpayPcPayReturn()
        {
            // 使用Dictionary保存参数
            Dictionary<string, string> resData = new Dictionary<string, string>();
            NameValueCollection coll = Request.Form;
            string[] requestItem = coll.AllKeys;

            for (int i = 0; i < requestItem.Length; i++)
            {
                resData.Add(requestItem[i], Request.Form[requestItem[i]]);
            }
            // 返回报文中不包含UPOG,表示Server端正确接收交易请求,则需要验证Server端返回报文的签名
            if (AcpService.Validate(resData, Encoding.UTF8))
            {
                //Response.Write("商户端验证返回报文签名成功\n");
                //获取隐藏订单Id

                string code = resData["respCode"]; //00、A6为成功，其余为失败。其他字段也可按此方式获取。
                var orderId = resData["orderId"]; //订单号

                if (code == "00" || code == "A6")
                {
                    //viewModel.IsSuccess = true;
                    ////更新订单信息
                    //OperateStatus status = await _orderLogic.PaymentSuccessful(new PayTypeOutpu { Id = order.OrderId, Paytype = 0 });
                    //Log.Info("代码位置：" + this.GetType() + ".OrderComplete", "--支付成功,商户订单号：" + viewModel.Code, "银联");
                    //if (status.ResultSign == ResultSign.Error)
                    //{
                    //    Log.Info("代码位置：" + this.GetType() + ".OrderComplete", "--支付失败,商户订单号：" + viewModel.Code, "银联");
                    //    viewModel.IsSuccess = false;
                    //    viewModel.Message = status.Message + ",订单号:" + viewModel.Code;
                    //}
                }
            }
            else
            {
                Response.Write("验证签名失败");
            }
            return View();
        }
        #endregion

        #region 微信

        /// <summary>
        ///     微信支付
        /// </summary>
        /// <returns></returns>
        public ViewResultBase WeiXinPcPay()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var nativePay = new NativePay();
            //订单号
            var code = "XXXX";
            //提示内容
            var body = "支付";
            //价格
            var realPrice = (Convert.ToDecimal(0.1 * 100).ToString("#.##"));
            var url2 = nativePay.GetPayUrl(code, realPrice, body);
            //将url生成二维码图片
            ViewData["imgUrl"] = "/Demo/Pay/MakeQrCode?data=" + HttpUtility.UrlEncode(url2);
            return View();
        }

        /// <summary>
        ///     生成二维码
        /// </summary>
        /// <returns></returns>
        public FileContentResult MakeQrCode()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["data"]))
            {
                var str = Request.QueryString["data"];
                //初始化二维码生成工具
                var qrCodeEncoder = new QRCodeEncoder();
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                qrCodeEncoder.QRCodeVersion = 0;
                qrCodeEncoder.QRCodeScale = 4;
                //将字符串生成二维码图片
                var image = qrCodeEncoder.Encode(str, Encoding.Default);
                //保存为PNG到内存流  
                var ms = new MemoryStream();
                image.Save(ms, ImageFormat.Png);

                //输出二维码图片
                return File(ms.ToArray(), @"image/png");
            }
            return null;
        }

        /// <summary>
        ///     微信回调界面
        /// </summary>
        /// <returns></returns>
        public ViewResultBase WeiXinPayReturn()
        {
            var res = new WxPayData();
            var notifyData = GetNotifyData();

            //检查支付结果中transaction_id是否存在
            if (!notifyData.IsSet("transaction_id"))
            {
                //若transaction_id不存在，则立即返回结果给微信支付后台
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "支付结果中微信订单号不存在");
                Log.Error("代码位置：" + GetType() + ".ResultNotifyPage", "--错误详细信息 " + res.ToXml(), "微信");
            }

            var transactionId = notifyData.GetValue("transaction_id").ToString();

            //查询订单，判断订单真实性
            //if (!QueryOrder(transactionId))
            //{
            //    // Log.Info(this.GetType().ToString(), "查询订单失败");
            //    //若订单查询失败，则立即返回结果给微信支付后台
            //    WxPayData res = new WxPayData();
            //    string outTradeNo = res.GetValue("out_trade_no").ToString();
            //    res.SetValue("return_code", "FAIL");
            //    res.SetValue("return_msg", "订单查询失败");
            //    Log.Error("代码位置：" + this.GetType() + ".ResultNotifyPage", "--支付失败,商户订单号 " + outTradeNo, "微信");

            //}
            ////查询订单成功
            //else
            //{
            var req = new WxPayData();
            req.SetValue("transaction_id", transactionId);
            res = WxPayApi.OrderQuery(req);
            var outTradeNo = res.GetValue("out_trade_no").ToString();
            var totalFee = res.GetValue("total_fee").ToString();
            //更新订单信息
            //var order = await _orderLogic.GetOrderByCode(new IdInput<string>() { Id = outTradeNo });
            //await _orderLogic.PaymentSuccessful(new PayTypeOutpu { Id = order.OrderId, Paytype = 1 });
            res.SetValue("return_code", "SUCCESS");
            res.SetValue("return_msg", "OK");
            Log.Info("代码位置：" + GetType() + ".ResultNotifyPage",
                "--支付成功,商户订单号：" + outTradeNo + "--支付金额：" + totalFee + "微信支付订单号：" + transactionId, "微信");
            //}
            return View();
        }

        /// <summary>
        ///     获取返回值数据
        /// </summary>
        /// <returns></returns>
        public WxPayData GetNotifyData()
        {
            //Log.Info(this.GetType().ToString(), "接收数据");
            //接收从微信后台POST过来的数据
            var s = Request.InputStream;
            int count;
            var buffer = new byte[1024];
            var builder = new StringBuilder();
            while ((count = s.Read(buffer, 0, 1024)) > 0)
            {
                builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
            }
            s.Flush();
            s.Close();
            s.Dispose();
            //转换数据格式并验证签名
            var data = new WxPayData();
            try
            {
                data.FromXml(builder.ToString());
            }
            catch (WxPayException ex)
            {
                //若签名错误，则立即返回结果给微信支付后台
                var res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", ex.Message);
                Response.Write(res.ToXml());
                Response.End();
            }

            return data;
        }

        #endregion

        #region 微信公众号
        JsApiPay jsApiPay = new JsApiPay();
        /// <summary>
        /// 微信公众号
        /// http://www.cnblogs.com/nangong/p/50ab3c60f0d1ae5b551373cf96cd060d.html
        /// https://github.com/Jeffrey9061/WeChat_PublicAccountPay/tree/master/%E5%BE%AE%E4%BF%A1%E6%94%AF%E4%BB%98%E4%B9%8B%E5%85%AC%E4%BC%97%E5%8F%B7%E6%94%AF%E4%BB%98
        /// </summary>
        /// <returns></returns>
        public ViewResultBase WeiXinJsApi()
        {
            if (Session["openid"] == null)
            {
                try
                {
                    //调用【网页授权获取用户信息】接口获取用户的openid和access_token
                    GetOpenidAndAccessToken();

                }
                catch (Exception ex)
                {
                    //Response.Write(ex.ToString());
                    //throw;
                }
            }
            return View();
        }

        /**
        * 
        * 网页授权获取用户基本信息的全部过程
        * 详情请参看网页授权获取用户基本信息：http://mp.weixin.qq.com/wiki/17/c0f37d5704f0b64713d5d2c37b468d75.html
        * 第一步：利用url跳转获取code
        * 第二步：利用code去获取openid和access_token
        * 
        */
        public void GetOpenidAndAccessToken()
        {
            if (Session["code"] != null)
            {
                //获取code码，以获取openid和access_token
                string code = Session["code"].ToString();
                //Log.Debug(this.GetType().ToString(), "Get code : " + code);
                jsApiPay.GetOpenidAndAccessTokenFromCode(code);
            }
            else
            {
                //构造网页授权获取code的URL
                string host = Request.Url.Host;
                string path = Request.Path;
                string redirect_uri = HttpUtility.UrlEncode("http://" + host + path);
                //string redirect_uri = HttpUtility.UrlEncode("http://gzh.lmx.ren");
                WxPayData data = new WxPayData();
                data.SetValue("appid", WxPayConfig.APPID);
                data.SetValue("redirect_uri", redirect_uri);
                data.SetValue("response_type", "code");
                data.SetValue("scope", "snsapi_base");
                data.SetValue("state", "STATE" + "#wechat_redirect");
                string url = "https://open.weixin.qq.com/connect/oauth2/authorize?" + data.ToUrl();
                //Log.Debug(this.GetType().ToString(), "Will Redirect to URL : " + url);
                Session["url"] = url;
            }
        }

        /// <summary>
        /// 通过code换取网页授权access_token和openid的返回数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetWxInfo()
        {
            object objResult = "";
            string strCode = Request.Form["code"];
            if (Session["access_token"] == null || Session["openid"] == null)
            {
                jsApiPay.GetOpenidAndAccessTokenFromCode(strCode);
            }
            string strAccess_Token = Session["access_token"].ToString();
            string strOpenid = Session["openid"].ToString();
            objResult = new { openid = strOpenid, access_token = strAccess_Token };
            return Json(objResult);
        }

        /// <summary>
        /// 获取code
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetCode()
        {
            return Json(Session["url"] != null ? Session["url"].ToString() : "url为空。");
        }

        /// <summary>
        /// 开始进行支付
        /// </summary>
        /// <returns></returns>
        public JsonResult WeiXinJsApiPay()
        {
            object objResult = "";
            string strTotal_fee = Request.Form["totalfee"];
            string strFee = (double.Parse(strTotal_fee) * 100).ToString();

            //若传递了相关参数，则调统一下单接口，获得后续相关接口的入口参数
            jsApiPay.openid = Session["openid"].ToString();
            jsApiPay.total_fee = int.Parse(strFee);

            //JSAPI支付预处理
            try
            {
                string strBody = "南宫萧尘微信支付";//商品描述
                WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult(strBody);
                WxPayData wxJsApiParam = jsApiPay.GetJsApiParameters();//获取H5调起JS API参数，注意，这里引用了官方的demo的方法，由于原方法是返回string的，所以，要对原方法改为下面的代码，代码在下一段

                ModelForOrder aOrder = new ModelForOrder()
                {
                    appId = wxJsApiParam.GetValue("appId").ToString(),
                    nonceStr = wxJsApiParam.GetValue("nonceStr").ToString(),
                    packageValue = wxJsApiParam.GetValue("package").ToString(),
                    paySign = wxJsApiParam.GetValue("paySign").ToString(),
                    timeStamp = wxJsApiParam.GetValue("timeStamp").ToString(),
                    msg = "成功下单,正在接入微信支付."
                };
                objResult = aOrder;
            }
            catch (Exception ex)
            {
                ModelForOrder aOrder = new ModelForOrder()
                {
                    appId = "",
                    nonceStr = "",
                    packageValue = "",
                    paySign = "",
                    timeStamp = "",
                    msg = "下单失败，请重试,多次失败,请联系管理员."
                };
                objResult = aOrder;
            }
            return Json(objResult);
        }

        #endregion

        #region 支付宝支付

        /// <summary>
        ///     支付宝支付
        /// </summary>
        /// <returns></returns>
        public ViewResultBase AlipayPcPay()
        {
            //订单号
            var out_trade_no = "xxxx";
            //配置价格
            var total_fee = "0.1";
            //内容
            var subject = "支付宝";
            var body = string.Empty;
            //把请求参数打包成数组
            var sParaTemp = new SortedDictionary<string, string>
            {
                {"service", Config.service},
                {"partner", Config.partner},
                {"seller_id", Config.seller_id},
                {"_input_charset", Config.input_charset.ToLower()},
                {"payment_type", Config.payment_type},
                {"notify_url", Config.notify_url},
                {"return_url", Config.return_url},
                {"anti_phishing_key", Config.anti_phishing_key},
                {"exter_invoke_ip", Config.exter_invoke_ip},
                {"out_trade_no", out_trade_no},
                {"subject", subject},
                {"total_fee", total_fee},
                {"body", body},
                {"goods_type", "0"},
                {"extra_common_param", ""}
            };

            //建立请求
            var sHtmlText = Submit.BuildRequest(sParaTemp, "get", "确认");
            Response.Write(sHtmlText);
            return View();
        }

        /// <summary>
        ///     支付成功后,跳转页面
        /// </summary>
        /// <returns></returns>
        public ViewResultBase AlipayPayReturn()
        {
            var sPara = GetRequestGet();
            if (sPara.Count > 0) //判断是否有带返回参数
            {
                //通知ID
                var notify_id = Request.QueryString["notify_id"];
                //签名
                var sign = Request.QueryString["sign"];
                var verifyResult = new Notify().Verify(sPara, notify_id, sign);
                if (verifyResult) //验证成功
                {
                    //商户订单号
                    var out_trade_no = Request.QueryString["out_trade_no"];
                    //交易状态
                    var trade_status = Request.QueryString["trade_status"];
                    //支付金额
                    var total_fee = decimal.Parse(Request.QueryString["total_fee"]);
                    //支付宝交易号
                    var trade_no = decimal.Parse(Request.QueryString["trade_no"]);
                    //Log.Info(this.GetType().ToString(), total_fee.ToString());
                    //交易完成
                    if ("TRADE_FINISHED".Equals(trade_status) || "TRADE_SUCCESS".Equals(trade_status))
                    {
                        //OperateStatus status = await _orderLogic.PaymentSuccessful(new PayTypeOutpu { Id = order.OrderId, Paytype = 2 });
                        //viewModel.IsSuccess = true;
                        //viewModel.Type = order.Type;
                        //viewModel.Code = out_trade_no;
                        //viewModel.Message = status.Message;
                        //Log.Info("代码位置：" + this.GetType() + ".PayAlipayReturn", "--支付成功,商户订单号：" + out_trade_no + "支付宝交易号：" + trade_no + "--支付金额：" + total_fee, "支付宝");
                    }
                }
                else //验证失败
                {
                    Response.Write("无传回参数");
                }
            }
            else
            {
                Response.Write("无参数返回");
            }
            return View();
        }

        /// <summary>
        ///     获取支付宝GET过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public SortedDictionary<string, string> GetRequestGet()
        {
            var sArray = new SortedDictionary<string, string>();
            var keys = Request.QueryString.AllKeys;
            foreach (var key in keys)
            {
                sArray.Add(key, Request.QueryString[key]);
            }
            return sArray;
        }

        #endregion
    }
}