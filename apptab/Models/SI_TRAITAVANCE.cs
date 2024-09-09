namespace apptab
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class SI_TRAITAVANCE
    {
        public int ID { get; set; }

        public Guid? No { get; set; }

        public string REF { get; set; }

        public string OBJ { get; set; }

        public string TITUL { get; set; }

        public string MONT { get; set; }

        public string COMPTE { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DATEMANDAT { get; set; }

        public int? ETAT { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DATECRE { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DATEVALIDATION { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DATENVOISIIGFP { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DATESIIG { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DATEANNUL { get; set; }

        public string PCOP { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DATEDEF { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DATETEF { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DATEBE { get; set; }

        public int? IDPROJET { get; set; }

        public int? IDUSERCREATE { get; set; }

        public int? IDUSERVALIDATE { get; set; }

        public int? IDUSERENVOISIIGFP { get; set; }

        [StringLength(50)]
        public string USERSIIG { get; set; }

        public int? IDUSERANNUL { get; set; }

        [StringLength(50)]
        public string NDEFSIIG { get; set; }

        [StringLength(50)]
        public string NTEFSIIG { get; set; }

        [StringLength(50)]
        public string NBESIIG { get; set; }
    }
}
