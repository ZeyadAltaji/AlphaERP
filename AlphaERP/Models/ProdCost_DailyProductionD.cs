namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProdCost_DailyProductionD
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short ReportYear { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ReportNo { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ItemSer { get; set; }

        public short? ProdPrepYear { get; set; }

        public int? ProdPrepNo { get; set; }

        [StringLength(20)]
        public string FinCode { get; set; }

        [Column(TypeName = "money")]
        public decimal? Prod_Qty { get; set; }

        public bool? PostFlag { get; set; }

        public short? ProdRecYear { get; set; }

        public int? ProdRecNo { get; set; }

        public bool? TransferFlag { get; set; }

        public short? ConsVouYear { get; set; }

        public int? ConsVouNo { get; set; }

        public bool? TransferFlag2 { get; set; }

        public short? ConsVouYear2 { get; set; }

        public int? ConsVouNo2 { get; set; }

        public int? TransNoIn { get; set; }

        public int? TransNoOut { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TransDateIn { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TransDateOut { get; set; }

        [StringLength(8)]
        public string TransUserIn { get; set; }

        [StringLength(8)]
        public string TransUserOut { get; set; }

        [StringLength(8)]
        public string UserID { get; set; }

        [StringLength(16)]
        public string Batch { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? ManDate { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? ExpDate { get; set; }

        [Column(TypeName = "money")]
        public decimal? ConsQty { get; set; }

        public short? UnitSerial { get; set; }

        [StringLength(5)]
        public string TUnit { get; set; }

        public virtual ProdCost_DailyProductionH ProdCost_DailyProductionH { get; set; }
    }
}
