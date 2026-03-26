namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LicensedModule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short ModuleID { get; set; }

        [StringLength(20)]
        public string ModuleName { get; set; }

        [StringLength(20)]
        public string ModuleNameEng { get; set; }

        public bool? Licensed { get; set; }
    }
}
