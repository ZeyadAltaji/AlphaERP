using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlphaERP.Models
{
    public partial class EmpStage
    {
        public short Comp_num { get; set; }
        public int Emp_num { get; set; }
        public string EmpName { get; set; }
        public string EmpEngName { get; set; }
        public string WorkPlaceEn { get; set; }
        public string WorkPlaceAr { get; set; }
        public string JobDescrAr { get; set; }
        public string JobDescrEn { get; set; } 
    }
}