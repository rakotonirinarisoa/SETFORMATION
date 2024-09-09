using System;

namespace apptab.Data.Entities
{
    public class TraitementPaiementDetails
    {
        //public string SOA { get; set; }
        public string NUM_ENGAGEMENT { get; set; }
        public string BENEFICIAIRE { get; set; }
        public string MONTENGAGEMENT { get; set; }
        public DateTime? DATENGAGEMENT { get; set; }
        public string MONTPAIE { get; set; }
        public DateTime? DATEPAIE { get; set; }
        public DateTime? DATETRANSFERTRAF { get; set; }
        public string TRANSFERTRAFAGENT { get; set; }
        public DateTime? DATEVALORDSEC { get; set; }
        public string VALORDSECAGENT { get; set; }

        public DateTime? DATESENDSIIG { get; set; }
        public string SENDSIIGAGENT { get; set; }
        public DateTime? DATESIIGFP { get; set; }
        public string SIIGFPAGENT { get; set; }

        public double DUREETRAITEMENTTRANSFERTOP { get; set; }
        public double DUREETRAITEMENTTRANSFERTAC { get; set; }
        public double DUREETRAITEMENTTRANSFERTBK { get; set; }

        public double DUREETRAITEMENTTRANSFERTRAF { get; set; }
        public double DUREETRAITEMENTVALORDSEC { get; set; }
        public double DUREETRAITEMENTSENDSIIG { get; set; }
        public double DUREETRAITEMENTSIIGFP { get; set; }

        public double DUREETRAITEMENTPREVUEOP { get; set; }
        public double DUREETRAITEMENTPREVUEAC { get; set; }
        public double DUREETRAITEMENTPREVUEBK { get; set; }

        public double DEPASSEMENTOP { get; set; }
        public double DEPASSEMENTAC { get; set; }
        public double DEPASSEMENTBK { get; set; }
    }
}
