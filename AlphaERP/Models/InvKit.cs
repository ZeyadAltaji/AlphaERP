namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class InvKit
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string KitCode { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(20)]
        public string ItemNo { get; set; }

        public double? Qty { get; set; }

        [StringLength(20)]
        public string Kit_ItemNo { get; set; }

        public double? Kit_Qty { get; set; }

        public double Factor { get; set; }

        public short? MainUnitSerial { get; set; }

        public short? UnitSerial { get; set; }

        public double? CostPercent { get; set; }
    }
}
