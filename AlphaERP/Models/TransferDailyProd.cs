using System;

namespace AlphaERP.Models
{
    public class TransferDailyProd
    {
        public bool TransferFlag { get; set; }
        public bool? TranByOrder { get; set; }
        public int Transyear { get; set; }
        public int ItemSer { get; set; }
        public short ProdPrepYear { get; set; }
        public int ProdPrepNo { get; set; }
        public string FinCode { get; set; }
        public string ItemDesc { get; set; }
        public string Batch { get; set; }
        public string ProdYrNo { get; set; }
        public decimal Prod_Qty { get; set; }
        public decimal TotPreviousQty { get; set; }
        public decimal TotReciptQty { get; set; }
        public decimal TotpreviosConsQty { get; set; }
        public decimal ConsQty { get; set; }
        public int ToStoreNo { get; set; }
        public string ToStoreDesc { get; set; }
        public int FromStoreNo { get; set; }
        public string FromStoreDesc { get; set; }
        public string Notes { get; set; }
        public byte? UnitSerial { get; set; }
        public DateTime ExpDate { get; set; }
        public DateTime ManDate { get; set; }
        public DateTime TransDate { get; set; }
        public string Note { get; set; }
        public bool PostFlag { get; set; }
        public short? ConsVouYear { get; set; }
        public short? ProdRecYear { get; set; }
        public int? ConsVouNo { get; set; }
        public int? ProdRecNo { get; set; }
    }
}
