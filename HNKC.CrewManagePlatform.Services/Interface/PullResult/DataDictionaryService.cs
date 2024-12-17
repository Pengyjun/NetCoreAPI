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
            List<DictionaryCtDto> rt = new();
            #region 数据接收
            var url = AppsettingsHelper.GetValue("PullMDM:ValueDomain:VDUrl");
            var sdt = pullRequestDto.CreateTime ?? DateTime.Parse("1900-01-01");
            var edt = pullRequestDto.UpdateTime ?? DateTime.Now;
            var replacements = new Dictionary<string, string>
               {
                   { "$createtime", sdt.ToString("yyyy-MM-dd") },
                   { "$updatetime", edt.ToString("yyyy-MM-dd") }
               };
            foreach (var replacement in replacements)
            {
                url = url.Replace(replacement.Key, replacement.Value);
            }
            var appKey = AppsettingsHelper.GetValue("PullMDM:Appkey");
            var appInterfaceCode = AppsettingsHelper.GetValue("PullMDM:ValueDomain:AppInterfaceCode");

            WebHelper web = new();
            web.Headers.Add("AppKey", appKey);
            web.Headers.Add("AppInterfaceCode", appInterfaceCode);
            var response = await web.DoGetAsync(url);
            if (response.Code == 200)
            {
                var jsonObject = JsonConvert.DeserializeObject(response.Result)?.ToString();
                var rsData = JsonConvert.DeserializeObject<PullResponseDto>(jsonObject);
                //获取字典数据
                rt = rsData.Data;
            }
            #endregion

            #region 数据写入
            if (rt != null && rt.Any())
            {
                //数据映射
                var mp = _mapper.Map<List<DictionaryCtDto>, List<DictionaryTable>>(rt);

                //获取源数据
                var dt = await _dbContext.Queryable<DictionaryTable>().Where(t => t.IsDelete == 1).ToListAsync();
                var vdids = dt.Select(x => x.VDId).ToList();

                List<DictionaryTable> insertTb = mp.Where(x => !vdids.Contains(x.VDId)).ToList();
                List<DictionaryTable> updateTb = mp.Where(x => vdids.Contains(x.VDId)).ToList();

                if (insertTb != null && insertTb.Any())
                {
                    insertTb.ForEach(x => x.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId());
                    await _dbContext.Insertable(insertTb).ExecuteCommandAsync();
                }
                if (updateTb != null && updateTb.Any())
                {
                    List<DictionaryTable> upTb = new();
                    foreach (var up in updateTb)
                    {
                        var u = dt.FirstOrDefault(x => x.VDId == up.Id);
                        if (u != null)
                        {
                            up.Id = u.Id;
                            upTb.Add(up);
                        }
                    }
                    await _dbContext.Updateable(upTb).WhereColumns(x => x.VDId).ExecuteCommandAsync();
                }
                return Result.Success("操作成功");
            }
            return Result.Success("无数据操作成功");
            #endregion
        }
    }
}
