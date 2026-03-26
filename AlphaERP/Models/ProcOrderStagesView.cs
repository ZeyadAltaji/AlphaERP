namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    public partial class ProcOrderStagesView
    {
        public int comp_no { get; set; }
        public int prepare_year { get; set; }
        public int prepare_code { get; set; }
        public string stage_code { get; set; }
        public short StageSer { get; set; }
        public string stage_desc { get; set; }
        public bool Stage_Status { get; set; }
        public bool? QualityControl { get; set; }
        public DateTime? begin_date { get; set; }
        public DateTime? Closed_Time { get; set; }
        public DateTime? Closed_Date { get; set; }
        public int? WorkedHour { get; set; }
        public int? workedmin { get; set; }
        public bool beginflag { get; set; }
        public string StageStat { get; set; }

    }
}