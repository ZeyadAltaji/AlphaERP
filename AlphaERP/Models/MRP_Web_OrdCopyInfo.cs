namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MRP_Web_OrdCopyInfo
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ReqforQuotyear { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ReqforQuotNo { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long VendorNo { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(250)]
        public string ItemNo { get; set; }

        public double? RequiredQty { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? SellDate { get; set; }

        [StringLength(250)]
        public string Notes { get; set; }

        [StringLength(250)]
        public string Port { get; set; }
        public bool? SendTestSample { get; set; }
        public short Curr { get; set; }
        public short Pmethod { get; set; }
        public short DeliveryPlace { get; set; }
        [StringLength(250)]
        public string CountryOfOrigin { get; set; }
        [StringLength(250)]
        public string ShippingPort { get; set; }
        public double? Qty { get; set; }
        public double? Qty2 { get; set; }

        public double? Bonus { get; set; }

        [Column(TypeName = "money")]
        public decimal? SellPrice { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DeliveryDate { get; set; }

        public bool? bVendorsOffers { get; set; }
        public bool? bPriceOffersAnalysis { get; set; }
        public bool? bBestOffer { get; set; }
        public bool? bNominationBestOffer { get; set; }

        public bool? bPriceOffersApproval { get; set; }

    }
}
