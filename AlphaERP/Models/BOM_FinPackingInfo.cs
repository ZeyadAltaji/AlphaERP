namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BOM_FinPackingInfo
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string FormCode { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(20)]
        public string FinItem { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(20)]
        public string PackItem { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(20)]

        public string RMCode { get; set; }

        [ForeignKey("CompNo,RMCode")]
        public virtual InvItemsMF item { get; set; }

        public short? UnitSerial { get; set; }

        [Column(TypeName = "money")]
        public decimal? Qty { get; set; }
    }
}
