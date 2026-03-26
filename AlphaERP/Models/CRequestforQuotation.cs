namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CRequestforQuotation
    {
        public long VendorNo { get; set; }
        public string VendorName { get; set; }
    }
}
