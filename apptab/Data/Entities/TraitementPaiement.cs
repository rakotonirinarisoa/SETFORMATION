using System.Collections.Generic;

namespace apptab.Data.Entities
{
    public class TraitementPaiement
    {
        public string SOA { get; set; }
        public List<TraitementPaiementDetails> TraitementPaiementDetails { get; set; }
    }
}
