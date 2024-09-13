using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 多组织-行政组织
    /// </summary>
    [SugarTable("t_administrativeorganization", IsDisabledDelete = true)]
    public class AdministrativeOrganization : BaseEntity<long>
    {
        /// <summary>
        /// 机构主数据编码
        /// </summary>
        [SugarColumn(Length = 64)]
        public string? MDM_CODE { get; set; }


        /// <summary>
        /// 机构名称（中文）
        /// </summary>
        [SugarColumn(Length = 256)]
        public string? ZZTNAME_ZH { get; set; }


        /// <summary>
        /// 机构简称（中文)
        /// </summary>
        [SugarColumn(Length = 256)]
        public string? ZZTSHNAME_CHS { get; set; }


        /// <summary>
        /// 机构名称（英文）
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? ZZTNAME_EN { get; set; }


        /// <summary>
        /// 机构简称（英文）
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? ZZTSHNAME_EN { get; set; }

        /// <summary>
        /// 机构名称（当地语言）
        /// </summary>
        [SugarColumn(Length = 64)]
        public string? ZZTNAME_LOC { get; set; }

        /// <summary>
        /// 机构简称（当地语言）  
        /// </summary>
        [SugarColumn(Length = 64)]
        public string? ZZTSHNAME_LOC { get; set; }


        /// <summary>
        /// 机构状态  值域：1-运营、2-筹备、3-停用、4-撤销
        /// </summary>
        [SugarColumn(Length = 32)]
        public string? ZOSTATE { get; set; }

        /// <summary>
        /// 上级机构主数据编码
        /// </summary>
        [SugarColumn(Length = 64)]
        public string? ZORGUP { get; set; }

        /// <summary>
        /// 机构主数据编码（旧）
        /// </summary>
        [SugarColumn(Length = 64)]
        public string? OID { get; set; }
        /// <summary>
        /// 上级机构主数据编码（旧）
        /// </summary>
        [SugarColumn(Length = 64)]
        public string? POID { get; set; }

        /// <summary>
        /// 机构规则码
        /// </summary>
        [SugarColumn(Length = 1024)]
        public string? ZORULE { get; set; }

        /// <summary>
        /// 所属二级单位编码
        /// </summary>
        [SugarColumn(Length = 64)]
        public string? ZGPOID { get; set; }

        /// <summary>
        /// 层级）
        /// </summary>
        [SugarColumn(Length = 8)]
        public string? ZO_LEVEL { get; set; }

        /// <summary>
        /// 节点排序号
        /// </summary>
        [SugarColumn(Length = 8)]
        public string? ZSNO { get; set; }

        /// <summary>
        /// 机构编码 机构27位有含义编码
        /// </summary>
        [SugarColumn(Length = 64)]
        public string? ZORGNO { get; set; }

        /// <summary>
        ///机构属性
        /// </summary>
        [SugarColumn(Length = 8)]
        public string? ZOATTR { get; set; }
        /// <summary>
        /// 机构子属性
        /// </summary>
        [SugarColumn(Length = 8)]
        public string? ZOCATTR { get; set; }
        /// <summary>
        /// 国家名称
        [SugarColumn(Length = 32)]
        public string? ZCYNAME { get; set; }
        /// <summary>
        /// 机构所在地
        /// </summary>
        [SugarColumn(Length = 64)]
        public string? ZORGLOC { get; set; }
        /// <summary>
        /// 地域属性
        /// </summary>
        [SugarColumn(Length = 32)]
        public string? ZREGIONAL { get; set; }
        /// <summary>
        /// 注册号/统一社会信用代码
        /// </summary>
        [SugarColumn(Length = 32)]
        public string? ZCUSCC { get; set; }
        /// <summary>
        /// 通讯地址
        /// </summary>
        [SugarColumn(Length = 32)]
        public string? ZADDRESS { get; set; }
        /// <summary>
        /// 持股情况
        /// </summary>
        [SugarColumn(Length = 32)]
        public string? ZHOLDING { get; set; }
        /// <summary>
        /// 是否独立核算  是否单独建立核算账套对本机构发生的业务进行会计核算，填写“是”或“否
        /// </summary>
        [SugarColumn(Length = 32)]
        public string? ZCHECKIND { get; set; }
        /// <summary>
        /// 组织树编码
        /// </summary>
        [SugarColumn(Length = 32)]
        public string? ZTREEID1 { get; set; }
        /// <summary>
        /// 组织树版本号
        /// </summary>
        [SugarColumn(Length = 32)]
        public string? ZTREEVER { get; set; }
        /// <summary>
        /// 视图标识 多值按,隔开，值域:zx-机构视图、zy-核算组织视图、ZG-管理组织视图、Z4-税务代管组织（行政）视图。
        /// </summary>
        [SugarColumn(Length = 32)]
        public string? VIEW_FLAG { get; set; }

        /// <summary>
        /// 企业分类代码 企业分类,如果多值则按“,”（英文逗号）进行隔开
        /// </summary>
        [SugarColumn(Length = 32)]
        public string? ZENTC { get; set; }
    }
}
