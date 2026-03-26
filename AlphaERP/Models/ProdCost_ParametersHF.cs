namespace AlphaERP.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("ProdCost_ParametersHF")]
    public partial class ProdCost_ParametersHF
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProdCost_ParametersHF()
        {
            ProdCost_Parameters = new HashSet<ProdCost_Parameter>();
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short ParmID { get; set; }

        [StringLength(50)]
        public string LocalDesc { get; set; }

        [StringLength(50)]
        public string EngDesc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProdCost_Parameter> ProdCost_Parameters { get; set; }
    }
}
