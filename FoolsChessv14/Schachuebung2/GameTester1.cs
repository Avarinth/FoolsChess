using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Schachuebung2;

namespace Narrenschach01
{
    class GameTester1
    {
        // Random initialisieren
        Random rnd = new Random();
        int NarrX;
        int NarrY;

        int wuerfel; // Anzahl der Wuerfel
        private int schwierigkeitsgrad;

        private int siegeNarr = 0;
        private int siegeGegner = 0;

        int spielNr = 0;
        int runde;
        private int rundengesamt = 0;

        private int figurenGeschlagen = 0;
        private int gesamtGeschlagen = 0;

        private Schachbrett schachbrett;
        private byte[] bytearray;
        private int averagerating = 0;

        private int hinderWunsch;
        private int gegnerWunsch;

        private bool gridAn = true;

        // Spiel in Konsole spielen
        public void Run()
        {
            // Konsolengroesse anpassen
            Console.SetWindowPosition(0, 0);
            Console.SetWindowSize(90, 60);

            showEinfuehrung();
            Console.ReadKey();

            bool gameOn = true;

            do
            {
                Console.Clear();
                
                Console.Write("(L)oad, (C)reate New or <Enter> Random Dungeon>"); 
                string answer = Console.ReadLine();

                if (answer == "l" || answer == "L")
                {
                    schachbrett = loadSpielstand();
                }
                else if (answer == "c" || answer == "C")
                {
                    schachbrett = manuellEingeben();
                }
                else
                {
                    enterEnemyNr();
                    
                    schachbrett = randomGame();
                }
                
                spielNr++;
                figurenGeschlagen = 0;
                
                runde = 0;

                int winner = -1; // 0: Spieler gewinnt; 1: Computer gewinnt
                bool spielerdran = true; // true: Spieler ist am Zug; false: PC ist am Zug

                do
                {

                    if (spielerdran)
                    {
                        winner = playerTurn(wuerfel);
                        spielerdran = false;
                        runde++;
                        rundengesamt++;
                    }
                    else
                    {
                        winner = gegnerTurn();
                        spielerdran = true;
                    }

                } while (winner == -1);

                if (winner == 0)
                {
                    if (averagerating == 0)
                    {
                        averagerating = schwierigkeitsgrad - runde + figurenGeschlagen;
                    }
                    else
                    {
                        averagerating = (averagerating + schwierigkeitsgrad - runde + figurenGeschlagen) / 2;
                    }
                    
                    siegeNarr ++;
                    Console.Clear();
                    zeigeSchachbrett();
                    zeigeRunde();
                    zeigeStats();
                    Console.WriteLine("\tRating: " + (schwierigkeitsgrad - runde) + "\tAvgRtg " + averagerating) ;
                    Console.Write("\nMuahahahaha! The Fool wins in {0} rounds!\n\ne'X'it or <return> play again >", runde);
                }
                else
                {
                    if (averagerating != 0)
                    {
                        averagerating /= 2;
                    }
                    siegeGegner ++;
                    Console.Clear();
                    zeigeSchachbrett();
                    zeigeStats();
                    Console.WriteLine("\tRating: 0\tAvgRtg " + averagerating);
                    Console.Write("\nThe fool gets a sound beating after {0} rounds\n\ne'X'it or <return> play again >", runde);
                }

                string taste = Console.ReadLine();

                if (taste == "x" || taste == "X")
                    gameOn = false;

            } while (gameOn);

            Console.WriteLine("Good bye!");

        }

        // Spielerzug gibt ggf. winner zurueck
        public int playerTurn(int wuerfel)
        {
            Console.Clear();
            zeigeSchachbrett(); 
            zeigeRunde();

            bool done = false;
            do
            {
                Console.Write("'M'enu or <Enter> to Play >");
                string k = Console.ReadLine();

                if (k == "m" || k == "M")
                {
                    Console.Clear();
                    zeigeSchachbrett();
                    zeigeRunde();
                    
                    Console.Write(
                        " - 'H'elp - 'S'ave - 'L'oad - e'X'it - 'I'nfo -\n" +
                        " - 'C'reate level  - 'E'dit level\n" +
                        " - 'R'andomize     - 'M'arkers on/off\n\n" +
                        " - 'G'ive up or Play <Enter> ! >");

                    k = Console.ReadLine();

                    if (k == "e" || k == "E")
                    {
                        schachbrett = editFeld();
                    }
                    else if (k == "h" || k == "H")
                    {
                        showHelp();
                    }
                    else if (k == "i" || k == "I")
                    {
                        Console.Clear();
                        zeigeSchachbrett();
                        zeigeRunde();
                        Console.WriteLine("Average rating: " + averagerating);
                        zeigeStats();

                        Console.WriteLine("The Fool has " + wuerfel + " dice.");
                        
                    }
                    else if (k == "m" || k == "M")
                    {
                        if (gridAn)
                        {
                            schachbrett.setGrid(false);
                            gridAn = false;
                        }
                        else
                        {
                            schachbrett.setGrid(true);
                            gridAn = true;
                        }

                        Console.Clear();
                        zeigeSchachbrett();
                        spielNr++;
                        runde = 0;
                        zeigeRunde();
                    }
                    else if (k == "r" || k == "R")
                    {
                        enterEnemyNr();

                        schachbrett = randomGame();

                        Console.Clear();
                        zeigeSchachbrett();
                        spielNr++;
                        runde = 0;
                        zeigeRunde();
                        zeigeStats();
                    }
                    else if (k == "x" || k == "X")
                    {
                        Environment.Exit(0);
                    }
                    else if (k == "l" || k == "L")
                    {
                        schachbrett = loadSpielstand();
                        Console.Clear();
                        zeigeSchachbrett();
                        spielNr++;
                        runde = 0;
                        zeigeRunde();
                        zeigeStats();
                    }
                    else if (k == "c" || k == "C")
                    {
                        schachbrett = manuellEingeben();
                    }
                    else if (k == "s" || k == "S")
                    {
                        saveSpielstand();
                    }
                    else if (k == "g" || k == "G")
                    {
                        Console.WriteLine("\nThe Fool sneaks away...\n");
                        Console.ReadKey();
                        return 1;
                    }
                    else
                    {
                        done = true;
                    }
                }
                else
                {
                    done = true;
                }
            } while (!done);

            int[] wurf = wuerfeln(wuerfel);
            int[] zugtyp = getZugtyp(wurf);

            Console.Clear();
            zeigeSchachbrett(NarrX, NarrY, zugtyp);

            //zeigeRunde();

            zeigeWurf(wurf);

            int[] zielfeld = eingabeKoordinaten(NarrX, NarrY, zugtyp);

            int gegnerzahl = schachbrett.getGegnerzahl();

            zugAusfuehren(zielfeld);

            if (schachbrett.getGegnerzahl() < gegnerzahl)
            {
                figurenGeschlagen ++;
                gesamtGeschlagen ++;
            }

            if (schachbrett.pruefeSpielerSieg())
            {
                return 0;
            }

            return -1;
        }

        // Gegnerzug gibt ggf. winner zurueck
        public int gegnerTurn()
        {
            Gegnerfigur[] gegnerarray = schachbrett.getGegnerArray();

            gegnerarray = gegnerSequenzOrdnen(gegnerarray);

            for (int i = 0; i < gegnerarray.Length; i++)
            {
                // Ermitteln, ob die Figur zieht
                int zufall = rnd.Next(1, 9);

                if (gegnerarray[i].eifer > zufall)
                {
                    // Besten Zug ermitteln und ziehen
                    schachbrett.zieheGegnerFigur(gegnerarray[i].posX, gegnerarray[i].posY, NarrX, NarrY, gegnerarray[i].type);
                }
            }
            
            // Gegnersieg pruefen
            if (schachbrett.pruefeGegnerSieg())
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        // Gegnerarray neu ordnen: Aufmerksam an den Anfang
        public Gegnerfigur[] gegnerSequenzOrdnen(Gegnerfigur[] gegner)
        {
            Gegnerfigur[] gegnerarray = new Gegnerfigur[gegner.Length];
            
            int zaehler = 0;

            foreach (Gegnerfigur figur in gegner)
            {
                if (figur.wachsam)
                {
                    gegnerarray[zaehler] = figur;
                    zaehler++;
                    
                }
            }
            foreach (Gegnerfigur figur in gegner)
            {
                if (!figur.wachsam)
                {
                    gegnerarray[zaehler] = figur;
                    zaehler++;
                    
                }
            }
            //Console.WriteLine("Erster Angreifer: " + gegnerarray[0].type);
            //Console.ReadKey();
            return gegnerarray;
        }

        // Wuerfeln mit n Wuerfeln
        public int[] wuerfeln(int n)
        {
            int[] ergebnisse = new int[n];
            for (int i = 0; i < n; i++)
            {
                ergebnisse[i] = rnd.Next(0, 7);
                Console.WriteLine(ergebnisse[i]);
            }
            return ergebnisse;
        }

        // Schachbrett auf der Konsole anzeigen
        public void zeigeSchachbrett()
        {
            string[] aktuellerSpielstand = schachbrett.getFullASCIISchachbrett();

            for (int i = (aktuellerSpielstand.Length - 1); i >= 0; i--)
            {
                Console.Write(aktuellerSpielstand[i]);
            }
            Console.WriteLine("\n");
        }

        // Schachbrett mit moeglichen Zuegen auf der Konsole anzeigen
        public void zeigeSchachbrett(int x, int y, int[] zugtyp)
        {
            string[] aktuellerSpielstand = schachbrett.getFullASCIISchachbrett(x, y, zugtyp);

            for (int i = (aktuellerSpielstand.Length - 1); i >= 0; i--)
            {
                Console.Write(aktuellerSpielstand[i]);
            }
            Console.WriteLine("\n");
        }

        // Zufallsbrett erstellen
        public Schachbrett randomGame()
        {
            // Stellung initialisieren
            schachbrett = new Schachbrett(rnd.Next(7, 12), rnd.Next(7, 12));

            // Narr zufaellig setzen
            NarrX = rnd.Next(0,schachbrett.getSizeX());
            NarrY = rnd.Next(0,2);

            schachbrett.setSide(NarrX, NarrY, 0);
            schachbrett.setType(NarrX, NarrY, 0);

            // Gegner zufaellig ermitteln und verteilen
			int gegnerzahl = gegnerWunsch;
            if (gegnerzahl < 1)
                gegnerzahl = 1;
            if (gegnerzahl > schachbrett.getSizeX()*schachbrett.getSizeY()/2 - 1)
                gegnerzahl = schachbrett.getSizeX()*schachbrett.getSizeY()/2 - 1;

            int gesamtEifer = 0;
            int gesamtPrimaer = 1;
            int gesamtWachsam = 0;
			
			for (int i = 0; i < gegnerzahl; i++)
			{
				bool positionOk = false;
			    int posx;
			    int posy;

				do
				{
					posx = rnd.Next(0, schachbrett.getSizeX());
                    posy = rnd.Next(schachbrett.getSizeY() / 2, schachbrett.getSizeY());
					
					if (schachbrett.getSide(posx, posy) == -1)
						positionOk = true;

				} while(!positionOk);

                if (i == 0)
                {
                    schachbrett.setPrimaerZiel(posx, posy, true);
                }

				schachbrett.setSide(posx, posy, 1);
                schachbrett.setType(posx, posy, rnd.Next(1, 6));
                schachbrett.setEifer(posx, posy, rnd.Next(1, 10));
			    gesamtEifer += schachbrett.getEifer(posx, posy);

                if (i == 0)
                {
                    schachbrett.setType(posx, posy, 6);
                }

				int helper = rnd.Next(0, 5);
				if (helper == 2)
				{
					schachbrett.setWachsam(posx, posy, true);
				    gesamtWachsam++;
				}

                if (i != 0)
                {
                    helper = rnd.Next(0, 6);
                    if (helper == 3)
                    {
                        schachbrett.setPrimaerZiel(posx, posy, true);
                        gesamtPrimaer ++;
                    }
                }
			}

            // Hindernisse setzen
            int obstaclesZahl = hinderWunsch;
            if (obstaclesZahl < 0)
                obstaclesZahl = 0;
            if (obstaclesZahl > (schachbrett.getSizeX() * schachbrett.getSizeY() / 2 - 1) - gegnerzahl)
                obstaclesZahl = ((schachbrett.getSizeX() * schachbrett.getSizeY() / 2 - 1) - gegnerzahl - 1);

            for (int i = 0; i < obstaclesZahl; i++)
            {
                bool positionOk = false;
                int posx;
                int posy;

                do
                {
                    posx = rnd.Next(0, schachbrett.getSizeX());
                    posy = rnd.Next(0, schachbrett.getSizeY());

                    if (schachbrett.getSide(posx, posy) == -1)
                        positionOk = true;

                } while (!positionOk);

                schachbrett.setSide(posx, posy, 0);
                schachbrett.setType(posx, posy, 7);
                schachbrett.setEifer(posx, posy, 0);
                schachbrett.setWachsam(posx, posy, false);
                schachbrett.setPrimaerZiel(posx, posy, false);
            }

            schwierigkeitsgrad = (gesamtEifer/gegnerzahl) * 2 + gesamtPrimaer * 3 + gesamtWachsam * 2 + gegnerzahl;

            if (schwierigkeitsgrad >= 45)
            {
                wuerfel = 3;
            }
            else if (schwierigkeitsgrad >= 25)
            {
                wuerfel = 2;
            }
            else
            {
                wuerfel = 1;
            }

            return schachbrett;
        }

        // Schachbrett manuell step by step eingeben
        public Schachbrett manuellEingeben()
        {
            int gegnerzahl;

            // Stellung initialisieren
            Console.Clear();
            Console.Write("Enter board width in fields (3 - 11) >");
            string eingabe = Console.ReadLine();
            if (eingabe == "")
                eingabe = "0";
            int breite = Convert.ToInt32(eingabe);
            if (breite < 3)
                breite = 3;
            if (breite > 11)
                breite = 11;
            Console.Write("Enter board length in fields (3 - 11) >");
            eingabe = Console.ReadLine();
            if (eingabe == "")
                eingabe = "0";
            int hoehe = Convert.ToInt32(eingabe);
            if (hoehe < 3)
                hoehe = 3;
            if (hoehe > 11)
                hoehe = 11;

            schachbrett = new Schachbrett(breite, hoehe);

            Console.Clear();
            zeigeSchachbrett();

            // Narr= setzen
            Console.Write("Enter Fool X-Position (1 - {0}) >", breite);
            eingabe = Console.ReadLine();
            if (eingabe == "")
                eingabe = "0";
            int nx = Convert.ToInt32(eingabe);
            if (nx < 1)
                nx = 1;
            if (nx > breite)
                nx = breite;
            nx--;
            Console.Write("Enter Fool Y-Position (1 - {0}) >", hoehe);
            eingabe = Console.ReadLine();
            if (eingabe == "")
                eingabe = "0";
            int ny = Convert.ToInt32(eingabe);
            if (ny < 1)
                ny = 1;
            if (ny > hoehe)
                ny = hoehe;
            ny--;

            NarrX = nx;
            NarrY = ny;

            schachbrett.setSide(NarrX, NarrY, 0);
            schachbrett.setType(NarrX, NarrY, 0);

            Console.Clear();
            zeigeSchachbrett();

            // Gegner eingeben und verteilen
            Console.Write("Number of enemies (1 - {0}) >", hoehe * breite / 3);
            eingabe = Console.ReadLine();
            if (eingabe == "")
                eingabe = "0";
            gegnerzahl = Convert.ToInt32(eingabe);
            if (gegnerzahl < 1)
                gegnerzahl = 1;
            if (gegnerzahl > hoehe * breite / 3)
                gegnerzahl = hoehe * breite / 3;

            int gesamtEifer = 0;
            int gesamtPrimaer = 1;
            int gesamtWachsam = 0;

            for (int i = 0; i < gegnerzahl; i++)
            {
                bool positionOk = false;
                int posx;
                int posy;

                do
                {
                    Console.Write("Enter Enemy {0} posX (1 - {1}) >", i , breite);
                    eingabe = Console.ReadLine();
                    if (eingabe == "")
                        eingabe = "0";
                    posx = Convert.ToInt32(eingabe);
                    if (posx < 1)
                        posx = 1;
                    if (posx > breite)
                        posx = breite;
                    posx--;

                    Console.Write("Enter Enemy {0} posY (1 - {1}) >", i, hoehe);
                    eingabe = Console.ReadLine();
                    if (eingabe == "")
                        eingabe = "0";
                    posy = Convert.ToInt32(eingabe);
                    if (posy < 1)
                        posy = 1;
                    if (posy > hoehe)
                        posy = hoehe;
                    posy--;

                    if (schachbrett.getSide(posx, posy) == -1)
                        positionOk = true;

                } while (!positionOk);

                if (i == 0)
                {
                    schachbrett.setPrimaerZiel(posx, posy, true);
                }

                Console.Write("Enter Enemy {0} Type (1 pawn, 2 rook, 3 bishop, 4 knight, 5 queen, 6 king) >", i);
                eingabe = Console.ReadLine();
                if (eingabe == "")
                    eingabe = "0";
                int typ = Convert.ToInt32(eingabe);
                if (typ < 1)
                    typ = 1;
                if (typ > 6)
                    typ = 6;

                Console.Write("Enter Enemy {0} Zeal (Probability to move or attack, 1 - 9) >", i);
                eingabe = Console.ReadLine();
                if (eingabe == "")
                    eingabe = "0";
                int eif = Convert.ToInt32(eingabe);
                if (eif < 1)
                    eif = 1;
                if (eif > 9)
                    eif = 9;

                schachbrett.setSide(posx, posy, 1);
                schachbrett.setType(posx, posy, typ);
                schachbrett.setEifer(posx, posy, eif);
                gesamtEifer += schachbrett.getEifer(posx, posy);

                if (i == 0)
                {
                    schachbrett.setType(posx, posy, 6);
                }

                Console.Write("Enter Enemy {0} Quickness (0 slow, 1 quick) >", i);
                eingabe = Console.ReadLine();
                if (eingabe == "")
                    eingabe = "0";
                int wac = Convert.ToInt32(eingabe);
                if (wac < 0)
                    wac = 0;
                if (wac > 1)
                    wac = 1;
                if (wac == 1)
                {
                    schachbrett.setWachsam(posx, posy, true);
                    gesamtWachsam++;
                }

                if (i != 0)
                {
                    Console.Write("Enter Enemy {0} isPrimaryTarget (0 is not p.t., 1 is p.t.) >", i);
                    eingabe = Console.ReadLine();
                    if (eingabe == "")
                        eingabe = "0";
                    int pri = Convert.ToInt32(eingabe);
                    if (pri < 0)
                        pri = 0;
                    if (pri > 1)
                        pri = 1;
                    if (pri == 1)
                    {
                        schachbrett.setPrimaerZiel(posx, posy, true);
                        gesamtPrimaer++;
                    }
                }
                Console.Clear();
                zeigeSchachbrett();
            }

            // Hindernisse setzen
            Console.Write("Number of obstacles (1 - {0}) >", breite * 3);
            eingabe = Console.ReadLine();
            if (eingabe == "")
                eingabe = "0";
            int obstaclesZahl = Convert.ToInt32(eingabe);
            if (obstaclesZahl < 1)
                obstaclesZahl = 1;
            if (obstaclesZahl > breite * 3)
                obstaclesZahl = breite * 3;

            for (int i = 0; i < obstaclesZahl; i++)
            {
                bool positionOk = false;
                int posx;
                int posy;

                do
                {
                    Console.Write("Obstacle {0} posX (1 - {1}) >", i, breite);
                    eingabe = Console.ReadLine();
                    if (eingabe == "")
                        eingabe = "0";
                    posx = Convert.ToInt32(eingabe);
                    if (posx < 1)
                        posx = 1;
                    if (posx > breite)
                        posx = breite;
                    posx--;

                    Console.Write("Obstacle {0} posY (1 - {1}) >", i, hoehe);
                    eingabe = Console.ReadLine();
                    if (eingabe == "")
                        eingabe = "0";
                    posy = Convert.ToInt32(eingabe);
                    if (posy < 1)
                        posy = 1;
                    if (posy > hoehe)
                        posy = hoehe;
                    posy--;

                    if (schachbrett.getSide(posx, posy) == -1)
                        positionOk = true;

                } while (!positionOk);

                schachbrett.setSide(posx, posy, 0);
                schachbrett.setType(posx, posy, 7);
                schachbrett.setEifer(posx, posy, 0);
                schachbrett.setWachsam(posx, posy, false);
                schachbrett.setPrimaerZiel(posx, posy, false);

                Console.Clear();
                zeigeSchachbrett();
            }

            schwierigkeitsgrad = (gesamtEifer / gegnerzahl) * 2 + gesamtPrimaer * 3 + gesamtWachsam * 2 + gegnerzahl;

            if (schwierigkeitsgrad >= 45)
            {
                wuerfel = 3;
            }
            else if (schwierigkeitsgrad >= 25)
            {
                wuerfel = 2;
            }
            else
            {
                wuerfel = 1;
            };

            return schachbrett;
        }

        // Einzelnes Feld editieren
        public Schachbrett editFeld()
        {
            Console.Clear();
            zeigeSchachbrett();

            Console.Write("Edit field with X-position> ");
            int posx = Convert.ToInt32(Console.ReadLine());
            posx--;

            Console.Write("Edit field with Y-positionn> ");
            int posy = Convert.ToInt32(Console.ReadLine());
            posy--;

            Console.Clear();
            zeigeSchachbrett();

            Console.Write("Field Side (-1 empty, 0 fool/obstacle, 1 enemy)>");
            int seite = Convert.ToInt32(Console.ReadLine());
            if (seite < -1)
                seite = -1;
            if (seite > 1)
                seite = 1;

            Console.Write("Field Type (-1 empty, 0 fool, 1 pawn - 6 king, 7 obstacle)>");
            int typ = Convert.ToInt32(Console.ReadLine());
            if (typ < -1)
                typ = -1;
            if (typ > 7)
                typ = 7;

            Console.Write("Zeal (0 empty/fool, 1 - 9 enemies)>");
            int eif = Convert.ToInt32(Console.ReadLine());
            if (eif < 0)
                eif = 0;
            if (eif > 9)
                eif = 9;

            schachbrett.setSide(posx, posy, seite);
            schachbrett.setType(posx, posy, typ);
            schachbrett.setEifer(posx, posy, eif);

            Console.Clear();
            zeigeSchachbrett();

            Console.Write("Quickness (0 slow, 1 quick)>");
            int wac = Convert.ToInt32(Console.ReadLine());
            if (wac < 0)
                wac = 0;
            if (wac > 1)
                wac = 1;
            if (wac == 1)
            {
                schachbrett.setWachsam(posx, posy, true);
            }
            else
            {
                schachbrett.setWachsam(posx, posy, false);
            }

            Console.Write("Primary target (0 no, 1 yes)>");
            int pri = Convert.ToInt32(Console.ReadLine());
            if (pri < 0)
                pri = 0;
            if (pri > 1)
                pri = 1;
            if (pri == 1)
            {
                schachbrett.setPrimaerZiel(posx, posy, true);
            }
            else
            {
                schachbrett.setPrimaerZiel(posx, posy, false);
            }

            Console.Clear();
            zeigeSchachbrett();

            return schachbrett;

        }

        // Aktuelles Schachbrett speichern
        public void saveSpielstand()
        {
            Console.Write("\nEnter savegame name > ");
            string fname = Console.ReadLine();
            byte[] hindernis = schachbrett.getHindernisBytes();
            byte[] bytearray = new byte[7 + (schachbrett.getGegnerzahl() * 6) + (hindernis.Length)];

            bytearray[0] = (byte)schachbrett.getSizeX();
            bytearray[1] = (byte)schachbrett.getSizeY();
            bytearray[2] = (byte)NarrX;
            bytearray[3] = (byte)NarrY;
            bytearray[4] = (byte)schachbrett.getGegnerzahl();
            bytearray[5] = (byte)schwierigkeitsgrad;

            Gegnerfigur[] gegnerarray = schachbrett.getGegnerArray();

            for (int i = 0; i < schachbrett.getGegnerzahl(); i++)
            {
                bytearray[i * 6 + 6] = (byte)gegnerarray[i].posX;
                bytearray[i * 6 + 7] = (byte)gegnerarray[i].posY;
                bytearray[i * 6 + 8] = (byte)gegnerarray[i].type;
                bytearray[i * 6 + 9] = (byte)gegnerarray[i].eifer;
                if (gegnerarray[i].wachsam)
                {
                    bytearray[i * 6 + 10] = 1;
                }
                else
                {
                    bytearray[i * 6 + 10] = 0;
                }
                if (gegnerarray[i].primaerZiel)
                {
                    bytearray[i * 6 + 11] = 1;
                }
                else
                {
                    bytearray[i * 6 + 11] = 0;
                }

            }

            for (int i = 0; i < hindernis.Length; i++)
            {
                bytearray[schachbrett.getGegnerzahl()*6 + 7 + i] = hindernis[i];
            }

            try
            {
                File.WriteAllBytes(fname, bytearray);
                Console.Clear();
                zeigeSchachbrett();
                Console.WriteLine("'{0}' successfully saved.", fname);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error. Please retry saving '{0}'.", fname);
            }
        }

        // Schachbrett laden
        public Schachbrett loadSpielstand()
        {
            Console.Write("\nPlease enter savegame name > ");
            string fname = Console.ReadLine();

            try
            {
                bytearray = File.ReadAllBytes(fname);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error trying to load '{0}'.", fname);
                Environment.Exit(0);
            }


            schachbrett = new Schachbrett(bytearray[0], bytearray[1]);

            NarrX = (int)bytearray[2];
            NarrY = (int)bytearray[3];

            schachbrett.setSide(NarrX, NarrY, 0);
            schachbrett.setType(NarrX, NarrY, 0);

            int gegnerzahl = bytearray[4];
            schwierigkeitsgrad = bytearray[5];

            for (int i = 0; i < gegnerzahl; i++)
            {
                schachbrett.setSide(bytearray[i * 6 + 6], bytearray[i * 6 + 7], 1);
                schachbrett.setType(bytearray[i * 6 + 6], bytearray[i * 6 + 7], bytearray[i * 6 + 8]);
                schachbrett.setEifer(bytearray[i * 6 + 6], bytearray[i * 6 + 7], bytearray[i * 6 + 9]);
                if (bytearray[i * 6 + 10] == 1)
                {
                    schachbrett.setWachsam(bytearray[i * 6 + 6], bytearray[i * 6 + 7], true);
                }
                else
                {
                    schachbrett.setWachsam(bytearray[i * 6 + 6], bytearray[i * 6 + 7], false);
                }
                if (bytearray[i * 6 + 11] == 1)
                {
                    schachbrett.setPrimaerZiel(bytearray[i * 6 + 6], bytearray[i * 6 + 7], true);
                }
                else
                {
                    schachbrett.setPrimaerZiel(bytearray[i * 6 + 6], bytearray[i * 6 + 7], false);
                }
            }

            // Hindernisse setzen
            int hindernisbytes = bytearray.Length - (gegnerzahl * 6 + 6);

            for (int i = 0; i < hindernisbytes - 1; i += 2)
            {
                int x = bytearray[(gegnerzahl*6 + 7) + i];
                int y = bytearray[(gegnerzahl*6 + 7) + i + 1];

                schachbrett.setSide(x, y, 0);
                schachbrett.setType(x, y, 7);
                schachbrett.setEifer(x, y, 0);
                schachbrett.setWachsam(x, y, false);
                schachbrett.setPrimaerZiel(x, y, false);
            }

            if (schwierigkeitsgrad >= 45)
            {
                wuerfel = 3;
            }
            else if (schwierigkeitsgrad >= 25)
            {
                wuerfel = 2;
            }
            else
            {
                wuerfel = 1;
            }

            return schachbrett;

        }

        // ZugTyp aus Wurf ermitteln
        public int[] getZugtyp(int[] wurf)
        {
            for (int i = 0; i < wurf.Length; i++)
            {
                if (wurf[i] == 0)
                {
                    int[] zugtyp = new int[6] { 1, 2, 3, 4, 5, 6 };
                    return zugtyp;
                }
            }
            return wurf;
        }

        // Wurf in Worten anzeigen
        public void zeigeWurf(int[] wurf)
        {
            zeigeRunde();
            Console.Write("You rolled your dice: ");

            for (int i = 0; i < wurf.Length; i++)
            {
                switch (wurf[i])
                {
                    case 0:
                        Console.Write("Fool (all move options available !)");
                        break;
                    case 1:
                        Console.Write("Pawn");
                        break;
                    case 2:
                        Console.Write("Rook");
                        break;
                    case 3:
                        Console.Write("Bishop");
                        break;
                    case 4:
                        Console.Write("kNight");
                        break;
                    case 5:
                        Console.Write("Queen");
                        break;
                    case 6:
                        Console.Write("King");
                        break;
                    default:
                        break;
                }
                if (i < wurf.Length - 1)
                    Console.Write(", ");
            }
            Console.WriteLine();
        }

        // Zielkoordinaten eingeben
        public int[] eingabeKoordinaten(int startX, int startY, int[] zugtyp)
        {
            bool done = false;
            bool ok;
            int[] koordinaten = new int[2];
            do
            {
                int counter;
                ok = false;
                do
                {
                    counter = 0;
                    Console.Write("\nEnter X > ");
                    try
                    {
                        string koord = Console.ReadLine();
                        if (koord == "")
                        {
                            koordinaten[0] = NarrX;
                            counter++;
                        }
                        else
                        {
                            koordinaten[0] = Convert.ToInt32(koord);
                            koordinaten[0] -= 1;
                        }   
                        
                        if ((koordinaten[0] < schachbrett.getSizeX() && koordinaten[0] >= 0))
                        {
                            ok = true;
                        }
                        else
                        {
                            Console.WriteLine("No comprendo, senhor.");
                        }

                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Pardon me?");
                    }
                } while (!ok);

                ok = false;
                do
                {
                    Console.Write("\nEnter Y > ");
                    try
                    {

                        string koord = Console.ReadLine();
                        if (koord == "")
                        {
                            koordinaten[1] = NarrY;
                            counter++;
                        }
                        else
                        {
                            koordinaten[1] = Convert.ToInt32(koord);
                            koordinaten[1] -= 1;
                        }
                        if ((koordinaten[1] < schachbrett.getSizeY() && koordinaten[1] >= 0) || (koord == ""))
                        {
                            ok = true;
                            counter++;
                        }
                        else
                        {
                            Console.WriteLine("I do not get this.");
                        }

                    }
                    catch (Exception)
                    {
                        Console.WriteLine("WTF?");
                    }

                } while (!ok);

                if ((koordinaten[0] == NarrX && koordinaten[1] == NarrY) || counter == 2)
                {
                    done = true;
                }
                else if (schachbrett.getZuege(NarrX, NarrY, zugtyp, 0)[koordinaten[0], koordinaten[1]] == -1)
                {
                    done = false;
                    Console.WriteLine("You cannot go to field {0} : {1} now.",koordinaten[0] + 1, koordinaten[1] + 1);
                }
                else
                {
                    done = true;
                }

                Console.Clear();
                zeigeSchachbrett(NarrX, NarrY, zugtyp);
                zeigeWurf(zugtyp);
            } while (!done);

            Console.Clear();
            zeigeSchachbrett(NarrX, NarrY, zugtyp);

            return koordinaten;
        }

        // Narrenzug ausfuehren
        public void zugAusfuehren(int[] zielfeld)
        {
            schachbrett.setSide(NarrX, NarrY, -1);

            schachbrett.setSide(NarrX, NarrY, -1);
            schachbrett.setEifer(NarrX, NarrY, 0);
            schachbrett.setType(NarrX, NarrY, -1);

            NarrX = zielfeld[0];
            NarrY = zielfeld[1];

            schachbrett.setSide(NarrX, NarrY, 0);
            schachbrett.setEifer(NarrX, NarrY, 0);
            schachbrett.setType(NarrX, NarrY, 0);
            schachbrett.setPrimaerZiel(NarrX, NarrY, false);
            schachbrett.setWachsam(NarrX, NarrY, false);
        }

        // SpielNr. und Runde anzeigen
        public void zeigeRunde()
        {
            Console.Write("Game Nr.: " + spielNr);
            Console.Write("\tDifficulty: " + schwierigkeitsgrad);
            Console.Write("\tRound: " + runde);
            Console.WriteLine("\tHits: " + figurenGeschlagen);
            
            Console.WriteLine();
        }

        // Stats anzeigen
        public void zeigeStats()
        {
            Console.WriteLine("Fool Victories {0} : {1} Enemy Victories ", siegeNarr, siegeGegner);
            Console.WriteLine("Hits this round {0} : {1} Total hits ", figurenGeschlagen, gesamtGeschlagen);
            Console.WriteLine("Rounds total: " + rundengesamt);
            Console.WriteLine();
        }

        // Startbildschirm
        public void showEinfuehrung()
        {
            Console.Clear();

            Console.WriteLine("*********************************************");
            Console.WriteLine("*                                           *");
            Console.WriteLine("*               Welcome to                  *");
            Console.WriteLine("*                                           *");
            Console.WriteLine("*          f O o L's  c H e s s             *");
            Console.WriteLine("*                                           *");
            Console.WriteLine("*      Engage massive enemy armies          *");
            Console.WriteLine("*    of dumb and lazy but determined        *");
            Console.WriteLine("*              chess goons!                 *");
            Console.WriteLine("*                                           *");
            Console.WriteLine("*      You are armed with nothing but       *");
            Console.WriteLine("*     your wits and your luck with dice     *");
            Console.WriteLine("*                                           *");
            Console.WriteLine("*             tactics & luck                *");
            Console.WriteLine("*     adapted dice & adapted chessboard     *");
            Console.WriteLine("*                                           *");
            Console.WriteLine("*********************************************");
            Console.WriteLine("*********************************************");
            Console.WriteLine("*                                           *");
            Console.WriteLine("*               NeW in V0.13:               *");
            Console.WriteLine("*               Level Editor                *");
            Console.WriteLine("*                                           *");
            Console.WriteLine("*********************************************");
            Console.WriteLine("*********************************************");
            Console.WriteLine("*       Copyright Andreas Warta 2015        *");
            Console.WriteLine("**************<press any key>****************");
        }

        // Helpbildschirm
        public void showHelp()
        {
            Console.Clear();

            
            Console.WriteLine("*********************************************");
            Console.WriteLine("*                                           *");
            Console.WriteLine("*     You want help? There is no help.      *");
            Console.WriteLine("*         There is only madness.....        *");
            Console.WriteLine("*                                           *");
            Console.WriteLine("*********************************************");
            Console.WriteLine("*                                           *");
            Console.WriteLine("*     ?                                     *");
            Console.WriteLine("*     F : The Fool - Protagonist            *");
            Console.WriteLine("*                                           *");
            Console.WriteLine("*     K : Enemy King                        *");
            Console.WriteLine("*     Q : Enemy Queen                       *");
            Console.WriteLine("*     N : Enemy kNight                      *");
            Console.WriteLine("*     B : Enemy Bishop                      *");
            Console.WriteLine("*     R : Enemy Rook                        *");
            Console.WriteLine("*     P : Enemy Pawn                        *");
            Console.WriteLine("*                                           *");
            Console.WriteLine("*     # : Obstacle                          *");
            Console.WriteLine("*                                           *");
            Console.WriteLine("*********************************************");
            Console.WriteLine("*                                           *");
            Console.WriteLine("*     * : Primary tagret -> Hit to win      *");
            Console.WriteLine("*                                           *");
            Console.WriteLine("*     1 - 9 under symbols:                  *");
            Console.WriteLine("*     Probability to move in % / 10         *");
            Console.WriteLine("*                                           *");
            Console.WriteLine("*     ! : Quickness on, moves early         *");
            Console.WriteLine("*                                           *");
            Console.WriteLine("*     + : Possible fool move                *");
            Console.WriteLine("*                                           *");
            Console.WriteLine("*     > : (probably foolish) hit possible   *");
            Console.WriteLine("*                                           *");
            Console.WriteLine("*********************************************");
            Console.WriteLine("*                                           *");
            Console.WriteLine("*     Fool move is randomized by dice       *");
            Console.WriteLine("*                                           *");
            Console.WriteLine("*       Randomized test levels with         *");
            Console.WriteLine("*     controlled enemy/obstacle numbers     *");
            Console.WriteLine("*                                           *");
            Console.WriteLine("*********************************************");
            Console.WriteLine("*                                           *");
            Console.WriteLine("*                  Editor                   *");
            Console.WriteLine("*                                           *");
            Console.WriteLine("*     To edit an individual field in a      *");
            Console.WriteLine("*    given level: use 'e' in ingame menu    *");
            Console.WriteLine("*                                           *");
            Console.WriteLine("*    To build a new Level from scratch      *");
            Console.WriteLine("*           use 'n' in ingame menu          *");
            Console.WriteLine("*                                           *");
            Console.WriteLine("*    To save the current Level anytime      *");
            Console.WriteLine("*           use 's' in ingame menu          *");
            Console.WriteLine("*                                           *");
            Console.WriteLine("*********************************************");
            Console.WriteLine("*                                           *");
            Console.WriteLine("*                 Have Fun!                 *");
            Console.WriteLine("*           Muahahahahahahahaaahaaa         *");
            Console.WriteLine("*                                           *");
            Console.WriteLine("**************<Press any key>****************");

            Console.ReadKey();
            Console.Clear();
            zeigeSchachbrett();
            zeigeRunde();
        }

        // Gewuenschte Gegnerzahl eingeben
        public void enterEnemyNr()
        {
            try
            {
                Console.Write("Enter max. Nr. of enemies (32max) or <return> for ParaDoXs Classic>");
                gegnerWunsch = Convert.ToInt32(Console.ReadLine());
                Console.Write("Enter max. Nr. of obstacles (32max) or <return> for ParaDoXs Classic> >");
                hinderWunsch = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception)
            {
                gegnerWunsch = 10;
                hinderWunsch = 20;
            }
        }
    }
}
