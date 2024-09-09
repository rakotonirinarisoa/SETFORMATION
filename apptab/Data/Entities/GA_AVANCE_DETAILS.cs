// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace apptab
{
    public class GA_AVANCE_DETAILS
    {
      
        public string SITE { get; set; }
        public string NUMERO { get; set; }
        public string LIBELLE { get; set; }

        public DateTime? DATE { get; set; }

        public DateTime? ECHEANCE { get; set; }
        public string NUMERO_PIECE { get; set; }

        public string COGE { get; set; }
        public string AUXI { get; set; }
        public string TYPE { get; set; }
        public string SOUS_TYPE { get; set; }

        public DateTime DEBUT_ECHEANCE { get; set; }

        public string MARCHE { get; set; }
       
        public string JOURNAL { get; set; }
        public string EXCERCICE { get; set; }
        public decimal MONTANT { get; set; }
        public string PLAN6 { get; set; }
        public string POSTE { get; set; }
        public string CONVENTION { get; set; }
        public string CATEGORIE { get; set; }
        public string ACTI { get; set; }
        public string GEO { get; set; }
    }
}
