namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProdCost_QASetupHF
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int QA_ProcNo { get; set; }

        [StringLength(50)]
        public string QA_Desc { get; set; }

        public bool? PostOnSeq { get; set; }

        public short? M_BusUnitID { get; set; }

        public short? C_BusUnitID { get; set; }
    }
}
