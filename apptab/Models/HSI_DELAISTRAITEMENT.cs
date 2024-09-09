namespace apptab
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class HSI_DELAISTRAITEMENT
    {
        public int ID { get; set; }

        public int? IDPROJET { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DELETIONDATE { get; set; }

        public int? DELRAF { get; set; }

        public int? DELTV { get; set; }

        public int? DELENVOISIIGFP { get; set; }

        public int? DELSIIGFP { get; set; }

        public int? DELARAF { get; set; }

        public int? DELAV { get; set; }

        public int? DELAENVOISIIGFP { get; set; }

        public int? DELASIIGFP { get; set; }

        public int? DELPE { get; set; }

        public int? DELPV { get; set; }

        public int? DELPP { get; set; }

        public int? DELPB { get; set; }

        public int? IDUSER { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CREATIONDATE { get; set; }

        public int? IDPARENT { get; set; }
    }
}
