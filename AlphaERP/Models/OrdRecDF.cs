namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrdRecDF")]
    public partial class OrdRecDF
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short RecYear { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RecNo { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(20)]
        public string ItemNo { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(16)]
        public string Batch { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short UnitSerial { get; set; }

        public short? ItemSr { get; set; }

        public short OrderYear { get; set; }

        public int OrderNo { get; set; }

        [Required]
        [StringLength(20)]
        public string TawreedNo { get; set; }

        public double? RecQty { get; set; }

        public double? InvQty { get; set; }

        public double? InvQty2 { get; set; }

        public double? InvBonusQty { get; set; }

        public double? Discount { get; set; }

        public double? Item_Tax_Val { get; set; }

        public double? Item_Tax_Per { get; set; }

        public double? NetAmount { get; set; }

        [StringLength(50)]
        public string RefNo { get; set; }

        public double? RecSer { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? RecSerDate { get; set; }

        public int? ExpenSer { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? ExpenSerDate { get; set; }

        public double? DiscAmt { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? ManDate { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime ExpDate { get; set; }

        [StringLength(20)]
        public string invno { get; set; }

        public int? vendorno { get; set; }

        public float? Bonus { get; set; }

        public float? BackQty { get; set; }

        [Column(TypeName = "money")]
        public decimal? Price { get; set; }

        public float? ConfirmQty { get; set; }

        [StringLength(250)]
        public string Note { get; set; }

        [StringLength(5)]
        public string Punit { get; set; }

        public virtual OrdRecHF OrdRecHF { get; set; }
    }
}
