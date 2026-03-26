namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ord_Web_EvalVenodrsHF
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Orderyear { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrderNo { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(20)]
        public string TawreedNo { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long EvalVendId { get; set; }

        public long VendorNo { get; set; }

        [StringLength(500)]
        public string doc { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? recdate { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? realdate { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? catdate { get; set; }

        public int? tawreedser { get; set; }
    }
}
