using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.Contract;
using HNKC.CrewManagePlatform.SqlSugars.Models;
using HNKC.CrewManagePlatform.Utils;
using SqlSugar;

namespace HNKC.CrewManagePlatform.Services.Interface.Contract
{
    /// <summary>
    /// 合同实现层
    /// </summary>
    public class ContractService : IContractService
    {
        private readonly ISqlSugarClient _dbContext;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public ContractService(ISqlSugarClient dbContext)
        {
            this._dbContext = dbContext;
        }
        /// <summary>
        /// 合同列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<PageResult<ContractSearch>> SearchContractAsync(ContractRequest requestBody)
        {
            PageResult<ContractSearch> rr = new();
            RefAsync<int> total = 0;

            var uentityFist = _dbContext.Queryable<UserEntryInfo>()
                .GroupBy(u => u.UserEntryId)
                .Select(x => new { x.UserEntryId, EndTime = SqlFunc.AggregateMax(x.EndTime) });
            var uentity = _dbContext.Queryable<UserEntryInfo>()
                .InnerJoin(uentityFist, (x, y) => x.UserEntryId == y.UserEntryId && x.EndTime == y.EndTime);


            var data = _dbContext.Queryable<User>()
                .InnerJoin(uentity, (t1, t2) => t1.BusinessId == t2.UserEntryId)
                //.LeftJoin<OwnerShip>((u, os) => u.OnBoard == os.BusinessId.ToString())
                //.LeftJoin<CountryRegion>((u, os, cr) => os.Country == cr.BusinessId.ToString())
                //.Select((u, os, cr) => new ContractSearch
                .Select((t1, t2) => new ContractSearch
                {
                    UserName = SqlFunc.Subqueryable<SkillCertificates>()
                               .Where(z => t1.BusinessId == z.SkillcertificateId)
                               .SelectStringJoin(z => z.SkillCertificateType.ToString(), ","),
                    Id = t1.BusinessId.ToString(),
                    //Country = os.Country,
                    //ShipType = os.ShipType,
                    //UserName = u.Name,
                    WorkNumber = t1.WorkNumber,
                    EndTime = t2.EndTime.ToString("yyyy/MM/dd"),
                    EntryTime = t2.EntryTime.ToString("yyyy/MM/dd"),
                })
                .MergeTable()
                .Where(t => SqlFunc.SplitIn(t.UserName, "1"))
                .ToList();
            rr.List = data;
            return rr;
            //return await GetResultAsync(rr, total);
        }
        ///// <summary>
        ///// 获取查询结果集
        ///// </summary>
        ///// <param name="rr"></param>
        ///// <param name="total"></param>
        ///// <returns></returns>
        //private async Task<PageResult<ContractSearch>> GetResultAsync(List<ContractSearch> rr, int total)
        //{
        //    PageResult<ContractSearch> rt = new();



        //    //var uIds = rr.Select(x => x.Id).ToList();
        //    ////最新的合同
        //    //var uiTable = await _dbContext.Queryable<UserEntryInfo>()
        //    //    .Where(t => uIds.Contains(t.UserEntryId.ToString()))
        //    //    .ToListAsync();
        //    ////用工类型
        //    //var empTable = await _dbContext.Queryable<EmploymentType>().ToListAsync();
        //    //var gbrr = uiTable.GroupBy(i => i.UserEntryId, (x, y) => y.OrderByDescending(od => od.EndTime).First()).ToList();
        //    //foreach (var u in rr)
        //    //{
        //    //    var ut = gbrr.FirstOrDefault(x => x.UserEntryId.ToString() == u.Id);
        //    //    if (ut != null)
        //    //    {
        //    //        u.ContractMain = ut.ContractMain;
        //    //        u.ContractType = ut.ContractType;
        //    //        u.ContractTypeName = EnumUtil.GetDescription(ut.ContractType);
        //    //        u.EmploymentType = ut.EmploymentId;
        //    //        u.EmploymentTypeName = empTable.FirstOrDefault(x => x.BusinessId.ToString() == ut.EmploymentId)?.Name;
        //    //        u.LaborCompany = ut.LaborCompany;
        //    //        u.EndTime = ut.EndTime.ToString("yyyy/MM/dd");
        //    //        u.EntryTime = ut.EntryTime.ToString("yyyy/MM/dd");
        //    //    }
        //    //}
        //    //rt.List = rr;
        //    return rt;
        //}
    }
}
