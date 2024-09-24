﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Domain.Shared.Annotation
{

    /// <summary>
    /// 接口拦截   打上此特性 会进行接口拦截验证
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class InterfaceInterceptAttribute:Attribute
    {
    }
}
