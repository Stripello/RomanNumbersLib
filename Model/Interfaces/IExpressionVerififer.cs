namespace Model.Interfaces
{
    public interface IExpressionVerififer
    {
        public bool VerifySymbols(string incomingString);
        public bool VerifyBrackets(string incomingString);
        public bool VerifyBracketlessAuxList(IList<(int value, bool isSymbol)> auxList);
    }
}