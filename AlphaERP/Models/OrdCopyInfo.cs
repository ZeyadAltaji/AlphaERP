namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrdCopyInfo
    {
        public int ReqforQuotyear { get; set; }
        public int ReqforQuotNo { get; set; }
        public short? ReqYear { get; set; }
        public string ReqNo { get; set; }
        public DateTime? SellDate { get; set; }
        public long? VendorNo { get; set; }
        public string ItemNo { get; set; }

    }
}
