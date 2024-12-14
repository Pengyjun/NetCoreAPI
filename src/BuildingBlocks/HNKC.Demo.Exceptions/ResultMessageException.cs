using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

<<<<<<<< HEAD:src/BuildingBlocks/HNKC.CrewManagePlatform.Exceptions/ResultMessageException.cs
namespace HNKC.CrewManagePlatform.Exceptions
========
namespace HNKC.Demo.Exceptions
>>>>>>>> 7fd224848dc4910963de00d8c3a15a3418dc1847:src/BuildingBlocks/HNKC.Demo.Exceptions/ResultMessageException.cs
{
    public class ResultMessageException : Exception
    {
        public ResultMessageException()
        { }

        public ResultMessageException(string message)
            : base(message)
        { }

        public ResultMessageException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
