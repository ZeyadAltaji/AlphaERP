namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ord_POAttachments
    {
        [Key]
        [Column(Order = 0)]
        public long Serial { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string TawreedNo { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long OrdNo { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short OrdYear { get; set; }

        [StringLength(300)]
        public string Description { get; set; }

        [StringLength(300)]
        public string DescriptionEng { get; set; }

        public int? AttachType { get; set; }

        [StringLength(50)]
        public string AttachTypeDesc { get; set; }
    }
}
