using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPay
{
    /// <summary>
    /// 订单比较器
    /// </summary>
    public class OrderCompare : IEqualityComparer<Order>
    {
        public bool Equals(Order x, Order y)
        {
            return x.OrderNo == y.OrderNo && x.BatchNo == y.BatchNo && x.TradeNo == y.TradeNo;
        }

        public int GetHashCode(Order obj)
        {
            return (obj.OrderNo + obj.TradeNo + obj.BatchNo).GetHashCode();
        }
    }
}
