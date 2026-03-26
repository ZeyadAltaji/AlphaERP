namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Invt_SlaughterOrderHF
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long SlaughterOrderNo { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? SlaughterOrderDate { get; set; }

        public int? StoreNo { get; set; }

        [StringLength(20)]
        public string ItemNo { get; set; }

        [StringLength(16)]
        public string Batch { get; set; }

        public double? Qty { get; set; }

        [StringLength(5)]
        public string TUnit { get; set; }

        public short? UnitSerial { get; set; }

        public double? Qty2 { get; set; }

        [StringLength(200)]
        public string Notes { get; set; }

        public bool? IsApproval { get; set; }
    }
}
