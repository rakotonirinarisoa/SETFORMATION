namespace apptab
{
    using System.ComponentModel.DataAnnotations;

    public partial class OPA_SOCIETES
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string SOCIETE { get; set; }
    }
}
