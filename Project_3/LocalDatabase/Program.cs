using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalDatabase
{
	public class Program
	{
		static void Main(string[] args)
		{
            string option;
            do
            {
                Console.WriteLine("Unesite region ili regione za koji želite da se formira lokalna baza: ");
                option = Console.ReadLine();

                string[] words = option.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                bool validInput = true;

                if (words.Length == 1 || words.Length >= 2)
                {
                    foreach (var word in words)
                    {
                        if (word.Any(char.IsDigit) || string.IsNullOrWhiteSpace(word))
                        {
                            validInput = false;
                            break;
                        }
                    }
                }
                else
                {
                    validInput = false;
                }

                if (validInput)
                {
                    break;
                }

                Console.WriteLine("Greška!! Unesite ili jedan region ili više regiona u formatu 'region, region' bez számokat és felesleges szóközöket!\n");
            } while (true);

        }

        // Funkcija koja parsira unet region ili regione
        public static string[] ParseRegions(string input)
        {
            string[] regions = input.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (regions.Length == 1)
            {
                return new string[] { regions[0] };
            }
            if (regions.Length >= 2)
            {
                string[] retV = new string[regions.Length];
                for (int i = 0; i < regions.Length; i++)
                {
                    retV[i] = regions[i].Trim();
                }

                return retV;
            }

            return null;
        }

    }
}
