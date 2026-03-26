namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MRPActivatePlan
    {
        public short CompNo { get; set; }
        public short ReqYear { get; set; }
        public int ReqNo { get; set; }
        public DateTime? ReqDate { get; set; }
        public short BUID { get; set; }
        public short PlanYear { get; set; }
        public int PlanNo { get; set; }
        public bool Activiate { get; set; }
        public string Reasoun { get; set; }
        public string Notes { get; set; }
        public bool ReqStatus { get; set; }
    }
}
