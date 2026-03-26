namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("glactmf")]
    public partial class glactmf
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short acc_comp { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long acc_num { get; set; }

        [StringLength(100)]
        public string acc_desc { get; set; }

        [StringLength(100)]
        public string acc_edesc { get; set; }

        public int? acc_type { get; set; }

        public int? acc_norbal { get; set; }

        public int? acc_report { get; set; }

        public int? acc_totlev { get; set; }

        public int? acc_lineadd { get; set; }

        public int? acc_curr { get; set; }

        public bool? acc_halt { get; set; }

        public long? acc_parent { get; set; }

        public int? acc_class { get; set; }

        [StringLength(50)]
        public string acc_refno { get; set; }
    }
}
