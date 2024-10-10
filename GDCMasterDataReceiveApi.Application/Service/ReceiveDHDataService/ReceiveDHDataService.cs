﻿using GDCMasterDataReceiveApi.Application.Contracts.Dto.DHData;
using GDCMasterDataReceiveApi.Application.Contracts.IService.IReceiveDHDataService;
using GDCMasterDataReceiveApi.Domain.Models;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlSugar;
using UtilsSharp;

namespace GDCMasterDataReceiveApi.Application.Service.ReceiveDHDataService
{
    /// <summary>
    /// 接收DH相关数据实现
    /// </summary>
    public class ReceiveDHDataService : IReceiveDHDataService
    {
        private readonly ISqlSugarClient _dbContext;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public ReceiveDHDataService(ISqlSugarClient dbContext)
        {
            this._dbContext = dbContext;
        }
        /// <summary>
        /// 获取DH数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        private async Task<string>? JData(KeyVerificationRequestDto requestDto)
        {
            var url = AppsettingsHelper.GetValue("DHData:Url") + $"{requestDto.InterfaceUrl}?{AppsettingsHelper.GetValue("DHData:SysCode")}&updatetime=1900-01-01&functionAuthorizationCode={requestDto.FCode}&pageindex={requestDto.PageIndex}";
            if (requestDto.UpdateTime != DateTime.MinValue)
            {
                url = AppsettingsHelper.GetValue("DHData:Url") + $"{requestDto.InterfaceUrl}?{AppsettingsHelper.GetValue("DHData:SysCode")}&updatetime={requestDto.UpdateTime}&functionAuthorizationCode={requestDto.FCode}&pageindex={requestDto.PageIndex}";
            }
            WebHelper web = new WebHelper();
            var response = await web.DoGetAsync(url);
            if (response.Code == 200)
            {
                return response.Result;
            }
            else
            {
                throw new Exception(response.Msg);
            }
        }
        /// <summary>
        /// Dh机构数据写入
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> ReceiveOrganzationAsync()
        {
            ResponseAjaxResult<bool> responseAjaxResult = new();

            var requestDto = new KeyVerificationRequestDto
            {
                FCode = "68AEA3249B7C43F79234B7618620C683",
                InterfaceUrl = "Department/GetOrganzationPageList",
                PageIndex = 1//从第一页开始拉取
            };

            //装所有页的数据
            List<DHOrganzation> data = new();
            // 解析 JSON
            var jsonObject = JObject.Parse(await JData(requestDto));
            int count = Convert.ToInt32(jsonObject["Count"]);//需要循环的总页数

            if (count > 1)
            {
                for (int i = 1; i <= count; i++)
                {
                    requestDto.PageIndex = i;
                    var jsonObject2 = JObject.Parse(await JData(requestDto));
                    var jData = jsonObject2["Data"].ToJson();
                    var nData = JsonConvert.DeserializeObject<List<DHOrganzation>>(jData);
                    data.AddRange(nData);
                }
            }
            else
            {
                var jData = jsonObject["Data"].ToJson();
                data = JsonConvert.DeserializeObject<List<DHOrganzation>>(jData);
            }

            if (data != null && data.Any())
            {
                List<DHOrganzation> insertTable = new();
                List<DHOrganzation> updateTable = new();

                var tData = await _dbContext.Queryable<DHOrganzation>().Where(x => x.IsDelete == 1).ToListAsync();
                var insCondition = data.Where(x => !tData.Select(t => t.OID).ToList().Contains(x.OID)).Select(x => x.OID).ToList();
                var updateCondition = data.Where(x => tData.Select(t => t.OID).ToList().Contains(x.OID)).Select(x => x.OID).ToList();

                if (insCondition.Any())
                {
                    insertTable = data.Where(x => insCondition.Contains(x.OID)).ToList();
                    insertTable.ForEach(x => x.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId());
                    insertTable.ForEach(x => x.IsDelete = Convert.ToInt32(x.DELETE));
                    await _dbContext.Fastest<DHOrganzation>().BulkCopyAsync(insertTable);
                }
                if (updateCondition.Any())
                {
                    updateTable = data.Where(x => updateCondition.Contains(x.OID)).ToList();
                    await _dbContext.Updateable(updateTable).WhereColumns(x => x.OID).ExecuteCommandAsync();
                }

                responseAjaxResult.SuccessResult(true);
            }
            else
            {
                responseAjaxResult.Fail();
            }
            return responseAjaxResult;
        }
    }
}
