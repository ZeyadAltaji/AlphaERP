namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GLDEPMF")]
    public partial class GLDEPMF
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short DEP_COMP { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DEP_NUM { get; set; }

        [StringLength(50)]
        public string DEP_NAME { get; set; }

        [StringLength(50)]
        public string DEP_ENAME { get; set; }

        public short DEP_Lev { get; set; }

        public short? DEP_Index { get; set; }

        public int? ParentIndex { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AutoId { get; set; }
    }
}
