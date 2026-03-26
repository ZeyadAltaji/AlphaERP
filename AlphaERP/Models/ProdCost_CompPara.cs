namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProdCost_CompPara
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        public short? YWDays { get; set; }

        public short? DWHours { get; set; }

        public bool? UsePackItem { get; set; }

        public bool? FixSerial { get; set; }

        public short? GlVouType { get; set; }

        public bool? RmClosing { get; set; }

        public bool? ProdCtl { get; set; }

        public int? RMDept { get; set; }

        public long? RMAcc { get; set; }

        public int? WiPDept { get; set; }

        public long? WIPAcc { get; set; }

        [StringLength(50)]
        public string ProdISOCode { get; set; }

        public bool? ControlRecPerc { get; set; }

        [Column(TypeName = "money")]
        public decimal? ProdRecPerc { get; set; }

        public short? SerialType { get; set; }

        public short? QCBU { get; set; }

        public bool? UseQcTests { get; set; }

        public bool? TransByOrder { get; set; }

        public int? PackVarDept { get; set; }

        public long? PackVarAcc { get; set; }

        public bool? ShowHideStoreDailyProd { get; set; }

        [Column(TypeName = "money")]
        public decimal? OHPerUnit { get; set; }

        public bool? UseSchudualSalesOrder { get; set; }

        public bool? BlockSerUnlockingStages { get; set; }
    }
}
