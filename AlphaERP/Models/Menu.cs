namespace AlphaERP.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("AlphaERP_Menu")]
    public partial class Menu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProgID { get; set; }

        public short ModuleID { get; set; }

        [StringLength(50)]
        public string ProgArName { get; set; }

        [StringLength(50)]
        public string ProgEnName { get; set; }

        [StringLength(50)]
        public string SourceForm { get; set; }

        [StringLength(50)]
        public string Icon { get; set; }

        public int? ParentID { get; set; }

        [StringLength(6)]
        public string sort { get; set; }

        public bool entry { get; set; }

        public bool inquiry { get; set; }

        public bool report { get; set; }

        public bool addButton { get; set; }

        public bool updateButton { get; set; }

        public bool deleteButton { get; set; }

        [ForeignKey("ParentID")]
        public virtual Menu _parent { get; set; }

    }
}
