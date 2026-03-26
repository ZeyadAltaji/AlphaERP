namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AlphaWeb_Menu
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProgID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProjectId { get; set; }

        [StringLength(250)]
        public string ProgArName { get; set; }

        [StringLength(250)]
        public string ProgEnName { get; set; }

        [StringLength(250)]
        public string SourceForm { get; set; }

        public int? sort { get; set; }

        public int? ParentID { get; set; }
    }
}
