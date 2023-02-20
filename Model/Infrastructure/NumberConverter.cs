using Model.Interfaces;
using System.Text;

namespace Model.Infrastructure
{
    public class NumberConverter : INumberConverter
    {
        private const string AllowedChars = "IVXLCDM";
        private const short MaximalArabicNumberToConvert = 3999;

        public short RomanToArabic(string incomingRomanNumber)
        {
            if (string.IsNullOrEmpty(incomingRomanNumber))
                throw new ArgumentException(incomingRomanNumber);

            foreach (char c in incomingRomanNumber)
            {
                if (!AllowedChars.Contains(c))
                    throw new ArgumentException($"Unexpected char in roman number = {c}.");
            }

            short res = 0;

            for (int i = 0; i < incomingRomanNumber.Length; i++)
            {
                var s1 = ArabicDigit(incomingRomanNumber[i]);

                if (i + 1 < incomingRomanNumber.Length)
                {
                    var s2 = ArabicDigit(incomingRomanNumber[i + 1]);
                    if (s1 >= s2)
                    {
                        res += s1;
                    }
                    else
                    {
                        res += s2;
                        res -= s1;
                        i++;
                    }
                }
                else
                {
                    res += s1;
                    i++;
                }
            }

            return res;
        }

        public string ArabicToRoman(int incomingArabicNumber)
        {
            if (incomingArabicNumber <= 0 || incomingArabicNumber > MaximalArabicNumberToConvert)
                throw new ArgumentOutOfRangeException($"Can't convert arabic number less then 1 and bigger then {MaximalArabicNumberToConvert}.");

            var answer = new StringBuilder();

            while (incomingArabicNumber > 0)
            {
                switch (incomingArabicNumber)
                {
                    case >= 1000:
                        answer.Append('M');
                        incomingArabicNumber -= 1000;
                        break;
                    case >= 900:
                        answer.Append("CM");
                        incomingArabicNumber -= 900;
                        break;
                    case >= 500:
                        answer.Append('D');
                        incomingArabicNumber -= 500;
                        break;
                    case >= 400:
                        answer.Append("CD");
                        incomingArabicNumber -= 400;
                        break;
                    case >= 100:
                        answer.Append('C');
                        incomingArabicNumber -= 100;
                        break;
                    case >= 90:
                        answer.Append("XC");
                        incomingArabicNumber -= 90;
                        break;
                    case >= 50:
                        answer.Append('L');
                        incomingArabicNumber -= 50;
                        break;
                    case >= 40:
                        answer.Append("XL");
                        incomingArabicNumber -= 40;
                        break;
                    case >= 10:
                        answer.Append('X');
                        incomingArabicNumber -= 10;
                        break;
                    case >= 9:
                        answer.Append("IX");
                        incomingArabicNumber -= 9;
                        break;
                    case >= 5:
                        answer.Append('V');
                        incomingArabicNumber -= 5;
                        break;
                    case >= 4:
                        answer.Append("IV");
                        incomingArabicNumber -= 4;
                        break;
                    case >= 1:
                        answer.Append('I');
                        incomingArabicNumber -= 1;
                        break;
                }
            }

            return answer.ToString();
        }

        private short ArabicDigit(char incomingChar)
        {
            if (incomingChar == 'I')
                return 1;
            if (incomingChar == 'V')
                return 5;
            if (incomingChar == 'X')
                return 10;
            if (incomingChar == 'L')
                return 50;
            if (incomingChar == 'C')
                return 100;
            if (incomingChar == 'D')
                return 500;
            if (incomingChar == 'M')
                return 1000;
            return 0;
        }
    }
}