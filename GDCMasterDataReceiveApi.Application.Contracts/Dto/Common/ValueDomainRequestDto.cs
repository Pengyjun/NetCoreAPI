using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.Common
{
    /// <summary>
    ///值域问题
    /// </summary>
    public class ValueDomainRequestDto
    {

        public string ZZSERIAL { get; set; }
        public string ZDOM_CODE { get; set; }
        public string ZDOM_DESC { get; set; }
        public string ZDOM_VALUE { get; set; }
        public string ZDOM_NAME { get; set; }
        public string ZDOM_LEVEL { get; set; }
        public string ZDOM_SUP { get; set; }
        public string ZREMARKS { get; set; }

        public string ZCHTIME { get; set; }
        public string ZVERSION { get; set; }
        public string ZSTATE { get; set; }
        public string ZDELETE { get; set; }
        public string ZLANG_LIST { get; set; }



    }
}
