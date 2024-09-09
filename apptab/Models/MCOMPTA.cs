namespace apptab
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MCOMPTA")]
    public partial class MCOMPTA
    {
        [Key]
        [Column(TypeName = "numeric")]
        public decimal NUMENREG { get; set; }

        [StringLength(10)]
        public string JL { get; set; }

        [Required]
        [StringLength(2)]
        public string SITE { get; set; }

        [StringLength(14)]
        public string NORD { get; set; }

        public DateTime DATCLE { get; set; }

        public DateTime? DATPIECE { get; set; }

        [StringLength(30)]
        public string NOPIECE { get; set; }

        [StringLength(255)]
        public string LIBELLE { get; set; }

        [StringLength(20)]
        public string COGE { get; set; }

        [StringLength(20)]
        public string AUXI { get; set; }

        [StringLength(50)]
        public string COGEAUXI { get; set; }

        [StringLength(20)]
        public string CP { get; set; }

        [StringLength(50)]
        public string CPAUXI { get; set; }

        [StringLength(20)]
        public string POSTE { get; set; }

        [StringLength(10)]
        public string CONVENTION { get; set; }

        [StringLength(3)]
        public string CATEGORIE { get; set; }

        [StringLength(3)]
        public string SOUSCATEGORIE { get; set; }

        [StringLength(20)]
        public string ACTI { get; set; }

        [StringLength(20)]
        public string GEO { get; set; }

        [StringLength(20)]
        public string PLAN6 { get; set; }

        [StringLength(1)]
        public string S { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MONTANT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MONTDEV { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MTREPORT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NLET { get; set; }

        [StringLength(1)]
        public string FACT { get; set; }

        [StringLength(1)]
        public string DRAPEAU { get; set; }

        [StringLength(3)]
        public string DEVISE { get; set; }

        [StringLength(2)]
        public string COMPTA { get; set; }

        [StringLength(50)]
        public string MARCHE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NBORD { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? RELEVE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ANCIENDRF { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NOUVDRF { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MONTEMIS { get; set; }

        public DateTime? ECHEANCE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NPRELET { get; set; }

        [StringLength(255)]
        public string LIBELLEM { get; set; }

        public bool BLNLOCAL { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? QTE { get; set; }

        [StringLength(15)]
        public string CODEDP { get; set; }

        public bool REGIE { get; set; }

        [StringLength(1)]
        public string MODEREG { get; set; }

        [StringLength(20)]
        public string SOURCE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PU { get; set; }

        [StringLength(10)]
        public string UNITE { get; set; }

        public DateTime? DATECRE { get; set; }

        public DateTime? DATEMAJ { get; set; }

        [StringLength(10)]
        public string USERCRE { get; set; }

        [StringLength(10)]
        public string USERMAJ { get; set; }

        [StringLength(50)]
        public string BL { get; set; }

        [StringLength(1)]
        public string NATSOURCE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? COURSREP { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? COURSDEV { get; set; }

        [StringLength(50)]
        public string SIGLE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NUMENGAG { get; set; }

        [StringLength(30)]
        public string NOORDO { get; set; }

        public DateTime? DATORDO { get; set; }

        [StringLength(1)]
        public string FACTCAA { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NOUVDRFCAA { get; set; }

        [StringLength(25)]
        public string NUMLIGNEBUD { get; set; }

        [StringLength(20)]
        public string PLAN7 { get; set; }

        [StringLength(50)]
        public string IMPORTID { get; set; }

        [StringLength(60)]
        public string CHAMPSADD1 { get; set; }

        [StringLength(20)]
        public string EVIREMENT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? IMPORTIDH { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NUMENREGSITE { get; set; }

        [StringLength(20)]
        public string PLAN8 { get; set; }

        [StringLength(50)]
        public string REFRELEVE { get; set; }

        [StringLength(20)]
        public string BROUUSERCRE { get; set; }

        [StringLength(20)]
        public string BROUUSERVALID { get; set; }

        [StringLength(20)]
        public string NUMDECLARATIONFISCAL { get; set; }

        [StringLength(20)]
        public string NUMDECLARATIONTMP { get; set; }

        public virtual RJL1 RJL1 { get; set; }
    }
}
