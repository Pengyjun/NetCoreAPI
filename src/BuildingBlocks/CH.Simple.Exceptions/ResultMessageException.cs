using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CH.Simple.Exceptions
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
