using System;
using System.Collections.Generic;
using System.Data;
namespace TheCompleteProject.Utility
{
    public static class DataTableHelper
    {

        public static DataSet ConvertListToDataSet<T>(this IList<T> list)
        {
            Type elementType = typeof(T);
            DataSet ds = new DataSet();
            DataTable Excel = new DataTable();
            Excel.TableName = "Excel";
            ds.Tables.Add(Excel);

            //add column to table for each public property on T
            foreach (var propInfo in elementType.GetProperties())
            {
                Type colType = propInfo.PropertyType;

                colType = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;
                Excel.Columns.Add(propInfo.Name, colType);

            }

            //go throuh each property of t and add each value to the table
            foreach (T items in list)
            {
                DataRow row = Excel.NewRow();
                foreach (var propInfo in elementType.GetProperties())
                {
                    row[propInfo.Name] = propInfo.GetValue(items, null);
                }
                Excel.Rows.Add(row);
            }
            return ds;
        }

        //public static DataSet ConvertListToDataSet<T>(this IList<T> list)
        //{
        //    Type elementType = typeof(T);
        //    DataSet ds = new DataSet();
        //    DataTable Excel = new DataTable();
        //    Excel.TableName = "Excel";
        //    ds.Tables.Add(Excel);

        //    //add column to table for each public property on T
        //    foreach (var propInfo in elementType.GetProperties())
        //    {
        //        Type colType = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;
        //        Excel.Columns.Add(propInfo.Name, colType);
        //    }

        //    //go throuh each property of t and add each value to the table
        //    foreach (T items in list)
        //    {
        //        DataRow row = Excel.NewRow();
        //        foreach (var propInfo in elementType.GetProperties())
        //        {
        //            row[propInfo.Name] = propInfo.GetValue(items, null);
        //        }
        //        Excel.Rows.Add(row);
        //    }
        //    return ds;
        //}
    }
}
