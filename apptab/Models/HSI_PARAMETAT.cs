
namespace apptab
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class HSI_PARAMETAT
    {
        public int ID { get; set; }

        public int? DEF { get; set; }

        public int? TEF { get; set; }

        public int? BE { get; set; }

        public int? DEFA { get; set; }

        public int? TEFA { get; set; }

        public int? BEA { get; set; }

        public int? IDPROJET { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DELETIONDATE { get; set; }

        public int? IDUSER { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CREATIONDATE { get; set; }

        public int? IDPARENT { get; set; }
    }
}
