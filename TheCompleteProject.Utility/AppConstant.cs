
namespace TheCompleteProject.Utility
{
    public static class AppConstant
    {
        public enum Message
        {
            Success,        // info.Message = AppConstant.Message.Success.ToString();
            Fail,
            NotRegister,
            Alreadyexists,
            Exception,
            NoRecord,
            InvalidModel,
            InvalidMobileNo,
            Unauthorised,
            BadRequest,
        };

        public const string EmailAlreadyExist = "Email Already Belongs To A User Please Try Again With Diffrent Email";
        public const string NoUserFound = "No User Found With That Email Or Password Found";

        public const string EMAILREQUIRED = "Email Is Required";
        public const string USERNAMEREQUIRED = "User Name Is Required";
        public const string InActiveUser = "User Is InActive";

        public const int Success = 200;
        public const int NotRegister = 204;
        public const int BadRequest = 400;
        public const int Unauthorised = 401;
        public const int NoRecord = 404;
        public const int Alreadyexists = 409;
        public const int Exception = 417;
    }
}
