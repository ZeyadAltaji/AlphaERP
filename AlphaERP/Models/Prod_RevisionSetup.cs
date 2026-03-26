namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Prod_RevisionSetup
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }
        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string ItemNo { get; set; }
        [Required]
        [StringLength(50)]
        public string RevisionNo { get; set; }
        public bool? ProductionFlag { get; set; }
        public bool? PurchasingFlag { get; set; }
    }
}
