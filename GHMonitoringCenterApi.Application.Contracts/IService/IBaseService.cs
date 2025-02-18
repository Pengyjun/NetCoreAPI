using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.ApprovalUser;
using GHMonitoringCenterApi.Application.Contracts.Dto.AttributeLabelc;
using GHMonitoringCenterApi.Application.Contracts.Dto.Common;
using GHMonitoringCenterApi.Application.Contracts.Dto.ConstructionQualification;
using GHMonitoringCenterApi.Application.Contracts.Dto.Currency;
using GHMonitoringCenterApi.Application.Contracts.Dto.DealingUnit;
using GHMonitoringCenterApi.Application.Contracts.Dto.DictionaryTable;
using GHMonitoringCenterApi.Application.Contracts.Dto.EquipmentManagement;
using GHMonitoringCenterApi.Application.Contracts.Dto.HelpCenter;
using GHMonitoringCenterApi.Application.Contracts.Dto.Information;
using GHMonitoringCenterApi.Application.Contracts.Dto.Institution;
using GHMonitoringCenterApi.Application.Contracts.Dto.Menu;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectArea;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectWBSUpload;
using GHMonitoringCenterApi.Application.Contracts.Dto.Province;
using GHMonitoringCenterApi.Application.Contracts.Dto.Ship;
using GHMonitoringCenterApi.Application.Contracts.Dto.Subsidiary;
using GHMonitoringCenterApi.Application.Contracts.Dto.Upload;
using GHMonitoringCenterApi.Application.Contracts.Dto.User;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.IService
{

    /// <summary>
    /// 基本接口层
    /// </summary>
    public interface IBaseService
    {

        /// <summary>
        /// 所属公司下拉列表接口
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchCompanyPullDownAsync(string oid, string keywords);
        /// <summary>
        /// 获取机构id
        /// </summary>
        /// <returns></returns>
        Task<List<Guid?>> GetCompanyIds();

        /// <summary>
        /// 下级公司下拉接口
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userList">是否是查询当前部门下面的用户使用 默认不是</param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchCompanySubPullDownAsync(Guid companyId, bool userList = false, bool department = false);

        /// <summary>
        /// 项目状态下拉接口
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchProjectStatusPullDownAsync();

        /// <summary>
        /// 项目区域下拉接口
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchProjectAreaPullDownAsync(ProjectAreaRequsetDto projectAreaRequsetDto);
        /// <summary>
        /// 项目类型下拉接口
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchProjectTypePullDownAsync(ProjectTypRequsetDto projectTypRequsetDto);
        /// <summary>
        /// 项目所在省份下拉接口
        /// </summary>
        /// <returns></returns>

        Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchProjectProvincePullDownAsync(ProvinceRequsetDto baseprovinceRequestDto);

        /// <summary>
        /// 获取机构组织树  懒加载模式
        /// </summary>
        /// <param name="poid"></param>
        /// <param name="defaultNode">默认根节点的父节点</param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<InstitutionTreeResponseDto>>> SearchInstitutionTreeAsync(string poid, string defaultNode = "101114066");

        /// <summary>
        /// 获取机构组织树  非懒加载模式
        /// </summary>
        /// <param name="defaultRootNode"></param>
        /// <param name="oid">可选参数 此字段为空查询全部  不为空查询当前部门下的所有</param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<InstitutionTreeResponseDto>>> SearchInstitutionNoLazyLoadingTreeAsync(string rootNode);
        /// <summary>
        /// 干系单位类型、人员职位类型下拉接口
        /// </summary>       
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchDictionaryTableTreeAsyncc(DictionaryTableRequestDto baseDictionaryTableRequestDto);

        /// <summary>
        /// 项目规模 下拉接口
        /// </summary>       
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchProjectScalePullDownAsync();
        /// <summary>
        /// 项目施工资质
        /// </summary>       
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchConstructionQualificationPullDownAsync(ConstructionQualificationRequsetDto qualificationRequsetDto);
        /// <summary>
        /// 行业分类标准
        /// </summary>       
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchIndustryClassificationPullDownAsync(IndustryClassificationRequsetDto currencyResponseDto);
        /// <summary>
        /// 水运工况级别
        /// </summary>       
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchWaterCarriagePullDownAsync();
        /// <summary>
        /// 属性标签
        /// </summary>       
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchAttributeLabelcPullDownAsync(AttributeLabelcResqusetDto attributeLabelcResqusetDto);
        /// <summary>
        /// 项目部
        /// </summary>       
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchProjectDepartmentPullDownAsync(ProjectDepartmentRuqusetDto projectDepartmentRuqusetDto);
        /// <summary>
        /// 币种
        /// </summary>       
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchCurrencyPullDownAsync();

        /// <summary>
        /// 获取职位类型的人员信息
        /// </summary>
        /// <param name="baseRequestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchPositionTypeUserDownAsync(PositionUserRequestDto positionUserRequestDto);

        /// <summary>
        /// 根据名称或手机号获取人员信息
        /// </summary>
        /// <param name="keyWords"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchUserDownAsync(BaseRequestDto baseRequestDto);

        /// <summary>
        /// 获取行业分类标准下拉树
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ClassifyStandardResponseDto>>> SerchClassifyTreeListAsync();
        /// <summary>
        /// 获取往来单位名称
        /// </summary>       
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchDealingUnitPullDownAsync(BaseRequestDto dealingUnitRequseDto);
        /// <summary>
        /// 获取当前公司的所有子公司
        /// </summary>       
        /// <returns></returns>
        Task<ResponseAjaxResult<List<SubordinateCompaniesDto>>> SearchSubsidiaryPullDownAsync(SubsidiaryRequsetDto subsidiaryRequsetDto);

        /// <summary>
        /// 境外区域地点模糊查询
        /// </summary>       
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ProjectAreaRegionResponseDto>>> SearchOverAreaRegionAsync(BaseKeyWordsRequestDto requestDto);

        /// <summary>
        /// 自有分包船舶模糊查询
        /// </summary>       
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchOwnerSubShipAsync(ShipRequestDto requestDto);


        /// <summary>
        /// 获取当前节点的所有上级
        /// </summary>
        /// <param name="grule"></param>
        /// <returns></returns>
        Task<InstitutionTreeResponseDto> GetCurrentAllPreNodeAsync(string grule, List<InstitutionTreeResponseDto> data);


        /// <summary>
        /// 项目列表导出字段查询
        /// </summary>       
        /// <returns></returns>
        ResponseAjaxResult<List<BasePullDownResponseDto>> SearchProjectImportColumns();

        /// <summary>
        /// 获取实体字段备注说明
        /// </summary>       
        /// <returns></returns>
        Task<List<EntityFieldRemark>> SearchEntityFieldRemarkAsync(string entityDtoName);
        /// <summary>
        /// 获取首页菜单列表
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<SearchHomePageMenuResponseDto>> SearchHomeMenuAsync(Guid? curLoginDepartmentId);

        /// <summary>
        /// 获取审批用户列表
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ApprovalUser>>> SearchApprovalUserAsync(Guid? Id, Guid ProjectId);
        /// <summary>
        ///  获取未填报日报 、 安监 、船舶 下拉集合
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> GetNotFillDayRepsSearch(CurrentUser currentUser, BaseKeyWordsRequestDto requestDto);
        /// <summary>
        /// 获取船舶名称
        /// </summary>
        /// <param name="baseRequestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchShipPingNameAsync(BaseRequestDto baseRequestDto);


        /// <summary>
        /// 获取当前机构的所属项目部以及所属公司
        /// </summary>
        /// <param name="oid">当前机构OID</param>
        /// <returns></returns>
        Task<InstitutionKeyValueResponseDto> GetCurrentInstitutionParent(string oid);

        /// <summary>
        /// 如果是广航局总部的机构提级为广航局的 返回广航局的机构
        /// </summary>
        /// <param name="institutionId"></param>
        /// <returns></returns>
        Task<Guid> IsGHJInstitution(Guid institutionId);

        /// <summary>
        /// 获取所属公司下拉接口
        /// </summary>
        /// <param name="institutionId"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchCompanyAsync(BaseKeyWordsRequestDto baseKeyWordsRequestDto, string? oid);
        /// <summary>
        /// 获取部门下项目信息下拉列表
        /// </summary>
        /// <param name="baseKeyWordsRequestDto"></param>
        /// <param name="oid"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ProjectInformationResponseDto>>> SearchProjectInformationAsync(BaseRequestDto baseRequestDto);
        /// <summary>
        /// 获取船舶港口数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> GetSearchShipPortAsync(BaseRequestDto requestDto);
        /// <summary>
        /// 获取施工性质模糊搜索
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> GetConstructionNatureAsync(BaseRequestDto requestDto);
        /// <summary>
        /// 产值日报自有分包船舶模糊查询
        /// </summary>       
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchLogOwnerSubShipAsync(ShipRequestDto requestDto);

        /// <summary>
        /// 疏浚吹填分类
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchShipWorkTypePullDownAsync();

        /// <summary>
        /// 工艺方式
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchShipWorkModePullDownAsync();

        /// <summary>
        /// 疏浚土分类
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchSoilPullDownAsync();

        /// <summary>
        /// 疏浚岩土分级
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchSoilGradePullDownAsync();

        /// <summary>
        /// 船级社
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchShipClassicPullDownAsync();
        /// <summary>
        /// 船舶类型
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchShipTypePullDownAsync();
        /// <summary>
        /// 船舶状态
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchShipStatusPullDownAsync();

        /// <summary>
        /// 获取项目组织结构
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ProjectWBSUpload>>> InsertProjectWBSAsync(ProjectWBSUploadRequestDto projectWBSUploadRequestDto);

        /// <summary>
        /// 获取预览和下载网址
        /// </summary>
        /// <param name="baseKeyWordsRequestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<SearchWebsiteResponseDto>> SearchWebsiteAsync(string website);
        /// <summary>
        /// 获取省份
        /// </summary>
        /// <param name="provinces"></param>
        /// <param name="pomId"></param>
        /// <returns></returns>
        Task<string> GetProvince(List<Province> provinces, Guid? pomId);
        /// <summary>
        /// 获取市
        /// </summary>
        /// <param name="provinces"></param>
        /// <param name="pomId"></param>
        /// <returns></returns>
        Task<string> GetProvincemarket(List<Province> provinces, Guid? pomId);
        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="getDeviceInformationResponseDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> GetDeviceInformationAsync(GetDeviceInformationRequesDto getDeviceInformationResponseDto);
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="getDeviceInformationResponseDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchUserInformationAsync(BaseRequestDto baseRequestDto);
        /// <summary>
        /// 获取首页菜单权限用户列表
        /// </summary>
        /// <param name="getDeviceInformationResponseDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<InformationResponseDto>>> GetHomeMenuPermissionUserAsync(BaseRequestDto baseRequestDto);

        /// <summary>
        /// 搜索所有项目供 分包项目选择时使用
        /// </summary>
        /// <param name="baseRequestDto"></param>
        /// <returns></returns>

        Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchProjectAsync(BaseRequestDto baseRequestDto);

        /// <summary>
        /// 记录每年的节假日日期  每年写一次
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>

        Task<ResponseAjaxResult<bool>> RecordHolidayAsync(int year);

        //Task UpdateProjectAsync();

        /// <summary>
        /// 管理类型
        /// </summary>
        /// <param name="projectTypRequsetDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchManagerTypeAsync(ProjectTypRequsetDto projectTypRequsetDto);

        /// <summary>
        /// 日报审批推送提醒
        /// </summary>
        /// <returns></returns>

        Task<bool> DayReportApprovePushAsync();


        /// <summary>
        /// 页面审核
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> DayReportApproveAsync(bool isApprove);
        Task<ResponseAjaxResult<string>> SearchDayReportApproveAsync();

    }
}