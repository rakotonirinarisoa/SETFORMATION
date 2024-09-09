namespace apptab
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class SI_TRAITANNUL
    {
        public int ID { get; set; }

        public Guid? No { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DATEANNUL { get; set; }

        [StringLength(50)]
        public string MOTIF { get; set; }

        public string COMMENTAIRE { get; set; }

        public int? IDPROJET { get; set; }

        public int? IDUSER { get; set; }
    }
}
