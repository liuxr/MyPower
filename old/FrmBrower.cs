/*
 *1.SQLlite 存储问题
 *2.
 * 
 * 
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Threading.Tasks;

namespace MyPay
{
    /// <summary>
    /// 更新登录名
    /// </summary>
    /// <param name="userName"></param>
    public delegate void UpdateName(string userName);
    public delegate void GetContext(List<Order> newOrders, bool canWatch = false);
    public delegate void UpdateHtml(string html);
    public partial class FrmBrower : Form
    {
        /// <summary>
        /// 订单服务
        /// </summary>
        private OrderServer orderServer = new OrderServer();

        /// <summary>
        /// 原来的订单列表
        /// </summary>
        private List<Order> OldList = new List<Order>();

        /// <summary>
        /// 新的订单列表
        /// </summary>
        private List<Order> NewList = new List<Order>();

        /// <summary>
        /// 订单操作类
        /// </summary>
        private OrderDAL orderDal = new OrderDAL();

        /// <summary>
        /// 获取内容
        /// </summary>
        public GetContext onGetContext;

        /// <summary>
        /// 更新用户名
        /// </summary>
        public UpdateName onUpdateName;

        public UpdateHtml onUpdaeHtml;

        /// <summary>
        /// Html元素操作类
        /// </summary>
        private Element element = new Element();

        /// <summary>
        /// 高级版的访问页面地址
        /// </summary>
        private string url = "https://consumeprod.alipay.com/record/advanced.htm";

        /// <summary>
        /// 登录界面Url
        /// </summary>
        private string loginUrl = "https://auth.alipay.com/login/index.htm?goto=https%3A%2F%2Fconsumeprod.alipay.com%2Frecord%2Fadvanced.htm";

        private string loginIndexUrl = "https://authzui.alipay.com/login/index.htm";

        private string NavigatingUrl = "";

        /// <summary>
        /// 登录名
        /// </summary>
        private string loginName = string.Empty;

        /// <summary>
        /// 定时器，进行定时采集
        /// </summary>
        private Timer timer = null;

        /// <summary>
        /// 是否监视
        /// </summary>
        private bool isWatch = false;

        /// <summary>
        /// 是否监视
        /// </summary>
        public bool IsWatch
        {
            get
            {
                return isWatch;
            }

            set
            {
                isWatch = value;
            }
        }
        /// <summary>
        /// 是否可以监视
        /// </summary>
        public bool CanWatch
        {
            get
            {
                return canWatch;
            }

            set
            {
                canWatch = value;
            }
        }

        private bool canWatch = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmBrower()
        {
            InitializeComponent();
            webBrowser1.IsWebBrowserContextMenuEnabled = false;
            webBrowser1.ScriptErrorsSuppressed = false;
            webBrowser1.WebBrowserShortcutsEnabled = false;

            timer = new Timer();
            timer.Interval = 20 * 1000;
            timer.Tick += Timer_Tick;
        }


        int i = 0;

        /// <summary>
        /// 定时访问地址
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            //正在监视，但是Url不是采集页面
            if (isWatch && !webBrowser1.Url.AbsoluteUri.ToLower().Equals(url))
            {
                ReInitData();
                MessageBox.Show("登录页已经失效，请重新登录");
                return;
            }
            webBrowser1.Navigate(url);
        }

        /// <summary>
        /// 重新初始化数据
        /// </summary>
        private void ReInitData()
        {
           // Stop();
            OldList.Clear();
            NewList.Clear();
            isWatch = false;
        }

        /// <summary>
        /// 监视开始
        /// </summary>
        public void Start()
        {
            timer.Start();
            isWatch = true;
        }

        /// <summary>
        /// 监视结束
        /// </summary>
        public void Stop()
        {
            timer.Stop();
            isWatch = false;
        }

        /// <summary>
        /// 设置采集时间间隔
        /// </summary>
        /// <param name="time"></param>
        public void SetInterval(int time)
        {
            timer.Interval = time * 1000;
        }

        /// <summary>
        /// 进入页面后直接访问地址
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate(url);
        }

        /// <summary>
        /// 访问地址完成后的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //请求的URL(全部转换为小写)
            string requestUrl = e.Url.AbsoluteUri.ToLower();
            if (NavigatingUrl.Equals(loginIndexUrl) && !requestUrl.Equals(loginIndexUrl))
            {
                if (onUpdateName != null)
                {
                    onUpdateName(loginName);
                }
            }

            //需要采集的页面
            if (!url.Equals(requestUrl))
                return;

            LoadInfo();
        }


        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            string requestUrl = e.Url.AbsoluteUri.ToLower();
            NavigatingUrl = requestUrl;
            //获取登录名并通知主界面更新用户名
            UpdateName(requestUrl);
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
           // string requestUrl = e.Url.AbsoluteUri.ToLower();
        }

        private void UpdateName(string requestUrl)
        {
            //如果是登录界面
            if (!requestUrl.Equals(loginIndexUrl.ToLower()))
                return;
            //获取登录界面的名称
            //  "J-input-user"
            HtmlElement htmlElement = element.GetElement_Id(webBrowser1, "J-input-user");
            if (htmlElement == null)
                return;
            loginName = htmlElement.GetAttribute("value");
        }

        private void LoadInfo()
        {
            canWatch = true;
            //获取页面中的Html字符串
            string html = GetHtmlString();
            //判断是否有订阅更新Html，如果有就去更新操作
            if (onUpdaeHtml != null)
            {
                onUpdaeHtml(html);
            }

            //获取Html中的订单列表
            List<Order> list = GetTableContent();

            //如果没有订单，不做任何操作
            if (list.Count <= 0)
                return;
            //最新订单
            List<Order> newOrders = new List<MyPay.Order>();
            //通过各种比较获取最新订单
            newOrders = GetOrderList(list);
            //判断是否满足条件
            if (newOrders == null || newOrders.Count <= 0)
                return;
            //进行倒序
            newOrders.Reverse();
            //条件满足以后做插入数据库操作
            //orderDal.Insert(newOrders);

            Task.Factory.StartNew(new Action(() =>
            {
                Scny(newOrders);
                //同步更新界面
                if (onGetContext != null)
                    onGetContext(newOrders, canWatch);
            }));
        }

        /// <summary>
        /// 文档流转文本
        /// </summary>
        /// <returns></returns>
        public string GetHtmlString()
        {
            string html = string.Empty;
            using (Stream st = webBrowser1.DocumentStream)
            {
                StreamReader sr = new StreamReader(st, Encoding.GetEncoding("gbk"));
                html = sr.ReadToEnd();
            }
            return html;
        }

        /// <summary>
        /// 获取Table中内容，组织成集合
        /// </summary>
        /// <returns>返回订单集合</returns>
        private List<Order> GetTableContent()
        {
            //定义订单集合
            List<Order> list = new List<Order>();
            //获取Html中的Table
            HtmlElement table = element.GetElement_Id(webBrowser1, "tradeRecordsIndex");
            //html中没有table或者子元素小于3就直接返回
            if (table == null || table.Children.Count < 3)
                return list;
            //获取Table中第三个元素tr
            HtmlElement child = table.Children[2];
            //遍历所有的tr元素将td中的元素记录到集合中
            foreach (HtmlElement tr in child.Children)
            {
                Order order = new Order();
                order.Account = loginName;
                HtmlElementCollection tds = tr.Children;
                //创建时间
                order.CreateDate = Convert.ToDateTime(Replace(tds[0].InnerText)).ToString("yyyy-MM-dd hh:mm");
                //名称
                order.Name = Replace(tds[2].InnerText);
                //分割获取商户订单号和交易号
                string no = tds[3].InnerText;
                string[] strs = no.Split(new char[] { '|', ':' });
                if (strs.Length == 2)
                {
                    //流水号
                    order.BatchNo = Replace( strs[1]);
                }
                else if (strs.Length == 4)
                {
                    //商户订单号
                    order.OrderNo = Replace( strs[1]);
                    //交易号
                    order.TradeNo = Replace( strs[3]);
                }

                //对方
                order.Payee = Replace(tds[4].InnerText);
                //金额
                order.Amount = Replace(tds[5].InnerText);
                //状态
                order.State = Replace(tds[7].InnerText);
                list.Add(order);
            }
            return list;
        }

        private void FrmBrower_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        /// <summary>
        /// 特殊字符串替换
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string Replace(string input) {
            return input.Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Trim();
        }

        /// <summary>
        /// 获取需要操作的List
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<Order> GetOrderList(List<Order> list)
        {
            Order m = orderDal.GetMaxOrder(loginName);
            if (m == null)
            {
                //没有获取到数据，说明之前没有数据，可能是第一次
                return list;
            }
            //1.判断是否在采集的数据里面,如果不存在就全部插入
            if (!list.Contains(m, new OrderCompare()))
            {
                return list;
            }
            //2.已存在，就找到索引,将索引之后的数据全部返回
            int index = -1;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Equals(m))
                {
                    index = i;
                    break;
                }
            }
            list.RemoveRange(index, list.Count - index);
            return list;
        }

        /// <summary>
        /// 异步提交采集的数据
        /// </summary>
        /// <param name="list"></param>
        private void Scny(List<Order> list)
        {
            list.ForEach(model =>
            {
                orderDal.Insert(model);
                RequestResult result = orderServer.Submit(model);
                if (result != null)
                {
                    //更新日期
                    model.UpLoadDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                    //上传次数增加1
                    model.UpLoadCount = 1;
                    if (result.status == 1)
                    {   //成功
                        model.UpLoadState = "成功";
                    }
                    else
                    {
                        //失败
                        model.UpLoadState = "失败";
                        model.UpLoadError = result.msg;
                    }
                    orderDal.Insert(model);
                }
            });
        }

        private void A(List<Order> list, List<Order> newOrders)
        {

            if (isWatch)//进入监视状态
            {
                NewList = list;
                //比较，前后两个集合的差集
                newOrders = NewList.Except(OldList, new OrderCompare()).ToList();
                //采集到数据了
                if (newOrders.Count > 0)
                {
                    orderDal.Insert(newOrders);
                    //foreach (Order model in newOrders) {
                    //    orderDal.Insert(model);
                    //    //orderServer.Submit(model);
                    //}

                    OldList = NewList;
                }
            }
            else
            {
                OldList = list;//未进入监视状态
            }
        }



        //自动登录
        private void AutoLogin()
        {
            //获取登录界面的名称
            //  "J-input-user"
            HtmlElement htmlElement = element.GetElement_Id(webBrowser1, "J-input-user");
            if (htmlElement == null)
                return;
            htmlElement.SetAttribute("value", "liu.xr90@163.com");

            htmlElement = element.GetElement_Id(webBrowser1, "password_rsainput");
            if (htmlElement == null)
                return;
            htmlElement.SetAttribute("value", "bbbbbb");

            //"J-login-btn" "login"
            htmlElement = element.GetElement_Id(webBrowser1, "login");
            if (htmlElement == null)
                return;
            element.Btn_click(htmlElement, "submit");


            //htmlElement = element.GetElement_Id(webBrowser1, "J-checkcodeIcon");
            //if (htmlElement == null)
            //    return;
            // htmlElement.Click += HtmlElement_Click;
        }

        private void HtmlElement_Click(object sender, HtmlElementEventArgs e)
        {
            throw new NotImplementedException();
        }
    }

}
