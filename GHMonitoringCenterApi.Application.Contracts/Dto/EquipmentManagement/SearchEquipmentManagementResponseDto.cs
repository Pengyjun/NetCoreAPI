using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.EquipmentManagement
{
    /// <summary>
    /// 设备管理返回Dto
    /// </summary>
    public class SearchEquipmentManagementResponseDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 填报月份
        /// </summary>
        public DateTime? ReportingMonth { get; set; }
        /// <summary>
        /// 所属公司Id
        /// </summary>
        public Guid? CompanyId { get; set; }
        /// <summary>
        /// 所属公司名称
        /// </summary>
        public string? CompanyName { get; set; }
        /// <summary>
        /// 特种设备检验证号
        /// </summary>

        public string? SpecialEquipmentNumber { get; set; }
        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid? ProjectId { get; set; }
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
        /// 设备大类Id
        /// </summary>
        public Guid? EquipmentCategoryId { get; set; }
        /// <summary>
        /// 船舶大类名称
        /// </summary>
        public string? EquipmentCategoryName { get; set; }
        /// <summary>
        /// 设备中类Id
        /// </summary>
        public Guid? DeviceClassId { get; set; }
        /// <summary>
        /// 船舶中类名称
        /// </summary>
        public string? DeviceClassName { get; set; }
        /// <summary>
        /// 设备小类Id
        /// </summary>
        public Guid? EquipmentSubcategoriesId { get; set; }
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
        public DateTime? FactoryDate { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 出厂合格证号
        /// </summary>
        public string? CertificateNumber { get; set; }
        /// <summary>
        /// 规格1
        /// </summary>
        public string? Specification1 { get; set; }
        /// <summary>
        /// 规格2
        /// </summary>
        public string? Specification2 { get; set; }
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
        /// <summary>
        /// 船舶Id
        /// </summary>
        public Guid? ShipId { get; set; }
        /// <summary>
        /// 船舶类型Id
        /// </summary>
        public Guid? ShipTypeId { get; set; }
        /// <summary>
        /// 船舶类型名称
        /// </summary>
        public string? ShipTypeName { get; set; }
        /// <summary>
        /// 船舶名称
        /// </summary>
        public string? ShipName { get; set; }
        /// <summary>
        /// 航区Id
        /// </summary>
        public Guid? NavigationAreaId { get; set; }
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
        public string ShipSizeSpecifications { get; set; }
        /// <summary>
        /// 船舶定员
        /// </summary>
        public string? ShipCapacity { get; set; }
    }
}
