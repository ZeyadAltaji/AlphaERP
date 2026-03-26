namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public partial class ProdCost_QARequestDataModels
    {
        public short CompNo { get; set; }
        public short OrderYear { get; set; }
        public int OrderNo { get; set; }
        public DateTime ReqDate { get; set; }
        public Int64 ReqNo { get; set; }
        public int QA_ProcNo { get; set; }
        public short Proc_SubNo { get; set; }

        [StringLength(50)]
        public string stage_desc { get; set; }
        public float? qty_prepare { get; set; }
        [StringLength(200)]
        public string ItemDesc_Ara { get; set; }
        [StringLength(100)]
        public string formula_desc { get; set; }
        public byte? UnitSerial { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? prepare_date { get; set; }
        [StringLength(8)]
        public string UserID { get; set; }
        public bool RequestStat { get; set; }
        [StringLength(100)]
        public string unitName { get; set; }
    }
}