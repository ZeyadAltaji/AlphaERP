namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrdPreOrder
    {
        public short BusUnitID { get; set; }
        public string BusUnitDescAr { get; set; }
        public string BusUnitDescEng { get; set; }
        public short ItemSr { get; set; }
        public string ItemNo { get; set; }
        public string ItemDesc { get; set; }
        public string ItemDesc_Ara { get; set; }
        public string UnitName { get; set; }
        public double Qty { get; set; }
        public double Bonus { get; set; }
        public decimal DiscPer { get; set; }
        public double Price { get; set; }
        public double STax_Perc { get; set; }
        public double ValueAfterDisc { get; set; }
        public string Note { get; set; }
    }
}
