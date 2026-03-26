namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrdRecHF")]
    public partial class OrdRecHF
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrdRecHF()
        {
            OrdRecDFs = new HashSet<OrdRecDF>();
        }

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

        public short? InvSR { get; set; }

        public int? OrderYear { get; set; }

        public int OrderNo { get; set; }

        [Required]
        [StringLength(20)]
        public string TawreedNo { get; set; }

        public DateTime? RecDate { get; set; }

        [StringLength(20)]
        public string InvNo { get; set; }

        public DateTime? InvDate { get; set; }

        public double? ConvRate { get; set; }

        public int? StoreNo { get; set; }

        public double? InvAmount { get; set; }

        public bool? UpdateInv { get; set; }

        public double? VouDisc { get; set; }

        public double? Vou_Tax { get; set; }

        public int? CurrCode { get; set; }

        public int? PoApplicant { get; set; }

        public double? NetAmt { get; set; }

        [StringLength(255)]
        public string Notes { get; set; }

        [StringLength(20)]
        public string FileNo { get; set; }

        [StringLength(50)]
        public string OrdUserDiff { get; set; }

        public double? InvAmountFr { get; set; }

        public DateTime? Rec2Date { get; set; }

        public bool printed { get; set; }

        public DateTime? RecDateRpt { get; set; }

        public bool? Deleted { get; set; }

        public int? vendorno { get; set; }

        [StringLength(20)]
        public string AwBillNo { get; set; }

        public float? LandCost { get; set; }

        public float? LocExpenses { get; set; }

        public float? OthExpenses { get; set; }

        public DateTime? DRecVD { get; set; }

        public int? DAprovC { get; set; }

        public bool? PrintedExp { get; set; }

        [StringLength(50)]
        public string costdiffapprove { get; set; }

        public short? InboundYear { get; set; }

        [StringLength(50)]
        public string InboundNo { get; set; }
        [StringLength(50)]
        public string InboundSer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrdRecDF> OrdRecDFs { get; set; }
    }
}
