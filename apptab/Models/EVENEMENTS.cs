// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace apptab.Models
{
    public class EVENEMENTS
    {
        public string SOA { get; set; }
        public string PROJET { get; set; }
        public int? TYPE { get; set; }
        public int? ETAT { get; set; }
        public string USER { get; set; }
        public int? COUNT { get; set; }
        public DateTime? DATE { get; set; }
    }
}
