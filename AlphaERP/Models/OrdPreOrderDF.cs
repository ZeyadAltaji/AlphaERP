namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrdPreOrderDF")]
    public partial class OrdPreOrderDF
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short OrdYear { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrderNo { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(20)]
        public string ItemNo { get; set; }
        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short? ItemSr { get; set; }

        [Required]
        [StringLength(20)]
        public string NSItemNo { get; set; }

        public double? Price { get; set; }

        public double? ExtraPrice { get; set; }

        public double? Qty { get; set; }

        public double? Qty2 { get; set; }

        public double? Bonus { get; set; }

        public bool? Confirmation { get; set; }

        public double? ConfirmQty { get; set; }

        [StringLength(200)]
        public string Note { get; set; }

        [StringLength(20)]
        public string VendItemNO { get; set; }

        public double? TotalValue { get; set; }

        [Column(TypeName = "money")]
        public decimal? DiscPer { get; set; }

        public double? DiscAmt { get; set; }

        public double? ValueAfterDisc { get; set; }

        public short? UnitSerial { get; set; }

        public virtual OrdPreOrderHF OrdPreOrderHF { get; set; }
    }
}
