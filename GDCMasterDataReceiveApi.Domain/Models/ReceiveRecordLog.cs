using GDCMasterDataReceiveApi.Domain.Shared.Enums;
using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 接收数据记录表
    /// </summary>
    [SugarTable("t_receiverecordlog", IsDisabledDelete = true)]
    public class ReceiveRecordLog:BaseEntity<long>
    {
        /// <summary>
        /// 1 人员
        /// 2  机构
        /// 3  项目
        /// 4 币种
        /// </summary>
        [SugarColumn(ColumnDataType = "int",Length =32)]
        public ReceiveDataType ReceiveType { get; set; }
        /// <summary>
        /// 请求参数
        /// </summary>
        [SugarColumn(ColumnDataType = "longvarchar")]
        public string? RequestParame { get; set; }
        /// <summary>
        /// 接收数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public  int? ReceiveNumber { get; set; }
        /// <summary>
        /// 失败数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public  int? FailNumber { get; set; }
        /// <summary>
        /// 失败的数据
        /// </summary>
        [SugarColumn(ColumnDataType = "longvarchar")]
        public string?  FailData { get; set; }
        /// <summary>
        /// 失败消息
        /// </summary>
        [SugarColumn(Length = 2048)]
        public  string? FailMessage { get; set; }
    }
}
