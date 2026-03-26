

namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    public partial class ListEventsummary
    {
        public short CompNo { get; set; }
        public short OrderYear { get; set; }
        public int OrderNo { get; set; }
        public long MainReqNo { get; set; }
        public string FinalPostUser { get; set; }
        public DateTime FPostDate { get; set; }
        public int eventserial { get; set; }
        public bool Valu{ get; set; }
    }
}