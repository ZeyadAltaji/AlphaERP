namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrdRequest
    {
        public short ReqYear { get; set; }
        public string ReqNo { get; set; }
        public DateTime ReqDate { get; set; }
        public short OrderPriority { get; set; }
        public string OrderPriorityDescAr { get; set; }
        public string OrderPriorityDescEn { get; set; }
        public string UserName { get; set; }
        public string RefNo { get; set; }
        public short ReqBU { get; set; }
        public string BusUnitDescAr { get; set; }
        public string BusUnitDescEng { get; set; }
        public int OrderType { get; set; }
        public string CodeDesc { get; set; }
        public string CodeDescEng { get; set; }
        public short ReqStatus { get; set; }
        public string ReqStatusDescAr { get; set; }
        public string ReqStatusDescEn { get; set; }
        public string Note { get; set; }
        public short? LevelApprv { get; set; }
        public string UserID { get; set; }
        public string RejectReason { get; set; }
        public string colorName { get; set; }

        

    }
}
