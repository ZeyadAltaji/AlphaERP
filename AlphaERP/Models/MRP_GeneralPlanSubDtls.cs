namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MRP_GeneralPlanSubDtls
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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Sr { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(20)]
        public string FormulaCode { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(20)]
        public string ItemNo { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FromDate { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? ToDate { get; set; }

        [Column(TypeName = "money")]
        public decimal? Qty { get; set; }
    }
}
