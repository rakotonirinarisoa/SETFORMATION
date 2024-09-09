namespace apptab.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MOP")]
    public partial class MOP
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(2)]
        public string SITE { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string NUMEROOP { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NUMENREG { get; set; }

        [StringLength(200)]
        public string LIBELLE { get; set; }

        public DateTime? DATEFACTURE { get; set; }

        [StringLength(50)]
        public string NUMEROFACTURE { get; set; }

        [StringLength(20)]
        public string COGE { get; set; }

        [StringLength(20)]
        public string AUXI { get; set; }

        [StringLength(10)]
        public string CONVENTION { get; set; }

        [StringLength(3)]
        public string CATEGORIE { get; set; }

        [StringLength(3)]
        public string SOUSCATEGORIE { get; set; }

        [StringLength(20)]
        public string POSTE { get; set; }

        [StringLength(20)]
        public string ACTI { get; set; }

        [StringLength(20)]
        public string GEO { get; set; }

        [StringLength(20)]
        public string PLAN6 { get; set; }

        [StringLength(20)]
        public string PLAN7 { get; set; }

        [StringLength(20)]
        public string PLAN8 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MONTANTLOC { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MONTANTDEV { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MONTANTRAP { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MONTANTTVA { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MONTANTAUTRETAXE { get; set; }

        [StringLength(20)]
        public string COGEFOURNISSEUR { get; set; }

        [StringLength(20)]
        public string AUXIFOURNISSEUR { get; set; }

        [StringLength(20)]
        public string NORDPEC { get; set; }

        [StringLength(20)]
        public string NORDPAIEMENT { get; set; }

        [StringLength(20)]
        public string NORDFRAISBQ { get; set; }

        public DateTime? DATECRE { get; set; }

        public DateTime? DATEMAJ { get; set; }

        [StringLength(10)]
        public string USERCRE { get; set; }

        [StringLength(10)]
        public string USERMAJ { get; set; }

        public DateTime? DATEREJET { get; set; }

        [StringLength(255)]
        public string OBSERVATIONREJET { get; set; }

        public decimal? MONTANTRETENUE { get; set; }

        [StringLength(20)]
        public string TYPETAXE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MONTANTTVADEV { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MONTANTTVARAP { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MONTANTAUTRETAXEDEV { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MONTANTAUTRETAXERAP { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MONTANTRETENUEDEV { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MONTANTRETENUERAP { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MONTANTCOMMISSION { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TAXECOMMISSION { get; set; }

        public bool? PAYE { get; set; }
    }
}
