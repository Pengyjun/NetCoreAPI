using GDCMasterDataReceiveApi.Application.Contracts.Dto.DHData;
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
            var url = $"{AppsettingsHelper.GetValue("DHData:Url")}{requestDto.InterfaceUrl}?" +
                      $"{AppsettingsHelper.GetValue("DHData:SysCode")}&" +
                      $"updatetime={(requestDto.UpdateTime != DateTime.MinValue ? requestDto.UpdateTime : "1900-01-01")}&" +
                      $"functionAuthorizationCode={requestDto.FCode}&" +
                      $"pageindex={requestDto.PageIndex}";

            WebHelper web = new WebHelper();

            const int maxRetries = 3; // 最大重试次数
            int currentTry = 0;

            while (currentTry < maxRetries)
            {
                var response = await web.DoGetAsync(url);

                if (response.Code == 200)
                {
                    return response.Result;
                }
                else
                {
                    currentTry++;
                    if (currentTry == maxRetries)
                    {
                        throw new Exception(response.Msg);
                    }

                    await Task.Delay(10000); // 等待10秒后重试
                }
            }

            return ""; // 默认返回值
        }
        /// <summary>
        /// 获取接收数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        private async Task<List<T>> GetDataAsync<T>(KeyVerificationRequestDto requestDto) where T : class
        {
            // 装所有页的数据
            List<T> data = new();

            // 解析 JSON
            var jsonObject = JObject.Parse(await JData(requestDto));
            int count = Convert.ToInt32(jsonObject["Count"]);// 需要循环的总页数

            for (int i = 1; i <= count; i++)
            {
                requestDto.PageIndex = i;
                var jsonObject2 = JObject.Parse(await JData(requestDto));
                var jData = jsonObject2["Data"].ToString();
                var nData = JsonConvert.DeserializeObject<List<T>>(jData);
                data.AddRange(nData);
            }

            return data;
        }
        /// <summary>
        /// DH机构数据写入
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> ReceiveOrganzationAsync()
        {
            ResponseAjaxResult<bool> responseAjaxResult = new();

            var requestDto = new KeyVerificationRequestDto
            {
                FCode = "68AEA3249B7C43F79234B7618620C683",
                InterfaceUrl = "Department/GetOrganzationPageList",
                //UpdateTime = DateTime.Now,
                PageIndex = 1//从第一页开始拉取
            };

            //获取接收数据
            var data = await GetDataAsync<DHOrganzation>(requestDto);

            if (data != null && data.Any())
            {
                var tData = await _dbContext.Queryable<DHOrganzation>().Where(x => x.IsDelete == 1).ToListAsync();

                var keyIds = new HashSet<string>(tData.Select(t => t.OID));
                var insertTable = data.Where(x => !keyIds.Contains(x.OID)).ToList();
                var updateTable = data.Where(x => keyIds.Contains(x.OID)).ToList();

                if (insertTable.Any())
                {
                    foreach (var item in insertTable) { item.IsDelete = Convert.ToInt32(item.DELETE); item.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(); item.CreateTime = DateTime.Now; }
                    await _dbContext.Fastest<DHOrganzation>().BulkCopyAsync(insertTable);
                }
                if (updateTable.Any())
                {
                    foreach (var item in updateTable) { item.UpdateTime = DateTime.Now; }
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
        /// <summary>
        /// DH行政和核算机构映射写入
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> ReceiveAdministrativeAsync()
        {
            ResponseAjaxResult<bool> responseAjaxResult = new();

            var requestDto = new KeyVerificationRequestDto
            {
                FCode = "68AEA3249B7C43F79234B7618620C692",
                InterfaceUrl = "Department/GetAdministrativePageList",
                //UpdateTime = DateTime.Now,
                PageIndex = 1//从第一页开始拉取
            };

            //获取接收数据
            var data = await GetDataAsync<DHAdministrative>(requestDto);

            if (data != null && data.Any())
            {
                var tData = await _dbContext.Queryable<DHAdministrative>().Where(x => x.IsDelete == 1).ToListAsync();

                var keyIds = new HashSet<string>(tData.Select(t => t.Fzid));
                var insertTable = data.Where(x => !keyIds.Contains(x.Fzid)).ToList();
                var updateTable = data.Where(x => keyIds.Contains(x.Fzid)).ToList();

                if (insertTable.Any())
                {
                    foreach (var item in insertTable)
                    {
                        item.IsDelete = Convert.ToInt32(item.Fzdelete);
                        item.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                        item.CreateTime = DateTime.Now;
                    }
                    await _dbContext.Fastest<DHAdministrative>().BulkCopyAsync(insertTable);
                }
                if (updateTable.Any())
                {
                    foreach (var item in updateTable) { item.UpdateTime = DateTime.Now; }
                    await _dbContext.Updateable(updateTable).WhereColumns(x => x.Fzid).ExecuteCommandAsync();
                }

                responseAjaxResult.SuccessResult(true);
            }
            else
            {
                responseAjaxResult.Fail();
            }
            return responseAjaxResult;
        }
        /// <summary>
        /// DH行政机构(多组织)写入
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> ReceiveOrganzationDepAsync()
        {
            ResponseAjaxResult<bool> responseAjaxResult = new();

            var requestDto = new KeyVerificationRequestDto
            {
                FCode = "68AEA3249B7C43F78234B7618620CB12",
                InterfaceUrl = "Department/GetMdmOrganzationPageList",
                //UpdateTime = DateTime.Now,
                PageIndex = 1//从第一页开始拉取
            };

            //获取接收数据
            var data = await GetDataAsync<DHOrganzationDep>(requestDto);

            if (data != null && data.Any())
            {
                var tData = await _dbContext.Queryable<DHOrganzationDep>().Where(x => x.IsDelete == 1).ToListAsync();

                var keyIds = new HashSet<string>(tData.Select(t => t.MdmCode));
                var insertTable = data.Where(x => !keyIds.Contains(x.MdmCode)).ToList();
                var updateTable = data.Where(x => keyIds.Contains(x.MdmCode)).ToList();

                if (insertTable.Any())
                {
                    foreach (var item in insertTable) { item.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(); item.CreateTime = DateTime.Now; }
                    await _dbContext.Fastest<DHOrganzationDep>().BulkCopyAsync(insertTable);
                }
                if (updateTable.Any())
                {
                    foreach (var item in updateTable) { item.UpdateTime = DateTime.Now; }
                    await _dbContext.Updateable(updateTable).WhereColumns(x => x.MdmCode).ExecuteCommandAsync();
                }

                responseAjaxResult.SuccessResult(true);
            }
            else
            {
                responseAjaxResult.Fail();
            }
            return responseAjaxResult;
        }
        /// <summary>
        /// DH核算机构(多组织)写入
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> ReceiveAdjustAccountsMultipleOrgAsync()
        {
            ResponseAjaxResult<bool> responseAjaxResult = new();

            var requestDto = new KeyVerificationRequestDto
            {
                FCode = "68AEA3249B7C43F78234C7613652CB17",
                InterfaceUrl = "Department/GetMdmAdjustAccountsMultipleOrgPageList",
                //UpdateTime = DateTime.Now,
                PageIndex = 1//从第一页开始拉取
            };

            //获取接收数据
            var data = await GetDataAsync<DHAdjustAccountsMultipleOrg>(requestDto);

            if (data != null && data.Any())
            {
                var tData = await _dbContext.Queryable<DHAdjustAccountsMultipleOrg>().Where(x => x.IsDelete == 1).ToListAsync();

                var keyIds = new HashSet<string>(tData.Select(t => t.Zaco));
                var insertTable = data.Where(x => !keyIds.Contains(x.Zaco)).ToList();
                var updateTable = data.Where(x => keyIds.Contains(x.Zaco)).ToList();

                if (insertTable.Any())
                {
                    foreach (var item in insertTable) { item.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(); item.CreateTime = DateTime.Now; }
                    await _dbContext.Fastest<DHAdjustAccountsMultipleOrg>().BulkCopyAsync(insertTable);
                }
                if (updateTable.Any())
                {
                    foreach (var item in updateTable) { item.UpdateTime = DateTime.Now; }
                    await _dbContext.Updateable(updateTable).WhereColumns(x => x.Zaco).ExecuteCommandAsync();
                }

                responseAjaxResult.SuccessResult(true);
            }
            else
            {
                responseAjaxResult.Fail();
            }
            return responseAjaxResult;
        }
        /// <summary>
        /// DH核算部门写入
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> ReveiveAccountingDeptAsync()
        {
            ResponseAjaxResult<bool> responseAjaxResult = new();

            var requestDto = new KeyVerificationRequestDto
            {
                FCode = "68AEA3249C7C43F78234C7613652A956",
                InterfaceUrl = "Department/GetMdmAccountingDeptPageList",
                //UpdateTime = DateTime.Now,
                PageIndex = 1//从第一页开始拉取
            };

            //获取接收数据
            var data = await GetDataAsync<DHAccountingDept>(requestDto);

            if (data != null && data.Any())
            {
                var tData = await _dbContext.Queryable<DHAccountingDept>().Where(x => x.IsDelete == 1).ToListAsync();

                var keyIds = new HashSet<string>(tData.Select(t => t.Zdid));
                var insertTable = data.Where(x => !keyIds.Contains(x.Zdid)).ToList();
                var updateTable = data.Where(x => keyIds.Contains(x.Zdid)).ToList();

                if (insertTable.Any())
                {
                    foreach (var item in insertTable) { item.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(); item.CreateTime = DateTime.Now; item.IsDelete = item.Zdatstate == "0" ? 1 : item.Zdatstate == "1" ? 0 : 1; }
                    await _dbContext.Fastest<DHAccountingDept>().BulkCopyAsync(insertTable);
                }
                if (updateTable.Any())
                {
                    foreach (var item in updateTable) { item.UpdateTime = DateTime.Now; }
                    await _dbContext.Updateable(updateTable).WhereColumns(x => x.Zdid).ExecuteCommandAsync();
                }

                responseAjaxResult.SuccessResult(true);
            }
            else
            {
                responseAjaxResult.Fail();
            }
            return responseAjaxResult;
        }
        /// <summary>
        /// DH项目信息写入
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> ReceiveProjectsAsync()
        {
            ResponseAjaxResult<bool> responseAjaxResult = new();

            var requestDto = new KeyVerificationRequestDto
            {
                FCode = "68AEA3249B7C43F79234B7618620C685",
                InterfaceUrl = "Project/GetProjectsPageList",
                //UpdateTime = DateTime.Now,
                PageIndex = 1//从第一页开始拉取
            };

            //获取接收数据
            var data = await GetDataAsync<DHProjects>(requestDto);

            if (data != null && data.Any())
            {
                var tData = await _dbContext.Queryable<DHProjects>().Where(x => x.IsDelete == 1).ToListAsync();

                var keyIds = new HashSet<string>(tData.Select(t => t.ZPROJECT));
                var insertTable = data.Where(x => !keyIds.Contains(x.ZPROJECT)).ToList();
                var updateTable = data.Where(x => keyIds.Contains(x.ZPROJECT)).ToList();

                if (insertTable.Any())
                {
                    foreach (var item in insertTable) { item.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(); item.IsDelete = Convert.ToInt32(item.Zdelete); item.CreateTime = DateTime.Now; }
                    await _dbContext.Fastest<DHProjects>().BulkCopyAsync(insertTable);
                }
                if (updateTable.Any())
                {
                    foreach (var item in updateTable) { item.UpdateTime = DateTime.Now; }
                    await _dbContext.Updateable(updateTable).WhereColumns(x => x.ZPROJECT).ExecuteCommandAsync();
                }

                responseAjaxResult.SuccessResult(true);
            }
            else
            {
                responseAjaxResult.Fail();
            }
            return responseAjaxResult;
        }
        /// <summary>
        /// DH虚拟项目写入
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> ReceiveVirtualProjectAsync()
        {
            ResponseAjaxResult<bool> responseAjaxResult = new();

            var requestDto = new KeyVerificationRequestDto
            {
                FCode = "68AEA3249B7C43F79234B7618620C678",
                InterfaceUrl = "VirtualProject/GetVirtualProjectAsync",
                //UpdateTime = DateTime.Now, 没有项目更新时间 全量拉
                PageIndex = 1//从第一页开始拉取
            };

            //获取接收数据
            var data = await GetDataAsync<DHVirtualProject>(requestDto);

            if (data != null && data.Any())
            {
                var tData = await _dbContext.Queryable<DHVirtualProject>().Where(x => x.IsDelete == 1).ToListAsync();

                var keyIds = new HashSet<string>(tData.Select(t => t.ZVTPROJ));
                var insertTable = data.Where(x => !keyIds.Contains(x.ZVTPROJ)).ToList();
                var updateTable = data.Where(x => keyIds.Contains(x.ZVTPROJ)).ToList();

                if (insertTable.Any())
                {
                    foreach (var item in insertTable) { item.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(); item.IsDelete = Convert.ToInt32(item.Zdelete); item.CreateTime = DateTime.Now; }
                    await _dbContext.Fastest<DHVirtualProject>().BulkCopyAsync(insertTable);
                }
                if (updateTable.Any())
                {
                    foreach (var item in updateTable) { item.UpdateTime = DateTime.Now; }
                    await _dbContext.Updateable(updateTable).WhereColumns(x => x.ZVTPROJ).ExecuteCommandAsync();
                }

                responseAjaxResult.SuccessResult(true);
            }
            else
            {
                responseAjaxResult.Fail();
            }
            return responseAjaxResult;
        }
        /// <summary>
        /// DH商机项目写入
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> ReceiveOpportunityAsync()
        {
            ResponseAjaxResult<bool> responseAjaxResult = new();

            var requestDto = new KeyVerificationRequestDto
            {
                FCode = "68AEA3249B7C43F79234B7618620C679",
                InterfaceUrl = "Opportunity/GetOpportunityAsync",
                //UpdateTime = DateTime.Now, 没有项目更新时间 全量拉
                PageIndex = 1//从第一页开始拉取
            };

            //获取接收数据
            var data = await GetDataAsync<DHOpportunity>(requestDto);

            if (data != null && data.Any())
            {
                var tData = await _dbContext.Queryable<DHOpportunity>().Where(x => x.IsDelete == 1).ToListAsync();

                var keyIds = new HashSet<string>(tData.Select(t => t.ZBOP));
                var insertTable = data.Where(x => !keyIds.Contains(x.ZBOP)).ToList();
                var updateTable = data.Where(x => keyIds.Contains(x.ZBOP)).ToList();

                if (insertTable.Any())
                {
                    foreach (var item in insertTable) { item.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(); item.IsDelete = Convert.ToInt32(item.Zdelete); item.CreateTime = DateTime.Now; }
                    await _dbContext.Fastest<DHOpportunity>().BulkMergeAsync(insertTable);
                }
                if (updateTable.Any())
                {
                    foreach (var item in updateTable) { item.UpdateTime = DateTime.Now; }
                    await _dbContext.Updateable(updateTable).WhereColumns(x => x.ZBOP).ExecuteCommandAsync();
                }

                responseAjaxResult.SuccessResult(true);
            }
            else
            {
                responseAjaxResult.Fail();
            }
            return responseAjaxResult;
        }
        /// <summary>
        /// DH科研项目写入
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> ReceiveResearchListAsync()
        {
            ResponseAjaxResult<bool> responseAjaxResult = new();

            var requestDto = new KeyVerificationRequestDto
            {
                FCode = "68AEA3249B7C43F79234B7618620C710",
                InterfaceUrl = "Research/GetResearchList",
                //UpdateTime = DateTime.Now, 没有项目更新时间 全量拉
                PageIndex = 1//从第一页开始拉取
            };

            //获取接收数据
            var data = await GetDataAsync<DHResearch>(requestDto);

            if (data != null && data.Any())
            {
                var tData = await _dbContext.Queryable<DHResearch>().Where(x => x.IsDelete == 1).ToListAsync();

                var keyIds = new HashSet<string>(tData.Select(t => t.FzsrpCode));
                var insertTable = data.Where(x => !keyIds.Contains(x.FzsrpCode)).ToList();
                var updateTable = data.Where(x => keyIds.Contains(x.FzsrpCode)).ToList();

                if (insertTable.Any())
                {
                    foreach (var item in insertTable) { item.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(); item.IsDelete = Convert.ToInt32(item.Fzdelete); item.CreateTime = DateTime.Now; }
                    await _dbContext.Fastest<DHResearch>().BulkMergeAsync(insertTable);
                }
                if (updateTable.Any())
                {
                    foreach (var item in updateTable) { item.UpdateTime = DateTime.Now; }
                    await _dbContext.Updateable(updateTable).WhereColumns(x => x.FzsrpCode).ExecuteCommandAsync();
                }

                responseAjaxResult.SuccessResult(true);
            }
            else
            {
                responseAjaxResult.Fail();
            }
            return responseAjaxResult;
        }
        /// <summary>
        /// DH生产经营管理组织
        /// </summary>
        /// <returns></returns>

        public async Task<ResponseAjaxResult<bool>> ReceiveGetMdmManagementOrgageListAsync()
        {
            ResponseAjaxResult<bool> responseAjaxResult = new();

            var requestDto = new KeyVerificationRequestDto
            {
                FCode = "68AEA32679C7C43F96587C7613652A18",
                InterfaceUrl = "Department/GetMdmManagementOrgageList",
                //UpdateTime = DateTime.Now, 没有项目更新时间 全量拉
                PageIndex = 1//从第一页开始拉取
            };

            //获取接收数据
            var data = await GetDataAsync<DHMdmManagementOrgage>(requestDto);

            if (data != null && data.Any())
            {
                var tData = await _dbContext.Queryable<DHMdmManagementOrgage>().Where(x => x.IsDelete == 1).ToListAsync();

                var keyIds = new HashSet<string>(tData.Select(t => t.Ztreeid));
                var insertTable = data.Where(x => !keyIds.Contains(x.Ztreeid)).ToList();
                var updateTable = data.Where(x => keyIds.Contains(x.Ztreeid)).ToList();

                if (insertTable.Any())
                {
                    foreach (var item in insertTable) { item.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(); item.IsDelete = Convert.ToInt32(item.Zdelete); item.CreateTime = DateTime.Now; }
                    await _dbContext.Fastest<DHMdmManagementOrgage>().BulkMergeAsync(insertTable);
                }
                if (updateTable.Any())
                {
                    foreach (var item in updateTable) { item.UpdateTime = DateTime.Now; }
                    await _dbContext.Updateable(updateTable).WhereColumns(x => x.Ztreeid).ExecuteCommandAsync();
                }

                responseAjaxResult.SuccessResult(true);
            }
            else
            {
                responseAjaxResult.Fail();
            }
            return responseAjaxResult;
        }
        /// <summary>
        /// DH委托关系
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> ReceiveGetDHMdmMultOrgAgencyRelPageListAsync()
        {
            ResponseAjaxResult<bool> responseAjaxResult = new();

            var requestDto = new KeyVerificationRequestDto
            {
                FCode = "68AEA3249C7C43F65932C7613CB2A365",
                InterfaceUrl = "Department/GetMdmMultOrgAgencyRelPageList",
                //UpdateTime = DateTime.Now, 没有项目更新时间 全量拉
                PageIndex = 1//从第一页开始拉取
            };

            //获取接收数据
            var data = await GetDataAsync<DHMdmMultOrgAgencyRelPage>(requestDto);

            if (data != null && data.Any())
            {
                var tData = await _dbContext.Queryable<DHMdmMultOrgAgencyRelPage>().Where(x => x.IsDelete == 1).ToListAsync();

                var keyIds = new HashSet<string>(tData.Select(t => t.Ztreeid));
                var key2Ids = new HashSet<string>(tData.Select(t => t.Znumc4x));
                var key3Ids = new HashSet<string>(tData.Select(t => t.ZmviewFlag));
                var insertTable = data.Where(x => !keyIds.Contains(x.Ztreeid) && !key2Ids.Contains(x.Znumc4x) && !key3Ids.Contains(x.ZmviewFlag)).ToList();
                var updateTable = data.Where(x => keyIds.Contains(x.Ztreeid) && key2Ids.Contains(x.Znumc4x) && key3Ids.Contains(x.ZmviewFlag)).ToList();

                if (insertTable.Any())
                {
                    foreach (var item in insertTable) { item.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(); item.CreateTime = DateTime.Now; }
                    await _dbContext.Fastest<DHMdmMultOrgAgencyRelPage>().BulkMergeAsync(insertTable);
                }
                if (updateTable.Any())
                {
                    foreach (var item in updateTable) { item.UpdateTime = DateTime.Now; }
                    await _dbContext.Updateable(updateTable).WhereColumns(x => new { x.Ztreeid, x.Znumc4x, x.ZmviewFlag }).ExecuteCommandAsync();
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
