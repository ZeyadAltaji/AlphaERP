namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class InvUnitCode
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(5)]
        public string UnitCode { get; set; }

        [StringLength(15)]
        public string UnitName { get; set; }

        [StringLength(15)]
        public string UnitNameEng { get; set; }

        public int? VSUnitCode { get; set; }
    }
}
