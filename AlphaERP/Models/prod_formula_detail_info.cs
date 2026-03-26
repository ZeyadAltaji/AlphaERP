namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class prod_formula_detail_info
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short comp_no { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string formula_code { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(20)]
        public string item_no { get; set; }

        [ForeignKey("comp_no,rawmaterial_no")]
        public virtual InvItemsMF Material { get; set; }
        [Key]
        [Column(Order = 3)]
        [StringLength(20)]
        public string rawmaterial_no { get; set; }

        public short? stage_no { get; set; }

        public double? lowest_qty { get; set; }

        public double? best_qty { get; set; }

        public byte? UnitSerial { get; set; }

        public bool length { get; set; }

        public bool width { get; set; }

        public bool hieght { get; set; }

        [Column("const")]
        public bool _const { get; set; }

        [Column(TypeName = "money")]
        public decimal? LossPerc { get; set; }

        public short? ItemType { get; set; }
    }
}
