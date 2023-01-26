using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TheCompleteProject.Utility.MultimediaHelpers
{
    public static class DataTableExtension
    {
        public static DataTable ToDataTable<T>(this List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        #region CODE NOT USED YET
        //public static string ToHtmlTable<T>(this IEnumerable<T> list)
        //{
        //    var sb = new StringBuilder();

        //    sb.Append("<table style='width: 95 %; margin-bottom: 1rem; color: #212529; border-collapse: collapse; margin-bottom: 35px !important; margin-right: auto; margin-left: auto;'>");
        //    sb.Append("<tr style=' background-color: #5d78ff; color: #f8f9fa !important;'>");

        //    //Get all the properties
        //    PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        //    foreach (PropertyInfo prop in Props)
        //    {
        //        sb.Append($"<th style='text-align: left; padding: 0.75rem; vertical-align: top; border-top: 1px solid #dee2e6; vertical-align: bottom;'> "
        //            + $"{prop.Name} </th>");
        //    }

        //    sb.Append("</tr>");

        //    foreach (T item in list)
        //    {
        //        sb.Append("<tr>");
        //        var values = new object[Props.Length];
        //        for (int i = 0; i < Props.Length; i++)
        //        {
        //            //inserting property values to datatable rows
        //            values[i] = Props[i].GetValue(item, null);
        //            sb.Append($"<td style='text-align: left; padding: 0.75rem; vertical-align: top; border-bottom: 1px solid #dee2e6;'> "
        //                 + $"{values[i]} </td>");
        //        }
        //        sb.Append("</tr>");
        //    }

        //    sb.Append("</table>");

        //    return sb.ToString();
        //    //put a breakpoint here and check datatable
        //}

        //public static string ToHtmlTable<T>(this IEnumerable<T> list, List<string> headerList, params Func<T, object>[] columns)
        //{
        //    var sb = new StringBuilder();

        //    sb.Append("<table style='width: 95 %; margin-bottom: 1rem; color: #212529; border-collapse: collapse; margin-bottom: 35px !important; margin-right: auto; margin-left: auto;'>");
        //    sb.Append("<tr style=' background-color: #5d78ff; color: #f8f9fa !important;'>");


        //    foreach (var header in headerList)
        //    {
        //        sb.Append($"<th style='text-align: left; padding: 0.75rem; vertical-align: top; border-top: 1px solid #dee2e6; vertical-align: bottom;'> "
        //            + $"{header} </th>");
        //    }

        //    foreach (var item in list)
        //    {
        //        sb.Append("<tr>");

        //        foreach (var column in columns)

        //            sb.Append($"<td style='text-align: left; padding: 0.75rem; vertical-align: top; border-bottom: 1px solid #dee2e6; width: 200px;'>"
        //                + $"{column(item)} </td>");

        //        sb.Append("</tr>");
        //    }


        //    sb.Append("</table>");

        //    return sb.ToString();
        //}

        //public static string ToHtmlTable<T>(this IEnumerable<T> list, List<string> headerList, int headerId, string tableTitle, params Func<T, object>[] columns)
        //{
        //    var sb = new StringBuilder();

        //    sb.Append("<table style='width: 95 %; margin-bottom: 1rem; color: #212529; border-collapse: collapse; margin-bottom: 35px !important;'>");

        //    sb.Append($"<h4>{tableTitle}<h4>");

        //    sb.Append("<tr style=' background-color: #5d78ff; color: #f8f9fa !important;'>");


        //    foreach (var header in headerList)
        //    {
        //        sb.Append($"<th style='text-align: left; padding: 0.75rem; vertical-align: top; border-top: 1px solid #dee2e6; vertical-align: bottom;'> "
        //            + $"{header} </th>");
        //    }

        //    foreach (var item in list)
        //    {
        //        string alignment = "left";
        //        string width = "200px";
        //        int count = 0;
        //        sb.Append("<tr>");

        //        foreach (var column in columns)
        //        {
        //            if (headerId == count)
        //            {
        //                alignment = "right";
        //                width = "110px";
        //            }

        //            sb.Append($"<td style='text-align: {alignment}; padding: 0.75rem; vertical-align: top; border-bottom: 1px solid #dee2e6; width: {width};'>"
        //                        + $"{column(item)} </td>");
        //            count++;
        //        }
        //        sb.Append("</tr>");
        //    }


        //    sb.Append("</table>");

        //    return sb.ToString();
        //}

        //public static string ToHtmlTable<T>(this IEnumerable<T> list, List<string> headerList, List<CustomTableStyle> customTableStyles, params Func<T, object>[] columns)
        //{
        //    if (customTableStyles == null)
        //        customTableStyles = new List<CustomTableStyle>();

        //    var tableCss = string.Join(" ", customTableStyles?.Where(w => w.CustomTableStylePosition == CustomTableStylePosition.Table).Where(w => w.ClassNameList != null).SelectMany(s => s.ClassNameList)) ?? "";
        //    var trCss = string.Join(" ", customTableStyles?.Where(w => w.CustomTableStylePosition == CustomTableStylePosition.Tr).Where(w => w.ClassNameList != null).SelectMany(s => s.ClassNameList)) ?? "";
        //    var thCss = string.Join(" ", customTableStyles?.Where(w => w.CustomTableStylePosition == CustomTableStylePosition.Th).Where(w => w.ClassNameList != null).SelectMany(s => s.ClassNameList)) ?? "";
        //    var tdCss = string.Join(" ", customTableStyles?.Where(w => w.CustomTableStylePosition == CustomTableStylePosition.Td).Where(w => w.ClassNameList != null).SelectMany(s => s.ClassNameList)) ?? "";

        //    var tableInlineCss = string.Join(";", customTableStyles?.Where(w => w.CustomTableStylePosition == CustomTableStylePosition.Table).Where(w => w.InlineStyleValueList != null).SelectMany(s => s.InlineStyleValueList?.Select(x => String.Format("{0}:{1}", x.Key, x.Value)))) ?? "";
        //    var trInlineCss = string.Join(";", customTableStyles?.Where(w => w.CustomTableStylePosition == CustomTableStylePosition.Tr).Where(w => w.InlineStyleValueList != null).SelectMany(s => s.InlineStyleValueList?.Select(x => String.Format("{0}:{1}", x.Key, x.Value)))) ?? "";
        //    var thInlineCss = string.Join(";", customTableStyles?.Where(w => w.CustomTableStylePosition == CustomTableStylePosition.Th).Where(w => w.InlineStyleValueList != null).SelectMany(s => s.InlineStyleValueList?.Select(x => String.Format("{0}:{1}", x.Key, x.Value)))) ?? "";
        //    var tdInlineCss = string.Join(";", customTableStyles?.Where(w => w.CustomTableStylePosition == CustomTableStylePosition.Td).Where(w => w.InlineStyleValueList != null).SelectMany(s => s.InlineStyleValueList?.Select(x => String.Format("{0}:{1}", x.Key, x.Value)))) ?? "";

        //    var sb = new StringBuilder();

        //    sb.Append($"<table{(string.IsNullOrEmpty(tableCss) ? "" : $" class=\"{tableCss}\"")}{(string.IsNullOrEmpty(tableInlineCss) ? "" : $" style=\"{tableInlineCss}\"")}>");
        //    if (headerList != null)
        //    {
        //        sb.Append($"<tr{(string.IsNullOrEmpty(trCss) ? "" : $" class=\"{trCss}\"")}{(string.IsNullOrEmpty(trInlineCss) ? "" : $" style=\"{trInlineCss}\"")}>");
        //        foreach (var header in headerList)
        //        {
        //            sb.Append($"<th{(string.IsNullOrEmpty(thCss) ? "" : $" class=\"{thCss}\"")}{(string.IsNullOrEmpty(thInlineCss) ? "" : $" style=\"{thInlineCss}\"")}>{header}</th>");
        //        }
        //        sb.Append("</tr>");
        //    }
        //    foreach (var item in list)
        //    {
        //        sb.Append($"<tr{(string.IsNullOrEmpty(trCss) ? "" : $" class=\"{trCss}\"")}{(string.IsNullOrEmpty(trInlineCss) ? "" : $" style=\"{trInlineCss}\"")}>");
        //        foreach (var column in columns)
        //            sb.Append($"<td{(string.IsNullOrEmpty(tdCss) ? "" : $" class=\"{tdCss}\"")}{(string.IsNullOrEmpty(tdInlineCss) ? "" : $" style=\"{tdInlineCss}\"")}>{column(item)}</td>");
        //        sb.Append("</tr>");
        //    }

        //    sb.Append("</table>");

        //    return sb.ToString();
        //}

        //public static string HtmlTable<T>(this IEnumerable<T> list, List<string> headerList, string tableTitle, params Func<T, object>[] columns)
        //{
        //    var sb = new StringBuilder();

        //    sb.Append("<table style='min-width: 80px; max-width: 95%; margin-bottom: 1rem; color: #212529; border-collapse: collapse; margin-bottom: 15px !important;'>");

        //    sb.Append($"<h4>{tableTitle}<h4>");

        //    sb.Append("<tr style=' background-color: #5d78ff; color: #f8f9fa !important;'>");

        //    foreach (var header in headerList)
        //    {
        //        sb.Append($"<th style='text-align: left; padding: 0.75rem; vertical-align: top; border-top: 1px solid #dee2e6; vertical-align: bottom;'> "
        //            + $"{header} </th>");
        //    }

        //    sb.Append("</tr>");

        //    foreach (var item in list)
        //    {
        //        sb.Append("<tr>");

        //        foreach (var column in columns)
        //            sb.Append($"<td style='min-width: 80px; max-width: 200px; word-wrap: break-word; text-align: left; padding: 0.75rem; vertical-align: top; border-bottom: 1px solid #dee2e6;'> "
        //                + $"{column(item)} </td>");

        //        sb.Append("</tr>");
        //    }
        //    sb.Append("</table>");

        //    return sb.ToString();
        //}

        //public static string ToHtmlTable<T>(this IEnumerable<T> list, List<string> headerList, string dataFor, params Func<T, object>[] columns)
        //{
        //    var sb = new StringBuilder();

        //    sb.Append("<table style='width: 95 %; margin-bottom: 1rem; color: #212529; border-collapse: collapse; margin-bottom: 35px !important; margin-right: auto; margin-left: auto;'>");
        //    if (dataFor == "lastOneDayLeadSource")
        //    {
        //        sb.Append("<h4 style='text-align:center'> Day Leads Source Report</h4>");
        //    }
        //    else if (dataFor == "lastOneWeekLeadSource")
        //    {
        //        sb.Append("<h4 style='text-align:center'> Weekly Leads Source Report </h4>");
        //    }
        //    else
        //    {
        //        sb.Append("<h4 style='text-align:center'>Daily Leads Report </h4>");
        //    }
        //    sb.Append("<tr style=' background-color: #5d78ff; color: #f8f9fa !important;'>");

        //    foreach (var header in headerList)
        //    {
        //        sb.Append($"<th style='text-align: left; padding: 0.75rem; vertical-align: top; border-top: 1px solid #dee2e6; vertical-align: bottom;'> "
        //            + $"{header} </th>");
        //    }
        //    sb.Append("<tr style=' background-color: #5d78ff; color: #f8f9fa !important;'>");

        //    foreach (var item in list)
        //    {
        //        sb.Append("<tr>");

        //        foreach (var column in columns)
        //            sb.Append($"<td style='text-align: left; padding: 0.75rem; vertical-align: top; border-bottom: 1px solid #dee2e6;'> "
        //                + $"{column(item)} </td>");

        //        sb.Append("</tr>");
        //    }
        //    sb.Append("</table>");

        //    return sb.ToString();
        //}

        //public static string GetMemberName<T>(this Expression<T> expression)
        //{
        //    switch (expression.Body)
        //    {
        //        case MemberExpression m:
        //            return m.Member.Name;
        //        case UnaryExpression u when u.Operand is MemberExpression m:
        //            return m.Member.Name;
        //        default:
        //            throw new NotImplementedException(expression.GetType().ToString());
        //    }
        //}


        //public class CustomTableStyle
        //{
        //    public CustomTableStylePosition CustomTableStylePosition { get; set; }

        //    public List<string> ClassNameList { get; set; }
        //    public Dictionary<string, string> InlineStyleValueList { get; set; }
        //}

        //public enum CustomTableStylePosition
        //{
        //    Table,
        //    Tr,
        //    Th,
        //    Td
        //} 
        #endregion
    }

}
