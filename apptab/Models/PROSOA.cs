using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace apptab.Models
{
    public class PROSOA
    {
        public int? PROJET { get; set; }

        public int? SOA { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DELETIONDATE { get; set; }
    }
}