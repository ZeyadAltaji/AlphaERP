namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ord_UserActions
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(8)]
        public string UserID { get; set; }

        public bool? ChckExtraQtyReceipt { get; set; }

        public bool? AmendConfDate { get; set; }

        public bool? AmendPOQty { get; set; }

        public bool? ShowPRCost { get; set; }

        public bool? ExceededInv { get; set; }

        public bool? HideOrdQtyInRec { get; set; }

        public short? ExtraReceivedPer { get; set; }

        public bool? AmendPOPrice { get; set; }

        public bool? lastPurchaseCost { get; set; }

        public double? lastPurchaseCostPer { get; set; }

        public bool? OldTransDate { get; set; }

        public bool? UserActionPosted { get; set; }
    }
}
