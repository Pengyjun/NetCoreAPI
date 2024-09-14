using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 物资设备明细属性
    /// </summary>
    [SugarTable("t_devicedetailattribute", IsDisabledDelete = true)]
    public class DeviceDetailAttribute : BaseEntity<long>
    {
        /// <summary>
        /// 物资设备主数据编码
        /// 11
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? ZMATERIAL { set; get; }
        /// <summary>
        /// 属性编码
        /// 10
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? ZATTRCODE { set; get; }
        /// <summary>
        /// 属性名称
        /// 100
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? ZATTRNAME { set; get; }
        /// <summary>
        /// 属性值编码
        /// 10
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? ZVALUECODE { set; get; }
        /// <summary>
        /// 属性值名称
        /// 100
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? ZVALUENAME { set; get; }
        /// <summary>
        /// 属性计量单位
        /// 50
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ZATTRUNIT { set; get; }
    }
}
