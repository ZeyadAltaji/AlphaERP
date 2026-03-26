namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InvItemsMF")]
    public partial class InvItemsMF
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string ItemNo { get; set; }

        [StringLength(200)]
        public string ItemDesc_Ara { get; set; }

        [StringLength(200)]
        public string ItemDesc { get; set; }
        [StringLength(100)]
        public string UnitC1 { get; set; }
        [StringLength(100)]
        public string UnitC2 { get; set; }
        [StringLength(100)]
        public string UnitC3 { get; set; }
        [StringLength(100)]
        public string UnitC4 { get; set; }

        public decimal? Conv2 { get; set; }

        public decimal? Conv3 { get; set; }

        public decimal? Conv4 { get; set; }


        public string Categ { get; set; }

        public double? ItemUnitCost { get; set; }
        public decimal? SG { get; set; }
        public double? LastRecCost { get; set; }

        public long? prefvendor { get; set; }

        public double? STax_Perc { get; set; }

    }
}
