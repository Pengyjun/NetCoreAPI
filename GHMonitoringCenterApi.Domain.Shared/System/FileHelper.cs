using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Spire.Doc;
using Spire.Doc.Documents;

namespace System.IO
{
   /// <summary>
   ///文件帮助类
   /// </summary>
    public static   class FileHelper
    {
        /// <summary>
        /// 获取文件格式
        /// </summary>
        /// <param name="fileFormatType">文件格式类型</param>
        /// <returns></returns>
        public static  string GetFileFormat(this FileFormatType fileFormatType)
        {
            switch (fileFormatType)
            {
                case FileFormatType.Docx: return "docx";
                case FileFormatType.PDF: return "pdf";
            }
            return string.Empty;
        }


        /// <summary>
        /// html转化成其他格式文件
        /// </summary>
        /// <param name="html">html文本</param>
        /// <param name="fileFormatType">文件格式类型</param>
        /// <exception cref="ArgumentException"></exception>
        public static  MemoryStream  HtmlConvertStream([NotNull]string html, FileFormatType fileFormatType)
        {
            if (string.IsNullOrWhiteSpace(html))
            {
                throw new ArgumentException("html文本不能为空", nameof(html));
            }
             FileFormat? spireFileFormat=null;
            switch (fileFormatType)
            {
                case FileFormatType.Docx:
                    spireFileFormat = FileFormat.Docx;break;
                case FileFormatType.PDF:
                    spireFileFormat = FileFormat.PDF; break;
            }
            if(spireFileFormat==null )
            {
                throw new ArgumentException("不支持该文件格式类型转换", nameof(fileFormatType));
            }
            Document document = new Document();
            var stream = new MemoryStream();
            using TextReader reader = new StringReader(html);
            //加载HTML文本
            document.LoadHTML(reader, XHTMLValidationType.None);
            //将HTML文件转为指定的文件格式存储到流
            document.SaveToStream(stream, (FileFormat)spireFileFormat);
            return stream;
        }
    }
}
