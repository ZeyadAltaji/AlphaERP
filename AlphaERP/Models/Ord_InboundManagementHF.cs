namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ord_InboundManagementHF
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short OrderYear { get; set; }

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
        [StringLength(50)]
        public string InboundSer { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(50)]
        public string InboundGRN { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InboundGRNDate { get; set; }

        public int? InboundGRNStoreNo { get; set; }

        [StringLength(150)]
        public string InvNo { get; set; }

        [StringLength(200)]
        public string InboundGRNNotes { get; set; }

        public bool? IsApproval { get; set; }

        public bool? IsMarketing { get; set; }

        public bool? Istqm { get; set; }

        public bool? IsQualityControl { get; set; }
        public bool? IsProduction { get; set; }

        public bool? IsReleaseOrder { get; set; }

        public bool? IsReserveFreeReleaseOrder { get; set; }

        public bool? IsApprovalQty { get; set; }
    }
}
