namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    public partial class ProdCost_QAEvent_SetupView
    {
        public int QA_ProcNo { get; set; }
        public long MainReqNo { get; set; }
        public Int64 ReqNo { get; set; }
        public short Proc_SubNo { get; set; }
        public short Eventserial { get; set; }
        public string Description { get; set; }
        public byte? MakerValue { get; set; }
        public DateTime? MDate { get; set; }
        public string MUser { get; set; } = string.Empty;
        public string MakerReadValue { get; set; } = string.Empty;
        public bool? MPostStat { get; set; }
        public byte? CheckerValue { get; set; }
        public DateTime? CDate { get; set; }
        public string CUser { get; set; } = string.Empty;
        public string CheckerReadValue { get; set; } = string.Empty;
        public bool? CPostStat { get; set; }
        public bool? AddMood{ get; set; }

    }
}