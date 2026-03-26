namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ord_PurchOrdExpensesHF
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

        [Column(TypeName = "smalldatetime")]
        public DateTime? TransDate { get; set; }

        public int? DeptNo { get; set; }

        public long? AccNo { get; set; }

        public int? CurrCode { get; set; }

        public double? ExRate { get; set; }

        public double? TotalAmount { get; set; }

        public double? FrAmount { get; set; }

        [StringLength(500)]
        public string Note { get; set; }

        public bool? IsPosted { get; set; }
    }
}
