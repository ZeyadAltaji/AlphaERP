namespace AlphaERP.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Mrp_YearPlanDetlsH")]

    public partial class YearPlanDetlsH
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short PlanYear { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? PlanDate { get; set; }

        [StringLength(100)]
        public string Notes { get; set; }

        public bool PUpdate { get; set; }
    }
}
