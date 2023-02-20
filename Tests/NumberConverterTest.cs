namespace Tests
{
    public class NumberConverterTest
    {
        private const short MaximalArabicNumberToConvert = 3999;

        [Theory]
        [InlineData("I", 1)]
        [InlineData("II", 2)]
        [InlineData("X", 10)]
        [InlineData("IX", 9)]
        [InlineData("XI", 11)]
        [InlineData("M", 1000)]
        [InlineData("MI", 1001)]
        public void RomanToArabic_CorrectInput_Succed(string incomingRomanNumber, short expected)
        {
            //Arrange
            var testObject = new NumberConverter();

            //Act
            var actual = testObject.RomanToArabic(incomingRomanNumber);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, "I")]
        [InlineData(2, "II")]
        [InlineData(10, "X")]
        [InlineData(9, "IX")]
        [InlineData(11, "XI")]
        [InlineData(1000, "M")]
        [InlineData(2001, "MMI")]
        public void ArabicToRoman_CorrectInput_Succed(short incomingArabicNumber, string expected)
        {
            //Arrange
            var testObject = new NumberConverter();

            //Act
            var actual = testObject.ArabicToRoman(incomingArabicNumber);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DoubleConversion_AllPossibleInput_Succed()
        {
            //Arrange 
            var testObject = new NumberConverter();
            var expectedElements = Enumerable.Range(1, MaximalArabicNumberToConvert).ToArray();
            var actualElements = new int[MaximalArabicNumberToConvert];

            //Act
            foreach (var element in expectedElements)
            {
                var roman = testObject.ArabicToRoman(element);
                actualElements[element - 1] = testObject.RomanToArabic(roman);
            }

            //Assert
            foreach (var element in expectedElements)
            {
                Assert.Equal(expectedElements[element - 1], actualElements[element - 1]);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(MaximalArabicNumberToConvert + 1)]
        public void ArabicToRoman_OutOfRangeInput_ThrowException(int incomingArabicNumber)
        {
            //Arrange
            var testObject = new NumberConverter();

            //Act
            var act = () => testObject.ArabicToRoman(incomingArabicNumber);

            //Assert
            Assert.Throws<ArgumentOutOfRangeException>(act);
        }

        [Theory]
        [InlineData("m")]
        [InlineData("B")]
        [InlineData("Æ")]
        [InlineData(".")]
        [InlineData("7")]
        [InlineData("")]
        public void RomanToArabic_IncorrectSymbols_ThrowException(string incomingRomanNumber)
        {
            //Arrange
            var testObject = new NumberConverter();

            //Act
            Action act = () => testObject.RomanToArabic(incomingRomanNumber);

            //Assert
            Assert.Throws<ArgumentException>(act);
        }
    }
}