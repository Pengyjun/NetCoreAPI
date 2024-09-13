using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Application.Contracts.IService.ISearchService;
using GDCMasterDataReceiveApi.Domain.Models;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Utils;
using SqlSugar;

namespace GDCMasterDataReceiveApi.Application.Service.SearchService
{
    /// <summary>
    /// 统计数据列表实现
    /// </summary>
    public class IncrementalDataSearchService : IIncrementalDataSearchService
    {
        private readonly ISqlSugarClient _dbContext;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public IncrementalDataSearchService(ISqlSugarClient dbContext)
        {
            this._dbContext = dbContext;
        }
        /// <summary>
        /// 公用值
        /// </summary>
        public class TResult
        {
            public long Id { get; set; }
            public DateTime? CreateTime { get; set; }
            public DateTime? UpdateTime { get; set; }
        }

        #region 列表接口
        /// <summary>
        /// 获取每日增量数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<IncrementalDataDto>> GetIncrementalSearchAsync(IncrementalSearchRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<IncrementalDataDto>();
            var result = new IncrementalDataDto();
            var item = new List<IncrementalSearchResponse>();

            requestDto.ResetModelProperty();
            Utils.TryConvertDateTimeFromDateDay(requestDto.DateDay, out DateTime time);

            //前六+当日  七天
            var sevDay = time.AddDays(-6).ToDateDay();
            Utils.TryConvertDateTimeFromDateDay(sevDay, out DateTime sevTime);

            List<TResult> insertList = new();
            List<TResult> updateList = new();

            switch (requestDto.TableName)
            {
                case Domain.Shared.Enums.TableNameType.User:
                    // 获取新增的数据
                    insertList = await _dbContext.Queryable<User>()
                                        .Where(t => !string.IsNullOrWhiteSpace(t.CreateTime.ToString()) && t.IsDelete == 1 && t.CreateTime.Value.Date >= sevTime && t.CreateTime.Value.Date <= time)
                                        .Select(x => new TResult { CreateTime = x.CreateTime, Id = x.Id })
                                        .ToListAsync();
                    //获取修改的数据
                    updateList = await _dbContext.Queryable<User>()
                       .Where(t => !string.IsNullOrWhiteSpace(t.UpdateTime.ToString()) && t.IsDelete == 1 && t.UpdateTime.Value.Date >= sevTime && t.UpdateTime.Value.Date <= time)
                       .Select(x => new TResult { UpdateTime = x.UpdateTime, Id = x.Id })
                       .ToListAsync();
                    break;
                case Domain.Shared.Enums.TableNameType.Institution:
                    insertList = await _dbContext.Queryable<Institution>()
                                        .Where(t => !string.IsNullOrWhiteSpace(t.CreateTime.ToString()) && t.IsDelete == 1 && t.CreateTime.Value.Date >= sevTime && t.CreateTime.Value.Date <= time)
                                        .Select(x => new TResult { CreateTime = x.CreateTime, Id = x.Id })
                                        .ToListAsync();
                    //获取修改的数据
                    updateList = await _dbContext.Queryable<Institution>()
                       .Where(t => !string.IsNullOrWhiteSpace(t.UpdateTime.ToString()) && t.IsDelete == 1 && t.UpdateTime.Value.Date >= sevTime && t.UpdateTime.Value.Date <= time)
                       .Select(x => new TResult { UpdateTime = x.UpdateTime, Id = x.Id })
                       .ToListAsync();
                    break;
            }

            for (int i = 0; i < 7; i++)
            {
                //初始化
                List<string> incDetails = new();

                //数据
                var tabInsertList = insertList.Where(x => x.CreateTime.Value.Date == sevTime).Select(x => x.Id.ToString()).ToList();
                var tabUpdateList = updateList.Where(x => x.UpdateTime.Value.Date == sevTime).Select(x => x.Id.ToString()).ToList();
                incDetails.AddRange(tabInsertList);
                incDetails.AddRange(tabUpdateList);

                //创建
                item.Add(new IncrementalSearchResponse
                {
                    DetailsIds = incDetails,
                    ChangeNums = tabInsertList.Count + tabUpdateList.Count,
                    TimeValue = sevTime.ToString("yyyy-MM-dd")
                });
                sevTime = sevTime.AddDays(1);
            }

            result = new IncrementalDataDto
            {
                Item = item
            };

            responseAjaxResult.SuccessResult(result);
            return responseAjaxResult;
        }
        #endregion
    }
}
