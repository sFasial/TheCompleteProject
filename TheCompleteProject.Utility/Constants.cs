using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCompleteProject.Utility
{
    public static class Constants
    {
        public const string DBConnectionName = "Connection";
        public const string ResponseMessageField = "Message";
        public const string ResponseDataField = "Data";
        public const string ResponseFailureField = "Failure";
        public const string ResponseErrorField = "ModelStateFailure";
        public const string UnAuthorizedResponseField = "UnAuthorizedData";
        public const string JwtTokenClaimKey = "User";
    }
}
