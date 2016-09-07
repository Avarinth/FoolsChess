using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schachuebung2
{
    class tester1
    {
        public void boardtest1()
        {
            Console.SetWindowPosition(0, 0);
            Console.SetWindowSize(100, 60);
            
            Schachbrett schachbrett = new Schachbrett();

            int[] zugtyp = new int[6];
            zugtyp[0] = 1;
            zugtyp[1] = 2;
            zugtyp[2] = 3;
            zugtyp[3] = 4;
            zugtyp[4] = 5;
            zugtyp[5] = 6; 

            schachbrett.setSide(4, 1, 0);
            schachbrett.setType(4, 1, 0);

            schachbrett.setSide(1, 1, 0);
            schachbrett.setType(1, 1, 4);

            schachbrett.setSide(5, 2, 1);
            schachbrett.setType(5, 2, 1);

            schachbrett.setSide(4, 3, 1);
            schachbrett.setType(4, 3, 1);

            schachbrett.setSide(6, 7, 1);
            schachbrett.setType(6, 7, 4);

            schachbrett.setSide(2, 6, 1);
            schachbrett.setType(2, 6, 6);
            schachbrett.setEifer(2, 6, 7);
            schachbrett.setWachsam(2, 6, true);

            schachbrett.setSide(6, 6, 1);
            schachbrett.setType(6, 6, 5);
            schachbrett.setPrimaerZiel(6, 6, true);

            Console.Clear();

            /*
             * Alte Testausgabe
            string[] ASCIIbild = schachbrett.getASCIISchachbrett();

            for (int i = (schachbrett.getSizeY() - 1); i >= 0; i--)
            {
                Console.Write(ASCIIbild[i]);
            }

            Console.WriteLine();

            
            int[] aktuellerZug = new int[1];
            for (int j = 0; j < zugtyp.Length; j++)
            {
                aktuellerZug[0] = zugtyp[j];
                string[] ASCIIzuege = schachbrett.getASCIIzuege(4, 1, aktuellerZug, 0);

                for (int i = (schachbrett.getSizeY() - 1); i >= 0; i--)
                {
                    Console.Write(ASCIIzuege[i]);
                }

                Console.WriteLine();

            }

             */
 
            string[] FullASCIIbild = schachbrett.getFullASCIISchachbrett();

            for (int i = (FullASCIIbild.Length - 1); i >= 0; i--)
            {
                Console.Write(FullASCIIbild[i]);
            }
            Console.WriteLine("\n\n\n");


            int[] aktuellerZug = new int[1];
            for (int j = 0; j < zugtyp.Length; j++)
            {
                aktuellerZug[0] = zugtyp[j];
                string[] ASCIIzuege = schachbrett.getFullASCIISchachbrett(4, 1, aktuellerZug);

                for (int i = (ASCIIzuege.Length - 1); i >= 0; i--)
                {
                    Console.Write(ASCIIzuege[i]);
                }

                Console.WriteLine("\n\n\n");

            }

            FullASCIIbild = schachbrett.getFullASCIISchachbrett(4, 1, zugtyp);

            for (int i = FullASCIIbild.Length - 1; i >= 0; i--)
            {
                Console.Write(FullASCIIbild[i]);
            }
            Console.WriteLine("\n\n\n");

            Console.ReadKey();
        }
    }
}
