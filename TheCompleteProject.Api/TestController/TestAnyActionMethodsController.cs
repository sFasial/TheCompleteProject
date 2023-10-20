using Microsoft.Extensions.DependencyInjection;
using NPOI.SS.Formula.Functions;
using System.Collections.Generic;
using System.Reflection;
using System;
using TheCompleteProject.Utility;
using System.Linq;

public static class ServiceCollectionExtensions
{
    private static Dictionary<string, string> EnvironmentVariables = new Dictionary<string, string>();

    /// <summary>
    /// This method will read all the environment variables
    /// </summary>
    private static void ReadEnvironmentVariables()
    {
        #region ReadEnvironmentVariables

        foreach (FieldInfo info in typeof(ConstantsEnviormentTest).GetFields().Where(x => x.IsStatic))
        {
            string key = info.GetRawConstantValue().ToString();
            EnvironmentVariables.Add(key, Environment.GetEnvironmentVariable(key));
        }

        #endregion
    }

    public static IServiceCollection AddDashServices(this IServiceCollection services, AppSettings appsettings)
    {

        //SETTING UP THE PRIVATE DICTIONARY OBJECT 
        ReadEnvironmentVariables();


        //READING THE VALUE IF VALUE EXIST INITIALIZING IT IN THE APP SETTINGS 
        if (EnvironmentVariables[ConstantsEnviormentTest.MyMachineEnviorment] != "Development")
            appsettings.ServerName = Environment.MachineName;

        string serverType = EnvironmentVariables[ConstantsEnviormentTest.MyMachineServer];
        if (string.IsNullOrWhiteSpace(serverType))
        {
            //Log.Error("Kiwi Server Type is required in environment variables.");
        }
        else
            appsettings.ServerName = serverType;

        return services;
    }
}