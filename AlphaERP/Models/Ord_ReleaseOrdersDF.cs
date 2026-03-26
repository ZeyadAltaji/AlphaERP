namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ord_ReleaseOrdersDF
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short OrderYear { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrderNo { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(20)]
        public string TawreedNo { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(50)]
        public string InboundSer { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(50)]
        public string InboundGRN { get; set; }

        [Key]
        [Column(Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ReleaseOrdId { get; set; }

        [Key]
        [Column(Order = 7)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ItemSrl { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(20)]
        public string ItemNo { get; set; }

        [StringLength(15)]
        public string batchNo { get; set; }

        [StringLength(5)]
        public string TUnit { get; set; }

        public short? UnitSerial { get; set; }

        public double? ReservedQty { get; set; }

        public double? FreeQty { get; set; }
    }
}
