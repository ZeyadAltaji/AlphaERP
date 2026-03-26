namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BOM_PakingInfo
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string FormCode { get; set; }

        [ForeignKey("CompNo,FinItem")]
        public virtual InvItemsMF FinItems { get; set; }
        [Key]
        [Column(Order = 2)]
        [StringLength(20)]
        public string FinItem { get; set; }

        [ForeignKey("CompNo,PackItem")]
        public virtual InvItemsMF PackItems { get; set; }
        [Required]
        [StringLength(20)]
        public string PackItem { get; set; }

        public double? Capacity { get; set; }

        public bool? IsLiter { get; set; }
    }
}
