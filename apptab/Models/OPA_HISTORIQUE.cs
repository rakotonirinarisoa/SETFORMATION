namespace apptab
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class OPA_HISTORIQUE
    {
        public int ID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NUMENREG { get; set; }

        public DateTime? DATEAFB { get; set; }

        public int? IDUSER { get; set; }

        [StringLength(100)]
        public string AFB { get; set; }

        public int? IDSOCIETE { get; set; }
    }
}
