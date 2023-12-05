namespace TestHarness;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.IO;

class Program
{
    const string informationTypeAttribute="UTILITYFUNCTIONS.INFORMATIONATTRIBUTE";
    static void Main(string[] args)
    {
        const string targetAssemblyFileName = "UtilityFunctions.dll";
        const string targetNameSpace = "UtilityFunctions";

        Assembly assembly=Assembly.LoadFile(Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            targetAssemblyFileName));
        
        List<System.Type> classes=assembly
            .GetTypes()
            .Where(t => t.Namespace == targetNameSpace && HasInformationAttribute(t)).ToList();
        
        WritePromptToScreen("Please press number key associated with the class wish to test");

        DisplayProgramElementList(classes);
        
        Type typeChoice = ReturnProgramElementReferenceFromList(classes);
       
        object classInstance = Activator.CreateInstance(typeChoice, null);
        Console.Clear();
        
        WriteHeadingToScreen($"Class: '{typeChoice}'");

        DisplayElementDescription(ReturnInformationCustomAttributeDescription(typeChoice));
        
        WritePromptToScreen("Please enter the number associated with the method you wish to test");

        List<MethodInfo> methods = typeChoice.GetMethods().Where(m => HasInformationAttribute(m)).ToList();
        
        DisplayProgramElementList(methods);

        MethodInfo methodChoice = ReturnProgramElementReferenceFromList(methods);

        if (methodChoice != null)
        {
            Console.Clear();
            WriteHeadingToScreen($"Class: '{typeChoice}' - Method: '{methodChoice.Name}'");
            DisplayElementDescription(ReturnInformationCustomAttributeDescription(methodChoice));
            ParameterInfo[] parameters = methodChoice.GetParameters();

            object result = GetResult(classInstance, methodChoice, parameters);
            
            WriteResultToScreen(result);
        }
    }

    private static bool HasInformationAttribute(MemberInfo memberInfo)
    {
        foreach (var attribute in memberInfo.GetCustomAttributes())
        {
            Type typeAttrib = attribute.GetType();
            if (typeAttrib.ToString().ToUpper().Equals(informationTypeAttribute))
            {
                return true;
            }
        }

        return false;
    }
    private static void WriteResultToScreen(object result)
    {
        Console.WriteLine();
        Console.BackgroundColor = ConsoleColor.DarkGreen;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"Result:{result}");
        Console.ResetColor();
        Console.WriteLine();
    }

    private static string ReturnInformationCustomAttributeDescription(MemberInfo miMemberInfo)
    {
        const string informationAttributeDescriptionPropertyName = "Description";
        foreach (var attribute in miMemberInfo.GetCustomAttributes())
        {
            Type typeAttribute=attribute.GetType();
            if (typeAttribute.ToString().ToUpper().Equals(informationTypeAttribute))
            {
                PropertyInfo propertyInfo = typeAttribute.GetProperty(informationAttributeDescriptionPropertyName);
                if (propertyInfo is not null)
                {
                    object s = propertyInfo.GetValue(attribute,null);
                    if (s is not null)
                        return s.ToString();
                }
            }
        }
        
        return default;
    }

    private static void DisplayElementDescription(string elementDescription)
    {
        if (elementDescription is not null)
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(elementDescription);
            Console.ResetColor();
            Console.WriteLine();
        }
    }
    private static object[] ReturnParameterValueInputAsObjectArray(ParameterInfo[]parameters)
    {
        object[] paramValues = new object[parameters.Length];
        int itemCount = 0;

        foreach (ParameterInfo parameterInfo in parameters)
        {
            WritePromptToScreen($"Please enter a value for the parameter named, '{parameterInfo.Name}'");
            
            if (parameterInfo.ParameterType == typeof(string))
            {
                string inputString = Console.ReadLine();
                paramValues[itemCount] = inputString;
            }
            else if(parameterInfo.ParameterType==typeof(int))
            {
                int inputInt= Int32.Parse(Console.ReadLine());
                paramValues[itemCount] = inputInt;
            }
            else if(parameterInfo.ParameterType==typeof(Double))
            {
                double inputInt= Double.Parse(Console.ReadLine());
                paramValues[itemCount] = inputInt;
            }
            itemCount++;
        }

        return paramValues;
    }
    private static object GetResult(object classInstance,MethodInfo methodInfo,ParameterInfo[]parameters)
    {
        object result = null;
        if (parameters.Length == 0)
        {
            result = methodInfo.Invoke(classInstance, null);
        }
        else
        {
            var paramValueArray = ReturnParameterValueInputAsObjectArray(parameters);
            result=methodInfo.Invoke(classInstance, paramValueArray);
        }

        return result;
    }
    

    private static T ReturnProgramElementReferenceFromList<T>(List<T> list)
    {
        ConsoleKey key = Console.ReadKey().Key;
        switch (key)
        {
            case ConsoleKey.D1:
                return list[0];

            case  ConsoleKey.D2:
                return list[1];

            case ConsoleKey.D3:
                return list[2];

            case  ConsoleKey.D4:
                return list[3];
        }

        return default;
    }
    private static void DisplayProgramElementList<T>(List<T> list)
    {
        int count = 0;

        foreach (var type in list)
        {
            WritePromptToScreen($"{++count}. {type}");
        }
    }
    private static void WritePromptToScreen(string message)
    {
        Console.WriteLine(message);
    }

    private static void WriteHeadingToScreen(string heading)
    {
        Console.WriteLine(heading);
        Console.WriteLine(new string('-',heading.Length));
        Console.WriteLine();
    }
}
