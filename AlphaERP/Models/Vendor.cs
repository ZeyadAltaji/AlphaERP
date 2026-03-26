namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Vendor
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short comp { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long VendorNo { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Eng_Name { get; set; }

        [StringLength(10)]
        public string Title { get; set; }

        public bool? IsHalt { get; set; }

        [StringLength(50)]
        public string Notes { get; set; }

        public short? Pmethod { get; set; }

        public double? Cr_Lim { get; set; }

        public short? Curr { get; set; }

        public short? DelayDays { get; set; }

        public short? VenLevel { get; set; }

        [StringLength(150)]
        public string Address { get; set; }

        public short? Country { get; set; }

        public short? Area { get; set; }

        public short? SArea { get; set; }

        public short? Street_No { get; set; }

        [StringLength(50)]
        public string Location { get; set; }

        [StringLength(50)]
        public string Resp_Person { get; set; }

        [StringLength(20)]
        public string Tel1 { get; set; }

        [StringLength(20)]
        public string Tel2 { get; set; }

        [StringLength(20)]
        public string Mobile_No { get; set; }

        [StringLength(50)]
        public string POBox { get; set; }

        [StringLength(50)]
        public string Postal_Code { get; set; }

        [StringLength(20)]
        public string Fax { get; set; }

        [StringLength(20)]
        public string Telex { get; set; }

        [StringLength(50)]
        public string EMail { get; set; }

        [StringLength(10)]
        public string Pay_Method { get; set; }

        public short? ChqDueDays { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreatDate { get; set; }

        [Column(TypeName = "money")]
        public decimal? Disc { get; set; }

        [StringLength(50)]
        public string Penf { get; set; }

        public bool? Evaluated { get; set; }

        [Column(TypeName = "text")]
        public string GenCondition { get; set; }

        public short? GroupNo { get; set; }

        [StringLength(20)]
        public string BankAccountNo { get; set; }

        public long? TransporterNo { get; set; }

        public bool? Taxable { get; set; }

    }
}
