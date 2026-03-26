namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProdCost_DailyProductionH
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProdCost_DailyProductionH()
        {
            ProdCost_DailyProductionD = new HashSet<ProdCost_DailyProductionD>();
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short ReportYear { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ReportNo { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Prod_Date { get; set; }

        public short? ShiftNo { get; set; }

        public int? Prod_stage { get; set; }

        public int? MachineNo { get; set; }

        public bool? Closed { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProdCost_DailyProductionD> ProdCost_DailyProductionD { get; set; }
    }
}
