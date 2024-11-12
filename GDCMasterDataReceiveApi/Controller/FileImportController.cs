using GDCDataSecurityApi.Application.Contracts.Dto.IncrementalData;
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
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Language;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.NationalEconomy;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.ProjectClassification;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Regional;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.RegionalCenter;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.UnitMeasurement;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.ValueDomain;
using GDCMasterDataReceiveApi.Application.Contracts.IService.ISearchService;
using GDCMasterDataReceiveApi.Domain.Models;
using GDCMasterDataReceiveApi.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SqlSugar;
using System.ComponentModel;
using System.Reflection;
using UtilsSharp;
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
        //private List<string> GetFields<T>(List<string>? coulumns)
        //{
        //    var result = new List<string>();
        //    coulumns = coulumns.Select(x => x.ToLower()).ToList();
        //    var fields = new Dictionary<string, object>();

        //    var properties = typeof(T).GetProperties();
        //    foreach (var property in properties)
        //    {
        //        if (coulumns != null && coulumns.Any() && coulumns.Contains(property.Name.ToLower()))
        //        {
        //            result.Add(property.Name);
        //        }
        //    }

        //    return result;
        //}


        #region 数据导出
        /// <summary>
        /// 业务导出
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("ImportExecl")]
        [AllowAnonymous]
        public async Task<IActionResult> ImportExeclAsync([FromBody] BaseRequestDto request)
        {
            var systemInterfaceInfoApi = AppsettingsHelper.GetValue("API:SystemInterfaceFiledRuleApi");
            var appKey = GlobalCurrentUser.AppKey;
            var appinterfaceCode = GlobalCurrentUser.AppinterfaceCode;

            FilterCondition condition = new();
            condition.PageIndex = request.PageIndex;
            condition.PageSize = request.IsFullExport ? int.MaxValue : request.PageSize;
            condition.IsFullExport = request.IsFullExport;
            condition.ImportType = request.ImportType;

            Dictionary<string, object> parames = new Dictionary<string, object>();//请求参数
            WebHelper webHelper = new WebHelper();
            var interfaceInfo = AppsettingsHelper.GetValue("API:SystemInterfaceInfoApi");
            interfaceInfo = interfaceInfo.Replace("$systemApi", appKey).Replace("$interfaceApi", appinterfaceCode);
            var responseInterfaceInfo = await webHelper.DoGetAsync<ResponseAjaxResult<List<DataInterfaceResponseDto>>>(interfaceInfo);
            parames.Add("InterfaceApiId", responseInterfaceInfo.Result.Data[0].Id);
            parames.Add("AppSystemId", responseInterfaceInfo.Result.Data[0].AppSystemId);

            string expName = string.Empty;
            if (request.ImportType == 1)
            {
                var responseResult = await searchService.GetUserSearchAsync(condition);
                parames.Add("JsonObj", responseResult.ToJson(true));
            }
            else if (request.ImportType == 2)
            {
                expName = "机构";
                var responseResult = await searchService.GetInstitutionTreeDetailsAsync(condition);
                parames.Add("JsonObj", responseResult.ToJson(true));
            }
            else if (request.ImportType == 3)
            {
                expName = "项目";
                var responseResult = await searchService.GetProjectSearchAsync(condition);
                parames.Add("JsonObj", responseResult.ToJson(true));
            }
            else if (request.ImportType == 4)
            {
                expName = "往来单位";
                var responseResult = await searchService.GetCorresUnitSearchAsync(condition);
                parames.Add("JsonObj", responseResult.ToJson(true));
            }
            else if (request.ImportType == 5)
            {
                expName = "国家地区";
                var responseResult = await searchService.GetCountryRegionSearchAsync(condition);
                parames.Add("JsonObj", responseResult.ToJson(true));
            }
            else if (request.ImportType == 6)
            {
                expName = "大洲";
                var responseResult = await searchService.GetCountryContinentSearchAsync(condition);
                parames.Add("JsonObj", responseResult.ToJson(true));
            }
            else if (request.ImportType == 7)
            {
                expName = "金融机构";
                var responseResult = await searchService.GetFinancialInstitutionSearchAsync(condition);
                parames.Add("JsonObj", responseResult.ToJson(true));
            }
            else if (request.ImportType == 8)
            {
                expName = "物资设备分类编码";
                var responseResult = await searchService.GetDeviceClassCodeSearchAsync(condition);
                parames.Add("JsonObj", responseResult.ToJson(true));
            }
            else if (request.ImportType == 9)
            {
                expName = "发票类型";
                var responseResult = await searchService.GetDeviceClassCodeSearchAsync(condition);
                parames.Add("JsonObj", responseResult.ToJson(true));
            }
            else if (request.ImportType == 10)
            {
                expName = "科研项目";
                var responseResult = await searchService.GetScientifiCNoProjectSearchAsync(condition);
                parames.Add("JsonObj", responseResult.ToJson(true));
            }
            else if (request.ImportType == 11)
            {
                expName = "语言语种";
                var responseResult = await searchService.GetLanguageSearchAsync(condition);
                parames.Add("JsonObj", responseResult.ToJson(true));
            }
            else if (request.ImportType == 12)
            {
                expName = "银行账号";
                var responseResult = await searchService.GetBankCardSearchAsync(condition);
                parames.Add("JsonObj", responseResult.ToJson(true));
            }
            else if (request.ImportType == 13)
            {
                expName = "设备物资编码明细";
                var responseResult = await searchService.GetDeviceDetailCodeSearchAsync(condition);
                parames.Add("JsonObj", responseResult.ToJson(true));
            }
            else if (request.ImportType == 14)
            {
                expName = "核算部门";
                var responseResult = await searchService.GetAccountingDepartmentSearchAsync(condition);
                parames.Add("JsonObj", responseResult.ToJson(true));
            }
            else if (request.ImportType == 15)
            {
                expName = "委托关系";
                var responseResult = await searchService.GetRelationalContractsSearchAsync(condition);
                parames.Add("JsonObj", responseResult.ToJson(true));
            }
            else if (request.ImportType == 16)
            {
                expName = "中交区域总部";
                var responseResult = await searchService.GetRegionalSearchAsync(condition);
                parames.Add("JsonObj", responseResult.ToJson(true));
            }
            else if (request.ImportType == 17)
            {
                expName = "计量单位";
                var responseResult = await searchService.GetUnitMeasurementSearchAsync(condition);
                parames.Add("JsonObj", responseResult.ToJson(true));
            }
            else if (request.ImportType == 18)
            {
                expName = "中交项目行业分类产业分类、业务板块、十二大业务类型、江河湖海对照关系";
                var responseResult = await searchService.GetProjectClassificationSearchAsync(condition);
                parames.Add("JsonObj", responseResult.ToJson(true));
            }
            else if (request.ImportType == 19)
            {
                expName = "中交区域中心";
                var responseResult = await searchService.GetRegionalCenterSearchAsync(condition);
                parames.Add("JsonObj", responseResult.ToJson(true));
            }
            else if (request.ImportType == 20)
            {
                expName = "国民经济行业分类";
                var responseResult = await searchService.GetNationalEconomySearchAsync(condition);
                parames.Add("JsonObj", responseResult.ToJson(true));
            }
            else if (request.ImportType == 21)
            {
                expName = "行政机构和核算机构映射关系";
                var responseResult = await searchService.GetAdministrativeAccountingMapperSearchAsync(condition);
                parames.Add("JsonObj", responseResult.ToJson(true));
            }
            else if (request.ImportType == 22)
            {
                expName = "多组织-税务代管组织(行政)";
                var responseResult = await searchService.GetEscrowOrganizationSearchAsync(condition);
                parames.Add("JsonObj", responseResult.ToJson(true));
            }
            else if (request.ImportType == 23)
            {
                expName = "商机项目(境内)";
                var responseResult = await searchService.GetBusinessNoCpportunitySearchAsync(condition, true);
                parames.Add("JsonObj", responseResult.ToJson(true));
            }
            else if (request.ImportType == 24)
            {
                expName = "商机项目(境外)";
                var responseResult = await searchService.GetBusinessNoCpportunitySearchAsync(condition, false);
                parames.Add("JsonObj", responseResult.ToJson(true));
            }
            else if (request.ImportType == 25)
            {
                expName = "境内行政区划";
                var responseResult = await searchService.GetAdministrativeDivisionSearchAsync(condition);
                parames.Add("JsonObj", responseResult.ToJson(true));
            }
            else if (request.ImportType == 26)
            {
                expName = "多组织-核算机构";
                var responseResult = await searchService.GetAccountingOrganizationSearchAsync(condition);
                parames.Add("JsonObj", responseResult.ToJson(true));
            }
            else if (request.ImportType == 27)
            {
                expName = "币种";
                var responseResult = await searchService.GetCurrencySearchAsync(condition);
                parames.Add("JsonObj", responseResult.ToJson(true));
            }
            else if (request.ImportType == 28)
            {
                expName = "值域";
                var responseResult = await searchService.GetValueDomainReceiveAsync(condition);
                parames.Add("JsonObj", responseResult.ToJson(true));
            }
            else if (request.ImportType == 29)
            {
                expName = "虚拟项目";
                var responseResult = await searchService.GetDHVirtualProjectAsync(condition);
                parames.Add("JsonObj", responseResult.ToJson(true));
            }
            else if (request.ImportType == 30)
            {
                expName = "多组织-行政组织";
                var responseResult = await searchService.GetXZOrganzationSearchAsync(condition);
                parames.Add("JsonObj", responseResult.ToJson(true));
            }
            else if (request.ImportType == 31)
            {
                expName = "生产经营管理组织";
                var responseResult = await searchService.GetDHMdmMultOrgAgencyRelPageAsync(condition);
                parames.Add("JsonObj", responseResult.ToJson(true));
            }
            List<string> ignoreColumns = new List<string>();
            if (request.IgoreColumns != null)
            {
                ignoreColumns = request.IgoreColumns.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            var interfaceInfoList = await webHelper.DoPostAsync(systemInterfaceInfoApi, parames);
            if (interfaceInfoList.Code == 200 && !string.IsNullOrWhiteSpace(interfaceInfoList.Result))
            {
                object data = null;

                switch (request.ImportType)
                {
                    case 1:
                        data = JsonConvert.DeserializeObject<ResponseAjaxResult<List<UserSearchDetailsDto>>>(interfaceInfoList.Result).Data;
                        break;
                    case 2:
                        data = JsonConvert.DeserializeObject<ResponseAjaxResult<List<InstitutionDetatilsDto>>>(interfaceInfoList.Result).Data;
                        break;
                    case 3:
                        data = JsonConvert.DeserializeObject<ResponseAjaxResult<List<DHProjects>>>(interfaceInfoList.Result).Data;
                        break;
                    case 4:
                        data = JsonConvert.DeserializeObject<ResponseAjaxResult<List<CorresUnitDetailsDto>>>(interfaceInfoList.Result).Data;
                        break;
                    case 5:
                        data = JsonConvert.DeserializeObject<ResponseAjaxResult<List<CountryRegionDetailsDto>>>(interfaceInfoList.Result).Data;
                        break;
                    case 6:
                        data = JsonConvert.DeserializeObject<ResponseAjaxResult<List<CountryContinentDetailsDto>>>(interfaceInfoList.Result).Data;
                        break;
                    case 7:
                        data = JsonConvert.DeserializeObject<ResponseAjaxResult<List<FinancialInstitutionDetailsDto>>>(interfaceInfoList.Result).Data;
                        break;
                    case 8:
                        data = JsonConvert.DeserializeObject<ResponseAjaxResult<List<DeviceClassCodeDetailsDto>>>(interfaceInfoList.Result).Data;
                        break;
                    case 9:
                        data = JsonConvert.DeserializeObject<ResponseAjaxResult<List<DeviceClassCodeDetailsDto>>>(interfaceInfoList.Result).Data;
                        break;
                    case 10:
                        data = JsonConvert.DeserializeObject<ResponseAjaxResult<List<DHResearchDto>>>(interfaceInfoList.Result).Data;
                        break;
                    case 11:
                        data = JsonConvert.DeserializeObject<ResponseAjaxResult<List<LanguageDetailsDto>>>(interfaceInfoList.Result).Data;
                        break;
                    case 12:
                        data = JsonConvert.DeserializeObject<ResponseAjaxResult<List<BankCardDetailsDto>>>(interfaceInfoList.Result).Data;
                        break;
                    case 13:
                        data = JsonConvert.DeserializeObject<ResponseAjaxResult<List<DeviceDetailCodeDetailsDto>>>(interfaceInfoList.Result).Data;
                        break;
                    case 14:
                        data = JsonConvert.DeserializeObject<ResponseAjaxResult<List<DHAccountingDept>>>(interfaceInfoList.Result).Data;
                        break;
                    case 15:
                        data = JsonConvert.DeserializeObject<ResponseAjaxResult<List<DHMdmMultOrgAgencyRelPage>>>(interfaceInfoList.Result).Data;
                        break;
                    case 16:
                        data = JsonConvert.DeserializeObject<ResponseAjaxResult<List<RegionalDetailsDto>>>(interfaceInfoList.Result).Data;
                        break;
                    case 17:
                        data = JsonConvert.DeserializeObject<ResponseAjaxResult<List<UnitMeasurementDetailsDto>>>(interfaceInfoList.Result).Data;
                        break;
                    case 18:
                        data = JsonConvert.DeserializeObject<ResponseAjaxResult<List<ProjectClassificationDetailsDto>>>(interfaceInfoList.Result).Data;
                        break;
                    case 19:
                        data = JsonConvert.DeserializeObject<ResponseAjaxResult<List<RegionalCenterDetailsDto>>>(interfaceInfoList.Result).Data;
                        break;
                    case 20:
                        data = JsonConvert.DeserializeObject<ResponseAjaxResult<List<NationalEconomyDetailsDto>>>(interfaceInfoList.Result).Data;
                        break;
                    case 21:
                        data = JsonConvert.DeserializeObject<ResponseAjaxResult<List<DHAdministrative>>>(interfaceInfoList.Result).Data;
                        break;
                    case 22:
                        data = JsonConvert.DeserializeObject<ResponseAjaxResult<List<EscrowOrganizationDetailsDto>>>(interfaceInfoList.Result).Data;
                        break;
                    case 23:
                        data = JsonConvert.DeserializeObject<ResponseAjaxResult<List<DHOpportunityDto>>>(interfaceInfoList.Result).Data;
                        break;
                    case 24:
                        data = JsonConvert.DeserializeObject<ResponseAjaxResult<List<DHOpportunityDto>>>(interfaceInfoList.Result).Data;
                        break;
                    case 25:
                        data = JsonConvert.DeserializeObject<ResponseAjaxResult<List<AdministrativeDivisionDetailsDto>>>(interfaceInfoList.Result).Data;
                        break;
                    case 26:
                        data = JsonConvert.DeserializeObject<ResponseAjaxResult<List<DHAdjustAccountsMultipleOrg>>>(interfaceInfoList.Result).Data;
                        break;
                    case 27:
                        data = JsonConvert.DeserializeObject<ResponseAjaxResult<List<CurrencyDetailsDto>>>(interfaceInfoList.Result).Data;
                        break;
                    case 28:
                        data = JsonConvert.DeserializeObject<ResponseAjaxResult<List<ValueDomainReceiveResponseDto>>>(interfaceInfoList.Result).Data;
                        break;
                    case 29:
                        data = JsonConvert.DeserializeObject<ResponseAjaxResult<List<DHVirtualProject>>>(interfaceInfoList.Result).Data;
                        break;
                    case 30:
                        data = JsonConvert.DeserializeObject<ResponseAjaxResult<List<DHOrganzationDep>>>(interfaceInfoList.Result).Data;
                        break;
                    case 31:
                        data = JsonConvert.DeserializeObject<ResponseAjaxResult<List<DHMdmManagementOrgage>>>(interfaceInfoList.Result).Data;
                        break;

                }

                // 在这里统一执行导入操作
                return await ExcelImportAsync(data, ignoreColumns, $"{expName}{DateTime.Now:yyyyMMdd}");
            }
            return Ok("");
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
    }
}
