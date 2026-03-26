namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrdOfferDF")]
    public partial class OrdOfferDF
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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OfferNo { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long VendorNo { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(20)]
        public string ItemNo { get; set; }

        [StringLength(5)]
        public string PUnit { get; set; }

        [StringLength(20)]
        public string ItemSourceCode { get; set; }

        public double? Qty { get; set; }

        public double? Qty2 { get; set; }

        public double? Bonus { get; set; }

        public double? PerDiscount { get; set; }

        public double? Price1 { get; set; }

        public double? Price2 { get; set; }

        public double? Price3 { get; set; }

        public bool? Paper { get; set; }

        public bool? Free_Sample { get; set; }

        [StringLength(20)]
        public string NSItemNo { get; set; }

        public virtual OrdOfferHF OrdOfferHF { get; set; }
    }
}
