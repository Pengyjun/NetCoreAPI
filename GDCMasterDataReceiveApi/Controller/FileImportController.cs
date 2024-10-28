using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.User;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.AdministrativeDivision;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.BankCard;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CorresUnit;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CountryContinent;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CountryRegion;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Currency;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.DeviceClassCode;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.DeviceDetailCode;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.DHData;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.EscrowOrganization;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.FinancialInstitution;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Institution;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.InvoiceType;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Language;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.NationalEconomy;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.ProjectClassification;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Regional;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.RegionalCenter;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.UnitMeasurement;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.ValueDomain;
using GDCMasterDataReceiveApi.Application.Contracts.IService.ISearchService;
using GDCMasterDataReceiveApi.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
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
            condition.PageSize = 10000000;
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
                return File(excelData, contentType, $"用户_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            else if (request.ImportType == 2)
            {
                columns = await GetExpColumns("GetInstitutionAsync");
                fields = GetFields<InstitutionDetatilsDto>(columns);
                var data = await searchService.GetInstitutionAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "机构");
                return File(excelData, contentType, $"机构_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            else if (request.ImportType == 3)
            {
                columns = await GetExpColumns("GetProjectSearchAsync");
                fields = GetFields<DHProjects>(columns);
                var data = await searchService.GetProjectSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "项目");
                return File(excelData, contentType, $"项目_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            else if (request.ImportType == 4)
            {
                columns = await GetExpColumns("GetCorresUnitSearchAsync");
                fields = GetFields<CorresUnitDetailsDto>(columns);
                var data = await searchService.GetCorresUnitSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "往来单位");
                return File(excelData, contentType, $"往来单位_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            else if (request.ImportType == 5)
            {
                columns = await GetExpColumns("GetCountryRegionSearchAsync");
                fields = GetFields<CountryRegionDetailsDto>(columns);
                var data = await searchService.GetCountryRegionSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "国家地区");
                return File(excelData, contentType, $"国家地区_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            else if (request.ImportType == 6)
            {
                columns = await GetExpColumns("GetCountryContinentSearchAsync");
                fields = GetFields<CountryContinentDetailsDto>(columns);
                var data = await searchService.GetCountryContinentSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "大洲");
                return File(excelData, contentType, $"大洲_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            else if (request.ImportType == 7)
            {
                columns = await GetExpColumns("GetFinancialInstitutionSearchAsync");
                fields = GetFields<FinancialInstitutionDetailsDto>(columns);
                var data = await searchService.GetFinancialInstitutionSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "金融机构");
                return File(excelData, contentType, $"金融机构_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            else if (request.ImportType == 8)
            {
                columns = await GetExpColumns("GetDeviceClassCodeSearchAsync");
                fields = GetFields<DeviceClassCodeDetailsDto>(columns);
                var data = await searchService.GetDeviceClassCodeSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "物资设备分类编码");
                return File(excelData, contentType, $"物资设备分类编码_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            else if (request.ImportType == 9)
            {
                columns = await GetExpColumns("GetInvoiceTypeSearchAsync");
                fields = GetFields<InvoiceTypeDetailshDto>(columns);
                var data = await searchService.GetInvoiceTypeSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "发票类型");
                return File(excelData, contentType, $"发票类型_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            else if (request.ImportType == 10)
            {
                columns = await GetExpColumns("GetScientifiCNoProjectSearchAsync");
                fields = GetFields<DHResearchDto>(columns);
                var data = await searchService.GetScientifiCNoProjectSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "科研项目");
                return File(excelData, contentType, $"科研项目_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            else if (request.ImportType == 11)
            {
                columns = await GetExpColumns("GetLanguageSearchAsync");
                fields = GetFields<LanguageDetailsDto>(columns);
                var data = await searchService.GetLanguageSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "语言语种");
                return File(excelData, contentType, $"语言语种_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            else if (request.ImportType == 12)
            {
                columns = await GetExpColumns("GetBankCardSearchAsync");
                fields = GetFields<BankCardDetailsDto>(columns);
                var data = await searchService.GetBankCardSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "银行账号");
                return File(excelData, contentType, $"银行账号_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            else if (request.ImportType == 13)
            {
                columns = await GetExpColumns("GetDeviceDetailCodeSearchAsync");
                fields = GetFields<DeviceDetailCodeDetailsDto>(columns);
                var data = await searchService.GetDeviceDetailCodeSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "设备物资编码明细");
                return File(excelData, contentType, $"设备物资编码明细_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            else if (request.ImportType == 14)
            {
                columns = await GetExpColumns("GetAccountingDepartmentSearchAsync");
                fields = GetFields<DHAccountingDept>(columns);
                var data = await searchService.GetAccountingDepartmentSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "核算部门");
                return File(excelData, contentType, $"核算部门_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            else if (request.ImportType == 15)
            {
                columns = await GetExpColumns("GetRelationalContractsSearchAsync");
                fields = GetFields<DHMdmMultOrgAgencyRelPage>(columns);
                var data = await searchService.GetRelationalContractsSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "委托关系");
                return File(excelData, contentType, $"委托关系_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            else if (request.ImportType == 16)
            {
                columns = await GetExpColumns("GetRegionalSearchAsync");
                fields = GetFields<RegionalDetailsDto>(columns);
                var data = await searchService.GetRegionalSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "中交区域总部");
                return File(excelData, contentType, $"中交区域总部_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            else if (request.ImportType == 17)
            {
                columns = await GetExpColumns("GetUnitMeasurementSearchAsync");
                fields = GetFields<UnitMeasurementDetailsDto>(columns);
                var data = await searchService.GetUnitMeasurementSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "计量单位");
                return File(excelData, contentType, $"计量单位_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            else if (request.ImportType == 18)
            {
                columns = await GetExpColumns("GetProjectClassificationSearchAsync");
                fields = GetFields<ProjectClassificationDetailsDto>(columns);
                var data = await searchService.GetProjectClassificationSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "中交项目行业分类产业分类、业务板块、十二大业务类型、江河湖海对照关系");
                return File(excelData, contentType, $"中交项目行业分类产业分类、业务板块、十二大业务类型、江河湖海对照关系_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            else if (request.ImportType == 19)
            {
                columns = await GetExpColumns("GetRegionalCenterSearchAsync");
                fields = GetFields<RegionalCenterDetailsDto>(columns);
                var data = await searchService.GetRegionalCenterSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "中交区域中心");
                return File(excelData, contentType, $"中交区域中心_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            else if (request.ImportType == 20)
            {
                columns = await GetExpColumns("GetNationalEconomySearchAsync");
                fields = GetFields<NationalEconomyDetailsDto>(columns);
                var data = await searchService.GetNationalEconomySearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "国民经济行业分类");
                return File(excelData, contentType, $"国民经济行业分类_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            else if (request.ImportType == 21)
            {
                columns = await GetExpColumns("GetAdministrativeAccountingMapperSearchAsync");
                fields = GetFields<DHAdministrative>(columns);
                var data = await searchService.GetAdministrativeAccountingMapperSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "行政机构和核算机构映射关系");
                return File(excelData, contentType, $"行政机构和核算机构映射关系_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            else if (request.ImportType == 22)
            {
                columns = await GetExpColumns("GetEscrowOrganizationSearchAsync");
                fields = GetFields<EscrowOrganizationDetailsDto>(columns);
                var data = await searchService.GetEscrowOrganizationSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "多组织-税务代管组织(行政)");
                return File(excelData, contentType, $"多组织-税务代管组织(行政)_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            else if (request.ImportType == 23)
            {
                columns = await GetExpColumns("GetBusinessNoCpportunitySearchAsync");
                fields = GetFields<DHOpportunityDto>(columns);
                var data = await searchService.GetBusinessNoCpportunitySearchAsync(condition, true);
                var excelData = ExportToExcel(data.Data, fields, "商机项目(境内)");
                return File(excelData, contentType, $"商机项目(境内)_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            else if (request.ImportType == 24)
            {
                columns = await GetExpColumns("GetBusinessNoCpportunitySearchAsync");
                fields = GetFields<DHOpportunityDto>(columns);
                var data = await searchService.GetBusinessNoCpportunitySearchAsync(condition, false);
                var excelData = ExportToExcel(data.Data, fields, "商机项目(境外)");
                return File(excelData, contentType, $"商机项目(境外)_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            else if (request.ImportType == 25)
            {
                columns = await GetExpColumns("GetAdministrativeDivisionSearchAsync");
                fields = GetFields<AdministrativeDivisionDetailsDto>(columns);
                var data = await searchService.GetAdministrativeDivisionSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "境内行政区划");
                return File(excelData, contentType, $"境内行政区划_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            else if (request.ImportType == 26)
            {
                columns = await GetExpColumns("GetAccountingOrganizationSearchAsync");
                fields = GetFields<DHAdjustAccountsMultipleOrg>(columns);
                var data = await searchService.GetAccountingOrganizationSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "多组织-核算机构");
                return File(excelData, contentType, $"多组织-核算机构_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            else if (request.ImportType == 27)
            {
                columns = await GetExpColumns("GetCurrencySearchAsync");
                fields = GetFields<CurrencyDetailsDto>(columns);
                var data = await searchService.GetCurrencySearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "币种");
                return File(excelData, contentType, $"币种_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            else if (request.ImportType == 28)
            {
                columns = await GetExpColumns("GetValueDomainReceiveAsync");
                fields = GetFields<ValueDomainReceiveResponseDto>(columns);
                var data = await searchService.GetValueDomainReceiveAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "值域");
                return File(excelData, contentType, $"值域_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            else if (request.ImportType == 29)
            {
                columns = await GetExpColumns("GetDHVirtualProjectAsync");
                fields = GetFields<DHVirtualProject>(columns);
                var data = await searchService.GetDHVirtualProjectAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "虚拟项目");
                return File(excelData, contentType, $"虚拟项目_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            else if (request.ImportType == 30)
            {
                columns = await GetExpColumns("GetXZOrganzationSearchAsync");
                fields = GetFields<DHOrganzationDep>(columns);
                var data = await searchService.GetXZOrganzationSearchAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "多组织-行政组织");
                return File(excelData, contentType, $"多组织-行政组织_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            else if (request.ImportType == 31)
            {
                columns = await GetExpColumns("GetDHMdmMultOrgAgencyRelPageAsync");
                fields = GetFields<DHMdmManagementOrgage>(columns);
                var data = await searchService.GetDHMdmMultOrgAgencyRelPageAsync(condition);
                var excelData = ExportToExcel(data.Data, fields, "生产经营管理组织");
                return File(excelData, contentType, $"生产经营管理组织_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            else
            {
                return Ok("导出失败");
            }
        }
        #endregion
        private byte[] ExportToExcel<T>(List<T> data, List<string> columns, string? sheetName)
        {
            // 创建工作簿
            using var memoryStream = new MemoryStream();
            IWorkbook workbook = new XSSFWorkbook();
            ISheet worksheet = workbook.CreateSheet(sheetName);

            // 创建标题行
            var headerRow = worksheet.CreateRow(0);

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

                // 设置标题单元格
                var cell = headerRow.CreateCell(i);
                cell.SetCellValue(title);

                // 设置标题样式
                var headerCellStyle = workbook.CreateCellStyle();
                var headerFont = workbook.CreateFont();
                headerFont.FontHeightInPoints = 13;
                headerFont.FontName = "微软雅黑";
                headerCellStyle.SetFont(headerFont);
                cell.CellStyle = headerCellStyle;
            }

            // 添加数据行
            for (int rowIndex = 0; rowIndex < data.Count; rowIndex++)
            {
                var rowData = data[rowIndex];
                var dataRow = worksheet.CreateRow(rowIndex + 1);

                for (int colIndex = 0; colIndex < columns.Count; colIndex++)
                {
                    var columnName = columns[colIndex];
                    var value = GetValueByColumnName(rowData, columnName);
                    dataRow.CreateCell(colIndex).SetCellValue(value?.ToString());
                }
            }

            // 设置数据行样式
            var dataCellStyle = workbook.CreateCellStyle();
            dataCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            dataCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;

            for (int rowIndex = 1; rowIndex <= data.Count; rowIndex++)
            {
                for (int colIndex = 0; colIndex < columns.Count; colIndex++)
                {
                    worksheet.GetRow(rowIndex).GetCell(colIndex).CellStyle = dataCellStyle;
                }
            }

            // 写入到内存流并返回字节数组
            workbook.Write(memoryStream);
            return memoryStream.ToArray();
        }
        private object? GetValueByColumnName<T>(T rowData, string columnName)
        {
            var property = typeof(T).GetProperty(columnName, BindingFlags.Public | BindingFlags.Instance);
            return property?.GetValue(rowData, null);
        }
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
