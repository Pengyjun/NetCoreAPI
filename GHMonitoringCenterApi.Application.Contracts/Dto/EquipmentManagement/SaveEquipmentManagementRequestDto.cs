using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.EquipmentManagement
{
    /// <summary>
    /// 保存设备请求Dto
    /// </summary>
    public class SaveEquipmentManagementRequestDto : IValidatableObject
    {
        /// <summary>
        /// 请求类型   true新增  false编辑
        /// </summary>
        public bool RequestType { get; set; }
        /// <summary>
        ///  设备类型 1. 水上设备  2. 陆域设备   3. 特种设备
        /// </summary>
        public int DeviceType { get; set; }
        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }
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
        /// 设备中类Id
        /// </summary>
        public Guid? DeviceClassId { get; set; }
        /// <summary>
        /// 设备小类Id
        /// </summary>
        public Guid? EquipmentSubcategoriesId { get; set; }
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
        /// <summary>
        /// 出厂合格证号
        /// </summary>
        public string? CertificateNumber { get; set; }
        /// <summary>
        /// 特种设备检验证号
        /// </summary>

        public string? SpecialEquipmentNumber { get; set; }
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
        /// 船舶名称
        /// </summary>
        public string? ShipName { get; set; }
        /// <summary>
        /// 航区Id
        /// </summary>
        public Guid? NavigationAreaId { get; set; }
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
        /// 船舶定员
        /// </summary>
        public string? ShipCapacity { get; set; }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            if (CompanyId == null || CompanyId == Guid.Empty)
            {
                yield return new ValidationResult("所属公司参数不能为空且类型是Id", new string[] { nameof(CompanyId) });
            }
            if (ProjectId == null || ProjectId == Guid.Empty)
            {
                yield return new ValidationResult("项目参数不能为空且类型是Id", new string[] { nameof(ProjectId) });
            }
            if (SubcontractorName == null)
            {
                yield return new ValidationResult("分包商名称不能为空且类型是string", new string[] { nameof(SubcontractorName) });
            }
            //if (ContractAmount == null)
            //{
            //    yield return new ValidationResult("分包商名称不能为空且类型是decimal", new string[] { nameof(ContractAmount) });
            //}
            if (DeviceType == 1 && ShipName == null)

            {
                yield return new ValidationResult("船舶名称参数不能为空且类型是string", new string[] { nameof(ShipName) });
            }
            if (DeviceType == 1 && (ShipTypeId == null || ShipTypeId == Guid.Empty))
            {
                yield return new ValidationResult("船舶类型Id参数不能为空且类型是Id", new string[] { nameof(ShipTypeId) });
            }
            if (DeviceType == 1 && (NavigationAreaId == null || NavigationAreaId == Guid.Empty))
            {
                yield return new ValidationResult("航区Id参数不能为空且类型是Id", new string[] { nameof(NavigationAreaId) });
            }
            if (DeviceType == 1 && MMSI == null)
            {
                yield return new ValidationResult("海上移动识别码（MMSI）参数不能为空且类型是string", new string[] { nameof(MMSI) });
            }
            if (DeviceType == 1 && ShipOwner == null)
            {
                yield return new ValidationResult("船舶所有人参数不能为空且类型是string", new string[] { nameof(ShipOwner) });
            }
            if (DeviceType == 1 && ShipSizeSpecifications == null)
            {
                yield return new ValidationResult("船舶规格尺寸参数不能为空且类型是string", new string[] { nameof(ShipSizeSpecifications) });
            }
            if (DeviceType == 1 && CompletionDate == null)
            {
                yield return new ValidationResult("建造完工日期参数不能为空且类型是DateTime", new string[] { nameof(CompletionDate) });
            }
            if (DeviceType == 1 && EntryDate == null)
            {
                yield return new ValidationResult("进场日期参数不能为空且类型是DateTime", new string[] { nameof(EntryDate) });
            }
            if ((DeviceType == 2 || DeviceType == 3) && (EquipmentCategoryId == null || EquipmentCategoryId == Guid.Empty))
            {
                yield return new ValidationResult("设备大类参数不能为空且类型是Id", new string[] { nameof(EquipmentCategoryId) });
            }
            if (DeviceType == 2 && (DeviceClassId == null || DeviceClassId == Guid.Empty))
            {
                yield return new ValidationResult("设备中类参数不能为空且类型是Id", new string[] { nameof(DeviceClassId) });
            }
            if (DeviceType == 2 && (EquipmentSubcategoriesId == null || EquipmentSubcategoriesId == Guid.Empty))
            {
                yield return new ValidationResult("设备小类参数不能为空且类型是Id", new string[] { nameof(EquipmentSubcategoriesId) });
            }
            if ((DeviceType == 2 || DeviceType == 3) && EquipmentModel == null)
            {
                yield return new ValidationResult("设备型号参数不能为空且类型是Id", new string[] { nameof(EquipmentModel) });
            }
            if ((DeviceType == 2 || DeviceType == 3) && Manufacturer == null)
            {
                yield return new ValidationResult("出厂场家参数不能为空且类型是Id", new string[] { nameof(Manufacturer) });
            }
            if ((DeviceType == 2 || DeviceType == 3) && FactoryDate == null)
            {
                yield return new ValidationResult("出厂日期参数不能为空且类型是Id", new string[] { nameof(FactoryDate) });
            }
            if ((DeviceType == 2 || DeviceType == 3) && CertificateNumber == null)
            {
                yield return new ValidationResult("出厂合格证号参数不能为空且类型是Id", new string[] { nameof(CertificateNumber) });
            }
            if (ExitDate < EntryDate)
            {
                yield return new ValidationResult("进场时间要小于退场时间", new string[] { nameof(ExitDate) });
            }
        }
    }

}
