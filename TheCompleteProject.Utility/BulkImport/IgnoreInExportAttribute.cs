using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCompleteProject.Utility.BulkImport
{
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public class IgnoreInExportAttribute : Attribute
    {
        //Class Members
    }

    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public class IsExcelAttribute : Attribute
    {
        //Class Members
    }
}
