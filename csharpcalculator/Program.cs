/* BRIEF#1 = MAKE A CALCULATOR WITH C# */

/* Sources used:

- https://learn.microsoft.com/en-us/dotnet/ -> notes/short lessons about making work "( )" and "%" in a calculator)
- https://stackoverflow.com/ -> DataTable.Compute method/explanations, as well as for "( )" and "%", and many other things in the program)
- https://chatgpt.com (DISCLAIMER: NOT used to make the program!) -> used to complement the explanations of the sources above, such as the use of "DataTable.Compute" method, and to get simplify definitions/explanations of many functions I used (helped me to take notes)
- https://www.w3schools.com -> same use as stackoverflow.com

 */

using System;
using System.Data; // added this so DataTable will be usuable (a DataTable is used to stock database in a table  form)

public class Brief1_Calculator
{
    private static string currentInput = "";
    private static string result = "";
    // this part allows the user to type in the console as well as diplaying the result* of the calculation
    // *the result will not be resetted after each calculation if "c" is not pressed

    public static void Main() // main is the base of the calculator, with the loop allowing it to never end as soon as the user is not pressing "escape" (exit the program)
    {
        Console.WriteLine("C# Calculator (brief1"); // simply displays the program title

        while (true) // the calculator loop
        {
            DisplayCalculator(); // this function allows to display the calculator in the console
            ConsoleKeyInfo keyInfo = Console.ReadKey(true); //ConsoleKeyInfo = info sent to the program when a key is pressed, Console.ReadKey = translate this info in the program to execute the input*

            if (keyInfo.Key == ConsoleKey.Escape) // *here for ex: when you push "escape", it sends the info to the program to exit
            {
                Console.Clear();
                break;
            }

            char keyChar = keyInfo.KeyChar; // char keyChar = allows to stock a character (such as "a", "1", etc.. just have to not be a word or a sentence

            if (keyChar == 'e')
            {
                string input = Console.ReadLine(); // string input will read the key that has been pressed and will translate it to the program to execute it in the consnole
                if (input.ToLower() == "exit")
                    break;
            }

            ProcessInput(keyChar); // this function processes the key we entered in the console (for ex: "e" for exit, "c" for clear etc)
        }

        Console.WriteLine("Calculator exited");

    }

    private static void CenterText(string line) // this function allows to center our texts in the console
    {
        int windowWidth = Console.WindowWidth; // it calculate the width of the window, so we can use it afterwards to calculate how much space we need to center our texts
        int paddingLeft = (windowWidth - line.Length) / 2;
        Console.SetCursorPosition(paddingLeft, Console.CursorTop);
        Console.WriteLine(line);
    }

    private static void DisplayScreen() // this function to display both the calcultation and the result, we could have also used "+-------+ / |" as for DisplayCalculator but I wanted it to look different than the screen
    {
        CenterText("\u2554\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2557");
        CenterText($"\u2551 {currentInput,-28}\u2551");
        CenterText($"\u2551 {result,-28}\u2551");
        CenterText("\u255A\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u255D");
    }


    private static void DisplayKeypad() // this function will displays our numpad (1,2,3, etc.. as well as the operators), < was meant to erase the last number inputed but I couldn't find a simple way to make it work
    {
        CenterText("");
        CenterText("c  ( )  %  /");
        CenterText("7   8   9  *");
        CenterText("4   5   6  -");
        CenterText("1   2   3  +");
        CenterText("0   .   <  =");
        CenterText("");
        Console.ForegroundColor = ConsoleColor.Cyan;
        CenterText("'c' = clear, '( )' are usable for multiplication");
        Console.ResetColor();
    }

    private static void DisplayCalculator() // this function will display the title of the program
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Magenta;
        CenterText("+-------------------------+");
        Console.ResetColor();
        CenterText($"|                         |");
        Console.ForegroundColor = ConsoleColor.Magenta;
        CenterText("| C# Calculator (brief#1) |");
        Console.ResetColor();
        CenterText($"|                         |");
        Console.ForegroundColor = ConsoleColor.Magenta;
        CenterText("+-------------------------+");
        Console.ResetColor();

        Console.WriteLine("");
        string message = "\t Press ''";
        message += "\u001b[31mEscape\u001b[0m";
        message += "'' to exit the calculator";

        CenterText(message);
        CenterText("");
        DisplayScreen();
        DisplayKeypad();
    }

    private static void ProcessInput(char keyChar) // this function will process the input of the user
    {
        if (keyChar == 'c')
        {
            Clear();
        }
        else if (keyChar == '=')
        {
            Evaluate();
        }
        else if ("0123456789".Contains(keyChar))
        {
            currentInput += keyChar;
        }
        else if ("+-*/()%".Contains(keyChar) && currentInput.Length > 0)
        {
            currentInput += keyChar;
        }
    }

    private static void Clear() // this function allows us to clear the screen everytime we press "c"
    {
        currentInput = "";
        result = "";
    }

    private static void Evaluate() // this function allows us to evaluate the calculation, it checks if everything is right, if not then displays the message "Invald expression"
    {
        try
        {
            string expression = AddMultiplicationWhenNeeded(currentInput);
            expression = Percentage(expression);

            Console.WriteLine(expression);

            var resultLine = new DataTable().Compute(expression, null);

            result = resultLine.ToString();
            currentInput = result;
        }
        catch
        {
            result = "Invalid expression";
        }
    }

    private static string AddMultiplicationWhenNeeded(string expression) // this function allows the use of "( )" for multiplication (for ex: 9(5+5) = 90)
    {
        for (int i = 0; i < expression.Length -1; i++)
        {
            if (char.IsDigit(expression[i]) && expression[i + 1] == '(')
            {
                expression = expression.Insert(i + 1, "*");
                i++;
            }
            else if (expression[i] == ')' && char.IsDigit(expression[i + 1]))
            {
                expression = expression.Insert(i + 1, "*");
                i++;
            }
        }
        return expression; //return the modified expression so we can use it without the screen being cleared
    }

    private static string Percentage(string expression) // this function allows the use of "%" to get a percentage
    {
        for (int i = 0; i < expression.Length; i++)
        {
            if (expression[i] == '%')
            {
                int start = i - 1;

                while (start >= 0 && char.IsDigit(expression[start])) start--; start++;
                {
                    string number = expression.Substring(start, i - start);
                    expression = expression.Remove(start, i - start + 1);
                    expression = expression.Insert(start, $"({number}/100)");
                    i = start + number.Length + 6;
                }
            }
        }
        return expression;
    }
}

/* Program made by THOMAS Donovan, Github: https://github.com/thomasdnv */