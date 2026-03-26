namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class prod_hiring_prepare_info
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
        public short hiring_no { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(20)]
        public string Rawmaterial_no { get; set; }

        public int? Store_No { get; set; }

        public float? Qty { get; set; }

        public float? Qty_Cost { get; set; }

        public int? Hiring_Status { get; set; }

        public bool Hiring_hold_Status { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Hiring_Hold_Date { get; set; }

        public int? ConsNo { get; set; }
    }
}
