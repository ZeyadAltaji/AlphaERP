namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ord_LcExpCodes
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short ExpID { get; set; }

        [Required]
        [StringLength(50)]
        public string ExpArDesc { get; set; }

        [StringLength(50)]
        public string ExpEDesc { get; set; }

        public int? ExpDeptNo { get; set; }

        public long? ExpAccNo { get; set; }

        public short? CostMethod { get; set; }
    }
}
