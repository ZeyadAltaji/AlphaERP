namespace AlphaERP.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Alpha_WorkFlowLog
    {
        [Key]
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal TID { get; set; }

        public short? CompNo { get; set; }

        public short? FID { get; set; }

        [StringLength(10)]
        public string K1 { get; set; }

        [StringLength(10)]
        public string K2 { get; set; }

        [StringLength(10)]
        public string K3 { get; set; }

        [StringLength(10)]
        public string K4 { get; set; }

        [StringLength(4)]
        public string RAction { get; set; }

        public short? BU { get; set; }

        [StringLength(10)]
        public string BUUser { get; set; }

        public short? BULvl { get; set; }

        [StringLength(1)]
        public string BUUserAction { get; set; }

        [StringLength(10)]
        public string OwnerUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime DateAdded { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? ActionDate { get; set; }

        [StringLength(300)]
        public string TrDesc { get; set; }

        [StringLength(500)]
        public string UserNotes { get; set; }

        public bool? ExecNot { get; set; }

    }
}
