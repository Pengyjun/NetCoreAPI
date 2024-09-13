using AutoMapper;
using GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.Institution;
using GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.User;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.AccountingDepartment;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.AccountingOrganization;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.AdministrativeAccountingMapper;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.AdministrativeDivision;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.AdministrativeOrganization;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.BankCard;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.BusinessNoCpportunity;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Common;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CorresUnit;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CountryContinent;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CountryRegion;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Currency;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.DeviceClassCode;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.DeviceDetailCode;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.EscrowOrganization;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.FinancialInstitution;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.InvoiceType;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Language;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.LouDong;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.NationalEconomy;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.POPManagOrg;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Project;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.ProjectClassification;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Regional;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.RegionalCenter;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.RelationalContracts;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.RoomNumber;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.ScientifiCNoProject;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.TypeOfBidDisclosureProjectTable;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.UnitMeasurement;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.ValueDomain;
using GDCMasterDataReceiveApi.Domain.Models;

namespace GDCMasterDataReceiveApi.Application.Contracts.AutoMapper
{
    /// <summary>
    /// 映射
    /// </summary>
    public class AutoMapperProfileFile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapperConfigurationExpression"></param>
        public static void AutoMapperProfileInit(IMapperConfigurationExpression mapperConfigurationExpression)
        {
            //mapperConfigurationExpression.CreateMap<PomCurrencyResponseDto, Currency>()
            //      .ForMember(x => x.PomId, y => y.MapFrom(u => u.Id));

            //楼栋
            mapperConfigurationExpression.CreateMap<LouDongItem, LouDong>();
            //核算部门
            mapperConfigurationExpression.CreateMap<AccountingDepartmentReceiveDto, AccountingDepartment>();
            //多组织-核算机构
            mapperConfigurationExpression.CreateMap<AccountingOrganizationReceiveDto, AccountingOrganization>();
            //行政机构和核算机构映射关系
            mapperConfigurationExpression.CreateMap<AdministrativeAccountingMapperItem, AdministrativeAccountingMapper>();
            //境内行政区划
            mapperConfigurationExpression.CreateMap<AdministrativeDivisionItem, AdministrativeDivision>();
            //银行账号
            mapperConfigurationExpression.CreateMap<BankCardItem, BankCard>();
            //商机项目(不含境外商机项目)
            mapperConfigurationExpression.CreateMap<BusinessNoCpportunityReceiveDto, BusinessNoCpportunity>();
            //通用类字典数据
            mapperConfigurationExpression.CreateMap<ValueDomainReceiveRequestDto, ValueDomain>();
            //往来单位主数据
            mapperConfigurationExpression.CreateMap<CorresUnitReceiveDto, CorresUnit>();
            //大洲
            mapperConfigurationExpression.CreateMap<CountryContinentReceiveDto, CountryContinent>();
            //国家地区
            mapperConfigurationExpression.CreateMap<CountryRegionReceiveDto, CountryRegion>();
            //币种
            mapperConfigurationExpression.CreateMap<CurrencyReceiveDto, Currency>();
            //物资设备分类编码
            mapperConfigurationExpression.CreateMap<DeviceClassCodeItem, DeviceClassCode>();
            //物资设备明细编码
            mapperConfigurationExpression.CreateMap<DeviceDetailCodeReceiveDto, DeviceDetailCode>();
            //多组织-税务代管组织(行政)
            mapperConfigurationExpression.CreateMap<EscrowOrganizationReceiveDto, EscrowOrganization>();
            //金融机构
            mapperConfigurationExpression.CreateMap<FinancialInstitutionReceiveDto, FinancialInstitution>();
            //机构主数据
            mapperConfigurationExpression.CreateMap<InstitutionItem, Institution>();
            //发票类型
            mapperConfigurationExpression.CreateMap<InvoiceTypeItem, InvoiceType>();
            //语言语种
            mapperConfigurationExpression.CreateMap<LanguageItem, Language>();
            //国民经济行业分类
            mapperConfigurationExpression.CreateMap<NationalEconomyItem, NationalEconomy>();
            //生产经营管理组织
            mapperConfigurationExpression.CreateMap<POPMangOrgItem, POPManagOrg>();
            //人员主数据
            mapperConfigurationExpression.CreateMap<Receive4AUser, User>();
            //项目类
            mapperConfigurationExpression.CreateMap<ProjectItem, Project>()
                //.ForMember(dest => dest.ZOLDNAME_LISTItem, opt => opt.MapFrom(src => src.ZOLDNAME_LIST))
                ;
            //mapperConfigurationExpression.CreateMap<ZOLDNAME_LIST, ZOLDNAME_LISTItem>()
            //  .ForMember(dest => dest.ProjectUsedNameItems, opt => opt.MapFrom(src => src.item));
            //中交项目行业分类产业分类、业务板块、十二大业务类型、江河湖海对照关系
            mapperConfigurationExpression.CreateMap<ProjectClassificationItem, ProjectClassification>();
            //中交区域总部
            mapperConfigurationExpression.CreateMap<RegionalItem, Regional>();
            //中交区域中心
            mapperConfigurationExpression.CreateMap<RegionalCenterItem, RegionalCenter>();
            //委托关系
            mapperConfigurationExpression.CreateMap<RelationalContractsItem, RelationalContracts>();
            //房号
            mapperConfigurationExpression.CreateMap<RoomNumberItem, RoomNumber>();
            //科研项目
            mapperConfigurationExpression.CreateMap<ScientifiCNoProjectItem, ScientifiCNoProject>();
            //中标交底项目表类型
            mapperConfigurationExpression.CreateMap<TypeOfBidDisclosureProjectTableReceiveDto, TypeOfBidDisclosureProjectTable>();
            //常用计量单位
            mapperConfigurationExpression.CreateMap<UnitMeasurementItem, UnitMeasurement>();
            //多组织行政机构
            mapperConfigurationExpression.CreateMap<AdministrativeOrganizationReceiveRequestDto, AdministrativeOrganization>();


            #region 其他映射
            //mapperConfigurationExpression.CreateMap<ClassDevice, DeviceClassAttribute>();
            //mapperConfigurationExpression.CreateMap<ClassDeviceValue, DeviceClassAttributeValue>();
            //mapperConfigurationExpression.CreateMap<InvoiceLanguageItem, InvoiceLanguage>();
            //mapperConfigurationExpression.CreateMap<ScientifiCNoProjectItem, ScientifiCNoProject>();
            //mapperConfigurationExpression.CreateMap<SecUnit, KySecUnit>();
            //mapperConfigurationExpression.CreateMap<CDUnit, KyCDUnit>();
            //mapperConfigurationExpression.CreateMap<NameCeng, KyNameCeng>();
            //mapperConfigurationExpression.CreateMap<CanYUnit, KyCanYUnit>();
            //mapperConfigurationExpression.CreateMap<WeiTUnit, KyWeiTUnit>();
            //mapperConfigurationExpression.CreateMap<PLeader, KyPLeader>();
            //mapperConfigurationExpression.CreateMap<CanYDep, KyCanYDep>();
            //mapperConfigurationExpression.CreateMap<LanguageC, LanguageDetails>();
            #endregion

        }
    }
}
