namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ven_bankInfo
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long VendorNo { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(10)]
        public string ID { get; set; }

        [StringLength(50)]
        public string BankName { get; set; }

        public long? AccNo { get; set; }

        [StringLength(50)]
        public string Iban { get; set; }
    }
}
