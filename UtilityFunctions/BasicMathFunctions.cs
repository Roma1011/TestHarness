namespace UtilityFunctions;

[Information(Description = "This class contain basic Math methods")]
public class BasicMathFunctions
{
    [Information(Description = "This Method divides the first number by secound number and return result")]
    public double DivideOperation(double numbe1, double number2)
    {
        return numbe1 / number2;
    }
    [Information(Description = "This Method Multiply the first number by secound number and return result")]
    public double MultiplyOperation(double numbe1, double number2)
    {
        return numbe1 * number2;
    }
}