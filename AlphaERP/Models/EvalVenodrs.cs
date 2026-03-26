namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EvalVenodrs
    {
        public string CodeId { get; set; }
        public int Marks { get; set; }
        public string sys_adesc { get; set; }
        public string sys_edesc { get; set; }

    }
}
