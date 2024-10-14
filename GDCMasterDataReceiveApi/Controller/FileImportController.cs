using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Application.Contracts.IService.ISearchService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GDCMasterDataReceiveApi.Controller
{


    /// <summary>
    /// 文件导出控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FileImportController : BaseController
    {
        #region 依赖注入
        private readonly ISearchService searchService;
        public FileImportController(ISearchService searchService)
        {
            this.searchService = searchService;
        }
        #endregion

        #region 人员数据导出
        /// <summary>
        /// 业务导出
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("ImportExecl")]
        public async Task<IActionResult> ImportExeclAsync([FromQuery] BaseRequestDto request)
        {
            if (request.ImportType == 1)
            {
                var data = await searchService.GetUserSearchAsync(new FilterCondition() { PageSize = 1000000, IsFullExport = true, ImportType = 1 });
                if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "人员信息"); else return Ok("无数据");
            }
            else if (request.ImportType == 2)
            {
                var data = await searchService.GetInstitutionAsync(new FilterCondition() { PageSize = 1000000, IsFullExport = true, ImportType = 2 });
                if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "机构信息"); else return Ok("无数据");
            }
            else if (request.ImportType == 3)
            {
                var data = await searchService.GetProjectSearchAsync(new FilterCondition() { PageSize = 1000000, IsFullExport = true, ImportType = 3 });
                if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "项目信息"); else return Ok("无数据");
            }
            else if (request.ImportType == 4)
            {
                var data = await searchService.GetCorresUnitSearchAsync(new FilterCondition() { PageSize = 1000000, IsFullExport = true, ImportType = 4 });
                if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "往来单位信息"); else return Ok("无数据");
            }
            else if (request.ImportType == 5)
            {
                var data = await searchService.GetCountryRegionSearchAsync(new FilterCondition() { PageSize = 1000000, IsFullExport = true, ImportType = 5 });
                if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "国家地区信息"); else return Ok("无数据");
            }
            else if (request.ImportType == 6)
            {
                var data = await searchService.GetCountryContinentSearchAsync(new FilterCondition() { PageSize = 1000000, IsFullExport = true, ImportType = 6 });
                if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "大洲信息"); else return Ok("无数据");
            }
            else if (request.ImportType == 7)
            {
                var data = await searchService.GetFinancialInstitutionSearchAsync(new FilterCondition() { PageSize = 1000000, IsFullExport = true, ImportType = 7 });
                if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "金融机构信息"); else return Ok("无数据");
            }
            else if (request.ImportType == 8)
            {
                var data = await searchService.GetDeviceClassCodeSearchAsync(new FilterCondition() { PageSize = 1000000, IsFullExport = true, ImportType = 8 });
                if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "物资设备分类编码信息"); else return Ok("无数据");
            }
            else if (request.ImportType == 9)
            {
                var data = await searchService.GetInvoiceTypeSearchAsync(new FilterCondition() { PageSize = 1000000, IsFullExport = true, ImportType = 9 });
                if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "发票类型信息"); else return Ok("无数据");
            }
            else if (request.ImportType == 10)
            {
                var data = await searchService.GetScientifiCNoProjectSearchAsync(new FilterCondition() { PageSize = 1000000, IsFullExport = true, ImportType = 10 });
                if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "科研项目信息"); else return Ok("无数据");
            }
            else if (request.ImportType == 11)
            {
                var data = await searchService.GetLanguageSearchAsync(new FilterCondition() { PageSize = 1000000, IsFullExport = true, ImportType = 11 });
                if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "语言语种信息"); else return Ok("无数据");
            }
            else if (request.ImportType == 12)
            {
                var data = await searchService.GetBankCardSearchAsync(new FilterCondition() { PageSize = 1000000, IsFullExport = true, ImportType = 12 });
                if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "银行账号信息"); else return Ok("无数据");
            }
            else if (request.ImportType == 13)
            {
                var data = await searchService.GetDeviceDetailCodeSearchAsync(new FilterCondition() { PageSize = 1000000, IsFullExport = true, ImportType = 13 });
                if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "设备物资编码明细信息"); else return Ok("无数据");
            }
            else if (request.ImportType == 14)
            {
                var data = await searchService.GetAccountingDepartmentSearchAsync(new FilterCondition() { PageSize = 1000000, IsFullExport = true, ImportType = 14 });
                if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "核算部门信息"); else return Ok("无数据");
            }
            else if (request.ImportType == 15)
            {
                var data = await searchService.GetRelationalContractsSearchAsync(new FilterCondition() { PageSize = 1000000, IsFullExport = true, ImportType = 15 });
                if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "委托关系信息"); else return Ok("无数据");
            }
            else if (request.ImportType == 16)
            {
                var data = await searchService.GetRegionalSearchAsync(new FilterCondition() { PageSize = 1000000, IsFullExport = true, ImportType = 16 });
                if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "中交区域总部信息"); else return Ok("无数据");
            }
            else if (request.ImportType == 17)
            {
                var data = await searchService.GetUnitMeasurementSearchAsync(new FilterCondition() { PageSize = 1000000, IsFullExport = true, ImportType = 17 });
                if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "计量单位信息"); else return Ok("无数据");
            }
            else if (request.ImportType == 18)
            {
                var data = await searchService.GetProjectClassificationSearchAsync(new FilterCondition() { PageSize = 1000000, IsFullExport = true, ImportType = 18 });
                if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "中交项目行业分类产业分类、业务板块、十二大业务类型、江河湖海对照关系信息"); else return Ok("无数据");
            }
            else if (request.ImportType == 19)
            {
                var data = await searchService.GetRegionalCenterSearchAsync(new FilterCondition() { PageSize = 1000000, IsFullExport = true, ImportType = 19 });
                if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "中交区域中心信息"); else return Ok("无数据");
            }
            else if (request.ImportType == 20)
            {
                var data = await searchService.GetNationalEconomySearchAsync(new FilterCondition() { PageSize = 1000000, IsFullExport = true, ImportType = 20 });
                if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "国民经济行业分类信息"); else return Ok("无数据");
            }
            else if (request.ImportType == 21)
            {
                var data = await searchService.GetAdministrativeAccountingMapperSearchAsync(new FilterCondition() { PageSize = 1000000, IsFullExport = true, ImportType = 21 });
                if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "行政机构和核算机构映射关系信息"); else return Ok("无数据");
            }
            else if (request.ImportType == 22)
            {
                var data = await searchService.GetEscrowOrganizationSearchAsync(new FilterCondition() { PageSize = 1000000, IsFullExport = true, ImportType = 22 });
                if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "多组织-税务代管组织(行政)信息"); else return Ok("无数据");
            }
            else if (request.ImportType == 23)
            {
                var data = await searchService.GetBusinessNoCpportunitySearchAsync(new FilterCondition() { PageSize = 1000000, IsFullExport = true, ImportType = 23 }, false);
                if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "商机项目(不含境外商机项目)信息"); else return Ok("无数据");
            }
            else if (request.ImportType == 24)
            {
                var data = await searchService.GetBusinessNoCpportunitySearchAsync(new FilterCondition() { PageSize = 1000000, IsFullExport = true, ImportType = 24 }, true);
                if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "商机项目(含境外商机项目)信息"); else return Ok("无数据");
            }
            else if (request.ImportType == 25)
            {
                var data = await searchService.GetAdministrativeDivisionSearchAsync(new FilterCondition() { PageSize = 1000000, IsFullExport = true, ImportType = 25 });
                if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "境内行政区划信息"); else return Ok("无数据");
            }
            else if (request.ImportType == 26)
            {
                var data = await searchService.GetAccountingOrganizationSearchAsync(new FilterCondition() { PageSize = 1000000, IsFullExport = true, ImportType = 26 });
                if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "多组织-核算机构信息"); else return Ok("无数据");
            }
            else if (request.ImportType == 27)
            {
                var data = await searchService.GetCurrencySearchAsync(new FilterCondition() { PageSize = 1000000, IsFullExport = true, ImportType = 27 });
                if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "币种信息"); else return Ok("无数据");
            }
            else if (request.ImportType == 28)
            {
                var data = await searchService.GetValueDomainReceiveAsync(new FilterCondition() { PageSize = 1000000, IsFullExport = true, ImportType = 28 });
                if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "值域信息"); else return Ok("无数据");
            }
            else if (request.ImportType == 29)
            {
                var data = await searchService.GetDHVirtualProjectAsync(new FilterCondition() { PageSize = 1000000, IsFullExport = true, ImportType = 28 });
                if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "值域信息"); else return Ok("无数据");
            }
            else if (request.ImportType == 30)
            {
                var data = await searchService.GetEscrowOrganzationSearchAsync(new FilterCondition() { PageSize = 1000000, IsFullExport = true, ImportType = 28 });
                if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "值域信息"); else return Ok("无数据");
            }

            return Ok("导出成功");
        }
        #endregion

    }
}
