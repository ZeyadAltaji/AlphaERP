namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BusunisUnit
    {
        public short? Id { get; set; }
        public long? VenderId { get; set; }
        public string DescAr { get; set; }
        public string DescEn { get; set; }
        public string Resp_Person { get; set; }
    }
}
