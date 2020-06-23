using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPCORE.Reporting
{
    public class VMDailyAttSummary
    {
        public int? LocationID { get; set; }
        public string LocationName { get; set; }
        public int? OUID { get; set; }
        public string OUName { get; set; }
        public DateTime? AttDate { get; set; }
        public double? TotalEmps { get; set; }
        public double? PresentEmps { get; set; }
        public double? AbsentEmps { get; set; }
        public double? RestEmps { get; set; }
        public double? LeaveEmps { get; set; }
        public double? LateIN { get; set; }
        public double? EarlyIN { get; set; }
        public double? LateOut { get; set; }
        public double? EarlyOut { get; set; }
        public double? Overtime { get; set; }
        public double? TimeBasedJC { get; set; }
        public double? DateBasedJC { get; set; }
    }
}
