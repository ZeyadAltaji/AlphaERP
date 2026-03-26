namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    public partial class LoadTestDetQcDfView
    {
        public long RefNo { get; set; }
        public long QCTrNo { get; set; }
        public short TestNo { get; set; }

        public DateTime? QCTrDate { get; set; }
        public string UserID { get; set; }
        public decimal? FromNo { get; set; }
        public decimal? ToNo { get; set; }
        public decimal? QCVal { get; set; }
        public string Notes { get; set; }
        public string GeneralQcNotes { get; set; }

        public bool? QCPassed { get; set; }

        public bool? QCClosed { get; set; }

        public string TestDesc { get; set; }
        public decimal? AlertFromNo { get; set; }
        public decimal? AlertToNo { get; set; }
        public decimal? ValFrom { get; set; }
        public decimal? ValTo { get; set; }
    }
}