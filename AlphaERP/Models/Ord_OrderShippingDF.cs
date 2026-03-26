namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ord_OrderShippingDF
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
        public string TUnit { get; set; }

        public short? UnitSerial { get; set; }

        public double? Qty2 { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? ETA { get; set; }

        public bool? Marketing { get; set; }

        public bool? tqm { get; set; }

        public bool? QualityControl { get; set; }

        [StringLength(50)]
        public string SupplierInvoiceNo { get; set; }

        public double? Amount { get; set; }

        public int? CurrCode { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? ShippingDate { get; set; }

        public int? JFDA { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? SampleDate { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? JFDAApprovalDate { get; set; }

        [StringLength(50)]
        public string Declaration { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? ActualArrivalDate { get; set; }

        [StringLength(50)]
        public string AWBBL { get; set; }

        [StringLength(50)]
        public string Transporter { get; set; }

        [StringLength(50)]
        public string TransporterInvoice { get; set; }
    }
}
