namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProdCost_QARequests
    {
        public short? CompNo { get; set; }

        public short? OrderYear { get; set; }

        public int? OrderNo { get; set; }

        [Key]
        public long ReqNo { get; set; }

        public int? QA_ProcNo { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? ReqDate { get; set; }

        public int? StageCode { get; set; }

        public short? StageSerial { get; set; }

        [StringLength(20)]
        public string UserID { get; set; }

        public bool? RequestStat { get; set; }

 
     }
}
