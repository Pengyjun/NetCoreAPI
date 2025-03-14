using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.Common;
using HNKC.CrewManagePlatform.SqlSugars.Models;
using HNKC.CrewManagePlatform.Utils;
using SqlSugar;

namespace HNKC.CrewManagePlatform.Services.Interface.Common
{
    /// <summary>
    /// 公共服务业务接口实现
    /// </summary>
    public class CommonService : ICommonService
    {
        private readonly ISqlSugarClient _dbContext;

        /// <summary>
        /// 注入db
        /// </summary>
        /// <param name="dbContext"></param>
        public CommonService(ISqlSugarClient dbContext)
        {
            this._dbContext = dbContext;
        }

        /// <summary>
        /// 获取船舶列表
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PageResult<OwnerShipVo>> GetShipList(OwnerShipDto dto)
        {
            RefAsync<int> total = 0;
            PageResult<OwnerShipVo> dt = new();
            var rr = await _dbContext.Queryable<OwnerShip>()
                .Where(x => x.IsDelete == 1)
                .LeftJoin<ShipProjectRelation>((x, y) => x.BusinessId == y.RelationShipId)
                .WhereIF(!string.IsNullOrEmpty(dto.ShipId), (x, y) => x.BusinessId.ToString() == dto.ShipId)
                .WhereIF(!string.IsNullOrEmpty(dto.Company.ToString()), (x, y) => x.Company == dto.Company)
                .WhereIF(!string.IsNullOrEmpty(dto.Country.ToString()), (x, y) => x.Country == dto.Country)
                .WhereIF(!string.IsNullOrEmpty(dto.ShipType.ToString()), (x, y) => x.ShipType == dto.ShipType)
                .WhereIF(!string.IsNullOrEmpty(dto.ShipName),(x,y)=> x.ShipName.Contains(dto.ShipName))
                .Select((x, y) => new OwnerShipVo
                {
                    Id = x.BusinessId.ToString(),
                    Company = x.Company,
                    Country = x.Country,
                    ShipId = x.BusinessId.ToString(),
                    ShipName = x.ShipName,
                    ShipType = x.ShipType,
                    ProjectName = y.ProjectName
                })
                .ToPageListAsync(dto.PageIndex, dto.PageSize, total);

            var countrys = await _dbContext.Queryable<CountryRegion>().Where(t => rr.Select(x => x.Country).ToList().Contains(t.BusinessId)).ToListAsync();
            var ins = await _dbContext.Queryable<Institution>().Where(t => rr.Select(x => x.Company).ToList().Contains(t.BusinessId)).ToListAsync();

            foreach (var item in rr)
            {
                item.CompanyName = ins.FirstOrDefault(x => x.BusinessId == item.Company)?.Name;
                item.CountryName = countrys.FirstOrDefault(x => x.BusinessId == item.Country)?.Name;
                item.ShipTypeName = EnumUtil.GetDescription(item.ShipType);
            }
            dt.List = rr;
            dt.TotalCount = total;
            return dt;
        }
    }
}
