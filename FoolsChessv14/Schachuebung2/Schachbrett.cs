using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Narrenschach01;

namespace Schachuebung2
{
    class Schachbrett
    {
        private Spielfeld[,] spielfeld; // Array aus Spielfeldern deklarieren
        private int sizeX; // Groesse des Spielfelds in X
        private int sizeY; // Groesse des Spielfelds in Y
        private bool gridAn = true; // grid an/aus

        // Konstruktor fuer Standard 8x8 Schachbrett
        public Schachbrett()
        {
            this.gridAn = true;
            this.sizeX = 8;
            this.sizeY = 8;
            this.spielfeld = new Spielfeld[sizeX, sizeY];

            for (int i = 0; i < sizeY; i++)
            {
                for (int j = 0; j < sizeX; j++)
                {
                    spielfeld[i, j] = new Spielfeld();
                }
            }
        }

        // Konstruktor fuer Custom Schachbretter
        public Schachbrett(int x, int y)
        {
            this.gridAn = true;
            this.sizeX = x;
            this.sizeY = y;
            this.spielfeld = new Spielfeld[sizeX, sizeY];

            for (int i = 0; i < sizeY; i++)
            {
                for (int j = 0; j < sizeX; j++)
                {
                    spielfeld[j, i] = new Spielfeld();
                }
            }
        }

        // Getter und Setter fuer gridAn
        public bool getGrid()
        {
            return gridAn;
        }

        public void setGrid(bool grid)
        {
            this.gridAn = grid;
        }

        // Getter und Setter fuer Seite, Spielfigurtyp, Primaerziel, Eifer und Wachsamkeit nach Koordinaten
        public int getSide(int x, int y)
        {
            return this.spielfeld[x, y].getSide();
        }

        public void setSide(int x, int y, int side)
        {
            this.spielfeld[x, y].setSide(side);
        }

        public int getType(int x, int y)
        {
            return this.spielfeld[x, y].getType();
        }

        public void setType(int x, int y, int type)
        {
            this.spielfeld[x, y].setType(type);
        }

        public int getEifer(int x, int y)
        {
            return this.spielfeld[x, y].getEifer();
        }

        public void setEifer(int x, int y, int e)
        {
            this.spielfeld[x, y].setEifer(e);
        }

        public bool getWachsam(int x, int y)
        {
            return this.spielfeld[x, y].getWachsam();
        }

        public void setWachsam(int x, int y, bool w)
        {
            this.spielfeld[x, y].setWachsam(w);
        }

        public bool getPrimaerZiel(int x, int y)
        {
            return this.spielfeld[x, y].getPrimaerZiel();
        }

        public void setPrimaerZiel(int x, int y, bool p)
        {
            this.spielfeld[x, y].setPrimaerZiel(p);
        }

        // Spielfeldgroesse abfragen
        public int getSizeX()
        {
            return this.sizeX;
        }

        public int getSizeY()
        {
            return this.sizeY;
        }

        // Zahl der Gegner im Spielfeld abfragen
        public int getGegnerzahl()
        {
            int gegner = 0;

            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    if (this.spielfeld[i, j].getSide() == 1)
                        gegner ++;
                }
            }

            return gegner;
        }

        // Array mit Gegnerinfos erstellen
        public Gegnerfigur[] getGegnerArray()
        {
            int anzahl = getGegnerzahl();
            int counter = 0;

            Gegnerfigur[] gegnerarray = new Gegnerfigur[anzahl];
            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    if (getSide(x, y) == 1)
                    {
                        gegnerarray[counter] = new Gegnerfigur(x, y, getType(x, y), getEifer(x, y), getWachsam(x, y), getPrimaerZiel(x, y));
                        counter++;
                    }
                }
            }

            return gegnerarray;
        }

        // Array mit Hinderniskoordinaten erstellen
        public byte[] getHindernisBytes()
        {
            int anzahl = 0;
            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    if (getType(x, y) == 7)
                    {
                        anzahl++;
                    }
                }
            }
            byte[] hindernis = new byte[anzahl * 2];
            int zaehler = 0;

            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    if (getType(x, y) == 7)
                    {
                        hindernis[zaehler] = (byte)x;
                        hindernis[zaehler + 1] = (byte)y;
                        zaehler += 2;
                    }
                }
            }

            return hindernis;
        }

        // Gegnerzug ermitteln und durchfuehren
        public void zieheGegnerFigur(int posX, int posY, int NarrX, int NarrY, int type)
        {
            int[] zugtyp = new int[1];
            zugtyp[0] = type;
            int[,] moeglicheZuege = getZuege(posX, posY, zugtyp, 1);
            double distance;

            if (getPrimaerZiel(posX, posY))
            {
                distance = 0;
            }
            else
            {
                distance = sizeX + sizeY;
            }
            
            int zielX = posX;
            int zielY = posY;

            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    if (moeglicheZuege[x, y] == 1)
                    {
                        zielX = x;
                        zielY = y;
                        x = sizeX - 1;
                        y = sizeY - 1;
                    } 
                    else if (moeglicheZuege[x, y] == 0)
                    {
                        if (getPrimaerZiel(posX, posY))
                        {
                            if (Math.Sqrt(Math.Pow(x - NarrX, 2) + Math.Pow(y - NarrY, 2)) > distance)
                            {
                                distance = Math.Sqrt(Math.Pow(x - NarrX, 2) + Math.Pow(y - NarrY, 2));
                                zielX = x;
                                zielY = y;
                            }
                            else
                            {
                                zielX = posX;
                                zielY = posY;
                            }
                        }
                        else
                        {
                            if (Math.Sqrt(Math.Pow(x - NarrX, 2) + Math.Pow(y - NarrY, 2)) < distance)
                            {
                                distance = Math.Sqrt(Math.Pow(x - NarrX, 2) + Math.Pow(y - NarrY, 2));
                                zielX = x;
                                zielY = y;
                            }
                        }

                    }
                }
            }

            if (moeglicheZuege.Length == 0)
            {
                zielX = posX;
                zielY = posY;
            }

            // Aenderung auf dem Schachbrett ausfuehren, vorher / nachher Status fuer animation speichern
            // wenn Figur Primaerziel und jede Bewegung naeher an Narr bringen wuerde, keine Bewegung

            if (getType(zielX, zielY) == 0 || !(getPrimaerZiel(posX, posY) && Math.Sqrt(Math.Pow(zielX - NarrX, 2) + Math.Pow(zielY - NarrY, 2)) <
                  Math.Sqrt(Math.Pow(posX - NarrX, 2) + Math.Pow(posY - NarrY, 2))))
            {
                string[] vorher = getFullASCIISchachbrett();

                this.setSide(zielX, zielY, getSide(posX, posY));
                this.setEifer(zielX, zielY, getEifer(posX, posY));
                this.setType(zielX, zielY, getType(posX, posY));
                this.setWachsam(zielX, zielY, getWachsam(posX, posY));
                this.setPrimaerZiel(zielX, zielY, getPrimaerZiel(posX, posY));

                if (!(zielX == posX && zielY == posY))
                {
                    this.setSide(posX, posY, -1);
                    this.setEifer(posX, posY, 0);
                    this.setType(posX, posY, -1);
                    this.setWachsam(posX, posY, false);
                    this.setPrimaerZiel(posX, posY, false);
                }

                string[] nachher = getFullASCIISchachbrett();

                for (int i = 0; i < 3; i++)
                {
                    Console.Clear();
                    for (int j = nachher.Length - 1; j >= 0; j--)
                    {
                        Console.Write(nachher[j]);
                    }
                    Thread.Sleep(300);
                    Console.Clear();
                    for (int j = vorher.Length - 1; j >= 0; j--)
                    {
                        Console.Write(vorher[j]);
                    }
                    Thread.Sleep(300);
                }
            }

            if (getType(zielX, zielY) == 1)
            {
                checkForBauerToDame();
            }
        }

        // Bauern umwandeln
        public void checkForBauerToDame()
        {
            for (int x = 0; x < sizeX; x++)
            {
                if (getType(x, 0) == 1 && getSide(x, 0) == 1)
                {
                    setType(x, 0, 5);
                }

                if (getType(x, this.getSizeY() - 1) == 1 && getSide(x, this.getSizeY() - 1) == -1)
                {
                    setType(x, this.getSizeY() - 1, 5);
                }
            }
        }

        // Gegnersieg pruefen
        public bool pruefeGegnerSieg()
        {
            int zielobjekteImSpiel = 0;

            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    if (this.spielfeld[i, j].getType() == 0)
                        zielobjekteImSpiel++;
                }
            }

            if (zielobjekteImSpiel == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Spielersieg pruefen
        public bool pruefeSpielerSieg()
        {
            int zielobjekteImSpiel = 0;

            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    if (this.spielfeld[i, j].getPrimaerZiel())
                        zielobjekteImSpiel++;
                }
            }

            if (zielobjekteImSpiel == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Aktueller Schachbrettstand als ASCII string array
        public string[] getASCIISchachbrett()
        {
            string[] ausgabe = new string[this.sizeY];
            
            for (int i = 0; i < sizeY; i++)
            {
                ausgabe[i] = "";
                for (int j = 0; j < sizeX; j++)
                {
                    switch (getSide(j, i))
                    {
                        case -1:
                            ausgabe[i] += ".";
                            break;
                        case 0:
                            switch (getType(j, i))
                            {
                                case 0:
                                    ausgabe[i] += "N";
                                    break;
                                case 1:
                                    ausgabe[i] += "b";
                                    break;
                                case 2:
                                    ausgabe[i] += "t";
                                    break;
                                case 3:
                                    ausgabe[i] += "l";
                                    break;
                                case 4:
                                    ausgabe[i] += "s";
                                    break;
                                case 5:
                                    ausgabe[i] += "d";
                                    break;
                                case 6:
                                    ausgabe[i] += "k";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case 1:
                            switch (getType(j, i))
                            {
                                case 1:
                                    ausgabe[i] += "B";
                                    break;
                                case 2:
                                    ausgabe[i] += "T";
                                    break;
                                case 3:
                                    ausgabe[i] += "L";
                                    break;
                                case 4:
                                    ausgabe[i] += "S";
                                    break;
                                case 5:
                                    ausgabe[i] += "D";
                                    break;
                                case 6:
                                    ausgabe[i] += "K";
                                    break;
                                case 7:
                                    ausgabe[i] += "#";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                }
                ausgabe[i] += "\n";
            }
            return ausgabe;
        }

        // Zuege fuer position x,y als int Array  
        public int[,] getZuege(int x, int y, int[] zugtyp, int seite)
        {
            int[,] zuege = new int[this.sizeX, this.sizeY];

            // Initialisierung des Felds, das moegliche Zuege abbildet
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    zuege[i, j] = -1;
                }
            }

            for (int i = 0; i < zugtyp.Length; i++)
                {
                    switch (zugtyp[i])
                    {
                        case 1:
                            zuege = wegeBauer(x, y, zuege, seite);
                            break;
                        case 2:
                            zuege = wegeTurm(x, y, zuege, seite);
                            break;
                        case 3:
                            zuege = wegeLaeufer(x, y, zuege, seite);
                            break;
                        case 4:
                            zuege = wegeSpringer(x, y, zuege, seite);
                            break;
                        case 5:
                            zuege = wegeDame(x, y, zuege, seite);
                            break;
                        case 6:
                            zuege = wegeKoenig(x, y, zuege, seite);
                            break;
                        default:
                            break;
                    }
                }

            return zuege;
        }

        // Zuege als ASCII array zurueckgeben
        public string[] getASCIIzuege(int x, int y, int[] zugtyp, int seite)
        {
            int[,] zuege = getZuege(x, y, zugtyp, seite);
			string[] ausgabe = new string[this.sizeY];
			
            // Umwandeln in ASCII string array
            for (int i = 0; i < sizeY; i++)
            {
                ausgabe[i] = "";
                for (int j = 0; j < sizeX; j++)
                {
                    switch (zuege[j, i])
                    {
                        case -1:
                            ausgabe[i] += ".";
                            break;
                        case 0:
                            ausgabe[i] += "+";
                            break;
                        case 1:
                            ausgabe[i] += "*";
                            break;
                        default:
                            break;
                    }
                }
                ausgabe[i] += "\n";
            }
            return ausgabe;
        }

        // Moegliche Turmzuege abfragen: int array -1 kein weg, 0 weg, 1 kann schlagen
        public int[,] wegeTurm(int x, int y, int[,] wege, int seite)
        {
            int gegner;
            if (seite == 0)
            {
                gegner = 1;
            }
            else
            {
                gegner = 0;
            }

            // Turmzuege in x-Richtung +
            if (x < (this.sizeX - 1))
            {
                for (int i = x; i < (this.sizeX -1); i++)
                {
                    if (-1 == this.getSide(i + 1, y))
                    {
                        wege[i + 1, y] = 0;
                    }
                    else if (this.getType(i + 1, y) == 7)
                    {
                        i = (this.sizeX - 1);
                    }
                    else if (gegner == this.getSide(i + 1, y))
                    {
                        wege[i + 1, y] = 1;
                        i = (this.sizeX - 1);
                    }
                    else if (seite == this.getSide(i + 1, y) || this.getType(i + 1, y) == 7)
                    {
                        i = (this.sizeX - 1);
                    }
                }
            }
            
            // Turmzuege in x-Richtung -
            if (x > 0)
            {
                for (int i = x; i > 0; i--)
                {
                    if (-1 == this.getSide(i - 1, y))
                    {
                        wege[i - 1, y] = 0;
                    }
                    else if (this.getType(i - 1, y) == 7)
                    {
                        i = 0;
                    }
                    else if (gegner == this.getSide(i - 1, y))
                    {
                        wege[i - 1, y] = 1;
                        i = 0;
                    }
                    else if (seite == this.getSide(i - 1, y))
                    {
                        i = 0;
                    }
                }
            }
            
            // Turmzuege in y-Richtung +
            if (y < (this.sizeY - 1))
            {
                for (int i = y; i < (this.sizeY - 1); i++)
                {
                    if (-1 == this.getSide(x, i + 1))
                    {
                        wege[x, i + 1] = 0;
                    }
                    else if (this.getType(x, i + 1) == 7)
                    {
                        i = (this.sizeY - 1);
                    }
                    else if (gegner == this.getSide(x, i + 1))
                    {
                        wege[x, i + 1] = 1;
                        i = (this.sizeY - 1);
                    }
                    else if (seite == this.getSide(x, i + 1))
                    {
                        i = (this.sizeY - 1);
                    }
                }
            }
            
            // Turmzuege in y-Richtung -
            if (y > 0)
            {
                for (int i = y; i > 0; i--)
                {
                    if (-1 == this.getSide(x, i - 1))
                    {
                        wege[x, i - 1] = 0;
                    }
                    else if (this.getType(x, i - 1) == 7)
                    {
                        i = 0;
                    }
                    else if (gegner == this.getSide(x, i - 1))
                    {
                        wege[x, i - 1] = 1;
                        i = 0;
                    }
                    else if (seite == this.getSide(x, i - 1))
                    {
                        i = 0;
                    }
                }
            }
            
            return wege;
        }

        // Moegliche Laeuferzuege abfragen: int array -1 kein weg, 0 weg, 1 kann schlagen
        public int[,] wegeLaeufer(int x, int y, int[,] wege, int seite)
        {
            int gegner;
            if (seite == 0)
            {
                gegner = 1;
            }
            else
            {
                gegner = 0;
            }

            int counter;
            int xhelper;
            int yhelper;

            // Laeuferzuege nach rechts oben
            if (x < (this.sizeX - 1) && y < (this.sizeY - 1))
            {
                if ((this.sizeY - 1) - y < (this.sizeX - 1) - x)
                {
                    counter = (this.sizeY - 1) - y;
                }
                else
                {
                    counter = (this.sizeX - 1) - x;
                }

                for (int i = 1; i <= counter; i++)
                {
                    xhelper = x + i;
                    yhelper = y + i;

                    if (-1 == this.getSide(xhelper, yhelper))
                    {
                        wege[xhelper, yhelper] = 0;
                    }
                    else if (this.getType(xhelper, yhelper) == 7)
                    {
                        i = counter;
                    }
                    else if (gegner == this.getSide(xhelper, yhelper))
                    {
                        wege[xhelper, yhelper] = 1;
                        i = counter;
                    }
                    else if (seite == this.getSide(xhelper, yhelper))
                    {
                        i = counter;
                    }
                }
            }
            // Laeuferzuege nach links unten
            if (x > 0 && y > 0)
            {
                if (y < x)
                {
                    counter = y;
                }
                else
                {
                    counter = x;
                }

                for (int i = 1; i <= counter; i++)
                {
                    xhelper = x - i;
                    yhelper = y - i;

                    if (-1 == this.getSide(xhelper, yhelper))
                    {
                        wege[xhelper, yhelper] = 0;
                    }
                    else if (this.getType(xhelper, yhelper) == 7)
                    {
                        i = counter;
                    }
                    else if (gegner == this.getSide(xhelper, yhelper))
                    {
                        wege[xhelper, yhelper] = 1;
                        i = counter;
                    }
                    else if (seite == this.getSide(xhelper, yhelper))
                    {
                        i = counter;
                    }
                }
            }
            // Laeuferzuege nach rechts unten
            if (x < (this.sizeX - 1) && y > 0)
            {
                if (y < (this.sizeX - 1) - x)
                {
                    counter = y;
                }
                else
                {
                    counter = (this.sizeX - 1) - x;
                }

                for (int i = 1; i <= counter; i++)
                {
                    xhelper = x + i;
                    yhelper = y - i;

                    if (-1 == this.getSide(xhelper, yhelper))
                    {
                        wege[xhelper, yhelper] = 0;
                    }
                    else if (this.getType(xhelper, yhelper) == 7)
                    {
                        i = counter;
                    }
                    else if (gegner == this.getSide(xhelper, yhelper))
                    {
                        wege[xhelper, yhelper] = 1;
                        i = counter;
                    }
                    else if (seite == this.getSide(xhelper, yhelper))
                    {
                        i = counter;
                    }
                }
            }
            // Laeuferzuege nach links oben
            if (x > 0 && y < (this.sizeY - 1))
            {
                if ((this.sizeY - 1) - y < x)
                {
                    counter = (this.sizeY - 1) - y;
                }
                else
                {
                    counter = x;
                }

                for (int i = 1; i <= counter; i++)
                {
                    xhelper = x - i;
                    yhelper = y + i;

                    if (-1 == this.getSide(xhelper, yhelper))
                    {
                        wege[xhelper, yhelper] = 0;
                    }
                    else if (this.getType(xhelper, yhelper) == 7)
                    {
                        i = counter;
                    }
                    else if (gegner == this.getSide(xhelper, yhelper))
                    {
                        wege[xhelper, yhelper] = 1;
                        i = counter;
                    }
                    else if (seite == this.getSide(xhelper, yhelper))
                    {
                        i = counter;
                    }
                }    
            }
            
            return wege;
        }

        // Moegliche Springerzuege abfragen: int array -1 kein weg, 0 weg, 1 kann schlagen
        public int[,] wegeSpringer(int x, int y, int[,] wege, int seite)
        {
            int gegner;
            if (seite == 0)
            {
                gegner = 1;
            }
            else
            {
                gegner = 0;
            }
            
            // Verschiebungen nach x und y beim Springerzug
            int[] vX = new int[8] { 1, 2, 1, 2, -1, -2, -1, -2 };
            int[] vY = new int[8] { 2, 1, -2, -1, 2, 1, -2, -1 };

            for (int i = 0; i < 8; i++)
            {
                if (x + vX[i] < this.sizeX && y + vY[i] < this.sizeY && x + vX[i] >= 0 && y + vY[i] >= 0)
                {
                    if (-1 == this.getSide(x + vX[i], y + vY[i]))
                    {
                        wege[x + vX[i], y + vY[i]] = 0;
                    }
                    else if (gegner == this.getSide(x + vX[i], y + vY[i]) && !(this.getType(x + vX[i], y + vY[i]) == 7))
                    {
                        wege[x + vX[i], y + vY[i]] = 1;
                    }
                }
            }
            return wege;
        }

        // Moegliche Damezuege abfragen: int array -1 kein weg, 0 weg, 1 kann schlagen
        public int[,] wegeDame(int x, int y, int[,] wege, int seite)
        {
            wege = wegeTurm(x, y, wege, seite);
            wege = wegeLaeufer(x, y, wege, seite);

            return wege;
        }

        // Moegliche Koenigzuege abfragen: int array -1 kein weg, 0 weg, 1 kann schlagen
        public int[,] wegeKoenig(int x, int y, int[,] wege, int seite)
        {
            int gegner;
            if (seite == 0)
            {
                gegner = 1;
            }
            else
            {
                gegner = 0;
            }
            // Verschiebungen nach x und y beim Koenigszug
            int[] vX = new int[8] { 1, 1, 1, -1, -1, -1, 0, 0 };
            int[] vY = new int[8] { 1, 0, -1, 1, 0, -1, 1, -1 };

            for (int i = 0; i < 8; i++)
            {
                if (x + vX[i] < this.sizeX && y + vY[i] < this.sizeY && x + vX[i] >= 0 && y + vY[i] >= 0)
                {
                    if (-1 == this.getSide(x + vX[i], y + vY[i]))
                    {
                        wege[x + vX[i], y + vY[i]] = 0;
                    }
                    else if (gegner == this.getSide(x + vX[i], y + vY[i]) && !(this.getType(x + vX[i], y + vY[i]) == 7))
                    {
                        wege[x + vX[i], y + vY[i]] = 1;
                    }
                }
            }
            return wege;
        }

        // Moegliche Bauernzuege abfragen: int array -1 kein weg, 0 weg, 1 kann schlagen
        public int[,] wegeBauer(int x, int y, int[,] wege, int seite)
        {
            int direction;
			if (seite == 0)
			{
				direction = 1;
			}
			else
			{
				direction = -1; 
			}
			
			int gegner;
            if (seite == 0)
            {
                gegner = 1;
            }
            else
            {
                gegner = 0;
            }
			
			if (y + direction < this.sizeY && y + direction >= 0)
			{
				if (-1 == this.getSide(x, y + direction))
				{
					wege[x, y + direction] = 0;
				}
			}

            if ((y == 1 && seite == 0) || (y == 6 && seite == 1))
			{
				if (-1 == this.getSide(x, y + direction * 2))
				{
					wege[x, y + direction * 2] = 0;
				}
			}

            if ((y + direction < this.sizeY && this.getSide(x, y) == 0) && (y + direction >= 0 && this.getSide(x, y) == 1) && x + 1 < this.sizeX && x - 1 >= 0)
			{
                if (gegner == this.getSide(x + 1, y + direction) && !(this.getType(x + 1, y + direction) == 7))
				{
					wege[x + 1, y + direction] = 1;
				}
			}

            if ((y + direction < this.sizeY && this.getSide(x, y) == 0) && (y + direction >= 0 && this.getSide(x, y) == 1) && x - 1 >= 0 && x + 1 < this.sizeX)
			{
                if (gegner == this.getSide(x - 1, y + direction) && !(this.getType(x - 1, y + direction) == 7))
				{
					wege[x - 1, y + direction] = 1;
				}
			}
			
            return wege;
        }

        // Aktueller Schachbrettstand als vollstaendiges ASCII string array
        public string[] getFullASCIISchachbrett()
        {
            
            string[] ausgabe = new string[this.sizeY * 4 + 2];

            string trenner = "  |";

            for (int i = 0; i < sizeX; i++)
                trenner += "    ";

            bool feldAn = true;

            trenner += "|\n";

	        for (int i = 0; i < sizeY; i++)
	        {
                ausgabe[i * 4 + 1] = trenner;
		        ausgabe[i * 4 + 2] = "  |";
                if ((i + 1) < 10)
                {
                    ausgabe[i * 4 + 3] = " " + (i + 1) + "|";
                }
                else
                {
                    ausgabe[i * 4 + 3] = "" + (i + 1) + "|";
                }
		        ausgabe[i * 4 + 4] = "  |";
		
		        for (int j = 0; j < sizeX; j++)
		        {
                    if (getPrimaerZiel(j, i))
                    {
                        ausgabe[i * 4 + 4] += "*";
                    }
                    else
                    {
                        ausgabe[i * 4 + 4] += " ";
                    }

                    if (getType(j, i) == 0)
                    {
                        ausgabe[i * 4 + 4] += "?";
                    }
		            else
		            {
                        ausgabe[i * 4 + 4] += " ";
		            }

                    if (getWachsam(j, i))
                    {
                        ausgabe[i * 4 + 4] += "! ";
                    }
                    else
                    {
                        ausgabe[i * 4 + 4] += "  ";
                    }
                    
		        }
                ausgabe[i * 4 + 4] += "|\n";

	            for (int j = 0; j < sizeX; j++)
	            {
	                switch (getSide(j, i))
	                {
	                    case -1:
	                        ausgabe[i*4 + 3] += "    ";
	                        break;
	                    case 0:
	                        switch (getType(j, i))
	                        {
	                            case 0:
	                                ausgabe[i*4 + 3] += " F  ";
	                                break;
	                            case 1:
	                                ausgabe[i*4 + 3] += " p  ";
	                                break;
	                            case 2:
	                                ausgabe[i*4 + 3] += " r  ";
	                                break;
	                            case 3:
	                                ausgabe[i*4 + 3] += " b  ";
	                                break;
	                            case 4:
	                                ausgabe[i*4 + 3] += " n  ";
	                                break;
	                            case 5:
	                                ausgabe[i*4 + 3] += " q  ";
	                                break;
	                            case 6:
	                                ausgabe[i*4 + 3] += " k  ";
	                                break;
                                case 7:
                                    ausgabe[i * 4 + 3] += " #  ";
                                    break;
	                            default:
	                                break;
	                        }
	                        break;
	                    case 1:
	                        switch (getType(j, i))
	                        {
	                            case 1:
	                                ausgabe[i*4 + 3] += " P  ";
	                                break;
	                            case 2:
	                                ausgabe[i*4 + 3] += " R  ";
	                                break;
	                            case 3:
	                                ausgabe[i*4 + 3] += " B  ";
	                                break;
	                            case 4:
	                                ausgabe[i*4 + 3] += " N  ";
	                                break;
	                            case 5:
	                                ausgabe[i*4 + 3] += " Q  ";
	                                break;
	                            case 6:
	                                ausgabe[i*4 + 3] += " K  ";
	                                break;
	                            default:
	                                break;
	                        }
	                        break;
	                    default:
	                        break;
	                }
	            }
                ausgabe[i * 4 + 3] += "|" + (i + 1) + "\n";

	            for (int j = 0; j < sizeX; j++)
	            {
	                if (getEifer(j, i) > 0)
	                {
                        ausgabe[i*4 + 2] += " " + getEifer(j, i) + "  ";
                        
	                }
	                else
	                {
                        if (gridAn)
                        {
                            ausgabe[i * 4 + 2] += " -  ";
                            
                        }
                        else
                        {
                            ausgabe[i * 4 + 2] += "    ";
                            
                        }
	                }
	            }
                ausgabe[i * 4 + 2] += "|\n";
	        }
            string skala = "   ";
            for (int i = 1; i <= sizeX; i++)
            {
                if (i < 10)
                {
                    skala += "_" + i + "__";
                }
                else
                {
                    skala += "_" + i + "_";
                }
            }
            ausgabe[sizeY * 4 + 1] = skala + "\n";
            ausgabe[0] = skala + "\n";
            return ausgabe;
        }

        // Aktueller Schachbrettstand als vollstaendiges ASCII string array mit Zuegen
        public string[] getFullASCIISchachbrett(int x, int y, int[] zugtyp)
        {
            int[,] zuege = getZuege(x, y, zugtyp, 0);
            
            string[] ausgabe = new string[this.sizeY * 4 + 2];

            string trenner = "  |";

            bool feldAn = true;

            for (int i = 0; i < sizeX; i++)
                trenner += "    ";

            trenner += "|\n";

            for (int i = 0; i < sizeY; i++)
            {
                ausgabe[i * 4 + 1] = trenner;
                ausgabe[i * 4 + 2] = "  |";
                if ((i + 1) < 10)
                {
                    ausgabe[i * 4 + 3] = " " + (i + 1) + "|";
                }
                else
                {
                    ausgabe[i * 4 + 3] = "" + (i + 1) + "|";
                }
                ausgabe[i * 4 + 4] = "  |";

                for (int j = 0; j < sizeX; j++)
                {
                    if (getPrimaerZiel(j, i))
                    {
                        ausgabe[i * 4 + 4] += "*";
                    }
                    else
                    {
                        ausgabe[i * 4 + 4] += " ";
                    }

                    if (getType(j, i) == 0)
                    {
                        ausgabe[i * 4 + 4] += "?";
                    }
                    else
                    {
                        ausgabe[i * 4 + 4] += " ";
                    }

                    if (getWachsam(j, i))
                    {
                        ausgabe[i * 4 + 4] += "! ";
                    }
                    else
                    {
                        ausgabe[i * 4 + 4] += "  ";
                    }

                }
                ausgabe[i * 4 + 4] += "|\n";

                for (int j = 0; j < sizeX; j++)
                {
                    if (zuege[j, i] == 0)
                    {
                        ausgabe[i*4 + 3] += "+";
                    }
                    else if (zuege[j, i] == 1)
                    {
                        ausgabe[i*4 + 3] += ">";
                    }
                    else
                    {
                        ausgabe[i*4 + 3] += " ";
                    }

                    switch (getSide(j, i))
                    {
                        case -1:
                            ausgabe[i * 4 + 3] += "   ";
                            break;
                        case 0:
                            switch (getType(j, i))
                            {
                                case 0:
                                    
                                    ausgabe[i * 4 + 3] += "F  ";
                                    
                                    break;
                                case 1:
                                    ausgabe[i * 4 + 3] += "p  ";
                                    break;
                                case 2:
                                    ausgabe[i * 4 + 3] += "r  ";
                                    break;
                                case 3:
                                    ausgabe[i * 4 + 3] += "b  ";
                                    break;
                                case 4:
                                    ausgabe[i * 4 + 3] += "n  ";
                                    break;
                                case 5:
                                    ausgabe[i * 4 + 3] += "q  ";
                                    break;
                                case 6:
                                    ausgabe[i * 4 + 3] += "k  ";
                                    break;
                                case 7:
                                    ausgabe[i * 4 + 3] += "#  ";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case 1:
                            switch (getType(j, i))
                            {
                                case 1:
                                    ausgabe[i * 4 + 3] += "P  ";
                                    break;
                                case 2:
                                    ausgabe[i * 4 + 3] += "R  ";
                                    break;
                                case 3:
                                    ausgabe[i * 4 + 3] += "B  ";
                                    break;
                                case 4:
                                    ausgabe[i * 4 + 3] += "N  ";
                                    break;
                                case 5:
                                    ausgabe[i * 4 + 3] += "Q  ";
                                    break;
                                case 6:
                                    ausgabe[i * 4 + 3] += "K  ";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                }
                ausgabe[i * 4 + 3] += "|" + (i + 1) + "\n";

                for (int j = 0; j < sizeX; j++)
                {
                    if (getEifer(j, i) > 0)
                    {
                        
                        ausgabe[i * 4 + 2] += " " + getEifer(j, i) + "  ";
                    }
                    else
                    {
                        if (gridAn)
                        {
                            ausgabe[i*4 + 2] += " -  ";
                        }
                        else
                        {
                            ausgabe[i * 4 + 2] += "    ";
                        }
                    }
                }
                ausgabe[i * 4 + 2] += "|\n";
            }
            string skala = "   ";
            for (int i = 1; i <= sizeX; i++)
            {
                if (i < 10)
                {
                    skala += "_" + i + "__";
                }
                else
                {
                    skala += "_" + i + "_";
                }
     
            }
            ausgabe[sizeY * 4 + 1] = skala + "\n";
            ausgabe[0] = skala + "\n";
            return ausgabe;
        }
    }
}
