namespace AlphaERP.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class MRP_GeneralPlanInfoD
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short PlanYear { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PlanNo { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(20)]
        public string FormulaCode { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(20)]
        public string ItemNo { get; set; }

        [Column(TypeName = "money")]
        public decimal? PQty { get; set; }

        public virtual MRP_GeneralPlanInfoH MRP_GeneralPlanInfoH { get; set; }
        [ForeignKey("CompNo,ItemNo")]
        public virtual InvItemsMF Item { get; set; }
        [ForeignKey("CompNo,FormulaCode")]
        public virtual prod_formula_header_info Formula { get; set; }
    }
    public partial class TMP_GeneralPlanInfoD
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short PlanYear { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PlanNo { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(20)]
        public string FormulaCode { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(20)]
        public string ItemNo { get; set; }

        [Column(TypeName = "money")]
        public decimal? PQty { get; set; }
        public string UnitCode { get; set; }
        public virtual MRP_GeneralPlanInfoH MRP_GeneralPlanInfoH { get; set; }
        [ForeignKey("CompNo,ItemNo")]
        public virtual InvItemsMF Item { get; set; }
        [ForeignKey("CompNo,FormulaCode")]
        public virtual prod_formula_header_info Formula { get; set; }
    }
}
