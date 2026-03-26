namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PROrder
    {
        public int OrderNo { get; set; }
        public string TawreedNo { get; set; }
        public DateTime OrderDate { get; set; }

        public string BuyerName { get; set; }
        public string BuyerName_Eng { get; set; }

        public string DlvState { get; set; }
        public string DlvState_Eng { get; set; }
        public long VendorNo { get; set; }
        public string VenName { get; set; }
        public string VenName_Eng { get; set; }
    }
}
