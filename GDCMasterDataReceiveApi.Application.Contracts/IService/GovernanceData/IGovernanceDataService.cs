using GDCMasterDataReceiveApi.Domain.Shared.Annotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Application.Contracts.IService.GovernanceData
{


    /// <summary>
    /// 数据治理
    /// </summary>
    [DependencyInjection]
    public interface IGovernanceDataService
    {

        /// <summary>
        /// 治理数据  1是金融机构  2是物资明细编码  3 是往来单位数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<bool> GovernanceDataAsync(int type = 1);
    }
}
