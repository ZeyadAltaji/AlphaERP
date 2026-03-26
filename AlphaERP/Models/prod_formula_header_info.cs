namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("prod_formula_header_info")]
    public partial class prod_formula_header_info
    {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public prod_formula_header_info()
        {
            prod_formula_detail_info = new HashSet<prod_formula_detail_info>();
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short comp_no { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string formula_code { get; set; }

        [StringLength(100)]
        public string formula_desc { get; set; }

        public short? FormLevel { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? formula_date { get; set; }

        [ForeignKey("comp_no,item_no")]
        public virtual InvItemsMF item { get; set; }
        [StringLength(20)]
        public string item_no { get; set; }

        public byte? UnitSerial { get; set; }

        public int? daily_expiry { get; set; }

        public double? lowest_qty { get; set; }

        public double? best_qty { get; set; }

        public short? Stage_no { get; set; }

        public int? length { get; set; }

        public int? width { get; set; }

        public int? hieght { get; set; }

        public int? widthconst { get; set; }

        public int? heightconst { get; set; }

        public double? Qty1 { get; set; }

        public double? Qty2 { get; set; }

        public double? Qty3 { get; set; }

        [StringLength(8000)]
        public string BomNotes { get; set; }

        [StringLength(20)]
        public string Visc { get; set; }

        [StringLength(20)]
        public string Spec_Gr { get; set; }

        [StringLength(20)]
        public string Grind { get; set; }

        [StringLength(20)]
        public string Covering { get; set; }

        [StringLength(20)]
        public string Gloss { get; set; }

        [StringLength(20)]
        public string Scratch { get; set; }

        [StringLength(20)]
        public string F_Ball { get; set; }

        [StringLength(20)]
        public string BEnding { get; set; }

        [StringLength(20)]
        public string SolVan { get; set; }

        [StringLength(20)]
        public string EVAPort { get; set; }

        [StringLength(20)]
        public string BLush { get; set; }

        [StringLength(20)]
        public string Colo { get; set; }

        [StringLength(20)]
        public string PH { get; set; }

        [StringLength(20)]
        public string Soli { get; set; }

        [StringLength(20)]
        public string Spr { get; set; }

        [StringLength(20)]
        public string Draging { get; set; }

        [StringLength(20)]
        public string Thickness { get; set; }

        [StringLength(20)]
        public string StgWidth { get; set; }

        [StringLength(20)]
        public string Fona { get; set; }

        [StringLength(20)]
        public string StgQty { get; set; }
        public short GroupId { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? ExpiryDate { get; set; }

        public bool? StandardCost { get; set; }

        public bool? SusFlag { get; set; }

        [Column(TypeName = "money")]
        public decimal? Calculated_SG { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<prod_formula_detail_info> prod_formula_detail_info { get; set; }


    }
}
