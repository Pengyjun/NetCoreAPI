namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.OtherModels
{
    public class TypeOfBidDisclosureProjectTableModels
    {
        public List<ZMDGTT_ZAWARDP_OUT>? Item { get; set; }
    }

    public class ZMDGTT_ZAWARDP_OUT
    {
        /// <summary>
        /// 
        /// </summary>
        public long? Id { get; set; }
        /// <summary>
        /// 中标交底项目编码:格式：BO000001-01
        /// </summary>
        public string? ZAWARDP { get; set; }
        /// <summary>
        /// 交底项目二级单位
        /// </summary>
        public string? ZAP2NDORG { get; set; }
        /// <summary>
        /// 中标资质单位
        /// </summary>
        public string? ZAWARDORG { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ZAWARDPN { get; set; }
        /// <summary>
        /// 项目外文名称:境外项目必填
        /// </summary>
        public string? ZAWPN_EN { get; set; }
        /// <summary>
        /// 中交项目业务分类
        /// </summary>
        public string? ZSCPBC { get; set; }
        /// <summary>
        /// 项目所在地:境内项目必填
        /// </summary>
        public string? ZSPROJLOC { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        public string? ZSPROTYPE { get; set; }
        /// <summary>
        /// 是否联合体总项目:0-否，1-是
        /// </summary>
        public string? ZSZCONTPRO { get; set; }
        /// <summary>
        /// 数据状态:0-停用，1-启用
        /// </summary>
        public string? ZSSTATE { get; set; }
    }

}
