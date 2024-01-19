using GHMonitoringCenterApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.File
{
    /// <summary>
    /// Html转换成文件的请求
    /// </summary>
    public class HtmlConvertFileRequestDto : IValidatableObject
    {
        /// <summary>
        /// Html 文本
        /// </summary>
        public string Html { get; set; } = string.Empty;

        /// <summary>
        /// 转换格式( 1 : docx , 2 : pdf ）
        /// </summary>
        public FileFormatType Format { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Html))
            {
                yield return new ValidationResult("Html文本不能为空", new string[] { nameof(Html) });
            }
            if (Format != FileFormatType.Docx&& Format != FileFormatType.PDF)
            {
                yield return new ValidationResult("转换格式仅支持（docx,pdf）", new string[] { nameof(Format) });
            }
        }
    }
}
