using AutoMapper;
using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.PullResult;
using HNKC.CrewManagePlatform.SqlSugars.Models;
using HNKC.CrewManagePlatform.Utils;
using Newtonsoft.Json;
using SqlSugar;
using UtilsSharp;

namespace HNKC.CrewManagePlatform.Services.Interface.PullResult
{
    /// <summary>
    /// 数据基础字典实现
    /// </summary>
    public class DataDictionaryService : IDataDictionaryService
    {
        private readonly ISqlSugarClient _dbContext;
        private readonly IMapper _mapper;

        /// <summary>
        /// 依赖注入
        /// </summary>
        public DataDictionaryService(ISqlSugarClient dbContext, IMapper mapper)
        {
            this._mapper = mapper;
            this._dbContext = dbContext;
        }
        /// <summary>
        /// 获取主数据值域&基础数据
        /// </summary>
        /// <param name="pullRequestDto"></param>
        /// <returns></returns>
        public async Task<Result> SaveValueDomainAsync(PullRequestDto pullRequestDto)
        {
            //数据结果集
            List<ValueDomainDto> rt = new();
            #region 数据接收
            var po = OutParams(pullRequestDto.CreateTime, pullRequestDto.UpdateTime, 1);
            WebHelper web = new();
            web.Headers.Add("AppKey", po.AppKey);
            web.Headers.Add("AppInterfaceCode", po.AppKey);
            var response = await web.DoGetAsync(po.Url);
            if (response.Code == 200)
            {
                var jsonObject = JsonConvert.DeserializeObject(response.Result)?.ToString();
                var rsData = JsonConvert.DeserializeObject<PullResponseDto>(jsonObject);
                //获取字典数据
                rt = rsData.Data;
            }
            #endregion

            #region 数据写入 全量增后续逻辑处理重复数据
            if (rt != null && rt.Any())
            {
                //数据映射
                var mp = _mapper.Map<List<ValueDomainDto>, List<ValueDomain>>(rt);

                if (mp != null && mp.Any())
                {
                    mp.ForEach(x => x.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId());
                    await _dbContext.Insertable(mp).ExecuteCommandAsync();
                }
                return Result.Success("操作成功");
            }
            return Result.Success("无数据操作成功");
            #endregion
        }

        private static PullOutResponseDto OutParams(DateTime? sdt, DateTime? edt, int tbType)
        {
            PullOutResponseDto po = new();

            var st = sdt ?? DateTime.Parse("1900-01-01");
            var et = edt ?? DateTime.Now;
            var replacements = new Dictionary<string, string>
               {
                   { "$createtime", st.ToString("yyyy-MM-dd") },
                   { "$updatetime", et.ToString("yyyy-MM-dd") }
               };
            var appkey = AppsettingsHelper.GetValue("PullMDM:Appkey");

            switch (tbType)
            {
                case 1:
                    var url = AppsettingsHelper.GetValue("PullMDM:ValueDomain:VDUrl");
                    foreach (var replacement in replacements) { url = url.Replace(replacement.Key, replacement.Value); }
                    po = new PullOutResponseDto
                    {
                        Url = url,
                        AppInterfaceCode = appkey,
                        AppKey = AppsettingsHelper.GetValue("PullMDM:ValueDomain:AppInterfaceCode")
                    };
                    break;
                case 2:
                    var url2 = AppsettingsHelper.GetValue("PullMDM:CountryRegion:VDUrl");
                    foreach (var replacement in replacements) { url2 = url2.Replace(replacement.Key, replacement.Value); }
                    po = new PullOutResponseDto
                    {
                        Url = url2,
                        AppInterfaceCode = appkey,
                        AppKey = AppsettingsHelper.GetValue("PullMDM:CountryRegion:AppInterfaceCode")
                    };
                    break;
                case 3:
                    var url3 = AppsettingsHelper.GetValue("PullMDM:AdministrativeDivision:VDUrl");
                    foreach (var replacement in replacements) { url3 = url3.Replace(replacement.Key, replacement.Value); }
                    po = new PullOutResponseDto
                    {
                        Url = url3,
                        AppInterfaceCode = appkey,
                        AppKey = AppsettingsHelper.GetValue("PullMDM:AdministrativeDivision:AppInterfaceCode")
                    };
                    break;
            }
            return po;
        }
    }
}
