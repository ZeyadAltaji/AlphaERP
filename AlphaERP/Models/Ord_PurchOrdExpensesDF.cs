namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ord_PurchOrdExpensesDF
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short TransYear { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TransNo { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short OrderYear { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrderNo { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(20)]
        public string TawreedNo { get; set; }

        [Key]
        [Column(Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ExpID { get; set; }

        [Column(TypeName = "money")]
        public decimal? Amount { get; set; }

        [Column(TypeName = "money")]
        public decimal? FrAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal? SalesTaxAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal? SalesTaxAmountFr { get; set; }

        [Column(TypeName = "money")]
        public decimal? IncomeTaxAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal? IncomeTaxAmountFr { get; set; }

        [Column(TypeName = "money")]
        public decimal? CustomtaxAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal? CustomtaxAmountFr { get; set; }
    }
}
