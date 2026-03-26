namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ord_VoucIssueDF
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short OrderYear { get; set; }
        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrderNo { get; set; }
        [Key]
        [Column(Order = 3)]
        [StringLength(20)]
        public string TawreedNo { get; set; }
        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short VouType { get; set; }
        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int VouNo { get; set; }
        [Key]
        [Column(Order = 6)]
        [StringLength(20)]
        public string ItemNo { get; set; }
        [StringLength(15)]
        public string ShipSer { get; set; }
        public double? Qty { get; set; }
        [StringLength(5)]
        public string TUnit { get; set; }
        public short? UnitSerial { get; set; }
        public double? Qty2 { get; set; }
    }
}
