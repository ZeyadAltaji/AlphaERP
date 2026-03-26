namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrderHF")]
    public partial class OrderHF
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

        public int? OfferNo { get; set; }

        public int? Vend_Dept { get; set; }

        public long? VendorNo { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? EnterDate { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? ExpShDate { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? ConfDate { get; set; }

        public int? CurType { get; set; }

        public int? PayWay { get; set; }

        public int? ShipWay { get; set; }

        public int? StoreNo { get; set; }

        [StringLength(5)]
        public string DlvryPlace { get; set; }

        [StringLength(255)]
        public string Notes { get; set; }

        [StringLength(20)]
        public string QutationRef { get; set; }

        public int? DlvDays { get; set; }

        public int? MainPeriod { get; set; }

        public double? Vou_Tax { get; set; }

        public double? Per_Tax { get; set; }

        public double? TotalAmt { get; set; }

        public double? NetAmt { get; set; }

        public bool? OrderClose { get; set; }

        public short? DlvState { get; set; }

        [StringLength(50)]
        public string PInCharge { get; set; }

        public bool Updated { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? ApprovalDate { get; set; }

        [StringLength(20)]
        public string UnitKind { get; set; }

        public int? LcDept { get; set; }

        public long? LcAccNo { get; set; }

        public double? VouDiscount { get; set; }

        public double? PerDiscount { get; set; }

        public short SourceType { get; set; }

        [Column(TypeName = "text")]
        public string GenNote { get; set; }

        public bool Approved { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? ETA { get; set; }

        [Column(TypeName = "text")]
        public string GenConditions { get; set; }

        [Column(TypeName = "text")]
        public string SpecialConditions { get; set; }

        [StringLength(50)]
        public string ShipTerms { get; set; }

        public short? ContactNo { get; set; }

        [StringLength(30)]
        public string Port { get; set; }

        public bool? IsLC { get; set; }

        [Column(TypeName = "money")]
        public decimal? FreightExpense { get; set; }

        [StringLength(50)]
        public string OrdUser { get; set; }

        public bool? HasRecommendation { get; set; }

        [StringLength(50)]
        public string OrdUserDiff { get; set; }

        [StringLength(150)]
        public string VendorName { get; set; }

        [Column(TypeName = "money")]
        public decimal? ExtraExpenses { get; set; }

        public bool? LastCost { get; set; }

        [StringLength(150)]
        public string RefNo { get; set; }

        public bool? IsExport { get; set; }

    }
}
