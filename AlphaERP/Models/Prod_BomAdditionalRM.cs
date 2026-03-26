namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Prod_BomAdditionalRM
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string BOMCode { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(20)]
        public string RMCode { get; set; }

        public byte? UnitSerial { get; set; }

        [Column(TypeName = "money")]
        public decimal? QTY { get; set; }

        [StringLength(100)]
        public string Notes { get; set; }
    }
}
