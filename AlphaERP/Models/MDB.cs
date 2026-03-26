namespace AlphaERP.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Validation;
    using System.Linq;

    public partial class MDB : DbContext
    {
        public MDB()
            : base()
        {
            string Server = System.Configuration.ConfigurationManager.AppSettings.Get("Server");
            Server = Server.Replace(@"\\", @"\");
            string DataBase_ = System.Configuration.ConfigurationManager.AppSettings.Get("DataBase");
            Database.Connection.ConnectionString = string.Format("Server={0};Database={1};User ID=admin;Password=GceSoft01042000", Server, DataBase_);
            Database.SetInitializer<MDB>(null);
        }

        public virtual DbSet<ProdCost_CompPara> ProdCost_CompPara { get; set; }
        public virtual DbSet<ProdCost_DailyProductionD> ProdCost_DailyProductionD { get; set; }
        public virtual DbSet<ProdCost_DailyProductionH> ProdCost_DailyProductionH { get; set; }
        public virtual DbSet<ProdCost_ProductionShift> ProdCost_ProductionShift { get; set; }
        public virtual DbSet<YearPlanDetlsD> YearPlanDetlsD { get; set; }
        public virtual DbSet<YearPlanDetlsH> YearPlanDetlsH { get; set; }
        public virtual DbSet<InvStoresMF> InvStoresMFs { get; set; }
        

        public virtual DbSet<ProdCost_OrderExtended> ProdCost_OrderExtended { get; set; }
        public virtual DbSet<prod_hiring_prepare_info> prod_hiring_prepare_info { get; set; }

        public virtual DbSet<prod_approve_manufacure> prod_approve_manufacure { get; set; }
        public virtual DbSet<prod_prepare_info> prod_prepare_info { get; set; }
        public virtual DbSet<Prod_prepare_info_detail> Prod_prepare_info_detail { get; set; }
        public virtual DbSet<ProdCost_MachineInfo> ProdCost_MachineInfo { get; set; }
        public virtual DbSet<BOM_FinPackingInfo> BOM_FinPackingInfo { get; set; }
        public virtual DbSet<BOM_PakingInfo> BOM_PakingInfo { get; set; }
        public virtual DbSet<GLN_UsersDept> GLN_UsersDept { get; set; }
        public virtual DbSet<GLDEPMF> GLDEPMFs { get; set; }
        public virtual DbSet<ProdCost_StageAccExpLink> ProdCost_StageAccExpLink { get; set; }
        public virtual DbSet<Prod_ItemStagesTestQuality> Prod_ItemStagesTestQuality { get; set; }
        public virtual DbSet<ProdCost_FormTest> ProdCost_FormTest { get; set; }
        public virtual DbSet<ProdCost_Parameter> ProdCost_Parameters { get; set; }
        public virtual DbSet<ProdCost_ParametersHF> ProdCost_ParametersHF { get; set; }
        public virtual DbSet<prod_prodstage_info> prod_prodstage_info { get; set; }
        public virtual DbSet<Prod_ItemStages> Prod_ItemStages { get; set; }
        public virtual DbSet<ProdCost_FormulaProperties> ProdCost_FormulaProperties { get; set; }
        public virtual DbSet<Prod_BomAdditionalRM> Prod_BomAdditionalRM { get; set; }
        public virtual DbSet<prod_extend_item> prod_extend_item { get; set; }
        public virtual DbSet<InvUnitCode> InvUnitCodes { get; set; }
        public virtual DbSet<Prod_BOMGroups> Prod_BOMGroups { get; set; }
        public virtual DbSet<prod_formula_detail_info> prod_formula_detail_info { get; set; }
        public virtual DbSet<UserPermission> UserPermissions { get; set; }
        public virtual DbSet<LicensedModule> LicensedModules { get; set; }

        public virtual DbSet<Alpha_WorkFlowFunctions> Alpha_WorkFlowFunctions { get; set; }
        public virtual DbSet<Alpha_WorkFlowLog> Alpha_WorkFlowLog { get; set; }
        public virtual DbSet<MRP_GeneralPlanSubDtls> MRPGeneralPlanSubDtls { get; set; }
        public virtual DbSet<OnlineUser> OnlineUsers { get; set; }
        public virtual DbSet<Alpha_Language> Languages { get; set; }
        public virtual DbSet<InvItemsMF> InvItemsMFs { get; set; }
        public virtual DbSet<prod_formula_header_info> prod_formula_header_info { get; set; }
        public virtual DbSet<MRP_GeneralPlanInfoD> MRP_GeneralPlanInfoD { get; set; }
        public virtual DbSet<MRP_GeneralPlanInfoH> MRP_GeneralPlanInfoH { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        // public virtual DbSet<MenuProgram> MenuPrograms { get; set; }
        //public virtual DbSet<UsersPermission> UsersPermissions { get; set; }
        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Ord_RequestDF> Ord_RequestDF { get; set; }
        public virtual DbSet<Ord_RequestHF> Ord_RequestHF { get; set; }
        public virtual DbSet<MRP_Web_OrdCopyInfo> MRP_Web_OrdCopyInfo { get; set; }
        public virtual DbSet<OrderDF> OrderDFs { get; set; }
        public virtual DbSet<OrderHF> OrderHFs { get; set; }
        public virtual DbSet<OrdCondsGuaranty> OrdCondsGuaranties { get; set; }
        public virtual DbSet<OrderDFShipDate> OrderDFShipDates { get; set; }
        public virtual DbSet<Ord_POAttachments> Ord_POAttachments { get; set; }
        public virtual DbSet<Ord_OrderShippingDF> Ord_OrderShippingDF { get; set; }
        public virtual DbSet<Ord_OrderShippingHF> Ord_OrderShippingHF { get; set; }
        public virtual DbSet<Vendor> Vendors { get; set; }
        public virtual DbSet<OrdPreOrderDF> OrdPreOrderDFs { get; set; }
        public virtual DbSet<OrdPreOrderHF> OrdPreOrderHFs { get; set; }
        public virtual DbSet<OrdOfferDF> OrdOfferDFs { get; set; }
        public virtual DbSet<OrdOfferHF> OrdOfferHFs { get; set; }
        public virtual DbSet<Ven_bankInfo> Ven_bankInfo { get; set; }
        public virtual DbSet<Ven_Contact> Ven_Contact { get; set; }
        public virtual DbSet<InvCompMF> InvCompMFs { get; set; }
        public virtual DbSet<Ord_PortInfoHF> Ord_PortInfoHF { get; set; }
        public virtual DbSet<Ord_PortInfoDF> Ord_PortInfoDF { get; set; }
        public virtual DbSet<Ord_OrdReceiptDF> Ord_OrdReceiptDF { get; set; }
        public virtual DbSet<Ord_OrdReceiptHF> Ord_OrdReceiptHF { get; set; }
        public virtual DbSet<Ord_VoucIssueDF> Ord_VoucIssueDF { get; set; }
        public virtual DbSet<Ord_VoucIssueHF> Ord_VoucIssueHF { get; set; }
        public virtual DbSet<InvStoreUser> InvStoreUsers { get; set; }
        public virtual DbSet<InvDocType> InvDocTypes { get; set; }
        public virtual DbSet<Invt_OutlaySlaughter> Invt_OutlaySlaughter { get; set; }
        public virtual DbSet<Invt_SlaughterOrderDF> Invt_SlaughterOrderDF { get; set; }
        public virtual DbSet<Invt_SlaughterOrderHF> Invt_SlaughterOrderHF { get; set; }
        public virtual DbSet<InvKit> InvKits { get; set; }
        public virtual DbSet<InvLink> InvLinks { get; set; }
        public virtual DbSet<Ord_InboundsInfoDF> Ord_InboundsInfoDF { get; set; }
        public virtual DbSet<Ord_InboundsInfoHF> Ord_InboundsInfoHF { get; set; }
        public virtual DbSet<Ord_InboundManagementDF> Ord_InboundManagementDF { get; set; }
        public virtual DbSet<Ord_InboundManagementHF> Ord_InboundManagementHF { get; set; }
        public virtual DbSet<OrdRecDF> OrdRecDFs { get; set; }
        public virtual DbSet<OrdRecHF> OrdRecHFs { get; set; }

        public virtual DbSet<Ord_WeightChangeRate> Ord_WeightChangeRate { get; set; }
        public virtual DbSet<ClientsActive> ClientsActives { get; set; }
        public virtual DbSet<Ord_UserActions> Ord_UserActions { get; set; }
        public virtual DbSet<Ord_ReleaseOrdersDF> Ord_ReleaseOrdersDF { get; set; }
        public virtual DbSet<Ord_ReleaseOrdersHF> Ord_ReleaseOrdersHF { get; set; }

        public virtual DbSet<Ord_Menu> Ord_Menu { get; set; }
        public virtual DbSet<Ord_Programs> Ord_Programs { get; set; }
        public virtual DbSet<Ord_UsersPermissions> Ord_UsersPermissions { get; set; }
        public virtual DbSet<Ord_LinkPrchOrdUsers> Ord_LinkPrchOrdUsers { get; set; }
        public virtual DbSet<OrdCompMF> OrdCompMFs { get; set; }
        public virtual DbSet<Ord_PurchOrdExpensesDF> Ord_PurchOrdExpensesDF { get; set; }
        public virtual DbSet<Ord_PurchOrdExpensesHF> Ord_PurchOrdExpensesHF { get; set; }
        public virtual DbSet<AlphaOrd_Email> AlphaOrd_Email { get; set; }
        public virtual DbSet<glactmf> glactmfs { get; set; }
        public virtual DbSet<Ord_LcExpCodes> Ord_LcExpCodes { get; set; }
        public virtual DbSet<Ord_Web_EvalVenodrsDF> Ord_Web_EvalVenodrsDF { get; set; }
        public virtual DbSet<Ord_Web_EvalVenodrsHF> Ord_Web_EvalVenodrsHF { get; set; }
        public virtual DbSet<syscode> syscodes { get; set; }
        public virtual DbSet<GLCOMMF> GLCOMMFs { get; set; }
        public virtual DbSet<Ord_LinkRquestDFAndPO> Ord_LinkRquestDFAndPO { get; set; }
        public virtual DbSet<ProdCost_QASetup> ProdCost_QASetup { get; set; }
        public virtual DbSet<ProdCost_QARequests> ProdCost_QARequests { get; set; }
        public virtual DbSet<ProdCost_QAEvents> ProdCost_QAEvents { get; set; }
        public virtual DbSet<ProdCost_QASetupHF> ProdCost_QASetupHF { get; set; }
        public virtual DbSet<Alpha_BusinessUnitDef> Alpha_BusinessUnitDef { get; set; }
        public virtual DbSet<ProdCost_EmpWorkHoursD_Web> ProdCost_EmpWorkHoursD_Web { get; set; }
        public virtual DbSet<ProdCost_EmpWorkHoursH_Web> ProdCost_EmpWorkHoursH_Web { get; set; }
        public virtual DbSet<ProdCost_QCInspHF> ProdCost_QCInspHF { get; set; }
        public virtual DbSet<Prod_RevisionSetup> Prod_RevisionSetup { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProdCost_QASetupHF>()
               .Property(e => e.QA_Desc)
               .IsUnicode(false);


            modelBuilder.Entity<ProdCost_QASetup>()
                .Property(e => e.CompNo);

            modelBuilder.Entity<ProdCost_QAEvents>()
               .Property(e => e.MUser)
               .IsUnicode(false);

            modelBuilder.Entity<ProdCost_QAEvents>()
                .Property(e => e.CUser)
                .IsUnicode(false);

            modelBuilder.Entity<ProdCost_QAEvents>()
                .Property(e => e.FinalPostUser)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_LinkRquestDFAndPO>()
                .Property(e => e.ReqNo)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_LinkRquestDFAndPO>()
                .Property(e => e.ItemNo)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_LinkRquestDFAndPO>()
                .Property(e => e.PurchaseOrdTawreedNo)
                .IsUnicode(false);
            modelBuilder.Entity<Ord_Web_EvalVenodrsDF>()
               .Property(e => e.CodeId)
               .IsUnicode(false);

            modelBuilder.Entity<Ord_Web_EvalVenodrsHF>()
                .Property(e => e.TawreedNo)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_Web_EvalVenodrsHF>()
                .Property(e => e.doc)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_OrderShippingDF>()
                .Property(e => e.TawreedNo)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_OrderShippingDF>()
                .Property(e => e.ShipSer)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_OrderShippingDF>()
                .Property(e => e.ItemNo)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_OrderShippingDF>()
                .Property(e => e.TUnit)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_LcExpCodes>()
               .Property(e => e.ExpArDesc)
               .IsUnicode(false);

            modelBuilder.Entity<Ord_LcExpCodes>()
                .Property(e => e.ExpEDesc)
                .IsUnicode(false);

            modelBuilder.Entity<glactmf>()
               .Property(e => e.acc_desc)
               .IsUnicode(false);

            modelBuilder.Entity<glactmf>()
                .Property(e => e.acc_edesc)
                .IsUnicode(false);

            modelBuilder.Entity<glactmf>()
                .Property(e => e.acc_refno)
                .IsUnicode(false);

            modelBuilder.Entity<AlphaOrd_Email>()
               .Property(e => e.SmtpAddress)
               .IsUnicode(false);

            modelBuilder.Entity<Ord_PurchOrdExpensesDF>()
                .Property(e => e.TawreedNo)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_PurchOrdExpensesDF>()
                .Property(e => e.Amount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Ord_PurchOrdExpensesDF>()
                .Property(e => e.FrAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OrdCompMF>()
                .Property(e => e.PRISOCode)
                .IsUnicode(false);

            modelBuilder.Entity<OrdCompMF>()
                .Property(e => e.POISOCode)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_LinkPrchOrdUsers>()
                .Property(e => e.ReqNo)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_LinkPrchOrdUsers>()
                .Property(e => e.ItemNo)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_Menu>()
                .Property(e => e.ProgArName)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_Menu>()
                .Property(e => e.ProgEnName)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_Menu>()
                .Property(e => e.SourceForm)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_Programs>()
                .Property(e => e.ProgDesc)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_Programs>()
                .Property(e => e.progEngDesc)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_Programs>()
                .HasMany(e => e.Ord_UsersPermissions)
                .WithRequired(e => e.Ord_Programs)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ord_UsersPermissions>()
                .Property(e => e.UserID)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_ReleaseOrdersDF>()
                .Property(e => e.TawreedNo)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_ReleaseOrdersDF>()
                .Property(e => e.InboundSer)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_ReleaseOrdersDF>()
                .Property(e => e.InboundGRN)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_ReleaseOrdersDF>()
                .Property(e => e.ItemNo)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_ReleaseOrdersDF>()
                .Property(e => e.batchNo)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_ReleaseOrdersDF>()
                .Property(e => e.TUnit)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_ReleaseOrdersHF>()
                .Property(e => e.TawreedNo)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_ReleaseOrdersHF>()
                .Property(e => e.InboundSer)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_ReleaseOrdersHF>()
                .Property(e => e.InboundGRN)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_ReleaseOrdersHF>()
                .Property(e => e.Notes)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_RequestDF>()
               .Property(e => e.ReqNo)
               .IsUnicode(false);

            modelBuilder.Entity<Ord_RequestDF>()
                .Property(e => e.ItemNo)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_RequestDF>()
                .Property(e => e.TUnit)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_RequestDF>()
                .Property(e => e.Note)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_RequestHF>()
                .Property(e => e.ReqNo)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_RequestHF>()
                .Property(e => e.UserID)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_RequestHF>()
                .Property(e => e.Note)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_RequestHF>()
                .Property(e => e.RejectReason)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_RequestHF>()
               .HasMany(e => e.Ord_RequestDF)
               .WithRequired(e => e.Ord_RequestHF)
               .HasForeignKey(e => new { e.CompNo, e.ReqYear, e.ReqNo })
               .WillCascadeOnDelete(false);


            modelBuilder.Entity<MRP_Web_OrdCopyInfo>()
                .Property(e => e.Notes)
                .IsUnicode(false);

            modelBuilder.Entity<MRP_Web_OrdCopyInfo>()
                .Property(e => e.Port)
                .IsUnicode(false);

            modelBuilder.Entity<MRP_Web_OrdCopyInfo>()
                .Property(e => e.SellPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OrderDF>()
                .Property(e => e.TawreedNo)
                .IsUnicode(false);

            modelBuilder.Entity<OrderDF>()
                .Property(e => e.ItemNo)
                .IsUnicode(false);

            modelBuilder.Entity<OrderDF>()
                .Property(e => e.NSItemNo)
                .IsUnicode(false);

            modelBuilder.Entity<OrderDF>()
                .Property(e => e.DlvStateItem)
                .IsUnicode(false);

            modelBuilder.Entity<OrderDF>()
                .Property(e => e.RefNo)
                .IsUnicode(false);

            modelBuilder.Entity<OrderDF>()
                .Property(e => e.PUnit)
                .IsUnicode(false);

            modelBuilder.Entity<OrderDF>()
                .Property(e => e.Note)
                .IsUnicode(false);

            modelBuilder.Entity<OrderDF>()
                .Property(e => e.NoteDtl)
                .IsUnicode(false);

            modelBuilder.Entity<OrderHF>()
                .Property(e => e.TawreedNo)
                .IsUnicode(false);

            modelBuilder.Entity<OrderHF>()
                .Property(e => e.DlvryPlace)
                .IsUnicode(false);

            modelBuilder.Entity<OrderHF>()
                .Property(e => e.Notes)
                .IsUnicode(false);

            modelBuilder.Entity<OrderHF>()
                .Property(e => e.QutationRef)
                .IsUnicode(false);

            modelBuilder.Entity<OrderHF>()
                .Property(e => e.PInCharge)
                .IsUnicode(false);

            modelBuilder.Entity<OrderHF>()
                .Property(e => e.UnitKind)
                .IsUnicode(false);

            modelBuilder.Entity<OrderHF>()
                .Property(e => e.GenNote)
                .IsUnicode(false);

            modelBuilder.Entity<OrderHF>()
                .Property(e => e.GenConditions)
                .IsUnicode(false);

            modelBuilder.Entity<OrderHF>()
                .Property(e => e.SpecialConditions)
                .IsUnicode(false);

            modelBuilder.Entity<OrderHF>()
                .Property(e => e.ShipTerms)
                .IsUnicode(false);

            modelBuilder.Entity<OrderHF>()
                .Property(e => e.Port)
                .IsUnicode(false);

            modelBuilder.Entity<OrderHF>()
                .Property(e => e.FreightExpense)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OrderHF>()
                .Property(e => e.OrdUser)
                .IsUnicode(false);

            modelBuilder.Entity<OrderHF>()
                .Property(e => e.OrdUserDiff)
                .IsUnicode(false);

            modelBuilder.Entity<OrderHF>()
                .Property(e => e.VendorName)
                .IsUnicode(false);

            modelBuilder.Entity<OrderHF>()
                .Property(e => e.ExtraExpenses)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OrdCondsGuaranty>()
                .Property(e => e.TwareedNo)
                .IsUnicode(false);

            modelBuilder.Entity<OrderDFShipDate>()
               .Property(e => e.TawreedNo)
               .IsUnicode(false);

            modelBuilder.Entity<OrderDFShipDate>()
                .Property(e => e.ItemNo)
                .IsUnicode(false);

            modelBuilder.Entity<OrderDFShipDate>()
                .Property(e => e.Qty)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OrderDFShipDate>()
                .HasOptional(e => e.OrderDFShipDate1)
                .WithRequired(e => e.OrderDFShipDate2);

            modelBuilder.Entity<Ord_POAttachments>()
               .Property(e => e.TawreedNo)
               .IsUnicode(false);

            modelBuilder.Entity<Ord_POAttachments>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_POAttachments>()
                .Property(e => e.DescriptionEng)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_POAttachments>()
                .Property(e => e.AttachTypeDesc)
                .IsUnicode(false);


            modelBuilder.Entity<Ord_OrderShippingHF>()
                .Property(e => e.TawreedNo)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_OrderShippingHF>()
                .Property(e => e.ShipSer)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_OrderShippingHF>()
                .Property(e => e.ShippingPolicyNo)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_OrderShippingHF>()
                .Property(e => e.ClearanceInvNo)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_OrderShippingHF>()
                .Property(e => e.ClearanceCompany)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_OrderShippingHF>()
                .Property(e => e.Transporter)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_OrderShippingHF>()
                .Property(e => e.TransportInvNo)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_OrderShippingHF>()
                .Property(e => e.ShippingNotes)
                .IsUnicode(false);

            modelBuilder.Entity<Vendor>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Vendor>()
                .Property(e => e.Eng_Name)
                .IsUnicode(false);

            modelBuilder.Entity<Vendor>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<Vendor>()
                .Property(e => e.Notes)
                .IsUnicode(false);

            modelBuilder.Entity<Vendor>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<Vendor>()
                .Property(e => e.Location)
                .IsUnicode(false);

            modelBuilder.Entity<Vendor>()
                .Property(e => e.Resp_Person)
                .IsUnicode(false);

            modelBuilder.Entity<Vendor>()
                .Property(e => e.Tel1)
                .IsUnicode(false);

            modelBuilder.Entity<Vendor>()
                .Property(e => e.Tel2)
                .IsUnicode(false);

            modelBuilder.Entity<Vendor>()
                .Property(e => e.Mobile_No)
                .IsUnicode(false);

            modelBuilder.Entity<Vendor>()
                .Property(e => e.POBox)
                .IsUnicode(false);

            modelBuilder.Entity<Vendor>()
                .Property(e => e.Postal_Code)
                .IsUnicode(false);

            modelBuilder.Entity<Vendor>()
                .Property(e => e.Fax)
                .IsUnicode(false);

            modelBuilder.Entity<Vendor>()
                .Property(e => e.Telex)
                .IsUnicode(false);

            modelBuilder.Entity<Vendor>()
                .Property(e => e.EMail)
                .IsUnicode(false);

            modelBuilder.Entity<Vendor>()
                .Property(e => e.Pay_Method)
                .IsUnicode(false);

            modelBuilder.Entity<Vendor>()
                .Property(e => e.Disc)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Vendor>()
                .Property(e => e.Penf)
                .IsUnicode(false);

            modelBuilder.Entity<Vendor>()
                .Property(e => e.GenCondition)
                .IsUnicode(false);

            modelBuilder.Entity<Vendor>()
                .Property(e => e.BankAccountNo)
                .IsUnicode(false);

            modelBuilder.Entity<OrdPreOrderDF>()
               .Property(e => e.ItemNo)
               .IsUnicode(false);

            modelBuilder.Entity<OrdPreOrderDF>()
                .Property(e => e.NSItemNo)
                .IsUnicode(false);

            modelBuilder.Entity<OrdPreOrderDF>()
                .Property(e => e.DiscPer)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OrdPreOrderHF>()
                .Property(e => e.DocNo)
                .IsUnicode(false);

            modelBuilder.Entity<OrdPreOrderHF>()
                .Property(e => e.UserID)
                .IsUnicode(false);

            modelBuilder.Entity<OrdPreOrderHF>()
                .Property(e => e.Note)
                .IsUnicode(false);

            modelBuilder.Entity<OrdPreOrderHF>()
                .HasMany(e => e.OrdPreOrderDFs)
                .WithRequired(e => e.OrdPreOrderHF)
                .HasForeignKey(e => new { e.CompNo, e.OrdYear, e.OrderNo })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ven_bankInfo>()
               .Property(e => e.ID)
               .IsFixedLength()
               .IsUnicode(false);

            modelBuilder.Entity<Ven_bankInfo>()
                .Property(e => e.BankName)
                .IsUnicode(false);

            modelBuilder.Entity<Ven_bankInfo>()
                .Property(e => e.Iban)
                .IsUnicode(false);

            modelBuilder.Entity<Ven_Contact>()
                .Property(e => e.ContactName)
                .IsUnicode(false);

            modelBuilder.Entity<InvCompMF>()
                .Property(e => e.RptSrcPath)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_PortInfoHF>()
                .Property(e => e.TawreedNo)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_PortInfoHF>()
                .Property(e => e.ShipSer)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_PortInfoHF>()
                .Property(e => e.PortNotes)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_PortInfoDF>()
                .Property(e => e.TawreedNo)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_PortInfoDF>()
                .Property(e => e.ShipSer)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_PortInfoDF>()
                .Property(e => e.ItemNo)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_PortInfoDF>()
                .Property(e => e.ShippingTUnit)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_PortInfoDF>()
                .Property(e => e.PortTUnit)
                .IsUnicode(false);
            modelBuilder.Entity<Ord_OrdReceiptDF>()
                .Property(e => e.TawreedNo)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_OrdReceiptDF>()
                .Property(e => e.ShipSer)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_OrdReceiptDF>()
                .Property(e => e.ItemNo)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_OrdReceiptDF>()
                .Property(e => e.PortTUnit)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_OrdReceiptDF>()
                .Property(e => e.RecTUnit)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_OrdReceiptHF>()
                .Property(e => e.TawreedNo)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_OrdReceiptHF>()
                .Property(e => e.ShipSer)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_VoucIssueDF>()
                .Property(e => e.TawreedNo)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_VoucIssueDF>()
                .Property(e => e.ItemNo)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_VoucIssueDF>()
                .Property(e => e.ShipSer)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_VoucIssueDF>()
                .Property(e => e.TUnit)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_VoucIssueHF>()
                .Property(e => e.TawreedNo)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_VoucIssueHF>()
                .Property(e => e.ShipSer)
                .IsUnicode(false);

            modelBuilder.Entity<InvStoreUser>()
               .Property(e => e.UserID)
               .IsUnicode(false);

            modelBuilder.Entity<InvDocType>()
               .Property(e => e.DocName)
               .IsUnicode(false);

            modelBuilder.Entity<InvDocType>()
                .Property(e => e.DocNameEng)
                .IsUnicode(false);

            modelBuilder.Entity<Invt_OutlaySlaughter>()
                .Property(e => e.DocEspCode)
                .IsUnicode(false);

            modelBuilder.Entity<Invt_SlaughterOrderDF>()
                .Property(e => e.ItemNo)
                .IsUnicode(false);

            modelBuilder.Entity<Invt_SlaughterOrderDF>()
                .Property(e => e.Batch)
                .IsUnicode(false);

            modelBuilder.Entity<Invt_SlaughterOrderDF>()
                .Property(e => e.TUnit)
                .IsUnicode(false);

            modelBuilder.Entity<Invt_SlaughterOrderHF>()
                .Property(e => e.ItemNo)
                .IsUnicode(false);

            modelBuilder.Entity<Invt_SlaughterOrderHF>()
                .Property(e => e.Batch)
                .IsUnicode(false);

            modelBuilder.Entity<Invt_SlaughterOrderHF>()
                .Property(e => e.TUnit)
                .IsUnicode(false);

            modelBuilder.Entity<InvKit>()
               .Property(e => e.KitCode)
               .IsUnicode(false);

            modelBuilder.Entity<InvKit>()
                .Property(e => e.ItemNo)
                .IsUnicode(false);

            modelBuilder.Entity<InvKit>()
                .Property(e => e.Kit_ItemNo)
                .IsUnicode(false);

            modelBuilder.Entity<InvLink>()
                .Property(e => e.Categ)
                .IsUnicode(false);

            modelBuilder.Entity<InvLink>()
                .Property(e => e.SubCateg)
                .IsUnicode(false);

            modelBuilder.Entity<InvLink>()
                .Property(e => e.SubCateg3)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_InboundsInfoDF>()
               .Property(e => e.InboundSer)
               .IsUnicode(false);

            modelBuilder.Entity<Ord_InboundsInfoDF>()
                .Property(e => e.ItemNo)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_InboundsInfoDF>()
                .Property(e => e.batchNo)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_InboundsInfoDF>()
                .Property(e => e.TUnit)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_InboundsInfoHF>()
                .Property(e => e.TawreedNo)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_InboundsInfoHF>()
                .Property(e => e.InboundSer)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_InboundsInfoHF>()
                .Property(e => e.InboundNotes)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_InboundManagementDF>()
                .Property(e => e.InboundSer)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_InboundManagementDF>()
                .Property(e => e.InboundGRN)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_InboundManagementDF>()
                .Property(e => e.ItemNo)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_InboundManagementDF>()
                .Property(e => e.batchNo)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_InboundManagementDF>()
                .Property(e => e.TUnit)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_InboundManagementHF>()
                .Property(e => e.TawreedNo)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_InboundManagementHF>()
                .Property(e => e.InboundSer)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_InboundManagementHF>()
                .Property(e => e.InboundGRN)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_InboundManagementHF>()
                .Property(e => e.InboundGRNNotes)
                .IsUnicode(false);

            modelBuilder.Entity<OrdRecDF>()
               .Property(e => e.ItemNo)
               .IsUnicode(false);

            modelBuilder.Entity<OrdRecDF>()
                .Property(e => e.Batch)
                .IsUnicode(false);

            modelBuilder.Entity<OrdRecDF>()
                .Property(e => e.TawreedNo)
                .IsUnicode(false);

            modelBuilder.Entity<OrdRecDF>()
                .Property(e => e.RefNo)
                .IsUnicode(false);

            modelBuilder.Entity<OrdRecDF>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OrdRecDF>()
                .Property(e => e.Punit)
                .IsUnicode(false);

            modelBuilder.Entity<OrdRecHF>()
                .Property(e => e.TawreedNo)
                .IsUnicode(false);

            modelBuilder.Entity<OrdRecHF>()
                .Property(e => e.InvNo)
                .IsUnicode(false);

            modelBuilder.Entity<OrdRecHF>()
                .Property(e => e.Notes)
                .IsUnicode(false);

            modelBuilder.Entity<OrdRecHF>()
                .Property(e => e.FileNo)
                .IsUnicode(false);

            modelBuilder.Entity<OrdRecHF>()
                .Property(e => e.OrdUserDiff)
                .IsUnicode(false);

            modelBuilder.Entity<OrdRecHF>()
                .HasMany(e => e.OrdRecDFs)
                .WithRequired(e => e.OrdRecHF)
                .HasForeignKey(e => new { e.CompNo, e.RecYear, e.RecNo })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ord_WeightChangeRate>()
                .Property(e => e.TawreedNo)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_WeightChangeRate>()
                .Property(e => e.ShipSer)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_WeightChangeRate>()
                .Property(e => e.WeightRateNote)
                .IsUnicode(false);

            modelBuilder.Entity<Ord_UserActions>()
               .Property(e => e.UserID)
               .IsUnicode(false);
            modelBuilder.Entity<Alpha_BusinessUnitDef>()
           .Property(e => e.BusUnitDescAr)
           .IsUnicode(false);

            modelBuilder.Entity<Alpha_BusinessUnitDef>()
                .Property(e => e.BusUnitDescEng)
                .IsUnicode(false);
            modelBuilder.Entity<ProdCost_QARequests>()
              .Property(e => e.UserID)
              .IsUnicode(false);
            modelBuilder.Entity<ProdCost_EmpWorkHoursD_Web>()
               .Property(e => e.ActualWorkHours)
               .IsUnicode(false);

            modelBuilder.Entity<ProdCost_EmpWorkHoursH_Web>()
                .Property(e => e.Notes)
                .IsUnicode(false);

            modelBuilder.Entity<ProdCost_EmpWorkHoursH_Web>()
                .HasMany(e => e.ProdCost_EmpWorkHoursD_Web)
                .WithRequired(e => e.ProdCost_EmpWorkHoursH_Web)
                .HasForeignKey(e => new { e.CompNo, e.PrepYear, e.PrepNo, e.TrDate, e.StageCode })
                .WillCascadeOnDelete(false);



            modelBuilder.Entity<ProdCost_QCInspHF>()
                .Property(e => e.Notes)
                .IsUnicode(false);

            modelBuilder.Entity<ProdCost_QCInspHF>()
                .Property(e => e.UserID)
                .IsUnicode(false);
            /*  modelBuilder.Entity<WorkPattern>()
             .HasMany(e => e.WorkPatternUsers)
             .WithRequired(e => e.WorkPattern)
             .HasForeignKey(e => new { e.PatternId, e.FuncId })
             .WillCascadeOnDelete(false);*/

        }

        public void sync<TDest, Td>(TDest ExistFiled, Td NewFiled, bool w = false)
                where Td : class
                where TDest : class
        {
            if (ExistFiled == null) { return; }
            if (NewFiled == null) { return; }
            System.Collections.Generic.IEnumerable<System.Reflection.PropertyInfo> p = ExistFiled.GetType().GetProperties()
                .Where(x => x.Name != "Id" && x.CanRead && x.CanWrite && !x.GetGetMethod().IsVirtual);
            System.Collections.Generic.IEnumerable<System.Reflection.PropertyInfo> pp = NewFiled.GetType().GetProperties()
                .Where(x => x.Name != "Id" && x.CanRead && x.CanWrite && !x.GetGetMethod().IsVirtual);
            System.Collections.Generic.IEnumerable<System.Reflection.PropertyInfo> cp = pp.Join(p, x => x.Name, y => y.Name, (x, y) => x);
            foreach (System.Reflection.PropertyInfo dp in cp)
            {
                System.Reflection.PropertyInfo prop = p.FirstOrDefault(x => x.Name == dp.Name);
                if (w == true)
                {
                    prop.SetValue(ExistFiled, dp.GetValue(NewFiled));
                }
                else
                {
                    if (dp.GetValue(NewFiled) != null)
                    {
                        prop.SetValue(ExistFiled, dp.GetValue(NewFiled));
                    }
                }
            }
            try
            {
                SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (DbEntityValidationResult eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (DbValidationError ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }

            }


        }
    }
}
