using HNKC.CrewManagePlatform.Models.Enums;
using SqlSugar;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 用户表
    /// </summary>
    [SugarTable("t_user", IsDisabledDelete = true, TableDescription = "用户表")]
    public class User : BaseEntity<long>
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [SugarColumn(Length = 128, ColumnDescription = "姓名")]
        public string? Name { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        [SugarColumn(Length = 36, ColumnDescription = "工号")]
        public string? WorkNumber { get; set; }

        /// <summary>
        /// 所属船舶（部门）OID
        /// </summary>
        [SugarColumn(Length = 36, ColumnDescription = "所属船舶（部门）OID")]
        public string? Oid { get; set; }

        /// <summary>
        /// 职务ID
        /// </summary>
        [SugarColumn(Length = 36, ColumnDescription = "职务ID")]
        public string? JobId { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [SugarColumn(Length = 36, ColumnDescription = "手机号")]
        public string? Phone { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [SugarColumn(Length = 36, ColumnDescription = "密码")]
        public string? Password { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        [SugarColumn(Length = 36, ColumnDescription = "身份证号")]
        public string? CardId { get; set; }

        /// <summary>
        /// 政治面貌
        /// </summary>
        [SugarColumn(Length = 36, ColumnDescription = "政治面貌")]
        public string? PoliticalStatus { get; set; }
        /// <summary>
        /// 船员照片 ,拼接文件
        /// </summary>
        [SugarColumn(Length = 100, ColumnDescription = "船员照片")]
        public string? CrewPhoto { get; set; }
        /// <summary>
        /// 身份证扫描件
        /// </summary>
        [SugarColumn(ColumnDataType = "text", ColumnDescription = "身份证扫描件")]
        public string? IdCardScans { get; set; }
        /// <summary>
        /// 籍贯(省)
        /// </summary>
        [SugarColumn(Length = 100, ColumnDescription = "籍贯")]
        public string? NativePlace { get; set; }
        /// <summary>
        /// 家庭地址
        /// </summary>
        [SugarColumn(Length = 256, ColumnDescription = "家庭地址")]
        public string? HomeAddress { get; set; }
        /// <summary>
        /// 常住地
        /// </summary>
        [SugarColumn(Length = 512, ColumnDescription = "常住地")]
        public string? BuildAddress { get; set; }
        /// <summary>
        /// 民族
        /// </summary>
        [SugarColumn(Length = 50, ColumnDescription = "民族")]
        public string? Nation { get; set; }
        /// <summary>
        /// 合同类型
        /// </summary>
        [SugarColumn(ColumnDataType = "int", ColumnDescription = "合同类型", DefaultValue = "0")]
        public ContractEnum ContarctType { get; set; }
        /// <summary>
        /// 船舶类型
        /// </summary>
        [SugarColumn(ColumnDataType = "int", ColumnDescription = "船舶类型", DefaultValue = "0")]
        public ShipTypeEnum ShipType { get; set; }
        /// <summary>
        /// 船员类型
        /// </summary>
        [SugarColumn(Length = 50, ColumnDescription = "船员类型")]
        public string? CrewType { get; set; }
        /// <summary>
        /// 服务簿类型
        /// </summary>
        [SugarColumn(ColumnDataType = "int", ColumnDescription = "服务簿类型", DefaultValue = "0")]
        public ServiceBookEnum ServiceBookType { get; set; }
        /// <summary>
        /// 所在船舶
        /// </summary>
        [SugarColumn(Length = 50, ColumnDescription = "所在船舶")]
        public string? OnBoard { get; set; }
        /// <summary>
        /// 在船职务
        /// </summary>
        [SugarColumn(Length = 50, ColumnDescription = "在船职务")]
        public string? PositionOnBoard { get; set; }
        /// <summary>
        /// 删除原因
        /// </summary>
        [SugarColumn(ColumnDataType = "int", ColumnDescription = "删除原因", DefaultValue = "0")]
        public CrewStatusEnum DeleteReson { get; set; }

        /// <summary>
        /// 是否是新增用户   从其他用户同步过来的用户不属于新增用户  默认值是1  不是新增   0是新增
        /// </summary>
        [SugarColumn(ColumnDataType = "int", ColumnDescription = "是否是新增用户   从其他用户同步过来的用户不属于新增用户  默认值是1  不是新增   0是新增", DefaultValue = "1")]
        public int IsInsert { get; set; }
    }
}
