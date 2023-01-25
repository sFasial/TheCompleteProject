using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCompleteProject.Utility
{
    public static class SQLHelper
    {

        public static string ConnectionString = "Data Source=.;Initial Catalog=Registration;Integrated Security=True"; //string.Empty;
        public static string LogConnectionString = string.Empty;

        public async static Task<IEnumerable<T>> ExecuteProcedure<T>(string sql, List<SqlParameter> sqlParameters = null) where T : new()
        {
            var tcs = new TaskCompletionSource<List<T>>();
            DataSet ds = new DataSet();
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (var cmd = new SqlCommand(sql, con))
                {
                    if (sqlParameters != null)
                        cmd.Parameters.AddRange(sqlParameters.ToArray());
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        da.SelectCommand = cmd;
                        da.Fill(ds);
                    }
                    con.Close();
                    string JSONString = string.Empty;
                    JSONString = JsonConvert.SerializeObject(ds.Tables[0]);
                    IEnumerable<T> list = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<T>>(JSONString));
                    return list;
                }
            }
        }

        public static DataSet ExecuteProc(string sql, List<SqlParameter> sqlParameters = null)
        {
            DataSet ds = new DataSet();
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (var cmd = new SqlCommand(sql, con))
                {
                    if (sqlParameters != null)
                        cmd.Parameters.AddRange(sqlParameters.ToArray());
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        da.SelectCommand = cmd;
                        da.Fill(ds);
                    }
                    con.Close();
                    return ds;
                }
            }
        }

        public async static Task<dynamic> ExecuteProcedure<T>(string sql, string columnName = "Column1", List<SqlParameter> sqlParameters = null) where T : new()
        {
            var tcs = new TaskCompletionSource<List<T>>();
            DataSet ds = new DataSet();
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (var cmd = new SqlCommand(sql, con))
                {
                    if (sqlParameters != null)
                        cmd.Parameters.AddRange(sqlParameters.ToArray());
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        da.SelectCommand = cmd;
                        da.Fill(ds);
                    }
                    con.Close();

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        var dt = await Task.Run(() => ds.Tables[0]);
                        dynamic value = dt.Rows[0].Field<T>(columnName);
                        return value;
                    }
                    return "";
                }
            }
        }

        public static dynamic ExecuteProcedure<T>(string sql, string columnName = "Column1", List<SqlParameter> sqlParameters = null, string outputParameterName = "") where T : new()
        {
            var tcs = new TaskCompletionSource<List<T>>();
            DataSet ds = new DataSet();
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (var cmd = new SqlCommand(sql, con))
                {
                    if (sqlParameters != null)
                        cmd.Parameters.AddRange(sqlParameters.ToArray());
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        da.SelectCommand = cmd;
                        da.Fill(ds);
                    }
                    object value = 0;
                    if (!string.IsNullOrEmpty(outputParameterName))
                    {
                        value = cmd.Parameters[outputParameterName]?.Value;
                    }
                    con.Close();

                    return value;
                }
            }
        }

        public static void ExecuteProcedure(string sql, List<SqlParameter> sqlParameters = null)
        {
            DataSet ds = new DataSet();
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (var cmd = new SqlCommand(sql, con))
                {
                    if (sqlParameters != null)
                        cmd.Parameters.AddRange(sqlParameters.ToArray());
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        da.SelectCommand = cmd;
                        da.Fill(ds);
                    }
                    con.Close();
                    //string JSONString = string.Empty;
                    //JSONString = JsonConvert.SerializeObject(ds.Tables[0]);
                    ////IEnumerable<T> list = (IEnumerable<T>)Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<T>>(JSONString));
                    //IEnumerable<T> list = JsonConvert.DeserializeObject<IEnumerable<T>>(JSONString);
                    //return list;
                }
            }
        }

        public async static Task<IEnumerable<T>> ExecuteQuery<T>(string tableName, string columnList, string whereCondition, string orderBy) where T : new()
        {
            var param = new List<SqlParameter>
            {
              new SqlParameter("@TABLENAME" ,tableName),
              new SqlParameter("@COLUMNNAME" ,columnList),
              new SqlParameter("@WHERECONDITION" ,whereCondition),
              new SqlParameter("@ORDERBY" ,orderBy),
            };
            var result = await ExecuteProcedure<T>("O_SP_FILLDATASET", param);
            return result;
        }

        public static int ExecuteProcedureRowsAffected(string sql, List<SqlParameter> sqlParameters = null)
        {
            DataSet ds = new DataSet();
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (var cmd = new SqlCommand(sql, con))
                {
                    if (sqlParameters != null)
                        cmd.Parameters.AddRange(sqlParameters.ToArray());
                    cmd.CommandType = CommandType.StoredProcedure;
                    //using (SqlDataAdapter da = new SqlDataAdapter())
                    //{
                    //    da.SelectCommand = cmd;
                    //    da.Fill(ds);
                    //}

                    int a = cmd.ExecuteNonQuery();
                    con.Close();
                    return a;
                }
            }
        }
    }
}
