using Model.Interfaces;
using System.Text.RegularExpressions;

namespace Model
{
    public class Calculator
    {
        private readonly IExpressionVerififer _expressionVerifyer;
        private readonly INumberConverter _numberConverter;
        private readonly Dictionary<char, short> _auxDigitsRelation;

        public Calculator(IExpressionVerififer expressionVerififer, INumberConverter numberConverter)
        {
            _expressionVerifyer = expressionVerififer;
            _numberConverter = numberConverter;
            _auxDigitsRelation = new Dictionary<char, short> { { '(', 1 }, { ')', 2 }, { '+', 3 }, { '-', 4 }, { '*', 5 } };
        }

        public string Evaluate(string incomingString)
        {
            incomingString = Regex.Replace(incomingString, @"\s+", "");

            if (!_expressionVerifyer.VerifySymbols(incomingString) || !_expressionVerifyer.VerifyBrackets(incomingString))
                throw new ArgumentException("Incoming string invalid.");

            var auxList = GetAuxillaryList(incomingString);
            var answer = EvaluateAuxList(auxList);

            return _numberConverter.ArabicToRoman(answer);
        }

        public List<(int value, bool isSymbol)> GetAuxillaryList(string incomingStrnig)
        {
            if (string.IsNullOrEmpty(incomingStrnig))
                throw new ArgumentException(incomingStrnig);

            var answer = new List<(int, bool)>();
            var currentDigitLength = 0;
            var stringLength = incomingStrnig.Length;

            for (var i = 0; i < stringLength; i++)
            {
                var digitEnds = _auxDigitsRelation.TryGetValue(incomingStrnig[i], out var value);
                if (digitEnds)
                {
                    if (currentDigitLength != 0)
                        answer.Add((_numberConverter.RomanToArabic(incomingStrnig[(i - currentDigitLength)..i]), false));

                    currentDigitLength = 0;
                    answer.Add((value, true));
                }
                else
                {
                    currentDigitLength++;
                }
            }

            if (currentDigitLength != 0)
            {
                answer.Add((_numberConverter.RomanToArabic(incomingStrnig[(stringLength - currentDigitLength)..(stringLength)]), false));
            }

            return answer;
        }

        public int EvaluateAuxList(List<(int value, bool isSymbol)> auxList)
        {
            var openBracketKey = _auxDigitsRelation.GetValueOrDefault('(');
            var closedBracketKey = _auxDigitsRelation.GetValueOrDefault(')');

            while (true)
            {
                var firstClosedBracketPosition = auxList.IndexOf((closedBracketKey, true));
                if (firstClosedBracketPosition == -1)
                    break;

                var lastOpenedBracketPosition = auxList
                    .Take(firstClosedBracketPosition)
                    .ToList()
                    .LastIndexOf((openBracketKey, true));

                var evaluatedExpression = EvaluateBracketlessExpression(auxList.GetRange
                    (lastOpenedBracketPosition + 1, firstClosedBracketPosition - lastOpenedBracketPosition - 1));

                auxList[lastOpenedBracketPosition] = (evaluatedExpression, false);
                auxList.RemoveRange(lastOpenedBracketPosition + 1, firstClosedBracketPosition - lastOpenedBracketPosition);
            }

            if (auxList.Count == 1)
                return auxList[0].value;

            return EvaluateBracketlessExpression(auxList);
        }

        public int EvaluateBracketlessExpression(IList<(int value, bool isSymbol)> auxList)
        {
            if (!_expressionVerifyer.VerifyBracketlessAuxList(auxList))
                throw new ArgumentException("Try to evaluate incorrect bracketless expression.");

            var plusKey = _auxDigitsRelation.GetValueOrDefault('+');
            var minusKey = _auxDigitsRelation.GetValueOrDefault('-');
            var multiplyKey = _auxDigitsRelation.GetValueOrDefault('*');

            while (true)
            {
                var firstMultiplierPosition = auxList.IndexOf((multiplyKey, true));
                if (firstMultiplierPosition == -1)
                    break;

                auxList[firstMultiplierPosition] = ((auxList[firstMultiplierPosition - 1].value * auxList[firstMultiplierPosition + 1].value), false);
                auxList.RemoveAt(firstMultiplierPosition + 1);
                auxList.RemoveAt(firstMultiplierPosition - 1);
            }

            var answer = auxList.First().value;
            for (int i = 1; i < auxList.Count; i += 2)
            {
                if (auxList[i].value == plusKey)
                    answer += auxList[i + 1].value;
                else if (auxList[i].value == minusKey)
                    answer -= auxList[i + 1].value;
                else
                    throw new ArgumentException("Attempt to evaluate unexpected symbols.");
            }

            return answer;
        }
    }
}