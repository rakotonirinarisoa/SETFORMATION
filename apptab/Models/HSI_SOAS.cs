namespace apptab.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class HSI_SOAS
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string SOA { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DELETIONDATE { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DATECREA { get; set; }

        public int? IDUSER { get; set; }

        public int? IDPARENT { get; set; }
    }
}
