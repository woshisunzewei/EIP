using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace EIP.Common.Core.Report.Excel
{
    /// <summary>
    /// 导出到Excel:基于OpenXml
    /// 1、建立模版,并将单元格进行命名
    /// 定义了三类的NameRange命名规则：
    ///   F_tablename_column：固定信息的单元格，一份Excel只出现一次。如人的姓名。
    ///   D_tablename_column: 动态信息的单元格，如人的工作经历的职务。
    ///   R_tablename : 动态区域，如人的工作经历会有很多条，每一条是一个动态区域。
    ///相对应数据库中的“主表 - 明细表”，主表是固态的，明细表是动态的，明细表的一条对应一个动态区域。
    /// eg:http://www.iyummy.com.cn/blog/detail/3 http://www.iyummy.com.cn/Blog/Detail/4
    /// </summary>
    public class OpenXmlExcel
    {
        /// <summary>
        /// 导出Excel
        /// 注意:1、模版上的工作标签不能为默认需要另外取名字
        ///      2、模版文件不能为只读
        /// </summary>
        /// <param name="templatePath">模版地址</param>
        /// <param name="downPath">下载地址</param>
        /// <param name="tables">表格数据</param>
        /// <returns></returns>
        public static string ExportExcel(string templatePath, string downPath, Dictionary<string, DataTable> tables)
        {
            #region 导出Excel
            File.Copy(templatePath, downPath);
            File.ReadAllBytes(downPath);
            using (SpreadsheetDocument document = SpreadsheetDocument.Open(downPath, true))
            {
                //1.去掉公式链
                var chain = document.WorkbookPart.CalculationChainPart;
                document.WorkbookPart.DeletePart(chain);

                var sheets = document.WorkbookPart.Workbook.Descendants<Sheet>();

                //2. 去掉所有的namerange
                var defineNames = document.WorkbookPart.Workbook.DefinedNames.Elements<DefinedName>();

                foreach (Sheet sheet in sheets)
                {
                    WorksheetPart part = (WorksheetPart)document.WorkbookPart.GetPartById(sheet.Id);
                    var excelrows = part.Worksheet.Descendants<Row>().ToList();

                    //2.去掉所有的formula
                    foreach (var excelrow in excelrows)
                    {
                        var excelcells = excelrow.Descendants<Cell>();
                        foreach (var cell in excelcells)
                        {
                            if (cell.CellFormula != null)
                            {
                                cell.CellFormula = null;
                                if (cell.CellValue != null) cell.CellValue.InnerXml = "";
                            }

                        }
                    }

                    //F_开头的单元格
                    var fix = defineNames.Where(o => o.InnerText.IndexOf(sheet.Name) == 0 && o.Name.Value.IndexOf("F_") == 0);
                    foreach (var f in fix)
                    {
                        string t = f.Name.Value.Substring(2);
                        string[] d = t.Split('_');
                        part.Worksheet.UpdateCell(
                            Utility.GetColumnLetter(f.InnerText),
                              (uint)Utility.GetRowIndex(f.InnerText),
                              (tables[d[0]].Rows[0][d[1]] as string));
                    }
                    //判断是否有动态
                    var dyn = defineNames.Where(o => o.InnerText.IndexOf(sheet.Name) == 0 && o.Name.Value.IndexOf("R_") == 0);
                    if (dyn.Count() > 0) //Todo:这里只处理一个动态区
                    {
                        var tb = dyn.First().Name.Value.Substring(2);
                        var row = part.Worksheet.GetRow((uint)Utility.GetRowIndex(dyn.First().InnerText)); //Tddo:动态行只能是一行,不能是合并行
                        var dcells = defineNames.Where(o => o.InnerText.IndexOf(sheet.Name) == 0 && o.Name.Value.IndexOf("D_") == 0);

                        for (int i = 0; i < tables[tb].Rows.Count; i++)
                        {
                            var newrow = part.Worksheet.CreateRow(row); //新增行

                            foreach (var c in dcells)
                            {
                                string t = c.Name.Value.Substring(2);
                                string[] d = t.Split('_');
                                newrow.UpdateCell(
                                        Utility.GetColumnLetter(c.InnerText),
                                       (tables[d[0]].Rows[i][d[1]] as string));
                            }
                        }
                        row.Hidden = new BooleanValue(true); //原来的模板行隐藏
                    }
                    part.Worksheet.Save();
                }
                document.WorkbookPart.Workbook.Save();
            }
            #endregion
            return downPath;
        }
    }

    /// <summary>
    /// 工具
    /// </summary>
    class Utility
    {

        /// <summary>
        /// Given a cell name, parses the specified cell to get the row index.
        /// </summary>
        /// <param name="cellReference">Address of the cell (ie. B2)</param>
        /// <returns>Row Index (ie. 2)</returns>
        public static int GetRowIndex(string cellReference)
        {
            // Create a regular expression to match the row index portion the cell name.
            Regex regex = new Regex(@"\d+");
            Match match = regex.Match(cellReference);

            return int.Parse(match.Value);
        }

        /// <summary>
        /// Given a cell name, parses the specified cell to get the column index.
        /// </summary>
        /// <param name="cellReference">Address of the cell (ie. C2)</param>
        /// <returns>Row Index (ie. 3)</returns>
        public static int GetColumnIndex(string cellReference)
        {
            // Create a regular expression to match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellReference);
            string name = match.Value.ToUpper();
            int number = 0;
            int pow = 1;
            for (int i = name.Length - 1; i >= 0; i--)
            {
                number += (name[i] - 'A' + 1) * pow;
                pow *= 26;
            }
            return number;
        }

        /// <summary>
        /// Given a cell name, parses the specified cell to get the column index.
        /// </summary>
        /// <param name="cellReference">Address of the cell (ie. C2)</param>
        /// <returns>Row Letter (ie. C)</returns>
        public static string GetColumnLetter(string cellReference)
        {
            // Create a regular expression to match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellReference);
            string name = match.Value.ToUpper();

            return name;
        }

        /// <summary>
        /// 将column从12345 转换成 ABCDE
        /// </summary>
        /// <param name="colindex"></param>
        /// <returns></returns>
        public static string ToColumnLetter(int colindex)
        {
            int quotient = colindex / 26;
            if (quotient > 0)
            {
                return ToColumnLetter(quotient) + Char.ConvertFromUtf32((colindex % 26) + 64);
            }
            else
                return char.ConvertFromUtf32(colindex + 64);
        }

        /// <summary>
        /// 得到一个cell区域的开始 如sheet1!A1:B3,返回{A1,B3} 或者 Sheet3!$B$3:$F$3 返回{B3,F3}
        /// </summary>
        /// <param name="celladdr">一个单元格的地址</param>
        /// <returns></returns>
        public static string[] GetCellAddr(string celladdr)
        {
            if (string.IsNullOrEmpty(celladdr)) return null;
            int i = celladdr.IndexOf("!");
            if (i > 0) celladdr = celladdr.Substring(i + 1);
            return celladdr.Replace("$", "").Trim().Split(':');
        }

        public static string GetSheetName(string celladdr)
        {
            if (string.IsNullOrEmpty(celladdr)) return null;
            int i = celladdr.IndexOf("!");
            if (i > 0)
            {

                if (celladdr.StartsWith("'"))
                    return celladdr.Substring(1, i - 2);
                else
                    return celladdr.Substring(0, i);
            }
            return string.Empty;
        }

        /// <summary>
        /// 增加新的行
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static string AddRow(string address)
        {
            var ad = address.Split(':');

            string startAddress = ad[0];
            string endAddress = ad[1];
            int start = GetRowIndex(startAddress);
            int end = GetRowIndex(startAddress);
            return startAddress.Replace(start.ToString(), (start + 1).ToString()) + ":" + endAddress.Replace(end.ToString(), (end + 1).ToString());
        }

    }

    static class WorkSheetExtend
    {
        // Given a worksheet, a column name, and a row index, 
        // gets the cell at the specified column and 
        private static Cell GetCell(this  Worksheet worksheet,
                     string columnName, uint rowIndex)
        {
            Row row = GetRow(worksheet, rowIndex);

            if (row == null)
                return null;
            var cell = row.Elements<Cell>().Where(c => String.Compare(c.CellReference.Value, columnName + rowIndex, StringComparison.OrdinalIgnoreCase) == 0);
            return cell.First();
        }


        // Given a worksheet and a row index, return the row.
        public static Row GetRow(this Worksheet worksheet, uint rowIndex)
        {
            return worksheet.GetFirstChild<SheetData>().
                Elements<Row>().First(r => r.RowIndex == rowIndex);
        }

        public static void UpdateCellNumber(this   Worksheet worksheet, string col, uint row, string text)
        {
            Cell cell = GetCell(worksheet, col, row);

            cell.CellValue = new CellValue(text);
            cell.DataType =
                new EnumValue<CellValues>(CellValues.Number);
        }
        public static void UpdateCell(this   Worksheet worksheet, string col, uint row, string text)
        {
            var cell = GetCell(worksheet, col, row);

            cell.CellValue = new CellValue(text);
            cell.DataType =
                new EnumValue<CellValues>(CellValues.String);
        }

        public static void UpdateCell(this   Row row, string col, string text)
        {
            var cell = row.Elements<Cell>().Where(c => string.Compare
                (c.CellReference.Value, col +
                row.RowIndex, true) == 0).First();
            cell.CellValue = new CellValue(text);
            cell.DataType =
                new EnumValue<CellValues>(CellValues.String);
        }
        public static void UpdateCellNumber(this   Row row, string col, string text)
        {
            var cell = row.Elements<Cell>().Where(c => string.Compare
                   (c.CellReference.Value, col +
                   row.RowIndex, true) == 0).First();

            cell.CellValue = new CellValue(text);
            cell.DataType =
                new EnumValue<CellValues>(CellValues.Number);
        }

        public static WorksheetPart GetWorksheetPartByName(this SpreadsheetDocument document, string sheetName)
        {
            IEnumerable<Sheet> sheets =
               document.WorkbookPart.Workbook.GetFirstChild<Sheets>().
               Elements<Sheet>().Where(s => s.Name == sheetName);

            if (sheets.Count() == 0)
            {
                // The specified worksheet does not exist.

                return null;
            }

            string relationshipId = sheets.First().Id.Value;
            WorksheetPart worksheetPart = (WorksheetPart)
                 document.WorkbookPart.GetPartById(relationshipId);
            return worksheetPart;

        }
        public static Row CreateRow(this  Worksheet worksheet, Row refRow)
        {
            SheetData sheetData = worksheet.GetFirstChild<SheetData>();
            uint newRowIndex = 0;
            var newRow = new Row() { RowIndex = refRow.RowIndex.Value };
            var cells = refRow.Elements<Cell>();
            newRow.Height = new DoubleValue(refRow.Height);
            newRow.CustomHeight = new BooleanValue(refRow.CustomHeight);
            foreach (Cell cell in cells)
            {
                Cell newCell = (Cell)cell.CloneNode(true);
                newCell.StyleIndex = new UInt32Value(cell.StyleIndex);
                newRow.Append(newCell);
            }

            IEnumerable<Row> rows = sheetData.Descendants<Row>().Where(r => r.RowIndex.Value >= refRow.RowIndex.Value);
            foreach (Row row in rows)
            {
                newRowIndex = System.Convert.ToUInt32(row.RowIndex.Value + 1);

                foreach (var cell in row.Elements<Cell>())
                {
                    // Update the references for reserved cells.
                    string cellReference = cell.CellReference.Value;
                    cell.CellReference = new StringValue(cellReference.Replace(row.RowIndex.Value.ToString(), newRowIndex.ToString()));
                }

                row.RowIndex = new UInt32Value(newRowIndex);
            }
            sheetData.InsertBefore(newRow, refRow);

            // process merge cell in cloned rows
            var mcells = worksheet.GetFirstChild<MergeCells>();
            if (mcells != null)
            {
                //处理所有动态行以下的merg
                var clonedMergeCells = mcells.Elements<MergeCell>().
                     Where(m => Utility.GetRowIndex(m.Reference.Value.Split(':')[0]) >= newRow.RowIndex.Value).ToList<MergeCell>();
                foreach (var cmCell in clonedMergeCells)
                {
                    cmCell.Reference.Value = Utility.AddRow(cmCell.Reference.Value);
                }

                //增加新的merg
                var newMergeCells = new List<MergeCell>();
                var rowMergeCells = mcells.Elements<MergeCell>().
                    Where(m => Utility.GetRowIndex(m.Reference.Value.Split(':')[0]) == refRow.RowIndex).ToList<MergeCell>();
                foreach (var mc in rowMergeCells)
                {
                    newMergeCells.Add(new MergeCell() { Reference = mc.Reference.Value.Replace(refRow.RowIndex.Value.ToString(), (newRow.RowIndex.Value).ToString()) });
                }

                uint count = mcells.Count.Value;
                mcells.Count = new UInt32Value(count + (uint)newMergeCells.Count);
                mcells.Append(newMergeCells.ToArray());
            }

            return newRow;
        }
    }
}