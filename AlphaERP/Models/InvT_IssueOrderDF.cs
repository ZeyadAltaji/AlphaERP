using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlphaERP.Models
{
    public partial class InvT_IssueOrderDF
    {
        public string ItemNo{ get; set; }
        public string Batch { get; set; }
        public short ItemSer { get; set; }
        public string UnitCode { get; set; }
        public double Qty { get; set; }
        public short UnitSerial { get; set; }
        public string ItemNotes { get; set; }
    }
}