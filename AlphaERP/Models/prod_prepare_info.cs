namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class prod_prepare_info
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short comp_no { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short prepare_year { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int prepare_code { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? prepare_date { get; set; }

        public long? CustNo { get; set; }

        [StringLength(20)]
        public string item_no { get; set; }

        [StringLength(20)]
        public string formula_code { get; set; }

        [StringLength(5)]
        public string formula_type { get; set; }

        public float? qty_prepare { get; set; }

        public int? hiring_no { get; set; }

        [StringLength(50)]
        public string emp_prepare { get; set; }

        [StringLength(5)]
        public string prepare_cause { get; set; }

        public long? order_no { get; set; }

        public float? length { get; set; }

        public float? width { get; set; }

        public float? height { get; set; }

        public bool KeepFlag { get; set; }

        [StringLength(255)]
        public string Note { get; set; }

        public int? PrepareTestNo { get; set; }

        public short? PlanYear { get; set; }

        public short? PlanMonth { get; set; }

        public short? PlanDay { get; set; }

        public bool? Issued { get; set; }

        public double? waste { get; set; }

        public int? MasterWorder { get; set; }

        public int? MinWorkOrder { get; set; }

        public short? SalesOrderY { get; set; }

        public int? SalesOrderN { get; set; }

        [Column(TypeName = "money")]
        public decimal? ExCost { get; set; }

        [StringLength(50)]
        public string ExCostReason { get; set; }

        public bool SusFlag { get; set; }

        [StringLength(10)]
        public string PersianDate { get; set; }

        public int? FileStanderExp { get; set; }

        public int? FileStanderRm { get; set; }

        public double? NoOfMix { get; set; }

        public short? BusUnitID { get; set; }

        public int? StoreNo { get; set; }

        public bool? QcChk1 { get; set; }

        public bool? QcChk2 { get; set; }

        [Column(TypeName = "money")]
        public decimal? Actual_SG { get; set; }

        public bool? IsComplete { get; set; }

        [Column(TypeName = "money")]
        public decimal? RMStdQty { get; set; }

        [Column(TypeName = "money")]
        public decimal? RMIssueQty { get; set; }

        [Column(TypeName = "money")]
        public decimal? RmIssueCost { get; set; }

        [Column(TypeName = "money")]
        public decimal? PKIssueCost { get; set; }

        [Column(TypeName = "money")]
        public decimal? FGRecQty { get; set; }

        [Column(TypeName = "money")]
        public decimal? FGRecCost { get; set; }

        [Column(TypeName = "money")]
        public decimal? OHCost { get; set; }

        [Column(TypeName = "money")]
        public decimal? DistributedPKIssueCost { get; set; }

        [StringLength(100)]
        public string CloseNotes { get; set; }
        [StringLength(100)]
        public string RefNo1 { get; set; }
        [StringLength(100)]
        public string RefNo2 { get; set; }
    }
}
