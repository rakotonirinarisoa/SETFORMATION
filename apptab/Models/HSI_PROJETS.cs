namespace apptab
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class HSI_PROJETS
    {
        public int ID { get; set; }

        public int IDPARENT { get; set; }

        public string PROJET { get; set; }
        public int IDUSER { get; set; }

        [Column(TypeName = "DATETIME")]
        public DateTime CREATIONDATE { get; set; }
    }
}
