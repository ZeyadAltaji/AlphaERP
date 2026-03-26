namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GLCOMMF")]
    public partial class GLCOMMF
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short GLCOM_COMNUM { get; set; }

        public int? GLCOM_LINKAC { get; set; }

        public long? GLCOM_ZERO_ACC { get; set; }

        public int? GLCOM_ASK_ZERO { get; set; }

        public int? GLCOM_CURR { get; set; }

        public int? GLCOM_ASK_CURR { get; set; }

        public int? GLCOM_START_MM { get; set; }

        public DateTime? GLCOM_SETUP { get; set; }

        public int? GLCOM_FIXBAL { get; set; }

        public DateTime? GLCOM_POST { get; set; }

        public DateTime? GLCOM_AUTO { get; set; }

        public DateTime? GLCOM_FIX_DATE { get; set; }

        public int? GLCOM_DRCR { get; set; }

        public int? GLCOM_LINK_INV { get; set; }

        public int? GLCOM_LINK_PAY { get; set; }

        public int? GLCOM_CUSTOMERS { get; set; }

        [StringLength(50)]
        public string GLCOM_NOTE1 { get; set; }

        public DateTime? GLCOM_EXT_START { get; set; }

        public DateTime? GLCOM_EXT_END { get; set; }

        public int? GLCOM_EXT_CHG { get; set; }

        public int? GLCOM_ACC_CLOSE { get; set; }

        public DateTime? GLCOM_ACC_PERIOD { get; set; }

        public int? StartAcc_DEP { get; set; }

        public long? StartAcc_ACC { get; set; }

        public int? GLCOM_rst_allow { get; set; }

        public int? IsBrotFor { get; set; }

        public DateTime? GLCOM_OI_Purg { get; set; }

        public short? GLCOM_Chqs { get; set; }

        public bool? GLCOM_PayInv { get; set; }

        public short? show_acc_num { get; set; }

        public short? show_mon_det { get; set; }

        public short? GlCommStep1 { get; set; }

        public short? GlCommStep2 { get; set; }

        public short? GlCommStep3 { get; set; }

        public short? GlCommStep4 { get; set; }

        public short? Language { get; set; }

        public int? GlPerVchAccNo { get; set; }

        public int? GlPerVchDepNo { get; set; }

        public int? GlPurVchType { get; set; }

        public bool? GLCOM_OI { get; set; }

        public bool? FixSerial { get; set; }

        public short? AccLength { get; set; }

        public bool? UseChkR { get; set; }

        public int? CurrType { get; set; }

        public bool UseCollectSrl { get; set; }

        public bool? PayVenOI { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? GLPayInvDate { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? PayVenOIDate { get; set; }

        public bool? CheckDailyPost { get; set; }

        public bool? HideValueFieldInVoucher { get; set; }

        public short? SettlementVouType { get; set; }

        public bool? UseChqsInCashRec { get; set; }

        public bool? HideActivity { get; set; }

        public bool? AccWithDeptSearch { get; set; }

        public bool? CheckDocNo { get; set; }

        public int? ContractDept { get; set; }

        public long? ContractAcc { get; set; }

        public long? RevenueAcc { get; set; }

        public int? InsuranceDept { get; set; }

        public long? InsuranceAcc { get; set; }

        public int? VisaDept { get; set; }

        public long? VisaAcc { get; set; }

        public int? VisaDept_IND { get; set; }

        public long? VisaAcc_IND { get; set; }

        public int? TicketDept { get; set; }

        public long? TicketAcc { get; set; }

        public int? TicketDept_IND { get; set; }

        public long? TicketAcc_IND { get; set; }

        public int? MedicalDept { get; set; }

        public long? MedicalAcc { get; set; }

        public int? MedicalDept_IND { get; set; }

        public int? HInsuranceDept { get; set; }

        public long? HInsuranceAcc { get; set; }

        public int? HInsuranceDept_IND { get; set; }

        public int? ContractIssueExpDept { get; set; }

        public long? ContractIssueExpAcc { get; set; }

        public int? ContractIssueExpDept_IND { get; set; }

        public int? ContractCompDept { get; set; }

        public int? InsuranceCompDept { get; set; }

        public long? InsuranceAccComp { get; set; }

        public bool? ShowCollector { get; set; }

        public short? AutoVouchType { get; set; }

        public short? SegmentsLvlNo { get; set; }

        public short? SegmentLength { get; set; }
    }
}
