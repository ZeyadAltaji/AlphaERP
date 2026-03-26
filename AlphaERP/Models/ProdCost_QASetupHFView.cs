using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlphaERP.Models
{
    public partial class ProdCost_QASetupHFView
    {

 
        public short CompNo { get; set; }

 
        public int QA_ProcNo { get; set; }

         public string QA_Desc { get; set; }

        public bool? PostOnSeq { get; set; }

        public short? M_BusUnitID { get; set; }

        public short? C_BusUnitID { get; set; }
        public int? TotalQASteps { get; set; }
        public string MakerBusUnitDesc { get; set; }
        public string CheckerBusUnitDesc { get; set; }
        public int UsedInBOMs { get; set; }
    }
}