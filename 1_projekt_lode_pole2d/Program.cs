using System.Globalization;

namespace _1_projekt_lode_pole2d
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool znova;
            do
            {
                char[,] herniPole = VyplnPole(); //inicializace herního pole
                UmisteniLode(herniPole); //umístění lodí

                Console.WriteLine("Vítejte ve hře Potápění lodí!");
                Console.WriteLine("Nachází se zde 4 lodě, které je potřeba potopit.");
                int pocetTahu = 0;

                do
                {
                    VypisPole(herniPole);
                    ZadaniTahu(herniPole);
                    pocetTahu++;

                } while (!LodePotopeny(herniPole)); // hra pokračuje, dokud nejsou všechny lodě potopeny
                Console.WriteLine($"Gratuluji! Všechny lodě byly potopeny na {pocetTahu}. tah!");
                Console.WriteLine("Hra skončila.");

                Console.WriteLine("Chceš hrát znovu? a/n");
                string volba = Console.ReadLine().ToLower();
                znova = (volba == "a");
                Console.ReadKey();
            } while (znova);

        }

        static char[,] VyplnPole()
        {
            char[,] pole = new char[10, 13]; 
            for (int r = 0; r < pole.GetLength(0); r++)
            {
                // Podmínky pro první řádek (A-L) a první sloupec (1-9)
                for (int s = 0; s < pole.GetLength(1); s++)
                {
                    if (r == 0 && s > 0)
                    {
                        // Sloupce A-L
                        pole[r, s] = (char)('A' + s - 1);
                    }
                    else if (s == 0 && r > 0)
                    {
                        // Řádky 1-9
                        pole[r, s] = (char)('0' + r);
                    }
                    else
                    {
                        // Prázdné moře '~'
                        pole[r, s] = '~';
                    }     
                }
            }
            return pole;
        }

        static void UmisteniLode(char[,] pole) 
        {
            Random random = new Random();
            for (int i = 0; i < 4; i++)
            {
                int r;
                int s;

                do
                {
                    r = random.Next(1, 11); // Náhodné řádky od 1 do 10
                    s = random.Next(1, 13);  // Náhodné sloupce od 1 do 11

                } while (ObsahujeLod(pole, r, s) || ObsahujeLod(pole, r, s + 1) || ObsahujeLod(pole, r, s + 2) || ObsahujeLod(pole, r, s + 3)); //kontrola pro aktualní pozici, vpravo od akt., vpravo od předchozí akt. 
                if (r >= 0 && r < pole.GetLength(0) && s >= 0 && s + 3 < pole.GetLength(1))
                {
                    // kontrola, zda je možné umístit loď o délce 4 na herní pole tak že
                    // => poslední sloupec (s + 3) (loď) nevyjde mimo hranice pole (délka druhého rozměru pole).
                    for (int j = 0; j < 4; j++)
                    {
                        pole[r, s + j] = 'O'; //umístění lodě na herní pole
                    }
                }
                else
                {
                    // pokud generované pozice jsou mimo rozsah pole, opakovaní generování nových pozic
                    i--; // snížení indexu i, aby byla zachována požadovaná celková délka cyklu
                }
            }
        }

        static bool ObsahujeLod(char[,] pole, int r, int s) //jestli pozice už neobsahuje jinou loď 
        {
            if (r < 0 || r >= pole.GetLength(0) || s < 0 || s >= pole.GetLength(1))
            {
                // indexy jsou mimo rozsah pole
                return false;
            }

            return pole[r, s] == 'O';
        }

        //jaký tah zadáváš
        static void ZadaniTahu(char[,] pole)
        {
            int r;
            int s;

            Console.Write("Zadej řádek 1-9: ");
            while (!int.TryParse(Console.ReadLine(), out r) || r < 1 || r > 9)
            {
                Console.WriteLine("Špatně zadaný řádek! Zkus znovu");
                Console.Write("Zadej řádek 1-9: ");
            }

            Console.Write("Zadej sloupec A-L: ");
            string input = Console.ReadLine().ToUpper();
            s = input.Length == 1 && input[0] >= 'A' && input[0] <= 'L' ? input[0] - 'A' + 1 : -1;
            //uživatel zadá jediné písmeno v rozsahu A-L => nastaví proměnnou s na odpovídající index sloupce (1-12), pokud je vstup neplatný, s bude -1.
            while (s == -1)
            {
                Console.WriteLine("Špatně zadaný sloupec! Zkus znovu");
                Console.Write("Zadej sloupec A-L: ");
                input = Console.ReadLine().ToUpper();
                s = input.Length == 1 && input[0] >= 'A' && input[0] <= 'L' ? input[0] - 'A' + 1 : -1;
            }

            KontrolaTahu(pole, r, s);
        } 

        static void KontrolaTahu(char[,] pole, int r, int s) 
        {
            if (pole[r,s] == 'O')
            {
                Console.WriteLine("Zásah!");
                pole[r, s] = 'X'; //X => loď byla zasažena

                if (LodPotopena(pole, r, s))
                {
                    Console.WriteLine("Loď byla potopena!");
                }
            }
            else
            {
                Console.WriteLine("Trefa vedle, zkus znova");
                pole[r, s] = '/';
            }
        }

        static bool LodPotopena(char[,] pole, int r, int s) //kontrola, zda je jedna celá loď potopená 
        {
            for (int j = 0; j < 4; j++)
            {
                if (s + j <= 12 && pole[r, s + j] == 'O')  // s + j <= 12 při kontrole nevychází mimo rozsah pole
                {
                    return false; // stále existuje nezasažená část lodi
                }
            }
            return true;
        }

        static bool LodePotopeny(char[,] pole) //kontrola, že všechny lodě jsou potopeny 
        {
            for (int r = 1; r < pole.GetLength(0); r++)
            {
                for (int s = 1; s < pole.GetLength(1); s++)
                {
                    if (pole[r,s] == 'O')
                    {
                        return false; //pořád existují lodě které nejsou potopené
                    }
                }
            }
            return true;
        }

        static void VypisPole(char[,] pole)
        {
            Console.WriteLine();
            for (int r = 0; r < pole.GetLength(0); r++)
            {
                for (int s = 0; s < pole.GetLength(1); s++)
                {
                    // char kryti = (pole[r, s] == 'O' ? '~' : pole[r, s]);
                    // Console.Write(kryti + " ");
                    Console.Write(pole[r, s] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}