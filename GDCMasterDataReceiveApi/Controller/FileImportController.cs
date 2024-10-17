using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.User;
using GDCMasterDataReceiveApi.Application.Contracts.IService.ISearchService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using SqlSugar;
using System.ComponentModel;
using System.Reflection;

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
        /// <param name="coulumns"></param>
        /// <returns></returns>
        private List<string> GetFields<T>(List<string>? coulumns)
        {
            var result = new List<string>();

            var fields = new Dictionary<string, object>();
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                if (coulumns != null && coulumns.Any() && coulumns.Contains(property.Name.ToLower()))
                {
                    result.Add(property.Name);
                }
            }

            return result;
        }
        #region 数据导出
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
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            List<string>? columns = new();//dto反射出的字段
            List<string>? fields = new();//需要导出的字段

            if (request.ImportType == 1)
            {
                columns = await GetExpColumns("GetUserSearchAsync");
                fields = GetFields<UserSearchDetailsDto>(columns);
                var data = await searchService.GetUserSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "用户");
                return File(excelData, contentType, "用户.xlsx");
            }
            else if (request.ImportType == 2)
            {
                columns = await GetExpColumns("GetInstitutionAsync");
                fields = GetFields<UserSearchDetailsDto>(columns);
                var data = await searchService.GetInstitutionAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "机构");
                return File(excelData, contentType, "机构.xlsx");
            }
            else if (request.ImportType == 3)
            {
                columns = await GetExpColumns("GetProjectSearchAsync");
                fields = GetFields<UserSearchDetailsDto>(columns);
                var data = await searchService.GetProjectSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "项目");
                return File(excelData, contentType, "项目.xlsx");
            }
            else if (request.ImportType == 4)
            {
                columns = await GetExpColumns("GetCorresUnitSearchAsync");
                fields = GetFields<UserSearchDetailsDto>(columns);
                var data = await searchService.GetCorresUnitSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "往来单位");
                return File(excelData, contentType, "往来单位.xlsx");
            }
            else if (request.ImportType == 5)
            {
                columns = await GetExpColumns("GetCountryRegionSearchAsync");
                fields = GetFields<UserSearchDetailsDto>(columns);
                var data = await searchService.GetCountryRegionSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "国家地区");
                return File(excelData, contentType, "国家地区.xlsx");
            }
            else if (request.ImportType == 6)
            {
                columns = await GetExpColumns("GetCountryContinentSearchAsync");
                fields = GetFields<UserSearchDetailsDto>(columns);
                var data = await searchService.GetCountryContinentSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "大洲");
                return File(excelData, contentType, "大洲.xlsx");
            }
            else if (request.ImportType == 7)
            {
                columns = await GetExpColumns("GetFinancialInstitutionSearchAsync");
                fields = GetFields<UserSearchDetailsDto>(columns);
                var data = await searchService.GetFinancialInstitutionSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "金融机构");
                return File(excelData, contentType, "金融机构.xlsx");
            }
            else if (request.ImportType == 8)
            {
                columns = await GetExpColumns("GetDeviceClassCodeSearchAsync");
                fields = GetFields<UserSearchDetailsDto>(columns);
                var data = await searchService.GetDeviceClassCodeSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "物资设备分类编码");
                return File(excelData, contentType, "物资设备分类编码.xlsx");
            }
            else if (request.ImportType == 9)
            {
                columns = await GetExpColumns("GetInvoiceTypeSearchAsync");
                fields = GetFields<UserSearchDetailsDto>(columns);
                var data = await searchService.GetInvoiceTypeSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "发票类型");
                return File(excelData, contentType, "发票类型.xlsx");
            }
            else if (request.ImportType == 10)
            {
                columns = await GetExpColumns("GetScientifiCNoProjectSearchAsync");
                fields = GetFields<UserSearchDetailsDto>(columns);
                var data = await searchService.GetScientifiCNoProjectSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "科研项目");
                return File(excelData, contentType, "科研项目.xlsx");
            }
            else if (request.ImportType == 11)
            {
                columns = await GetExpColumns("GetLanguageSearchAsync");
                fields = GetFields<UserSearchDetailsDto>(columns);
                var data = await searchService.GetLanguageSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "语言语种");
                return File(excelData, contentType, "语言语种.xlsx");
            }
            else if (request.ImportType == 12)
            {
                columns = await GetExpColumns("GetBankCardSearchAsync");
                fields = GetFields<UserSearchDetailsDto>(columns);
                var data = await searchService.GetBankCardSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "银行账号");
                return File(excelData, contentType, "银行账号.xlsx");
            }
            else if (request.ImportType == 13)
            {
                columns = await GetExpColumns("GetDeviceDetailCodeSearchAsync");
                fields = GetFields<UserSearchDetailsDto>(columns);
                var data = await searchService.GetDeviceDetailCodeSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "设备物资编码明细");
                return File(excelData, contentType, "设备物资编码明细.xlsx");
            }
            else if (request.ImportType == 14)
            {
                columns = await GetExpColumns("GetAccountingDepartmentSearchAsync");
                fields = GetFields<UserSearchDetailsDto>(columns);
                var data = await searchService.GetAccountingDepartmentSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "核算部门");
                return File(excelData, contentType, "核算部门.xlsx");
            }
            else if (request.ImportType == 15)
            {
                columns = await GetExpColumns("GetRelationalContractsSearchAsync");
                fields = GetFields<UserSearchDetailsDto>(columns);
                var data = await searchService.GetRelationalContractsSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "委托关系");
                return File(excelData, contentType, "委托关系.xlsx");
            }
            else if (request.ImportType == 16)
            {
                columns = await GetExpColumns("GetRegionalSearchAsync");
                fields = GetFields<UserSearchDetailsDto>(columns);
                var data = await searchService.GetRegionalSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "中交区域总部");
                return File(excelData, contentType, "中交区域总部.xlsx");
            }
            else if (request.ImportType == 17)
            {
                columns = await GetExpColumns("GetUnitMeasurementSearchAsync");
                fields = GetFields<UserSearchDetailsDto>(columns);
                var data = await searchService.GetUnitMeasurementSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "计量单位");
                return File(excelData, contentType, "计量单位.xlsx");
            }
            else if (request.ImportType == 18)
            {
                columns = await GetExpColumns("GetProjectClassificationSearchAsync");
                fields = GetFields<UserSearchDetailsDto>(columns);
                var data = await searchService.GetProjectClassificationSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "中交项目行业分类产业分类、业务板块、十二大业务类型、江河湖海对照关系");
                return File(excelData, contentType, "中交项目行业分类产业分类、业务板块、十二大业务类型、江河湖海对照关系.xlsx");
            }
            else if (request.ImportType == 19)
            {
                columns = await GetExpColumns("GetRegionalCenterSearchAsync");
                fields = GetFields<UserSearchDetailsDto>(columns);
                var data = await searchService.GetRegionalCenterSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "中交区域中心");
                return File(excelData, contentType, "中交区域中心.xlsx");
            }
            else if (request.ImportType == 20)
            {
                columns = await GetExpColumns("GetNationalEconomySearchAsync");
                fields = GetFields<UserSearchDetailsDto>(columns);
                var data = await searchService.GetNationalEconomySearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "国民经济行业分类");
                return File(excelData, contentType, "国民经济行业分类.xlsx");
            }
            else if (request.ImportType == 21)
            {
                columns = await GetExpColumns("GetAdministrativeAccountingMapperSearchAsync");
                fields = GetFields<UserSearchDetailsDto>(columns);
                var data = await searchService.GetAdministrativeAccountingMapperSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "行政机构和核算机构映射关系");
                return File(excelData, contentType, "行政机构和核算机构映射关系.xlsx");
            }
            else if (request.ImportType == 22)
            {
                columns = await GetExpColumns("GetEscrowOrganizationSearchAsync");
                fields = GetFields<UserSearchDetailsDto>(columns);
                var data = await searchService.GetEscrowOrganizationSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "多组织-税务代管组织(行政)");
                return File(excelData, contentType, "多组织-税务代管组织(行政).xlsx");
            }
            else if (request.ImportType == 23)
            {
                columns = await GetExpColumns("GetBusinessNoCpportunitySearchAsync");
                fields = GetFields<UserSearchDetailsDto>(columns);
                var data = await searchService.GetBusinessNoCpportunitySearchAsync(condition, false);
                var excelData = ExportToExcel(data.Data, fields, "商机项目(境内)");
                return File(excelData, contentType, "商机项目(境内).xlsx");
            }
            else if (request.ImportType == 24)
            {
                columns = await GetExpColumns("GetBusinessNoCpportunitySearchAsync");
                fields = GetFields<UserSearchDetailsDto>(columns);
                var data = await searchService.GetBusinessNoCpportunitySearchAsync(condition, true);
                var excelData = ExportToExcel(data.Data, fields, "商机项目(境外)");
                return File(excelData, contentType, "商机项目(境外).xlsx");
            }
            else if (request.ImportType == 25)
            {
                columns = await GetExpColumns("GetAdministrativeDivisionSearchAsync");
                fields = GetFields<UserSearchDetailsDto>(columns);
                var data = await searchService.GetAdministrativeDivisionSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "境内行政区划");
                return File(excelData, contentType, "境内行政区划.xlsx");
            }
            else if (request.ImportType == 26)
            {
                columns = await GetExpColumns("GetAccountingOrganizationSearchAsync");
                fields = GetFields<UserSearchDetailsDto>(columns);
                var data = await searchService.GetAccountingOrganizationSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "多组织-核算机构");
                return File(excelData, contentType, "多组织-核算机构.xlsx");
            }
            else if (request.ImportType == 27)
            {
                columns = await GetExpColumns("GetCurrencySearchAsync");
                fields = GetFields<UserSearchDetailsDto>(columns);
                var data = await searchService.GetCurrencySearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "币种");
                return File(excelData, contentType, "币种.xlsx");
            }
            else if (request.ImportType == 28)
            {
                columns = await GetExpColumns("GetValueDomainReceiveAsync");
                fields = GetFields<UserSearchDetailsDto>(columns);
                var data = await searchService.GetValueDomainReceiveAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "值域");
                return File(excelData, contentType, "值域.xlsx");
            }
            else if (request.ImportType == 29)
            {
                columns = await GetExpColumns("GetDHVirtualProjectAsync");
                fields = GetFields<UserSearchDetailsDto>(columns);
                var data = await searchService.GetDHVirtualProjectAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "虚拟项目");
                return File(excelData, contentType, "虚拟项目.xlsx");
            }
            else if (request.ImportType == 30)
            {
                columns = await GetExpColumns("GetXZOrganzationSearchAsync");
                fields = GetFields<UserSearchDetailsDto>(columns);
                var data = await searchService.GetXZOrganzationSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "多组织-行政组织");
                return File(excelData, contentType, "多组织-行政组织.xlsx");
            }
            else
            {
                return Ok("导出失败");
            }
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
        private byte[] ExportToExcel<T>(List<T> data, List<string> columns, string? sheetName)
        {
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add(sheetName);

            worksheet.Cells.Style.Font.Name = "微软雅黑";// 设置全局字体为微软雅黑

            // 添加标题行
            for (int i = 0; i < columns.Count; i++)
            {
                // 使用反射获取属性
                string columnName = columns[i];
                var property = typeof(T).GetProperty(columnName, BindingFlags.Public | BindingFlags.Instance);
                string title;

                // 获取 DisplayNameAttribute 中的中文标题
                if (property != null)
                {
                    var displayNameAttribute = property.GetCustomAttributes(typeof(DisplayNameAttribute), false)
                        .FirstOrDefault() as DisplayNameAttribute;

                    title = displayNameAttribute != null ? displayNameAttribute.DisplayName : columnName; // 如果没有对应的中文标题，使用属性名
                }
                else
                {
                    title = columnName; // 如果找不到属性，使用原列名
                }

                worksheet.Cells[1, i + 1].Value = title;

                // 设置标题行的样式
                worksheet.Cells[1, i + 1].Style.Font.Size = 13; // 字体放大
                worksheet.View.FreezePanes(2, 1); // 冻结标题行
            }

            // 添加数据行
            for (int rowIndex = 0; rowIndex < data.Count; rowIndex++)
            {
                var rowData = data[rowIndex];

                for (int colIndex = 0; colIndex < columns.Count; colIndex++)
                {
                    var columnName = columns[colIndex];
                    var value = GetValueByColumnName(rowData, columnName);
                    worksheet.Cells[rowIndex + 2, colIndex + 1].Value = value;
                }
            }

            // 设置数据行样式
            worksheet.Cells[2, 1, data.Count + 1, columns.Count].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[2, 1, data.Count + 1, columns.Count].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            // 返回 Excel 文件的字节数组
            return package.GetAsByteArray();
        }
        private object? GetValueByColumnName<T>(T rowData, string columnName)
        {
            var property = typeof(T).GetProperty(columnName, BindingFlags.Public | BindingFlags.Instance);
            return property?.GetValue(rowData, null);
        }
    }
}
