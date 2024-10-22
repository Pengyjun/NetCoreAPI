using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.GovernanceData
{
    public class DbQueryResponseDto
    {
        /// <summary>
        /// 自己表的主键ID
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 调用接口数据 外部传过来的ID
        /// </summary>
        public string? PId { get; set; }
    }
}
