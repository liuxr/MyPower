/* 
   ======================================================================== 
    文 件 名：     
	功能描述：                
    作    者:	Cumin           
    创建时间： 	2016/10/24 17:53:36
	版    本:	V1.0.0
   ------------------------------------------------------------------------
	历史更新纪录
   ------------------------------------------------------------------------
	版    本：           修改时间：           修改人：          
	修改内容：
   ------------------------------------------------------------------------
	Copyright (C) 2016   北京荣大科技有限公司
   ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SQLite;
using Dapper;
using System.Text;

namespace MyPay
{

    public class InfoDAL
    {
        private static string DataSource = @AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "DB.db";
        public InfoDAL()
        {

        }

        /// <summary>
        /// 获取所有的资源信息
        /// </summary>
        /// <returns></returns>
        public List<Info> GetList(string top = "", string where = "")
        {
            List<Info> list = new List<Info>();
            SQLiteConnectionStringBuilder sb = new SQLiteConnectionStringBuilder();
            sb.DataSource = DataSource;
            using (IDbConnection con = new SQLiteConnection(sb.ToString()))
            {
                con.Open();
                string sql = "select  * from 'info' ";
                if (!string.IsNullOrEmpty(where))
                {
                    sql += "where " + where;
                }
                if (!string.IsNullOrEmpty(top)) {
                    sql += " LIMIT 10000 ";
                }
                list = Dapper.SqlMapper.Query<Info>(con, sql).ToList();
                con.Close();
            }

            return list;
        }


        /// <summary>
        /// 根据ID获取账号下的最近一条数据
        /// </summary>
        /// <returns></returns>
        public Order GetMaxOrder(string account)
        {
            Order model = null;
            SQLiteConnectionStringBuilder sb = new SQLiteConnectionStringBuilder();
            sb.DataSource = DataSource;
            using (IDbConnection con = new SQLiteConnection(sb.ToString()))
            {
                con.Open();
                string sql = "select * from 'order' where account='" + account + "' ORDER BY ID DESC LIMIT 1 ";
                List<Order> list = Dapper.SqlMapper.Query<Order>(con, sql).ToList();
                if (list != null && list.Count == 1)
                {
                    model = list.Single();
                }
                con.Close();
            }
            return model;
        }


        public void Insert(List<Order> list)
        {
            //开始事务
            IDbTransaction transaction = null;
            try
            {
                SQLiteConnectionStringBuilder sb = new SQLiteConnectionStringBuilder();
                sb.DataSource = DataSource;


                using (IDbConnection con = new SQLiteConnection(sb.ToString()))
                {
                    con.Open();
                    transaction = con.BeginTransaction();
                    foreach (Order model in list)
                    {
                        string sql = "insert into 'order' (account,createDate,name,orderNo,tradeNo,batchNo,payee,amount,state,UpLoadState,UpLoadCount,UpLoadError,UpLoadDate) values (@account,@createDate,@name,@orderNo,@tradeNo,@batchNo,@payee,@amount,@state,@UpLoadState,@UpLoadCount,@UpLoadError,@UpLoadDate)";
                        int i = Dapper.SqlMapper.Execute(con, sql, new
                        {
                            Account = model.Account,
                            createDate = model.CreateDate,
                            name = model.Name,
                            orderNo = model.OrderNo,
                            tradeNo = model.TradeNo,
                            batchNo = model.BatchNo,
                            payee = model.Payee,
                            amount = model.Amount,
                            state = model.State,
                            UpLoadState = model.UpLoadState,
                            UpLoadCount = model.UpLoadCount,
                            UpLoadError = model.UpLoadError,
                            UpLoadDate = model.UpLoadDate
                        }, transaction, null, null);
                    }
                    //提交事务
                    transaction.Commit();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    //出现异常，事务Rollback
                    transaction.Rollback();
                    Console.WriteLine(ex);
                }
            }
        }

        public void Insert(Info model)
        {
            try
            {
                SQLiteConnectionStringBuilder sb = new SQLiteConnectionStringBuilder();
                sb.DataSource = DataSource;
                using (IDbConnection con = new SQLiteConnection(sb.ToString()))
                {
                    con.Open();
                    string sql = "insert into 'info' (PatentNo,ApplyDate,PatentType,Name,OrginalNo,Address,ApplyName)  values (@PatentNo,@ApplyDate,@PatentType,@Name,@OrginalNo,@Address,@ApplyName)";
                    int i = Dapper.SqlMapper.Execute(con, sql, new
                    {
                        PatentNo = model.PatentNo,
                        ApplyDate = model.ApplyDate,
                        PatentType = model.PatentType,
                        Name = model.Name,
                        OrginalNo = model.OrginalNo,
                        Address=model.Address,
                        ApplyName=model.ApplyName
                    });
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

            }
        }

        /// <summary>
        /// 更新单条数据
        /// </summary>
        /// <param name="model"></param>
        public void Update(Order model)
        {
            try
            {
                SQLiteConnectionStringBuilder sb = new SQLiteConnectionStringBuilder();
                sb.DataSource = DataSource;
                using (IDbConnection con = new SQLiteConnection(sb.ToString()))
                {
                    con.Open();
                    string sql = "update  'order' set account=@account,createDate=@createDate,name=@name,orderNo=@orderNo,tradeNo=@tradeNo,batchNo=@batchNo,payee=@payee,amount=@amount,state=@state,UpLoadState=@UpLoadState,UpLoadCount=@UpLoadCount,UpLoadError=@UpLoadError,UpLoadDate=@UpLoadDate where ID=@ID";
                    int i = Dapper.SqlMapper.Execute(con, sql, new
                    {
                        Account = model.Account,
                        createDate = model.CreateDate,
                        name = model.Name,
                        orderNo = model.OrderNo,
                        tradeNo = model.TradeNo,
                        batchNo = model.BatchNo,
                        payee = model.Payee,
                        amount = model.Amount,
                        state = model.State,
                        UpLoadState = model.UpLoadState,
                        UpLoadCount = model.UpLoadCount,
                        UpLoadError = model.UpLoadError,
                        UpLoadDate = model.UpLoadDate,
                        ID = model.ID
                    });

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// 动态创建表
        /// </summary>
        /// <param name="tableName"></param>
        public void CreateTable(string tableName)
        {
            try
            {
                //tableName="Info_2017";
                SQLiteConnectionStringBuilder sb = new SQLiteConnectionStringBuilder();
                sb.DataSource = DataSource;
                using (IDbConnection con = new SQLiteConnection(sb.ToString()))
                {
                    con.Open();

                    StringBuilder sql = new StringBuilder();
                    sql.Append(" DROP TABLE IF EXISTS 'main'.'" + tableName + "';");
                    sql.Append(" CREATE TABLE '" + tableName + "'(");
                    sql.Append(" 'ID'  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,");
                    sql.Append(" 'PatentNo'  TEXT(50) NOT NULL DEFAULT 申请号,");
                    sql.Append(" 'ApplyDate'  TEXT(10) DEFAULT 申请日期,");
                    sql.Append(" 'PatentType'  TEXT(20) DEFAULT 类型,");
                    sql.Append(" 'Name'  TEXT(100) DEFAULT 专利名称,");
                    sql.Append(" 'OrginalNo'  Text(20) DEFAULT 主分类号)");
                    int i = Dapper.SqlMapper.Execute(con, sql.ToString());
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }


        /// <summary>
        /// 清空表数据
        /// </summary>
        /// <param name="tableName"></param>
        public void ClearTable(string tableName)
        {
            try
            {
                //tableName="Info_2017";
                SQLiteConnectionStringBuilder sb = new SQLiteConnectionStringBuilder();
                sb.DataSource = DataSource;
                using (IDbConnection con = new SQLiteConnection(sb.ToString()))
                {
                    con.Open();
                    StringBuilder sql = new StringBuilder();
                    sql.Append("delete from " + tableName + ";  ");
                    sql.Append("update sqlite_sequence SET seq = 0 where name ='" + tableName + "';");
                    int i = Dapper.SqlMapper.Execute(con, sql.ToString());
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
