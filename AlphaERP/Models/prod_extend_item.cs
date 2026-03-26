namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class prod_extend_item
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short comp_no { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string formula_code { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(20)]
        public string item_no { get; set; }

        [StringLength(50)]
        public string itemdesc { get; set; }

        public double? Qty1 { get; set; }

        public double? Qty2 { get; set; }

        public double? Qty3 { get; set; }

        [Column(TypeName = "money")]
        public decimal? percost { get; set; }

        public byte? UnitSerial { get; set; }
    }
}
