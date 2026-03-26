namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class UploadFileReleaseOrders
    {
        public short OrderYear { get; set; }
        public int OrderNo { get; set; }
        public string TawreedNo { get; set; }
        public string InboundSer { get; set; }
        public string InboundGRN { get; set; }
        public int ReleaseOrdId { get; set; }
        public int FileId { set; get; }
        public string Description { get; set; }
        public byte[] File { set; get; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public int? FileSize { get; set; }
        public DateTime DateUploded { get; set; }
    }
}
