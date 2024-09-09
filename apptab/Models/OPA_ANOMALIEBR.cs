namespace apptab
{
    using System.ComponentModel.DataAnnotations;

    public partial class OPA_ANOMALIEBR
    {
        [Key]
        [StringLength(100)]
        public string NUM { get; set; }

        public int? IDSOCIETE { get; set; }
    }
}
