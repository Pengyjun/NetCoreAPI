using AutoMapper;
using GDCMasterDataReceiveApi.Application.Contracts;
using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.Institution;
using GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.User;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CountryRegion;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Currency;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Project;
using GDCMasterDataReceiveApi.Application.Contracts.IService.IReceiveService;
using GDCMasterDataReceiveApi.Domain.Models;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Const;
using GDCMasterDataReceiveApi.Domain.Shared.Utils;
using SqlSugar;
using UtilsSharp;

namespace GDCMasterDataReceiveApi.Application.Service.ReceiveService
{
    /// <summary>
    /// 接收主数据推送接口实现
    /// </summary>
    public class ReceiveService : IReceiveService
    {
        private readonly ISqlSugarClient _dbContext;
        private readonly IMapper _mapper;
        /// <summary>
        /// 注入上下文
        /// </summary>
        public ReceiveService(ISqlSugarClient dbContext, IMapper mapper)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }
        /// <summary>
        /// 获取通用字典数据
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> CommonDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 往来单位主数据
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> CorresUnitDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 多组织-税务代管组织(行政)
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> EscrowOrganizationDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 商机项目(含境外商机项目)
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> BusinessProjectDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 国家地区
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<ResponseResult>> CountryRegionDataAsync(RequestResult<CountryRegionReceiveDto> requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<ResponseResult>();
            //获取数据
            var getData = requestDto.IT_DATA;
            if (getData != null && getData.Any())
            {
                var rst = new List<RESULT>();
                foreach (var item in getData)
                {
                    rst.Add(new RESULT
                    {
                        ZZSERIAL = item.ZZSERIAL,
                        ZZMSG = ResponseMessage.OPERATION_SUCCESS,
                        ZZSTAT = ResponseStatus.SUCCESS
                    });
                }
                var mData = _mapper.Map<List<CountryRegionReceiveDto>, List<CountryRegion>>(getData);

                foreach (var item in mData)
                {
                    item.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                }
                await _dbContext.Insertable(mData).ExecuteCommandAsync();
                var rtn = new RETURN
                {
                    ZINSTID = requestDto.IS_REQ_HEAD_ASYNC.ZINSTID,
                    ZZRESTIME = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    ZZMSG = ResponseMessage.OPERATION_SUCCESS,
                    ZZSTAT = ResponseStatus.SUCCESS
                };
                var res = new ResponseResult()
                {
                    ES_RETURN = rtn,
                    ET_RESULT = rst
                };
                responseAjaxResult.Success();
            }

            return responseAjaxResult;
        }
        /// <summary>
        /// 大洲
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> CountryContinentDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 中交区域总部
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> RegionalDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 常用计量单位
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> UnitMeasurementDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 中交项目行业分类产业分类、业务板块、十二大业务类型、江河湖海对照关系
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> ProjectClassificationDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 金融机构
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> FinancialInstitutionDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 物资设备分类编码
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> DeviceClassCodeDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 核算部门
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> AccountingDepartmentDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 中交区域中心
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> RegionalCenterDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 银行账号
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> BankCardDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 国民经济行业分类
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> NationalEconomyDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 多组织-行政组织
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> AdministrativeOrganizationDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 发票类型
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> InvoiceTypeDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 币种
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> CurrencyDataAsync(BaseReceiveDataRequestDto<CurrencyReceiveDto> baseReceiveDataRequestDto)
        {

            await Console.Out.WriteLineAsync("接收到的数据：" + baseReceiveDataRequestDto.IT_DATA.ToJson());
            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 行政机构和核算机构映射关系
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> AdministrativeAccountingMapperDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 项目类
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> ProjectDataAsync(BaseReceiveDataRequestDto<ProjectItem> receiveDataMDMRequestDto)
        {
            MDMResponseResult responseAjaxResult = new MDMResponseResult();
            await Console.Out.WriteLineAsync("接收到的数据："+ receiveDataMDMRequestDto.IT_DATA.ToJson());
            var projectList=_mapper.Map<List<ProjectItem>, List<Project>>(receiveDataMDMRequestDto.IT_DATA.item);
            await _dbContext.Insertable<Project>(projectList).ExecuteCommandAsync();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 科研项目
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> ScientifiCNoProjectDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 商机项目(不含境外商机项目)
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> BusinessNoCpportunityDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 委托关系
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> RelationalContractsDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 生产经营管理组织
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> ManagementOrganizationDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 楼栋
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> LouDongDataAsync()
        {
            ///***
            // * 测试写入数据
            // */
            //var tt = new List<LouDong>();
            //var test = new LouDong()
            //{
            //    Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
            //    ZZSERIAL = "1",
            //    ZBLDG = "2",
            //    ZBLDG_NAME = "3",
            //    ZSTATE = "4",
            //    ZFORMATINF = "5",
            //    ZSYSTEM = "6",
            //    ZPROJECT = "7"
            //};
            //tt.Add(test);
            //var x = _dbContext.Storageable(tt).ToStorage();
            //await x.AsInsertable.ExecuteCommandAsync();//不存在插入
            //await x.AsUpdateable.ExecuteCommandAsync();//存在更新
            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 房号
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> RoomNumberDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 境内行政区划
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> AdministrativeDivisionDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 语言语种
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> LanguageDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 物资设备明细编码
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> DeviceDetailCodeDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 多组织-核算机构
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> AccountingOrganizationDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 人员主数据
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> PersonDataAsync(ReceiveUserRequestDto receiveUserRequestDto)
        {
            var responseAjaxResult = new MDMResponseResult();
            try
            {
                //创建用户
                if (receiveUserRequestDto.OP_TYPE != null && receiveUserRequestDto.OP_TYPE.ToUpper() == "CREATE")
                {
                    var isExistUser = await _dbContext.Queryable<User>().Where(x => x.IsDelete == 1 && x.EMP_CODE == receiveUserRequestDto.user.EMP_CODE).SingleAsync();
                    if (isExistUser == null)
                    {
                        var user = _mapper.Map<User>(receiveUserRequestDto.user);
                        user.Enable = 1;
                        user.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                        await _dbContext.Insertable<User>(user).ExecuteCommandAsync();
                        responseAjaxResult.Success();
                    }
                    else
                    {
                        if (isExistUser.Enable == 0)
                        {
                            isExistUser.Enable = 1;
                        }
                        await _dbContext.Updateable<User>(isExistUser).ExecuteCommandAsync();
                        responseAjaxResult.UpdateSuccess();
                    }
                }
                //修改用户   禁用  启用用户
                else if (receiveUserRequestDto.OP_TYPE != null && receiveUserRequestDto.OP_TYPE.ToUpper() == "EDIT"
                    || receiveUserRequestDto.OP_TYPE != null && receiveUserRequestDto.OP_TYPE.ToUpper() == "DISABLE"
                    || receiveUserRequestDto.OP_TYPE != null && receiveUserRequestDto.OP_TYPE.ToUpper() == "ENABLE")
                {

                    var isExistUser = await _dbContext.Queryable<User>().Where(x => x.IsDelete == 1 && x.EMP_CODE == receiveUserRequestDto.user.EMP_CODE).SingleAsync();
                    if (isExistUser == null)
                    {
                        responseAjaxResult.UserNoExist();
                        return responseAjaxResult;
                    }
                    if (receiveUserRequestDto.OP_TYPE != null && receiveUserRequestDto.OP_TYPE.ToUpper() == "EDIT")
                    {
                        var user = _mapper.Map<User>(receiveUserRequestDto.user);
                        await _dbContext.Updateable<User>(user).Where(x => x.EMP_CODE == isExistUser.EMP_CODE).IgnoreColumns(x => x.Id).ExecuteCommandAsync();
                        responseAjaxResult.Success(); 
                        return responseAjaxResult;
                    }
                    //禁用
                    else if (receiveUserRequestDto.OP_TYPE != null && receiveUserRequestDto.OP_TYPE.ToUpper() == "DISABLE")
                    {
                        isExistUser.Enable = 0;
                    }
                    //启用
                    else if (receiveUserRequestDto.OP_TYPE != null && receiveUserRequestDto.OP_TYPE.ToUpper() == "ENABLE")
                    {
                        isExistUser.Enable = 1;
                    } 

                    await _dbContext.Updateable<User>(isExistUser).Where(x => x.EMP_CODE == isExistUser.EMP_CODE).IgnoreColumns(x => x.Id).ExecuteCommandAsync();
                    responseAjaxResult.Success();
                }

            }
            catch (Exception ex)
            {
                responseAjaxResult.Fail();

            }
            return responseAjaxResult;
        }
        /// <summary>
        /// 机构主数据
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> InstitutionDataAsync(ReceiveInstitutionRequestDto receiveInstitutionRequestDto)
        {
            var responseAjaxResult = new MDMResponseResult();
            try
            {
                var institutions = _mapper.Map<List<InstitutionItem>, List<Institution>>(receiveInstitutionRequestDto.OrganizeItem);
                foreach (var item in institutions)
                {
                    item.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                }
                var institutiontOids = await _dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1).Select(x => x.OID).ToListAsync();
                //var oids = institutiontOids.Select(x => x).ToList();
                var insertOids = institutions.Where(x =>!institutiontOids.Contains(x.OID)).Select(x=>x.OID).ToList();
                var updateOids = institutions.Where(x =>institutiontOids.Contains(x.OID)).Select(x => x.OID).ToList();
                if (insertOids.Any())
                {
                    //插入操作
                    var batchData = institutions.Where(x => insertOids.Contains(x.OID)).ToList();
                    await _dbContext.Insertable<Institution>(batchData).ExecuteCommandAsync();
                }
                if (updateOids.Any())
                {
                    //插入操作
                    var batchData = institutions.Where(x => updateOids.Contains(x.OID)).ToList();
                    await _dbContext.Updateable<Institution>(batchData).ExecuteCommandAsync();
                }
                responseAjaxResult.Success();
            }
            catch (Exception ex)
            {
                responseAjaxResult.Fail();
            }
            return responseAjaxResult;
        }
    }
}
