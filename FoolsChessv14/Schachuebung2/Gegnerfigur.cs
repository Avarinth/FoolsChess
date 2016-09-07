using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrenschach01
{
    class Gegnerfigur
    {
        public int posX;
        public int posY;
        public int type;
        public int eifer;
        public bool wachsam;
        public bool primaerZiel;

        public Gegnerfigur(int posX, int posY, int type, int eifer, bool wachsam, bool primaerZiel)
        {
            this.posX = posX;
            this.posY = posY;
            this.type = type;
            this.eifer = eifer;
            this.wachsam = wachsam;
            this.primaerZiel = primaerZiel;
        }

        
    }
}
