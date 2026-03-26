namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InvCompMF")]
    public partial class InvCompMF
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        public short? CostMethod { get; set; }

        public bool AccountLink { get; set; }

        public short? AccLinkType { get; set; }

        public bool? PrintZero { get; set; }

        public bool? AskZero { get; set; }

        public DateTime? StartDate { get; set; }

        public bool? UseBatches { get; set; }

        public DateTime? PostDate { get; set; }

        public short? ClosedYear { get; set; }

        public DateTime? PurgeDate { get; set; }

        public DateTime? FrozenDate { get; set; }

        [StringLength(50)]
        public string Notes { get; set; }

        public bool Bonus_InPur { get; set; }

        public bool Disc_InPur { get; set; }

        public short? Decimal_qty { get; set; }

        public bool Use_Tax { get; set; }

        public double? Def_Tax_Per { get; set; }

        public bool UseMasterSell { get; set; }

        public bool UseMasterLevels { get; set; }

        public bool? ExpiredItems { get; set; }

        public int? SaftyPeriod { get; set; }

        public short? Back_Front { get; set; }

        public short? Vou_Serial { get; set; }

        public bool LinkWithProduction { get; set; }

        public bool LinkWithOrdering { get; set; }

        public bool? IssuAcc { get; set; }

        [StringLength(25)]
        public string Comp_Tax_No { get; set; }

        public DateTime? AccumulatDate { get; set; }

        public bool UseBatchesIssue { get; set; }

        public bool Use_Store_Serials { get; set; }

        public bool? AppSItmByDef { get; set; }

        public int? CashBDep { get; set; }

        public long? CashBAcc { get; set; }

        public int? ChqBDep { get; set; }

        public long? ChqBAcc { get; set; }

        public bool? Costing { get; set; }

        public bool? RecCostSell { get; set; }

        [StringLength(300)]
        public string RptSrcPath { get; set; }

        public bool? AllowZeroCost { get; set; }

        public bool? UseForginCurr { get; set; }

        public bool? UseDimItems { get; set; }

        public bool? UseEnterKey { get; set; }

        public bool? UseDeliverInfo { get; set; }

        public bool? UseItemSerial { get; set; }

        public bool? DayCheckPost { get; set; }

        public bool? AllowMinuseQty { get; set; }

        public bool? HideQtyOH { get; set; }

        public short? DefultUnit { get; set; }

        public bool? IntegrationWithVS { get; set; }

        public bool? PrintRecPortrait { get; set; }

        public bool? PostInTime { get; set; }

        public byte? CostFraction { get; set; }

        public bool? ForceSaleOrdInDNote { get; set; }

        public bool? ForceTransOrderInCons { get; set; }

        public bool? AmendProdRecWithoutIssue { get; set; }

        public bool? IssueByBatchSerial { get; set; }

        public bool WriteRecGLVouOnTime { get; set; }

        public int? WasteStoreNo { get; set; }

        public short? StockTakingVouType { get; set; }

        public bool? BlockExpery { get; set; }

        public bool? ShowScientificName { get; set; }

        public int? QtyFraction { get; set; }

        public bool? AllowAutoWIssueMoreThanOnce { get; set; }

        public bool? ShowItemSer { get; set; }

        public short? BufferStoreOption { get; set; }

        public bool? CopyPrevLine { get; set; }

        public bool? ChkSalesmanRiskValue { get; set; }

        public int? ConsignmentPriceListNo { get; set; }

        public bool? ConfirmTransferOption { get; set; }

        public bool? UseQty2 { get; set; }

        public bool? ChkQtyUpToDate { get; set; }

        public bool? AssemplyByDate { get; set; }

        public byte? MaxUsedCateg { get; set; }

        public bool? UseBatchIssueOrder { get; set; }

        public bool? ForceNewBatchOnReceipt { get; set; }
    }
}
