using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Eval;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace EIP.Common.Core.Report.Excel
{
    /// <summary>
    /// 操作Excel：NPOI
    /// </summary>
    public static class NpoiExcel
    {
        public static void InsertSheet(string outputFile, string sheetname, DataTable dt)
        {
            var readfile = new FileStream(outputFile, FileMode.Open, FileAccess.Read);
            var hssfworkbook = WorkbookFactory.Create(readfile);
            var num = hssfworkbook.GetSheetIndex(sheetname);
            ISheet sheet1;
            if (num >= 0)
                sheet1 = hssfworkbook.GetSheet(sheetname);
            else
            {
                sheet1 = hssfworkbook.CreateSheet(sheetname);
            }


            if (sheet1.GetRow(0) == null)
            {
                sheet1.CreateRow(0);
            }
            for (var coluid = 0; coluid < dt.Columns.Count; coluid++)
            {
                if (sheet1.GetRow(0).GetCell(coluid) == null)
                {
                    sheet1.GetRow(0).CreateCell(coluid);
                }

                sheet1.GetRow(0).GetCell(coluid).SetCellValue(dt.Columns[coluid].ColumnName);
            }


            for (var i = 1; i <= dt.Rows.Count; i++)
            {
                try
                {
                    if (sheet1.GetRow(i) == null)
                    {
                        sheet1.CreateRow(i);
                    }
                    for (var coluid = 0; coluid < dt.Columns.Count; coluid++)
                    {
                        if (sheet1.GetRow(i).GetCell(coluid) == null)
                        {
                            sheet1.GetRow(i).CreateCell(coluid);
                        }

                        sheet1.GetRow(i).GetCell(coluid).SetCellValue(dt.Rows[i - 1][coluid].ToString());
                    }
                }
                catch (Exception ex)
                {
                    //throw;
                }
            }
            try
            {
                readfile.Close();

                var writefile = new FileStream(outputFile, FileMode.OpenOrCreate, FileAccess.Write);
                hssfworkbook.Write(writefile);
                writefile.Close();
            }
            catch (Exception ex)
            {
                // ignored
            }
        }

        public static int GetSheetNumber(string outputFile)
        {
            var number = 0;
            try
            {
                var readfile = new FileStream(outputFile, FileMode.Open, FileAccess.Read);

                var hssfworkbook = new HSSFWorkbook(readfile);
                number = hssfworkbook.NumberOfSheets;
            }
            catch (Exception exception)
            {
                // ignored
            }
            return number;
        }

        public static ArrayList GetSheetName(string outputFile)
        {
            var arrayList = new ArrayList();
            try
            {
                var readfile = new FileStream(outputFile, FileMode.Open, FileAccess.Read);

                var hssfworkbook = new HSSFWorkbook(readfile);
                for (var i = 0; i < hssfworkbook.NumberOfSheets; i++)
                {
                    arrayList.Add(hssfworkbook.GetSheetName(i));
                }
            }
            catch (Exception exception)
            {
                // ignored
            }
            return arrayList;
        }

        public static bool IsNumeric(String message, out double result)
        {
            var rex = new Regex(@"^[-]?\d+[.]?\d*$");
            result = -1;
            if (rex.IsMatch(message))
            {
                result = double.Parse(message);
                return true;
            }
            return false;
        }


        /// <summary>
        ///     用于Web导出                                                                                             第一步
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        /// <param name="strFileName">文件名</param>
        public static void ExportByWeb(DataTable dtSource, string strHeaderText, string strFileName)
        {
            var curContext = HttpContext.Current;

            // 设置编码和附件格式
            curContext.Response.ContentType = "application/vnd.ms-excel";
            curContext.Response.ContentEncoding = Encoding.UTF8;
            curContext.Response.Charset = "";
            curContext.Response.AppendHeader("Content-Disposition",
                "attachment;filename=" + HttpUtility.UrlEncode(strFileName, Encoding.UTF8));
            curContext.Response.BinaryWrite(Export(dtSource, strHeaderText).GetBuffer());
            curContext.Response.End();
        }

        /// <summary>
        ///     DataTable导出到Excel的MemoryStream                                                                      第二步
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        public static MemoryStream Export(DataTable dtSource, string strHeaderText)
        {
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet() as HSSFSheet;

            #region 右击文件 属性信息

            {
                var dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                dsi.Company = "NPOI";
                workbook.DocumentSummaryInformation = dsi;

                var si = PropertySetFactory.CreateSummaryInformation();
                si.Author = "文件作者信息"; //填加xls文件作者信息
                si.ApplicationName = "创建程序信息"; //填加xls文件创建程序信息
                si.LastAuthor = "最后保存者信息"; //填加xls文件最后保存者信息
                si.Comments = "作者信息"; //填加xls文件作者信息
                si.Title = "标题信息"; //填加xls文件标题信息
                si.Subject = "主题信息"; //填加文件主题信息

                si.CreateDateTime = DateTime.Now;
                workbook.SummaryInformation = si;
            }

            #endregion

            var dateStyle = workbook.CreateCellStyle() as HSSFCellStyle;
            var format = workbook.CreateDataFormat() as HSSFDataFormat;
            if (dateStyle != null)
            {
                if (format != null) dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

                //取得列宽
                var arrColWidth = new int[dtSource.Columns.Count];
                foreach (DataColumn item in dtSource.Columns)
                {
                    arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName).Length;
                }
                for (var i = 0; i < dtSource.Rows.Count; i++)
                {
                    for (var j = 0; j < dtSource.Columns.Count; j++)
                    {
                        var intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                        if (intTemp > arrColWidth[j])
                        {
                            arrColWidth[j] = intTemp;
                        }
                    }
                }
                var rowIndex = 0;
                foreach (DataRow row in dtSource.Rows)
                {
                    #region 新建表，填充表头，填充列头，样式

                    if (rowIndex == 65535 || rowIndex == 0)
                    {
                        if (rowIndex != 0)
                        {
                            sheet = workbook.CreateSheet() as HSSFSheet;
                        }

                        #region 表头及样式

                        {
                            if (string.IsNullOrEmpty(strHeaderText))
                            {
                                var headerRow = sheet.CreateRow(0) as HSSFRow;
                                headerRow.HeightInPoints = 25;
                                headerRow.CreateCell(0).SetCellValue(strHeaderText);
                                var headStyle = workbook.CreateCellStyle() as HSSFCellStyle;
                                //headStyle.Alignment = CellHorizontalAlignment.Center;
                                var font = workbook.CreateFont() as HSSFFont;
                                font.FontHeightInPoints = 20;
                                font.Boldweight = 700;
                                headStyle.SetFont(font);
                                headerRow.GetCell(0).CellStyle = headStyle;
                                sheet.AddMergedRegion(new Region(0, 0, 0, dtSource.Columns.Count - 1));
                                //headerRow.Dispose();
                            }
                        }

                        #endregion

                        #region 列头及样式

                        {
                            var headerRow = sheet.CreateRow(0) as HSSFRow;
                            var headStyle = workbook.CreateCellStyle() as HSSFCellStyle;
                            //headStyle.Alignment = CellHorizontalAlignment.Center;
                            var font = workbook.CreateFont() as HSSFFont;
                            font.FontHeightInPoints = 10;
                            font.Boldweight = 700;
                            headStyle.SetFont(font);
                            foreach (DataColumn column in dtSource.Columns)
                            {
                                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                                headerRow.GetCell(column.Ordinal).CellStyle = headStyle;

                                //设置列宽
                                sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                            }
                            //headerRow.Dispose();
                        }

                        #endregion

                        rowIndex = 1;
                    }

                    #endregion

                    #region 填充内容

                    var dataRow = sheet.CreateRow(rowIndex) as HSSFRow;
                    foreach (DataColumn column in dtSource.Columns)
                    {
                        var newCell = dataRow.CreateCell(column.Ordinal) as HSSFCell;

                        var drValue = row[column].ToString();

                        switch (column.DataType.ToString())
                        {
                            case "System.String": //字符串类型
                                newCell.SetCellValue(drValue);
                                break;
                            case "System.DateTime": //日期类型
                                DateTime dateV;
                                DateTime.TryParse(drValue, out dateV);
                                newCell.SetCellValue(dateV);

                                newCell.CellStyle = dateStyle; //格式化显示
                                break;
                            case "System.Boolean": //布尔型
                                var boolV = false;
                                bool.TryParse(drValue, out boolV);
                                newCell.SetCellValue(boolV);
                                break;
                            case "System.Int16": //整型
                            case "System.Int32":
                            case "System.Int64":
                            case "System.Byte":
                                var intV = 0;
                                int.TryParse(drValue, out intV);
                                newCell.SetCellValue(intV);
                                break;
                            case "System.Decimal": //浮点型
                            case "System.Double":
                                double doubV = 0;
                                double.TryParse(drValue, out doubV);
                                newCell.SetCellValue(doubV);
                                break;
                            case "System.DBNull": //空值处理
                                newCell.SetCellValue("");
                                break;
                            default:
                                newCell.SetCellValue("");
                                break;
                        }
                    }

                    #endregion

                    rowIndex++;
                }
            }
            using (var ms = new MemoryStream())
            {
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;

                //sheet.Dispose();
                //workbook.Dispose();//一般只用写这一个就OK了，他会遍历并释放所有资源，但当前版本有问题所以只释放sheet
                return ms;
            }
        }

        /// <summary>
        ///     /注：分浏览器进行编码（IE必须编码，FireFox不能编码，Chrome可编码也可不编码）
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="strHeaderText"></param>
        /// <param name="strFileName"></param>
        public static void ExportByWeb(DataSet ds, string strHeaderText, string strFileName)
        {
            var curContext = HttpContext.Current;
            curContext.Response.ContentType = "application/vnd.ms-excel";
            curContext.Response.Charset = "";
            if (curContext.Request.UserAgent.ToLower().IndexOf("firefox", StringComparison.Ordinal) > 0)
            {
                curContext.Response.AppendHeader("Content-Disposition", "attachment;filename=" + strFileName);
            }
            else
            {
                curContext.Response.AppendHeader("Content-Disposition",
                    "attachment;filename=" + HttpUtility.UrlEncode(strFileName, Encoding.UTF8));
            }

            //  curContext.Response.AppendHeader("Content-Disposition", "attachment;filename=" +strFileName);
            curContext.Response.ContentEncoding = Encoding.GetEncoding("utf-8");
            curContext.Response.BinaryWrite(ExportDataSetToExcel(ds, strHeaderText).GetBuffer());
            curContext.Response.End();
        }

        /// <summary>
        ///     由DataSet导出Excel
        /// </summary>
        /// <param name="sourceTable">要导出数据的DataTable</param>
        /// <param name="sheetName">工作表名称</param>
        /// <returns>Excel工作表</returns>
        private static MemoryStream ExportDataSetToExcel(DataSet sourceDs, string sheetName)
        {
            var workbook = new HSSFWorkbook();
            var ms = new MemoryStream();
            var sheetNames = sheetName.Split(',');
            for (var i = 0; i < sheetNames.Length; i++)
            {
                var sheet = workbook.CreateSheet(sheetNames[i]);

                #region 列头

                var headerRow = sheet.CreateRow(0);
                var headStyle = workbook.CreateCellStyle() as HSSFCellStyle;
                var font = workbook.CreateFont() as HSSFFont;
                font.FontHeightInPoints = 10;
                font.Boldweight = 700;
                headStyle.SetFont(font);

                //取得列宽
                var arrColWidth = new int[sourceDs.Tables[i].Columns.Count];
                foreach (DataColumn item in sourceDs.Tables[i].Columns)
                {
                    arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName).Length;
                }

                // 处理列头
                foreach (DataColumn column in sourceDs.Tables[i].Columns)
                {
                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                    headerRow.GetCell(column.Ordinal).CellStyle = headStyle;
                    //设置列宽
                    sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                }

                #endregion

                #region 填充值

                var rowIndex = 1;
                foreach (DataRow row in sourceDs.Tables[i].Rows)
                {
                    var dataRow = sheet.CreateRow(rowIndex);
                    foreach (DataColumn column in sourceDs.Tables[i].Columns)
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    }
                    rowIndex++;
                }

                #endregion
            }
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            workbook = null;
            return ms;
        }

        /// <summary>
        ///     验证导入的Excel是否有数据
        /// </summary>
        /// <param name="excelFileStream"></param>
        /// <returns></returns>
        public static bool HasData(Stream excelFileStream)
        {
            using (excelFileStream)
            {
                IWorkbook workBook = new HSSFWorkbook(excelFileStream);
                if (workBook.NumberOfSheets > 0)
                {
                    var sheet = workBook.GetSheetAt(0);
                    return sheet.PhysicalNumberOfRows > 0;
                }
            }
            return false;
        }

        #region 从datatable中将数据导出到excel

        /// <summary>
        ///     DataTable导出到Excel的MemoryStream
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        private static MemoryStream ExportDt(DataTable dtSource, string strHeaderText)
        {
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet() as HSSFSheet;

            #region 右击文件 属性信息

            //{
            //    DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            //    dsi.Company = "http://www.yongfa365.com/";
            //    workbook.DocumentSummaryInformation = dsi;

            //    SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            //    si.Author = "柳永法"; //填加xls文件作者信息
            //    si.ApplicationName = "NPOI测试程序"; //填加xls文件创建程序信息
            //    si.LastAuthor = "柳永法2"; //填加xls文件最后保存者信息
            //    si.Comments = "说明信息"; //填加xls文件作者信息
            //    si.Title = "NPOI测试"; //填加xls文件标题信息
            //    si.Subject = "NPOI测试Demo"; //填加文件主题信息
            //    si.CreateDateTime = DateTime.Now;
            //    workbook.SummaryInformation = si;
            //}

            #endregion

            var dateStyle = workbook.CreateCellStyle() as HSSFCellStyle;
            var format = workbook.CreateDataFormat() as HSSFDataFormat;
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

            //取得列宽
            var arrColWidth = new int[dtSource.Columns.Count];
            foreach (DataColumn item in dtSource.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName).Length;
            }
            for (var i = 0; i < dtSource.Rows.Count; i++)
            {
                for (var j = 0; j < dtSource.Columns.Count; j++)
                {
                    var intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp;
                    }
                }
            }
            var rowIndex = 0;

            foreach (DataRow row in dtSource.Rows)
            {
                #region 新建表，填充表头，填充列头，样式

                if (rowIndex == 65535 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        sheet = workbook.CreateSheet() as HSSFSheet;
                    }

                    #region 表头及样式
                    {
                        var headerRow = sheet.CreateRow(0) as HSSFRow;
                        headerRow.HeightInPoints = 25;
                        headerRow.CreateCell(0).SetCellValue(strHeaderText);
                        var headStyle = workbook.CreateCellStyle() as HSSFCellStyle;
                        headStyle.Alignment = HorizontalAlignment.Center;
                        var font = workbook.CreateFont() as HSSFFont;
                        font.FontHeightInPoints = 20;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);
                        headerRow.GetCell(0).CellStyle = headStyle;
                        sheet.AddMergedRegion(new Region(0, 0, 0, dtSource.Columns.Count - 1));
                        //headerRow.Dispose();
                    }

                    #endregion

                    #region 列头及样式
                    {
                        var headerRow = sheet.CreateRow(1) as HSSFRow;
                        var headStyle = workbook.CreateCellStyle() as HSSFCellStyle;
                        headStyle.Alignment = HorizontalAlignment.Center;
                        var font = workbook.CreateFont() as HSSFFont;
                        font.FontHeightInPoints = 10;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);
                        foreach (DataColumn column in dtSource.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;

                            //设置列宽
                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                        }
                        //headerRow.Dispose();
                    }

                    #endregion

                    rowIndex = 2;
                }

                #endregion

                #region 填充内容

                var dataRow = sheet.CreateRow(rowIndex) as HSSFRow;
                foreach (DataColumn column in dtSource.Columns)
                {
                    var newCell = dataRow.CreateCell(column.Ordinal) as HSSFCell;

                    var drValue = row[column].ToString();

                    switch (column.DataType.ToString())
                    {
                        case "System.String": //字符串类型
                            double result;
                            if (IsNumeric(drValue, out result))
                            {
                                double.TryParse(drValue, out result);
                                newCell.SetCellValue(result);
                                break;
                            }
                            newCell.SetCellValue(drValue);
                            break;

                        case "System.DateTime": //日期类型
                            DateTime dateV;
                            DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);

                            newCell.CellStyle = dateStyle; //格式化显示
                            break;
                        case "System.Boolean": //布尔型
                            var boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16": //整型
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            var intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal": //浮点型
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull": //空值处理
                            newCell.SetCellValue("");
                            break;
                        default:
                            newCell.SetCellValue("");
                            break;
                    }
                }

                #endregion

                rowIndex++;
            }
            using (var ms = new MemoryStream())
            {
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;

                //sheet.Dispose();
                //workbook.Dispose();

                return ms;
            }
        }

        /// <summary>
        ///     DataTable导出到Excel的MemoryStream
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        private static void ExportDti(DataTable dtSource, string strHeaderText, FileStream fs)
        {
            var workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet() as XSSFSheet;

            #region 右击文件 属性信息

            //{
            //    DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            //    dsi.Company = "http://www.yongfa365.com/";
            //    workbook.DocumentSummaryInformation = dsi;

            //    SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            //    si.Author = "柳永法"; //填加xls文件作者信息
            //    si.ApplicationName = "NPOI测试程序"; //填加xls文件创建程序信息
            //    si.LastAuthor = "柳永法2"; //填加xls文件最后保存者信息
            //    si.Comments = "说明信息"; //填加xls文件作者信息
            //    si.Title = "NPOI测试"; //填加xls文件标题信息
            //    si.Subject = "NPOI测试Demo"; //填加文件主题信息
            //    si.CreateDateTime = DateTime.Now;
            //    workbook.SummaryInformation = si;
            //}

            #endregion

            var dateStyle = workbook.CreateCellStyle() as XSSFCellStyle;
            var format = workbook.CreateDataFormat() as XSSFDataFormat;
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

            //取得列宽
            var arrColWidth = new int[dtSource.Columns.Count];
            foreach (DataColumn item in dtSource.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName).Length;
            }
            for (var i = 0; i < dtSource.Rows.Count; i++)
            {
                for (var j = 0; j < dtSource.Columns.Count; j++)
                {
                    var intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp;
                    }
                }
            }
            var rowIndex = 0;

            foreach (DataRow row in dtSource.Rows)
            {
                #region 新建表，填充表头，填充列头，样式

                if (rowIndex == 0)
                {
                    #region 表头及样式

                    //{
                    //    XSSFRow headerRow = sheet.CreateRow(0) as XSSFRow;
                    //    headerRow.HeightInPoints = 25;
                    //    headerRow.CreateCell(0).SetCellValue(strHeaderText);

                    //    XSSFCellStyle headStyle = workbook.CreateCellStyle() as XSSFCellStyle;
                    //    headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    //    XSSFFont font = workbook.CreateFont() as XSSFFont;
                    //    font.FontHeightInPoints = 20;
                    //    font.Boldweight = 700;
                    //    headStyle.SetFont(font);

                    //    headerRow.GetCell(0).CellStyle = headStyle;

                    //    //sheet.AddMergedRegion(new Region(0, 0, 0, dtSource.Columns.Count - 1));
                    //    //headerRow.Dispose();
                    //}

                    #endregion

                    #region 列头及样式

                    {
                        var headerRow = sheet.CreateRow(0) as XSSFRow;


                        var headStyle = workbook.CreateCellStyle() as XSSFCellStyle;
                        headStyle.Alignment = HorizontalAlignment.Center;
                        var font = workbook.CreateFont() as XSSFFont;
                        font.FontHeightInPoints = 10;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);


                        foreach (DataColumn column in dtSource.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;

                            //设置列宽
                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                        }
                        //headerRow.Dispose();
                    }

                    #endregion

                    rowIndex = 1;
                }

                #endregion

                #region 填充内容

                var dataRow = sheet.CreateRow(rowIndex) as XSSFRow;
                foreach (DataColumn column in dtSource.Columns)
                {
                    var newCell = dataRow.CreateCell(column.Ordinal) as XSSFCell;

                    var drValue = row[column].ToString();

                    switch (column.DataType.ToString())
                    {
                        case "System.String": //字符串类型
                            double result;
                            if (IsNumeric(drValue, out result))
                            {
                                double.TryParse(drValue, out result);
                                newCell.SetCellValue(result);
                                break;
                            }
                            newCell.SetCellValue(drValue);
                            break;

                        case "System.DateTime": //日期类型
                            DateTime dateV;
                            DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);

                            newCell.CellStyle = dateStyle; //格式化显示
                            break;
                        case "System.Boolean": //布尔型
                            var boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16": //整型
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            var intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal": //浮点型
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull": //空值处理
                            newCell.SetCellValue("");
                            break;
                        default:
                            newCell.SetCellValue("");
                            break;
                    }
                }

                #endregion

                rowIndex++;
            }
            workbook.Write(fs);
            fs.Close();
        }

        /// <summary>
        ///     DataTable导出到Excel文件
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        /// <param name="strFileName">保存位置</param>
        public static void ExportDTtoExcel(DataTable dtSource, string strHeaderText, string strFileName)
        {
            var temp = strFileName.Split('.');

            if (temp[temp.Length - 1] == "xls" && dtSource.Columns.Count < 256 && dtSource.Rows.Count < 65536)
            {
                using (var ms = ExportDt(dtSource, strHeaderText))
                {
                    using (var fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
                    {
                        var data = ms.ToArray();
                        fs.Write(data, 0, data.Length);
                        fs.Flush();
                    }
                }
            }
            else
            {
                if (temp[temp.Length - 1] == "xls")
                    strFileName = strFileName + "x";

                using (var fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
                {
                    ExportDti(dtSource, strHeaderText, fs);
                }
            }
        }

        #endregion

        #region 从excel中将数据导出到datatable

        /// <summary>
        ///     读取excel 默认第一行为标头
        /// </summary>
        /// <param name="strFileName">excel文档路径</param>
        /// <returns></returns>
        public static DataTable ImportExceltoDt(string strFileName)
        {
            var dt = new DataTable();
            IWorkbook wb;
            using (var file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                wb = WorkbookFactory.Create(file);
            }
            var sheet = wb.GetSheetAt(0);
            dt = ImportDt(sheet, 0, true);
            return dt;
        }

        /// <summary>
        ///     读取Excel流到DataTable
        /// </summary>
        /// <param name="stream">Excel流</param>
        /// <returns>第一个sheet中的数据</returns>
        public static DataTable ImportExceltoDt(Stream stream)
        {
            try
            {
                var dt = new DataTable();
                IWorkbook wb;
                using (stream)
                {
                    wb = WorkbookFactory.Create(stream);
                }
                var sheet = wb.GetSheetAt(0);
                dt = ImportDt(sheet, 0, true);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///     读取Excel流到DataTable
        /// </summary>
        /// <param name="stream">Excel流</param>
        /// <param name="sheetName">表单名</param>
        /// <param name="headerRowIndex">列头所在行号，-1表示没有列头</param>
        /// <returns>指定sheet中的数据</returns>
        public static DataTable ImportExceltoDt(Stream stream, string sheetName, int headerRowIndex)
        {
            try
            {
                var dt = new DataTable();
                IWorkbook wb;
                using (stream)
                {
                    wb = WorkbookFactory.Create(stream);
                }
                var sheet = wb.GetSheet(sheetName);
                dt = ImportDt(sheet, headerRowIndex, true);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///     读取Excel流到DataSet
        /// </summary>
        /// <param name="stream">Excel流</param>
        /// <returns>Excel中的数据</returns>
        public static DataSet ImportExceltoDs(Stream stream)
        {
            try
            {
                var ds = new DataSet();
                IWorkbook wb;
                using (stream)
                {
                    wb = WorkbookFactory.Create(stream);
                }
                for (var i = 0; i < wb.NumberOfSheets; i++)
                {
                    var dt = new DataTable();
                    var sheet = wb.GetSheetAt(i);
                    dt = ImportDt(sheet, 0, true);
                    ds.Tables.Add(dt);
                }
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///     读取Excel流到DataSet
        /// </summary>
        /// <param name="stream">Excel流</param>
        /// <param name="dict">字典参数，key：sheet名，value：列头所在行号，-1表示没有列头</param>
        /// <returns>Excel中的数据</returns>
        public static DataSet ImportExceltoDs(Stream stream, Dictionary<string, int> dict)
        {
            try
            {
                var ds = new DataSet();
                IWorkbook wb;
                using (stream)
                {
                    wb = WorkbookFactory.Create(stream);
                }
                foreach (var key in dict.Keys)
                {
                    var dt = new DataTable();
                    var sheet = wb.GetSheet(key);
                    dt = ImportDt(sheet, dict[key], true);
                    ds.Tables.Add(dt);
                }
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///     读取excel
        /// </summary>
        /// <param name="strFileName">excel文件路径</param>
        /// <param name="sheetName"></param>
        /// <param name="headerRowIndex">列头所在行号，-1表示没有列头</param>
        /// <returns></returns>
        public static DataTable ImportExceltoDt(string strFileName, string sheetName, int headerRowIndex)
        {
            HSSFWorkbook workbook;
            IWorkbook wb;
            using (var file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                wb = new HSSFWorkbook(file);
            }
            var sheet = wb.GetSheet(sheetName);
            var table = new DataTable();
            table = ImportDt(sheet, headerRowIndex, true);
            //ExcelFileStream.Close();
            workbook = null;
            sheet = null;
            return table;
        }

        /// <summary>
        ///     读取excel
        /// </summary>
        /// <param name="strFileName">excel文件路径</param>
        /// <param name="sheetIndex"></param>
        /// <param name="headerRowIndex">列头所在行号，-1表示没有列头</param>
        /// <returns></returns>
        public static DataTable ImportExceltoDt(string strFileName, int sheetIndex, int headerRowIndex)
        {
            HSSFWorkbook workbook;
            IWorkbook wb;
            using (var file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                wb = WorkbookFactory.Create(file);
            }
            var isheet = wb.GetSheetAt(sheetIndex);
            var table = new DataTable();
            table = ImportDt(isheet, headerRowIndex, true);
            //ExcelFileStream.Close();
            workbook = null;
            isheet = null;
            return table;
        }

        /// <summary>
        ///     读取excel
        /// </summary>
        /// <param name="strFileName">excel文件路径</param>
        /// <param name="sheetName"></param>
        /// <param name="headerRowIndex">列头所在行号，-1表示没有列头</param>
        /// <returns></returns>
        public static DataTable ImportExceltoDt(string strFileName, string sheetName, int headerRowIndex, bool needHeader)
        {
            HSSFWorkbook workbook;
            IWorkbook wb;
            using (var file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                wb = WorkbookFactory.Create(file);
            }
            var sheet = wb.GetSheet(sheetName);
            var table = new DataTable();
            table = ImportDt(sheet, headerRowIndex, needHeader);
            //ExcelFileStream.Close();
            workbook = null;
            sheet = null;
            return table;
        }

        /// <summary>
        ///     读取excel
        /// </summary>
        /// <param name="strFileName">excel文件路径</param>
        /// <param name="sheetIndex"></param>
        /// <param name="headerRowIndex">列头所在行号，-1表示没有列头</param>
        /// <returns></returns>
        public static DataTable ImportExceltoDt(string strFileName, int sheetIndex, int headerRowIndex, bool needHeader)
        {
            HSSFWorkbook workbook;
            IWorkbook wb;
            using (var file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                wb = WorkbookFactory.Create(file);
            }
            var sheet = wb.GetSheetAt(sheetIndex);
            var table = new DataTable();
            table = ImportDt(sheet, headerRowIndex, needHeader);
            //ExcelFileStream.Close();
            workbook = null;
            sheet = null;
            return table;
        }

        /// <summary>
        /// 获取导入Excel实体信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> ImportExceltoList<T>(Stream stream) where T : new()
        {
            List<T> enlist = new List<T>();
            Dictionary<string, string> cellHeard = new Dictionary<string, string>();

            //将T反射转换为对应的集合
            PropertyInfo[] propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            //得到所有属性
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                //得到自定义的特性Description
                var descriptionCustomerAttr = propertyInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (descriptionCustomerAttr.Any())
                {
                    var des = descriptionCustomerAttr[0] as DescriptionAttribute;
                    //得到描述值
                    if (des != null)
                        cellHeard.Add(propertyInfo.Name, des.Description);
                }
            }
            List<string> keys = cellHeard.Keys.ToList(); // 要赋值的实体对象属性名称
            IWorkbook wb;
            using (stream)
            {
                wb = WorkbookFactory.Create(stream);
                ISheet sheet = wb.GetSheetAt(0); // 获取此文件第一个Sheet页
                for (int i = 1; i <= sheet.LastRowNum; i++) // 从1开始，第0行为单元头
                {
                    // 1.判断当前行是否空行，若空行就不在进行读取下一行操作，结束Excel读取操作
                    if (sheet.GetRow(i) == null)
                    {
                        break;
                    }

                    T en = new T();
                    for (int j = 0; j < keys.Count; j++)
                    {
                        // 3.给指定的属性赋值
                        PropertyInfo properotyInfo = en.GetType().GetProperty(keys[j]);
                        if (properotyInfo != null)
                        {
                            try
                            {
                                // Excel单元格的值转换为对象属性的值，若类型不对，记录出错信息
                                properotyInfo.SetValue(en,
                                    GetExcelCellToProperty(properotyInfo.PropertyType, sheet.GetRow(i).GetCell(j)), null);
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                    enlist.Add(en);
                }
            }
            return enlist;


        }

        /// <summary>
        /// 从Excel获取值传递到对象的属性里
        /// </summary>
        /// <param name="distanceType">目标对象类型</param>
        /// <param name="sourceCell">对象属性的值</param>
        private static Object GetExcelCellToProperty(Type distanceType, ICell sourceCell)
        {
            object rs = distanceType.IsValueType ? Activator.CreateInstance(distanceType) : null;

            // 1.判断传递的单元格是否为空
            if (sourceCell == null || string.IsNullOrEmpty(sourceCell.ToString()))
            {
                return rs;
            }

            // 2.Excel文本和数字单元格转换，在Excel里文本和数字是不能进行转换，所以这里预先存值
            object sourceValue = null;
            switch (sourceCell.CellType)
            {
                case CellType.Blank:
                    break;

                case CellType.Boolean:
                    break;

                case CellType.Error:
                    break;

                case CellType.Formula:
                    break;

                case CellType.Numeric: sourceValue = sourceCell.NumericCellValue;
                    break;

                case CellType.String: sourceValue = sourceCell.StringCellValue;
                    break;

                case CellType.Unknown:
                    break;

                default:
                    break;
            }

            string valueDataType = distanceType.Name;

            // 在这里进行特定类型的处理
            switch (valueDataType.ToLower()) // 以防出错，全部小写
            {
                case "string":
                    rs = sourceValue.ToString();
                    break;
                case "int":
                case "int16":
                case "int32":
                    rs = (int)Convert.ChangeType(sourceCell.NumericCellValue.ToString(), distanceType);
                    break;
                case "float":
                case "single":
                    rs = (float)Convert.ChangeType(sourceCell.NumericCellValue.ToString(), distanceType);
                    break;
                case "datetime":
                    rs = sourceCell.DateCellValue;
                    break;
                case "guid":
                    rs = (Guid)Convert.ChangeType(sourceCell.NumericCellValue.ToString(), distanceType);
                    return rs;
            }
            return rs;
        }
        /// <summary>
        ///     将制定sheet中的数据导出到datatable中
        /// </summary>
        /// <param name="sheet">需要导出的sheet</param>
        /// <param name="headerRowIndex">列头所在行号，-1表示没有列头</param>
        /// <param name="needHeader"></param>
        /// <returns></returns>
        private static DataTable ImportDt(ISheet sheet, int headerRowIndex, bool needHeader)
        {
            var table = new DataTable();
            IRow headerRow;
            int cellCount;
            try
            {
                if (headerRowIndex < 0 || !needHeader)
                {
                    headerRow = sheet.GetRow(0);
                    cellCount = headerRow.LastCellNum;

                    for (int i = headerRow.FirstCellNum; i <= cellCount; i++)
                    {
                        var column = new DataColumn(Convert.ToString(i));
                        table.Columns.Add(column);
                    }
                }
                else
                {
                    headerRow = sheet.GetRow(headerRowIndex);
                    cellCount = headerRow.LastCellNum;

                    for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                    {
                        if (headerRow.GetCell(i) == null)
                        {
                            if (table.Columns.IndexOf(Convert.ToString(i)) > 0)
                            {
                                var column = new DataColumn(Convert.ToString("重复列名" + i));
                                table.Columns.Add(column);
                            }
                            else
                            {
                                var column = new DataColumn(Convert.ToString(i));
                                table.Columns.Add(column);
                            }
                        }
                        else if (table.Columns.IndexOf(headerRow.GetCell(i).ToString()) > 0)
                        {
                            var column = new DataColumn(Convert.ToString("重复列名" + i));
                            table.Columns.Add(column);
                        }
                        else
                        {
                            var column = new DataColumn(headerRow.GetCell(i).ToString());
                            table.Columns.Add(column);
                        }
                    }
                }
                var rowCount = sheet.LastRowNum;
                for (var i = (headerRowIndex + 1); i <= sheet.LastRowNum; i++)
                {
                    try
                    {
                        IRow row;
                        if (sheet.GetRow(i) == null)
                        {
                            row = sheet.CreateRow(i);
                        }
                        else
                        {
                            row = sheet.GetRow(i);
                        }

                        var dataRow = table.NewRow();

                        for (int j = row.FirstCellNum; j <= cellCount; j++)
                        {
                            try
                            {
                                if (row.GetCell(j) != null)
                                {
                                    switch (row.GetCell(j).CellType)
                                    {
                                        case CellType.String:
                                            var str = row.GetCell(j).StringCellValue;
                                            if (str != null && str.Length > 0)
                                            {
                                                dataRow[j] = str;
                                            }
                                            else
                                            {
                                                dataRow[j] = null;
                                            }
                                            break;
                                        case CellType.Numeric:
                                            if (DateUtil.IsCellDateFormatted(row.GetCell(j)))
                                            {
                                                dataRow[j] = DateTime.FromOADate(row.GetCell(j).NumericCellValue);
                                            }
                                            else
                                            {
                                                dataRow[j] = Convert.ToDouble(row.GetCell(j).NumericCellValue);
                                            }
                                            break;
                                        case CellType.Boolean:
                                            dataRow[j] = Convert.ToString(row.GetCell(j).BooleanCellValue);
                                            break;
                                        case CellType.Error:
                                            dataRow[j] = ErrorEval.GetText(row.GetCell(j).ErrorCellValue);
                                            break;
                                        case CellType.Formula:
                                            switch (row.GetCell(j).CachedFormulaResultType)
                                            {
                                                case CellType.String:
                                                    var strFORMULA = row.GetCell(j).StringCellValue;
                                                    if (strFORMULA != null && strFORMULA.Length > 0)
                                                    {
                                                        dataRow[j] = strFORMULA;
                                                    }
                                                    else
                                                    {
                                                        dataRow[j] = null;
                                                    }
                                                    break;
                                                case CellType.Numeric:
                                                    dataRow[j] = Convert.ToString(row.GetCell(j).NumericCellValue);
                                                    break;
                                                case CellType.Boolean:
                                                    dataRow[j] = Convert.ToString(row.GetCell(j).BooleanCellValue);
                                                    break;
                                                case CellType.Error:
                                                    dataRow[j] = ErrorEval.GetText(row.GetCell(j).ErrorCellValue);
                                                    break;
                                                default:
                                                    dataRow[j] = "";
                                                    break;
                                            }
                                            break;
                                        default:
                                            dataRow[j] = "";
                                            break;
                                    }
                                }
                            }
                            catch (Exception exception)
                            {
                            }
                        }
                        table.Rows.Add(dataRow);
                    }
                    catch (Exception exception)
                    {
                    }
                }
            }
            catch (Exception exception)
            {
            }
            return table;
        }

        #endregion

        #region 更新excel中的数据

        /// <summary>
        ///     更新Excel表格
        /// </summary>
        /// <param name="outputFile">需更新的excel表格路径</param>
        /// <param name="sheetname">sheet名</param>
        /// <param name="updateData">需更新的数据</param>
        /// <param name="coluid">需更新的列号</param>
        /// <param name="rowid">需更新的开始行号</param>
        public static void UpdateExcel(string outputFile, string sheetname, string[] updateData, int coluid, int rowid)
        {
            //FileStream readfile = new FileStream(outputFile, FileMode.Open, FileAccess.Read);
            IWorkbook hssfworkbook = null; // WorkbookFactory.Create(outputFile);
            //HSSFWorkbook hssfworkbook = new HSSFWorkbook(readfile);
            var sheet1 = hssfworkbook.GetSheet(sheetname);
            for (var i = 0; i < updateData.Length; i++)
            {
                try
                {
                    if (sheet1.GetRow(i + rowid) == null)
                    {
                        sheet1.CreateRow(i + rowid);
                    }
                    if (sheet1.GetRow(i + rowid).GetCell(coluid) == null)
                    {
                        sheet1.GetRow(i + rowid).CreateCell(coluid);
                    }

                    sheet1.GetRow(i + rowid).GetCell(coluid).SetCellValue(updateData[i]);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            try
            {
                //readfile.Close();
                var writefile = new FileStream(outputFile, FileMode.OpenOrCreate, FileAccess.Write);
                hssfworkbook.Write(writefile);
                writefile.Close();
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        ///     更新Excel表格
        /// </summary>
        /// <param name="outputFile">需更新的excel表格路径</param>
        /// <param name="sheetname">sheet名</param>
        /// <param name="updateData">需更新的数据</param>
        /// <param name="coluids">需更新的列号</param>
        /// <param name="rowid">需更新的开始行号</param>
        public static void UpdateExcel(string outputFile, string sheetname, string[][] updateData, int[] coluids, int rowid)
        {
            var readfile = new FileStream(outputFile, FileMode.Open, FileAccess.Read);

            var hssfworkbook = new HSSFWorkbook(readfile);
            readfile.Close();
            var sheet1 = hssfworkbook.GetSheet(sheetname);
            for (var j = 0; j < coluids.Length; j++)
            {
                for (var i = 0; i < updateData[j].Length; i++)
                {
                    try
                    {
                        if (sheet1.GetRow(i + rowid) == null)
                        {
                            sheet1.CreateRow(i + rowid);
                        }
                        if (sheet1.GetRow(i + rowid).GetCell(coluids[j]) == null)
                        {
                            sheet1.GetRow(i + rowid).CreateCell(coluids[j]);
                        }
                        sheet1.GetRow(i + rowid).GetCell(coluids[j]).SetCellValue(updateData[j][i]);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            try
            {
                var writefile = new FileStream(outputFile, FileMode.Create);
                hssfworkbook.Write(writefile);
                writefile.Close();
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        ///     更新Excel表格
        /// </summary>
        /// <param name="outputFile">需更新的excel表格路径</param>
        /// <param name="sheetname">sheet名</param>
        /// <param name="updateData">需更新的数据</param>
        /// <param name="coluid">需更新的列号</param>
        /// <param name="rowid">需更新的开始行号</param>
        public static void UpdateExcel(string outputFile, string sheetname, double[] updateData, int coluid, int rowid)
        {
            var readfile = new FileStream(outputFile, FileMode.Open, FileAccess.Read);

            var hssfworkbook = new HSSFWorkbook(readfile);
            var sheet1 = hssfworkbook.GetSheet(sheetname);
            for (var i = 0; i < updateData.Length; i++)
            {
                try
                {
                    if (sheet1.GetRow(i + rowid) == null)
                    {
                        sheet1.CreateRow(i + rowid);
                    }
                    if (sheet1.GetRow(i + rowid).GetCell(coluid) == null)
                    {
                        sheet1.GetRow(i + rowid).CreateCell(coluid);
                    }

                    sheet1.GetRow(i + rowid).GetCell(coluid).SetCellValue(updateData[i]);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            try
            {
                readfile.Close();
                var writefile = new FileStream(outputFile, FileMode.Create, FileAccess.Write);
                hssfworkbook.Write(writefile);
                writefile.Close();
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        ///     更新Excel表格
        /// </summary>
        /// <param name="outputFile">需更新的excel表格路径</param>
        /// <param name="sheetname">sheet名</param>
        /// <param name="updateData">需更新的数据</param>
        /// <param name="coluids">需更新的列号</param>
        /// <param name="rowid">需更新的开始行号</param>
        public static void UpdateExcel(string outputFile, string sheetname, double[][] updateData, int[] coluids, int rowid)
        {
            var readfile = new FileStream(outputFile, FileMode.Open, FileAccess.Read);

            var hssfworkbook = new HSSFWorkbook(readfile);
            readfile.Close();
            var sheet1 = hssfworkbook.GetSheet(sheetname);
            for (var j = 0; j < coluids.Length; j++)
            {
                for (var i = 0; i < updateData[j].Length; i++)
                {
                    try
                    {
                        if (sheet1.GetRow(i + rowid) == null)
                        {
                            sheet1.CreateRow(i + rowid);
                        }
                        if (sheet1.GetRow(i + rowid).GetCell(coluids[j]) == null)
                        {
                            sheet1.GetRow(i + rowid).CreateCell(coluids[j]);
                        }
                        sheet1.GetRow(i + rowid).GetCell(coluids[j]).SetCellValue(updateData[j][i]);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            try
            {
                var writefile = new FileStream(outputFile, FileMode.Create);
                hssfworkbook.Write(writefile);
                writefile.Close();
            }
            catch (Exception ex)
            {
            }
        }

        #endregion
    }
}