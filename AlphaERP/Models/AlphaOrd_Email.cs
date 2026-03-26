namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AlphaOrd_Email
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        public string SmtpAddress { get; set; }

        public long? SmtpPort { get; set; }

        public bool? IsUsingSSL { get; set; }

        public bool? IsHtmlBody { get; set; }
    }
}
