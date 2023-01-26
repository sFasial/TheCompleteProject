using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCompleteProject.ModelsAndDto_s
{
    public class MultipleExcelSheetsDto
    {
        public DataTable dt { get; set; }
        public string WorkSheetName { get; set; }
        public string ReportHeading { get; set; }
    }
}
