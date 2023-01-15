using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheCompleteProject.Api.Infrastructure.Middelware.CustomExceptions
{
    [Serializable]
    public class BadResultException : Exception
    {
        public BadResultException()
        {

        }

        public BadResultException(string message) : base(message)
        {

        }
        public BadResultException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
