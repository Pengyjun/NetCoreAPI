using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CH.Simple.Exceptions
{
    public class SimpleException : Exception
    {
        public SimpleException()
        { }

        public SimpleException(string message)
            : base(message)
        { }

        public SimpleException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
