using MyPay;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
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
                ICellStyle cellStyle = workbook.CreateCellStyle();
                //设置单元格的样式：水平对齐居中
                cellStyle.VerticalAlignment = VerticalAlignment.Center;//垂直对齐(默认应该为center，如果center无效则用justify)

                for (int i = 0; i < list.Count; i++)
                {
                    var m = list[i];
                    var row = sheet.CreateRow(i + 1);
                    var cell = row.CreateCell(0);
                    cell.SetCellValue(m.PatentNo);
                    cell = row.CreateCell(1);
                    cell.SetCellValue(m.Name);
                    cell = row.CreateCell(2);
                    cell.CellStyle = cellStyle;
                    cell.SetCellValue(m.ApplyName);
                    cell = row.CreateCell(3);
                    cell.CellStyle = cellStyle;
                    cell.SetCellValue(m.Address);
                    cell = row.CreateCell(4);
                    cell.SetCellValue(m.PublishDate);
                    cell = row.CreateCell(5);
                    cell.SetCellValue(m.PatentType);
                }
                string tmpName = "";
                int count = 0;
                string tmpName1 = "";
                int count1 = 0;
                for (int i = 1; i <= sheet.LastRowNum - 1; i++) {
                    ICell cell = sheet.GetRow(i).Cells[2];
                    string name = cell.ToString();
                    if (tmpName.Equals(name))
                    {
                        count = count + 1;
                    }
                    else
                    {
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(i - count-1, i-1, 2, 2));
                        count = 0;
                        tmpName = name;
                    }

                    cell = sheet.GetRow(i).Cells[3];
                     name = cell.ToString();
                    if (tmpName1.Equals(name))
                    {
                        count1 = count1 + 1;
                    }
                    else
                    {
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(i - count1 - 1, i - 1, 3, 3));
                        count1 = 0;
                        tmpName1 = name;
                    }

                }

                using (var fs = File.OpenWrite(fileName))
                {
                    workbook.Write(fs);   //向打开的这个xls文件中写入mySheet表并保存。
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("导出失败，请重新导出");
                return;
            }
            MessageBox.Show("导出成功");
        }

        public void CreateHeader(NPOI.SS.UserModel.ISheet sheet)
        {
            var row = sheet.CreateRow(0);
            var cell = row.CreateCell(0);
            cell.SetCellValue("申请号");
            cell = row.CreateCell(1);
            cell.SetCellValue("专利名称");
            cell = row.CreateCell(2);
            cell.SetCellValue("申请人");
            cell = row.CreateCell(3);
            cell.SetCellValue("地址");
            cell = row.CreateCell(4);
            cell.SetCellValue("公开/公告日");
            cell = row.CreateCell(5);
            cell.SetCellValue("法律状态");
            for (int i = 0; i < 6; i++)
            {
                var w = 20 * 256;
                if (i == 1 || i == 2 || i == 3)
                {
                    w = 40 * 256;
                }
                sheet.SetColumnWidth(i, w);
            }
        }

    }
}
