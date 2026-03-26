namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PayCodes
    {
        public int Id { get; set; }
        public short SYS_MINORId { get; set; }
        public string DescAr { get; set; }
        public string DescEn { get; set; }
        public string sys_minor { get; set; }

        public short? BULvl { get; set; }

    }
}
