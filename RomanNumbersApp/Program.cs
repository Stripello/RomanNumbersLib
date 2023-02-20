using Model;
using RomanNumbersApp;

var converter = new NumberConverter();
var verifyer = new ExpressionVerifier();
var calculator = new Calculator(verifyer, converter);

var result = calculator.Evaluate("(MMMDCCXXIV - MMCCXXIX) * II");
Console.WriteLine(result);
