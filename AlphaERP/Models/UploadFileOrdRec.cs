namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class UploadFileOrdRec
    {
        public short OrderYear { get; set; }
        public int OrderNo { get; set; }
        public string TawreedNo { get; set; }
        public string ShipSer { get; set; }
        public int FileId { set; get; }
        public int RecNo { set; get; }
        public string Description { get; set; }
        public byte[] File { set; get; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public int? FileSize { get; set; }
        public DateTime DateUploded { get; set; }
    }
}
