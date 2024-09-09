namespace apptab
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class SI_TYPECRITURE
    {
        public int ID { get; set; }

        public int? TYPE { get; set; }

        public int? IDPROJET { get; set; }

        public int? IDUSER { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DELETIONDATE { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CREATIONDATE { get; set; }
    }
}
