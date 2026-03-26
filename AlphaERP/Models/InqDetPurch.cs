using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlphaERP.Models
{
    public class InqDetPurch
    {
        public int Status { get; set; }
        public DateTime PRFromDate { get; set; }
        public DateTime PRToDate { get; set; }
        public string FromPRNo { get; set; }
        public string ToPRNo { get; set; }
        public string FromUser { get; set; }
        public string ToUser { get; set; }

        public DateTime POFromDate { get; set; }
        public DateTime POToDate { get; set; }
        public string FromPONo { get; set; }
        public string ToPONo { get; set; }
        public string FromPOUser { get; set; }
        public string ToPOUser { get; set; }

        public DateTime PIFromDate { get; set; }
        public DateTime PIToDate { get; set; }
        public string FromPINo { get; set; }
        public string ToPINo { get; set; }

    }
}