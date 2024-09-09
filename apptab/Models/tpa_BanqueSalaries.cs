namespace apptab
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class tpa_BanqueSalaries
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string code_etablissement { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string code_banque { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(10)]
        public string matricule { get; set; }

        [StringLength(5)]
        public string codeGuichet { get; set; }

        [StringLength(2)]
        public string cle_RIB { get; set; }

        [StringLength(50)]
        public string numCompte { get; set; }

        [StringLength(30)]
        public string Agence { get; set; }

        [StringLength(27)]
        public string IBAN { get; set; }

        [StringLength(50)]
        public string codeLibelleBanque { get; set; }

        public decimal? taux { get; set; }
    }
}
