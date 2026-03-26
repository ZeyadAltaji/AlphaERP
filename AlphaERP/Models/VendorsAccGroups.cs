namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VendorsAccGroups
    {
        public long GroupNo { get; set; }
        public string acc_desc { get; set; }
        public string acc_edesc { get; set; }
        public byte GroupLevel { get; set; }

    }
}
