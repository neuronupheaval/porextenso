using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PorExtenso
{
    public static class PowersOfThousand
    {
        static Thousand Current { get; set; }

        static string[][] Powers => new string[][] 
        {
            new string[] { string.Empty, "mil", "milhão", "bilhão", "trilhão" },
            new string[] { string.Empty, "mil", "milhões", "bilhões", "trilhões" }
        };

        public static string Parse(string theNumber)
        {
            Stack<Thousand> powersOfThousand = new Stack<Thousand>();

            Queue<char> digits = new Queue<char>();
            int power = 0;

            Current = null;

            foreach (char c in theNumber.Reverse())
            {
                digits.Enqueue(c);
                    
                if (digits.Count == 3)
                {
                    Thousand next = new Thousand(digits, power++);
                    PushEarlier(next, powersOfThousand);
                    digits = new Queue<char>();
                }
            }

            if (digits.Any())
            {
                Thousand next = new Thousand(digits, power++);
                PushEarlier(next, powersOfThousand);
            }
            PushEarlier(null, powersOfThousand);

            StringBuilder sb = new StringBuilder();

            foreach (Thousand powerOfThousand in powersOfThousand)
            {
                string parsed = powerOfThousand.Parse();

                if (!string.IsNullOrEmpty(parsed))
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(powerOfThousand.Separator);
                    }

                    sb.AppendFormat("{0} {1}", parsed, powerOfThousand.Plurality.HasValue
                                                         ? Powers[powerOfThousand.Plurality.Value][powerOfThousand.Power]
                                                         : string.Empty);
                }
            }

            return sb.ToString().TrimEnd();
        }

        static void PushEarlier(Thousand next, Stack<Thousand> powersOfThousand)
        {
            if (Current == null)
            {
                Current = next;
                if (next != null)
                {
                    next.Earlier = null;
                }
            }
            else
            {
                if (next != null)
                {
                    next.Earlier = Current;
                }
                powersOfThousand.Push(Current);
                Current = next;
            }
        }
    }
}
