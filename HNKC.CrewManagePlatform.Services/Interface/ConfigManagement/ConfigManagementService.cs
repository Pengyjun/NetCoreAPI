using HNKC.CrewManagePlatform.Models.CommonRequest;
using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos;
using HNKC.CrewManagePlatform.Models.Dtos.ConfigManagement;
using HNKC.CrewManagePlatform.Models.Enums;
using HNKC.CrewManagePlatform.SqlSugars.Models;
using HNKC.CrewManagePlatform.Utils;
using SqlSugar;
using System.ComponentModel;

namespace HNKC.CrewManagePlatform.Services.Interface.ConfigManagement
{
    /// <summary>
    ///配置管理
    /// </summary>
    public class ConfigManagementService : IConfigManagementService
    {
        private readonly ISqlSugarClient _dbContext;
        public ConfigManagementService(ISqlSugarClient dbContext)
        {
            this._dbContext = dbContext;
        }
        /// <summary>
        /// 保存船舶信息
        /// </summary>
        /// <param name="shipRequest"></param>
        /// <returns></returns>
        public async Task<Result> SaveShipAsync(SaveShipRequest shipRequest)
        {
            if (shipRequest.Type == 2)
            {
                var rt = await _dbContext.Queryable<OwnerShip>().FirstAsync(t => t.IsDelete == 1 && t.BusinessId.ToString() == shipRequest.ShipId);
                if (rt != null)
                {
                    rt.ShipName = shipRequest.ShipName;
                    rt.ShipType = shipRequest.ShipType;
                    rt.Company = shipRequest.Company;
                    rt.Country = shipRequest.Country;
                    if (!string.IsNullOrEmpty(shipRequest.Project))
                    {
                        var shirel = await _dbContext.Queryable<ShipProjectRelation>().FirstAsync(t => t.RelationShipId == rt.BusinessId);
                        if (shirel != null)
                        {
                            shirel.ProjectName = shipRequest.Project;
                            await _dbContext.Updateable(shirel).UpdateColumns(x => x.ProjectName).ExecuteCommandAsync();
                        }
                    }
                    await _dbContext.Updateable(rt).ExecuteCommandAsync();
                }
            }
            else
            {
                var bid = GuidUtil.Next();
                var addShip = new OwnerShip()
                {
                    Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    BusinessId = bid,
                    Company = shipRequest.Company,
                    Country = shipRequest.Country,
                    ShipName = shipRequest.ShipName,
                    ShipType = shipRequest.ShipType
                };
                if (!string.IsNullOrEmpty(shipRequest.Project))
                {
                    var addRelShip = new ShipProjectRelation()
                    {
                        Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                        BusinessId = GuidUtil.Next(),
                        ProjectName = shipRequest.Project,
                        RelationShipId = bid
                    };
                    await _dbContext.Insertable(addRelShip).ExecuteCommandAsync();
                }
                await _dbContext.Insertable(addShip).ExecuteCommandAsync();

            }
            return Result.Success();
        }


        /// <summary>
        /// 船舶列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<PageResult<ShipSearch>> SearchShipAsync(ShipRequest requestBody)
        {
            PageResult<ShipSearch> dt = new();
            var rr = await _dbContext.Queryable<OwnerShip>()
                .Where(x => x.IsDelete == 1)
                .LeftJoin<ShipProjectRelation>((x, y) => x.BusinessId == y.RelationShipId)
                .Select((x, y) => new ShipSearch
                {
                    Id = x.BusinessId.ToString(),
                    Company = x.Company,
                    Country = x.Country,
                    ShipId = x.BusinessId.ToString(),
                    ShipName = x.ShipName,
                    ShipType = x.ShipType,
                    ProjectName = y.ProjectName
                })
                .ToListAsync();

            var countrys = await _dbContext.Queryable<CountryRegion>().Where(t => rr.Select(x => x.Country).ToList().Contains(t.BusinessId.ToString())).ToListAsync();
            var ins = await _dbContext.Queryable<Institution>().Where(t => rr.Select(x => x.Company).ToList().Contains(t.BusinessId.ToString())).ToListAsync();

            foreach (var item in rr)
            {
                item.CompanyName = ins.FirstOrDefault(x => x.BusinessId.ToString() == item.Company)?.Name;
                item.CountryName = countrys.FirstOrDefault(x => x.BusinessId.ToString() == item.Country)?.Name;
                item.ShipTypeName = EnumUtil.GetDescription(item.ShipType);
            }
            dt.List = rr;
            dt.TotalCount = rr.Count;
            return dt;
        }

        /// <summary>
        /// 提醒配置
        /// </summary>
        /// <returns></returns>
        public async Task<Result> SearchRemindSettingAsync()
        {
            List<RemindSearch> rt = new();
            var rr = await _dbContext.Queryable<RemindSetting>()
                .Where(t => t.IsDelete == 1)
                .ToListAsync();

            var enumConvertList = Enum.GetValues(typeof(CertificatesEnum))
                                                .Cast<CertificatesEnum>()
                                                .Select(x => new DropDownResponse
                                                {
                                                    Key = ((int)x).ToString(),
                                                    Value = GetEnumDescription(x),
                                                    Type = (int)x
                                                })
                                                .ToList();
            enumConvertList.Add(new DropDownResponse
            {
                Key = "1",
                Type = 0,
                Value = "劳务合同"
            });

            foreach (var item in enumConvertList)
            {
                var ex = rr.FirstOrDefault(x => (int)x.Types == Convert.ToInt32(item.Key));
                if (ex != null)
                {
                    rt.Add(new RemindSearch
                    {
                        Id = ex.BusinessId.ToString(),
                        Days = ex.Days,
                        Enable = ex.Enable,
                        RemindType = ex.RemindType,
                        Types = Convert.ToInt32(ex.Types).ToString(),
                        TypesName = item.Value
                    });
                }
                else
                {
                    int type = 0;
                    type = item.Key == "1" ? 1 : type;
                    rt.Add(new RemindSearch
                    {
                        Days = 0,
                        Enable = 0,
                        RemindType = type,
                        Types = Convert.ToInt32(item.Key).ToString(),
                        TypesName = item.Value
                    });
                }

            }

            return Result.Success(rt.OrderBy(x => x.RemindType));
        }
        /// <summary>
        /// 保存提醒配置
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<Result> SaveRemindSettingAsync(RemindRequest requestBody)
        {
            var rt = await _dbContext.Queryable<RemindSetting>().FirstAsync(t => t.IsDelete == 1 && t.BusinessId.ToString() == requestBody.Id);
            if (rt != null)
            {
                rt.Enable = requestBody.Enable;
                rt.Days = requestBody.Days;
                rt.Types = requestBody.Types;
                await _dbContext.Updateable(rt).IgnoreColumns(x => x.RemindType).ExecuteCommandAsync();
                return Result.Success("修改成功");
            }
            else
            {
                var add = new RemindSetting
                {
                    Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    Types = requestBody.Types,
                    RemindType = requestBody.RemindType,
                    BusinessId = GuidUtil.Next(),
                    Days = requestBody.Days,
                    Enable = requestBody.Enable
                };

                await _dbContext.Insertable(add).ExecuteCommandAsync();
                return Result.Success("新增成功");
            }
        }
        /// <summary>
        /// 获取枚举项的描述信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute != null ? attribute.Description : value.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Result> DeleteShipAsync(string id)
        {
            var rt = await _dbContext.Queryable<OwnerShip>().FirstAsync(t => t.BusinessId.ToString() == id && t.IsDelete == 1);
            if (rt != null)
            {
                rt.IsDelete = 0;
                await _dbContext.Updateable(rt).UpdateColumns(x => x.IsDelete).ExecuteCommandAsync();
                return Result.Success("删除成功");
            }
            return Result.Success("删除失败");
        }
    }
}
