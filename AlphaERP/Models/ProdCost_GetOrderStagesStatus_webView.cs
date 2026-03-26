namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;
    public partial class ProdCost_GetOrderStagesStatus_webView
    {
        public short CompNo { get; set; }
        public int OrderYear { get; set; }
        public int OrderNo { get; set; }
        public short StageSer { get; set; }
        [StringLength(5)]
        public string stage_code { get; set; }

        [StringLength(50)]
        public string stage_desc { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? begin_date { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? Closed_Date { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Closed_Time { get; set; }
    }
}