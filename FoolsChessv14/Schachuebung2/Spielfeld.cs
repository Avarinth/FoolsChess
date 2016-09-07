using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schachuebung2
{
    class Spielfeld // Repraesentiert ein einzelnes Spielfeld, das von genau einer Figur besetzt werden kann
    {
        private int occupierSide; // -1 wenn Feld unbesetzt, 0 wenn Spielerfigur, 1 wenn Gegnerfigur
        private int occupierType;

        /* 
         * -1 fuer unbesetzt 
         * 0 fuer Narr 
         * 1 fuer Bauer
         * 2 fuer Turm
         * 3 fuer Laeufer
         * 4 fuer Springer
         * 5 fuer Dame
         * 6 fuer Koenig
         */

        private bool primaerZiel; // true wenn Gegner und Primaerziel

        private int eifer; // 0 bei Spieler; 1-9 Wahrscheinlichkeit zu ziehen in % / 10 bei Gegnern

        private bool wachsam; // Erste ziehen alle Gegner mit true, dann jene mit false (in zufaelliger Reihenfolge)

        // Konstruktor setzt Spielfeld auf "unbesetzt"
        public Spielfeld()
        {
            this.occupierSide = -1;
            this.occupierType = -1;
            this.primaerZiel = false;
            this.eifer = 0;
            this.wachsam = false;
        }

        public void setSide(int side)
        {
            this.occupierSide = side;
        }

        public int getSide()
        {
            return this.occupierSide;
        }

        public void setType(int type)
        {
            this.occupierType = type;
        }

        public int getType()
        {
            return this.occupierType;
        }
        public void setPrimaerZiel(bool z)
        {
            this.primaerZiel = z;
        }

        public bool getPrimaerZiel()
        {
            return this.primaerZiel;
        }

        public void setEifer(int e)
        {
            this.eifer = e;
        }

        public int getEifer()
        {
            return this.eifer;
        }

        public void setWachsam(bool w)
        {
            this.wachsam = w;
        }

        public bool getWachsam()
        {
            return this.wachsam;
        }
    }
}
