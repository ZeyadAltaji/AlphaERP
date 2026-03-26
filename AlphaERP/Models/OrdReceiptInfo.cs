namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrdReceiptInfo
    {
        public short OrderYear { get; set; }
        public int OrderNo { get; set; }
        public string TawreedNo { get; set; }
        public string ShipSer { get; set; }
        public long VendorNo { get; set; }
        public DateTime ArrivalDate { get; set; }
    }
}
