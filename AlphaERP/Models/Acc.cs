namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Acc
    {
        public int deptid { get; set; }
        public long AccId { get; set; }
        public string AccDescAr { get; set; }
        public string AccDescEn { get; set; }
    }
}
