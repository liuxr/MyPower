using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPay
{
    /// <summary>
    /// 服务器请求结果
    /// </summary>
    public class RequestResult
    {
        public string m { get; set; }

        public string c { get; set; }

        public string a { get; set; }

        public int status { get; set; }

        public string msg { get; set; }
    }
}
