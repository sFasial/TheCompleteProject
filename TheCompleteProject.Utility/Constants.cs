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

        public const string ConnectionString = "Data Source=192.168.49.15; Initial Catalog=ESOP_Dev_New; User ID-SUP_USR_AD; Password-Intime@12345; Encryp public const ";

        public const string OLEDB_ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source =#fullExcelPath#; Extended Properties='Excel 12.0 Xml";

    }

    public  class ConstantsEnviormentTest
    {
        public const string Connection = "Connection";
        public const string MyMachineEnviorment = "MyMachineEnviorment";
        public const string MyMachineServer = "MyMachineServer";
        public string MyProperty { get; set; } = "Hahahahah Testing ";

        public const string ResponseMessageField = "Message";
        public const string ResponseDataField = "Data";
        public const string ResponseFailureField = "Failure";
        public const string ResponseErrorField = "ModelStateFailure";
        public const string UnAuthorizedResponseField = "UnAuthorizedData";
        public const string JwtTokenClaimKey = "User";

        public const string ConnectionString = "Data Source=192.168.49.15; Initial Catalog=ESOP_Dev_New; User ID-SUP_USR_AD; Password-Intime@12345; Encryp public const ";

        public const string OLEDB_ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source =#fullExcelPath#; Extended Properties='Excel 12.0 Xml";

    }
}
