using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos;
using HNKC.CrewManagePlatform.Models.Dtos.Salary;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Services.Interface.Salary
{
    /// <summary>
    /// 工资列表接口
    /// </summary>
    public interface ISalaryService
    {
        /// <summary>
        /// 工资列表
        /// </summary>
        /// <param name="salaryRequest"></param>
        /// <returns></returns>
        Task<PageResult<SalaryResponse>> SearchSalaryListAsync(SalaryRequest salaryRequest);

        /// <summary>
        ///查询所有用户的工资记录
        /// </summary>
        /// <param name="salaryPushRequest"></param>
        /// <returns></returns>
        Task<PageResult<SalaryPushResponse>> SearchSalaryPushRecordAsync(SalaryPushRequest  salaryPushRequest);
        /// <summary>
        /// 查询单个用户的工资记录
        /// </summary>
        /// <param name="baseRequest"></param>
        /// <returns></returns>
        Task<PageResult<SalaryPushResponse>> GetSalaryPushRecordByUserAsync(SalaryPushRequest salaryPushRequest);

        /// <summary>
        /// 移动端 点击链接查询
        /// </summary>
        /// <param name="baseRequest"></param>
        /// <returns></returns>
        Task<SalaryAsExcelResponse> FindUserInfoAsync(string sign);

        /// <summary>
        ///参数为空即 群发  否则单发
        /// </summary>
        /// <returns></returns>
        Task<Result> SendSmsAllAsync(BaseRequest baseRequest);

        /// <summary>
        /// 发短信
        /// </summary>
        /// <returns></returns>

        Task<Result> SendSmsAsync();
    }
}
