namespace apptab
{
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class OPA_ANOMALIE
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal NUM { get; set; }

        public int? IDSOCIETE { get; set; }

        public string LIBELLE { get; set; }
    }
}
