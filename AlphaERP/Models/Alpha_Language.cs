namespace AlphaERP.Models
{
    using System.ComponentModel.DataAnnotations;

    public partial class Alpha_Language
    {
        [Key]
        public long TxtID { get; set; }

        [Required]
        [StringLength(250)]
        public string ARTxt { get; set; }

        [StringLength(250)]
        public string ENTxt { get; set; }

    }
}
