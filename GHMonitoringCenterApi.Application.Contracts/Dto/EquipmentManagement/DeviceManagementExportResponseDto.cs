using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.EquipmentManagement
{
    /// <summary>
    /// 设备管理导出
    /// </summary>
    public class DeviceManagementExportResponseDto
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 水上设备
        /// </summary>
        public List<ExportMarineEquipment>? exportMarineEquipment { get; set; } = new List<ExportMarineEquipment>();
        /// <summary>
        /// 陆域设备
        /// </summary>
        public List<ExportLandEquipment>? exportLandEquipment { get; set; } = new List<ExportLandEquipment>();
        /// <summary>
        /// 特种设备
        /// </summary>
        public List<ExportSpecialEquipment>? exportSpecialEquipment { get; set; } = new List<ExportSpecialEquipment>();
    }
    /// <summary>
    /// 水上设备
    /// </summary>
    public class ExportMarineEquipment
    {
        /// <summary>
        /// 填报月份
        /// </summary>
        public string? ReportingMonth { get; set; }
        /// <summary>
        /// 所属公司名称
        /// </summary>
        public string? CompanyName { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ProjectName { get; set; }
        /// <summary>
        /// 分包商名称
        /// </summary>
        public string? SubcontractorName { get; set; }
        /// <summary>
        /// 合同额
        /// </summary>
        public string? ContractAmount { get; set; }
        /// <summary>
        /// 船舶名称
        /// </summary>
        public string? ShipName { get; set; }
        /// <summary>
        /// 船舶类型名称
        /// </summary>
        public string? ShipTypeName { get; set; }
        /// <summary>
        /// 航区名称
        /// </summary>
        public string? NavigationAreaName { get; set; }
        /// <summary>
        /// 海上移动识别码（MMSI）
        /// </summary>
        public string? MMSI { get; set; }
        /// <summary>
        /// 船舶所有人
        /// </summary>
        public string? ShipOwner { get; set; }
        /// <summary>
        /// 完工日期
        /// </summary>
        public DateTime? CompletionDate { get; set; }
        /// <summary>
        /// 船舶规格尺寸
        /// </summary>
        public string? ShipSizeSpecifications { get; set; }
        /// <summary>
        /// 规格1
        /// </summary>
        public string? Specification1 { get; set; }
        /// <summary>
        /// 规格2
        /// </summary>
        public string? Specification2 { get; set; }
        /// <summary>
        /// 船舶定员
        /// </summary>
        public string? ShipCapacity { get; set; }
        /// <summary>
        /// 进场时间
        /// </summary>
        public DateTime? EntryDate { get; set; }
        /// <summary>
        /// 退场时间
        /// </summary>
        public DateTime? ExitDate { get; set; }
        /// <summary>
        /// 是否在场  1，是  2，否
        /// </summary>
        public string? PresentNot { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Notes { get; set; }
    }
    /// <summary>
    /// 陆域设备
    /// </summary>
    public class ExportLandEquipment
    {
        /// <summary>
        /// 填报月份
        /// </summary>
        public string? ReportingMonth { get; set; }
        /// <summary>
        /// 所属公司名称
        /// </summary>
        public string? CompanyName { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ProjectName { get; set; }
        /// <summary>
        /// 分包商名称
        /// </summary>
        public string? SubcontractorName { get; set; }
        /// <summary>
        /// 合同额
        /// </summary>
        public string? ContractAmount { get; set; }
        /// <summary>
        /// 设备大类名称
        /// </summary>
        public string? EquipmentCategoryName { get; set; }
        /// <summary>
        /// 设备中类名称
        /// </summary>
        public string? DeviceClassName { get; set; }
        /// <summary>
        /// 设备小类名称
        /// </summary>
        public string? EquipmentSubcategoriesName { get; set; }
        /// <summary>
        /// 设备型号
        /// </summary>
        public string? EquipmentModel { get; set; }
        /// <summary>
        /// 生产厂家
        /// </summary>
        public string? Manufacturer { get; set; }
        /// <summary>
        /// 出厂日期
        /// </summary>
        public DateTime FactoryDate { get; set; }
        /// <summary>
        /// 出厂合格证号
        /// </summary>
        public string? CertificateNumber { get; set; }
        /// <summary>
        /// 设备规格
        /// </summary>
        public string? EquipmentSpecifications { get; set; }
        /// <summary>
        /// 规格2
        /// </summary>.0
        public string? Specifications { get; set; }
        /// <summary>
        /// 进场时间
        /// </summary>
        public DateTime EntryDate { get; set; }
        /// <summary>
        /// 退场时间
        /// </summary>
        public DateTime? ExitDate { get; set; }
        /// <summary>
        /// 是否在场  1，是  2，否
        /// </summary>
        public string? PresentNot { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Notes { get; set; }
    }
    /// <summary>
    /// 特种设备
    /// </summary>
    public class ExportSpecialEquipment
    {
        /// <summary>
        /// 填报月份
        /// </summary>
        public string? ReportingMonth { get; set; }
        /// <summary>
        /// 所属公司名称
        /// </summary>
        public string? CompanyName { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ProjectName { get; set; }
        /// <summary>
        /// 分包商名称
        /// </summary>
        public string? SubcontractorName { get; set; }
        /// <summary>
        /// 合同额
        /// </summary>
        public string? ContractAmount { get; set; }
        /// <summary>
        /// 设备大类名称
        /// </summary>
        public string? EquipmentCategoryName { get; set; }
        /// <summary>
        /// 设备型号
        /// </summary>
        public string? EquipmentModel { get; set; }
        /// <summary>
        /// 生产厂家
        /// </summary>
        public string? Manufacturer { get; set; }
        /// <summary>
        /// 出厂日期
        /// </summary>
        public DateTime FactoryDate { get; set; }
        /// <summary>
        /// 出厂合格证号
        /// </summary>
        public string? CertificateNumber { get; set; }

        /// <summary>
        /// 特种设备检验证号
        /// </summary>

        public string? SpecialEquipmentNumber { get; set; }
        /// <summary>
        /// 设备规格
        /// </summary>
        public string? EquipmentSpecifications { get; set; }
        /// <summary>
        /// 规格2
        /// </summary>
        public string? Specifications { get; set; }
        /// <summary>
        /// 进场时间
        /// </summary>
        public DateTime EntryDate { get; set; }
        /// <summary>
        /// 退场时间
        /// </summary>
        public DateTime? ExitDate { get; set; }
        /// <summary>
        /// 是否在场  1，是  2，否
        /// </summary>
        public string? PresentNot { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Notes { get; set; }
    }
}
