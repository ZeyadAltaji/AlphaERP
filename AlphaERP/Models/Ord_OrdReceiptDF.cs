namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ord_OrdReceiptDF
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
        [StringLength(15)]
        public string ShipSer { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RecNo { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(20)]
        public string ItemNo { get; set; }

        public double? PortQty { get; set; }

        [StringLength(5)]
        public string PortTUnit { get; set; }

        public short? PortUnitSerial { get; set; }

        public double? PortQty2 { get; set; }

        public double? RecQty { get; set; }

        [StringLength(5)]
        public string RecTUnit { get; set; }

        public short? RecUnitSerial { get; set; }

        public double? RecQty2 { get; set; }

        public double? CantTransport { get; set; }

        public double? Shortage { get; set; }

        public double? lostQty { get; set; }

        public double? WeightBreaker { get; set; }
    }
}
