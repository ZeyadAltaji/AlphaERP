namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AllStores
    {
        public string AlternativeItemNo { get; set; }
        public string ItemDesc { get; set; }
        public int StoreNo { get; set; }
        public string StoreName { get; set; }
        public string BatchNo { get; set; }
        [Column(TypeName = "money")]
        public decimal? StoreQty { get; set; }
        public DateTime ManDate { get; set; }
        public DateTime ExpDate { get; set; }
        public bool IsHalt { get; set; }
        [Column(TypeName = "money")]
        public decimal QtyOh { get; set; }
        [Column(TypeName = "money")]
        public decimal TotQtyRes { get; set; }
        public string RefNo { get; set; }
        public string UnitC4 { get; set; }
        [Column(TypeName = "float")]
        public double UnitCost { get; set; }

    }
}
