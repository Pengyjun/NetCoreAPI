using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CH.Simple.Web.Modles
{
    public class OkMsgObjectResult : ObjectResult
    {
        public OkMsgObjectResult(object value) : base(value)
        {
            StatusCode = 200;
        }
    }
}
