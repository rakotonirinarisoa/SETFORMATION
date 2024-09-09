namespace apptab
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class RJL1
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RJL1()
        {
            MCOMPTA = new HashSet<MCOMPTA>();
        }

        [Key]
        [StringLength(10)]
        public string CODE { get; set; }

        [StringLength(100)]
        public string LIBELLELONG { get; set; }

        [StringLength(60)]
        public string LIBELLE { get; set; }

        [StringLength(100)]
        public string LIBELLELONGM { get; set; }

        [StringLength(60)]
        public string LIBELLEM { get; set; }

        public bool JLTRESOR { get; set; }

        [StringLength(20)]
        public string COMPTEASSOCIE { get; set; }

        public bool GESTIONDEVISE { get; set; }

        [StringLength(3)]
        public string CODEDEVISE { get; set; }

        public bool EXCLUDRF { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NUMEROBR { get; set; }

        [StringLength(10)]
        public string FINASSOCIE { get; set; }

        [StringLength(2)]
        public string MODELE { get; set; }

        [StringLength(10)]
        public string PLANCONSOLIDE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal INCREMENTATIONAUTO { get; set; }

        public DateTime? DATECRE { get; set; }

        public DateTime? DATEMAJ { get; set; }

        [StringLength(10)]
        public string USERCRE { get; set; }

        [StringLength(10)]
        public string USERMAJ { get; set; }

        [StringLength(10)]
        public string SIGNATAIRE { get; set; }

        public bool? STATUT { get; set; }

        public bool? GERERPREFIXE { get; set; }

        [StringLength(10)]
        public string VALEURPREFIXE { get; set; }

        public bool? GERERANNEE { get; set; }

        public bool? GERERMOIS { get; set; }

        [StringLength(4)]
        public string NATURE { get; set; }

        [StringLength(10)]
        public string BANQUE { get; set; }

        [StringLength(50)]
        public string AGENCE { get; set; }

        [StringLength(50)]
        public string GUICHET { get; set; }

        [StringLength(50)]
        public string RIB { get; set; }

        [StringLength(50)]
        public string IBAN { get; set; }

        public bool? VUE { get; set; }

        public bool? TERME { get; set; }

        [StringLength(2)]
        public string CLE { get; set; }

        [StringLength(20)]
        public string AUXIASSOCIE { get; set; }

        public bool? TYPEORDONNANCEMENT { get; set; }

        public bool? TYPECAA { get; set; }

        public bool? NUMDEMANDECAAUNIQUE { get; set; }

        public bool? GERERSITE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MCOMPTA> MCOMPTA { get; set; }
    }
}
