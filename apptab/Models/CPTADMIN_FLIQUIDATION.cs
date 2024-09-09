namespace apptab
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class CPTADMIN_FLIQUIDATION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CPTADMIN_FLIQUIDATION()
        {
            CPTADMIN_MLIQUIDATION = new HashSet<CPTADMIN_MLIQUIDATION>();
            CPTADMIN_MLIQUIDATIONPJ = new HashSet<CPTADMIN_MLIQUIDATIONPJ>();
        }

        public Guid ID { get; set; }

        [Required]
        [StringLength(2)]
        public string SITE { get; set; }

        [StringLength(50)]
        public string CODEMARCHE { get; set; }

        [StringLength(255)]
        public string DESCRIPTION { get; set; }

        public DateTime? DATELIQUIDATION { get; set; }

        [StringLength(20)]
        public string COGEBENEFICIAIRE { get; set; }

        [StringLength(20)]
        public string AUXIBENEFICIAIRE { get; set; }

        [StringLength(3)]
        public string DEVISE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? COURSDEVISE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? COURSRAPPORT { get; set; }

        [StringLength(20)]
        public string NUMEROFACTURE { get; set; }

        public DateTime? DATEFACTURE { get; set; }

        [StringLength(50)]
        public string NORDPEC { get; set; }

        [StringLength(50)]
        public string NUMEROOP { get; set; }

        [StringLength(50)]
        public string NUMEROOP2 { get; set; }

        [StringLength(50)]
        public string NUMEROOP3 { get; set; }

        public DateTime? DATESERVICEFAIT { get; set; }

        [StringLength(10)]
        public string USERCRE { get; set; }

        public DateTime? DATECRE { get; set; }

        [StringLength(10)]
        public string USERMAJ { get; set; }

        public DateTime? DATEMAJ { get; set; }

        [StringLength(50)]
        public string NUMEROCA { get; set; }

        [StringLength(20)]
        public string ACTIVITE { get; set; }

        [StringLength(20)]
        public string FINANCEMENT { get; set; }

        [StringLength(20)]
        public string CATEGORIE { get; set; }

        [StringLength(20)]
        public string TYPEENGAGEMENT { get; set; }

        [StringLength(20)]
        public string TYPEPROCEDURE { get; set; }

        public DateTime? DATETAPE1 { get; set; }

        public DateTime? DATETAPE2 { get; set; }

        public DateTime? DATETAPE3 { get; set; }

        public DateTime? DATETAPE4 { get; set; }

        public DateTime? DATETAPE5 { get; set; }

        public DateTime? DATETAPE6 { get; set; }

        public DateTime? DATETAPE7 { get; set; }

        public DateTime? DATETAPE8 { get; set; }

        public DateTime? DATETAPE9 { get; set; }

        public DateTime? DATETAPE10 { get; set; }

        [StringLength(50)]
        public string ETAPE1USER { get; set; }

        [StringLength(50)]
        public string ETAPE2USER { get; set; }

        [StringLength(50)]
        public string ETAPE3USER { get; set; }

        [StringLength(50)]
        public string ETAPE4USER { get; set; }

        [StringLength(50)]
        public string ETAPE5USER { get; set; }

        [StringLength(50)]
        public string ETAPE6USER { get; set; }

        [StringLength(50)]
        public string ETAPE7USER { get; set; }

        [StringLength(50)]
        public string ETAPE8USER { get; set; }

        [StringLength(50)]
        public string ETAPE9USER { get; set; }

        [StringLength(50)]
        public string ETAPE10USER { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CPTADMIN_MLIQUIDATION> CPTADMIN_MLIQUIDATION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CPTADMIN_MLIQUIDATIONPJ> CPTADMIN_MLIQUIDATIONPJ { get; set; }
    }
}
