namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class InvStoreUser
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int StoreNo { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(8)]
        public string UserID { get; set; }

        public bool? AllPerm { get; set; }

        public bool? StartBal { get; set; }

        public bool? RecV { get; set; }

        public bool? RetRecV { get; set; }

        public bool? ProdRecV { get; set; }

        public bool? SalV { get; set; }

        public bool? RetSalV { get; set; }

        public bool? IssV { get; set; }

        public bool? RetIssV { get; set; }

        public bool? AddV { get; set; }

        public bool? SubV { get; set; }

        public bool? WriV { get; set; }

        public bool? ConsV { get; set; }

        public bool? ConsVTo { get; set; }

        public bool? DeliveryNote { get; set; }

        public bool? SalesOrder { get; set; }

        public bool? ConsOrd { get; set; }

        public bool? ConsOrdTo { get; set; }
    }
}
