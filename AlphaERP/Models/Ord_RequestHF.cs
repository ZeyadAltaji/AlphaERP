namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ord_RequestHF
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ord_RequestHF()
        {
            Ord_RequestDF = new HashSet<Ord_RequestDF>();
        }
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short ReqYear { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(10)]
        public string ReqNo { get; set; }

        public DateTime? ReqDate { get; set; }

        public DateTime? ReqDeliveryDate { get; set; }

        public short? ReqBU { get; set; }

        public short? AuthBU { get; set; }
        [StringLength(10)]
        public string UserIDApprovel { get; set; }

        public short? CurrNo { get; set; }

        [StringLength(10)]
        public string UserID { get; set; }

        public short? ReqStatus { get; set; }

        [StringLength(100)]
        public string Note { get; set; }

        public int? DeptNo { get; set; }

        public long? AccNo { get; set; }

        [StringLength(300)]
        public string RejectReason { get; set; }

        public short? OrderPriority { get; set; }

        [StringLength(200)]
        public string RefNo { get; set; }
        public int? OrderType { get; set; }
        public int? Purpose { get; set; }

        public short? ReqStatusPO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ord_RequestDF> Ord_RequestDF { get; set; }
    }
}
