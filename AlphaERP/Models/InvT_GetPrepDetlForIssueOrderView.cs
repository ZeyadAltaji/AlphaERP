using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlphaERP.Models
{
    public partial class InvT_GetPrepDetlForIssueOrderView
    {
        public short CompNo { get; set; }
        public short prepare_year { get; set; }
         public int prepare_code { get; set; }
        public string IUnit { get; set; }
        public string ItemNo { get; set; }
        public string ItemDesc { get; set; }
        //public double TotQty { get; set; }
        public bool DimItem { get; set; }
        public int UnitSerial { get; set; }

    }
}