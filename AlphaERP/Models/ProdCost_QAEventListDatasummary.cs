namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.ComponentModel.DataAnnotations;
    public partial class ProdCost_QAEventListDatasummary
    {
        public short CompNo { get; set; }
        public int OderNo { get; set; }
        public short OrderYear { get; set; }
        public Int64? MainReqNo { get; set; }
        public string FinalPostUser { get; set; } = string.Empty;
        public DateTime? FPostDate { get; set; }
        public DateTime? reqdate { get; set; }
        public short eventserial { get; set; }
        public bool? Valu { get; set; }



    }
}