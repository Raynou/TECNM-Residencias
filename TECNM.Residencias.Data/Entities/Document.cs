using System;

namespace TECNM.Residencias.Data.Entities
{
    public sealed class Document
    {
        public long Id { get; set; }

        public long StudentId { get; set; }

        public int Type { get; set; }

        public string FullPath { get; set; } = "";

        public string OriginalName { get; set; } = "";

        public long Size { get; set; }

        public string Hash { get; set; } = "";

        public bool Enabled { get; set; } = true;

        public DateTime UpdatedOn { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
