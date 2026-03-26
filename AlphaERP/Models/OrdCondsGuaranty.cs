namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrdCondsGuaranty")]
    public partial class OrdCondsGuaranty
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short OrdYear { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrderNo { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(20)]
        public string TwareedNo { get; set; }

        public int? PayCond1 { get; set; }

        public int? PayCond2 { get; set; }

        public int? PayCond3 { get; set; }

        public int? PayCond4 { get; set; }

        public int? PayCond5 { get; set; }

        public double? CondPerc1 { get; set; }

        public double? CondPerc2 { get; set; }

        public double? CondPerc3 { get; set; }

        public double? CondPerc4 { get; set; }

        public double? CondPerc5 { get; set; }

        public int? Guaranty1 { get; set; }

        public int? Guaranty2 { get; set; }

        public int? Guaranty3 { get; set; }

        public int? Guaranty4 { get; set; }

        public int? Guaranty5 { get; set; }

        public double? GuarantyPerc1 { get; set; }

        public double? GuarantyPerc2 { get; set; }

        public double? GuarantyPerc3 { get; set; }

        public double? GuarantyPerc4 { get; set; }

        public double? GuarantyPerc5 { get; set; }
    }
}
