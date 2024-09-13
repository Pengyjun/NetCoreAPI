using Castle.DynamicProxy;
using GDCMasterDataReceiveApi.Application.Contracts;
using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.AccountingDepartment;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.AdministrativeOrganization;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CorresUnit;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CountryContinent;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CountryRegion;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Currency;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.DeviceClassCode;
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
using GDCMasterDataReceiveApi.Application.Contracts.Dto.UnitMeasurement;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.ValueDomain;
using GDCMasterDataReceiveApi.Domain.Models;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Enums;
using GDCMasterDataReceiveApi.Domain.Shared.Utils;
using UtilsSharp;

namespace GHElectronicFileApi.AopInterceptor
{

    /// <summary>
    /// 接收数据拦截器AOP
    /// </summary>
    public class ReceiveServiceInterceptor : IInterceptor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="invocation"></param>
        public async void Intercept(IInvocation invocation)
        {
            //请求唯一
            var traceIdentifier = HttpContentAccessFactory.Current.TraceIdentifier;
            //日志实例
            ReceiveRecordLog receiveRecordLog = null;
            //雪花ID
            var receiceRecordId = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
            //注入baseService  
            var baseService = (IBaseService)HttpContentAccessFactory.Current.RequestServices.GetService<IBaseService>();
            //接收参数
            var requestParame = string.Empty;
            //参数数量
            var parameCount = 0;
            //请求的方法名称
            var methodName = invocation.Method.Name;
            //接收数据类型
            ReceiveDataType receiveDataType = 0;

            #region 接收类型数据判断
            if (methodName == "ProjectDataAsync")
            {
                var receiveParame = ((BaseReceiveDataRequestDto<ProjectItem>)invocation.Arguments[0]).IT_DATA;
                parameCount = receiveParame.item.Count;
                requestParame = receiveParame.item.ToJson();
                receiveDataType = ReceiveDataType.Project;
            }
            else if (methodName == "CountryContinentDataAsync")
            {
                var receiveParame = ((BaseReceiveDataRequestDto<CountryContinentReceiveDto>)invocation.Arguments[0]).IT_DATA;
                parameCount = receiveParame.item.Count;
                requestParame = receiveParame.item.ToJson();
                receiveDataType = ReceiveDataType.CountryContinent;
            }
            else if (methodName == "CountryRegionDataAsync")
            {
                var receiveParame = ((BaseReceiveDataRequestDto<CountryRegionReceiveDto>)invocation.Arguments[0]).IT_DATA;
                parameCount = receiveParame.item.Count;
                requestParame = receiveParame.item.ToJson();
                receiveDataType = ReceiveDataType.CountryRegion;
            }
            else if (methodName == "CorresUnitDataAsync")
            {
                var receiveParame = ((BaseReceiveDataRequestDto<CorresUnitReceiveDto>)invocation.Arguments[0]).IT_DATA;
                parameCount = receiveParame.item.Count;
                requestParame = receiveParame.item.ToJson();
                receiveDataType = ReceiveDataType.CrossUnit;
            }
            else if (methodName == "CurrencyDataAsync")
            {
                var receiveParame = ((BaseReceiveDataRequestDto<CurrencyReceiveDto>)invocation.Arguments[0]).IT_DATA;
                parameCount = receiveParame.item.Count;
                requestParame = receiveParame.item.ToJson();
                receiveDataType = ReceiveDataType.Currency;
            }
            else if (methodName == "InvoiceTypeDataAsync")
            {
                var receiveParame = ((BaseReceiveDataRequestDto<InvoiceTypeItem>)invocation.Arguments[0]).IT_DATA;
                parameCount = receiveParame.item.Count;
                requestParame = receiveParame.item.ToJson();
                receiveDataType = ReceiveDataType.Invoice;
            }
            else if (methodName == "DeviceClassCodeDataAsync")
            {
                var receiveParame = ((BaseReceiveDataRequestDto<DeviceClassCodeItem>)invocation.Arguments[0]).IT_DATA;
                parameCount = receiveParame.item.Count;
                requestParame = receiveParame.item.ToJson();
                receiveDataType = ReceiveDataType.ClassDevice;
            }
            else if (methodName == "ScientifiCNoProjectDataAsync")
            {
                var receiveParame = ((BaseReceiveDataRequestDto<ScientifiCNoProjectItem>)invocation.Arguments[0]).IT_DATA;
                parameCount = receiveParame.item.Count;
                requestParame = receiveParame.item.ToJson();
                receiveDataType = ReceiveDataType.Rcientific;
            }
            else if (methodName == "RoomNumberDataAsync")
            {
                var receiveParame = ((BaseReceiveDataRequestDto<RoomNumberItem>)invocation.Arguments[0]).IT_DATA;
                parameCount = receiveParame.item.Count;
                requestParame = receiveParame.item.ToJson();
                receiveDataType = ReceiveDataType.Room;
            }
            else if (methodName == "LanguageDataAsync")
            {
                var receiveParame = ((BaseReceiveDataRequestDto<LanguageItem>)invocation.Arguments[0]).IT_DATA;
                parameCount = receiveParame.item.Count;
                requestParame = receiveParame.item.ToJson();
                receiveDataType = ReceiveDataType.Language;
            }
            else if (methodName == "LouDongDataAsync")
            {
                var receiveParame = ((BaseReceiveDataRequestDto<LouDongItem>)invocation.Arguments[0]).IT_DATA;
                parameCount = receiveParame.item.Count;
                requestParame = receiveParame.item.ToJson();
                receiveDataType = ReceiveDataType.LouDong;
            }
            else if (methodName == "UnitMeasurementDataAsync")
            {
                var receiveParame = ((BaseReceiveDataRequestDto<UnitMeasurementItem>)invocation.Arguments[0]).IT_DATA;
                parameCount = receiveParame.item.Count;
                requestParame = receiveParame.item.ToJson();
                receiveDataType = ReceiveDataType.UnitMeasurement;
            }
            else if (methodName == "RegionalDataAsync")
            {
                var receiveParame = ((BaseReceiveDataRequestDto<RegionalItem>)invocation.Arguments[0]).IT_DATA;
                parameCount = receiveParame.item.Count;
                requestParame = receiveParame.item.ToJson();
                receiveDataType = ReceiveDataType.Regional;
            }
            else if (methodName == "RegionalCenterDataAsync")
            {
                var receiveParame = ((BaseReceiveDataRequestDto<RegionalCenterItem>)invocation.Arguments[0]).IT_DATA;
                parameCount = receiveParame.item.Count;
                requestParame = receiveParame.item.ToJson();
                receiveDataType = ReceiveDataType.RegionalCenter;
            }
            else if (methodName == "NationalEconomyDataAsync")
            {
                var receiveParame = ((BaseReceiveDataRequestDto<NationalEconomyItem>)invocation.Arguments[0]).IT_DATA;
                parameCount = receiveParame.item.Count;
                requestParame = receiveParame.item.ToJson();
                receiveDataType = ReceiveDataType.NationalEconomy;
            }
            else if (methodName == "ProjectClassificationDataAsync")
            {
                var receiveParame = ((BaseReceiveDataRequestDto<ProjectClassificationItem>)invocation.Arguments[0]).IT_DATA;
                parameCount = receiveParame.item.Count;
                requestParame = receiveParame.item.ToJson();
                receiveDataType = ReceiveDataType.ProjectClassification;
            }
            else if (methodName == "RelationalContractsDataAsync")
            {
                var receiveParame = ((BaseReceiveDataRequestDto<RelationalContractsItem>)invocation.Arguments[0]).IT_DATA;
                parameCount = receiveParame.item.Count;
                requestParame = receiveParame.item.ToJson();
                receiveDataType = ReceiveDataType.Regional; 
            }
            else if (methodName == "AccountingDepartmentDataAsync")
            {
                var receiveParame = ((BaseReceiveDataRequestDto<AccountingDepartmentReceiveDto>)invocation.Arguments[0]).IT_DATA;
                parameCount = receiveParame.item.Count;
                requestParame = receiveParame.item.ToJson();
                receiveDataType = ReceiveDataType.AccountingDepartment;
            }
            else if (methodName == "AdministrativeOrganizationDataAsync")
            {
                var receiveParame = ((BaseReceiveDataRequestDto<AdministrativeOrganizationReceiveRequestDto>)invocation.Arguments[0]).IT_DATA;
                parameCount = receiveParame.item.Count;
                requestParame = receiveParame.item.ToJson();
                receiveDataType = ReceiveDataType.AdministrativeOrganization;
            }
            else if (methodName == "CommonDataAsync")
            {
                var receiveParame = ((BaseReceiveDataRequestDto<ValueDomainReceiveRequestDto>)invocation.Arguments[0]).IT_DATA;
                parameCount = receiveParame.item.Count;
                requestParame = receiveParame.item.ToJson();
                receiveDataType = ReceiveDataType.AdministrativeOrganization;
            }
            else if (methodName == "ManagementOrganizationDataAsync")
            {
                var receiveParame = ((BaseReceiveDataRequestDto<POPMangOrgItem>)invocation.Arguments[0]).IT_DATA;
                parameCount = receiveParame.item.Count;
                requestParame = receiveParame.item.ToJson();
                receiveDataType = ReceiveDataType.POPManger;
            }
            #endregion

            #region 记录接收数据的请求日志
            if (!string.IsNullOrWhiteSpace(requestParame))
            {
                receiveRecordLog = new ReceiveRecordLog()
                {
                    Id = receiceRecordId,
                    ReceiveNumber = parameCount,
                    RequestParame = requestParame,
                    Traceidentifier = traceIdentifier,
                    ReceiveType = receiveDataType,
                };
                baseService.ReceiveRecordLogAsync(receiveRecordLog, DataOperationType.Insert);
            }
            #endregion

            //目标方法
            try
            {
                invocation.Proceed();
                var res = (Task)invocation.ReturnValue;
                res.Wait();//如果任务有异常 直接报错
                #region 更新请求日志

                if (!string.IsNullOrWhiteSpace(requestParame))
                {
                    receiveRecordLog = new ReceiveRecordLog()
                    {
                        Id = receiceRecordId,
                        ReceiveNumber = parameCount,
                        RequestParame = requestParame,
                        SuccessNumber = parameCount,
                        Traceidentifier = traceIdentifier,
                        ReceiveType = receiveDataType,
                    };
                    baseService.ReceiveRecordLogAsync(receiveRecordLog, DataOperationType.Update);
                }
                #endregion
            }
            catch (Exception ex)
            {
                //这里不处理  会自动向上抛

            }

        }
    }
}
