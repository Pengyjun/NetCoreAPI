using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Web.ActionResults

{
    public class OkMessageObjectResult : ObjectResult
    {
        public OkMessageObjectResult(object value) : base(value)
        {
            StatusCode = 200;
        }
    }
}
