using System;

namespace PorExtenso
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string input;
            do
            {
                Console.Write("Digite um número (0 para terminar): ");
                input = Console.ReadLine();
                if (!long.TryParse(input, out long theNumber) || theNumber > 999_999_999_999_999 || theNumber < 0)
                {
                    Console.WriteLine("Entrada inválida");
                }
                else
                {
                    Console.WriteLine("---> {0} <---", PorExtenso(input.TrimStart('0', ' ', '\t')));
                }
                Console.WriteLine();
            } while (input != "0");
        }

        static string PorExtenso(string theNumber)
        {
            return PowersOfThousand.Parse(theNumber);
        }
    }
}
