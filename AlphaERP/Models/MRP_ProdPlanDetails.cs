namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MRP_ProdPlanDetails
    {
        public short PlanYear { get; set; }
        public int PlanNo { get; set; }
        public string ItemNo { get; set; }
        public string ItemDesc { get; set; }
        public string ItemDesc_Ara { get; set; }
        public string FormulaCode { get; set; }
        public string formula_desc { get; set; }
        public string UnitDesc { get; set; }
        public decimal PQty { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PlanNote { get; set; }
    }
}
