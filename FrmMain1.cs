/*
 备注订单号
 支付宝交易号
 收款金额
 付款人姓名
 付款时间 
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyPower;
namespace MyPay
{
    public partial class FrmMain1 : Form
    {
        delegate void MyDelegate();

        MyClass me = new MyClass();
        public FrmMain1()
        {
            this.SetStyle(ControlStyles.DoubleBuffer |
         ControlStyles.UserPaint |
         ControlStyles.AllPaintingInWmPaint,
         true);
            this.UpdateStyles();
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            me.onCollected += (r) =>
            {
                InvokeControlAction(lvNear, new Action<ListView>((l) =>
                {
                    int count = lvNear.Items.Count + 1;
                    string guid = Guid.NewGuid().ToString();
                    ListViewItem item = new ListViewItem(new string[] { guid, r, "未开始" });
                    lvNear.Items.Add(item);
                }));
            };
            me.onUpdateCount += (count) =>
            {
                lblCount.Text = count.ToString();
            };
            me.onUpdateTime += (time) =>
            {
                lblTime.Text = time.ToString();
            };
            me.onComplate += (guid, url) =>
            {
                InvokeControlAction(lvNear, new Action<ListView>((l) =>
                {
                    ListViewItem item = GetItem(guid);
                    item.SubItems[2].Text = "采集完成";
                }));
            };
        }

        private void FrmMain_SizeChanged(object sender, EventArgs e)
        {
            // SetWidth(lvNear);
            //  SetWidth(lvWorking, false);
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("确定要关闭程序吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        private void btnCollection_Click(object sender, EventArgs e)
        {
            if (lvNear.Items.Count <= 0)
            {
                MessageBox.Show("请先分析");
                return;
            }

            //已采集，冲洗采集？？？

            //首先清空采集的表数据
            dal.ClearTable("info");
            
            List<Test> urls = new List<Test>();
            for (int i = 0; i < lvNear.Items.Count; i++) {
                ListViewItem item = lvNear.Items[i];
                urls.Add(new Test() { ID =i, Guid = item.SubItems[0].Text, Url = item.SubItems[1].Text });
            }
            //foreach (ListViewItem item in lvNear.Items)
            //{
            //    urls.Add(new Test() {ID= Guid = item.SubItems[0].Text, Url = item.SubItems[1].Text });
            //}
            //利用线程池安排一个线程执行并行循环
            System.Threading.ThreadPool.QueueUserWorkItem(w =>
            {
                //获取所有地址
                Parallel.ForEach(urls, new ParallelOptions() { MaxDegreeOfParallelism = 5 }, t =>
                {
                    InvokeControlAction(lvNear, new Action<ListView>((l) =>
                    {
                        lvNear.Items[t.ID].SubItems[2].Text = "采集中..";
                        me.Collection(t.Guid, t.Url);
                    }));
                });
            }, null);

        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            lvNear.Items.Clear();
            Task.Factory.StartNew(() =>
            {
                me.Run(txtDate.Text.Trim());
            });
        }

        private int GetItemIndex(string guid)
        {
            int index = -1;
            for (int i = 0; i < lvNear.Items.Count; i++)
            {
                ListViewItem item = lvNear.Items[i];
                if (guid == item.SubItems[0].Text)
                {
                    return i;
                }
                // urls.Add(new Test() { Guid = item.SubItems[0].Text, Url = item.SubItems[1].Text });
            }
            return index;
        }

        private ListViewItem GetItem(string guid)
        {
            for (int i = 0; i < lvNear.Items.Count; i++)
            {
                ListViewItem item = lvNear.Items[i];
                if (guid == item.SubItems[0].Text)
                {
                    return item;
                }
                // urls.Add(new Test() { Guid = item.SubItems[0].Text, Url = item.SubItems[1].Text });
            }
            return null;
        }
        InfoDAL dal = new MyPay.InfoDAL();
        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel(*.xls)|*.xls";
            sfd.FileName = "知识产权.xls";
            DialogResult dr = sfd.ShowDialog();
            if (dr == DialogResult.Cancel) return;

            List<Info> list = dal.GetList();
            ExcelHelper helper = new ExcelHelper(sfd.FileName);
            helper.Export(list);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, panel1.ClientRectangle,
                Color.Black, 1, ButtonBorderStyle.Solid, //左边
　　　　　        Color.Black, 1, ButtonBorderStyle.Solid, //上边
　　　　　        Color.Black, 1, ButtonBorderStyle.Solid, //右边
　　　　　        Color.Black, 1, ButtonBorderStyle.None);//底边
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, panel1.ClientRectangle,
               Color.Black, 1, ButtonBorderStyle.Solid, //左边
            Color.Black, 1, ButtonBorderStyle.Solid, //上边
            Color.Black, 1, ButtonBorderStyle.Solid, //右边
            Color.Black, 1, ButtonBorderStyle.None);//底边
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            me.QCC();
            return;
            try
            {
                //时间升序
                List<Info> list = dal.GetList(" top 10000 ").OrderBy(m => m.ApplyDate).ToList();
                if (list == null || list.Count <= 0)
                {
                    MessageBox.Show("未查询到数据");
                    return;
                }
                foreach (var m in list)
                {
                    ListViewItem item = new ListViewItem(new string[] {
                    m.PatentNo,m.ApplyName,m.Name,m.Address,m.ApplyDate,m.PatentType,m.OrginalNo });
                    lvResult.Items.Add(item);
                }
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询失败");
            }
        }

        public void InvokeControlAction<t>(t cont, Action<t> action) where t : Control
        {
            if (cont.InvokeRequired)
            {
                cont.Invoke(new Action<t, Action<t>>(InvokeControlAction),
                    new object[] { cont, action });
            }
            else
            {
                action(cont);
            }
        }
    }
}



public class Test
{
    public int ID { get; set; }
    public string Guid { get; set; }

    public string Url { get; set; }
}