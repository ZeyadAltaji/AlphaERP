namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ord_Menu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProgID { get; set; }

        [StringLength(100)]
        public string ProgArName { get; set; }

        [StringLength(100)]
        public string ProgEnName { get; set; }

        [StringLength(50)]
        public string SourceForm { get; set; }

        public short? Flevel { get; set; }

        public short? Loc1 { get; set; }

        public short? Loc2 { get; set; }

        public short? Loc3 { get; set; }

        public short? Loc4 { get; set; }

        public short? Loc5 { get; set; }

        public int ProgNo { get; set; }

        public int ParentID { get; set; }
    }
}
