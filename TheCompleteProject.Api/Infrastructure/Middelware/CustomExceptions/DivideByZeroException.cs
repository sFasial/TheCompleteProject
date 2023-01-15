using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheCompleteProject.Api.Infrastructure.Middelware.CustomExceptions
{
    [Serializable]
    public class DivideByZeroException : Exception
    {
        public DivideByZeroException()
        {

        }

        public DivideByZeroException(string message) : base(message)
        {

        }
        public DivideByZeroException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
