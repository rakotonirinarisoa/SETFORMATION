namespace apptab
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class HOPA_CRYPTO
    {
        public int ID { get; set; }

        public string CRYPTOPWD { get; set; }

        public int? IDPROJET { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DELETIONDATE { get; set; }

        public int? IDUSER { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CREATIONDATE { get; set; }

        public int? IDPARENT { get; set; }
    }
}
