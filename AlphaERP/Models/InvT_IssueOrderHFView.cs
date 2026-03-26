using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlphaERP.Models
{
    public partial class InvT_IssueOrderHFView
    {
        public string DocNo { get; set; }
        public bool IsApproved { get; set; }
        public short BusUnitID { get; set; }
        public DateTime OrdDate { get; set; }
        public int OrdNo { get; set; }
        public int StoreNo { get; set; }
        public short OrdYear { get; set; }
        public int StageCode { get; set; }
        public bool AddititiveItems { get; set; }
        public bool ByStage { get; set; }


    }
}