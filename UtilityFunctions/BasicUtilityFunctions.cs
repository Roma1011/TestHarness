namespace UtilityFunctions;

[Information(Description = "This class contain basic utility methods")]
public class BasicUtilityFunctions
{
    [Information(Description = "This method returns a welcome message")]
    public string WtiteWelcomeMessage()
    {
        return "Welcome to 'BasicUtilityFunctions' class";
    }
    [Information(Description = "This method returns a Integer Plus message")]
    public int IntegerPlusInteger(int operand1, int operand2)
    {
        return operand1 + operand2;
    }
    [Information(Description = "This method returns a concatined three string")]
    public string ConcatThreeStrings(string string1,string string2,string string3)
    {
        return string.Concat(string1, string2,string3);
    }
    [Information(Description = "This method returns a string length")]
    public int GetStringLength(string str)
    {
        return str.Length;
    }
}