using AutoMapper;
using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.Role;
using HNKC.CrewManagePlatform.Models.Dtos.Salary;
using HNKC.CrewManagePlatform.Models.Enums;
using HNKC.CrewManagePlatform.SqlSugars.Models;
using HNKC.CrewManagePlatform.Util;
using HNKC.CrewManagePlatform.Utils;
using MiniExcelLibs;
using SqlSugar;
using SqlSugar.Extensions;
namespace HNKC.CrewManagePlatform.Services.Interface
{

    /// <summary>
    /// 基本业务实现层
    /// </summary>
    public class BaseService : HNKC.CrewManagePlatform.Services.Interface.CurrentUser.CurrentUserService, IBaseService
    {
        #region 依赖注入
        private readonly ISqlSugarClient dbContext;
        private readonly IMapper mapper;
        public BaseService(ISqlSugarClient dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        #endregion
        /// <summary>
        /// 读取excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="useHead"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Result> ReadExcelAsModelAsync(Stream stream)
        {
            var readResult = await MiniExcel.QueryAsync<SalaryAsExcelResponse>(stream);
            List<SalaryAsExcelResponse> res = readResult.ToList();
            #region 数据验证
            //数据验证
            var num = res.Where(x => x.DataMonth.ToString().Length != 6).Count();
            if (num > 0)
            {
                return Result.Fail("数据日期有误请检查");
            }

            num = res.Select(x => x.DataMonth).Distinct().Count();
            if (num > 1)
            {
                return Result.Fail("存在多个月的数据");
            }
            #endregion

            #region 删除就数据
            //导入的日期
            var dataMonth = res.Select(x => x.DataMonth).Distinct().FirstOrDefault();
            var salaryMonthList = await dbContext.Queryable<HNKC.CrewManagePlatform.SqlSugars.Models.Salary>().Where(x => x.IsDelete == 1 && x.DataMonth == dataMonth).ToListAsync();
            foreach (var item in salaryMonthList)
            {
                item.IsDelete = 0;
            }
            await dbContext.Updateable<HNKC.CrewManagePlatform.SqlSugars.Models.Salary>(salaryMonthList).ExecuteCommandAsync();
            #endregion

            var salaryList = mapper.Map<List<SalaryAsExcelResponse>, List<HNKC.CrewManagePlatform.SqlSugars.Models.Salary>>(res);
            if (salaryList.Any())
            {
                var userList = await dbContext.Queryable<User>().Where(x => x.IsDelete == 1).ToListAsync();
                foreach (var item in salaryList)
                {
                    item.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                    var userInfo = userList.Where(x => x.WorkNumber == item.WorkNumber).FirstOrDefault();
                    if (userInfo != null)
                    {
                        item.UserId = userInfo.Id;
                    }
                    item.DataSource = (int)DataSourceEnum.Import;
                    item.Year = item.DataMonth.ToString().Substring(0, 4).ObjToInt();
                    item.Month = item.DataMonth.ToString().Substring(4, 2).ObjToInt();
                }
                await dbContext.Insertable<HNKC.CrewManagePlatform.SqlSugars.Models.Salary>(salaryList).ExecuteCommandAsync();
            }
            return Result.Success("导入成功");
        }


        /// <summary>
        /// 机构树
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Result> SearchInstitutionTreeAsync()
        {
            var userInfo = GlobalCurrentUser;
            #region 获取当前节点下的所有子节点
            var allInstitution = await dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1)
                 .Select(x => new InstitutionTree()
                 {
                     GPoid = x.GPoid,
                     Name = x.Name,
                     POid = x.Poid,
                     ShortName = x.ShortName,
                     Oid = x.Oid,
                     Sno = x.Sno,
                     BusinessId = x.BusinessId
                 })
                .ToListAsync();
            var instrturionTree = new ListToTreeUtil().GetTree(userInfo.Oid, allInstitution);
            var rootNode = allInstitution.Where(x => x.Oid == "101162350").FirstOrDefault();
            rootNode.Nodes = instrturionTree;
            return Result.Success(data: rootNode, "响应成功");
            #endregion


        }


    }
}
