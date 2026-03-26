using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AlphaERP.Models
{
    public partial class ViewRawMateria
    {

        public double IssueCost { get; set; }
        public decimal issuedQty { get; set; }
        public string UnitName { get; set; }
        public string ItemNo { get; set; }
        public string ItemDesc_Ara { get; set; }
        public string ItemDesc { get; set; }
        public decimal stdqty { get; set; }
        public decimal qtyvar { get; set; }


        public double lastreccost { get; set; }
    }
}