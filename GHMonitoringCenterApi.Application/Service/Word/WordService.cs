using Aspose.Words.Tables;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report;
using GHMonitoringCenterApi.Application.Contracts.Dto.Word;
using GHMonitoringCenterApi.Application.Contracts.IService.Project;
using GHMonitoringCenterApi.Application.Contracts.IService.Word;
using GHMonitoringCenterApi.Application.Service.File;
using Microsoft.Extensions.Logging;
using NPOI.OpenXmlFormats.Dml;
using NPOI.OpenXmlFormats.Shared;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.Streaming.Values;
using NPOI.XSSF.UserModel;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using static NPOI.XWPF.UserModel.XWPFTableCell;

namespace GHMonitoringCenterApi.Application.Service.Word
{
    /// <summary>
    /// word相关操作实现层
    /// </summary>
    public class WordService : IWordService
    {


        #region 依赖注入
        public ILogger<WordService> logger { get; set; }

        public IProjectReportService projectReportService { get; set; }
        public WordService(ILogger<WordService> logger, IProjectReportService projectReportService)
        {
            this.logger = logger;
            this.projectReportService = projectReportService;
        }
        #endregion

        #region 项目月报简报导出word
        /// <summary>
        /// 项目月报简报导出word
        /// </summary>
        /// <param name="baseConfig"></param>
        /// <returns></returns>
        public async Task<Stream> MonthReportImportWordAsync(BaseConfig baseConfig, MonthtReportsRequstDto model)
        {
            MemoryStream memoryStream = new MemoryStream();
            var fileName = Guid.NewGuid().ToString();
            var path = $"Template/{fileName}.docx";
            //var path = @$"C:\Users\12413\Desktop\{fileName}.docx";
            XWPFDocument word = new XWPFDocument();
            #region 基本配置
            var paragraph = word.CreateParagraph();
            var run = paragraph.CreateRun();
            //添加图片
            run.AddPicture(baseConfig.WordImageSetup.LogoStream, (int)baseConfig.WordImageSetup.type,
               baseConfig.WordImageSetup.FileName, baseConfig.WordImageSetup.Width, baseConfig.WordImageSetup.Height);
            //设置居中
            paragraph.Alignment = ParagraphAlignment.CENTER;
            //添加三个换行符
            for (int i = 0; i < 3; i++)
            {
                run.AddCarriageReturn();
            }
            //设置字体
            run.FontSize = baseConfig.Size;
            run.FontFamily = baseConfig.Foot;
            run.SetText(baseConfig.Title);
            run.IsBold = true;
            for (int i = 0; i < 1; i++)
            {
                run.AddCarriageReturn();
            }
            run = paragraph.CreateRun();
            run.IsBold = true;
            run.FontSize = baseConfig.Size - 8;
            run.SetText($"（{baseConfig.Time}）");
            for (int i = 0; i < 8; i++)
            {
                run.AddCarriageReturn();
            }
            run = paragraph.CreateRun();
            run.FontSize = baseConfig.Size - 12;
            run.SetText(baseConfig.SubTitle);
            run.IsBold = true;
            run = paragraph.CreateRun();
            for (int i = 0; i < 1; i++)
            {
                run.AddCarriageReturn();
            }
            run.SetText(baseConfig.SubTime);
            run.IsBold = true;
            //添加换页符
            run.AddBreak();
            #endregion
            model.IsFullExport = true;
            if (model.StartTime == null)
            {
                if (DateTime.Now.Day > 25)
                {
                    model.StartTime = DateTime.Now;
                }
                else
                {
                    model.StartTime = DateTime.Now.AddMonths(-1);
                }
            }
            #region 生成worod
            var data = await projectReportService.SearchMonthReportProjectWordAsync(model);
            var collectionRatio = data.Data.OrderBy(x => x.ProjectProportion).ToList();
            var outputValue = data.Data.OrderBy(x => x.OwnerProportion).ToList();

            int Number = 0;

            #region 收款比例低十大项目
            if (collectionRatio.Any())
            {
                var paragraph2 = word.CreateParagraph();
                paragraph2.Alignment = ParagraphAlignment.CENTER;
                var run2 = paragraph2.CreateRun();
                run2.SetText("收款比例低十大项目");
                run2.IsBold = true;

                #region 基本配置
                XWPFTable oneTable = word.CreateTable(11, 8);
                CT_TcPr ctPr = new CT_TcPr();
                CT_Tbl ctbl = oneTable.GetCTTbl();
                //清空单元格样式
                ctbl.tblPr = new CT_TblPr();
                ctbl.tblPr.tblLayout = new CT_TblLayoutType();
                ctbl.tblPr.tblLayout.type = ST_TblLayoutType.@fixed;
                for (int i = 0; i < oneTable.Rows.Count; i++)
                {
                    XWPFTableRow row = oneTable.GetRow(i);
                    for (int col = 0; col < row.GetTableCells().Count; col++)
                    {

                        XWPFTableCell cell = oneTable.GetRow(i).GetCell(col);
                        //paragraph.Alignment = ParagraphAlignment.CENTER;
                        // 设置单元格样式
                        CT_TcPr tcPr = cell.GetCTTc().AddNewTcPr();
                        CT_VerticalJc vertAlign = new CT_VerticalJc();
                        vertAlign.val = ST_VerticalJc.center; // 垂直居中
                        tcPr.AddNewVAlign().val = ST_VerticalJc.center; // 垂直居中
                        tcPr.AddNewTcW().w = "500"; // 设置单元格宽度（可调整）
                    }
                }
                //增加单元格新样式
                CT_TblBorders tblBorders = new CT_TblBorders();
                CT_Border border = new CT_Border();
                border.val = ST_Border.single;
                border.color = "000000"; // 边框颜色，这里使用黑色，默认为自动颜色
                tblBorders.top = border;
                tblBorders.bottom = border;
                tblBorders.left = border;
                tblBorders.right = border;
                tblBorders.insideH = border; // 表格内部水平边框
                tblBorders.insideV = border; // 表格内垂直边框
                ctbl.tblPr.tblBorders = tblBorders;

                for (int i = 0; i < oneTable.Rows.Count; i++)
                {
                    XWPFTableRow row = oneTable.GetRow(i);
                    row.Height = 500; // 设置行高为 500，单位为 1/20 磅
                }
                oneTable.Width = 5000;
                #endregion

                #region 合并单元格
                oneTable.GetRow(0).MergeCells(2, 4);
                oneTable.GetRow(1).MergeCells(2, 4);
                oneTable.GetRow(2).MergeCells(2, 4);
                oneTable.GetRow(3).MergeCells(2, 4);
                oneTable.GetRow(4).MergeCells(2, 4);
                oneTable.GetRow(5).MergeCells(2, 4);
                oneTable.GetRow(6).MergeCells(2, 4);
                oneTable.GetRow(7).MergeCells(2, 4);
                oneTable.GetRow(8).MergeCells(2, 4);
                oneTable.GetRow(9).MergeCells(2, 4);
                oneTable.GetRow(10).MergeCells(2, 4);
                #endregion

                #region 填充数据
                var collectionRatioCount = collectionRatio.Count >= 10 ? 10 : collectionRatio.Count;
                XWPFTableCell cells = oneTable.GetRow(0).GetCell(0);
                // 创建一个段落对象以存放加粗字体
                XWPFParagraph para = cells.Paragraphs[0];
                XWPFRun runs = para.CreateRun();
                // 设置段落文本和字体样式
                runs.SetText("序号");
                runs.IsBold = true;
                // 将样式应用到文档中
                cells.SetParagraph(para);
                oneTable.GetRow(1).GetCell(0).SetText("1");
                oneTable.GetRow(2).GetCell(0).SetText("2");
                oneTable.GetRow(3).GetCell(0).SetText("3");
                oneTable.GetRow(4).GetCell(0).SetText("4");
                oneTable.GetRow(5).GetCell(0).SetText("5");
                oneTable.GetRow(6).GetCell(0).SetText("6");
                oneTable.GetRow(7).GetCell(0).SetText("7");
                oneTable.GetRow(8).GetCell(0).SetText("8");
                oneTable.GetRow(9).GetCell(0).SetText("9");
                oneTable.GetRow(10).GetCell(0).SetText("10");

                cells = oneTable.GetRow(0).GetCell(1);
                // 创建一个段落对象以存放加粗字体
                para = cells.Paragraphs[0];
                runs = para.CreateRun();
                // 设置段落文本和字体样式
                runs.SetText("项目归属");
                runs.IsBold = true;
                // 将样式应用到文档中
                cells.SetParagraph(para);
                cells = oneTable.GetRow(0).GetCell(2);
                // 创建一个段落对象以存放加粗字体
                para = cells.Paragraphs[0];
                runs = para.CreateRun();
                // 设置段落文本和字体样式
                runs.SetText("项目名称");
                runs.IsBold = true;
                // 将样式应用到文档中
                cells.SetParagraph(para);
                cells = oneTable.GetRow(0).GetCell(3);
                // 创建一个段落对象以存放加粗字体
                para = cells.Paragraphs[0];
                runs = para.CreateRun();
                // 设置段落文本和字体样式
                runs.SetText("产值完成比例");
                runs.IsBold = true;
                // 将样式应用到文档中
                cells.SetParagraph(para);
                cells = oneTable.GetRow(0).GetCell(4);
                // 创建一个段落对象以存放加粗字体
                para = cells.Paragraphs[0];
                runs = para.CreateRun();
                // 设置段落文本和字体样式
                runs.SetText("产值确认比例");
                runs.IsBold = true;
                // 将样式应用到文档中
                cells.SetParagraph(para);
                cells = oneTable.GetRow(0).GetCell(5);
                // 创建一个段落对象以存放加粗字体
                para = cells.Paragraphs[0];
                runs = para.CreateRun();
                // 设置段落文本和字体样式
                runs.SetText("实际收款比例");
                runs.IsBold = true;
                // 将样式应用到文档中
                cells.SetParagraph(para);
                for (int i = 0; i < collectionRatioCount; i++)
                {
                    oneTable.GetRow(i + 1).GetCell(1).SetText(collectionRatio[i].CompanyName);

                    oneTable.GetRow(i + 1).GetCell(2).SetText(collectionRatio[i].ProjectName);

                    oneTable.GetRow(i + 1).GetCell(3).SetText(Math.Round((collectionRatio[i].EngineeringProportion * 100).Value, 2).ToString() + "%");

                    oneTable.GetRow(i + 1).GetCell(4).SetText(Math.Round((collectionRatio[i].OwnerProportion * 100).Value, 2).ToString() + "%");

                    oneTable.GetRow(i + 1).GetCell(5).SetText(Math.Round((collectionRatio[i].ProjectProportion * 100).Value, 2).ToString() + "%");
                }
                #endregion

                #region 单元格属性
                foreach (XWPFTableRow row in oneTable.Rows)
                {
                    foreach (XWPFTableCell cell in row.GetTableCells())
                    {
                        foreach (XWPFParagraph paragraphs in cell.Paragraphs)
                        {
                            if ((oneTable.Rows.IndexOf(row) == 1 && row.GetTableCells().IndexOf(cell) == 2)
                                || (oneTable.Rows.IndexOf(row) == 2 && row.GetTableCells().IndexOf(cell) == 2)
                                || (oneTable.Rows.IndexOf(row) == 3 && row.GetTableCells().IndexOf(cell) == 2)
                                || (oneTable.Rows.IndexOf(row) == 4 && row.GetTableCells().IndexOf(cell) == 2)
                                || (oneTable.Rows.IndexOf(row) == 5 && row.GetTableCells().IndexOf(cell) == 2)
                                || (oneTable.Rows.IndexOf(row) == 6 && row.GetTableCells().IndexOf(cell) == 2)
                                || (oneTable.Rows.IndexOf(row) == 7 && row.GetTableCells().IndexOf(cell) == 2)
                                || (oneTable.Rows.IndexOf(row) == 8 && row.GetTableCells().IndexOf(cell) == 2)
                                || (oneTable.Rows.IndexOf(row) == 9 && row.GetTableCells().IndexOf(cell) == 2)
                                || (oneTable.Rows.IndexOf(row) == 10 && row.GetTableCells().IndexOf(cell) == 2))

                            {
                                paragraphs.Alignment = ParagraphAlignment.LEFT;
                            }
                            else
                            {
                                paragraphs.Alignment = ParagraphAlignment.CENTER;
                            }

                        }
                    }
                }
                #endregion


                //换页
                var paragraph1 = word.CreateParagraph();
                var run1 = paragraph1.CreateRun();
                run1.AddBreak();
            }
            #endregion

            #region 产值确认比例低十大项目
            if (outputValue.Any())
            {
                var paragraph2 = word.CreateParagraph();
                paragraph2.Alignment = ParagraphAlignment.CENTER;
                var run2 = paragraph2.CreateRun();
                run2.SetText("产值确认比例低十大项目");
                run2.IsBold = true;

                #region 基本配置
                XWPFTable oneTable = word.CreateTable(11, 8);
                CT_TcPr ctPr = new CT_TcPr();
                CT_Tbl ctbl = oneTable.GetCTTbl();
                //清空单元格样式
                ctbl.tblPr = new CT_TblPr();
                ctbl.tblPr.tblLayout = new CT_TblLayoutType();
                ctbl.tblPr.tblLayout.type = ST_TblLayoutType.@fixed;
                for (int i = 0; i < oneTable.Rows.Count; i++)
                {
                    XWPFTableRow row = oneTable.GetRow(i);
                    for (int col = 0; col < row.GetTableCells().Count; col++)
                    {

                        XWPFTableCell cell = oneTable.GetRow(i).GetCell(col);
                        //paragraph.Alignment = ParagraphAlignment.CENTER;
                        // 设置单元格样式
                        CT_TcPr tcPr = cell.GetCTTc().AddNewTcPr();
                        CT_VerticalJc vertAlign = new CT_VerticalJc();
                        vertAlign.val = ST_VerticalJc.center; // 垂直居中
                        tcPr.AddNewVAlign().val = ST_VerticalJc.center; // 垂直居中
                        tcPr.AddNewTcW().w = "500"; // 设置单元格宽度（可调整）
                    }
                }
                //增加单元格新样式
                CT_TblBorders tblBorders = new CT_TblBorders();
                CT_Border border = new CT_Border();
                border.val = ST_Border.single;
                border.color = "000000"; // 边框颜色，这里使用黑色，默认为自动颜色
                tblBorders.top = border;
                tblBorders.bottom = border;
                tblBorders.left = border;
                tblBorders.right = border;
                tblBorders.insideH = border; // 表格内部水平边框
                tblBorders.insideV = border; // 表格内垂直边框
                ctbl.tblPr.tblBorders = tblBorders;

                for (int i = 0; i < oneTable.Rows.Count; i++)
                {
                    XWPFTableRow row = oneTable.GetRow(i);
                    row.Height = 500; // 设置行高为 500，单位为 1/20 磅
                }
                oneTable.Width = 5000;
                #endregion

                #region 合并单元格
                oneTable.GetRow(0).MergeCells(2, 4);
                oneTable.GetRow(1).MergeCells(2, 4);
                oneTable.GetRow(2).MergeCells(2, 4);
                oneTable.GetRow(3).MergeCells(2, 4);
                oneTable.GetRow(4).MergeCells(2, 4);
                oneTable.GetRow(5).MergeCells(2, 4);
                oneTable.GetRow(6).MergeCells(2, 4);
                oneTable.GetRow(7).MergeCells(2, 4);
                oneTable.GetRow(8).MergeCells(2, 4);
                oneTable.GetRow(9).MergeCells(2, 4);
                oneTable.GetRow(10).MergeCells(2, 4);
                #endregion

                #region 填充数据
                var outputValueCount = outputValue.Count >= 10 ? 10 : outputValue.Count;
                XWPFTableCell cells = oneTable.GetRow(0).GetCell(0);
                // 创建一个段落对象以存放加粗字体
                XWPFParagraph para = cells.Paragraphs[0];
                XWPFRun runs = para.CreateRun();
                // 设置段落文本和字体样式
                runs.SetText("序号");
                runs.IsBold = true;
                // 将样式应用到文档中
                cells.SetParagraph(para);
                oneTable.GetRow(1).GetCell(0).SetText("1");
                oneTable.GetRow(2).GetCell(0).SetText("2");
                oneTable.GetRow(3).GetCell(0).SetText("3");
                oneTable.GetRow(4).GetCell(0).SetText("4");
                oneTable.GetRow(5).GetCell(0).SetText("5");
                oneTable.GetRow(6).GetCell(0).SetText("6");
                oneTable.GetRow(7).GetCell(0).SetText("7");
                oneTable.GetRow(8).GetCell(0).SetText("8");
                oneTable.GetRow(9).GetCell(0).SetText("9");
                oneTable.GetRow(10).GetCell(0).SetText("10");
                cells = oneTable.GetRow(0).GetCell(1);
                // 创建一个段落对象以存放加粗字体
                para = cells.Paragraphs[0];
                runs = para.CreateRun();
                // 设置段落文本和字体样式
                runs.SetText("项目归属");
                runs.IsBold = true;
                // 将样式应用到文档中
                cells.SetParagraph(para);
                cells = oneTable.GetRow(0).GetCell(2);
                // 创建一个段落对象以存放加粗字体
                para = cells.Paragraphs[0];
                runs = para.CreateRun();
                // 设置段落文本和字体样式
                runs.SetText("项目名称");
                runs.IsBold = true;
                // 将样式应用到文档中
                cells.SetParagraph(para);
                cells = oneTable.GetRow(0).GetCell(3);
                // 创建一个段落对象以存放加粗字体
                para = cells.Paragraphs[0];
                runs = para.CreateRun();
                // 设置段落文本和字体样式
                runs.SetText("实际产值完成比例");
                runs.IsBold = true;
                // 将样式应用到文档中
                cells.SetParagraph(para);
                cells = oneTable.GetRow(0).GetCell(4);
                // 创建一个段落对象以存放加粗字体
                para = cells.Paragraphs[0];
                runs = para.CreateRun();
                // 设置段落文本和字体样式
                runs.SetText("产值确认比例");
                runs.IsBold = true;
                // 将样式应用到文档中
                cells.SetParagraph(para);
                cells = oneTable.GetRow(0).GetCell(5);
                // 创建一个段落对象以存放加粗字体
                para = cells.Paragraphs[0];
                runs = para.CreateRun();
                // 设置段落文本和字体样式
                runs.SetText("备注");
                runs.IsBold = true;
                // 将样式应用到文档中
                cells.SetParagraph(para);
                for (int i = 0; i < outputValueCount; i++)
                {


                    oneTable.GetRow(i + 1).GetCell(1).SetText(outputValue[i].CompanyName);

                    oneTable.GetRow(i + 1).GetCell(2).SetText(outputValue[i].ProjectName);

                    oneTable.GetRow(i + 1).GetCell(3).SetText(Math.Round((outputValue[i].EngineeringProportion * 100).Value, 2).ToString() + "%");

                    oneTable.GetRow(i + 1).GetCell(4).SetText(Math.Round((outputValue[i].OwnerProportion * 100).Value, 2).ToString() + "%");

                    oneTable.GetRow(i + 1).GetCell(5).SetText(outputValue[0].EngineeringRemarks);

                }
                #endregion

                #region 单元格属性
                foreach (XWPFTableRow row in oneTable.Rows)
                {
                    foreach (XWPFTableCell cell in row.GetTableCells())
                    {
                        foreach (XWPFParagraph paragraphs in cell.Paragraphs)
                        {
                            if ((oneTable.Rows.IndexOf(row) == 1 && row.GetTableCells().IndexOf(cell) == 2)
                                || (oneTable.Rows.IndexOf(row) == 2 && row.GetTableCells().IndexOf(cell) == 2)
                                || (oneTable.Rows.IndexOf(row) == 3 && row.GetTableCells().IndexOf(cell) == 2)
                                || (oneTable.Rows.IndexOf(row) == 4 && row.GetTableCells().IndexOf(cell) == 2)
                                || (oneTable.Rows.IndexOf(row) == 5 && row.GetTableCells().IndexOf(cell) == 2)
                                || (oneTable.Rows.IndexOf(row) == 6 && row.GetTableCells().IndexOf(cell) == 2)
                                || (oneTable.Rows.IndexOf(row) == 7 && row.GetTableCells().IndexOf(cell) == 2)
                                || (oneTable.Rows.IndexOf(row) == 8 && row.GetTableCells().IndexOf(cell) == 2)
                                || (oneTable.Rows.IndexOf(row) == 9 && row.GetTableCells().IndexOf(cell) == 2)
                                || (oneTable.Rows.IndexOf(row) == 10 && row.GetTableCells().IndexOf(cell) == 2))

                            {
                                paragraphs.Alignment = ParagraphAlignment.LEFT;
                            }
                            else
                            {
                                paragraphs.Alignment = ParagraphAlignment.CENTER;
                            }

                        }
                    }
                }
                #endregion

                //换页
                var paragraph1 = word.CreateParagraph();
                var run1 = paragraph1.CreateRun();
                run1.AddBreak();
            }
            #endregion

            foreach (var item in data.Data)
            {
                Number++;
                var paragraph2 = word.CreateParagraph();
                var run2 = paragraph2.CreateRun();
                run2.SetText(Number + "." + item.ProjectName);
                XWPFTable oneTable = word.CreateTable(16, 7);

                #region 设置单元格样式
                CT_TcPr ctPr = new CT_TcPr();
                CT_Tbl ctbl = oneTable.GetCTTbl();
                //清空单元格样式
                ctbl.tblPr = new CT_TblPr();
                ctbl.tblPr.tblLayout = new CT_TblLayoutType();
                ctbl.tblPr.tblLayout.type = ST_TblLayoutType.@fixed;
                for (int i = 0; i < oneTable.Rows.Count; i++)
                {
                    XWPFTableRow row = oneTable.GetRow(i);
                    for (int col = 0; col < row.GetTableCells().Count; col++)
                    {

                        XWPFTableCell cell = oneTable.GetRow(i).GetCell(col);
                        //paragraph.Alignment = ParagraphAlignment.CENTER;
                        // 设置单元格样式
                        CT_TcPr tcPr = cell.GetCTTc().AddNewTcPr();
                        CT_VerticalJc vertAlign = new CT_VerticalJc();
                        vertAlign.val = ST_VerticalJc.center; // 垂直居中
                        tcPr.AddNewVAlign().val = ST_VerticalJc.center; // 垂直居中
                        tcPr.AddNewTcW().w = "500"; // 设置单元格宽度（可调整）
                    }
                }
                //增加单元格新样式
                CT_TblBorders tblBorders = new CT_TblBorders();
                CT_Border border = new CT_Border();
                border.val = ST_Border.single;
                border.color = "000000"; // 边框颜色，这里使用黑色，默认为自动颜色
                tblBorders.top = border;
                tblBorders.bottom = border;
                tblBorders.left = border;
                tblBorders.right = border;
                tblBorders.insideH = border; // 表格内部水平边框
                tblBorders.insideV = border; // 表格内垂直边框
                ctbl.tblPr.tblBorders = tblBorders;

                for (int i = 0; i < oneTable.Rows.Count; i++)
                {
                    XWPFTableRow row = oneTable.GetRow(i);
                    row.Height = 500; // 设置行高为 500，单位为 1/20 磅
                }
                oneTable.Width = 5000;
                #endregion

                #region 调整单元格列宽
                for (int i = 0; i < 7; i++)
                {
                    ctPr = oneTable.GetRow(8).GetCell(i).GetCTTc().AddNewTcPr();
                    ctPr.tcW = new CT_TblWidth();
                    ctPr.tcW.w = "3000";//单元格宽
                    ctPr.tcW.type = ST_TblWidth.dxa;
                }
                #endregion

                XWPFTableCell cells = oneTable.GetRow(8).GetCell(0);
                CT_TcPr tcPrs = cells.GetCTTc().AddNewTcPr();
                CT_VerticalJc vertAligns = new CT_VerticalJc();
                vertAligns.val = ST_VerticalJc.center; // 垂直居中
                tcPrs.AddNewVAlign().val = ST_VerticalJc.center; // 垂直居中
                cells = oneTable.GetRow(8).GetCell(1);
                tcPrs = cells.GetCTTc().AddNewTcPr();
                vertAligns = new CT_VerticalJc();
                vertAligns.val = ST_VerticalJc.center; // 垂直居中
                tcPrs.AddNewVAlign().val = ST_VerticalJc.center; // 垂直居中

                #region 合并单元格（同列不同行）
                MYMergeCells(oneTable, 0, 0, 9, 12);
                #endregion

                #region 合并单元格（同行不同列）
                oneTable.GetRow(0).MergeCells(1, 6);

                oneTable.GetRow(1).MergeCells(1, 3);
                oneTable.GetRow(1).MergeCells(3, 4);

                oneTable.GetRow(2).MergeCells(1, 3);
                oneTable.GetRow(2).MergeCells(3, 4);

                oneTable.GetRow(3).MergeCells(1, 6);

                oneTable.GetRow(4).MergeCells(1, 6);

                oneTable.GetRow(5).MergeCells(1, 2);
                oneTable.GetRow(5).MergeCells(2, 3);
                oneTable.GetRow(5).MergeCells(3, 4);

                oneTable.GetRow(6).MergeCells(1, 6);

                oneTable.GetRow(7).MergeCells(1, 6);

                oneTable.GetRow(8).MergeCells(1, 6);

                oneTable.GetRow(13).MergeCells(1, 6);
                oneTable.GetRow(14).MergeCells(1, 6);
                oneTable.GetRow(15).MergeCells(1, 6);
                #endregion

                #region 填充单元格
                oneTable.GetRow(0).GetCell(0).SetText("项目名称");
                oneTable.GetRow(0).GetCell(0).SetColor("#EEEEEE");
                //XWPFTableCell cellr = oneTable.GetRow(0).GetCell(1);
                //XWPFParagraph paragraph3 = cellr.AddParagraph();
                //XWPFRun run3 = paragraph3.CreateRun();
                //run3.SetText(item.ProjectName);
                //paragraph3.Alignment = ParagraphAlignment.LEFT;
                oneTable.GetRow(0).GetCell(1).SetText(item.ProjectName);

                oneTable.GetRow(1).GetCell(0).SetText("所属公司");
                oneTable.GetRow(1).GetCell(0).SetColor("#EEEEEE");
                oneTable.GetRow(1).GetCell(1).SetText(item.CompanyName);
                oneTable.GetRow(1).GetCell(2).SetText("项目规模");
                oneTable.GetRow(1).GetCell(2).SetColor("#EEEEEE");
                oneTable.GetRow(1).GetCell(3).SetText(item.ProjectGrade);

                oneTable.GetRow(2).GetCell(0).SetText("项目类型");
                oneTable.GetRow(2).GetCell(0).SetColor("#EEEEEE");
                oneTable.GetRow(2).GetCell(1).SetText(item.ProjectType);
                oneTable.GetRow(2).GetCell(2).SetText("项目状态");
                oneTable.GetRow(2).GetCell(2).SetColor("#EEEEEE");
                oneTable.GetRow(2).GetCell(3).SetText(item.ProjectState);

                oneTable.GetRow(3).GetCell(0).SetText("项目位置");
                oneTable.GetRow(3).GetCell(0).SetColor("#EEEEEE");
                oneTable.GetRow(3).GetCell(1).SetText(item.ProjectLocation);

                oneTable.GetRow(4).GetCell(0).SetText("项目内容");
                oneTable.GetRow(4).GetCell(0).SetColor("#EEEEEE");
                oneTable.GetRow(4).GetCell(1).SetText(item.ProjectContent);

                oneTable.GetRow(5).GetCell(0).SetText("合同额(万元)");
                oneTable.GetRow(5).GetCell(0).SetColor("#EEEEEE");
                oneTable.GetRow(5).GetCell(1).SetText("原合同额:" + item.OriginalContractAmount.ToString());
                oneTable.GetRow(5).GetCell(2).SetText("变更额:" + item.ChangeAmount.ToString());
                oneTable.GetRow(5).GetCell(3).SetText("实际合同额:" + item.ActualContractAmount.ToString());

                oneTable.GetRow(6).GetCell(0).SetText("合同变更信息");
                oneTable.GetRow(6).GetCell(0).SetColor("#EEEEEE");
                oneTable.GetRow(6).GetCell(1).SetText(item.ContractChangeInformation);

                oneTable.GetRow(7).GetCell(0).SetText("工期信息");
                oneTable.GetRow(7).GetCell(0).SetColor("#EEEEEE");
                var gqorkg = string.Empty;
                if (string.IsNullOrEmpty(item.DurationInformation))
                {
                    if (!string.IsNullOrEmpty(item.CommencementTime))
                    {
                        gqorkg = item.CommencementTime;
                    }
                }
                else
                {
                    gqorkg = item.DurationInformation;
                }
                //oneTable.GetRow(7).GetCell(1).SetText(item.DurationInformation + item.CommencementTime);
                oneTable.GetRow(7).GetCell(1).SetText(gqorkg);

                oneTable.GetRow(8).GetCell(0).SetText("项目进展");
                oneTable.GetRow(8).GetCell(0).SetColor("#EEEEEE");
                oneTable.GetRow(8).GetCell(1).SetText(item.MonthProjectDebriefing);


                oneTable.GetRow(9).GetCell(0).SetText("完成产值、确认产值、工程收款（万元）");
                oneTable.GetRow(9).GetCell(0).SetColor("#EEEEEE");
                //oneTable.GetRow(9).GetCell(1).SetText(item.MonthProjectDebriefing);
                oneTable.GetRow(9).GetCell(1).SetColor("#F5F9FA");
                //oneTable.GetRow(10).GetCell(0).SetText("工程进度(万元)");
                //oneTable.GetRow(10).GetCell(0).SetColor("#D4F0FC");
                //oneTable.GetRow(10).GetCell(0).SetText("工程进度(万元)");
                //oneTable.GetRow(11).GetCell(0).SetText("工程进度(万元)");
                //oneTable.GetRow(12).GetCell(0).SetText("工程进度(万元)");
                //oneTable.GetRow(10).GetCell(0).SetColor("#D4F0FC");
                //oneTable.GetRow(11).GetCell(0).SetColor("#D4F0FC");
                //oneTable.GetRow(12).GetCell(0).SetColor("#D4F0FC");

                oneTable.GetRow(9).GetCell(1).SetText("指标项目");
                oneTable.GetRow(9).GetCell(1).SetColor("#D4F0FC");
                oneTable.GetRow(9).GetCell(2).SetText("本月");
                oneTable.GetRow(9).GetCell(2).SetColor("#D4F0FC");
                oneTable.GetRow(9).GetCell(3).SetText("本年");
                oneTable.GetRow(9).GetCell(3).SetColor("#D4F0FC");
                oneTable.GetRow(9).GetCell(4).SetText("工程累计");
                oneTable.GetRow(9).GetCell(4).SetColor("#D4F0FC");
                oneTable.GetRow(9).GetCell(5).SetText("比例");
                oneTable.GetRow(9).GetCell(5).SetColor("#D4F0FC");
                oneTable.GetRow(9).GetCell(6).SetText("备注");
                oneTable.GetRow(9).GetCell(6).SetColor("#D4F0FC");
                oneTable.GetRow(10).GetCell(1).SetText("工程产值");
                oneTable.GetRow(10).GetCell(1).SetColor("#F5F9FA");
                oneTable.GetRow(10).GetCell(2).SetText(item.EngineeringMonthOutputValue.ToString());
                oneTable.GetRow(10).GetCell(2).SetColor("#F5F9FA");
                oneTable.GetRow(10).GetCell(3).SetText(item.EngineeringYeahOutputValue.ToString());
                oneTable.GetRow(10).GetCell(3).SetColor("#F5F9FA");
                oneTable.GetRow(10).GetCell(4).SetText(item.EngineeringAccumulatedEngineering.ToString());
                oneTable.GetRow(10).GetCell(4).SetColor("#F5F9FA");
                oneTable.GetRow(10).GetCell(5).SetText(Math.Round((item.EngineeringProportion * 100).Value, 2).ToString() + "%");
                oneTable.GetRow(10).GetCell(5).SetColor("#F5F9FA");
                oneTable.GetRow(10).GetCell(6).SetText(item.EngineeringRemarks);
                oneTable.GetRow(10).GetCell(6).SetColor("#F5F9FA");

                oneTable.GetRow(11).GetCell(1).SetText("业主确认");
                oneTable.GetRow(11).GetCell(1).SetColor("#F5F9FA");
                oneTable.GetRow(11).GetCell(2).SetText(item.OwnerMonthOutputValue.ToString());
                oneTable.GetRow(11).GetCell(2).SetColor("#F5F9FA");
                oneTable.GetRow(11).GetCell(3).SetText(item.OwnerYeahOutputValue.ToString());
                oneTable.GetRow(11).GetCell(3).SetColor("#F5F9FA");
                oneTable.GetRow(11).GetCell(4).SetText(item.OwnerAccumulatedEngineering.ToString());
                oneTable.GetRow(11).GetCell(4).SetColor("#F5F9FA");
                oneTable.GetRow(11).GetCell(5).SetText(Math.Round((item.OwnerProportion * 100).Value, 2).ToString() + "%");
                oneTable.GetRow(11).GetCell(5).SetColor("#F5F9FA");
                oneTable.GetRow(11).GetCell(6).SetText(item.OwnerRemarks);
                oneTable.GetRow(11).GetCell(6).SetColor("#F5F9FA");

                oneTable.GetRow(12).GetCell(1).SetText("工程收款");
                oneTable.GetRow(12).GetCell(1).SetColor("#F5F9FA");
                oneTable.GetRow(12).GetCell(2).SetText(item.ProjectMonthOutputValue.ToString());
                oneTable.GetRow(12).GetCell(2).SetColor("#F5F9FA");
                oneTable.GetRow(12).GetCell(3).SetText(item.ProjectYeahOutputValue.ToString());
                oneTable.GetRow(12).GetCell(3).SetColor("#F5F9FA");
                oneTable.GetRow(12).GetCell(4).SetText(item.ProjectAccumulatedEngineering.ToString());
                oneTable.GetRow(12).GetCell(4).SetColor("#F5F9FA");
                oneTable.GetRow(12).GetCell(5).SetText(Math.Round((item.ProjectProportion * 100).Value, 2).ToString() + "%");
                oneTable.GetRow(12).GetCell(5).SetColor("#F5F9FA");
                oneTable.GetRow(12).GetCell(6).SetText(item.ProjectRemarks);
                oneTable.GetRow(12).GetCell(6).SetColor("#F5F9FA");

                oneTable.GetRow(13).GetCell(0).SetText("存在问题");
                oneTable.GetRow(13).GetCell(0).SetColor("#EEEEEE");
                oneTable.GetRow(13).GetCell(1).SetText(item.ExistingProblems);

                oneTable.GetRow(14).GetCell(0).SetText("解决措施");
                oneTable.GetRow(14).GetCell(0).SetColor("#EEEEEE");
                oneTable.GetRow(14).GetCell(1).SetText(item.TakeSteps);

                oneTable.GetRow(15).GetCell(0).SetText("需公司协调事项");
                oneTable.GetRow(15).GetCell(0).SetColor("#EEEEEE");
                oneTable.GetRow(15).GetCell(1).SetText(item.CoordinationMatters);
                #endregion

                #region 设置单元格对齐方式
                foreach (XWPFTableRow row in oneTable.Rows)
                {
                    foreach (XWPFTableCell cell in row.GetTableCells())
                    {
                        foreach (XWPFParagraph paragraphs in cell.Paragraphs)
                        {
                            if ((oneTable.Rows.IndexOf(row) == 0 && row.GetTableCells().IndexOf(cell) == 1)
                                || (oneTable.Rows.IndexOf(row) == 1 && row.GetTableCells().IndexOf(cell) == 1)
                                || (oneTable.Rows.IndexOf(row) == 1 && row.GetTableCells().IndexOf(cell) == 3)
                                || (oneTable.Rows.IndexOf(row) == 2 && row.GetTableCells().IndexOf(cell) == 1)
                                || (oneTable.Rows.IndexOf(row) == 2 && row.GetTableCells().IndexOf(cell) == 3)
                                || (oneTable.Rows.IndexOf(row) == 3 && row.GetTableCells().IndexOf(cell) == 1)
                                || (oneTable.Rows.IndexOf(row) == 4 && row.GetTableCells().IndexOf(cell) == 1)
                                || (oneTable.Rows.IndexOf(row) == 5 && row.GetTableCells().IndexOf(cell) == 1)
                                || (oneTable.Rows.IndexOf(row) == 5 && row.GetTableCells().IndexOf(cell) == 2)
                                || (oneTable.Rows.IndexOf(row) == 5 && row.GetTableCells().IndexOf(cell) == 3)
                                || (oneTable.Rows.IndexOf(row) == 6 && row.GetTableCells().IndexOf(cell) == 1)
                                || (oneTable.Rows.IndexOf(row) == 7 && row.GetTableCells().IndexOf(cell) == 1)
                                || (oneTable.Rows.IndexOf(row) == 8 && row.GetTableCells().IndexOf(cell) == 1)
                                || (oneTable.Rows.IndexOf(row) == 13 && row.GetTableCells().IndexOf(cell) == 1)
                                || (oneTable.Rows.IndexOf(row) == 14 && row.GetTableCells().IndexOf(cell) == 1)
                                || (oneTable.Rows.IndexOf(row) == 15 && row.GetTableCells().IndexOf(cell) == 1))
                            {
                                paragraphs.Alignment = ParagraphAlignment.LEFT;
                            }
                            else
                            {
                                paragraphs.Alignment = ParagraphAlignment.CENTER;
                            }

                        }
                    }
                }

                #endregion

                //换页
                var paragraph1 = word.CreateParagraph();
                var run1 = paragraph1.CreateRun();
                run1.AddBreak();
            }
            #endregion

            //return await Task.Factory.StartNew(() => {
            try
            {

                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                {
                    word.Write(fs);
                }
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    //读取文件流
                    byte[] bytes = new byte[fs.Length];
                    fs.Read(bytes, 0, bytes.Length);
                    memoryStream.Write(bytes, 0, bytes.Length);
                    #region 删除文件

                    #endregion
                }
                System.IO.File.Delete(path);
            }
            catch (Exception ex)
            {
                try
                {
                    System.IO.File.Delete(path);
                }
                catch (Exception exs)
                {
                    logger.LogError($"删除文件失败:{exs}");
                }
                logger.LogError($"项目月报简报导出Word失败错误信息:{ex}");
            }
            return memoryStream;
            //});
        }
        #endregion

        #region 项目月报简报导出Pdf
        /// <summary>
        /// 项目月报简报导出Pdf
        /// </summary>
        /// <param name="baseConfig"></param>
        /// <returns></returns>
        public async Task<Stream> MonthReportImportWordAsync1(BaseConfig baseConfig, MonthtReportsRequstDto model)
        {
            MemoryStream memoryStream = new MemoryStream();
            var fileName = Guid.NewGuid().ToString();
            var path = $"Template/Images/pdfExport.docx";
            //var path = @$"C:\Users\12413\Desktop\aaa.docx";
            XWPFDocument word = new XWPFDocument();
            #region 基本配置
            var paragraph = word.CreateParagraph();
            var run = paragraph.CreateRun();
            //添加图片
            run.AddPicture(baseConfig.WordImageSetup.LogoStream, (int)baseConfig.WordImageSetup.type,
               baseConfig.WordImageSetup.FileName, baseConfig.WordImageSetup.Width, baseConfig.WordImageSetup.Height);
            //设置居中
            paragraph.Alignment = ParagraphAlignment.CENTER;
            //添加三个换行符
            for (int i = 0; i < 3; i++)
            {
                run.AddCarriageReturn();
            }
            //设置字体
            run.FontSize = baseConfig.Size;
            run.FontFamily = baseConfig.Foot;
            run.SetText(baseConfig.Title);
            run.IsBold = true;
            for (int i = 0; i < 1; i++)
            {
                run.AddCarriageReturn();
            }
            run = paragraph.CreateRun();
            run.IsBold = true;
            run.FontSize = baseConfig.Size - 8;
            run.SetText($"（{baseConfig.Time}）");
            for (int i = 0; i < 8; i++)
            {
                run.AddCarriageReturn();
            }
            run = paragraph.CreateRun();
            run.FontSize = baseConfig.Size - 12;
            run.SetText(baseConfig.SubTitle);
            run.IsBold = true;
            run = paragraph.CreateRun();
            for (int i = 0; i < 1; i++)
            {
                run.AddCarriageReturn();
            }
            run.SetText(baseConfig.SubTime);
            run.IsBold = true;
            //添加换页符
            run.AddBreak();
            #endregion
            model.IsFullExport = true;
            if (model.StartTime == null)
            {
                if (DateTime.Now.Day > 25)
                {
                    model.StartTime = DateTime.Now;
                }
                else
                {
                    model.StartTime = DateTime.Now.AddMonths(-1);
                }
            }
            #region 生成worod
            var data = await projectReportService.SearchMonthReportProjectWordAsync(model);
            var collectionRatio = data.Data.OrderBy(x => x.ProjectProportion).ToList();
            var outputValue = data.Data.OrderBy(x => x.OwnerProportion).ToList();

            int Number = 0;

            #region 收款比例低十大项目
            if (collectionRatio.Any())
            {
                var paragraph2 = word.CreateParagraph();
                paragraph2.Alignment = ParagraphAlignment.CENTER;
                var run2 = paragraph2.CreateRun();
                run2.SetText("收款比例低十大项目");
                run2.IsBold = true;

                #region 基本配置
                XWPFTable oneTable = word.CreateTable(11, 8);
                CT_TcPr ctPr = new CT_TcPr();
                CT_Tbl ctbl = oneTable.GetCTTbl();
                //清空单元格样式
                ctbl.tblPr = new CT_TblPr();
                ctbl.tblPr.tblLayout = new CT_TblLayoutType();
                ctbl.tblPr.tblLayout.type = ST_TblLayoutType.@fixed;
                for (int i = 0; i < oneTable.Rows.Count; i++)
                {
                    XWPFTableRow row = oneTable.GetRow(i);
                    for (int col = 0; col < row.GetTableCells().Count; col++)
                    {

                        XWPFTableCell cell = oneTable.GetRow(i).GetCell(col);
                        //paragraph.Alignment = ParagraphAlignment.CENTER;
                        // 设置单元格样式
                        CT_TcPr tcPr = cell.GetCTTc().AddNewTcPr();
                        CT_VerticalJc vertAlign = new CT_VerticalJc();
                        vertAlign.val = ST_VerticalJc.center; // 垂直居中
                        tcPr.AddNewVAlign().val = ST_VerticalJc.center; // 垂直居中
                        tcPr.AddNewTcW().w = "500"; // 设置单元格宽度（可调整）
                    }
                }
                //增加单元格新样式
                CT_TblBorders tblBorders = new CT_TblBorders();
                CT_Border border = new CT_Border();
                border.val = ST_Border.single;
                border.color = "000000"; // 边框颜色，这里使用黑色，默认为自动颜色
                tblBorders.top = border;
                tblBorders.bottom = border;
                tblBorders.left = border;
                tblBorders.right = border;
                tblBorders.insideH = border; // 表格内部水平边框
                tblBorders.insideV = border; // 表格内垂直边框
                ctbl.tblPr.tblBorders = tblBorders;

                for (int i = 0; i < oneTable.Rows.Count; i++)
                {
                    XWPFTableRow row = oneTable.GetRow(i);
                    row.Height = 500; // 设置行高为 500，单位为 1/20 磅
                }
                oneTable.Width = 5000;
                #endregion

                #region 合并单元格
                oneTable.GetRow(0).MergeCells(2, 4);
                oneTable.GetRow(1).MergeCells(2, 4);
                oneTable.GetRow(2).MergeCells(2, 4);
                oneTable.GetRow(3).MergeCells(2, 4);
                oneTable.GetRow(4).MergeCells(2, 4);
                oneTable.GetRow(5).MergeCells(2, 4);
                oneTable.GetRow(6).MergeCells(2, 4);
                oneTable.GetRow(7).MergeCells(2, 4);
                oneTable.GetRow(8).MergeCells(2, 4);
                oneTable.GetRow(9).MergeCells(2, 4);
                oneTable.GetRow(10).MergeCells(2, 4);
                #endregion

                #region 填充数据
                var collectionRatioCount = collectionRatio.Count >= 10 ? 10 : collectionRatio.Count;
                XWPFTableCell cells = oneTable.GetRow(0).GetCell(0);
                // 创建一个段落对象以存放加粗字体
                XWPFParagraph para = cells.Paragraphs[0];
                XWPFRun runs = para.CreateRun();
                // 设置段落文本和字体样式
                runs.SetText("序号");
                runs.IsBold = true;
                // 将样式应用到文档中
                cells.SetParagraph(para);
                oneTable.GetRow(1).GetCell(0).SetText("1");
                oneTable.GetRow(2).GetCell(0).SetText("2");
                oneTable.GetRow(3).GetCell(0).SetText("3");
                oneTable.GetRow(4).GetCell(0).SetText("4");
                oneTable.GetRow(5).GetCell(0).SetText("5");
                oneTable.GetRow(6).GetCell(0).SetText("6");
                oneTable.GetRow(7).GetCell(0).SetText("7");
                oneTable.GetRow(8).GetCell(0).SetText("8");
                oneTable.GetRow(9).GetCell(0).SetText("9");
                oneTable.GetRow(10).GetCell(0).SetText("10");

                cells = oneTable.GetRow(0).GetCell(1);
                // 创建一个段落对象以存放加粗字体
                para = cells.Paragraphs[0];
                runs = para.CreateRun();
                // 设置段落文本和字体样式
                runs.SetText("项目归属");
                runs.IsBold = true;
                // 将样式应用到文档中
                cells.SetParagraph(para);
                cells = oneTable.GetRow(0).GetCell(2);
                // 创建一个段落对象以存放加粗字体
                para = cells.Paragraphs[0];
                runs = para.CreateRun();
                // 设置段落文本和字体样式
                runs.SetText("项目名称");
                runs.IsBold = true;
                // 将样式应用到文档中
                cells.SetParagraph(para);
                cells = oneTable.GetRow(0).GetCell(3);
                // 创建一个段落对象以存放加粗字体
                para = cells.Paragraphs[0];
                runs = para.CreateRun();
                // 设置段落文本和字体样式
                runs.SetText("产值完成比例");
                runs.IsBold = true;
                // 将样式应用到文档中
                cells.SetParagraph(para);
                cells = oneTable.GetRow(0).GetCell(4);
                // 创建一个段落对象以存放加粗字体
                para = cells.Paragraphs[0];
                runs = para.CreateRun();
                // 设置段落文本和字体样式
                runs.SetText("产值确认比例");
                runs.IsBold = true;
                // 将样式应用到文档中
                cells.SetParagraph(para);
                cells = oneTable.GetRow(0).GetCell(5);
                // 创建一个段落对象以存放加粗字体
                para = cells.Paragraphs[0];
                runs = para.CreateRun();
                // 设置段落文本和字体样式
                runs.SetText("实际收款比例");
                runs.IsBold = true;
                // 将样式应用到文档中
                cells.SetParagraph(para);
                for (int i = 0; i < collectionRatioCount; i++)
                {
                    oneTable.GetRow(i + 1).GetCell(1).SetText(collectionRatio[i].CompanyName);

                    oneTable.GetRow(i + 1).GetCell(2).SetText(collectionRatio[i].ProjectName);

                    oneTable.GetRow(i + 1).GetCell(3).SetText(Math.Round((collectionRatio[i].EngineeringProportion * 100).Value, 2).ToString() + "%");

                    oneTable.GetRow(i + 1).GetCell(4).SetText(Math.Round((collectionRatio[i].OwnerProportion * 100).Value, 2).ToString() + "%");

                    oneTable.GetRow(i + 1).GetCell(5).SetText(Math.Round((collectionRatio[i].ProjectProportion * 100).Value, 2).ToString() + "%");
                }
                #endregion

                #region 单元格属性
                foreach (XWPFTableRow row in oneTable.Rows)
                {
                    foreach (XWPFTableCell cell in row.GetTableCells())
                    {
                        foreach (XWPFParagraph paragraphs in cell.Paragraphs)
                        {
                            if ((oneTable.Rows.IndexOf(row) == 1 && row.GetTableCells().IndexOf(cell) == 2)
                                || (oneTable.Rows.IndexOf(row) == 2 && row.GetTableCells().IndexOf(cell) == 2)
                                || (oneTable.Rows.IndexOf(row) == 3 && row.GetTableCells().IndexOf(cell) == 2)
                                || (oneTable.Rows.IndexOf(row) == 4 && row.GetTableCells().IndexOf(cell) == 2)
                                || (oneTable.Rows.IndexOf(row) == 5 && row.GetTableCells().IndexOf(cell) == 2)
                                || (oneTable.Rows.IndexOf(row) == 6 && row.GetTableCells().IndexOf(cell) == 2)
                                || (oneTable.Rows.IndexOf(row) == 7 && row.GetTableCells().IndexOf(cell) == 2)
                                || (oneTable.Rows.IndexOf(row) == 8 && row.GetTableCells().IndexOf(cell) == 2)
                                || (oneTable.Rows.IndexOf(row) == 9 && row.GetTableCells().IndexOf(cell) == 2)
                                || (oneTable.Rows.IndexOf(row) == 10 && row.GetTableCells().IndexOf(cell) == 2))

                            {
                                paragraphs.Alignment = ParagraphAlignment.LEFT;
                            }
                            else
                            {
                                paragraphs.Alignment = ParagraphAlignment.CENTER;
                            }

                        }
                    }
                }
                #endregion


                //换页
                var paragraph1 = word.CreateParagraph();
                var run1 = paragraph1.CreateRun();
                run1.AddBreak();
            }
            #endregion

            #region 产值确认比例低十大项目
            if (outputValue.Any())
            {
                var paragraph2 = word.CreateParagraph();
                paragraph2.Alignment = ParagraphAlignment.CENTER;
                var run2 = paragraph2.CreateRun();
                run2.SetText("产值确认比例低十大项目");
                run2.IsBold = true;

                #region 基本配置
                XWPFTable oneTable = word.CreateTable(11, 8);
                CT_TcPr ctPr = new CT_TcPr();
                CT_Tbl ctbl = oneTable.GetCTTbl();
                //清空单元格样式
                ctbl.tblPr = new CT_TblPr();
                ctbl.tblPr.tblLayout = new CT_TblLayoutType();
                ctbl.tblPr.tblLayout.type = ST_TblLayoutType.@fixed;
                for (int i = 0; i < oneTable.Rows.Count; i++)
                {
                    XWPFTableRow row = oneTable.GetRow(i);
                    for (int col = 0; col < row.GetTableCells().Count; col++)
                    {

                        XWPFTableCell cell = oneTable.GetRow(i).GetCell(col);
                        //paragraph.Alignment = ParagraphAlignment.CENTER;
                        // 设置单元格样式
                        CT_TcPr tcPr = cell.GetCTTc().AddNewTcPr();
                        CT_VerticalJc vertAlign = new CT_VerticalJc();
                        vertAlign.val = ST_VerticalJc.center; // 垂直居中
                        tcPr.AddNewVAlign().val = ST_VerticalJc.center; // 垂直居中
                        tcPr.AddNewTcW().w = "500"; // 设置单元格宽度（可调整）
                    }
                }
                //增加单元格新样式
                CT_TblBorders tblBorders = new CT_TblBorders();
                CT_Border border = new CT_Border();
                border.val = ST_Border.single;
                border.color = "000000"; // 边框颜色，这里使用黑色，默认为自动颜色
                tblBorders.top = border;
                tblBorders.bottom = border;
                tblBorders.left = border;
                tblBorders.right = border;
                tblBorders.insideH = border; // 表格内部水平边框
                tblBorders.insideV = border; // 表格内垂直边框
                ctbl.tblPr.tblBorders = tblBorders;

                for (int i = 0; i < oneTable.Rows.Count; i++)
                {
                    XWPFTableRow row = oneTable.GetRow(i);
                    row.Height = 500; // 设置行高为 500，单位为 1/20 磅
                }
                oneTable.Width = 5000;
                #endregion

                #region 合并单元格
                oneTable.GetRow(0).MergeCells(2, 4);
                oneTable.GetRow(1).MergeCells(2, 4);
                oneTable.GetRow(2).MergeCells(2, 4);
                oneTable.GetRow(3).MergeCells(2, 4);
                oneTable.GetRow(4).MergeCells(2, 4);
                oneTable.GetRow(5).MergeCells(2, 4);
                oneTable.GetRow(6).MergeCells(2, 4);
                oneTable.GetRow(7).MergeCells(2, 4);
                oneTable.GetRow(8).MergeCells(2, 4);
                oneTable.GetRow(9).MergeCells(2, 4);
                oneTable.GetRow(10).MergeCells(2, 4);
                #endregion

                #region 填充数据
                var outputValueCount = outputValue.Count >= 10 ? 10 : outputValue.Count;
                XWPFTableCell cells = oneTable.GetRow(0).GetCell(0);
                // 创建一个段落对象以存放加粗字体
                XWPFParagraph para = cells.Paragraphs[0];
                XWPFRun runs = para.CreateRun();
                // 设置段落文本和字体样式
                runs.SetText("序号");
                runs.IsBold = true;
                // 将样式应用到文档中
                cells.SetParagraph(para);
                oneTable.GetRow(1).GetCell(0).SetText("1");
                oneTable.GetRow(2).GetCell(0).SetText("2");
                oneTable.GetRow(3).GetCell(0).SetText("3");
                oneTable.GetRow(4).GetCell(0).SetText("4");
                oneTable.GetRow(5).GetCell(0).SetText("5");
                oneTable.GetRow(6).GetCell(0).SetText("6");
                oneTable.GetRow(7).GetCell(0).SetText("7");
                oneTable.GetRow(8).GetCell(0).SetText("8");
                oneTable.GetRow(9).GetCell(0).SetText("9");
                oneTable.GetRow(10).GetCell(0).SetText("10");
                cells = oneTable.GetRow(0).GetCell(1);
                // 创建一个段落对象以存放加粗字体
                para = cells.Paragraphs[0];
                runs = para.CreateRun();
                // 设置段落文本和字体样式
                runs.SetText("项目归属");
                runs.IsBold = true;
                // 将样式应用到文档中
                cells.SetParagraph(para);
                cells = oneTable.GetRow(0).GetCell(2);
                // 创建一个段落对象以存放加粗字体
                para = cells.Paragraphs[0];
                runs = para.CreateRun();
                // 设置段落文本和字体样式
                runs.SetText("项目名称");
                runs.IsBold = true;
                // 将样式应用到文档中
                cells.SetParagraph(para);
                cells = oneTable.GetRow(0).GetCell(3);
                // 创建一个段落对象以存放加粗字体
                para = cells.Paragraphs[0];
                runs = para.CreateRun();
                // 设置段落文本和字体样式
                runs.SetText("实际产值完成比例");
                runs.IsBold = true;
                // 将样式应用到文档中
                cells.SetParagraph(para);
                cells = oneTable.GetRow(0).GetCell(4);
                // 创建一个段落对象以存放加粗字体
                para = cells.Paragraphs[0];
                runs = para.CreateRun();
                // 设置段落文本和字体样式
                runs.SetText("产值确认比例");
                runs.IsBold = true;
                // 将样式应用到文档中
                cells.SetParagraph(para);
                cells = oneTable.GetRow(0).GetCell(5);
                // 创建一个段落对象以存放加粗字体
                para = cells.Paragraphs[0];
                runs = para.CreateRun();
                // 设置段落文本和字体样式
                runs.SetText("备注");
                runs.IsBold = true;
                // 将样式应用到文档中
                cells.SetParagraph(para);
                for (int i = 0; i < outputValueCount; i++)
                {


                    oneTable.GetRow(i + 1).GetCell(1).SetText(outputValue[i].CompanyName);

                    oneTable.GetRow(i + 1).GetCell(2).SetText(outputValue[i].ProjectName);

                    oneTable.GetRow(i + 1).GetCell(3).SetText(Math.Round((outputValue[i].EngineeringProportion * 100).Value, 2).ToString() + "%");

                    oneTable.GetRow(i + 1).GetCell(4).SetText(Math.Round((outputValue[i].OwnerProportion * 100).Value, 2).ToString() + "%");

                    oneTable.GetRow(i + 1).GetCell(5).SetText(outputValue[0].EngineeringRemarks);

                }
                #endregion

                #region 单元格属性
                foreach (XWPFTableRow row in oneTable.Rows)
                {
                    foreach (XWPFTableCell cell in row.GetTableCells())
                    {
                        foreach (XWPFParagraph paragraphs in cell.Paragraphs)
                        {
                            if ((oneTable.Rows.IndexOf(row) == 1 && row.GetTableCells().IndexOf(cell) == 2)
                                || (oneTable.Rows.IndexOf(row) == 2 && row.GetTableCells().IndexOf(cell) == 2)
                                || (oneTable.Rows.IndexOf(row) == 3 && row.GetTableCells().IndexOf(cell) == 2)
                                || (oneTable.Rows.IndexOf(row) == 4 && row.GetTableCells().IndexOf(cell) == 2)
                                || (oneTable.Rows.IndexOf(row) == 5 && row.GetTableCells().IndexOf(cell) == 2)
                                || (oneTable.Rows.IndexOf(row) == 6 && row.GetTableCells().IndexOf(cell) == 2)
                                || (oneTable.Rows.IndexOf(row) == 7 && row.GetTableCells().IndexOf(cell) == 2)
                                || (oneTable.Rows.IndexOf(row) == 8 && row.GetTableCells().IndexOf(cell) == 2)
                                || (oneTable.Rows.IndexOf(row) == 9 && row.GetTableCells().IndexOf(cell) == 2)
                                || (oneTable.Rows.IndexOf(row) == 10 && row.GetTableCells().IndexOf(cell) == 2))

                            {
                                paragraphs.Alignment = ParagraphAlignment.LEFT;
                            }
                            else
                            {
                                paragraphs.Alignment = ParagraphAlignment.CENTER;
                            }

                        }
                    }
                }
                #endregion

                //换页
                var paragraph1 = word.CreateParagraph();
                var run1 = paragraph1.CreateRun();
                run1.AddBreak();
            }
            #endregion

            foreach (var item in data.Data)
            {
                Number++;
                var paragraph2 = word.CreateParagraph();
                var run2 = paragraph2.CreateRun();
                run2.SetText(Number + "." + item.ProjectName);
                XWPFTable oneTable = word.CreateTable(16, 7);

                #region 设置单元格样式
                CT_TcPr ctPr = new CT_TcPr();
                CT_Tbl ctbl = oneTable.GetCTTbl();
                //清空单元格样式
                ctbl.tblPr = new CT_TblPr();
                ctbl.tblPr.tblLayout = new CT_TblLayoutType();
                ctbl.tblPr.tblLayout.type = ST_TblLayoutType.@fixed;
                for (int i = 0; i < oneTable.Rows.Count; i++)
                {
                    XWPFTableRow row = oneTable.GetRow(i);
                    for (int col = 0; col < row.GetTableCells().Count; col++)
                    {

                        XWPFTableCell cell = oneTable.GetRow(i).GetCell(col);
                        //paragraph.Alignment = ParagraphAlignment.CENTER;
                        // 设置单元格样式
                        CT_TcPr tcPr = cell.GetCTTc().AddNewTcPr();
                        CT_VerticalJc vertAlign = new CT_VerticalJc();
                        vertAlign.val = ST_VerticalJc.center; // 垂直居中
                        tcPr.AddNewVAlign().val = ST_VerticalJc.center; // 垂直居中
                        tcPr.AddNewTcW().w = "500"; // 设置单元格宽度（可调整）
                    }
                }
                //增加单元格新样式
                CT_TblBorders tblBorders = new CT_TblBorders();
                CT_Border border = new CT_Border();
                border.val = ST_Border.single;
                border.color = "000000"; // 边框颜色，这里使用黑色，默认为自动颜色
                tblBorders.top = border;
                tblBorders.bottom = border;
                tblBorders.left = border;
                tblBorders.right = border;
                tblBorders.insideH = border; // 表格内部水平边框
                tblBorders.insideV = border; // 表格内垂直边框
                ctbl.tblPr.tblBorders = tblBorders;

                for (int i = 0; i < oneTable.Rows.Count; i++)
                {
                    XWPFTableRow row = oneTable.GetRow(i);
                    row.Height = 500; // 设置行高为 500，单位为 1/20 磅
                }
                oneTable.Width = 5000;
                #endregion

                #region 调整单元格列宽
                for (int i = 0; i < 7; i++)
                {
                    ctPr = oneTable.GetRow(8).GetCell(i).GetCTTc().AddNewTcPr();
                    ctPr.tcW = new CT_TblWidth();
                    ctPr.tcW.w = "3000";//单元格宽
                    ctPr.tcW.type = ST_TblWidth.dxa;
                }
                #endregion

                XWPFTableCell cells = oneTable.GetRow(8).GetCell(0);
                CT_TcPr tcPrs = cells.GetCTTc().AddNewTcPr();
                CT_VerticalJc vertAligns = new CT_VerticalJc();
                vertAligns.val = ST_VerticalJc.center; // 垂直居中
                tcPrs.AddNewVAlign().val = ST_VerticalJc.center; // 垂直居中
                cells = oneTable.GetRow(8).GetCell(1);
                tcPrs = cells.GetCTTc().AddNewTcPr();
                vertAligns = new CT_VerticalJc();
                vertAligns.val = ST_VerticalJc.center; // 垂直居中
                tcPrs.AddNewVAlign().val = ST_VerticalJc.center; // 垂直居中

                #region 合并单元格（同列不同行）
                MYMergeCells(oneTable, 0, 0, 9, 12);
                #endregion

                #region 合并单元格（同行不同列）
                oneTable.GetRow(0).MergeCells(1, 6);

                oneTable.GetRow(1).MergeCells(1, 3);
                oneTable.GetRow(1).MergeCells(3, 4);

                oneTable.GetRow(2).MergeCells(1, 3);
                oneTable.GetRow(2).MergeCells(3, 4);

                oneTable.GetRow(3).MergeCells(1, 6);

                oneTable.GetRow(4).MergeCells(1, 6);

                oneTable.GetRow(5).MergeCells(1, 2);
                oneTable.GetRow(5).MergeCells(2, 3);
                oneTable.GetRow(5).MergeCells(3, 4);

                oneTable.GetRow(6).MergeCells(1, 6);

                oneTable.GetRow(7).MergeCells(1, 6);

                oneTable.GetRow(8).MergeCells(1, 6);

                oneTable.GetRow(13).MergeCells(1, 6);
                oneTable.GetRow(14).MergeCells(1, 6);
                oneTable.GetRow(15).MergeCells(1, 6);
                #endregion

                #region 填充单元格
                oneTable.GetRow(0).GetCell(0).SetText("项目名称");
                oneTable.GetRow(0).GetCell(0).SetColor("#EEEEEE");
                //XWPFTableCell cellr = oneTable.GetRow(0).GetCell(1);
                //XWPFParagraph paragraph3 = cellr.AddParagraph();
                //XWPFRun run3 = paragraph3.CreateRun();
                //run3.SetText(item.ProjectName);
                //paragraph3.Alignment = ParagraphAlignment.LEFT;
                oneTable.GetRow(0).GetCell(1).SetText(item.ProjectName);

                oneTable.GetRow(1).GetCell(0).SetText("所属公司");
                oneTable.GetRow(1).GetCell(0).SetColor("#EEEEEE");
                oneTable.GetRow(1).GetCell(1).SetText(item.CompanyName);
                oneTable.GetRow(1).GetCell(2).SetText("项目规模");
                oneTable.GetRow(1).GetCell(2).SetColor("#EEEEEE");
                oneTable.GetRow(1).GetCell(3).SetText(item.ProjectGrade);

                oneTable.GetRow(2).GetCell(0).SetText("项目类型");
                oneTable.GetRow(2).GetCell(0).SetColor("#EEEEEE");
                oneTable.GetRow(2).GetCell(1).SetText(item.ProjectType);
                oneTable.GetRow(2).GetCell(2).SetText("项目状态");
                oneTable.GetRow(2).GetCell(2).SetColor("#EEEEEE");
                oneTable.GetRow(2).GetCell(3).SetText(item.ProjectState);

                oneTable.GetRow(3).GetCell(0).SetText("项目位置");
                oneTable.GetRow(3).GetCell(0).SetColor("#EEEEEE");
                oneTable.GetRow(3).GetCell(1).SetText(item.ProjectLocation);

                oneTable.GetRow(4).GetCell(0).SetText("项目内容");
                oneTable.GetRow(4).GetCell(0).SetColor("#EEEEEE");
                oneTable.GetRow(4).GetCell(1).SetText(item.ProjectContent);

                oneTable.GetRow(5).GetCell(0).SetText("合同额(万元)");
                oneTable.GetRow(5).GetCell(0).SetColor("#EEEEEE");
                oneTable.GetRow(5).GetCell(1).SetText("原合同额:" + item.OriginalContractAmount.ToString());
                oneTable.GetRow(5).GetCell(2).SetText("变更额:" + item.ChangeAmount.ToString());
                oneTable.GetRow(5).GetCell(3).SetText("实际合同额:" + item.ActualContractAmount.ToString());

                oneTable.GetRow(6).GetCell(0).SetText("合同变更信息");
                oneTable.GetRow(6).GetCell(0).SetColor("#EEEEEE");
                oneTable.GetRow(6).GetCell(1).SetText(item.ContractChangeInformation);

                oneTable.GetRow(7).GetCell(0).SetText("工期信息");
                oneTable.GetRow(7).GetCell(0).SetColor("#EEEEEE");
                oneTable.GetRow(7).GetCell(1).SetText(item.DurationInformation);

                oneTable.GetRow(8).GetCell(0).SetText("项目进展");
                oneTable.GetRow(8).GetCell(0).SetColor("#EEEEEE");
                oneTable.GetRow(8).GetCell(1).SetText(item.MonthProjectDebriefing);


                oneTable.GetRow(9).GetCell(0).SetText("完成产值、确认产值、工程收款（万元）");
                oneTable.GetRow(9).GetCell(0).SetColor("#EEEEEE");
                //oneTable.GetRow(9).GetCell(1).SetText(item.MonthProjectDebriefing);
                oneTable.GetRow(9).GetCell(1).SetColor("#F5F9FA");
                //oneTable.GetRow(10).GetCell(0).SetText("工程进度(万元)");
                //oneTable.GetRow(10).GetCell(0).SetColor("#D4F0FC");
                //oneTable.GetRow(10).GetCell(0).SetText("工程进度(万元)");
                //oneTable.GetRow(11).GetCell(0).SetText("工程进度(万元)");
                //oneTable.GetRow(12).GetCell(0).SetText("工程进度(万元)");
                //oneTable.GetRow(10).GetCell(0).SetColor("#D4F0FC");
                //oneTable.GetRow(11).GetCell(0).SetColor("#D4F0FC");
                //oneTable.GetRow(12).GetCell(0).SetColor("#D4F0FC");

                oneTable.GetRow(9).GetCell(1).SetText("指标项目");
                oneTable.GetRow(9).GetCell(1).SetColor("#D4F0FC");
                oneTable.GetRow(9).GetCell(2).SetText("本月");
                oneTable.GetRow(9).GetCell(2).SetColor("#D4F0FC");
                oneTable.GetRow(9).GetCell(3).SetText("本年");
                oneTable.GetRow(9).GetCell(3).SetColor("#D4F0FC");
                oneTable.GetRow(9).GetCell(4).SetText("工程累计");
                oneTable.GetRow(9).GetCell(4).SetColor("#D4F0FC");
                oneTable.GetRow(9).GetCell(5).SetText("比例");
                oneTable.GetRow(9).GetCell(5).SetColor("#D4F0FC");
                oneTable.GetRow(9).GetCell(6).SetText("备注");
                oneTable.GetRow(9).GetCell(6).SetColor("#D4F0FC");
                oneTable.GetRow(10).GetCell(1).SetText("工程产值");
                oneTable.GetRow(10).GetCell(1).SetColor("#F5F9FA");
                oneTable.GetRow(10).GetCell(2).SetText(item.EngineeringMonthOutputValue.ToString());
                oneTable.GetRow(10).GetCell(2).SetColor("#F5F9FA");
                oneTable.GetRow(10).GetCell(3).SetText(item.EngineeringYeahOutputValue.ToString());
                oneTable.GetRow(10).GetCell(3).SetColor("#F5F9FA");
                oneTable.GetRow(10).GetCell(4).SetText(item.EngineeringAccumulatedEngineering.ToString());
                oneTable.GetRow(10).GetCell(4).SetColor("#F5F9FA");
                oneTable.GetRow(10).GetCell(5).SetText(Math.Round((item.EngineeringProportion * 100).Value, 2).ToString() + "%");
                oneTable.GetRow(10).GetCell(5).SetColor("#F5F9FA");
                oneTable.GetRow(10).GetCell(6).SetText(item.EngineeringRemarks);
                oneTable.GetRow(10).GetCell(6).SetColor("#F5F9FA");

                oneTable.GetRow(11).GetCell(1).SetText("业主确认");
                oneTable.GetRow(11).GetCell(1).SetColor("#F5F9FA");
                oneTable.GetRow(11).GetCell(2).SetText(item.OwnerMonthOutputValue.ToString());
                oneTable.GetRow(11).GetCell(2).SetColor("#F5F9FA");
                oneTable.GetRow(11).GetCell(3).SetText(item.OwnerYeahOutputValue.ToString());
                oneTable.GetRow(11).GetCell(3).SetColor("#F5F9FA");
                oneTable.GetRow(11).GetCell(4).SetText(item.OwnerAccumulatedEngineering.ToString());
                oneTable.GetRow(11).GetCell(4).SetColor("#F5F9FA");
                oneTable.GetRow(11).GetCell(5).SetText(Math.Round((item.OwnerProportion * 100).Value, 2).ToString() + "%");
                oneTable.GetRow(11).GetCell(5).SetColor("#F5F9FA");
                oneTable.GetRow(11).GetCell(6).SetText(item.OwnerRemarks);
                oneTable.GetRow(11).GetCell(6).SetColor("#F5F9FA");

                oneTable.GetRow(12).GetCell(1).SetText("工程收款");
                oneTable.GetRow(12).GetCell(1).SetColor("#F5F9FA");
                oneTable.GetRow(12).GetCell(2).SetText(item.ProjectMonthOutputValue.ToString());
                oneTable.GetRow(12).GetCell(2).SetColor("#F5F9FA");
                oneTable.GetRow(12).GetCell(3).SetText(item.ProjectYeahOutputValue.ToString());
                oneTable.GetRow(12).GetCell(3).SetColor("#F5F9FA");
                oneTable.GetRow(12).GetCell(4).SetText(item.ProjectAccumulatedEngineering.ToString());
                oneTable.GetRow(12).GetCell(4).SetColor("#F5F9FA");
                oneTable.GetRow(12).GetCell(5).SetText(Math.Round((item.ProjectProportion * 100).Value, 2).ToString() + "%");
                oneTable.GetRow(12).GetCell(5).SetColor("#F5F9FA");
                oneTable.GetRow(12).GetCell(6).SetText(item.ProjectRemarks);
                oneTable.GetRow(12).GetCell(6).SetColor("#F5F9FA");

                oneTable.GetRow(13).GetCell(0).SetText("存在问题");
                oneTable.GetRow(13).GetCell(0).SetColor("#EEEEEE");
                oneTable.GetRow(13).GetCell(1).SetText(item.ExistingProblems);

                oneTable.GetRow(14).GetCell(0).SetText("解决措施");
                oneTable.GetRow(14).GetCell(0).SetColor("#EEEEEE");
                oneTable.GetRow(14).GetCell(1).SetText(item.TakeSteps);

                oneTable.GetRow(15).GetCell(0).SetText("需公司协调事项");
                oneTable.GetRow(15).GetCell(0).SetColor("#EEEEEE");
                oneTable.GetRow(15).GetCell(1).SetText(item.CoordinationMatters);
                #endregion

                #region 设置单元格对齐方式
                foreach (XWPFTableRow row in oneTable.Rows)
                {
                    foreach (XWPFTableCell cell in row.GetTableCells())
                    {
                        foreach (XWPFParagraph paragraphs in cell.Paragraphs)
                        {
                            if ((oneTable.Rows.IndexOf(row) == 0 && row.GetTableCells().IndexOf(cell) == 1)
                                || (oneTable.Rows.IndexOf(row) == 1 && row.GetTableCells().IndexOf(cell) == 1)
                                || (oneTable.Rows.IndexOf(row) == 1 && row.GetTableCells().IndexOf(cell) == 3)
                                || (oneTable.Rows.IndexOf(row) == 2 && row.GetTableCells().IndexOf(cell) == 1)
                                || (oneTable.Rows.IndexOf(row) == 2 && row.GetTableCells().IndexOf(cell) == 3)
                                || (oneTable.Rows.IndexOf(row) == 3 && row.GetTableCells().IndexOf(cell) == 1)
                                || (oneTable.Rows.IndexOf(row) == 4 && row.GetTableCells().IndexOf(cell) == 1)
                                || (oneTable.Rows.IndexOf(row) == 5 && row.GetTableCells().IndexOf(cell) == 1)
                                || (oneTable.Rows.IndexOf(row) == 5 && row.GetTableCells().IndexOf(cell) == 2)
                                || (oneTable.Rows.IndexOf(row) == 5 && row.GetTableCells().IndexOf(cell) == 3)
                                || (oneTable.Rows.IndexOf(row) == 6 && row.GetTableCells().IndexOf(cell) == 1)
                                || (oneTable.Rows.IndexOf(row) == 7 && row.GetTableCells().IndexOf(cell) == 1)
                                || (oneTable.Rows.IndexOf(row) == 8 && row.GetTableCells().IndexOf(cell) == 1)
                                || (oneTable.Rows.IndexOf(row) == 13 && row.GetTableCells().IndexOf(cell) == 1)
                                || (oneTable.Rows.IndexOf(row) == 14 && row.GetTableCells().IndexOf(cell) == 1)
                                || (oneTable.Rows.IndexOf(row) == 15 && row.GetTableCells().IndexOf(cell) == 1))
                            {
                                paragraphs.Alignment = ParagraphAlignment.LEFT;
                            }
                            else
                            {
                                paragraphs.Alignment = ParagraphAlignment.CENTER;
                            }

                        }
                    }
                }

                #endregion

                //换页
                var paragraph1 = word.CreateParagraph();
                var run1 = paragraph1.CreateRun();
                run1.AddBreak();
            }
            #endregion
            try
            {

                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                {
                    word.Write(fs);
                }
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    //读取文件流
                    byte[] bytes = new byte[fs.Length];
                    fs.Read(bytes, 0, bytes.Length);
                    memoryStream.Write(bytes, 0, bytes.Length);
                    #region 删除文件

                    #endregion
                }
                //System.IO.File.Delete(path);
            }
            catch (Exception ex)
            {
                try
                {
                    System.IO.File.Delete(path);
                }
                catch (Exception exs)
                {
                    logger.LogError($"删除文件失败:{exs}");
                }
                logger.LogError($"项目月报简报导出Word失败错误信息:{ex}");
            }
            return memoryStream;
            //});
        }
        #endregion
        /// <summary>
        /// 合并单元格
        /// </summary>
        /// <param name="table"></param>
        /// <param name="fromCol">起始列</param>
        /// <param name="toCol">结束列</param>
        /// <param name="fromRow">起始行</param>
        /// <param name="toRow">结束行</param>
        /// <returns></returns>
        public static XWPFTableCell MYMergeCells(XWPFTable table, int fromCol, int toCol, int fromRow, int toRow)
        {
            for (int rowIndex = fromRow; rowIndex <= toRow; rowIndex++)
            {
                if (fromCol < toCol)
                {
                    table.GetRow(rowIndex).MergeCells(fromCol, toCol);
                }
                XWPFTableCell rowcell = table.GetRow(rowIndex).GetCell(fromCol);
                CT_Tc cttc = rowcell.GetCTTc();
                if (cttc.tcPr == null)
                {
                    cttc.AddNewTcPr();
                }
                if (rowIndex == fromRow)
                {
                    // The first merged cell is set with RESTART merge value  
                    rowcell.GetCTTc().tcPr.AddNewVMerge().val = ST_Merge.restart;
                }
                else
                {
                    // Cells which join (merge) the first one, are set with CONTINUE  
                    rowcell.GetCTTc().tcPr.AddNewVMerge().val = ST_Merge.@continue;
                }
            }
            table.GetRow(fromRow).GetCell(fromCol).SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);
            table.GetRow(fromRow).GetCell(fromCol).Paragraphs[0].Alignment = ParagraphAlignment.CENTER;
            return table.GetRow(fromRow).GetCell(fromCol);
        }
    }
}
