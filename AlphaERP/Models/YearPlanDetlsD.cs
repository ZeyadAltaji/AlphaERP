namespace AlphaERP.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Mrp_YearPlanDetlsD")]

    public partial class YearPlanDetlsD
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short PlanYear { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(20)]
        public string FormCode { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(20)]
        public string ItemNo { get; set; }

        public double? MQty1 { get; set; }

        public double? MQty2 { get; set; }

        public double? MQty3 { get; set; }

        public double? MQty4 { get; set; }

        public double? MQty5 { get; set; }

        public double? MQty6 { get; set; }

        public double? MQty7 { get; set; }

        public double? MQty8 { get; set; }

        public double? MQty9 { get; set; }

        public double? MQty10 { get; set; }

        public double? MQty11 { get; set; }

        public double? MQty12 { get; set; }
    }
}
