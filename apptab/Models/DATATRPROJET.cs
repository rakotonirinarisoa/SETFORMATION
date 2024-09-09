// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace apptab
{
    public class DATATRPROJET
    {
        public Guid? No { get; set; }
        public string REF { get; set; }
        public string OBJ { get; set; }
        public string TITUL { get; set; }
        public string MONT { get; set; }
        public string COMPTE { get; set; }
        public string PCOP { get; set; }
        public DateTime? DATE { get; set; }
        public string STAT { get; set; }
        public DateTime? DATEDEF { get; set; }
        public DateTime? DATETEF { get; set; }
        public DateTime? DATEBE { get; set; }
        public string LIEN { get; set; }
        public DateTime? DATECREATION { get; set; }

        public string SOA { get; set; }
        public string PROJET { get; set; }
        public bool? isLATE { get; set; }
        public bool? isAvance { get; set; }
    }
}
