namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ord_RequestDF
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short ReqYear { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(10)]
        public string ReqNo { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short ItemSr { get; set; }

        [Required]
        [StringLength(20)]
        public string ItemNo { get; set; }

        [StringLength(5)]
        public string TUnit { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double? Qty { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double? Qty2 { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double? Price { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double? TotalValue { get; set; }

        [StringLength(200)]
        public string Note { get; set; }

        public short? UnitSerial { get; set; }

        [StringLength(20)]
        public string SubItemNo { get; set; }

        [StringLength(5)]
        public string SubTUnit { get; set; }

        public short? SubUnitSerial { get; set; }

        public DateTime ReqDeliveryDate { get; set; }

        public short? ReqStatus { get; set; }
        public bool? IsReject { get; set; }
        public string RejectReason { get; set; }

        public bool? bPurchaseOrder { get; set; }

        public short? PurchaseOrderYear { get; set; }

        public int? PurchaseOrderNo { get; set; }
        public string PurchaseOrdTawreedNo { get; set; }

        public int? PurchaseOrdOfferNo { get; set; }
        public bool? bRequestforQuotation { get; set; }

        public short? RequestforQuotationYear { get; set; }

        public int? RequestforQuotationNo { get; set; }

        [Column(TypeName = "money")]
        public decimal? QtyStock { get; set; }

        public virtual Ord_RequestHF Ord_RequestHF { get; set; }
    }
}
