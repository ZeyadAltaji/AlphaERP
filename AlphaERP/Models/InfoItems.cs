namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class InfoItems
    {
        public string ItemNo { get; set; }
        public string ItemDesc { get; set; }
        public double SellPrice_1 { get; set; }
        public string RefrenceNo { get; set; }
        [Column(TypeName = "money")]
        public decimal Conv2 { get; set; }
        public string UnitC2 { get; set; }
        public string CatName { get; set; }
        public string SubCatName { get; set; }
        public string BinLoc { get; set; }
        [Column(TypeName = "float")]
        public double ItemUnitCost { get; set; }
        public short ValidityInMonth { get; set; }
        public bool UseExpiry { get; set; }

    }
}
