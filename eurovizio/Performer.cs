﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eurovizio
{
    public class Performer
    {
        int ev;
        string eloado;
        string cim;
        int helyezes;
        int pontszam;

        public int Ev { get => ev; set => ev = value; }
        public string Eloado { get => eloado; set => eloado = value; }
        public string Cim { get => cim; set => cim = value; }
        public int Helyezes { get => helyezes; set => helyezes = value; }
        public int Pontszam { get => pontszam; set => pontszam = value; }

        public Performer(int ev, string eloado, string cim, int helyezes, int pontszam)
        {
            this.ev = ev;
            this.eloado = eloado;
            this.cim = cim;
            this.helyezes = helyezes;
            this.pontszam = pontszam;
        }
    }
}
