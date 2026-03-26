namespace AlphaERP.Models
{
    using System;

    public partial class MRP_Calculation
    {
        public short PlanYear { get; set; }
        public int PlanNo { get; set; }
        public int Days { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PlanNote { get; set; }
        public string ItemNo { get; set; }
        public string ItemDesc { get; set; }
        public string UnitDesc { get; set; }
        public decimal TotalReqQty { get; set; }
        public decimal TotalPrvQty { get; set; }
        public decimal QtyOH { get; set; }
        public int QtyOHCDays { get; set; }
        public decimal QtyInOrder { get; set; }
        public int QtyInOrderCDays { get; set; }
        public decimal TotalAvialbeQty { get; set; }
        public int StockCDays { get; set; }
        public bool OrderStat { get; set; }
        public decimal QtyShoratge { get; set; }
        public decimal NoOforders { get; set; }
        public decimal QtyToOrder { get; set; }
        public long Prefvendor { get; set; }
        public string SupplierName { get; set; }
        public string PlanPRNo { get; set; }
        public bool? Action { get; set; }

        public int LeadTime { get; set; }
        public int StockCoverage { get; set; }
        public short ReOrderPoint { get; set; }
        public decimal MOQ { get; set; }
        public long OrderMulti { get; set; }
        public double TargqtQty { get; set; }
        public double ReOrderQty { get; set; }
        public DateTime? OrderArivDate { get; set; }
        public short GapStat { get; set; }
    }
}
