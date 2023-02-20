using Model.Interfaces;

namespace Model.Infrastructure
{
    public class ExpressionVerifier : IExpressionVerififer
    {
        private const string AllowedChars = "IVXLCDM()+-* ";

        public bool VerifySymbols(string incomingString)
        {
            if (string.IsNullOrEmpty(incomingString))
                return false;

            foreach (var element in incomingString)
            {
                if (!AllowedChars.Contains(element))
                    return false;
            }

            return true;
        }

        public bool VerifyBrackets(string incomingString)
        {
            var currentNumber = 0;
            foreach (var element in incomingString)
            {
                if (element == '(')
                {
                    currentNumber++;
                }
                else if (element == ')')
                {
                    currentNumber--;
                    if (currentNumber < 0)
                        return false;
                }
            }
            if (currentNumber != 0)
                return false;

            return true;
        }

        public bool VerifyBracketlessAuxList(IList<(int value, bool isSymbol)> auxList)
        {
            var length = auxList.Count;
            if (length == 0 || length % 2 == 0)
                return false;

            for (int i = 0; i < length; i += 2)
            {
                if (auxList[i].isSymbol == true || auxList[i].isSymbol == true)
                    return false;
            }

            return true;
        }
    }
}