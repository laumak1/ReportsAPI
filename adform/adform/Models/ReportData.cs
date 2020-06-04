using System.Collections.Generic;

namespace adform.Models
{
    public class ReportData
    {
        public List<string> columnHeaders { get; set; }
        public List<List<string>> rows { get; set; }
    }
}
