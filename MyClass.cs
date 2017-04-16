using HtmlAgilityPack;
using MyPay;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPower
{
    public delegate void Collected(string result);
    public delegate void UpdateCount(int count);
    public delegate void UpdateTime(long time);
    public delegate void Complate(string guid,string url);
    public class MyClass
    {
        public Collected onCollected = null;
        public UpdateCount onUpdateCount = null;
        public UpdateTime onUpdateTime = null;
        public Complate onComplate = null;

        private string Year = "2017";

        private string urlTemp = "http://so.baiten.cn/results?q=ad%253A%2528{0}%2529%2520and%2520aa%253A%2528%25u5317%25u4EAC%25u5E02%2529&type=14&s=0&law=0&v=l";

        public void Run(string year)
        {
            this.Year = year;
            try
            {
                Stopwatch Watch = new Stopwatch();
                Watch.Start();
                //组织url
                string url = string.Format(urlTemp, Year);
                //获取总数量
                int totalCount = Total(url);
                if (onUpdateCount != null)
                {
                    onUpdateCount(totalCount);
                }
                //判断总数量,小于10000条直接采集
                if (totalCount <= 10000)
                {
                    //计算多少页
                    int pageCount = Convert.ToInt32(Math.Ceiling((float)totalCount / 10));
                    List<int> list = GetList(pageCount);
                    for (int i = 1; i <= list.Count; i++)
                    {
                        string u = string.Format(url + "&page=" + i);
                        if (onCollected != null)
                        {
                            onCollected(u);
                        }
                    }
                    //Parallel.ForEach(list, (i) =>
                    //{
                    //    string u = string.Format(url + "&page=" + i);
                    //    Collection(u);
                    //});

                }
                else
                {
                    //超过10000的话就分月来采集
                    for (int i = 1; i <= 12; i++)
                    {
                        //获取新的url
                        string newUrl = string.Format(urlTemp, Year + i.ToString("d2"));
                        // 获取总数量
                        totalCount = Total(newUrl);
                        int pageCount = Convert.ToInt32(Math.Ceiling((float)totalCount / 10));
                        List<int> list = GetList(pageCount);

                        for (int j = 1; j <= list.Count; j++)
                        {
                            string u = string.Format(newUrl + "&page=" + j);
                            if (onCollected != null)
                            {
                                onCollected(u);
                            }
                        }
                        //Parallel.ForEach(list, (j) =>
                        //{
                        //    string u = string.Format(newUrl + "&page=" + j);
                        //    Collection(u);
                        //});
                    }
                }
                Watch.Stop();
                long watchTime = Watch.ElapsedMilliseconds;//花费时间 
                if (onUpdateTime != null)
                {
                    onUpdateTime(watchTime);
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// 获取int列表
        /// </summary>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        private List<int> GetList(int pageCount)
        {
            List<int> d = new List<int>();
            for (int i = 1; i <= pageCount; i++)
            {
                d.Add(i);
            }
            return d;
        }

        InfoDAL infoDal = new InfoDAL();

        //单页采集
        public void Collection(string guid, string url)
        {
            HtmlWeb htmlWeb = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument htmlDoc = null;
            htmlDoc = htmlWeb.Load(url);
            HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//table//tr[contains(@class,'lm-l-lh')]");
            if (nodes == null)
            {
                return;
            }
            foreach (HtmlNode node in nodes)
            {
                var node1 = node.SelectSingleNode("td[1]//span[contains(@class,'tcenter')]");
                var node2 = node.SelectSingleNode("td[2]//a[1]");
                var node3 = node.SelectSingleNode("td[3]//em[1]");
                var node4 = node.SelectSingleNode("td[4]//span[1]");
                var node41 = node.SelectSingleNode("td[4]//a[1]");
                var node5 = node.SelectSingleNode("td[5]//a[1]");
                //序号
                string sNo = node1.InnerText;
                //获取了专利号(申请号)
                string patentNo = node2.InnerText;
                //申请日
                string applyDate = node3.InnerText;
                //类型
                string patentType = node4.InnerText;
                //名称
                string name = node41.InnerText;
                //主分类号
                string orginalNo = node5.InnerText;

                Console.WriteLine(sNo + "  " + patentNo);
                infoDal.Insert(new Info() { PatentNo = patentNo,ApplyDate=applyDate,PatentType=patentType,Name=name,OrginalNo=orginalNo });
            }
            if (onComplate != null)
            {
                onComplate(guid,url);
            }
        }

        /// <summary>
        /// 获取总数量
        /// </summary>
        /// <returns></returns>
        public int Total(string url)
        {
            try
            {
                HtmlWeb htmlWeb = new HtmlWeb();
                HtmlAgilityPack.HtmlDocument htmlDoc = null;
                htmlDoc = htmlWeb.Load(url);
                HtmlNode totalNode = htmlDoc.DocumentNode.SelectSingleNode("//span[@id='sop-totalCount']");
                int totalCount = int.Parse(totalNode.InnerText ?? "0");
                return totalCount;
            }
            catch
            {
                return 0;
            }
        }
    }
}
