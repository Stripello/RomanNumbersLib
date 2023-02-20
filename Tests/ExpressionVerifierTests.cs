namespace Tests
{
    public class ExpressionVerifierTests
    {
        [Theory]
        [InlineData("(I)", true)]
        [InlineData("I2", false)]
        [InlineData("", false)]
        [InlineData(" ", true)]
        [InlineData(null, false)]
        [InlineData("MA", false)]
        [InlineData("MMMMMMMXXXXXX+-", true)]
        [InlineData(".", false)]
        public void ExpressionVerifier_CorrectInput_Succed(string incomingExpression, bool expected)
        {
            //Arrange
            var testObject = new ExpressionVerifier();

            //Act
            var actual = testObject.VerifySymbols(incomingExpression);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("(I))", false)]
        [InlineData("(I2)", true)]
        [InlineData(")(", false)]
        [InlineData("MA", true)]
        [InlineData("(()))(", false)]
        public void BracketVerifier_CorrectInput_Succed(string incomingExpression, bool expected)
        {
            //Arrange
            var testObject = new ExpressionVerifier();

            //Act
            var actual = testObject.VerifyBrackets(incomingExpression);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void VerifyBracketlessAuxList_CorrectInput_Succed(List<(int, bool)> incomingExpression, bool expected)
        {
            //Arrange
            var testObject = new ExpressionVerifier();

            //Act
            var actual = testObject.VerifyBracketlessAuxList(incomingExpression);

            //Assert
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> Data()
        {
            yield return new object[] { new List<(int, bool)> { (0, false), (0, false) }, false };
            yield return new object[] { new List<(int, bool)> { (0, false) }, true };
            yield return new object[] { new List<(int, bool)> { (0, false), (0, false) }, false };
            yield return new object[] { new List<(int, bool)> { (0, true) }, false };
            yield return new object[] { new List<(int, bool)> { (0, false), (0, true) }, false };
            yield return new object[] { new List<(int, bool)> { (0, true), (0, false) }, false };
            yield return new object[] { new List<(int, bool)> { (0, false), (0, true), (0, false) }, true };
            yield return new object[] { new List<(int, bool)> { (0, false), (0, true), (0, true), (0, false) }, false };
        }
    }
}