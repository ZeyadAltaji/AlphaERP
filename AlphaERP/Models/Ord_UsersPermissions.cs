namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ord_UsersPermissions
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(8)]
        public string UserID { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProgID { get; set; }

        public bool? ProgAccess { get; set; }

        public bool? ProgAdd { get; set; }

        public bool? ProgMod { get; set; }

        public bool? ProgDel { get; set; }

        public virtual Ord_Programs Ord_Programs { get; set; }
    }
}
