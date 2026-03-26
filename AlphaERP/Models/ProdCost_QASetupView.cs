namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public partial class ProdCost_QASetupView
    {
        public short CompNo { get; set; }
    
        public int QA_ProcNo { get; set; }
 
        public short Proc_SubNo { get; set; }
         public string Description { get; set; }
        public DateTime AddDate { get; set; }
       
        public string UserID { get; set; }
    }
}