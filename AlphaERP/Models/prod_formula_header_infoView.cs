namespace AlphaERP.Models
{
    using System;
  
    public partial class prod_formula_header_infoView
    {
  
       
        public short comp_no { get; set; }
 
        public string formula_code { get; set; }

        public string formula_desc { get; set; }

 
         public string item_no { get; set; }
        public string ItemDesc { get; set; }
        public short GroupId { get; set; }

        public string GroupDesc { get; set; }
        public DateTime? formula_date { get; set; }

        public bool? SusFlag { get; set; }
        public int ALLOrders { get; set; }
        public int ActiveOrders { get; set; }



    }
}