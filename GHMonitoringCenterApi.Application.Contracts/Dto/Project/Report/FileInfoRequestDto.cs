using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report
{

    /// <summary>
    /// 文件信息请求Dto
    /// </summary>
    public class FileInfoRequestDto : IValidatableObject
    {
        /// <summary>
        /// 文件Id
        /// </summary>
        public Guid? FileId { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 原始文件名称
        /// </summary>
        public string? OriginName { get; set; }

        /// <summary>
        /// 后缀名称
        /// </summary>
        public string? SuffixName { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FileId == null || FileId == Guid.Empty)
            {
                yield return new ValidationResult("文件Id不能为空", new string[] { nameof(FileId) });
            }
            if (string.IsNullOrWhiteSpace(Name))
            {
                yield return new ValidationResult("文件名称不能为空", new string[] { nameof(FileId) });
            }
        }

    }
}
