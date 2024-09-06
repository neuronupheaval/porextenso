using System.Collections.Generic;
using System.Linq;

namespace PorExtenso
{
    internal class Thousand
    {
        static string[] UNITS => new string[]
        {
            "zero",
            "um",
            "dois",
            "três",
            "quatro",
            "cinco",
            "seis",
            "sete",
            "oito",
            "nove"
        };

        static string[] TENS => new string[]
        {
            string.Empty,
            "dez",
            "vinte",
            "trinta",
            "quarenta",
            "cinquenta",
            "sessenta",
            "setenta",
            "oitenta",
            "noventa"
        };

        static string[] HUNDREDS => new string[]
        {
            string.Empty,
            "cento",
            "duzentos",
            "trezentos",
            "quatrocentos",
            "quinhentos",
            "seiscentos",
            "setecentos",
            "oitocentos",
            "novecentos"
        };

        static string[] ROWOFTEN => new string[]
        {
            string.Empty,
            "onze",
            "doze",
            "treze",
            "quatorze",
            "quinze",
            "dezesseis",
            "dezessete",
            "dezoito",
            "dezenove"
        };

        public Thousand Earlier { get; set; }

        public int Power { get; }

        int[] Digits { get; } = new int[] { 0, 0, 0 };

        public Thousand(Queue<char> digits, int power)
        {
            this.Power = power;

            int i = 0;

            do
            {
                this.Digits[i++] = digits.Dequeue() - '0';
            } while (digits.Any());
        }

        public bool HasSeparator => ((this.Digits[2] == 0 && (this.Digits[1] != 0 || this.Digits[0] != 0)) ||
                                    (this.Digits[2] != 0 && this.Digits[1] == 0 && this.Digits[0] == 0)) && (Earlier?.IsZero ?? true);

        public string Separator => this.HasSeparator
            ? " e "
            : (this.IsZero
                ? string.Empty
                : ", ");

        public bool IsZero => (Earlier?.IsZero ?? true) && this.Digits[2] == 0 && this.Digits[1] == 0 && this.Digits[0] == 0;

        public int? Plurality => this.Digits[2] == 0 && this.Digits[1] == 0 && this.Digits[0] == 0 ? (int?)null : 
            (this.Digits[2] == 0 && this.Digits[1] == 0 && this.Digits[0] == 1 
                ? 0 
                : 1);

        public string Parse()
        {
            string result = this.Hundreds() + this.HundredsAndTensSeparator() + this.Tens() + this.TensAndUnitsSeparator() + this.HundredsAndUnitsSeparator() + this.Units();
            return result.Trim();
        }

        private string Hundreds() => this.Digits[2] == 1 && this.Digits[1] == 0 && this.Digits[0] == 0 ? "cem" : 
            (this.Digits[2] != 0 ? HUNDREDS[this.Digits[2]] : string.Empty);

        private string Tens() => this.Digits[1] == 1 && this.Digits[0] != 0 ? ROWOFTEN[this.Digits[0]] : TENS[this.Digits[1]];
        
        private string Units() => this.Digits[1] != 1 && this.Digits[0] != 0 ? UNITS[this.Digits[0]] : string.Empty;
        
        private string HundredsAndTensSeparator() => this.Hundreds().IsNotEmpty() && this.Tens().IsNotEmpty() ? " e " : string.Empty;

        private string TensAndUnitsSeparator() => this.Tens().IsNotEmpty() && this.Units().IsNotEmpty() ? " e " : string.Empty;

        private string HundredsAndUnitsSeparator() => this.Hundreds().IsNotEmpty() && this.Units().IsNotEmpty() && this.TensAndUnitsSeparator().IsEmpty() ? " e " : string.Empty;
    }
}
