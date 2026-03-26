using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlphaERP.Models
{
    public partial class ProdOrders
    {
        public short prepare_year { get; set; }
        public int prepare_code { get; set; }
        public DateTime prepare_date { get; set; }
        public string item_no { get; set; }
        public string ItemDesc { get; set; }
        public string ItemDesc_Ara { get; set; }
        public decimal qty_prepare { get; set; }
        public int? StoreNo { get; set; }
        public string StoreName { get; set; }
        public string stage_code { get; set; }
        public bool ClosingStat { get; set; }
        public byte? UnitSerial { get; set; }
        public string TUnit { get; set; }
        public double Qty { get; set; }
        public int serial { get; set; }
    }
}