namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ExpCodes
    {
        public short OrdYear { get; set; }
        public int OrderNo { get; set; }
        public string TawreedNo { get; set; }
        public short ExpID { get; set; }
        public string ExpArDesc { get; set; }
        public string ExpEDesc { get; set; }

    }
}
