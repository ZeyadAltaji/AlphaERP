namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InvLink")]
    public partial class InvLink
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int StoreNo { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(5)]
        public string Categ { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(5)]
        public string SubCateg { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(5)]
        public string SubCateg3 { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short VouType { get; set; }

        [Key]
        [Column(Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CaCr { get; set; }

        [Key]
        [Column(Order = 7)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short DocType { get; set; }

        public short? Acc_VouType { get; set; }

        public int? DB_Dep { get; set; }

        public long? DB_Acc { get; set; }

        public int? CR_Dep { get; set; }

        public long? CR_Acc { get; set; }

        public int? Tax_Dep { get; set; }

        public long? Tax_Acc { get; set; }

        public int? Tax_Dep_DB { get; set; }

        public long? Tax_Acc_DB { get; set; }

        public int? Goods_Dep { get; set; }

        public long? Goods_Acc { get; set; }

        public int? Stock_Dep { get; set; }

        public long? Stock_Acc { get; set; }

        public int? Discount_Dep { get; set; }

        public long? Discount_Acc { get; set; }

        public int? SpTaxD { get; set; }

        public long? SpTaxA { get; set; }

        public int? PerTaxD { get; set; }

        public long? PerTaxA { get; set; }

        public int? AddCost_Dep { get; set; }

        public long? AddCost_Acc { get; set; }
    }
}
