namespace AlphaERP.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Alpha_WorkFlowFunctions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Alpha_WorkFlowFunctions()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short FID { get; set; }

        [StringLength(50)]
        public string FDescAr { get; set; }

        [StringLength(50)]
        public string FDescEn { get; set; }

        [StringLength(50)]
        public string SourceForm { get; set; }

    }
}
