using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheCompleteProject.Utility;

namespace TheCompleteProject.Repository.CommonSpCalls
{
    public static  class CommonStoreProcedure
    {
        public static async Task<string> GetUserNameByIdAsync(int userId)
        {
            var param = new List<SqlParameter>
            {
                new SqlParameter("@UserId",userId)
            };

            var result =await SQLHelper.ExecuteProcedure<dynamic>("sp_FetchUserName", "UserName", param);
            return result;
        }

        public static void AddUsersBySp(string UserName, string Email, string Password, int RoleId)
        {
            var param = new List<SqlParameter>
            {
                new SqlParameter("@UserName",UserName),
                new SqlParameter("@Email",Email),
                new SqlParameter("@Password",Password),
                new SqlParameter("@RoleId",RoleId)
            };

            SQLHelper.ExecuteProcedure("sp_AddUsers", param);
        }
    }
}
