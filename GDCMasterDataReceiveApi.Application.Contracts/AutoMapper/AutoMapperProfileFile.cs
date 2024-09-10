using AutoMapper;
using GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.Institution;
using GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.User;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.AccountingDepartment;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.AccountingOrganization;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.AdministrativeAccountingMapper;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.AdministrativeDivision;
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
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Institution;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.InvoiceType;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Language;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.LouDong;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.NationalEconomy;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Project;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.ProjectClassification;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Regional;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.RegionalCenter;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.RelationalContracts;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.RoomNumber;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.TypeOfBidDisclosureProjectTable;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.UnitMeasurement;
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
            mapperConfigurationExpression.CreateMap<LouDongReceiveDto, LouDong>();
            //核算部门
            mapperConfigurationExpression.CreateMap<AccountingDepartmentReceiveDto, AccountingDepartment>();
            //多组织-核算机构
            mapperConfigurationExpression.CreateMap<AccountingOrganizationReceiveDto, AccountingOrganization>();
            //行政机构和核算机构映射关系
            mapperConfigurationExpression.CreateMap<AdministrativeAccountingMapperReceiveDto, AdministrativeAccountingMapper>();
            //境内行政区划
            mapperConfigurationExpression.CreateMap<AdministrativeDivisionReceiveDto, AdministrativeDivision>();
            //银行账号
            mapperConfigurationExpression.CreateMap<BankCardReceiveDto, BankCard>();
            //商机项目(不含境外商机项目)
            mapperConfigurationExpression.CreateMap<BusinessNoCpportunityReceiveDto, BusinessNoCpportunity>();
            //通用类字典数据
            mapperConfigurationExpression.CreateMap<CommonReceiveDto, Commons>();
            //往来单位主数据
            mapperConfigurationExpression.CreateMap<CorresUnitReceiveDto, CorresUnit>();
            //大洲
            mapperConfigurationExpression.CreateMap<CountryContinentReceiveDto, CountryContinent>();
            //国家地区
            mapperConfigurationExpression.CreateMap<CountryRegionReceiveDto, CountryRegion>();
            //币种
            mapperConfigurationExpression.CreateMap<CurrencyReceiveDto, Currency>();
            //物资设备分类编码
            mapperConfigurationExpression.CreateMap<DeviceClassCodeReceiveDto, DeviceClassCode>();
            //物资设备明细编码
            mapperConfigurationExpression.CreateMap<DeviceDetailCodeReceiveDto, DeviceDetailCode>();
            //多组织-税务代管组织(行政)
            mapperConfigurationExpression.CreateMap<EscrowOrganizationReceiveDto, EscrowOrganization>();
            //金融机构
            mapperConfigurationExpression.CreateMap<FinancialInstitutionReceiveDto, FinancialInstitution>();
            //机构主数据
            mapperConfigurationExpression.CreateMap<InstitutionItem, Institution>();
            //发票类型
            mapperConfigurationExpression.CreateMap<InvoiceTypeReceiveDto, InvoiceType>();
            //语言语种
            mapperConfigurationExpression.CreateMap<LanguageReceiveDto, Language>();
            //国民经济行业分类
            mapperConfigurationExpression.CreateMap<NationalEconomyReceiveDto, NationalEconomy>();
            //人员主数据
            mapperConfigurationExpression.CreateMap<Receive4AUser, User>();
            //项目类
            mapperConfigurationExpression.CreateMap<ProjectItem, Project>();
            //中交项目行业分类产业分类、业务板块、十二大业务类型、江河湖海对照关系
            mapperConfigurationExpression.CreateMap<ProjectClassificationReceiveDto, ProjectClassification>();
            //中交区域总部
            mapperConfigurationExpression.CreateMap<RegionalReceiveDto, Regional>();
            //中交区域中心
            mapperConfigurationExpression.CreateMap<RegionalCenterReceiveDto, RegionalCenter>();
            //委托关系
            mapperConfigurationExpression.CreateMap<RelationalContractsReceiveDto, RelationalContracts>();
            //房号
            mapperConfigurationExpression.CreateMap<RoomNumberReceiveDto, RoomNumber>();
            //科研项目
            //mapperConfigurationExpression.CreateMap<ScientifiCNoProjectReceiveDto, ScientifiCNoProject>();
            //中标交底项目表类型
            mapperConfigurationExpression.CreateMap<TypeOfBidDisclosureProjectTableReceiveDto, TypeOfBidDisclosureProjectTable>();
            //常用计量单位
            mapperConfigurationExpression.CreateMap<UnitMeasurementReceiveDto, UnitMeasurement>();


            #region 自行映射
            //mapperConfigurationExpression.CreateMap<User, UserSearchOtherColumns>()
            //      .ForMember(x => x.EmpCode, y => y.MapFrom(u => u.NAME_SPELL))
            //      .ForMember(x => x.Name, y => y.MapFrom(u => u.NAME_SPELL))
            //      .ForMember(x => x.Phone, y => y.MapFrom(u => u.NAME_SPELL))
            //      .ForMember(x => x.Email, y => y.MapFrom(u => u.NAME_SPELL))
            //      .ForMember(x => x.CertNo, y => y.MapFrom(u => u.NAME_SPELL))
            //      .ForMember(x => x.OfficeDepId, y => y.MapFrom(u => u.NAME_SPELL))
            //      .ForMember(x => x.UserInfoStatus, y => y.MapFrom(u => u.NAME_SPELL))
            //      .ForMember(x => x.Enable, y => y.MapFrom(u => u.NAME_SPELL))
            //      .ForMember(x => x.NameSpell, y => y.MapFrom(u => u.NAME_SPELL))
            //      .ForMember(x => x.EnName, y => y.MapFrom(u => u.EN_NAME))
            //      .ForMember(x => x.CertType, y => y.MapFrom(u => u.CERT_TYPE))
            //      .ForMember(x => x.Sex, y => y.MapFrom(u => u.SEX))
            //      .ForMember(x => x.Birthday, y => y.MapFrom(u => u.BIRTHDAY))
            //      .ForMember(x => x.Nationality, y => y.MapFrom(u => u.NATIONALITY))
            //      .ForMember(x => x.Nation, y => y.MapFrom(u => u.NATION))
            //      .ForMember(x => x.Positions, y => y.MapFrom(u => u.POSITIONS))
            //      .ForMember(x => x.PositionName, y => y.MapFrom(u => u.POSITION_NAME))
            //      .ForMember(x => x.PositionGrade, y => y.MapFrom(u => u.POSITION_GRADE))
            //      .ForMember(x => x.JobType, y => y.MapFrom(u => u.JOB_TYPE))
            //      .ForMember(x => x.JobName, y => y.MapFrom(u => u.JOB_NAME))
            //      .ForMember(x => x.Sno, y => y.MapFrom(u => u.SNO))
            //      .ForMember(x => x.SubDepts, y => y.MapFrom(u => u.SUB_DEPTS))
            //      .ForMember(x => x.EmpSort, y => y.MapFrom(u => u.EMP_SORT))
            //      .ForMember(x => x.UserLogin, y => y.MapFrom(u => u.USER_LOGIN))
            //      .ForMember(x => x.HrEmpCode, y => y.MapFrom(u => u.HR_EMP_CODE))
            //      .ForMember(x => x.EntryTime, y => y.MapFrom(u => u.ENTRY_TIME))
            //      .ForMember(x => x.Tel, y => y.MapFrom(u => u.TEL))
            //      .ForMember(x => x.Fax, y => y.MapFrom(u => u.FAX))
            //      .ForMember(x => x.OfficeNum, y => y.MapFrom(u => u.OFFICE_NUM))
            //      .ForMember(x => x.Attribute1, y => y.MapFrom(u => u.ATTRIBUTE1))
            //      .ForMember(x => x.Attribute2, y => y.MapFrom(u => u.ATTRIBUTE2))
            //      .ForMember(x => x.Attribute3, y => y.MapFrom(u => u.ATTRIBUTE3))
            //      .ForMember(x => x.PositionGradeNorm, y => y.MapFrom(u => u.POSITIONGRADENORM))
            //      .ForMember(x => x.HighEstGrade, y => y.MapFrom(u => u.HIGHESTGRADE))
            //      .ForMember(x => x.SameHighEstGrade, y => y.MapFrom(u => u.SAMEHIGHESTGRADE))
            //      .ForMember(x => x.PoliticsFace, y => y.MapFrom(u => u.POLITICSFACE))
            //      .ForMember(x => x.DispatchunitName, y => y.MapFrom(u => u.DISPATCHUNITNAME))
            //      .ForMember(x => x.DispatchunitShortName, y => y.MapFrom(u => u.DISPATCHUNITSHORTNAME))
            //      .ForMember(x => x.Externaluser, y => y.MapFrom(u => u.EXTERNALUSER))
            //      .ForMember(x => x.Attribute4, y => y.MapFrom(u => u.ATTRIBUTE4))
            //      .ForMember(x => x.Attribute5, y => y.MapFrom(u => u.ATTRIBUTE5));

            //mapperConfigurationExpression.CreateMap<Institution, InstitutionDetatilsDto>()
            //      .ForMember(x => x.BizType, y => y.MapFrom(u => u.BIZTYPE))
            //      .ForMember(x => x.OSecBid, y => y.MapFrom(u => u.O2BID))
            //      .ForMember(x => x.Gpoid, y => y.MapFrom(u => u.GPOID))
            //      .ForMember(x => x.Orule, y => y.MapFrom(u => u.ORULE))
            //      .ForMember(x => x.Type, y => y.MapFrom(u => u.TYPE))
            //      .ForMember(x => x.TypeExt, y => y.MapFrom(u => u.TYPEEXT))
            //      .ForMember(x => x.Mrut, y => y.MapFrom(u => u.MRUT))
            //      .ForMember(x => x.Sno, y => y.MapFrom(u => u.SNO))
            //      .ForMember(x => x.Coid, y => y.MapFrom(u => u.COID))
            //      .ForMember(x => x.Crossorgan, y => y.MapFrom(u => u.CROSSORGAN))
            //      .ForMember(x => x.Goid, y => y.MapFrom(u => u.GOID))
            //      .ForMember(x => x.Grade, y => y.MapFrom(u => u.GRADE))
            //      .ForMember(x => x.Oper, y => y.MapFrom(u => u.OPER))
            //      .ForMember(x => x.Note, y => y.MapFrom(u => u.NOTE))
            //      .ForMember(x => x.TemorganName, y => y.MapFrom(u => u.TEMORGANNAME))
            //      .ForMember(x => x.OrgProvince, y => y.MapFrom(u => u.ORGPROVINCE))
            //      .ForMember(x => x.Carea, y => y.MapFrom(u => u.CAREA))
            //      .ForMember(x => x.TerritoryPro, y => y.MapFrom(u => u.TERRITORYPRO))
            //      .ForMember(x => x.BizDomain, y => y.MapFrom(u => u.BIZDOMAIN))
            //      .ForMember(x => x.EntClass, y => y.MapFrom(u => u.ENTCLASS))
            //      .ForMember(x => x.OrgGrade, y => y.MapFrom(u => u.ORGGRADE))
            //      .ForMember(x => x.ProjectScale, y => y.MapFrom(u => u.PROJECTSCALE))
            //      .ForMember(x => x.ProjectManType, y => y.MapFrom(u => u.PROJECTMANTYPE))
            //      .ForMember(x => x.ProjectType, y => y.MapFrom(u => u.PROJECTTYPE))
            //      .ForMember(x => x.StartDate, y => y.MapFrom(u => u.STARTDATE))
            //      .ForMember(x => x.Organemp, y => y.MapFrom(u => u.ORGANEMP))
            //      .ForMember(x => x.OrganGrade, y => y.MapFrom(u => u.ORGANGRADE))
            //      .ForMember(x => x.Rown, y => y.MapFrom(u => u.ROWN))
            //      .ForMember(x => x.RoId, y => y.MapFrom(u => u.ROID))
            //      .ForMember(x => x.GlobalSno, y => y.MapFrom(u => u.GLOBAL_SNO))
            //      .ForMember(x => x.QyGrade, y => y.MapFrom(u => u.QYGRADE))
            //      .ForMember(x => x.RegisterCode, y => y.MapFrom(u => u.REGISTERCODE))
            //      .ForMember(x => x.Version, y => y.MapFrom(u => u.VERSION))
            //      .ForMember(x => x.ShareHoldings, y => y.MapFrom(u => u.SHAREHOLDINGS))
            //      .ForMember(x => x.IsIndependent, y => y.MapFrom(u => u.IS_INDEPENDENT))
            //      .ForMember(x => x.MdmCode, y => y.MapFrom(u => u.MDM_CODE));

            #endregion
        }
    }
}
