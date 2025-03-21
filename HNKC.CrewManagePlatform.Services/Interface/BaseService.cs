﻿using AutoMapper;
using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.Common;
using HNKC.CrewManagePlatform.Models.Dtos.CrewArchives;
using HNKC.CrewManagePlatform.Models.Dtos.Salary;
using HNKC.CrewManagePlatform.Models.Enums;
using HNKC.CrewManagePlatform.SqlSugars.Models;
using HNKC.CrewManagePlatform.Util;
using HNKC.CrewManagePlatform.Utils;
using MiniExcelLibs;
using SqlSugar;
using SqlSugar.Extensions;
using System.Globalization;
namespace HNKC.CrewManagePlatform.Services.Interface
{

    /// <summary>
    /// 基本业务实现层
    /// </summary>
    public class BaseService : HNKC.CrewManagePlatform.Services.Interface.CurrentUser.CurrentUserService, IBaseService
    {
        #region 依赖注入
        private readonly ISqlSugarClient _dbContext;
        private readonly IMapper mapper;
        public BaseService(ISqlSugarClient dbContext, IMapper mapper)
        {
            this._dbContext = dbContext;
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
            if (res.Where(x => string.IsNullOrWhiteSpace(x.WorkNumber)).Count() > 0)
            {
                return Result.Fail("职工号存在为空的情况请检查");
            }

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
            var salaryMonthList = await _dbContext.Queryable<HNKC.CrewManagePlatform.SqlSugars.Models.Salary>().Where(x => x.IsDelete == 1 && x.DataMonth == dataMonth).ToListAsync();
            foreach (var item in salaryMonthList)
            {
                item.IsDelete = 0;
            }
            await _dbContext.Updateable<HNKC.CrewManagePlatform.SqlSugars.Models.Salary>(salaryMonthList).ExecuteCommandAsync();
            #endregion

            var salaryList = mapper.Map<List<SalaryAsExcelResponse>, List<HNKC.CrewManagePlatform.SqlSugars.Models.Salary>>(res);
            if (salaryList.Any())
            {
                var userList = await _dbContext.Queryable<User>().Where(x => x.IsDelete == 1 && !SqlFunc.IsNullOrEmpty(x.WorkNumber)).ToListAsync();
                foreach (var item in salaryList)
                {
                    item.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                    var userInfo = userList.Where(x => x.WorkNumber == item.WorkNumber).FirstOrDefault();
                    if (userInfo != null)
                    {
                        item.UserId = userInfo.Id;
                    }
                    item.BusinessId = GuidUtil.Next();
                    item.DataSource = (int)DataSourceEnum.Import;
                    item.Year = item.DataMonth.ToString().Substring(0, 4).ObjToInt();
                    item.Month = item.DataMonth.ToString().Substring(4, 2).ObjToInt();
                }
                await _dbContext.Insertable<HNKC.CrewManagePlatform.SqlSugars.Models.Salary>(salaryList).ExecuteCommandAsync();
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
            var addInstitution = await _dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1 && (x.Oid == "101162350" || x.Oid == "101162354"))
                 .Select(x => new InstitutionTree()
                 {
                     GPoid = x.GPoid,
                     Name = x.Name,
                     POid = x.Poid,
                     ShortName = x.ShortName,
                     Oid = x.Oid,
                     Sno = x.Sno,
                     Grule = x.Grule,
                     BusinessId = x.BusinessId
                 })
                .ToListAsync();

            var allInstitution = await _dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1 && x.Grule.Contains("101341960"))
                 .Select(x => new InstitutionTree()
                 {
                     GPoid = x.GPoid,
                     Name = x.Name,
                     POid = x.Poid,
                     ShortName = x.ShortName,
                     Oid = x.Oid,
                     Sno = x.Sno,
                     Grule = x.Grule,
                     BusinessId = x.BusinessId
                 })
                .ToListAsync();
            allInstitution.AddRange(addInstitution);
            //获取树形
            var instrturionTree = new ListToTreeUtil().GetTree(userInfo.Oid, allInstitution);
            if (instrturionTree.Count == 0)
            {
                //添加自身节点
                var salfNode = allInstitution.Where(x => x.Oid == userInfo.Oid).FirstOrDefault();
                instrturionTree.Add(salfNode);
            }
            InstitutionTree rootNode = null;
            InstitutionTree currentNode = null;
            if (instrturionTree.Count != 0)
            {
                var grule = instrturionTree.First().Grule;
                var arr = grule.Split("-", StringSplitOptions.RemoveEmptyEntries);
                for (int i = 4; i < arr.Length - 1; i++)
                {
                    if (i == 4 && rootNode == null)
                    {
                        rootNode = allInstitution.First(x => x.Oid == arr[i]);
                    }
                    else
                    {
                        var childNode = allInstitution.First(x => x.Oid == arr[i]);
                        if (currentNode == null)
                        {
                            currentNode = childNode;
                            rootNode.Nodes.Add(currentNode);
                        }
                        else
                        {
                            currentNode.Nodes.Add(childNode);
                            currentNode = childNode;
                        }

                    }

                    if (i == (arr.Length - 2))
                    {
                        if (currentNode == null)
                        {
                            currentNode = rootNode;
                        }
                        currentNode.Nodes = instrturionTree;
                    }
                }
            }
            return Result.Success(data: rootNode, "响应成功");
            #endregion

        }

        /// <summary>
        /// 添加机构树
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<Result> AddInstitutionTreeAsync(AddInstutionRequestDto requestDto)
        {
            //修改机构树名称
            if (!string.IsNullOrWhiteSpace(requestDto.Oid))
            {
                var instutionInfo = await _dbContext.Queryable<Institution>().Where(t => t.IsDelete == 1 && t.Oid == requestDto.Oid).FirstAsync();
                instutionInfo.Name = requestDto.Name;
                instutionInfo.ShortName = requestDto.Name;
                await _dbContext.Updateable(instutionInfo).ExecuteCommandAsync();
                return Result.Success("修改成功");
            }
            else
            {
                //查询对应部门下序号最大的
                var instutionInfo = await _dbContext.Queryable<Institution>().Where(t => t.IsDelete == 1 && t.Grule.Contains(requestDto.Poid) && t.Ocode.Contains("000S")).OrderByDescending(t => t.Sno + 1).FirstAsync();
                Institution model = new Institution();
                model = instutionInfo;
                model.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                var sno = Convert.ToInt32(model.Oid?.Substring(model.Oid.Length - 2)) + 1;
                var str = model.Oid?.Substring(0, model.Oid.Length - 2);
                model.Oid = str + sno;
                model.O2bid = model.BizType + model.Oid;
                model.Orule = ReplaceStr(model.Orule, model.Oid, 6);
                model.Grule = ReplaceStr(model.Grule, model.Oid, 10);
                model.Mrut = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                model.Sno = (30 + 1).ToString();
                model.Name = requestDto.Name;
                model.ShortName = requestDto.Name;
                string fixedPrefix = "31102139P2023005";
                int prefixLength = fixedPrefix.Length;
                // 从目标字符串的固定部分后开始，找到需要替换的内容
                if (model.Ocode.Length > prefixLength)
                {
                    string partToReplace = model.Ocode.Substring(prefixLength, 2);  // 取固定后面的 2 个字符
                    model.Ocode = model.Ocode.Substring(0, prefixLength) + model.Sno + model.Ocode.Substring(prefixLength + 2);
                }
                model.StartDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                model.Global_Sno = ReplaceStr(model.Global_Sno, model.Sno, 11);
                model.Version = string.Empty;
                model.BusinessId = GuidUtil.Next();

                await _dbContext.Insertable(model).ExecuteCommandAsync();
                return Result.Success("新增成功");
            }
        }

        /// <summary>
        /// 内容替换
        /// </summary>
        /// <returns></returns>
        public string ReplaceStr(string? input, string newValue, int index)
        {
            int targetIndex = index;
            if (input == null)
            {
                return string.Empty;
            }
            // 使用分隔符"-"拆分字符串
            string[] parts = input.Split('-');
            // 判断目标索引位置是否合法
            if (targetIndex >= 0 && targetIndex < parts.Length)
            {
                // 替换目标位置的内容
                parts[targetIndex] = newValue;
            }
            // 拼接回新的字符串
            string output = string.Join("-", parts);
            return output;
        }


        #region 通过身份证与当前日期计算年龄
        /// <summary>
        /// 通过身份证与当前日期计算年龄
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        public int CalculateAgeFromIdCard(string? idCard)
        {
            if (idCard?.Length != 18)
            {
                return 0;
            }

            // 提取出生日期（身份证的前 6 位是出生年月日，格式为yyyyMMdd）
            string birthDateString = idCard.Substring(6, 8);

            DateTime birthDate = DateTime.ParseExact(birthDateString, "yyyyMMdd", CultureInfo.InvariantCulture);

            DateTime currentDate = DateTime.Now;

            // 计算年龄
            int age = currentDate.Year - birthDate.Year;

            // 如果当前日期的月份和日子还没到出生日期的月份和日子，就减去 1 年
            if (currentDate.Month < birthDate.Month || (currentDate.Month == birthDate.Month && currentDate.Day < birthDate.Day))
            {
                age--;
            }

            return age;
        }
        /// <summary>
        /// 船员状态
        /// </summary>
        /// <param name="StartTime">上船时间</param>
        /// <param name="EndTime">下船时间</param>
        /// <param name="deleteResonEnum">是否删除</param>
        /// <returns></returns>
        public CrewStatusEnum ShipUserStatus(DateTime? StartTime, DateTime? EndTime, CrewStatusEnum deleteResonEnum)
        {
            var status = new CrewStatusEnum();
            if (deleteResonEnum != CrewStatusEnum.Normal)
            {
                //删除：管理人员手动操作，包括离职、调离和退休，优先于其他任何状态
                status = deleteResonEnum;
            }
            //else if (holidayTime.HasValue)
            //{
            //    if (holidayTime < DateTime.Now)
            //    {
            //        //休假：提交离船申请且经审批同意后，按所申请离船、归船日期设置为休假状态，优先于在岗、待岗状态
            //        status = CrewStatusEnum.XiuJia;
            //    }
            //}
            else if (EndTime != null && EndTime <= DateTime.Now)
            {
                //在岗、待岗:船员登记时必填任职船舶数据，看其中最新的任职船舶上船时间和下船时间，在此时间内为在岗状态，否则为待岗状态
                status = CrewStatusEnum.XiuJia;
            }
            else
            {
                if (StartTime > DateTime.Now)
                {
                    status = CrewStatusEnum.XiuJia;
                }
            }
            return status;
        }
        #endregion

        #region 保存文件
        /// <summary>
        /// 新增文件
        /// </summary>
        /// <param name="uploadResponse"></param>
        /// <param name="uId"></param>
        /// <returns></returns>
        public async Task<Result> InsertFileAsync(List<UploadResponse> uploadResponse, Guid? uId)
        {

            if (uploadResponse != null && uploadResponse.Any())
            {
                List<Files> files = new();
                var fileId = GuidUtil.Next();
                foreach (var item in uploadResponse)
                {
                    files.Add(new Files
                    {
                        Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                        BusinessId = item.BId,
                        FileSize = item.FileSize,
                        FileType = item.FileType,
                        Name = item.Name,
                        OriginName = item.OriginName,
                        SuffixName = item.SuffixName,
                        FileId = item.FileId,
                        UserId = uId
                    });
                }
                if (files.Any()) await _dbContext.Insertable(files).ExecuteCommandAsync();
                return Result.Success();
            }
            else { return Result.Fail("文件保存失败"); }
        }
        /// <summary>
        /// 修改文件
        /// </summary>
        /// <param name="uploadResponse"></param>
        /// <param name="uId"></param>
        /// <returns></returns>
        public async Task<Result> UpdateFileAsync(List<UploadResponse> uploadResponse, Guid? uId)
        {
            if (uploadResponse != null && uploadResponse.Any())
            {
                var bids = uploadResponse.Select(x => x.FileId).ToList();
                //删除原有文件
                var oldFiles = await _dbContext.Queryable<Files>().Where(t => t.IsDelete == 1 && uId == t.UserId).ToListAsync();
                if (oldFiles.Any()) await _dbContext.Deleteable(oldFiles).ExecuteCommandAsync();
                //重新新增文件
                return await InsertFileAsync(uploadResponse, uId);
            }
            else { return Result.Fail("文件保存失败"); }

        }
        #endregion

        #region 获取当前角色

        public async Task<int> CurRoleType()
        {
            //是管理员不做任何处理
            if (!GlobalCurrentUser.IsAdmin)
            {
                if (GlobalCurrentUser.Type != 3 && GlobalCurrentUser.Type != 1 && GlobalCurrentUser.Type != 4) return -1;//4船员部 3船长 1管理员  否则-1
                else return GlobalCurrentUser.Type.Value;
            }
            else return 0;
        }
        #endregion
    }
}
