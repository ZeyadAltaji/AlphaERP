using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlphaERP.Models
{
    public class InvStoresMFView
    {
        public short CompNo { get; set; }
        public short PrepYear { get; set; }
        public short OrdYear { get; set; }
        public int PrepNo { get; set; }
        public int OrdNo { get; set; }
        public int StoreNo { get; set; }
        public DateTime OrdDate { get; set; }

        public string DocNo { get; set; }
        public short BusUnitID { get; set; }
        public bool ByStage { get; set; }
        public bool AddititiveItems { get; set; }
        public int StageCode { get; set; }
        public bool? IsApproved { get; set; }
    }
}