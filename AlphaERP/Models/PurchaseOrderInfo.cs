namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PurchaseOrderInfo
    {
        public string ReqNo { get; set; }
        public short OrdYear { get; set; }
        public int OrderNo { get; set; }
        public string TawreedNo { get; set; }
        public string ShipSer { get; set; }
        public DateTime EnterDate { get; set; }
        public long VendorNo { get; set; }
        public double? Qty { get; set; }
        public string RefNo { get; set; }
        public short DlvStateCode { get; set; }
        public string DlvState { get; set; }
        public string DlvState_Eng { get; set; }
        public string Name { get; set; }
        public string Eng_Name { get; set; }
        public DateTime ReqDeliveryDate { get; set; }
        public bool IsClosed { get; set; }
        public string colorName { get; set; }

    }
}
