namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LInboundsInfo
    {
        public short CompNo { get; set; }
        public short OrdYear { get; set; }
        public int OrderNo { get; set; }
        public string TawreedNo { get; set; }
        public string InboundSer { get; set; }
        public DateTime InboundDate { get; set; }
        public string ReqNo { get; set; }
        public long VendorNo { get; set; }
        public string VenName { get; set; }
        public string VenName_Eng { get; set; }
        public int StoreNo { get; set; }
        public string StoreName { get; set; }
        public string StoreNameEng { get; set; }
        public string InboundNotes { get; set; }
        public bool IsApproval { get; set; }
        public bool IsClosed { get; set; }
        public short ReqStatus { get; set; }
        public short RecStatus { get; set; }
        public string RecStatusDescAr { get; set; }
        public string RecStatusDescEn { get; set; }
        public string colorName { get; set; }

    }
}
