namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ord_InboundsInfoHF
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

        [Column(TypeName = "smalldatetime")]
        public DateTime InboundDate { get; set; }

        public int? InboundStoreNo { get; set; }

        [StringLength(200)]
        public string InboundNotes { get; set; }

        public bool? IsApproval { get; set; }

        public bool? IsClosed { get; set; }
        public short? ReqStatus { get; set; }
        public short? RecStatus { get; set; }

    }
}
