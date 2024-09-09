namespace apptab
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class SI_PROSOA
    {
        public int ID { get; set; }

        public int? IDPROJET { get; set; }

        public int? IDSOA { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DELETIONDATE { get; set; }

        public int? IDUSER { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CREATIONDATE { get; set; }

        public int? IDUSERDEL { get; set; }
    }
}
