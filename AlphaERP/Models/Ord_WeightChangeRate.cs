namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ord_WeightChangeRate
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
        [StringLength(50)]
        public string ShipSer { get; set; }

        [Key]
        [Column(Order = 5)]
        public int Ser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? WeightRateDate { get; set; }

        public double? WeightRate { get; set; }

        [StringLength(200)]
        public string WeightRateNote { get; set; }

        public double? StandardCost { get; set; }
    }
}
