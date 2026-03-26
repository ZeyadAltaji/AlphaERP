using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlphaERP.Models
{
    public partial class ProdCost_EmpWorkHoursD_WebView
    {
        public short CompNo { get; set; }
        public short PrepYear { get; set; }
        public int PrepNo { get; set; }
        public DateTime TrDate { get; set; }
        public short ShiftNo { get; set; }
        public int EmpNo { get; set; }
        public int StageCode { get; set; }
        public string ActualWorkHours { get; set; }
        public string EmpName { get; set; }

    }
}