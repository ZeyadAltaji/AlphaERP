namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class prod_prodstage_info
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int comp_no { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int stage_code { get; set; }

        [StringLength(50)]
        public string stage_desc { get; set; }

        public short? GroupID { get; set; }

        public short? prev_prod_stage { get; set; }

        public int? no_yearly_daily_work { get; set; }

        [StringLength(100)]
        public string notes { get; set; }

        public double? Hr { get; set; }

        [Column(TypeName = "money")]
        public decimal? SetupTime { get; set; }

        [Column(TypeName = "money")]
        public decimal? SetupTimePrc { get; set; }

        [Column(TypeName = "money")]
        public decimal? StopTimePrc { get; set; }

        public bool? QualityControl { get; set; }
    }
}
