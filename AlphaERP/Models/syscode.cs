namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class syscode
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(5)]
        public string sys_major { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(5)]
        public string sys_minor { get; set; }

        [StringLength(50)]
        public string sys_adesc { get; set; }

        [StringLength(50)]
        public string sys_edesc { get; set; }

        [StringLength(5)]
        public string Sys_ID { get; set; }
    }
}
