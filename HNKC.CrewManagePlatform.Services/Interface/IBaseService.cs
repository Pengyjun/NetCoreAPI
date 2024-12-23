using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.Salary;
using HNKC.CrewManagePlatform.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Services.Interface
{

    /// <summary>
    /// 基本业务层
    /// </summary>
    public interface IBaseService
    {
        /// <summary>
        /// 读取excel
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Task<Result> ReadExcelAsModelAsync(Stream stream);



        /// <summary>
        /// 搜索机构数
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<InstitutionTree>> SearchInstitutionTreeAsync();
    }
}
