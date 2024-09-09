namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.Institution
{
    /// <summary>
    /// 机构主数据  反显
    /// </summary>
    public class InstitutionDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 机构 ID
        /// </summary>
        public string? Oid { get; set; }
        /// <summary>
        /// 企业分类
        /// 0 外经企业 1 基建企业 2 疏浚企业 3 设计企业 4 装备制造企业 5 投资企业 6 房地产企业 7 贸易企业 8 金融企业 9 其他
        /// </summary>
        public string? EntClass { get; set; }
        /// <summary>
        /// 上级机构 id
        /// </summary>
        public string? POid { get; set; }
        /// <summary>
        /// "机构状态，类型编码"
        /// 1运营 2筹备 3停用 4撤销
        /// </summary>
        public string? Status { get; set; }
        /// <summary>
        /// 机构编码"
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// 机构全称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 机构规则码
        /// </summary>
        public string? Orule { get; set; }
        /// <summary>
        /// 机构简称
        /// </summary>
        public string? ShortName { get; set; }
        /// <summary>
        /// 英文全称
        /// </summary>
        public string? EnglishName { get; set; }
        /// <summary>
        /// 英文简称
        /// </summary>
        public string? EnglishShortName { get; set; }
        /// <summary>
        /// 机构子集
        /// </summary>
        public List<InstitutionDto>? Children { get; set; }
    }
}
