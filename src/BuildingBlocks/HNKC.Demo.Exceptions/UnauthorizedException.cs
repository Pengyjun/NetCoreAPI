using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

<<<<<<<< HEAD:src/BuildingBlocks/HNKC.CrewManagePlatform.Exceptions/UnauthorizedException.cs
namespace HNKC.CrewManagePlatform.Exceptions
========
namespace HNKC.Demo.Exceptions
>>>>>>>> 7fd224848dc4910963de00d8c3a15a3418dc1847:src/BuildingBlocks/HNKC.Demo.Exceptions/UnauthorizedException.cs
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException()
        { }

        public UnauthorizedException(string message)
            : base(message)
        { }

        public UnauthorizedException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
