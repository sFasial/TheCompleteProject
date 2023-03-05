using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheCompleteProject.ModelsAndDto_s.DbModels;
using TheCompleteProject.Utility;

namespace TheCompleteProject.Repository.CommonSpCalls
{
    public static class CommonStoreProcedure
    {
        //TO CALL GetUsersBySp IN SERVICE
        // public IEnumerable<EmployeeDto> GetEmployeesByStoreProcedureTotalRecords(int from, int to, out int TotalRecords, string search = "")
        // {
        //     var result = _employeeRepository.GetEmployeesByStoreProcedureTotalRecords(from, to, outTotalRecords search);
        //     return result;
        // }
        public static GetUsersDto GetUsersBySp(int from, int to, out int totalRecords, string search = "")
        {
            totalRecords = 0;

            var fromParameter = new SqlParameter("@From", from);
            var toParameter = new SqlParameter("@To", to);
            var searchParameter = new SqlParameter("@Search", search == null ? "" : search);

            var totalRecordsParameter = new SqlParameter("@TotalRecords", totalRecords);
            totalRecordsParameter.Direction = ParameterDirection.Output;

            var param = new List<SqlParameter>
            {
                fromParameter
               ,toParameter
               ,searchParameter
               ,totalRecordsParameter
            };

            //var result = Task.Run(async () => await SQLHelper.ExecuteProcedure<Users>("sp_FetchUser", param)).Result;

            var result = Task.Run(async () => await SQLHelper.ExecuteProcedure<Users>("sp_FetchUser", param)).Result;
            totalRecords = Convert.ToInt32(totalRecordsParameter.Value);

            GetUsersDto usersDto = new GetUsersDto();
            usersDto.Users = result.ToList();
            usersDto.TotalRecords = totalRecords;

            return usersDto;
        }

        public static async Task<string> GetUserNameByIdAsync(int userId)
        {
            var param = new List<SqlParameter>
            {
                new SqlParameter("@UserId",userId)
            };

            var result = await SQLHelper.ExecuteProcedure<dynamic>("sp_FetchUserName", "UserName", param);
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
