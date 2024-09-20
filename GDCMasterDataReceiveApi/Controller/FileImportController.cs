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
        [HttpGet("ImportUserExecl")]
        public async Task<IActionResult> ImportUserExeclAsync([FromQuery] BaseRequestDto request)
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

            return Ok("导出成功");
        }
        #endregion


        /// <summary>
        /// 机构
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("ImportInstitutionExecl")]
        public async Task<IActionResult> ImportInstitutionExeclAsync([FromQuery] BaseRequestDto request)
        {
            if (request.IsFullExport)
            {
                request.PageSize = 1000000;
            }
            if (request.ImportType == 1)
            {
                var data = await searchService.GetInstitutionAsync(new FilterCondition() { ImportType = 1 });
                return await ExcelImportAsync(data.Data, null, "机构信息");
            }

            return Ok("导出成功");
        }
        /// <summary>
        /// 项目
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("ImportProjectExecl")]
        public async Task<IActionResult> ImportProjectExeclAsync([FromQuery] BaseRequestDto request)
        {
            if (request.IsFullExport)
            {
                request.PageSize = 1000000;
            }
            if (request.ImportType == 1)
            {
                var data = await searchService.GetProjectSearchAsync(new FilterCondition() { ImportType = 1 });
                return await ExcelImportAsync(data.Data, null, "项目信息");
            }

            return Ok("导出成功");
        }
        /// <summary>
        /// 往来单位
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("ImportCorresUnitExecl")]
        public async Task<IActionResult> ImportCorresUnitExeclAsync([FromQuery] BaseRequestDto request)
        {
            if (request.IsFullExport)
            {
                request.PageSize = 1000000;
            }
            if (request.ImportType == 1)
            {
                var data = await searchService.GetCorresUnitSearchAsync(new FilterCondition() { ImportType = 1 });
                return await ExcelImportAsync(data.Data, null, "往来单位信息");
            }

            return Ok("导出成功");
        }
        /// <summary>
        /// 国家地区
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("ImportCountryRegionExecl")]
        public async Task<IActionResult> ImportCountryRegionExeclAsync([FromQuery] BaseRequestDto request)
        {
            if (request.IsFullExport)
            {
                request.PageSize = 1000000;
            }
            if (request.ImportType == 1)
            {
                var data = await searchService.GetCountryRegionSearchAsync(new FilterCondition() { ImportType = 1 });
                return await ExcelImportAsync(data.Data, null, "国家地区信息");
            }

            return Ok("导出成功");
        }
        /// <summary>
        /// 大洲
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("ImportCountryContinentExecl")]
        public async Task<IActionResult> ImportCountryContinentExeclAsync([FromQuery] BaseRequestDto request)
        {
            if (request.IsFullExport)
            {
                request.PageSize = 1000000;
            }
            if (request.ImportType == 1)
            {
                var data = await searchService.GetCountryContinentSearchAsync(new FilterCondition() { ImportType = 1 });
                return await ExcelImportAsync(data.Data, null, "大洲信息");
            }

            return Ok("导出成功");
        }
        /// <summary>
        /// 金融机构
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("ImportFinancialExecl")]
        public async Task<IActionResult> ImportFinancialExeclAsync([FromQuery] BaseRequestDto request)
        {
            if (request.IsFullExport)
            {
                request.PageSize = 1000000;
            }
            if (request.ImportType == 1)
            {
                var data = await searchService.GetFinancialInstitutionSearchAsync(new FilterCondition() { ImportType = 1 });
                return await ExcelImportAsync(data.Data, null, "金融机构信息");
            }

            return Ok("导出成功");
        }
        /// <summary>
        /// 物资设备分类编码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("ImportDeviceClassCodeExecl")]
        public async Task<IActionResult> ImportDeviceClassCodeExeclAsync([FromQuery] BaseRequestDto request)
        {
            if (request.IsFullExport)
            {
                request.PageSize = 1000000;
            }
            if (request.ImportType == 1)
            {
                var data = await searchService.GetDeviceClassCodeSearchAsync(new FilterCondition() { ImportType = 1 });
                return await ExcelImportAsync(data.Data, null, "物资设备分类编码信息");
            }

            return Ok("导出成功");
        }
        /// <summary>
        /// 发票类型
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("ImportInvoiceTypeExecl")]
        public async Task<IActionResult> ImportInvoiceTypeExeclAsync([FromQuery] BaseRequestDto request)
        {
            if (request.IsFullExport)
            {
                request.PageSize = 1000000;
            }
            if (request.ImportType == 1)
            {
                var data = await searchService.GetInvoiceTypeSearchAsync(new FilterCondition() { ImportType = 1 });
                return await ExcelImportAsync(data.Data, null, "发票类型信息");
            }

            return Ok("导出成功");
        }
        /// <summary>
        /// 科研项目
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("ImportScientifiCNoProjectExecl")]
        public async Task<IActionResult> ImportScientifiCNoProjectExeclAsync([FromQuery] BaseRequestDto request)
        {
            if (request.IsFullExport)
            {
                request.PageSize = 1000000;
            }
            if (request.ImportType == 1)
            {
                var data = await searchService.GetScientifiCNoProjectSearchAsync(new FilterCondition() { ImportType = 1 });
                return await ExcelImportAsync(data.Data, null, "科研项目信息");
            }

            return Ok("导出成功");
        }
        /// <summary>
        /// 语言语种
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("ImportLanguageExecl")]
        public async Task<IActionResult> ImportLanguageExeclAsync([FromQuery] BaseRequestDto request)
        {
            if (request.IsFullExport)
            {
                request.PageSize = 1000000;
            }
            if (request.ImportType == 1)
            {
                var data = await searchService.GetLanguageSearchAsync(new FilterCondition() { ImportType = 1 });
                return await ExcelImportAsync(data.Data, null, "语言语种信息");
            }

            return Ok("导出成功");
        }
        /// <summary>
        /// 银行账号
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("ImportBankCardExecl")]
        public async Task<IActionResult> ImportBankCardExeclAsync([FromQuery] BaseRequestDto request)
        {
            if (request.IsFullExport)
            {
                request.PageSize = 1000000;
            }
            if (request.ImportType == 1)
            {
                var data = await searchService.GetBankCardSearchAsync(new FilterCondition() { ImportType = 1 });
                return await ExcelImportAsync(data.Data, null, "银行账号信息");
            }

            return Ok("导出成功");
        }
        /// <summary>
        /// 备物资编码明细
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("ImportDeviceDetailCodeExecl")]
        public async Task<IActionResult> ImportDeviceDetailCodeExeclAsync([FromQuery] BaseRequestDto request)
        {
            if (request.IsFullExport)
            {
                request.PageSize = 1000000;
            }
            if (request.ImportType == 1)
            {
                var data = await searchService.GetDeviceDetailCodeSearchAsync(new FilterCondition() { ImportType = 1 });
                return await ExcelImportAsync(data.Data, null, "设备物资编码明细信息");
            }

            return Ok("导出成功");
        }
        /// <summary>
        /// 核算部门
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("ImportAccountingDepartmentExecl")]
        public async Task<IActionResult> ImportAccountingDepartmentExeclAsync([FromQuery] BaseRequestDto request)
        {
            if (request.IsFullExport)
            {
                request.PageSize = 1000000;
            }
            if (request.ImportType == 1)
            {
                var data = await searchService.GetAccountingDepartmentSearchAsync(new FilterCondition() { ImportType = 1 });
                return await ExcelImportAsync(data.Data, null, "核算部门信息");
            }

            return Ok("导出成功");
        }
        /// <summary>
        /// 委托关系
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("ImportRelationalContractExecl")]
        public async Task<IActionResult> ImportRelationalContractExeclAsync([FromQuery] BaseRequestDto request)
        {
            if (request.IsFullExport)
            {
                request.PageSize = 1000000;
            }
            if (request.ImportType == 1)
            {
                var data = await searchService.GetRelationalContractsSearchAsync(new FilterCondition() { ImportType = 1 });
                return await ExcelImportAsync(data.Data, null, "委托关系信息");
            }

            return Ok("导出成功");
        }
        /// <summary>
        /// 中交区域总部
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("ImportRegionalExecl")]
        public async Task<IActionResult> ImportRegionalExeclAsync([FromQuery] BaseRequestDto request)
        {
            if (request.IsFullExport)
            {
                request.PageSize = 1000000;
            }
            if (request.ImportType == 1)
            {
                var data = await searchService.GetRegionalSearchAsync(new FilterCondition() { ImportType = 1 });
                return await ExcelImportAsync(data.Data, null, "中交区域总部信息");
            }

            return Ok("导出成功");
        }
        /// <summary>
        /// 计量单位
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("ImportUnitMeasurementExecl")]
        public async Task<IActionResult> ImportUnitMeasurementExeclAsync([FromQuery] BaseRequestDto request)
        {
            if (request.IsFullExport)
            {
                request.PageSize = 1000000;
            }
            if (request.ImportType == 1)
            {
                var data = await searchService.GetUnitMeasurementSearchAsync(new FilterCondition() { ImportType = 1 });
                return await ExcelImportAsync(data.Data, null, "计量单位信息");
            }

            return Ok("导出成功");
        }
        /// <summary>
        /// 中交项目行业分类产业分类、业务板块、十二大业务类型、江河湖海对照关系
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("ImportProjectClassificationExecl")]
        public async Task<IActionResult> ImportProjectClassificationExeclAsync([FromQuery] BaseRequestDto request)
        {
            if (request.IsFullExport)
            {
                request.PageSize = 1000000;
            }
            if (request.ImportType == 1)
            {
                var data = await searchService.GetProjectClassificationSearchAsync(new FilterCondition() { ImportType = 1 });
                return await ExcelImportAsync(data.Data, null, "中交项目行业分类产业分类、业务板块、十二大业务类型、江河湖海对照关系信息");
            }

            return Ok("导出成功");
        }
        /// <summary>
        /// 中交区域中心
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("ImportRegionalCenterExecl")]
        public async Task<IActionResult> ImportRegionalCenterExeclAsync([FromQuery] BaseRequestDto request)
        {
            if (request.IsFullExport)
            {
                request.PageSize = 1000000;
            }
            if (request.ImportType == 1)
            {
                var data = await searchService.GetRegionalCenterSearchAsync(new FilterCondition() { ImportType = 1 });
                return await ExcelImportAsync(data.Data, null, "中交区域中心信息");
            }

            return Ok("导出成功");
        }
        /// <summary>
        /// 国民经济行业分类
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("ImportNationalEconomyExecl")]
        public async Task<IActionResult> ImportNationalEconomyExeclAsync([FromQuery] BaseRequestDto request)
        {
            if (request.IsFullExport)
            {
                request.PageSize = 1000000;
            }
            if (request.ImportType == 1)
            {
                var data = await searchService.GetNationalEconomySearchAsync(new FilterCondition() { ImportType = 1 });
                return await ExcelImportAsync(data.Data, null, "国民经济行业分类信息");
            }

            return Ok("导出成功");
        }
        /// <summary>
        /// 行政机构和核算机构映射关系
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("ImportAdministrativeAccountingMapperExecl")]
        public async Task<IActionResult> ImportAdministrativeAccountingMapperExeclAsync([FromQuery] BaseRequestDto request)
        {
            if (request.IsFullExport)
            {
                request.PageSize = 1000000;
            }
            if (request.ImportType == 1)
            {
                var data = await searchService.GetAdministrativeAccountingMapperSearchAsync(new FilterCondition() { ImportType = 1 });
                return await ExcelImportAsync(data.Data, null, "行政机构和核算机构映射关系信息");
            }

            return Ok("导出成功");
        }
        /// <summary>
        /// 多组织-税务代管组织(行政)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("ImportEscrowOrganizationExecl")]
        public async Task<IActionResult> ImportEscrowOrganizationExeclAsync([FromQuery] BaseRequestDto request)
        {
            if (request.IsFullExport)
            {
                request.PageSize = 1000000;
            }
            if (request.ImportType == 1)
            {
                var data = await searchService.GetEscrowOrganizationSearchAsync(new FilterCondition() { ImportType = 1 });
                return await ExcelImportAsync(data.Data, null, "多组织-税务代管组织(行政)信息");
            }

            return Ok("导出成功");
        }
        /// <summary>
        /// 商机项目(不含境外商机项目)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("ImportBusinessNoCpportunityExecl")]
        public async Task<IActionResult> ImportBusinessNoCpportunityExeclAsync([FromQuery] BaseRequestDto request)
        {
            if (request.IsFullExport)
            {
                request.PageSize = 1000000;
            }
            if (request.ImportType == 1)
            {
                var data = await searchService.GetBusinessNoCpportunitySearchAsync(new FilterCondition() { ImportType = 1 }, false);
                return await ExcelImportAsync(data.Data, null, "商机项目(不含境外商机项目)信息");
            }

            return Ok("导出成功");
        }
        /// <summary>
        /// 商机项目(含境外商机项目)
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
                var data = await searchService.GetBusinessNoCpportunitySearchAsync(new FilterCondition() { ImportType = 1 }, true);
                return await ExcelImportAsync(data.Data, null, "商机项目(含境外商机项目)信息");
            }

            return Ok("导出成功");
        }
        /// <summary>
        /// 境内行政区划
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("ImportAdministrativeDivisionExecl")]
        public async Task<IActionResult> ImportAdministrativeDivisionExeclAsync([FromQuery] BaseRequestDto request)
        {
            if (request.IsFullExport)
            {
                request.PageSize = 1000000;
            }
            if (request.ImportType == 1)
            {
                var data = await searchService.GetAdministrativeDivisionSearchAsync(new FilterCondition() { ImportType = 1 });
                return await ExcelImportAsync(data.Data, null, "境内行政区划信息");
            }

            return Ok("导出成功");
        }
        /// <summary>
        /// 多组织-核算机构
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("ImportAccountingOrganizationExecl")]
        public async Task<IActionResult> ImportAccountingOrganizationExeclAsync([FromQuery] BaseRequestDto request)
        {
            if (request.IsFullExport)
            {
                request.PageSize = 1000000;
            }
            if (request.ImportType == 1)
            {
                var data = await searchService.GetAccountingOrganizationSearchAsync(new FilterCondition() { ImportType = 1 });
                return await ExcelImportAsync(data.Data, null, "多组织-核算机构信息");
            }

            return Ok("导出成功");
        }
        /// <summary>
        /// 币种
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("ImportCurrencyExecl")]
        public async Task<IActionResult> ImportCurrencyExeclAsync([FromQuery] BaseRequestDto request)
        {
            if (request.IsFullExport)
            {
                request.PageSize = 1000000;
            }
            if (request.ImportType == 1)
            {
                var data = await searchService.GetCurrencySearchAsync(new FilterCondition() { ImportType = 1 });
                return await ExcelImportAsync(data.Data, null, "币种信息");
            }

            return Ok("导出成功");
        }
        /// <summary>
        /// 值域
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("ImportValueDomainExecl")]
        public async Task<IActionResult> ImportValueDomainExeclAsync([FromQuery] BaseRequestDto request)
        {
            if (request.IsFullExport)
            {
                request.PageSize = 1000000;
            }
            if (request.ImportType == 1)
            {
                var data = await searchService.GetValueDomainReceiveAsync(new FilterCondition() { ImportType = 1 });
                return await ExcelImportAsync(data.Data, null, "值域信息");
            }

            return Ok("导出成功");
        }
    }
}
