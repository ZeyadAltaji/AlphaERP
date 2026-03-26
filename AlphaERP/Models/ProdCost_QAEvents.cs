namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProdCost_QAEvents
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long MainReqNo { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short EventSerial { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Proc_MainNo { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Proc_SubNo { get; set; }

        public byte? MakerValue { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? MDate { get; set; }

        [StringLength(8)]
        public string MUser { get; set; }
        public string MakerReadValue { get; set; }

        public bool? MPostStat { get; set; }

        public byte? CheckerValue { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CDate { get; set; }

        [StringLength(8)]
        public string CUser { get; set; }
        public string CheckerReadValue { get; set; }

        public bool? CPostStat { get; set; }

        [StringLength(8)]
        public string FinalPostUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FPostDate { get; set; }
        public bool? FinalPostStat { get; set; }

    }
}
