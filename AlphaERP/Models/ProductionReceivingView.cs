using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlphaERP.Models
{
    public partial class ProductionReceivingView
    {
        public double RecCost { get; set; }
        public decimal RecQty { get; set; }
        public string UnitName { get; set; }
        public string ItemNo { get; set; }
        public string ItemDesc_Ara { get; set; }
        public string ItemDesc { get; set; }
        public decimal stdqty { get; set; }
        public decimal qtyvar { get; set; }
        public double lastreccost { get; set; }
    }
}