using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Application.Contracts.IService.ISearchService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using SqlSugar;

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
        private readonly ISqlSugarClient _dbContext;
        public FileImportController(ISearchService searchService, ISqlSugarClient dbContext)
        {
            this.searchService = searchService;
            this._dbContext = dbContext;
        }
        #endregion
        /// <summary>
        /// 获取 DTO 字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dtos"></param>
        /// <param name="coulumns"></param>
        /// <returns></returns>
        private List<Dictionary<string, object>> GetFields<T>(List<T> dtos, List<string>? coulumns)
        {
            var result = new List<Dictionary<string, object>>();

            foreach (var dto in dtos)
            {
                var fields = new Dictionary<string, object>();
                var properties = typeof(T).GetProperties();

                foreach (var property in properties)
                {
                    var value = property.GetValue(dto);
                    if (coulumns != null && coulumns.Any() && coulumns.Contains(property.Name.ToLower()))
                    {
                        fields.Add(property.Name, value);
                    }
                }

                result.Add(fields);
            }

            return result;
        }
        #region 人员数据导出
        /// <summary>
        /// 业务导出
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("ImportExecl")]
        [AllowAnonymous]
        public async Task<IActionResult> ImportExeclAsync([FromQuery] BaseRequestDto request)
        {
            FilterCondition condition = new();
            condition.PageSize = 1000000;
            condition.IsFullExport = true;
            condition.ImportType = request.ImportType;


            //if (request.ImportType == 1)
            //{
                //获取需要返回的字段
                var columns = await GetExpColumns("GetUserSearchAsync");
                var data = await searchService.GetUserSearchAsync(condition);

                // 获取字段
                var fields = GetFields(data.Data, columns);

            // 设置流的位置为开始
            // 返回文件流
            return Ok("");
            //}
            //else if (request.ImportType == 2)
            //{
            //    var data = await searchService.GetInstitutionAsync(condition);
            //    if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "机构"); else return Ok("无数据");
            //}
            //else if (request.ImportType == 3)
            //{
            //    var data = await searchService.GetProjectSearchAsync(condition);
            //    if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "项目"); else return Ok("无数据");
            //}
            //else if (request.ImportType == 4)
            //{
            //    var data = await searchService.GetCorresUnitSearchAsync(condition);
            //    if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "往来单位"); else return Ok("无数据");
            //}
            //else if (request.ImportType == 5)
            //{
            //    var data = await searchService.GetCountryRegionSearchAsync(condition);
            //    if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "国家地区"); else return Ok("无数据");
            //}
            //else if (request.ImportType == 6)
            //{
            //    var data = await searchService.GetCountryContinentSearchAsync(condition);
            //    if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "大洲"); else return Ok("无数据");
            //}
            //else if (request.ImportType == 7)
            //{
            //    var data = await searchService.GetFinancialInstitutionSearchAsync(condition);
            //    if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "金融机构"); else return Ok("无数据");
            //}
            //else if (request.ImportType == 8)
            //{
            //    var data = await searchService.GetDeviceClassCodeSearchAsync(condition);
            //    if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "物资设备分类编码"); else return Ok("无数据");
            //}
            //else if (request.ImportType == 9)
            //{
            //    var data = await searchService.GetInvoiceTypeSearchAsync(condition);
            //    if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "发票类型"); else return Ok("无数据");
            //}
            //else if (request.ImportType == 10)
            //{
            //    var data = await searchService.GetScientifiCNoProjectSearchAsync(condition);
            //    if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "科研项目"); else return Ok("无数据");
            //}
            //else if (request.ImportType == 11)
            //{
            //    var data = await searchService.GetLanguageSearchAsync(condition);
            //    if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "语言语种"); else return Ok("无数据");
            //}
            //else if (request.ImportType == 12)
            //{
            //    var data = await searchService.GetBankCardSearchAsync(condition);
            //    if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "银行账号"); else return Ok("无数据");
            //}
            //else if (request.ImportType == 13)
            //{
            //    var data = await searchService.GetDeviceDetailCodeSearchAsync(condition);
            //    if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "设备物资编码明细"); else return Ok("无数据");
            //}
            //else if (request.ImportType == 14)
            //{
            //    var data = await searchService.GetAccountingDepartmentSearchAsync(condition);
            //    if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "核算部门"); else return Ok("无数据");
            //}
            //else if (request.ImportType == 15)
            //{
            //    var data = await searchService.GetRelationalContractsSearchAsync(condition);
            //    if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "委托关系"); else return Ok("无数据");
            //}
            //else if (request.ImportType == 16)
            //{
            //    var data = await searchService.GetRegionalSearchAsync(condition);
            //    if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "中交区域总部"); else return Ok("无数据");
            //}
            //else if (request.ImportType == 17)
            //{
            //    var data = await searchService.GetUnitMeasurementSearchAsync(condition);
            //    if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "计量单位"); else return Ok("无数据");
            //}
            //else if (request.ImportType == 18)
            //{
            //    var data = await searchService.GetProjectClassificationSearchAsync(condition);
            //    if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "中交项目行业分类产业分类、业务板块、十二大业务类型、江河湖海对照关系"); else return Ok("无数据");
            //}
            //else if (request.ImportType == 19)
            //{
            //    var data = await searchService.GetRegionalCenterSearchAsync(condition);
            //    if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "中交区域中心"); else return Ok("无数据");
            //}
            //else if (request.ImportType == 20)
            //{
            //    var data = await searchService.GetNationalEconomySearchAsync(condition);
            //    if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "国民经济行业分类"); else return Ok("无数据");
            //}
            //else if (request.ImportType == 21)
            //{
            //    var data = await searchService.GetAdministrativeAccountingMapperSearchAsync(condition);
            //    if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "行政机构和核算机构映射关系"); else return Ok("无数据");
            //}
            //else if (request.ImportType == 22)
            //{
            //    var data = await searchService.GetEscrowOrganizationSearchAsync(condition);
            //    if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "多组织-税务代管组织(行政)"); else return Ok("无数据");
            //}
            //else if (request.ImportType == 23)
            //{
            //    var data = await searchService.GetBusinessNoCpportunitySearchAsync(condition, false);
            //    if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "商机项目(不含境外商机项目)"); else return Ok("无数据");
            //}
            //else if (request.ImportType == 24)
            //{
            //    var data = await searchService.GetBusinessNoCpportunitySearchAsync(condition, true);
            //    if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "商机项目(含境外商机项目)"); else return Ok("无数据");
            //}
            //else if (request.ImportType == 25)
            //{
            //    var data = await searchService.GetAdministrativeDivisionSearchAsync(condition);
            //    if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "境内行政区划"); else return Ok("无数据");
            //}
            //else if (request.ImportType == 26)
            //{
            //    var data = await searchService.GetAccountingOrganizationSearchAsync(condition);
            //    if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "多组织-核算机构"); else return Ok("无数据");
            //}
            //else if (request.ImportType == 27)
            //{
            //    var data = await searchService.GetCurrencySearchAsync(condition);
            //    if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "币种"); else return Ok("无数据");
            //}
            //else if (request.ImportType == 28)
            //{
            //    var data = await searchService.GetValueDomainReceiveAsync(condition);
            //    if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "值域"); else return Ok("无数据");
            //}
            //else if (request.ImportType == 29)
            //{
            //    var data = await searchService.GetDHVirtualProjectAsync(condition);
            //    if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "虚拟项目"); else return Ok("无数据");
            //}
            //else if (request.ImportType == 30)
            //{
            //    var data = await searchService.GetXZOrganzationSearchAsync(condition);
            //    if (data.Data != null && data.Data.Any()) return await ExcelImportAsync(data.Data, null, "多组织-行政组织"); else return Ok("无数据");
            //}
        }
        #endregion
        /// <summary>
        /// 跨库读取导出字段列
        /// </summary>
        /// <param name="interfaceName"></param>
        /// <returns></returns>
        private async Task<List<string>> GetExpColumns(string? interfaceName)
        {
            List<string> columns = new();

            //跨库获取系统接口
            var interfaceFirst = await _dbContext.AsTenant().QueryableWithAttr<SystemInterface>()
                .Where(t => t.IsDelete == 1 && interfaceName == t.InterfaceName && t.Enable == 1)
                .FirstAsync();

            if (interfaceFirst != null)
            {
                //获取接口dto字段
                var interfaceData = await _dbContext.AsTenant().QueryableWithAttr<SystemInterfaceField>()
                    .Where(t => t.AppSystemInterfaceId == interfaceFirst.Id && t.Enable == 1)
                    .Select(t => new { t.FieidName, t.Id })
                    .ToListAsync();

                //获取当前接口已脱敏字段
                var interfaceIds = interfaceData.Select(x => x.Id).ToList();
                var desentIds = await _dbContext.AsTenant().QueryableWithAttr<DataDesensitizationRule>()
                    .Where(t => interfaceIds.Contains(t.AppSystemInterfaceFieIdId) && !string.IsNullOrWhiteSpace(t.StartIndex.ToString()) && !string.IsNullOrWhiteSpace(t.EndIndex.ToString()) && t.Enable == 1 && t.AppSystemApiId == 1844293323817357312)//广航局主数据
                    .Select(t => t.AppSystemInterfaceFieIdId)
                    .ToListAsync();

                //获取需要返回的字段
                columns = interfaceData.Where(t => !desentIds.Contains(t.Id)).Select(x => x.FieidName.ToLower()).ToList();
            }
            else { throw new Exception("接口不存在"); }

            return columns;
        }
    }
}
