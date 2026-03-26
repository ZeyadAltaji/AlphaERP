namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProdCost_QCInspDF
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long RefNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long QCTrNo { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short TestNo { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? QCTrDate { get; set; }

        [StringLength(10)]
        public string UserID { get; set; }

        [Column(TypeName = "money")]
        public decimal? ValFrom { get; set; }

        [Column(TypeName = "money")]
        public decimal? ValTo { get; set; }

        [Column(TypeName = "money")]
        public decimal? QCVal { get; set; }

        [StringLength(100)]
        public string Notes { get; set; }

        [StringLength(100)]
        public string GeneralQcNotes { get; set; }

        public bool? QCPassed { get; set; }

        public bool? QCClosed { get; set; }

        public short? compno { get; set; }
    }
}
