using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlphaERP.Models
{
    public partial class Prod_GetOrderNoteCloseView
    {
        public int prepare_code { get; set; }
        public DateTime prepare_date { get; set; }
        public string item_no { get; set; }
        public string ItemDesc { get; set; }
        public string ItemDesc_Ara { get; set; }
        public float? qty_prepare { get; set; }
        public string emp_prepare { get; set; }
        public bool ClosingStat { get; set; }
        public short prepare_year { get; set; }
        public int? StoreNo { get; set; }
        public string StoreName { get; set; }
        public string stage_code { get; set; }
        public byte? UnitSerial { get; set; }
        public string TUnit { get; set; }



    }
}