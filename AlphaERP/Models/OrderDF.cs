namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrderDF")]
    public partial class OrderDF
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
        public string TawreedNo { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(20)]
        public string ItemNo { get; set; }
        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short? ItemSr { get; set; }

        public short? SourceType { get; set; }

        [StringLength(50)]
        public string NSItemNo { get; set; }

        public double? PerDiscount { get; set; }

        public double? Bonus { get; set; }

        public double? Qty { get; set; }

        public double? Qty2 { get; set; }

        public double? Price { get; set; }

        public double? Price2 { get; set; }

        public double? Price3 { get; set; }

        public double? ordamt { get; set; }

        [StringLength(50)]
        public string DlvStateItem { get; set; }

        [StringLength(50)]
        public string RefNo { get; set; }

        public bool? Paper { get; set; }

        public double? RecQty { get; set; }

        public double? RecBonus { get; set; }

        [StringLength(5)]
        public string PUnit { get; set; }

        public double? ItemTaxVal { get; set; }

        public double? ItemTaxPer { get; set; }

        public bool? ItemTaxType { get; set; }

        public double? VouDiscount { get; set; }

        public short? UnitSerial { get; set; }

        [StringLength(200)]
        public string Note { get; set; }

        [Column(TypeName = "text")]
        public string NoteDtl { get; set; }

        public short? Pallets { get; set; }

        public short? Carons { get; set; }

        public short? Count20F { get; set; }

        public short? Count40F { get; set; }

        public short? DlvStateDet { get; set; }

        public DateTime? ReqDeliveryDate { get; set; }

    }
}
