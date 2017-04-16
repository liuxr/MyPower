/* 
   ======================================================================== 
    文 件 名：     
	功能描述：                
    作    者:	Cumin           
    创建时间： 	2016/11/2 15:03:52
	版    本:	V1.0.0
   ------------------------------------------------------------------------
	历史更新纪录
   ------------------------------------------------------------------------
	版    本：           修改时间：           修改人：          
	修改内容：
   ------------------------------------------------------------------------
	Copyright (C) 2016   北京荣大科技有限公司
   ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace MyPay
{
    public class Request
    {
        /// <summary>
        /// 密钥
        /// </summary>
        public static string Key = "keyjingcai2l.8ke520lhJin.Cai@ss283.229808e";

        /// <summary>
        /// 请求地址
        /// </summary>
        public static string Url = "http://520jingcai.com/index.php";

        ///<summary>
        ///采用https协议访问网络
        ///</summary>
        ///<param name="URL">url地址</param>
        ///<param name="strPostdata">发送的数据</param>
        ///<returns></returns>
        public static string OpenReadWithHttps(string URL, string strPostdata, string strEncoding="utf-8")
        {
            Encoding encoding = Encoding.Default;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "post";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] buffer = encoding.GetBytes(strPostdata);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding(strEncoding)))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Get方式提交数据
        /// </summary>
        /// <param name="url">url+param</param>
        /// <returns></returns>
        public static string Get(string url)
        {
            try
            {
                // string strURL = "http://520jingcai.com/index.php?m=Api&c=Alipay&a=alipaylog&inputparam=IWKZZd-XuwSgXLyOweGOGtAKZqr9kwsSsdD9nHz88raSqHCYRqfnn4zVGTuv407XE7RcBwDIHAv7zJ4udOvDSC3LosQQ86yOQHQ1HTrReM-zvWYmSrWs4N6SW-ffXcODOX9lx6exLU64Me021TmmSHEj3J--UskbSaAcvJm4D9kjJxARJl0NaBCvtdyZFmsuuqK5vGaHEBFXwFH05ApGp-iCXXjh98VpBeKmAlwkgZXf27fp4_Gg5PwOvEDiwWcOH911xhPl1IiC-_e8ynO5AZ-8I--UjWeoiUbZUYooM6j6em8dlBKz1c4v-dUE0vDeB8I2aJWx0yL4GwYl6Gg-j5avabb29Kn9SG1MMsbKhVNKj9Q3cIr2Cekjdyilb4HqsdD9nHz88rZvyXRRvASgAy6oSMmv87nB_k8xSFNIDKEg6Fg07HgQfS_6OjZ-8-DA";
                System.Net.HttpWebRequest request;
                // 创建一个HTTP请求
                request = (System.Net.HttpWebRequest)WebRequest.Create(url);
                //request.Method="get";
                System.Net.HttpWebResponse response;
                response = (System.Net.HttpWebResponse)request.GetResponse();
                System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string responseText = myreader.ReadToEnd();
                myreader.Close();
                return Unicode2String(responseText);
            }
            catch (Exception ex)
            {
                return "{\"m\":\"Api\",\"c\":\"Alipay\",\"a\":\"alipaylog\",\"status\":0,\"msg\":\"网络异常\"}";
            }
        }

        /// <summary>
        /// Post提交数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Post(string url,string data) {
           // string strURL = "http://localhost/WinformSubmit.php";
            System.Net.HttpWebRequest request;
            request = (System.Net.HttpWebRequest)WebRequest.Create(url);
            //Post请求方式
            request.Method = "POST";
            // 内容类型
            request.ContentType = "application/x-www-form-urlencoded";
            // 参数经过URL编码
            //string paraUrlCoded = System.Web.HttpUtility.UrlEncode("keyword");
            //paraUrlCoded += "=" + System.Web.HttpUtility.UrlEncode("多月");
            byte[] payload;
            //将URL编码后的字符串转化为字节
            payload = System.Text.Encoding.UTF8.GetBytes(data);
            //设置请求的 ContentLength 
            request.ContentLength = payload.Length;
            //获得请 求流
            System.IO.Stream writer = request.GetRequestStream();
            //将请求参数写入流
            writer.Write(payload, 0, payload.Length);
            // 关闭请求流
            writer.Close();
            System.Net.HttpWebResponse response;
            // 获得响应流
            response = (System.Net.HttpWebResponse)request.GetResponse();
            System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string responseText = myreader.ReadToEnd();
            myreader.Close();
            return responseText;
        }

        public void Test() {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param["title"] = "交易名称";
            param["orderno"] = "商户订单号";
            param["tradeno"] = "支付宝交易单号";
            param["serialno"] = "流水号";
            param["money"] = "0.01";
            param["tradetime"] = "2016-10012 11：33：22";
            param["tradestatus"] = "交易成功";
            param["nickname"] = "支付宝名称";
            param["account"] = "wangjingcan@126.com";


            //m=Api&c=Alipay&a=alipaylog&inputparam
            string para = string.Empty;
            para = "m=Api&c=Alipay&a=alipaylog&inputparam=";
           

        }

        /// <summary>  
        /// <summary>  
        /// 字符串转Unicode  
        /// </summary>  
        /// <param name="source">源字符串</param>  
        /// <returns>Unicode编码后的字符串</returns>  
        public static string String2Unicode(string source)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(source);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i += 2)
            {
                stringBuilder.AppendFormat("\\u{0}{1}", bytes[i + 1].ToString("x").PadLeft(2, '0'), bytes[i].ToString("x").PadLeft(2, '0'));
            }
            return stringBuilder.ToString();
        }

        /// <summary>  
        /// Unicode转字符串  
        /// </summary>  
        /// <param name="source">经过Unicode编码的字符串</param>  
        /// <returns>正常字符串</returns>  
        public static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                         source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }
    }
}
