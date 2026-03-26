namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Prod_ItemStages
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Comp_no { get; set; }

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
        public int Stage_no { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Stage_code { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short StageSer { get; set; }

        public double? Hr { get; set; }

        public double? Min { get; set; }

        public double? ReqQty { get; set; }

        public short? Repeat { get; set; }

        public short? UnitPcs { get; set; }

        [Column(TypeName = "money")]
        public decimal? StopTime { get; set; }

        [Column(TypeName = "money")]
        public decimal TimePerUnit { get; set; }

        public bool? FixedTime { get; set; }

        public bool? ClosePStage { get; set; }

        public short? Serial1 { get; set; }
        public short? QAProc { get; set; }
        public short? QCFromNo { get; set; }
    }
}
