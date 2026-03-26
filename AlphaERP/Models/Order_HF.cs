namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Order_HF
    {
        public short OrdYear { get; set; }
        public int OrderNo { get; set; }
        public string TawreedNo { get; set; }
        public int? OfferNo { get; set; }
        public long? VendorNo { get; set; }
        public int? ShipWay { get; set; }
        public int? CurType { get; set; }
        public string DlvryPlace { get; set; }
        public double? NetAmt { get; set; }
        public DateTime? EnterDate { get; set; }
        public string RefNo { get; set; }
    }
}
