namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OutlaySlaugCode
    {
        public string MinCode { get; set; }
        public string CodeDesc { get; set; }
        public string CodeDescEng { get; set; }
        public double DocEspVal { get; set; }
    }
}
