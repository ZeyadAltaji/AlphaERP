namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProdCost_FormTest
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FormNo { get; set; }

        [StringLength(50)]
        public string FormDesc { get; set; }

        [StringLength(150)]
        public string Notes { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short TestNo { get; set; }

        public string FromNo { get; set; }

        public string ToNo { get; set; }

        public string AlertFromNo { get; set; }

        public string AlertToNo { get; set; }

        public short? AlertBU { get; set; }

        public short? RangType { get; set; }


    }
}
