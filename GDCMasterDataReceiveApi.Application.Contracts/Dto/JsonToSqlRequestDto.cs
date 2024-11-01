using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto
{

    /// <summary>
    /// json转sql请求类
    /// </summary>
    public class JsonToSqlRequestDto:IValidatableObject
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        ///  条件类型
        /// </summary>
        public ConditionalType ConditionalType { get; set; }
        /// <summary>
        /// 字段值
        /// </summary>
        public string FieldValue  { get; set; }

        /// <summary>
        /// 0是 and 1是or
        /// </summary>
        public int Type { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(FieldName))
            {
                yield return new ValidationResult("字段名称不能为空", new string[] { nameof(FieldName) });
            }
            //if (string.IsNullOrWhiteSpace(ConditionalType))
            //{
            //    yield return new ValidationResult("条件类型不能为空", new string[] { nameof(ConditionalType) });
            //}
            if (string.IsNullOrWhiteSpace(FieldValue))
            {
                yield return new ValidationResult("字段值不能为空", new string[] { nameof(FieldValue) });
            }
        }
    }
}
