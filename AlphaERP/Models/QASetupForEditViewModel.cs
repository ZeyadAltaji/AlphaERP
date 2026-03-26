namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    public partial class QASetupForEditViewModel
    {
        public string QA_Desc { get; set; }

        public int QA_ProcNo { get; set; }

        public bool? PostOnSeq { get; set; }

        public short? M_BusUnitID { get; set; }

        public short? C_BusUnitID { get; set; }

        public string Description { get; set; }
    }
}