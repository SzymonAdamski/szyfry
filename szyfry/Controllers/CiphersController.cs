using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text;

namespace szyfry.Controllers
{
    public class CiphersController : Controller
    {
        // GET: /Ciphers/Caesar
        [HttpGet]
        public IActionResult Caesar()
        {
            return View();
        }

        // POST: /Ciphers/Caesar
        [HttpPost]
        public IActionResult Caesar(string tekst, int shift, bool decrypt = false)
        {
            string resultText = decrypt ? DeszyfrCezara(tekst, shift) : SzyfrCezara(tekst, shift);
            ViewBag.ResultText = resultText;
            return View();
        }

        private string SzyfrCezara(string tekst, int przesuniecie)
        {
            string alfabet = "AĄBCĆDEĘFGHIJKLŁMNŃOÓPQRSŚTUVWXYZŹŻaąbcćdeęfghijklłmnńoópqrsśtuvwxyzźż";
            int dlugoscAlfabetu = alfabet.Length;
            char[] zaszyfrowanyTekst = new char[tekst.Length];

            for (int i = 0; i < tekst.Length; i++)
            {
                char znak = tekst[i];
                int index = alfabet.IndexOf(znak);

                if (index == -1)
                {
                    zaszyfrowanyTekst[i] = znak; // znak spoza alfabetu, dodaj bez zmian
                }
                else
                {
                    // Obliczanie nowego indeksu z obsługą ujemnych wartości
                    int nowyIndex = (index + przesuniecie) % dlugoscAlfabetu;

                    // Jeśli nowyIndex jest ujemny, dodaj długość alfabetu, aby przeskoczyć na koniec
                    if (nowyIndex < 0)
                    {
                        nowyIndex += dlugoscAlfabetu;
                    }

                    zaszyfrowanyTekst[i] = alfabet[nowyIndex];
                }
            }

            return new string(zaszyfrowanyTekst);
        }

        private string DeszyfrCezara(string tekst, int przesuniecie)
        {
            // Zastosuj przesunięcie odwrotne (ujemne) do deszyfrowania
            return SzyfrCezara(tekst, -przesuniecie);
        }

        // GET: /Ciphers/Polibius
        [HttpGet]
        public IActionResult Polibius()
        {
            return View();
        }

        // POST: /Ciphers/Polibius
        [HttpPost]
        public IActionResult Polibius(string tekst, int a = 1, int b = 0, bool decrypt = false)
        {
            string resultText = decrypt ? DeszyfrPolibiusza(tekst, a, b) : SzyfrPolibiusza(tekst, a, b);
            ViewBag.ResultText = resultText;
            return View();
        }

        private string SzyfrPolibiusza(string tekst, int a, int b)
        {
            Dictionary<char, int> mapaPolibiusza = new Dictionary<char, int>()
            {
                {'A', 11}, {'Ą', 12}, {'B', 13}, {'C', 14}, {'Ć', 15},
                {'D', 21}, {'E', 22}, {'Ę', 23}, {'F', 24}, {'G', 25},
                {'H', 31}, {'I', 32}, {'J', 33}, {'K', 34}, {'L', 35},
                {'Ł', 41}, {'M', 42}, {'N', 43}, {'Ń', 44}, {'O', 45},
                {'Ó', 51}, {'P', 52}, {'R', 54}, {'S', 55}, {'Ś', 61},
                {'T', 62}, {'U', 63}, {'W', 65}, {'Y', 72}, {'Z', 73},
                {'Ź', 74}, {'Ż', 75}, {'a', 11}, {'ą', 12}, {'b', 13},
                {'c', 14}, {'ć', 15}, {'d', 21}, {'e', 22}, {'ę', 23},
                {'f', 24}, {'g', 25}, {'h', 31}, {'i', 32}, {'j', 33},
                {'k', 34}, {'l', 35}, {'ł', 41}, {'m', 42}, {'n', 43},
                {'ń', 44}, {'o', 45}, {'ó', 51}, {'p', 52}, {'r', 54},
                {'s', 55}, {'ś', 61}, {'t', 62}, {'u', 63}, {'w', 65},
                {'y', 72}, {'z', 73}, {'ź', 74}, {'ż', 75}
            };

            StringBuilder zaszyfrowanyTekst = new StringBuilder();

            foreach (char znak in tekst)
            {
                if (mapaPolibiusza.ContainsKey(znak))
                {
                    int x = mapaPolibiusza[znak];
                    int y = a * x * x + b; // zastosowanie wzoru
                    zaszyfrowanyTekst.Append(y.ToString());
                    zaszyfrowanyTekst.Append(" "); // dodaj spację jako separator
                }
                else
                {
                    zaszyfrowanyTekst.Append(znak);
                }
            }

            return zaszyfrowanyTekst.ToString().Trim();
        }

        private string DeszyfrPolibiusza(string tekst, int a, int b)
        {
            Dictionary<int, char> odwrotnaMapaPolibiusza = new Dictionary<int, char>()
            {
                {11, 'A'}, {12, 'Ą'}, {13, 'B'}, {14, 'C'}, {15, 'Ć'},
                {21, 'D'}, {22, 'E'}, {23, 'Ę'}, {24, 'F'}, {25, 'G'},
                {31, 'H'}, {32, 'I'}, {33, 'J'}, {34, 'K'}, {35, 'L'},
                {41, 'Ł'}, {42, 'M'}, {43, 'N'}, {44, 'Ń'}, {45, 'O'},
                {51, 'Ó'}, {52, 'P'}, {54, 'R'}, {55, 'S'}, {61, 'Ś'},
                {62, 'T'}, {63, 'U'}, {65, 'W'}, {72, 'Y'}, {73, 'Z'},
                {74, 'Ź'}, {75, 'Ż'}
            };

            StringBuilder odszyfrowanyTekst = new StringBuilder();
            string[] liczby = tekst.Split(' ');

            foreach (string liczbaStr in liczby)
            {
                if (int.TryParse(liczbaStr, out int y))
                {
                    // Próbujemy znaleźć odpowiedni 'x' rozwiązując równanie y = a * x^2 + b
                    for (int x = 11; x <= 75; x++)
                    {
                        if (a * x * x + b == y && odwrotnaMapaPolibiusza.ContainsKey(x))
                        {
                            odszyfrowanyTekst.Append(odwrotnaMapaPolibiusza[x]);
                            break;
                        }
                    }
                }
                else
                {
                    odszyfrowanyTekst.Append(liczbaStr);
                }
            }

            return odszyfrowanyTekst.ToString();
        }
          [HttpGet]
        public IActionResult Vigenere()
        {
            return View();
        }

        // POST: /Ciphers/Vigenere
        [HttpPost]
        public IActionResult Vigenere(string tekst, string klucz, bool decrypt = false)
        {
            string resultText = decrypt ? DeszyfrVigenere(tekst, klucz) : SzyfrVigenere(tekst, klucz);
            ViewBag.ResultText = resultText;
            return View();
        }

        private string SzyfrVigenere(string tekst, string klucz)
        {
            string alfabet = "AĄBCĆDEĘFGHIJKLŁMNŃOÓPQRSŚTUVWXYZŹŻaąbcćdeęfghijklłmnńoópqrsśtuvwxyzźż";
            int dlugoscAlfabetu = alfabet.Length;
            StringBuilder zaszyfrowanyTekst = new StringBuilder();

            klucz = klucz.ToLower();
            int indexKlucza = 0;

            foreach (char znak in tekst)
            {
                int indexZnaku = alfabet.IndexOf(znak);
                if (indexZnaku == -1)
                {
                    zaszyfrowanyTekst.Append(znak); // Znak spoza alfabetu, dodaj bez zmian
                }
                else
                {
                    int indexKlucz = alfabet.IndexOf(klucz[indexKlucza % klucz.Length]);
                    int nowyIndex = (indexZnaku + indexKlucz) % dlugoscAlfabetu;

                    zaszyfrowanyTekst.Append(alfabet[nowyIndex]);
                    indexKlucza++;
                }
            }

            return zaszyfrowanyTekst.ToString();
        }

        private string DeszyfrVigenere(string tekst, string klucz)
        {
            string alfabet = "AĄBCĆDEĘFGHIJKLŁMNŃOÓPQRSŚTUVWXYZŹŻaąbcćdeęfghijklłmnńoópqrsśtuvwxyzźż";
            int dlugoscAlfabetu = alfabet.Length;
            StringBuilder odszyfrowanyTekst = new StringBuilder();

            klucz = klucz.ToLower();
            int indexKlucza = 0;

            foreach (char znak in tekst)
            {
                int indexZnaku = alfabet.IndexOf(znak);
                if (indexZnaku == -1)
                {
                    odszyfrowanyTekst.Append(znak); // Znak spoza alfabetu, dodaj bez zmian
                }
                else
                {
                    int indexKlucz = alfabet.IndexOf(klucz[indexKlucza % klucz.Length]);
                    int nowyIndex = (indexZnaku - indexKlucz + dlugoscAlfabetu) % dlugoscAlfabetu;

                    odszyfrowanyTekst.Append(alfabet[nowyIndex]);
                    indexKlucza++;
                }
            }

            return odszyfrowanyTekst.ToString();
        }
    }
}
