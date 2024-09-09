namespace apptab
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class SI_PROCEDURE
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string PROCEDURE { get; set; }

        [StringLength(50)]
        public string PROCEDUREDEG { get; set; }

        public int? IDPROJET { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DELETIONDATE { get; set; }

        [StringLength(50)]
        public string CODE { get; set; }

        [StringLength(50)]
        public string CODEDEG { get; set; }
        public int? IDUSER { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CREATIONDATE { get; set; }
    }
}
