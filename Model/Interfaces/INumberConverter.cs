namespace Model.Interfaces
{
    public interface INumberConverter
    {
        public short RomanToArabic(string incomingRomanNumber);
        public string ArabicToRoman(int incomingArabicNumber);
    }
}