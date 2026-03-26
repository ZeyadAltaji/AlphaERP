namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Prod_prepare_info_detail
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short comp_no { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short prepare_year { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int prepare_code { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(20)]
        public string item_no { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(20)]
        public string rawmaterial_no { get; set; }
        public string RevisionNo { get; set; }

        public double? iqty { get; set; }

        public double? rqty { get; set; }

        public double? StockQty { get; set; }

        public double? RservedQty { get; set; }

        public double? RecQty { get; set; }

        public double? costqty { get; set; }

        public short? UnitSerial { get; set; }

        [StringLength(300)]
        public string NoteDet { get; set; }

        public virtual prod_prepare_info prod_prepare_info { get; set; }
    }
}
