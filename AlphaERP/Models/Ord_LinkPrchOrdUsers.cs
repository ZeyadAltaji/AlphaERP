namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ord_LinkPrchOrdUsers
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short ReqYear { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(10)]
        public string ReqNo { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(20)]
        public string ItemNo { get; set; }

        [StringLength(50)]
        public string UserId { get; set; }
    }
}
