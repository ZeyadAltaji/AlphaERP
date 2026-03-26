namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Prod_ItemStagesTestQuality
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string Formula_code { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string Item_no { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Stage_code { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short StageSer { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short TestNo { get; set; }

        [Column(TypeName = "money")]
        public decimal FromNo { get; set; }

        [Column(TypeName = "money")]
        public decimal ToNo { get; set; }

        public int? Serial1 { get; set; }
    }
}
