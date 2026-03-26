using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlphaERP.Models
{
    public partial class Prod_prepare_infoView
    {
        public short? comp_no { get; set; }
        public short? prepare_year { get; set; }
        public int? prepare_code { get; set; }

        public DateTime? prepare_date { get; set; }
        public string item_no { get; set; }

        public string formula_code { get; set; }
        public string formula_desc { get; set; }
        public string ItemDesc { get; set; }

        public bool? Approved { get; set; }
        public string OrderStatus { get; set; }
        public int? UnitSerial { get; set; }
        public double? best_qty { get; set; }
       public string UOM { get; set; }
        public int? DueDays { get; set; }
        public DateTime? ActionDate{get;set;}
        public string TestDesc { get; set; }
        public string stage_desc { get; set; }

    }
}