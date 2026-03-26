using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlphaERP.Models
{
    public partial class Prod_prodstage_infoView
    {
 
        public int comp_no { get; set; }
 
        public int stage_code { get; set; }

         public string stage_desc { get; set; }

        public short? GroupID { get; set; }

        public short? prev_prod_stage { get; set; }

        public int? no_yearly_daily_work { get; set; }

         public string notes { get; set; }

        public double? Hr { get; set; }

         public decimal? SetupTime { get; set; }

         public decimal? SetupTimePrc { get; set; }

         public decimal? StopTimePrc { get; set; }

        public bool? QualityControl { get; set; }
        public int? NoOfBoms { get; set; }
     }
}