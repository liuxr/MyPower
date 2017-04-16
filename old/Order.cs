using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace MyPay
{
    public class Order
    {
      public   int ID { get; set; }

        /// <summary>
        /// 创建时间 2016.10.24 12:12
        /// </summary>
        private string createDate = string.Empty;

        /// <summary>
        /// 名称  支付宝支付 
        /// </summary>
        private string name = string.Empty;

        private string orderNo = string.Empty;
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string OrderNo
        {
            get { return orderNo; }
            set { orderNo = value; }
        }

        private string tradeNo = string.Empty;
        /// <summary>
        /// 交易号
        /// </summary>
        public string TradeNo
        {
            get { return tradeNo; }
            set { tradeNo = value; }
        }

        private string batchNo = string.Empty;
        /// <summary>
        /// 流水号
        /// </summary>
        public string BatchNo
        {
            get { return batchNo; }
            set { batchNo = value; }
        }

        /// <summary>
        /// 对方（收款方）
        /// </summary>
        private string payee = string.Empty;

        /// <summary>
        /// 金额
        /// </summary>
        private string  amount = "0";

        /// <summary>
        /// 状态 交易成功
        /// </summary>
        private string state = string.Empty;

        public string CreateDate
        {
            get
            {
                return createDate;
            }

            set
            {
                createDate = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public string Payee
        {
            get
            {
                return payee;
            }

            set
            {
                payee = value;
            }
        }

        public string Amount
        {
            get
            {
                return amount;
            }

            set
            {
                amount = value;
            }
        }

        public string State
        {
            get
            {
                return state;
            }

            set
            {
                state = value;
            }
        }

        /// <summary>
        /// 上传状态
        /// </summary>
        public string UpLoadState { get; set; }

        /// <summary>
        /// 上传次数
        /// </summary>
        public int UpLoadCount { get; set; }

        /// <summary>
        /// 上传错误
        /// </summary>
        public string UpLoadError { get; set; }

        /// <summary>
        /// 最后上传时间
        /// </summary>
        public string UpLoadDate { get; set; }

        public string Account
        {
            get
            {
                return account;
            }

            set
            {
                account = value;
            }
        }

        private string account;

        public override bool Equals(object obj)
        {
            if (obj == null) throw new ArgumentNullException { };
            Order o = (Order)obj;
            if (o == null) throw new NotSupportedException { };
            return this.OrderNo == o.OrderNo && this.BatchNo == o.BatchNo && this.TradeNo == o.TradeNo;
        }

        public override int GetHashCode()
        {
            return (this.OrderNo + this.TradeNo + this.BatchNo).GetHashCode();
        }
    }
}
