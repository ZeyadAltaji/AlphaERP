namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ord_LinkRquestDFAndPO
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short ReqYear { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(10)]
        public string ReqNo { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(20)]
        public string ItemNo { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short PurchaseOrderYear { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PurchaseOrderNo { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(20)]
        public string PurchaseOrdTawreedNo { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double? Qty { get; set; }
    }
}
