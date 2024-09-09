namespace apptab
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class CPTADMIN_MODELEETATS
    {
        [Key]
        [StringLength(20)]
        public string NUMERO { get; set; }

        [StringLength(70)]
        public string LIBELLE { get; set; }

        [StringLength(70)]
        public string MODELE { get; set; }

        [StringLength(10)]
        public string EXTENSION { get; set; }

        public DateTime? DATECRE { get; set; }

        public DateTime? DATEMAJ { get; set; }

        [StringLength(10)]
        public string USERCRE { get; set; }

        [StringLength(10)]
        public string USERMAJ { get; set; }
    }
}
