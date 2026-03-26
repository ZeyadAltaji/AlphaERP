namespace AlphaERP.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("AlphaERP_UserPermissions")]
    public partial class UserPermission
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short ModuleID { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(8)]
        public string UserID { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProgID { get; set; }

        public bool? ProgAccess { get; set; }

        public bool? ProgAdd { get; set; }

        public bool? ProgMod { get; set; }

        public bool? ProgDel { get; set; }

        [ForeignKey("ProgID")]
        public virtual Menu Menu { get; set; }
    }
}
