namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrdCopy
    {
        public int ReqforQuotyear { get; set; }
        public int ReqforQuotNo { get; set; }
        public string ItemNo { get; set; }

    }
}
