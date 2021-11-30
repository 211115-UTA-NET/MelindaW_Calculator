using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SimpleCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Calculator.");
            Console.WriteLine("Accpets written values -999 to 999 (ie. negative one)");
            Console.WriteLine("Enter 'Q' to quit.");

            // call the calculator method
            Calculator();

            Console.WriteLine("Exiting Calculator.");
        }
        public static void Calculator()
        {
            bool isRunning = true;
            while (isRunning)
            {
                // Create a List to store each number and operator
                List<string> problem = new List<string>();
                // Create a sting to hold the input
                string? input;
                string soultion = "";
                // Ask user for input (file path or math problem)
                Console.WriteLine("Enter your math problem or the file path "
                    + "to a text file with math problems.");
                // read input
                input = Console.ReadLine()?.ToLower();
                // pattern to match .txt file using a regular expration
                string pattern = @"\.txt$";
                Regex txtRegex = new Regex(pattern);
                Match txtMatch = txtRegex.Match(input);
                bool isTxtFile = txtMatch.Success;
                // Check if the user entered in a txt file
                if (input == "")
                {
                    Calculator();
                    break;
                }
                else if (input == "q")
                {
                    isRunning = false;
                    break;
                }
                else if (isTxtFile)
                {
                    ReadTextFile(input, problem, soultion);
                }
                else
                {
                    // Format input to numbers and operators
                    input = FormatInput(input);
                    // put input into a List
                    problem = ProcessInput(input);
                    string x = "";
                    foreach (string value in problem)
                    {
                        x+=value;
                    }
                    // solve the math problems
                    soultion = SolveProblem(problem);
                    // solve the math problems
                    soultion = x + "=" + SolveProblem(problem);

                    pattern = @"[a-zA-Z]";
                    txtRegex = new Regex(pattern);
                    txtMatch = txtRegex.Match(x);
                    bool isLetters = txtMatch.Success;
                    if ( isLetters)
                    {
                        soultion = x + "=" + "Error";
                    }
                    // print the soultion to the console and file
                    PrintSoultion(soultion);
                }
            }
        }
        public static string ReadTextFile(
            string input, List<string> problem, string soultion)
        {
            try
            {
                StreamReader readFileLine = new StreamReader(input);
                input = readFileLine.ReadLine().ToLower();
                while (input != null)
                {
                    // Format input to numbers and operators
                    input = FormatInput(input);
                    // put input into a List
                    problem = ProcessInput(input);
                    string x = "";
                    foreach (string value in problem)
                    {
                        x+=value;
                    }
                    // solve the math problems
                    soultion = x + "=" + SolveProblem(problem);
                    string pattern = @"[a-zA-Z]";
                    Regex txtRegex = new Regex(pattern);
                    Match txtMatch = txtRegex.Match(x);
                    bool isLetters = txtMatch.Success;
                    if ( isLetters)
                    {
                        soultion = x + "=" + "Error";
                    }
                    // print the soultion to the console and file
                    PrintSoultion(soultion);
                    input = readFileLine.ReadLine();
                }
                readFileLine.Close();
                return input;
            }
            catch (Exception e) 
            {
                Console.WriteLine("Exception: " + e.Message);
                return input;
            }
        }
        public static string FormatInput(string input)
        {
            string[] operators = {"negative","plus","minus","multiply","divide"};
            string[] symbols = {"-","+","-","*","/"};
            string[] digitsArray = {"zero","one","two","three","four","five","six",
                "seven","eight","nine"};
            string[] tenToNineteen = {"ten","eleven","twelve","thirteen","fourteen",
                "fifteen","sixteen","seventeen","eighteen","nineteen"};
            string[] twentyToNinety = {"twenty","thirty","forty","fifty","sixty",
                "seventy","eighty","ninety"};
            string[] onehundredToNinehundred = {"onehundred","twohundred",
                "threehundred","fourhundred","fivehundred","sixhundred",
                "sevenhundred","eighthundred","ninehundred"};
            
            // remove spaces
            input = input.Replace(" ", "");
            // give error if input is not in corret fromat
            // for example one one or one two
            input = IsCorretFormat(input, onehundredToNinehundred);
            input = IsCorretFormat(input, digitsArray);
            input = IsCorretFormat(input, tenToNineteen);
            input = IsCorretFormat(input, twentyToNinety);
            // change written numbers to digits
            // and written operators to symbols
            input = ConvertToSymbols(input, operators, symbols);
            input = ConvertToDigits120To999(
                input, onehundredToNinehundred, twentyToNinety, digitsArray);
            input = ConvertToDigits110To919Skipping120(
                input, onehundredToNinehundred, tenToNineteen);
            input = ConvertToDigits101To909Skipping100s(
                input, onehundredToNinehundred, digitsArray);
            input = ConvertToDigitsBy100s(input, onehundredToNinehundred);
            input = ConvertToDigits21To99(input, twentyToNinety, digitsArray);
            input = ConvertToDigits20To90By10(input, twentyToNinety);
            input = ConvertToDigits10To19(input, tenToNineteen);
            input = ConvertToDigits0To9(input, digitsArray);

            return input;
        }
        public static string IsCorretFormat(string input, string[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = 0; j < array.Length; j++)
                {
                    if (input.Contains(array[i] + array [j]))
                    {
                        input = "Error";
                        break;
                    }
                }
            }
            return input;
        }
        public static string ConvertToSymbols(
            string input, string[] arrayOne, string[] arrayTwo)
        {
            for (int i = 0; i < arrayOne.Length; i++)
            {
                input = input.Replace(arrayOne[i], arrayTwo[i]);
            }
            return input;
        }
        public static string ConvertToDigits0To9(string input, string[] arrayOne)
        {
            for(int i = 0; i < arrayOne.Length; i++)
            {
                input = input.Replace(arrayOne[i], i.ToString());
            }
            return input;
        }
        public static string ConvertToDigits10To19(string input, string[] arrayOne)
        {
            for(int i = 0; i < arrayOne.Length; i++)
            {
                input = input.Replace(arrayOne[i], (i + 10).ToString());
            }
            return input;
        }
        public static string ConvertToDigits20To90By10(
            string input, string[] arrayOne)
            {
                for (int i = 0; i < arrayOne.Length; i++)
                {
                    input = input.Replace(arrayOne[i], (i + 2).ToString() + "0");
                }
                return input;
            }
        public static string ConvertToDigits21To99(
            string input, string[] arrayOne, string[] arrayTwo)
            {
                for (int i = 0; i < arrayOne.Length; i++)
                {
                    for (int j = 1; j < arrayTwo.Length; j++)
                    {
                        input = input.Replace((arrayOne[i] + arrayTwo[j]),
                            (i + 2).ToString() + j.ToString());
                    }
                }
                return input;
            }
        public static string ConvertToDigits101To909Skipping100s(
            string input, string[] arrayOne, string[] arrayTwo)
        {
            for (int i = 0; i < arrayOne.Length; i++)
            {
                for (int j = 1; j < arrayTwo.Length; j++)
                {
                    input = input.Replace(arrayOne[i] + arrayTwo[j],
                        (i + 1).ToString() + "0" + j.ToString());
                }
            }
            return input;
        }
        public static string ConvertToDigitsBy100s(string input, string[] arrayOne)
        {
            for (int i = 0; i < arrayOne.Length; i++)
            {
                input = input.Replace(arrayOne[i], (i + 1).ToString() + "00");
            }
            return input;
        }
        public static string ConvertToDigits110To919Skipping120(
            string input, string[] arrayOne, string[] arrayTwo)
        {
            for (int i = 0; i < arrayOne.Length; i++)
            {
                for (int j = 0; j < arrayTwo.Length; j++)
                {
                    input = input.Replace(arrayOne[i] + arrayTwo[j],
                        (i + 1).ToString() + "1" + j.ToString());
                }
            }
            return input;
        }
        public static string ConvertToDigits120To999(
            string input, string[] arrayOne, string[] arrayTwo, string[] arrayThree)
        {
            for (int i = 0; i < arrayOne.Length; i++)
            {
                for (int j = 0; j < arrayTwo.Length; j++)
                {
                    for (int k = 1; k < arrayThree.Length; k++)
                    {
                        input = input.Replace(arrayOne[i] + arrayTwo[j] + arrayThree[k],
                            (i + 1).ToString() + (j + 2).ToString() + (k).ToString());
                    }
                }
            }
            return input;
        }
        public static List<string> ProcessInput(string input)
        {
            List<string> problem = new List<string>();
            string var = "";
            for (int i = 0; i < input.Length; i++)
            {
                int integer = 0;
                bool isInt = int.TryParse(input[i].ToString(), out integer);
                if(isInt || input[i] == '.')
                {
                    var += input[i];
                }
                else
                {
                    if(var != "")
                    {
                        problem.Add(var);
                    }
                var = input[i].ToString();
                problem.Add(var);
                var = "";
                }
                if(i == (input.Length - 1))
                {
                    problem.Add(var);
                }
            }
            return problem;
        }
        public static string SolveProblem(List<string> problem) {
            for (int i = 0; i < problem.Count; i++)
            {
                string value = problem[i];
                double a = 0;
                double b = 0;
                if (problem.Contains("*") || problem.Contains("/"))
                {
                    try
                    {
                        switch (value)
                        {
                            case "*":
                                a = Double.Parse(problem[i - 1]);
                                b = Double.Parse(problem[i + 1]);
                                double multiple = a * b;
                                problem[i + 1] = multiple.ToString();
                                problem.RemoveRange((i - 1), 2);
                                return SolveProblem(problem);
                            case "/":
                                a = Double.Parse(problem[i - 1]);
                                b = Double.Parse(problem[i + 1]);
                                double division = a / b;
                                problem[i + 1] = division.ToString();
                                problem.RemoveRange((i - 1), 2);
                                return SolveProblem(problem);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception:" + e.Message);
                        problem.Clear();
                        problem.Add("Error");
                    }
                }
                if (!problem.Contains("*") && !problem.Contains("/"))
                {
                    try
                    {
                        if(problem[0] == "-" || problem[0] == "+")
                        {
                            problem[1] = problem[0] + problem[1];
                            problem.RemoveRange(0, 1);
                            return SolveProblem(problem);
                        }
                        switch (value)
                        {
                            case "+":
                                a = Double.Parse(problem[i - 1]);
                                b = Double.Parse(problem[i + 1]);
                                double sum = a + b;
                                problem[i + 1] = sum.ToString();
                                problem.RemoveRange((i - 1), 2);
                                return SolveProblem(problem);
                            case "-":
                                a = Double.Parse(problem[i - 1]);
                                b = Double.Parse(problem[i + 1]);
                                double diff = a - b;
                                problem[i + 1] = diff.ToString();
                                problem.RemoveRange((i - 1), 2);
                                return SolveProblem(problem);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception: " + e.Message);
                        problem.Clear();
                        problem.Add("Error");
                    }
                } 
            }
            string resultString;
            double num;
            bool isDouble = Double.TryParse(problem[0], out num);
            if (isDouble)
            {
                resultString = problem[0];
            }
            else
            {
                problem[0] = "Error";
                resultString = problem[0];
            }
            return resultString;
        }
        public static void PrintSoultion(string soultion)
        {
            Console.WriteLine(soultion);
            try
            {
                string path = "./MathData/Write_Math.txt";
                StreamWriter writeToFile = new StreamWriter(path, true);
                writeToFile.WriteLine(soultion);
                writeToFile.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }
    }
}