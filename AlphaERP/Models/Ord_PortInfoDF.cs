namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ord_PortInfoDF
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
        [StringLength(20)]
        public string ItemNo { get; set; }

        public double? ShippingQty { get; set; }

        [StringLength(5)]
        public string ShippingTUnit { get; set; }

        public short? ShippingUnitSerial { get; set; }

        public double? ShippingQty2 { get; set; }

        public double? PortQty { get; set; }

        [StringLength(5)]
        public string PortTUnit { get; set; }

        public short? PortUnitSerial { get; set; }

        public double? PortQty2 { get; set; }
    }
}
