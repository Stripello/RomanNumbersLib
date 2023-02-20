using Model;
using Moq;

namespace Tests
{
    public class CalculatorTest
    {
        private readonly IExpressionVerififer verifier;
        private readonly INumberConverter converter;

        public CalculatorTest()
        {
            var verifierMock = new Mock<IExpressionVerififer>();
            verifierMock.Setup(x => x.VerifyBrackets(It.IsAny<string>())).Returns(true);
            verifierMock.Setup(x => x.VerifySymbols(It.IsAny<string>())).Returns(true);
            verifierMock.Setup(x => x.VerifyBracketlessAuxList(It.IsAny<List<(int, bool)>>())).Returns(true);
            verifier = verifierMock.Object;

            var converterMock = new Mock<INumberConverter>();
            converterMock.Setup(x => x.RomanToArabic(It.IsAny<string>())).Returns(0);
            converter = converterMock.Object;
        }

        [Theory]
        [MemberData(nameof(CorrectData_Bracketless))]
        public void EvaluateBracketlessExpression_CorrectInput_Succed(List<(int, bool)> auxList, int expected)
        {
            //Arrange
            var testObject = new Calculator(verifier, converter);

            //Act
            var actual = testObject.EvaluateBracketlessExpression(auxList);

            //Assert
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> CorrectData_Bracketless()
        {
            yield return new object[] { new List<(int, bool)> { (1, false), (3, true), (2, false) }, 3 };
            yield return new object[] { new List<(int, bool)> { (3, false), (4, true), (1, false), (5, true), (2, false) }, 1 };
            yield return new object[] { new List<(int, bool)> { (3, false), (4, true), (-1, false), (5, true), (2, false) }, 5 };
            yield return new object[] { new List<(int, bool)> { (2, false), (5, true), (0, false), (4, true), (1, false) }, -1 };
        }

        [Theory]
        [MemberData(nameof(IncorrectData_Bracketless))]
        public void EvaluateBracketlessExpression_IncorrectInput_ThrowException(List<(int, bool)> auxList)
        {
            //Arrange
            var testObject = new Calculator(verifier, converter);

            //Act
            Action act = () => testObject.EvaluateBracketlessExpression(auxList);

            //Assert
            Assert.Throws<ArgumentException>(act);
        }

        public static IEnumerable<object[]> IncorrectData_Bracketless()
        {
            yield return new object[] { new List<(int, bool)> { (1, false), (1, true), (2, false) } };
            yield return new object[] { new List<(int, bool)> { (3, false), (4, true), (1, false), (2, true), (2, false) } };
        }

        [Theory]
        [MemberData(nameof(CorrectData_Evaluate))]
        public void EvaluateAuxList_CorrectInput_Succed(List<(int value, bool isSymbol)> auxList, int expected)
        {
            //Arrange
            var testObject = new Calculator(verifier, converter);

            //Act
            var actual = testObject.EvaluateAuxList(auxList);

            //Assert
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> CorrectData_Evaluate()
        {
            yield return new object[] { new List<(int, bool)> { (1, true), (1, false), (3, true), (2, false), (2, true), (4, true), (1, false) }, 2 };
            yield return new object[] { new List<(int, bool)> { (1, true), (2, false), (3, true), (1, true), (1, false), (3, true), (2, false), (2, true), (2, true), (4, true), (1, false) }, 4 };
        }

        [Theory]
        [MemberData(nameof(CorrectData_AuxList))]
        public void GetAuxillaryList_CorrectInput_Succed(List<(int value, bool isSymbol)> expected, string input)
        {
            //Arrange
            var testObject = new Calculator(verifier, converter);

            //Act
            var actual = testObject.GetAuxillaryList(input);

            //Assert
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> CorrectData_AuxList()
        {
            yield return new object[] { new List<(int, bool)> { (0, false), (3, true), (1, true), (0, false), (4, true), (0, false), (2, true) }, "I+(II-I)" };
            yield return new object[] { new List<(int, bool)> { (1, true), (0, false), (3, true), (0, false), (2, true), (4, true), (0, false) }, "(I+II)-I" };
            yield return new object[] { new List<(int, bool)> { (1, true), (0, false), (3, true), (1, true), (0, false), (3, true), (0, false), (2, true), (2, true), (4, true), (0, false) }, "(II+(I+II))-I" };
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void GetAuxillaryList_EmptyOrNullInput_ThrowException(string input)
        {
            //Arrange
            var testObject = new Calculator(verifier, converter);

            //Act
            Action act = () => testObject.GetAuxillaryList(input);

            //Assert
            Assert.Throws<ArgumentException>(act);
        }
    }
}