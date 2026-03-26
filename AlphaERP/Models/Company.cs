namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("COMPANY")]
    public partial class Company
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short comp_num { get; set; }

        [StringLength(50)]
        public string comp_name { get; set; }

        [StringLength(50)]
        public string comp_ename { get; set; }

        [StringLength(50)]
        public string comp_addr1 { get; set; }

        [Column(TypeName = "image")]
        public byte[] comp_logo { get; set; }

    }
}
