namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrdPreOrderHF")]
    public partial class OrdPreOrderHF
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrdPreOrderHF()
        {
            OrdPreOrderDFs = new HashSet<OrdPreOrderDF>();
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Year { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrderNo { get; set; }

        public DateTime? OrderDate { get; set; }

        public bool? OrdApproved { get; set; }

        public int? OrderPerpose { get; set; }

        [StringLength(20)]
        public string DocNo { get; set; }

        public int? OrderSource { get; set; }

        public int? OrderType { get; set; }

        public short? OrderCateg { get; set; }

        [StringLength(10)]
        public string UserID { get; set; }

        public int? OperationType { get; set; }

        public DateTime? OperationDate { get; set; }

        public DateTime? SiteDate { get; set; }

        public bool? StockItem { get; set; }

        public int? BuyerNo { get; set; }

        public int? OrdOrg { get; set; }

        public bool? Confirmation { get; set; }

        public int? CurrType { get; set; }

        public short? BusUnitID { get; set; }

        public int? BudDeptNo { get; set; }

        public long? BudAccNo { get; set; }

        public short? PlanYear { get; set; }

        public int? PlanNo { get; set; }

        public bool? BelowMinCost { get; set; }

        [StringLength(100)]
        public string Note { get; set; }

        public short? AgreeYear { get; set; }

        public long? AgreeNo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrdPreOrderDF> OrdPreOrderDFs { get; set; }
    }
}
