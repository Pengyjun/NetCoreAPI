using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 陆地设备管理表
    /// </summary>
    [SugarTable("t_landdevice", IsDisabledDelete = true)]
    public class LandDevice : BaseEntity<Guid>
    {
        /// <summary>
        /// 填报月份
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int ReportingMonth { get; set; }
        /// <summary>
        /// 所属公司Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid CompanyId { get; set; }
        /// <summary>
        /// 项目Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ProjectId { get; set; }
        /// <summary>
        /// 分包商名称
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? SubcontractorName { get; set; }
        /// <summary>
        /// 合同额
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? ContractAmount { get; set; }
        /// <summary>
        /// 设备大类Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? EquipmentCategoryId { get; set; }
        /// <summary>
        /// 设备中类Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? DeviceClassId { get; set; }
        /// <summary>
        /// 设备小类Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? EquipmentSubcategoriesId { get; set; }
        /// <summary>
        /// 设备型号
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? EquipmentModel { get; set; }
        /// <summary>
        /// 生产厂家
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? Manufacturer { get; set; }
        /// <summary>
        /// 出厂日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime FactoryDate { get; set; }
        /// <summary>
        /// 出厂合格证号
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? CertificateNumber { get; set; }
        /// <summary>
        /// 设备规格
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? EquipmentSpecifications { get; set; }
        /// <summary>
        /// 规格2
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? Specifications { get; set; }
        /// <summary>
        /// 进场时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime EntryDate { get; set; }
        /// <summary>
        /// 退场时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? ExitDate { get; set; }
        /// <summary>
        /// 是否在场  1，是  2，否
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? PresentNot { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 2000)]
        public string? Notes { get; set; }
    }
}
