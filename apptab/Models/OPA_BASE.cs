namespace apptab
{
    using System.ComponentModel.DataAnnotations;

    public partial class OPA_BASE
    {
        public int ID { get; set; }

        [StringLength(100)]
        public string NOMBASE { get; set; }

        public int? INCREMENTATION { get; set; }

        public int? INCRORDREVIR { get; set; }

        public int? IDSOCIETE { get; set; }
    }
}
