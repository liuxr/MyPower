using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace MyPower
{
    public class MyTest
    {
        private string urlTemp = "http://so.baiten.cn/results?q=pd%253A%2528{0}%2529%2520and%2520aa%253A%2528%25u5317%25u4EAC%25u5E02%2529&type=14&s=0&law=0&v=s";
        public void Get()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format(urlTemp, 2017));
            request.CookieContainer = new CookieContainer();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            
            using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("windows-1251")))
            {
                string html = sr.ReadToEnd();
               // webBrowser1.DocumentText = doc.DocumentNode.OuterHtml;
            }
        }

    }
}
