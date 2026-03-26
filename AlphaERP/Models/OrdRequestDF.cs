namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrdRequestDF
    {
        public short CompNo { get; set; }
        public short ReqYear { get; set; }
        public string ReqNo { get; set; }
        public short ItemSr { get; set; }
        public short OrderType { get; set; }
        public string SubItemNo { get; set; }
        public string SubTUnit { get; set; }
        public short? SubUnitSerial { get; set; }
        public double? Qty { get; set; }
        public double? Qty2 { get; set; }
        public double? Price { get; set; }
        public double? TotalValue { get; set; }
        public long prefvendor { get; set; }
        public DateTime ReqDeliveryDate { get; set; }
        public string UserID { get; set; }
        public string Note { get; set; }


    }
}
