using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Application.Contracts.IService.ISearchService;
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
        /// 人员数据导出
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("ImportExecl")]
        public async Task<IActionResult> ImportExeclAsync([FromQuery] BaseRequestDto request)
        {
            if (request.IsFullExport)
            {
                request.PageSize = 1000000;
            }
            if (request.ImportType == 1)
            {
                var data = await searchService.GetUserSearchAsync(new FilterCondition() { ImportType = 1 });
                return await ExcelImportAsync(data.Data, null, "人员信息");
            }
            else if (request.ImportType == 2)
            {
                var data = await searchService.GetInstitutionAsync(new FilterCondition() { ImportType = 2 });
                return await ExcelImportAsync(data.Data, null, "机构信息");
            }
            else if (request.ImportType == 3)
            {
                var data = await searchService.GetProjectSearchAsync(new FilterCondition() { ImportType = 3 });
                return await ExcelImportAsync(data.Data, null, "项目信息");
            }
            else if (request.ImportType == 4)
            {
                var data = await searchService.GetCorresUnitSearchAsync(new FilterCondition() { ImportType = 4 });
                return await ExcelImportAsync(data.Data, null, "往来单位信息");
            }
            else if (request.ImportType == 5)
            {
                var data = await searchService.GetCountryRegionSearchAsync(new FilterCondition() { ImportType = 5 });
                return await ExcelImportAsync(data.Data, null, "国家地区信息");
            }
            else if (request.ImportType == 6)
            {
                var data = await searchService.GetCountryContinentSearchAsync(new FilterCondition() { ImportType = 6 });
                return await ExcelImportAsync(data.Data, null, "大洲信息");
            }
            else if (request.ImportType == 7)
            {
                var data = await searchService.GetFinancialInstitutionSearchAsync(new FilterCondition() { ImportType = 7 });
                return await ExcelImportAsync(data.Data, null, "金融机构信息");
            }
            else if (request.ImportType == 8)
            {
                var data = await searchService.GetDeviceClassCodeSearchAsync(new FilterCondition() { ImportType = 8 });
                return await ExcelImportAsync(data.Data, null, "物资设备分类编码信息");
            }
            else if (request.ImportType == 9)
            {
                var data = await searchService.GetInvoiceTypeSearchAsync(new FilterCondition() { ImportType = 9 });
                return await ExcelImportAsync(data.Data, null, "发票类型信息");
            }
            else if (request.ImportType == 10)
            {
                var data = await searchService.GetScientifiCNoProjectSearchAsync(new FilterCondition() { ImportType = 10 });
                return await ExcelImportAsync(data.Data, null, "科研项目信息");
            }
            else if (request.ImportType == 11)
            {
                var data = await searchService.GetLanguageSearchAsync(new FilterCondition() { ImportType = 11 });
                return await ExcelImportAsync(data.Data, null, "语言语种信息");
            }
            else if (request.ImportType == 12)
            {
                var data = await searchService.GetBankCardSearchAsync(new FilterCondition() { ImportType = 12 });
                return await ExcelImportAsync(data.Data, null, "银行账号信息");
            }
            else if (request.ImportType == 13)
            {
                var data = await searchService.GetDeviceDetailCodeSearchAsync(new FilterCondition() { ImportType = 13 });
                return await ExcelImportAsync(data.Data, null, "设备物资编码明细信息");
            }
            else if (request.ImportType == 14)
            {
                var data = await searchService.GetAccountingDepartmentSearchAsync(new FilterCondition() { ImportType = 14 });
                return await ExcelImportAsync(data.Data, null, "核算部门信息");
            }
            else if (request.ImportType == 15)
            {
                var data = await searchService.GetRelationalContractsSearchAsync(new FilterCondition() { ImportType = 15 });
                return await ExcelImportAsync(data.Data, null, "委托关系信息");
            }
            else if (request.ImportType == 16)
            {
                var data = await searchService.GetRegionalSearchAsync(new FilterCondition() { ImportType = 16 });
                return await ExcelImportAsync(data.Data, null, "中交区域总部信息");
            }
            else if (request.ImportType == 17)
            {
                var data = await searchService.GetUnitMeasurementSearchAsync(new FilterCondition() { ImportType = 17 });
                return await ExcelImportAsync(data.Data, null, "计量单位信息");
            }
            else if (request.ImportType == 18)
            {
                var data = await searchService.GetProjectClassificationSearchAsync(new FilterCondition() { ImportType = 18 });
                return await ExcelImportAsync(data.Data, null, "中交项目行业分类产业分类、业务板块、十二大业务类型、江河湖海对照关系信息");
            }
            else if (request.ImportType == 19)
            {
                var data = await searchService.GetRegionalCenterSearchAsync(new FilterCondition() { ImportType = 19 });
                return await ExcelImportAsync(data.Data, null, "中交区域中心信息");
            }
            else if (request.ImportType == 20)
            {
                var data = await searchService.GetNationalEconomySearchAsync(new FilterCondition() { ImportType = 20 });
                return await ExcelImportAsync(data.Data, null, "国民经济行业分类信息");
            }
            else if (request.ImportType == 21)
            {
                var data = await searchService.GetAdministrativeAccountingMapperSearchAsync(new FilterCondition() { ImportType = 21 });
                return await ExcelImportAsync(data.Data, null, "行政机构和核算机构映射关系信息");
            }
            else if (request.ImportType == 22)
            {
                var data = await searchService.GetEscrowOrganizationSearchAsync(new FilterCondition() { ImportType = 22 });
                return await ExcelImportAsync(data.Data, null, "多组织-税务代管组织(行政)信息");
            }
            else if (request.ImportType == 23)
            {
                var data = await searchService.GetBusinessNoCpportunitySearchAsync(new FilterCondition() { ImportType = 23 }, false);
                return await ExcelImportAsync(data.Data, null, "商机项目(不含境外商机项目)信息");
            }
            else if (request.ImportType == 24)
            {
                var data = await searchService.GetBusinessNoCpportunitySearchAsync(new FilterCondition() { ImportType = 24 }, true);
                return await ExcelImportAsync(data.Data, null, "商机项目(含境外商机项目)信息");
            }
            else if (request.ImportType == 25)
            {
                var data = await searchService.GetAdministrativeDivisionSearchAsync(new FilterCondition() { ImportType = 25 });
                return await ExcelImportAsync(data.Data, null, "境内行政区划信息");
            }
            else if (request.ImportType == 26)
            {
                var data = await searchService.GetAccountingOrganizationSearchAsync(new FilterCondition() { ImportType = 26 });
                return await ExcelImportAsync(data.Data, null, "多组织-核算机构信息");
            }
            else if (request.ImportType == 27)
            {
                var data = await searchService.GetCurrencySearchAsync(new FilterCondition() { ImportType = 27 });
                return await ExcelImportAsync(data.Data, null, "币种信息");
            }
            else if (request.ImportType == 28)
            {
                var data = await searchService.GetValueDomainReceiveAsync(new FilterCondition() { ImportType = 28 });
                return await ExcelImportAsync(data.Data, null, "值域信息");
            }

            return Ok("导出成功");
        }
        #endregion

    }
}
