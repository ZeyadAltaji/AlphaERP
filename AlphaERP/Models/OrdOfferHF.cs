namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrdOfferHF")]
    public partial class OrdOfferHF
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrdOfferHF()
        {
            OrdOfferDFs = new HashSet<OrdOfferDF>();
        }

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

        public int? OfferCurr { get; set; }

        public int? DelivDay { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? OfferDate { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? ValidateDate { get; set; }

        public int? ShipCenter { get; set; }

        public int? PayWay { get; set; }

        public int? MainPeriod { get; set; }

        [StringLength(250)]
        public string Notes { get; set; }

        [StringLength(20)]
        public string VendorOfferNo { get; set; }

        public short? LcType { get; set; }

        public int? PreOrderNo { get; set; }

        public short? PreOrderYear { get; set; }

        [StringLength(5)]
        public string RecievePlace { get; set; }

        [StringLength(5)]
        public string Cond1 { get; set; }

        [StringLength(5)]
        public string Cond2 { get; set; }

        [StringLength(5)]
        public string Cond3 { get; set; }

        [StringLength(5)]
        public string Cond4 { get; set; }

        [StringLength(5)]
        public string Cond5 { get; set; }

        public float? PerCond1 { get; set; }

        public float? PerCond2 { get; set; }

        public float? PerCond3 { get; set; }

        public float? PerCond4 { get; set; }

        public float? PerCond5 { get; set; }

        [StringLength(5)]
        public string Guaranty1 { get; set; }

        [StringLength(5)]
        public string Guaranty2 { get; set; }

        [StringLength(5)]
        public string Guaranty3 { get; set; }

        [StringLength(5)]
        public string Guaranty4 { get; set; }

        [StringLength(5)]
        public string Guaranty5 { get; set; }

        public float? PerGuaranty1 { get; set; }

        public float? PerGuaranty2 { get; set; }

        public float? PerGuaranty3 { get; set; }

        public float? PerGuaranty4 { get; set; }

        public float? PerGuaranty5 { get; set; }

        public int? Guaranty { get; set; }

        public int? OldPreOrderNo { get; set; }

        [StringLength(50)]
        public string Delivplace { get; set; }

        public double? BookNo { get; set; }

        public DateTime? lastdate { get; set; }

        [StringLength(100)]
        public string Vendorname { get; set; }

        [StringLength(100)]
        public string vendoraddress { get; set; }

        public double? ConvRate { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string VenOfferNo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrdOfferDF> OrdOfferDFs { get; set; }
    }
}
