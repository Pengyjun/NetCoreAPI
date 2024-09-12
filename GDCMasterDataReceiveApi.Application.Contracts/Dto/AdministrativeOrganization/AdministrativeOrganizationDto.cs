namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.AdministrativeOrganization
{
    /// <summary>
    /// 接收多组织-行政组织
    /// </summary>

    public class AdministrativeOrganizationReceiveRequestDto 
    {
        /// <summary>
        /// 
        /// </summary>
        public long? Id { get; set; }
        /// <summary>
        /// 机构主数据编码
        /// </summary>
        public string? MDM_CODE { get; set; }


        /// <summary>
        /// 机构名称（中文）
        /// </summary>
        public string? ZZTNAME_ZH { get; set; }


        /// <summary>
        /// 机构简称（中文)
        /// </summary>
        public string? ZZTSHNAME_CHS { get; set; }


        /// <summary>
        /// 机构名称（英文）
        /// </summary>
        public string? ZZTNAME_EN { get; set; }


        /// <summary>
        /// 机构简称（英文）
        /// </summary>
        public string? ZZTSHNAME_EN { get; set; }

        /// <summary>
        /// 机构名称（当地语言）
        /// </summary>
        public string? ZZTNAME_LOC { get; set; }

        /// <summary>
        /// 机构简称（当地语言）  
        /// </summary>
        public string? ZZTSHNAME_LOC { get; set; }


        /// <summary>
        /// 机构状态  值域：1-运营、2-筹备、3-停用、4-撤销
        /// </summary>
        public string? ZOSTATE { get; set; }

        /// <summary>
        /// 上级机构主数据编码
        /// </summary>
        public string? ZORGUP { get; set; }

        /// <summary>
        /// 机构主数据编码（旧）
        /// </summary>
        public string? OID { get; set; }
        /// <summary>
        /// 上级机构主数据编码（旧）
        /// </summary>
        public string? POID { get; set; }

        /// <summary>
        /// 机构规则码
        /// </summary>
        public string? ZORULE { get; set; }

        /// <summary>
        /// 所属二级单位编码
        /// </summary>
        public string? ZGPOID { get; set; }

        /// <summary>
        /// 层级）
        /// </summary>
        public string? ZO_LEVEL { get; set; }

        /// <summary>
        /// 节点排序号
        /// </summary>
        public string? ZSNO { get; set; }

        /// <summary>
        /// 机构编码 机构27位有含义编码
        /// </summary>
        public string? ZORGNO { get; set; }

        /// <summary>
        ///机构属性
        /// </summary>
        public string? ZOATTR { get; set; }
        /// <summary>
        /// 机构子属性
        /// </summary>
        public string? ZOCATTR { get; set; }
        /// <summary>
        /// 国家名称
        public string? ZCYNAME { get; set; }
        /// <summary>
        /// 机构所在地
        /// </summary>
        public string? ZORGLOC { get; set; }
        /// <summary>
        /// 地域属性
        /// </summary>
        public string? ZREGIONAL { get; set; }
        /// <summary>
        /// 注册号/统一社会信用代码
        /// </summary>
        public string? ZCUSCC { get; set; }
        /// <summary>
        /// 通讯地址
        /// </summary>
        public string? ZADDRESS { get; set; }
        /// <summary>
        /// 持股情况
        /// </summary>
        public string? ZHOLDING { get; set; }
        /// <summary>
        /// 是否独立核算  是否单独建立核算账套对本机构发生的业务进行会计核算，填写“是”或“否
        /// </summary>
        public string? ZCHECKIND { get; set; }
        /// <summary>
        /// 组织树编码
        /// </summary>
        public string? ZTREEID1 { get; set; }
        /// <summary>
        /// 组织树版本号
        /// </summary>
        public string? ZTREEVER { get; set; }
        /// <summary>
        /// 视图标识 多值按,隔开，值域:zx-机构视图、zy-核算组织视图、ZG-管理组织视图、Z4-税务代管组织（行政）视图。
        /// </summary>
        public string? VIEW_FLAG { get; set; }

        /// <summary>
        /// 企业分类代码 企业分类,如果多值则按“,”（英文逗号）进行隔开
        /// </summary>
        public string? ZENTC { get; set; }
    }
}
