using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MyPay
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //string strURL = "http://520jingcai.com/index.php?m=Api&c=Alipay&a=alipaylog&inputparam=IWKZZd-XuwSgXLyOweGOGtAKZqr9kwsSsdD9nHz88raSqHCYRqfnn4zVGTuv407XE7RcBwDIHAv7zJ4udOvDSC3LosQQ86yOQHQ1HTrReM-zvWYmSrWs4N6SW-ffXcODOX9lx6exLU64Me021TmmSHEj3J--UskbSaAcvJm4D9kjJxARJl0NaBCvtdyZFmsuuqK5vGaHEBFXwFH05ApGp-iCXXjh98VpBeKmAlwkgZXf27fp4_Gg5PwOvEDiwWcOH911xhPl1IiC-_e8ynO5AZ-8I--UjWeoiUbZUYooM6j6em8dlBKz1c4v-dUE0vDeB8I2aJWx0yL4GwYl6Gg-j5avabb29Kn9SG1MMsbKhVNKj9Q3cIr2Cekjdyilb4HqsdD9nHz88rZvyXRRvASgAy6oSMmv87nB_k8xSFNIDKEg6Fg07HgQfS_6OjZ-8-DA";
            //string result = Request.Get(strURL);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain1());
        }


        private static void Test5() {
            string strURL = "http://520jingcai.com/index.php?m=Api&c=Alipay&a=alipaylog&inputparam=IWKZZd-XuwSgXLyOweGOGtAKZqr9kwsSsdD9nHz88raSqHCYRqfnn4zVGTuv407XE7RcBwDIHAv7zJ4udOvDSC3LosQQ86yOQHQ1HTrReM-zvWYmSrWs4N6SW-ffXcODOX9lx6exLU64Me021TmmSHEj3J--UskbSaAcvJm4D9kjJxARJl0NaBCvtdyZFmsuuqK5vGaHEBFXwFH05ApGp-iCXXjh98VpBeKmAlwkgZXf27fp4_Gg5PwOvEDiwWcOH911xhPl1IiC-_e8ynO5AZ-8I--UjWeoiUbZUYooM6j6em8dlBKz1c4v-dUE0vDeB8I2aJWx0yL4GwYl6Gg-j5avabb29Kn9SG1MMsbKhVNKj9Q3cIr2Cekjdyilb4HqsdD9nHz88rZvyXRRvASgAy6oSMmv87nB_k8xSFNIDKEg6Fg07HgQfS_6OjZ-8-DA";
            System.Net.HttpWebRequest request;
            // 创建一个HTTP请求
            request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
            //request.Method="get";
            System.Net.HttpWebResponse response;
            response = (System.Net.HttpWebResponse)request.GetResponse();
            System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string responseText = myreader.ReadToEnd();
            myreader.Close();

            MessageBox.Show(Unicode2String(responseText));
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

        private static void Test4() {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Request.Url);

            request.Method = "POST";
            request.Referer = Request.Url;
            request.Accept = "*/*";
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1;  Embedded Web Browser from: http://bsalsa.com/; .NET CLR 3.0.04506.30; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";

            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-cn");
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");

            Console.WriteLine("Request Headers:" + request.Headers);

            string postData = "m=Api&c=Alipay&a=alipaylog&inputparam=IWKZZd-XuwSgXLyOweGOGtAKZqr9kwsSsdD9nHz88raSqHCYRqfnn4zVGTuv407XE7RcBwDIHAv7zJ4udOvDSC3LosQQ86yOQHQ1HTrReM-zvWYmSrWs4N6SW-ffXcODOX9lx6exLU64Me021TmmSHEj3J--UskbSaAcvJm4D9kjJxARJl0NaBCvtdyZFmsuuqK5vGaHEBFXwFH05ApGp-iCXXjh98VpBeKmAlwkgZXf27fp4_Gg5PwOvEDiwWcOH911xhPl1IiC-_e8ynO5AZ-8I--UjWeoiUbZUYooM6j6em8dlBKz1c4v-dUE0vDeB8I2aJWx0yL4GwYl6Gg-j5avabb29Kn9SG1MMsbKhVNKj9Q3cIr2Cekjdyilb4HqsdD9nHz88rZvyXRRvASgAy6oSMmv87nB_k8xSFNIDKEg6Fg07HgQfS_6OjZ-8-DA";

            // Create POST data and convert it to a byte array. 
            //string postData = "This is a test that posts this string to a Web server.";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            // Set the ContentType property of the WebRequest. 
            request.ContentType = "application/x-www-form-urlencoded";
            // Set the ContentLength property of the WebRequest. 
            request.ContentLength = byteArray.Length;
            // Get the request stream. 
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream. 
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object. 
            dataStream.Close();


            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream());

            Console.WriteLine("Response body:" + reader.ReadToEnd());
        }


        private static void Test3()
        {
            string postUrl = Request.Url;
            string postData = "m=Api&c=Alipay&a=alipaylog&inputparam=IWKZZd-XuwSgXLyOweGOGtAKZqr9kwsSsdD9nHz88raSqHCYRqfnn4zVGTuv407XE7RcBwDIHAv7zJ4udOvDSC3LosQQ86yOQHQ1HTrReM-zvWYmSrWs4N6SW-ffXcODOX9lx6exLU64Me021TmmSHEj3J--UskbSaAcvJm4D9kjJxARJl0NaBCvtdyZFmsuuqK5vGaHEBFXwFH05ApGp-iCXXjh98VpBeKmAlwkgZXf27fp4_Gg5PwOvEDiwWcOH911xhPl1IiC-_e8ynO5AZ-8I--UjWeoiUbZUYooM6j6em8dlBKz1c4v-dUE0vDeB8I2aJWx0yL4GwYl6Gg-j5avabb29Kn9SG1MMsbKhVNKj9Q3cIr2Cekjdyilb4HqsdD9nHz88rZvyXRRvASgAy6oSMmv87nB_k8xSFNIDKEg6Fg07HgQfS_6OjZ-8-DA";
            // Create a request using a URL that can receive a post.  
            WebRequest request = WebRequest.Create(postUrl);
            // Set the Method property of the request to POST. 
            request.Method = "POST";
            // Create POST data and convert it to a byte array. 
            //string postData = "This is a test that posts this string to a Web server.";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            // Set the ContentType property of the WebRequest. 
            request.ContentType = "application/x-www-form-urlencoded";
            // Set the ContentLength property of the WebRequest. 
            request.ContentLength = byteArray.Length;
            // Get the request stream. 
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream. 
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object. 
            dataStream.Close();
            // Get the response. 
            WebResponse response = request.GetResponse();
            // Display the status. 
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server. 
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access. 
            StreamReader reader = new StreamReader(dataStream);
            // Read the content. 
            string responseFromServer = reader.ReadToEnd();
            // Display the content. 
            Console.WriteLine(responseFromServer);
            // Clean up the streams. 
            reader.Close();
            dataStream.Close();
            response.Close();

        }


        private static string PostWebRequest(string postUrl, string paramData, Encoding dataEncode)
        {
            string ret = string.Empty;
            try
            {
                postUrl = Request.Url;
                paramData = "m=Api&c=Alipay&a=alipaylog&inputparam=IWKZZd-XuwSgXLyOweGOGtAKZqr9kwsSsdD9nHz88raSqHCYRqfnn4zVGTuv407XE7RcBwDIHAv7zJ4udOvDSC3LosQQ86yOQHQ1HTrReM-zvWYmSrWs4N6SW-ffXcODOX9lx6exLU64Me021TmmSHEj3J--UskbSaAcvJm4D9kjJxARJl0NaBCvtdyZFmsuuqK5vGaHEBFXwFH05ApGp-iCXXjh98VpBeKmAlwkgZXf27fp4_Gg5PwOvEDiwWcOH911xhPl1IiC-_e8ynO5AZ-8I--UjWeoiUbZUYooM6j6em8dlBKz1c4v-dUE0vDeB8I2aJWx0yL4GwYl6Gg-j5avabb29Kn9SG1MMsbKhVNKj9Q3cIr2Cekjdyilb4HqsdD9nHz88rZvyXRRvASgAy6oSMmv87nB_k8xSFNIDKEg6Fg07HgQfS_6OjZ-8-DA";
                byte[] byteArray = dataEncode.GetBytes(paramData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";
                webReq.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";


                webReq.ContentLength = byteArray.Length;
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return ret;
        }

        private static void Test2()
        {
            string strURL = Request.Url;
            System.Net.HttpWebRequest request;
            request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
            //Post请求方式
            request.Method = "POST";
            // 内容类型
            request.ContentType = "application/x-www-form-urlencoded";
            // 参数经过URL编码
            string paraUrlCoded = "m=Api&c=Alipay&a=alipaylog&inputparam=IWKZZd-XuwSgXLyOweGOGtAKZqr9kwsSsdD9nHz88raSqHCYRqfnn4zVGTuv407XE7RcBwDIHAv7zJ4udOvDSC3LosQQ86yOQHQ1HTrReM-zvWYmSrWs4N6SW-ffXcODOX9lx6exLU64Me021TmmSHEj3J--UskbSaAcvJm4D9kjJxARJl0NaBCvtdyZFmsuuqK5vGaHEBFXwFH05ApGp-iCXXjh98VpBeKmAlwkgZXf27fp4_Gg5PwOvEDiwWcOH911xhPl1IiC-_e8ynO5AZ-8I--UjWeoiUbZUYooM6j6em8dlBKz1c4v-dUE0vDeB8I2aJWx0yL4GwYl6Gg-j5avabb29Kn9SG1MMsbKhVNKj9Q3cIr2Cekjdyilb4HqsdD9nHz88rZvyXRRvASgAy6oSMmv87nB_k8xSFNIDKEg6Fg07HgQfS_6OjZ-8-DA";
            byte[] payload;
            //将URL编码后的字符串转化为字节
            payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
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
            MessageBox.Show(responseText);

        }

        private static void Test()
        {

            System.Text.Encoding utf8 = System.Text.Encoding.UTF8;

            //key为abcdefghijklmnopqrstuvwx的Base64编码

            byte[] bytes = Encoding.Default.GetBytes(Request.Key);
            string encode = Convert.ToBase64String(bytes);
            byte[] a = Convert.FromBase64String(encode);


            byte[] key = Convert.FromBase64String("YWJjZGVmZ2hpamtsbW5vcHFyc3R1dnd4");
            byte[] iv = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };      //当模式为ECB时，IV无用
            byte[] data = utf8.GetBytes("中国ABCabc123");

            System.Console.WriteLine("ECB模式:");
            byte[] str1 = Crypt3Des.Des3EncodeECB(key, iv, data);
            // byte[] str2 = Des3.Des3DecodeECB(key, iv, str1);
            System.Console.WriteLine(Convert.ToBase64String(str1));
            // System.Console.WriteLine(System.Text.Encoding.UTF8.GetString(str2));
        }
    }
}
