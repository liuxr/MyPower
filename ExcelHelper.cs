using MyPay;
using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyPower
{
    public class ExcelHelper
    {
        public string fileName = string.Empty;
        public ExcelHelper(string fileName) { this.fileName = fileName; }
        public void Export(List<Info> list)
        {
            try
            {
                //创建工作薄
                var workbook = new HSSFWorkbook();
                //创建表
                var sheet = workbook.CreateSheet("知识产权");
                //创建表头
                CreateHeader(sheet);

                for (int i = 0; i < list.Count; i++)
                {
                    var m = list[i];
                    var row = sheet.CreateRow(i + 1);
                    var cell = row.CreateCell(0);
                    cell.SetCellValue(m.PatentNo);
                    cell = row.CreateCell(1);
                    cell.SetCellValue(m.ApplyDate);
                    cell = row.CreateCell(2);
                    cell.SetCellValue(m.PatentType);
                    cell = row.CreateCell(3);
                    cell.SetCellValue(m.Name);
                    cell = row.CreateCell(4);
                    cell.SetCellValue(m.OrginalNo);
                }

                using (var fs = File.OpenWrite(fileName))
                {
                    workbook.Write(fs);   //向打开的这个xls文件中写入mySheet表并保存。
                }
            }
            catch (Exception ex) {
                MessageBox.Show("导出失败，请重新导出");
            }
        }

        public void CreateHeader(NPOI.SS.UserModel.ISheet sheet)
        {
            var row = sheet.CreateRow(0);
            var cell = row.CreateCell(0);
            cell.SetCellValue("申请号");
            cell = row.CreateCell(1);
            cell.SetCellValue("申请日");
            cell = row.CreateCell(2);
            cell.SetCellValue("专利类型");
            cell = row.CreateCell(3);
            cell.SetCellValue("专利名称");
            cell = row.CreateCell(4);
            cell.SetCellValue("主分类号");
            for (int i = 0; i < 5; i++)
            {
                var w = 20 * 256;
                if (i == 3)
                {
                    w = 40 * 256;
                }
                sheet.SetColumnWidth(i, w);
            }
        }

    }
}
