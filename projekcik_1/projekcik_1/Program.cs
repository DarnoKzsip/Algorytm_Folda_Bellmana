using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projekcik_1//Algorytm_Folda_Bellmana
{
    class Program
    {
        static void Main(string[] args)
        {
            const short BrakDrogi = short.MaxValue;
            int n;//Liczba wierzchołków grafu
            int i, j, k; //pomocnicze zmienne indeksowe
            string INPUT;
            Console.WriteLine("\n\tProgram realizuje algorytm Forda-Bellmana, który umożliwa wyznaczenie" +
                              " najmniejszych odległości od podanego \n\twierzchlołka do wszystkich " +
                              "pozostałych wierzchołków w grafie skierowanym (bez cykli)");
            /* wczytanie danych wejściowych, kolejność danych wejściowych: n(liczba węzłów),
             * macierz wag, warunek wejściowy: n>0 i graf nie może mieć cykli ujemnych*/

            do
            {
                Console.Write("\n\n\tPodaj liczbę węzłów grafu (n>0): ");
                while (!int.TryParse(Console.ReadLine(), out n))
                {//sygnalizacje błędu
                    Console.WriteLine("\n\tERROR: Wystąpił niedozwolony znak w zapisie liczby węzłów grafu");
                    Console.WriteLine("\n\tMasz kolejną szanę: wprowadź tą liczbę jeszcze raz!");

                };
                if (n <= 0) //sprawdzenie warunku wejściowego
                {
                    Console.WriteLine("\n\tERROR: Liczba węzłów w grafie musi spełniać warunek: n>0");
                    Console.WriteLine("\n\tMasz kolejną szanę: wprowadź tą liczbę jeszcze raz!");
                }

            } while (n <= 0);

            //deklaracja i utworzenie egzemplarza macierzy (tablicy) sąsiedztwa(wag)
            int[,] A = new int[n, n];  //A: od Adjacency 
            //wczytanie macierzy wag/sąsiedztwa
            Console.WriteLine("\n\tWczytywanie elementów macierzy wag (wierszami)");
            Console.WriteLine("\n\t(UWAGA: jeżeli między podanymi numerami wierzchołków grafu " +
                " nie ma drogi bezpośredniej, to naciśnij ENTER)");

            for (j = 0; j < A.GetLength(0); j++)
                for (i = 0; i < A.GetLength(1); i++)
                {
                    if (i == j)
                        A[j, i] = 0;
                    else
                    {
                        Console.Write("\n\tPodaj wagę dla krawędzi między węzłami ({0}, {1}) :", j, i);
                        INPUT = Console.ReadLine(); //Wczutanie ciągu znaków (po wciśnieciu ENTER)
                        INPUT = INPUT.Trim(); //funkcja usuwa przypadkowe spacje
                        if (INPUT.Equals(""))
                            //został naciśniety klawisz ENTER, czyli nie ma drogi między tymi węzłami
                            A[j, i] = BrakDrogi;
                        else
                            while (!int.TryParse(INPUT, out A[j, i]))
                            {
                                Console.WriteLine("\n\tERROR: w zapisie podanej wagi krawędzi grafu" +
                                                  " wystąpił niedozwolony znak!");
                                Console.Write("\n\tPodaj tą wagę jeszcze raz: ");
                                //wczytanie nowej wagi
                                INPUT = INPUT = Console.ReadLine(); //Wczytanie ciągu znaków (po wcisnieciu enter)
                                INPUT = INPUT.Trim(); //usuwamy przypadkowe spacje
                            }
                    }
                }
            //deklaracja zmiennej START dla węzła startowego trasy
            int START;
            do
            {
                Console.Write("\n\tPodaj numer węzła startowego grafu ( 0<= START < n): ");
                while (!int.TryParse(Console.ReadLine(), out START))
                {//sygnalizacja błedu
                    Console.WriteLine("\n\tERROR: Wystąpił niedozwolony znak w zapisie" +
                                       "numeru startowego grafu");
                    Console.Write("\n\tWprowadź ten numer jeszcze raz");
                };
                if ((START < 0) || (START > (n - 1)))//sprawdzenie warunku wejściowego
                {
                    Console.WriteLine("\n\tERROR: numer startowy węzła grafu musi spełniać warunek: " +
                                      " 0 <= START < n");
                    Console.WriteLine("\n\tWprowadz ten numer jeszcze raz");
                }
            } while ((START < 0) || (START > (n - 1)));

            //deklaracja i utworzenie egzemplarza wektora odległości D
            int[] D = new int[A.GetLength(0)]; // D: od Distance

            /*Ustawienie stanu początkowego wektora odległości D, który musi być równy wierszowi
             * węzła startowego macierzy wag (sąsiedztwa), co oznacza, że D[. . .] = A[START, . . .] */

            for (int l = 0; l < A.GetLength(0); l++)
                D[l] = A[START, l];

            /*Przeprowadzamy relaksacje (n-1) razy dla grafu o n węzłach
             * Relaksacja (inaczej: osłabiania ograniczeń) krawędzi (i, j) w grafie polega na ,
             * sprawdzaniu, czy możemy "skrócić" ścieżkę (drogę) z węzła i do węzła j, jeśli
             * droga będzie wiodła przez inny (pośredni: k-ty) wierzchołek grafu 
             */
            for(k = START; k < A.GetLength(0) - 2; k++)
            for (i = 0; i < A.GetLength(0); i++)
                for (j = 0; j < A.GetLength(1); j++)
                    //Relaksacja drogi (j,i)
                    if (D[i] > (D[j] + A[j, i]))
                    {
                        D[i] = D[j] + A[j, i];
                        //lub
                        //D[i] = (D[i] > (D[j] + A[j, i])) ? D[j] + A[j ,i] : D[i];
                    }

            /*Sprawdzamy wystąpienie cyyklu ujemnego w grafie ( po wyznaczeniu rozwiązania w
             * wektorze D) 
             */
            for (i = 0; i < A.GetLength(0); i++)
                for (j = 0; j < A.GetLength(1); j++)
                    //Relaksacja drogi (j,i)
                    if (D[i] > (D[j] + A[j, i]))
                    {
                        Console.WriteLine("\n\t Graf zawiera CYKL UJEMNY!!!");
                        //chwilowe zatrzymanie programu
                        Console.WriteLine("\n\tDla kontynuacji działania programu naciśnij dowolny klawisz!");
                        Console.ReadKey();
                        return; //wyjście z programu
                    }
            Console.Write("\n\tKońcowa postać wektora odległości D (najkrótszych dróg) ");
            Console.WriteLine("z węzła {0,3} do pozostałych węzłów :\n\n", START);
            for (int l = 0; l < D.Length; l++)
                if (D[l] < BrakDrogi)
                    Console.Write("\t\tD[{0}] = {1, 3}\n", l, D[l]);
                else
                    Console.Write("\t\tD[{0}] = {1, 3}\n", l, "*");
            Console.WriteLine();
            //wydruk metryki zakończenia programu
            Console.WriteLine("\n\tProgram realizowany na zajęiach labolatoryjnych!");
            Console.WriteLine("\n\tAutor Programu: Grupa3: 2017/2018");
            Console.WriteLine("\n\tData realizacji (ukończenia programu: 15.11.2017)");
            //chwilowe zatrzymanie programu
            Console.WriteLine("\n\tDla kontynuacji działania programu naciśnij dowolny klawisz!");
            Console.ReadKey();
        }
        }
}
