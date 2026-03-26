namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GLCR
    {
        public short CompNo { get; set; }
        public int DeptNo { get; set; }
        public long AccNo { get; set; }
    }
}
