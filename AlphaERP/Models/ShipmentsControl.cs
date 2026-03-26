namespace AlphaERP.Models
{
    using System;

    public partial class ShipmentsControl
    {
        public short OrdYear { get; set; }
        public int OrderNo { get; set; }
        public string TawreedNo { get; set; }
        public string ShipSer { get; set; }
        public DateTime RecDate { get; set; }
        public double PurchOrdQty { get; set; }
        public double PurchOrdQty2 { get; set; }
        public double PortQty { get; set; }
        public double PortQty2 { get; set; }
        public double ReceiptQty { get; set; }
        public double ReceiptQty2 { get; set; }
        public double WriteOffQty { get; set; }
        public double WriteOffQty2 { get; set; }
        public double SalesQty { get; set; }
        public double SalesQty2 { get; set; }
        public double TotalAdjustedWeight { get; set; }
        public decimal QtyOH { get; set; }
        public decimal QtyOH2 { get; set; }
        public double TotalIssues { get; set; }
        public double TotalIssuesTreatments { get; set; }
        public double IssuesQty { get; set; }
        public double IssuesTreatmentsQty { get; set; }
        public double Total { get; set; }
    }
}
