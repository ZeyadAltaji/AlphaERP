namespace AlphaERP.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class OnlineUser
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(8)]
        public string UserID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        public DateTime Date { get; set; }

        [Required]
        [StringLength(50)]
        public string Ip { get; set; }
        [ForeignKey("CompNo")]
        public virtual Company Company { get; set; }
  
    }
}
