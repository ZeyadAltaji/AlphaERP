using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace AlphaERP.Models
{
    public class ReportParams
    {
        public string ReportTitle { get; set; }
        public DataSet myDataSet { get; set; }
        public string path { get; set; }
        public string ReportName { get; set; }
        public ReportDocument ReportSource { get; set; }
        public ParameterFields fields { get; set; }

    }
}