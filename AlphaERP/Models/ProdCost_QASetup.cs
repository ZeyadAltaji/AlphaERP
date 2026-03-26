namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;
    public partial class ProdCost_QASetup
    {
        [Key]
        [Column(Order = 0)]
        public short CompNo { get; set; }
        [Key]
        [Column(Order = 1)]
        public int QA_ProcNo { get; set; }
        [Key]
        [Column(Order = 2)]
        public short Proc_SubNo { get; set; }
        [MaxLength(255)]
        public string Description { get; set; }
        public DateTime AddDate { get; set; }
        [StringLength(8)]
        public string UserID { get; set; }
    }
}