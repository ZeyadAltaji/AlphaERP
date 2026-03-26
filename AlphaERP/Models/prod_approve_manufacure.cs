namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class prod_approve_manufacure
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int comp_no { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int prepare_year { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int prepare_code { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int hiring_no { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int begin_code { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(5)]
        public string stage_code { get; set; }

        [Key]
        [Column(Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short StageSer { get; set; }

        public int? order_no { get; set; }

        [StringLength(20)]
        public string item_no { get; set; }

        public bool beginflag { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? begin_date { get; set; }

        [StringLength(50)]
        public string emp_no_begin { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? real_date_begining { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? real_time_begining { get; set; }

        public float? Qty_mnf { get; set; }

        public float? TotCost { get; set; }

        public bool Stage_Status { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Closed_Date { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Closed_Time { get; set; }

        public int? WorkedHour { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? workedmin { get; set; }

        public int? vouno { get; set; }

        [Column(TypeName = "money")]
        public decimal? CalcTime { get; set; }

        public int? ManualWorkedTime { get; set; }

        public int? SetupTime { get; set; }

        public int? StopTime { get; set; }
    }
}
