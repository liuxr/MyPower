using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace MyPay
{
    /// <summary>
    /// 订单提交到服务器服务
    /// </summary>
    public class OrderServer
    {
        Crypt3Des crypt = null;
        public OrderServer()
        {
            crypt = new Crypt3Des(Request.Key);
        }

        /// <summary>
        /// 提交数据
        /// </summary>
        /// <param name="model"></param>
        public RequestResult Submit(Order model)
        {
            //根据订单转换为Json数据
            string json = OrderConvertJson(model);
            //对提交的数据进行3DES加密处理            
            string inputparam = crypt.Encrypt(json);
            //组合成参数
            string para = "m=Api&c=Alipay&a=alipaylog&inputparam=" + inputparam;
            //Post数据
            string result = Request.Get(string.Format("{0}?{1}", Request.Url, para));

            return JsonConvert.DeserializeObject<RequestResult>(result);
        }

        /// <summary>
        /// 订单转Json字符串
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string OrderConvertJson(Order model)
        {
            StringBuilder json = new StringBuilder();
            json.Append("{");
            json.Append(string.Format("\"title\":\"{0}\"", model.Name));
            json.Append(string.Format(",\"orderno\":\"{0}\"", model.OrderNo));
            json.Append(string.Format(",\"tradeno\":\"{0}\"", model.TradeNo));
            json.Append(string.Format(",\"serialno\":\"{0}\"", model.BatchNo));
            json.Append(string.Format(",\"money\":\"{0}\"", model.Amount));
            json.Append(string.Format(",\"tradetime\":\"{0}\"", model.CreateDate));
            json.Append(string.Format(",\"tradestatus\":\"{0}\"", model.State));
            json.Append(string.Format(",\"nickname\":\"{0}\"", model.Payee));
            json.Append(string.Format(",\"account\":\"{0}\"", model.Account));
            json.Append("}");
            return json.ToString();
        }
    }
}
