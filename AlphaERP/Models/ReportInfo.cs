using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.Security.Cryptography;
using System.Text;

namespace AlphaERP.Models
{
    public class ReportInformation
    {
        public string Id { get; set; }
        public DataSet myDataSet { get; set; }
        public string path { get; set; }
        public string ReportName { get; set; }
        public ReportDocument ReportSource { get; set; }
        public ParameterFields fields { get; set; }
    }

    public static class ReportInfoManager
    {
        private static Dictionary<string, ReportInformation> dicReports = new Dictionary<string, ReportInformation>();

        public static void AddReport(string uniqueName, ReportInformation report)
        {
            string reportId = CalculateMD5Hash(uniqueName);
            report.Id = reportId;

            if (dicReports.ContainsKey(reportId))
            {
                dicReports.Remove(reportId);
                dicReports.Add(reportId, report);
                return;
            }
          
            dicReports.Add(reportId, report);
        }

        public static void ClearReports()
        {
            dicReports.Clear();
        }
        public static ReportInformation GetReport(string reportId)
        {
            if (dicReports.ContainsKey(reportId))
            {
                return dicReports[reportId];
            }

            return null;
        }

        private static string CalculateMD5Hash(string input)
        {
            return Guid.NewGuid().ToString();


            // step 1, calculate MD5 hash from input

            //MD5 md5 = System.Security.Cryptography.MD5.Create();

            //byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);

            //byte[] hash = md5.ComputeHash(inputBytes);

            //// step 2, convert byte array to hex string

            //StringBuilder sb = new StringBuilder();

            //for (int i = 0; i < hash.Length; i++)
            //{
            //    sb.Append(hash[i].ToString("X2"));
            //}

        }


    }

}