using GHMonitoringCenterApi.Domain.Shared.Util;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 添加或修改项目请求DTO
    /// </summary>
    public class AddOrUpdateProjectRequestDto : IValidatableObject
    {
        /// <summary>
        /// 项目管理类型
        /// </summary>
        public string? ManagerType { get; set; }
        /// <summary>
        /// 请求类型  true是添加   false是修改
        /// </summary>
        public bool RequestType { get; set; }
        /// <summary>
        /// 项目ID  新增时不进行传参
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 项目主数据编码
        /// </summary>
        public string? MasterCode { get; set; }
        /// <summary>
        /// 项目编码
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// 项目简称
        /// </summary>
        public string? ShortName { get; set; }
        /// <summary>
        /// 所属公司ID
        /// </summary>
        public Guid? CompanyId { get; set; }
        /// <summary>
        /// 所属项目部
        /// </summary>
        public Guid? ProjectDept { get; set; }
        /// <summary>
        /// 合同工程量
        /// </summary>
        //public decimal? Quantity { get; set; }
        /// <summary>
        /// 项目部地址
        /// </summary>
        public string? ProjectDeptAddress { get; set; }
        /// <summary>
        /// 施工地点Id
        /// </summary>
        public Guid? AreaId { get; set; }
        /// <summary>
        /// 施工区域Id
        /// </summary>
        public Guid? RegionId { get; set; }
        /// <summary>
        /// 项目类别
        /// </summary>
        public int Category { get; set; }
        /// <summary>
        /// 类型ID
        /// </summary>
        public Guid? TypeId { get; set; }
        /// <summary>
        /// 税率
        /// </summary>
        public decimal? Rate { get; set; }
        /// <summary>
        /// 汇率
        /// </summary>
        public decimal? ExchangeRate { get; set; }
        /// <summary>
        /// 合同金额
        /// </summary>
        public decimal? Amount { get; set; }
        /// <summary>
        /// 有效合同额
        /// </summary>
        public decimal? ECAmount { get; set; }
        /// <summary>
        /// 合同约定的计量支付比例(%)
        /// </summary>
        public decimal? ContractMeaPayProp { get; set; }

        /// <summary>
        /// 项目施工资质Id
        /// </summary>
        public Guid? ProjectConstructionQualificationId { get; set; }
        /// <summary>
        /// 项目施工资质
        /// </summary>
        public string? ProjectConstructionQualificationName { get; set; }
        /// <summary>
        /// 货币Id
        /// </summary>
        public Guid? CurrencyId { get; set; }
        /// <summary>
        /// 项目状态ID
        /// </summary>
        public Guid? StatusId { get; set; }
        /// <summary>
        /// 项目规模Id
        /// </summary>
        public Guid? GradeId { get; set; }
        /// <summary>
        /// 传统、新兴工程类别标签（0：传统；1：新兴）
        /// </summary>
        public int Tag { get; set; }
        /// <summary>
        /// 现汇、投资工程类别标签（0：现汇；1：投资）
        /// </summary>
        public int Tag2 { get; set; }
        /// <summary>
        /// 是否完成编制
        /// </summary>
        public bool IsStrength { get; set; }
        /// <summary>
        /// 标后预算毛利率
        /// </summary>
        public decimal? BudgetInterestRate { get; set; }
        /// <summary>
        /// 计划完成编制时间
        /// </summary>
        public DateTime? CompilationTime { get; set; }
        /// <summary>
        /// 未编制标后预算原因
        /// </summary>
        public string? BudgetaryReasons { get; set; }
        /// <summary>
        /// 行业分类标准
        /// </summary>
        public string? ClassifyStandard { get; set; }
        /// <summary>
        /// 项目所在地点经度
        /// </summary>
        public decimal? Longitude { get; set; }
        /// <summary>
        /// 项目所在地点维度
        /// </summary>
        public decimal? Latitude { get; set; }
        /// <summary>
        /// 具有特殊社会效应
        /// </summary>
        public bool SocietySpecEffect { get; set; }
        /// <summary>
        /// 疏浚吹填面积(万方)
        /// </summary>
        public decimal? ReclamationArea { get; set; }
        /// <summary>
        /// 工况级数id
        /// </summary>
        public string? ConditionGradeId { get; set; }
        /// <summary>
        /// 管理员人数(人)
        /// </summary>
        public int? Administrator { get; set; }
        /// <summary>
        /// 施工人数(人)
        /// </summary>
        public int? ConstructorNum { get; set; }
        /// <summary>
        /// 报表负责人
        /// </summary>
        public string? ReportFormer { get; set; }
        /// <summary>
        /// 报表负责人联系方式
        /// </summary>
        public string? ReportForMertel { get; set; }
        /// <summary>
        /// 项目概况
        /// </summary>
        public string? QuantityRemarks { get; set; }
        /// <summary>
        /// 主数据项目Id  新增时不进行传参
        /// </summary>
        public Guid? MasterProjectId { get; set; }
        /// <summary>
        /// 项目干系单位集合
        /// </summary>
        public List<ProjectOrgDtos>? projectOrgDtos { get; set; }
        /// <summary>
        /// 项目干系人员信息集合
        /// </summary>
        public List<ProjectDutyDtos>? projectDutyDtos { get; set; }
        /// <summary>
        /// 开工时间
        /// </summary>
        public DateTime? CommencementTime { get; set; }
        /// <summary>
        /// 完工日期
        /// </summary>
        public DateTime? CompletionTime { get; set; }
        /// <summary>
        /// 完工工程量
        /// </summary>
        public decimal? CompleteQuantity { get; set; }
        /// <summary>
        ///  完工产值
        /// </summary>
        public decimal? CompleteOutput { get; set; }
        /// <summary>
        /// 项目合同工期(开始)
        /// </summary>
        public DateTime? StartContractDuration { get; set; }
        /// <summary>
        /// 项目合同工期(结束)
        /// </summary>
        public DateTime? EndContractDuration { get; set; }
        /// <summary>
        /// 工程位置
        /// </summary>
        public string? ProjectLocation { get; set; }
        /// <summary>
        /// 中标日期
        /// </summary>
        public DateTime? BidWinningDate { get; set; }
        /// <summary>
        /// 停工日期
        /// </summary>
        public DateTime? ShutdownDate { get; set; }
        /// <summary>
        /// 工期信息
        /// </summary>
        public string? DurationInformation { get; set; }
        /// <summary>
        /// 停工原因
        /// </summary>
        public string? ShutDownReason { get; set; }
        /// <summary>
        /// 合同变更原因
        /// </summary>
        public string? ContractChangeInfo { get; set; }
        /// <summary>
        /// 工期默认给1
        /// </summary>
        public int WorkDay { get; set; }
        /// <summary>
        /// 合同约定开工日期
        /// </summary>
        public DateTime? ContractStipulationStartDate { get; set; }
        /// <summary>
        /// 合同约定完工日期
        /// </summary>
        public DateTime? ContractStipulationEndDate { get; set; }
        /// <summary>
        /// 合同签约日期
        /// </summary>
        public DateTime? ContractSignDate { get; set; }

        /// <summary>
        /// 分包项目主数据编码
        /// </summary>
        public string? PProjectMasterCode { get; set; }
        /// <summary>
        /// 是否是分包项目  0不是   1是  
        /// </summary>
        public int? IsSubContractProject { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                yield return new ValidationResult("Name参数不能为空且类型是string类型", new string[] { nameof(Name) });
            }

            if (Name != null && Name.Length > 300)
            {
                yield return new ValidationResult("项目名称过长，长度不能超过300", new string[] { nameof(Name) });
            }

            if (StatusId == Guid.Empty)
            {
                yield return new ValidationResult("StatusId参数不能为空且类型是guid类型", new string[] { nameof(StatusId) });
            }


            //中标已签  中表未签 项目状态为中标已签或中标未签 其他必填项可为空
            if (StatusId != Guid.Empty && (StatusId == "75089b9a-b18b-442c-bfc8-fde4024d737f".ToGuid() || StatusId == "fa66f679-c749-4f25-8f1a-5e1728a219ad".ToGuid()))
            {
                Console.WriteLine("aaa");
            }
            else
            {
                if (WorkDay == 0)
                {
                    WorkDay = 1;
                }

                //if (ManagerType==null&&ManagerType<1|| ManagerType >6)
                //{
                //    yield return new ValidationResult("项目类型不合法请重新填", new string[] { nameof(ManagerType) });
                //}
                if (string.IsNullOrWhiteSpace(ManagerType))
                {
                    yield return new ValidationResult("项目类型不合法请重新填", new string[] { nameof(ManagerType) });
                }
                if (ProjectLocation != null && ProjectLocation.Length > 2000)
                {
                    yield return new ValidationResult("工程位置字数不能超过2000", new string[] { nameof(ProjectLocation) });
                }
                if (DurationInformation != null && DurationInformation.Length > 2000)
                {
                    yield return new ValidationResult("工程信息字数不能超过2000", new string[] { nameof(DurationInformation) });
                }
                //if (string.IsNullOrWhiteSpace(Code))
                //{
                //    yield return new ValidationResult("Code参数不能为空且类型是string类型", new string[] { nameof(Code) });
                //}
                if (string.IsNullOrWhiteSpace(ShortName))
                {
                    yield return new ValidationResult("ShortName参数不能为空且类型是string类型", new string[] { nameof(ShortName) });
                }
                if (ShortName != null && ShortName.Length > 300)
                {
                    yield return new ValidationResult("项目简称过长，长度不能超过300", new string[] { nameof(ShortName) });
                }
                if (CompanyId == Guid.Empty)
                {
                    yield return new ValidationResult("CompanyId参数不能为空且类型是guid类型", new string[] { nameof(CompanyId) });
                }
                if (ProjectDept == Guid.Empty)
                {
                    yield return new ValidationResult("DownCompanyId参数不能为空且类型是string类型", new string[] { nameof(ProjectDept) });
                }
                if (string.IsNullOrWhiteSpace(ProjectDeptAddress))
                {
                    yield return new ValidationResult("ProjectDeptAddress参数不能为空且类型是string类型", new string[] { nameof(ProjectDeptAddress) });
                }
                if (Rate < 0)
                {
                    yield return new ValidationResult("Rate参数不能为空，不能为负数且类型是decimal类型", new string[] { nameof(Rate) });
                }
                if (Amount < 0)
                {
                    yield return new ValidationResult("Amount参数不能为空，不能为负数且类型是decimal类型", new string[] { nameof(Amount) });
                }
                if (ECAmount < 0)
                {
                    yield return new ValidationResult("ECAmount参数不能为空，不能为负数且类型是decimal类型", new string[] { nameof(ECAmount) });
                }
                if (ContractMeaPayProp < 0)
                {
                    yield return new ValidationResult("ContractMeaPayProp参数不能为空，不能为负数且类型是decimal类型", new string[] { nameof(ContractMeaPayProp) });
                }
                if (TypeId == Guid.Empty)
                {
                    yield return new ValidationResult("TypeId参数不能为空且类型是guid类型", new string[] { nameof(TypeId) });
                }
                if (GradeId == Guid.Empty)
                {
                    yield return new ValidationResult("GradeId参数不能为空且类型是guid类型", new string[] { nameof(GradeId) });
                }
                if (IsStrength)
                {
                    if (string.IsNullOrWhiteSpace(BudgetInterestRate.ToString()))
                    {
                        yield return new ValidationResult("BudgetInterestRate参数不能为空且类型是decimal类型", new string[] { nameof(BudgetInterestRate) });
                    }
                }
                if (string.IsNullOrWhiteSpace(ClassifyStandard))
                {
                    yield return new ValidationResult("ClassifyStandard参数不能为空且类型是sting类型", new string[] { nameof(ClassifyStandard) });
                }
                if (!IsStrength)
                {
                    if (!CompilationTime.HasValue)
                    {
                        yield return new ValidationResult("CompilationTime参数不能为空且类型是datetime类型", new string[] { nameof(CompilationTime) });
                    }
                }
                if (ProjectConstructionQualificationId == Guid.Empty)
                {
                    yield return new ValidationResult("ProjectConstructionQualificationId参数不能为空且类型是guid类型", new string[] { nameof(ProjectConstructionQualificationId) });
                }
                //其他非施工类业务 不走此验证 暂时和项目状态不关联
                if (TypeId != "048120ae-1e9f-46d8-a38f-5d5e9e49ecba".ToGuid())
                {
                    if (StatusId == "cd3c6e83-1b7c-40c2-a415-5a44f13584cc".ToGuid() && !CommencementTime.HasValue)
                    {
                        yield return new ValidationResult("在建项目必须输入开工时间", new string[] { nameof(CommencementTime) });
                    }
                }
                //其他非施工类业务 不走此验证 暂时和项目状态不关联
                if (TypeId != "048120ae-1e9f-46d8-a38f-5d5e9e49ecba".ToGuid())
                {
                    if ((StatusId == "2a30d69d-ad3a-4a51-a1d7-7b363150437d".ToGuid() || StatusId == "28bc58dc-41ed-4135-8628-a4e6a571032b".ToGuid() || StatusId == "62986752-9b40-4d02-8887-b014a6ee7a9d".ToGuid()) && !CompletionTime.HasValue)
                    {
                        yield return new ValidationResult("交工、竣工、完工项目必须输入完工时间", new string[] { nameof(CompletionTime) });
                    }
                }
                if (projectDutyDtos.Any())
                {
                    foreach (var item in projectDutyDtos)
                    {
                        if (item.BeginDate != null && item.EndDate != null && item.BeginDate > item.EndDate)
                        {
                            yield return new ValidationResult("创建时间必须小于结束时间", new string[] { nameof(item.EndDate) });
                        }
                    }

                }

                if (IsSubContractProject == 1 && string.IsNullOrWhiteSpace(PProjectMasterCode))
                {
                    yield return new ValidationResult("分包项目必须要填项目主数据编码", new string[] { nameof(PProjectMasterCode) });
                }
                //if (ManagerType==null||!ManagerType.HasValue||ManagerType.Value<1||ManagerType.Value>5)
                //{
                //    yield return new ValidationResult("项目管理类型不合法", new string[] { nameof(ManagerType) });
                //}

            }
        }
    }
    /// <summary>
    /// 项目干系 项目单位名称
    /// </summary>
    public class ProjectOrgDtos
    {
        /// <summary>
        /// Id 新增时不进行传参
        /// </summary>
        public Guid? Id { get; set; }
        ///// <summary>
        ///// PomId  新增时不进行传参
        ///// </summary>
        //public Guid? PomId { get; set; }
        /// <summary>
        /// 项目Id  新增时不进行传参
        /// </summary>
        public Guid? ProjectId { get; set; }
        /// <summary>
        /// 项目干系单位Id
        /// </summary>
        public Guid? OrganizationId { get; set; }
        /// <summary>
        /// 项目干系单位类型
        /// </summary>
        public int Type { get; set; }
    }

    /// <summary>
    /// 项目干系人员信息
    /// </summary>
    public class ProjectDutyDtos
    {
        /// <summary>
        /// Id 新增时不进行传参
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// PomId  新增时不进行传参
        /// </summary>
        //public Guid? PomId { get; set; }
        /// <summary>
        /// 人员职位类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 项目Id  新增时不进行传参
        /// </summary>
        public Guid? ProjectId { get; set; }
        /// <summary>
        /// 项目编码
        /// </summary>
        //public string? ProjectCode { get; set; }
        /// <summary>
        /// 人员 Id
        /// </summary>
        public Guid AssistantManagerId { get; set; }
        /// <summary>
        /// 是否在任 true:在任 false:离任
        /// </summary>
        public bool IsPresent { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? BeginDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndDate { get; set; }
        public IEnumerable<ValidationResult> Validates(ValidationContext validationContext)
        {
            if (BeginDate.Value.ToDateDay() > EndDate.Value.ToDateDay())
            {
                yield return new ValidationResult("开始日期必须小于结束日期", new string[] { nameof(EndDate) });
            }
        }
    }
}
