using System;

namespace apptab.Data.Entities
{
    public class TraitementEngagementDetails
    {
        public string PROJET { get; set; }
        public string TYPE { get; set; }
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
        public double DUREETRAITEMENTTRANSFERTRAF { get; set; }
        public double DUREETRAITEMENTVALORDSEC { get; set; }
        public double DUREETRAITEMENTSENDSIIG { get; set; }
        public double DUREETRAITEMENTSIIGFP { get; set; }

        public double DURPREVUTRANSFERT { get; set; }
        public double DURPREVUVALIDATION { get; set; }
        public double DURPREVUTRANSFSIIG { get; set; }
        public double DURPREVUSIIG { get; set; }

        public double DEPASTRANSFERT { get; set; }
        public double DEPASVALIDATION { get; set; }
        public double DEPASTRANSFSIIG { get; set; }
        public double DEPASSIIG { get; set; }
    }
}
