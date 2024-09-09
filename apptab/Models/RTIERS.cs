namespace apptab
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class RTIERS
    {
        [Key]
        [StringLength(50)]
        public string COGEAUXI { get; set; }

        [StringLength(20)]
        public string COGE { get; set; }

        [StringLength(20)]
        public string AUXI { get; set; }

        [StringLength(100)]
        public string NOM { get; set; }

        [StringLength(100)]
        public string CONTACT { get; set; }

        [StringLength(100)]
        public string AD1 { get; set; }

        [StringLength(100)]
        public string AD2 { get; set; }

        [StringLength(100)]
        public string AD3 { get; set; }

        [StringLength(3)]
        public string PAYS { get; set; }

        [StringLength(50)]
        public string TEL { get; set; }

        [StringLength(50)]
        public string FAX { get; set; }

        [StringLength(50)]
        public string EMAIL { get; set; }

        [StringLength(50)]
        public string RIB1 { get; set; }

        [StringLength(50)]
        public string RIB2 { get; set; }

        [StringLength(50)]
        public string DOM1 { get; set; }

        [StringLength(50)]
        public string DOM2 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NBJOURS { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? JOURREF { get; set; }

        [StringLength(10)]
        public string COGECONSOLIDE { get; set; }

        [StringLength(12)]
        public string AUXICONSOLIDE { get; set; }

        public DateTime? DATECRE { get; set; }

        public DateTime? DATEMAJ { get; set; }

        [StringLength(10)]
        public string USERCRE { get; set; }

        [StringLength(10)]
        public string USERMAJ { get; set; }

        [StringLength(50)]
        public string PLANCONSOLIDE { get; set; }

        public bool? STATUT { get; set; }

        [StringLength(100)]
        public string RC { get; set; }

        [StringLength(100)]
        public string NIF { get; set; }

        [StringLength(100)]
        public string STAT { get; set; }

        [StringLength(50)]
        public string BQNOM { get; set; }

        [StringLength(50)]
        public string BQVILLE { get; set; }

        [StringLength(50)]
        public string BQPAYS { get; set; }

        [StringLength(50)]
        public string BQSWIFT { get; set; }

        [StringLength(50)]
        public string BQIBAN { get; set; }

        [StringLength(50)]
        public string RIB1CORRES { get; set; }

        [StringLength(50)]
        public string RIB2CORRES { get; set; }

        [StringLength(50)]
        public string DOM1CORRES { get; set; }

        [StringLength(50)]
        public string DOM2CORRES { get; set; }

        [StringLength(50)]
        public string BQNOMCORRES { get; set; }

        [StringLength(50)]
        public string BQVILLECORRES { get; set; }

        [StringLength(50)]
        public string BQPAYSCORRES { get; set; }

        [StringLength(50)]
        public string BQSWIFTCORRES { get; set; }

        [StringLength(50)]
        public string BQIBANCORRES { get; set; }

        [StringLength(10)]
        public string RIBGUICHET { get; set; }

        [StringLength(10)]
        public string RIBCLE { get; set; }

        [StringLength(10)]
        public string RIBGUICHETCORRES { get; set; }

        [StringLength(10)]
        public string RIBCLECORRES { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? IMPORTID { get; set; }

        [StringLength(20)]
        public string TYPETAXE { get; set; }

        [StringLength(50)]
        public string NUMAGREMENT { get; set; }

        public DateTime? DATEVALIDITEAGREMENT { get; set; }

        [StringLength(200)]
        public string SPECIALITES { get; set; }

        public int? ID_CONTACT { get; set; }

        [StringLength(250)]
        public string BQINTITULE { get; set; }

        [StringLength(50)]
        public string NUMBAD { get; set; }
    }
}
