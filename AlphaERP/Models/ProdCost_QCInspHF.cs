namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProdCost_QCInspHF
    {
        [Key]
        public long RefNo { get; set; }

        public short CompNo { get; set; }

        public short OrderYear { get; set; }

        public int OrderNo { get; set; }

        public short StageCode { get; set; }

        public short StageSer { get; set; }

        public short? QCProNo { get; set; }

        [StringLength(250)]
        public string Notes { get; set; }

        public bool? QCStatus { get; set; }

        public bool? QCPassed { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? ActionDate { get; set; }

        [StringLength(10)]
        public string UserID { get; set; }
    }
}
