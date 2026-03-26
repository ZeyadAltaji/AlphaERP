namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    public partial class QAProceduresDefinitionView
    {
        public IEnumerable<ProdCost_QASetupHF> ProdCost_QASetupHF { get; set; }
        public IEnumerable<Alpha_BusinessUnitDef> Alpha_BusinessUnitDef { get; set; }
        public IEnumerable<ProdCost_QASetup> ProdCost_QASetup { get; set; }
    }
}