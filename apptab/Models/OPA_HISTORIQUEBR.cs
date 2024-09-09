namespace apptab.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class OPA_HISTORIQUEBR
    {
        public int ID { get; set; }

        [StringLength(100)]
        public string NUMENREG { get; set; }

        public DateTime? DATEAFB { get; set; }

        public int? IDUSER { get; set; }

        [StringLength(100)]
        public string AFB { get; set; }

        public int? IDSOCIETE { get; set; }
    }
}
