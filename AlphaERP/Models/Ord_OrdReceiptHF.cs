namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ord_OrdReceiptHF
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short OrderYear { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrderNo { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(20)]
        public string TawreedNo { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(15)]
        public string ShipSer { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RecNo { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? RecDate { get; set; }

        [StringLength(50)]
        public string CarNo { get; set; }

        [StringLength(500)]
        public string DriverName { get; set; }

        public short? DocType { get; set; }

        public short? CurrNo { get; set; }

        public long? VendorNo { get; set; }

        public int? StoreNo { get; set; }

        [StringLength(200)]
        public string Notes { get; set; }

        public bool? IsApproval { get; set; }
    }
}
