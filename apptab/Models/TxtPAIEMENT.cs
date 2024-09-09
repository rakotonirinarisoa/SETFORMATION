// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace apptab
{
    public class TxtPAIEMENT
    {
        public string No { get; set; }
        public string BENEF { get; set; }
        public string MONTANT { get; set; }
        public DateTime? DATEVALIDATIONOP { get; set; }
        public DateTime? DATEVALIDATIONAC { get; set; }
        public DateTime? DATEPAIEBANQUE { get; set; }
        public DateTime? DATEREJETAC { get; set; }
        public string SOA { get; set; }
        public string PROJET { get; set; }
        public bool? isLATE { get; set; }


        public DateTime? DATETRAITEMENTBANQUE { get; set; }
    }
}
