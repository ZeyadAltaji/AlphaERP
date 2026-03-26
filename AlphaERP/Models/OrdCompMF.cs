namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrdCompMF")]
    public partial class OrdCompMF
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        public bool? ModSrl { get; set; }

        public bool? ChkWriteVouchInClosed { get; set; }

        public bool? ManualBatch { get; set; }

        public bool? UseOrderAprv { get; set; }

        public int? tax_Dept { get; set; }

        public long? tax_acc { get; set; }

        public int? LcDept { get; set; }

        public long? LcAcc { get; set; }

        public int? PLDept { get; set; }

        public long? PLAcc { get; set; }

        public int? LcCostDept { get; set; }

        public long? LcCostAcc { get; set; }

        public int? IncomeTaxDept { get; set; }

        public long? IncomeTaxAcc { get; set; }

        public bool? ChckQtyReceipt { get; set; }

        public short? ExtraReceivedPer { get; set; }

        public bool? UseEMail { get; set; }

        public bool? FixSerial { get; set; }

        public short? ExpVouType { get; set; }

        public short? InvVouType { get; set; }

        public short? DoCrVouType { get; set; }

        public bool? FixLcAcc { get; set; }

        public bool AccLink { get; set; }

        public bool? ShowOrdBonus { get; set; }

        public bool? UsePriceList { get; set; }

        [StringLength(50)]
        public string PRISOCode { get; set; }

        [StringLength(50)]
        public string POISOCode { get; set; }

        public int? BugDeptNo { get; set; }

        public long? BugAccNo { get; set; }

        public bool? AllowOpenInvoice { get; set; }

        public bool? ShowItemSer { get; set; }

        public short? SettlementVouType { get; set; }

        public int? CustomsTaxDept { get; set; }

        public long? CustomsTaxAcc { get; set; }

        public bool? duplicationVendorInvoiceNo { get; set; }

        public long? POAccMain { get; set; }

        public bool? LastCostByCurr { get; set; }

        public short? CloseMethod { get; set; }

        public bool? LastCostByPList { get; set; }

        public bool? MoreSerForRecOrd { get; set; }

        public bool? LinkReqOrdByUser { get; set; }
        public bool? IsDivisionPurchaseOrder { get; set; }
    }
}
