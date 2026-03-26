namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ord_OrderShippingHF
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ord_OrderShippingHF()
        {
            Ord_OrderShippingDF = new HashSet<Ord_OrderShippingDF>();
        }

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

        [Column(TypeName = "smalldatetime")]
        public DateTime ShippingDate { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? ArrivalDate { get; set; }

        [StringLength(50)]
        public string ShippingPolicyNo { get; set; }

        [StringLength(50)]
        public string ClearanceInvNo { get; set; }

        [StringLength(100)]
        public string ClearanceCompany { get; set; }

        [StringLength(100)]
        public string Transporter { get; set; }

        [StringLength(50)]
        public string TransportInvNo { get; set; }

        [StringLength(200)]
        public string ShippingNotes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ord_OrderShippingDF> Ord_OrderShippingDF { get; set; }
    }
}
