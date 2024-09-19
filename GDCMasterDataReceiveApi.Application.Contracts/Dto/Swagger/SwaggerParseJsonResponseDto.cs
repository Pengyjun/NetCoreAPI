using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.Swagger
{
    /// <summary>
    /// 解析swagger json
    /// </summary>
    public class SwaggerParseJsonResponseDto
    {

        public string? Paths { get; set; }
        public string? Components { get; set; }
    }
}
