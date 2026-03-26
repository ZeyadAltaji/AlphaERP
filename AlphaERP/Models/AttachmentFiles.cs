namespace AlphaERP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AttachmentFiles
    {
        public Int64 Serial { get; set; }
        public string FileName { get; set; }
        public string DateUploded { get; set; }
        public int FileSize { get; set; }
        public string ContentType { get; set; }
        public byte[] ArchiveData { get; set; }
    }
}
