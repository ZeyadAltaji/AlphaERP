namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LPROrder
    {
        public short OrdYear { get; set; }
        public int OrderNo { get; set; }
        public string TawreedNo { get; set; }
        public DateTime OrderDate { get; set; }
        public long VendorNo { get; set; }
        public string VenName { get; set; }
        public string VenName_Eng { get; set; }
        public string OrderCloseDescAr { get; set; }
        public string OrderCloseDescEn { get; set; }
        public string DlvState { get; set; }
        public string DlvState_Eng { get; set; }
        public short LevelApprv { get; set; }
        public string ApprovedDescAr { get; set; }
        public string ApprovedDescEn { get; set; }
        public string ReqNo { get; set; }
        public bool Approved { get; set; }
        public string RefNo { get; set; }
        public bool IsExport { get; set; }

    }
}
