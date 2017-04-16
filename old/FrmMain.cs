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

namespace MyPay
{
    public partial class FrmMain : Form
    {
        private Timer timer = new Timer();

        private OrderDAL orderDal = new OrderDAL();
        /// <summary>
        /// 上传服务
        /// </summary>
        private OrderServer orderServer = new OrderServer();

        FrmBrower frmBrower = new MyPay.FrmBrower();
        public FrmMain()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            timer.Interval = 20 * 1000;
            timer.Tick += Time_Tick;
        }

        private void Time_Tick(object sender, EventArgs e)
        {
            SynchroData();
        }

        private void btnShowWebBrower_Click(object sender, EventArgs e)
        {
            frmBrower.StartPosition = FormStartPosition.CenterScreen;
            frmBrower.Show();
            frmBrower.BringToFront();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            SetWidth(lvNear);
            SetWidth(lvWorking, false);

            //更新用户名
            frmBrower.onUpdateName += (name) =>
            {
                if (string.IsNullOrEmpty(name))
                    return;
                lblName.Text = name;
                LoadData();
                timer.Enabled = true;
                timer.Start();
                frmBrower.Start();
            };

            frmBrower.onUpdaeHtml += (html) =>
            {
                rtbHtml.Clear();
                rtbHtml.AppendText(html);
            };

            frmBrower.onGetContext += ((newOrders, canWatch) =>
            {
                btnWatch.Enabled = canWatch;

                AppendData(newOrders);
            });
            frmBrower.StartPosition = FormStartPosition.CenterScreen;
            frmBrower.Show();
        }

        private void btnWatch_Click(object sender, EventArgs e)
        {
            if (btnWatch.Tag.Equals("0"))
            {
                btnWatch.Text = "监控已开启";
                btnWatch.Tag = "1";
                tabControl2.SelectedIndex = 1;
                frmBrower.Start();

            }
            else
            {
                btnWatch.Text = "监控已关闭";
                btnWatch.Tag = "0";
                frmBrower.Stop();
            }
        }

        private void LoadData()
        {
            lvNear.BeginInvoke(new Action(() =>
            {
                lvNear.Items.Clear();
                string where = " Account='" + lblName.Text + "'";
                List<Order> list = orderDal.GetList(where);
                foreach (Order model in list)
                {
                    ListViewItem item = new ListViewItem(new string[] {
                    model.ID.ToString(),
                    model.CreateDate,
                    model.Name,
                    model.OrderNo,
                    model.TradeNo,
                    model.BatchNo,
                    model.Payee,
                    model.Amount,
                    model.State,
                    model.UpLoadState,
                    model.UpLoadCount.ToString(),
                    model.UpLoadDate
                });

                    lvNear.Items.Insert(0, item);
                }
            }));
        }

        private void AppendData(List<Order> list)
        {
            foreach (Order model in list)
            {
                ListViewItem item = new ListViewItem(new string[] {
                      model.ID.ToString(),
                    model.CreateDate,
                    model.Name,
                    model.OrderNo,
                    model.TradeNo,
                    model.BatchNo,
                    model.Payee,
                    model.Amount,
                    model.State,
                    model.UpLoadState,
                    model.UpLoadCount.ToString(),
                    model.UpLoadDate
                });
                lvWorking.Invoke(new Action(() =>
                {
                    lvWorking.Items.Insert(0, item);
                }));
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            int result = 20;
            bool b = int.TryParse(txtTimeSpan.Text.Trim(), out result);
            if (!b || result <= 0)
            {
                MessageBox.Show("Time is Invaid");
                return;
            }

            frmBrower.SetInterval(result);
        }


        private void SetWidth(ListView lv, bool showId = true)
        {
            int width = lv.Width;
            int w1 = lv.Columns[0].Width = showId ? 50 : 0;
            int w2 = lv.Columns[1].Width = 120;
            int w6 = lv.Columns[7].Width = 80;
            int w7 = lv.Columns[8].Width = 60;
            int w8 = lv.Columns[9].Width = 60;
            int w9 = lv.Columns[10].Width = 0;
            int w10 = lv.Columns[11].Width = 140;
            int w = width - (w1 + w2 + w6 + w7 + w8 + w9 + w10 + 16);

            lv.Columns[2].Width = w / 5;
            lv.Columns[3].Width = w / 5;
            lv.Columns[4].Width = w / 5;
            lv.Columns[5].Width = w / 5;
            lv.Columns[6].Width = w / 5;
        }

        private void FrmMain_SizeChanged(object sender, EventArgs e)
        {
            SetWidth(lvNear);
            SetWidth(lvWorking, false);
        }


        //上传服务，单独开启线程去做上传操作
        private void SynchroData()
        {
            RequestResult result = null;
            //获取未成功的数据
            string where = " Account='" + lblName.Text + "' and ifnull(UpLoadState,'') <> '成功'";
            List<Order> list = orderDal.GetList(where);
            foreach (Order model in list)
            {
                result = orderServer.Submit(model);
                if (result == null) continue;
                //更新日期
                model.UpLoadDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                //上传次数增加1
                model.UpLoadCount = model.UpLoadCount >= 20 ? 1 : model.UpLoadCount + 1;
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
                orderDal.Update(model);
                //更新界面
                UpdateUI(model);
            }
        }

        private void UpdateUI(Order model)
        {
            ////最近扫描
            //lvNear.Invoke(new Action(() =>
            //{
            //    Task.Factory.StartNew(new Action(() =>
            //    {
            //        LoadData();
            //    }));
            //}));

            //已扫描
            lvWorking.Invoke(new Action(() =>
            {
                Task.Factory.StartNew(new Action(() =>
                {
                    foreach (ListViewItem item in lvWorking.Items)
                    {
                        //获取
                        string orderNo = item.SubItems[3].Text;
                        string tradeNo = item.SubItems[4].Text;
                        string batchNo = item.SubItems[5].Text;
                        if (orderNo.Equals(model.OrderNo) && tradeNo.Equals(model.TradeNo) && batchNo.Equals(model.BatchNo))
                        {
                            item.SubItems[9].Text = model.UpLoadState;
                            item.SubItems[10].Text = model.UpLoadCount.ToString();
                            item.SubItems[11].Text = model.UpLoadDate;
                        }
                    }
                }));
            }));
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("确定要关闭程序吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }
    }
}
