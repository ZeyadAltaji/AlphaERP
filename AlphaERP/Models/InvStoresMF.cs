namespace AlphaERP.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("InvStoresMF")]
    public partial class InvStoresMF
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CompNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int StoreNo { get; set; }

        [StringLength(50)]
        public string StoreName { get; set; }

        [StringLength(50)]
        public string StoreNameEng { get; set; }

        public int CashDep { get; set; }

        public long CashAcc { get; set; }

        //  public bool? Employee { get; set; }
        //
        //  public double? valu { get; set; }
        //
        //  public short? StoreType { get; set; }
        //
        //  public bool? AllowSales { get; set; }
        //
        //  public int? CashDep { get; set; }
        //
        //  public long? CashAcc { get; set; }
        //
        //  public int? DeptCode { get; set; }
        //
        //  public bool? WithTax { get; set; }
        //
        //  public short? PriceListNo { get; set; }
        //
        //  [StringLength(20)]
        //  public string VanStoreBarcode { get; set; }
        //
        //  public short? SalesManNo { get; set; }
        //
        //  public bool? IsBonded { get; set; }
        //
        //  public bool? AllowMinuseQty { get; set; }
        //
        //  public bool? UseBufferStore { get; set; }
        //
        //  public int? BufferStore { get; set; }
        //
        //  public bool? AllowReserve { get; set; }
    }
}
