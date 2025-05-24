using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.IO;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

//Make goal,
//input the money until goal is met
//if the money exceeds goal, it will added up and labelled as exceeded money
//if goal has not been met, goal labelled as fail

/*
  FOR EVERY GOAL:
    - Start date
    - End date
    - Goal Amount
    - Start Amount
    - Current Amout
    - Num of entries?
External Storage: TXT FILE: 

BONUSE FEATURES TO ADD(FUTURE):
    - Calender(Time Period of goal)
    - CountDown
    - SQL database Records
    - OPTION to stay signed in- Check TXT file for stay signed in option at start and present relevant information
 */

//ASCII ART : https://ascii.co.uk/art
//TEXT TO ASCII : https://patorjk.com/software/taag/#p=display&f=Graffiti&t=Type%20Something%204
//CUTE SYMBOLS: https://emojicombos.com/cute-symbols
class Goal
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; } 
    public double GoalAmount { get; set; }

    public long StartAmount { get; set; }
    public long CurrentAmount { get; set; }
    public string GoalTitle { get; set; }
    public List<(double Amount, string Source)> RecentEarnings { get; set; } = new List<(double,string)>();
}
class Program
{
    public List<Goal> data = new List<Goal>();
    public const char exit = 'X';
    public const string construction = "FEATURE IS CURRENTLY UNDERGOING CONSTRUCTION";

    public static void DisplayWelcomeMenu()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;//THIS WILL ENSURE THE ART SHOWS

        string font = "✩°｡ ୨୧ Cute and fun program to track and reach money goals ୨୧ ｡°✩";
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($@"
    ╭───────────────────────────────────────────────────────────────────────────────────────────────────────────.★..─╮
          ⢠⡾⠲⠶⣤⣀⣠⣤⣤⣤⡿⠛⠿⡴⠾⠛⢻⡆⠀⠀⠀           ___ ___   ___   ____     ___  __ __      ____    ____  ____   __  _ 
    ⠀⠀   ⠀⣼⠁⠀⠀⠀⠉⠁⠀⢀⣿⠐⡿⣿⠿⣶⣤⣤⣷⡀⠀⠀          |   |   | /   \ |    \   /  _]|  |  |    |    \  /    ||    \ |  |/ ]
    ⠀⠀   ⠀⢹⡶⠀⠀⠀⠀⠀⠀⠈⢯⣡⣿⣿⣀⣸⣿⣦⢓⡟⠀⠀          | _   _ ||     ||  _  | /  [_ |  |  |    |  o  )|  o  ||  _  ||  ' /   
    ⠀⠀   ⢀⡿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠉⠹⣍⣭⣾⠁⠀⠀          |  \_/  ||  O  ||  |  ||    _]|  ~  |    |     ||     ||  |  ||    \
       ⠀⣀⣸⣇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣸⣷⣤⡀          |   |   ||     ||  |  ||   [_ |___, |    |  O  ||  _  ||  |  ||     \
       ⠈⠉⠹⣏⡁⠀⢸⣿⠀⠀⠀⢀⡀⠀⠀⠀⣿⠆⠀⢀⣸⣇⣀⠀          |   |   ||     ||  |  ||     ||     |    |     ||  |  ||  |  ||  .  |
    ⠀   ⠐⠋⢻⣅⣄⢀⣀⣀⡀⠀⠯⠽⠂⢀⣀⣀⡀⠀⣤⣿⠀⠉⠀          |___|___| \___/ |__|__||_____||____/     |_____||__|__||__|__||__|\_|  
    ⠀   ⠀⠴⠛⠙⣳⠋⠉⠉⠙⣆⠀⠀⢰⡟⠉⠈⠙⢷⠟⠉⠙⠂⠀           {font}
            ⢻⣄⣠⣤⣴⠟⠛⠛⠛⢧⣤⣤⣀⡾                                                     ⠀⠀⠀⠀                                  
    ╰─..★.───────────────────────────────────────────────────────────────────────────────────────────────────────────╯
");
        Console.ResetColor();
    }

    public static void CreateGoal()
    {
        Console.Write("\nEnter goal amount: \n > ");
        string goalAmount = Console.ReadLine();
        Console.Write("Enter current amount: \n >  ");
        double currentAmount = Convert.ToInt32(Console.ReadLine());

        double goalamount = Convert.ToDouble(goalAmount);
    }

    public static void LogIn(Goal goal)
    {
        while (true)
        {
            Console.Write("Enter Username: ");
            string username = Console.ReadLine();
            if (string.Equals(username, exit.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                Main();
                return;
            }

            Console.Write("Enter Password: ");
            string password = Console.ReadLine();
            if (string.Equals(password, exit.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                Main();
                return;
            }

            if (VerifyAccount(username, password, goal))
            {
                Console.WriteLine($"Welcome Back {goal.UserName}");
                Console.WriteLine($"Your goal title is: {goal.GoalTitle}");
                Console.WriteLine($"Goal Amount: R{goal.GoalAmount}");
                Console.WriteLine($"Current Amount: R{goal.CurrentAmount}");
                
                Console.WriteLine("Would you like to add new income? (Y/N)");
                char option = Console.ReadKey().KeyChar;
                Console.WriteLine();

                if (char.ToLower(option) == 'y')
                {
                    AddNewIncome(goal);
                }
                else if (char.ToLower(option) == 'n')
                {
                    Console.WriteLine("View current Income Details? (Y/N)");
                    char viewIncomes = Convert.ToChar(Console.ReadKey().KeyChar);
                    if (char.ToLower(viewIncomes) == 'y')
                    {
                        DisplayCurrentDetails(goal);
                    }
                    else if (char.ToLower(viewIncomes) == 'n')
                    {
                        Main();
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid username or password.");
            }
        }
    }

    public static void CreateAccount()
    {
        Console.WriteLine(construction);
        Main();
    }

    public static void DisplayCurrentDetails(Goal goal)
    {
        Console.WriteLine($"\nGoal Title: {goal.GoalTitle}");
        Console.WriteLine($"Goal Amount:R {goal.GoalAmount}");
        Console.WriteLine($"Start Amount:R {goal.StartAmount}");
        Console.WriteLine($"Current Amount:R {goal.CurrentAmount}");

        if (goal.RecentEarnings.Count == 0)
        {
            Console.WriteLine("No Recent Earnings");
        }
        else 
        {
            foreach (var earning in goal.RecentEarnings)
            {
                Console.WriteLine($" > {earning.Amount} ({earning.Source})\n");
            }
        }
        
    }

    public static void AddNewIncome(Goal goal)
    {
        while (true)
        {
            Console.Write("Enter new income amount (or 'X' to return to main menu): ");
            string input = Console.ReadLine().Trim();

            if (string.Equals(input, exit.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                Main();
                return;
            }

            if (double.TryParse(input, out double amount))
            {
                Console.Write("Enter income source (or 'X' to return to main menu): ");
                string source = Console.ReadLine().Trim();

                if (string.Equals(source, exit.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    Main();
                    return;
                }

                goal.CurrentAmount += (long)amount;
                goal.RecentEarnings.Add((amount, source));

                SaveToTxtFile(goal, @"C:\Visual Studio IDE\Side Projects\MoneySaveDraft1\GoalDat.txt.txt");
                Console.WriteLine("✓ Income added and saved successfully!");

                // Optionally, ask if user wants to add another income
                Console.Write("Add another income? (Y/N): ");
                string again = Console.ReadLine().Trim();
                if (again.Equals("y", StringComparison.OrdinalIgnoreCase))
                    continue;
                else
                {
                    Main();
                    return;
                }
            }
            else
            {
                Console.WriteLine("Invalid amount format. Use numbers only.");
            }
        }
    }

    public static Goal ReadFromTxtFile(string path)
    {
        Goal goal = new Goal();
        bool readingEarnings = false;

        try
        {
            using (StreamReader sr = new StreamReader(path))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (string.IsNullOrEmpty(line))
                    {
                        readingEarnings = false;
                        continue;
                    }

                    if (readingEarnings)
                    {
                        if (line.StartsWith(">"))
                        {
                            var parts = line.Substring(1).Split(new[] { '(' }, 2);
                            if (parts.Length == 2)
                            {
                                double amount = double.Parse(parts[0].Trim());
                                string source = parts[1].TrimEnd(')').Trim();
                                goal.RecentEarnings.Add((amount, source));
                            }
                        }
                        continue;
                    }

                    if (line.StartsWith("Username:")) goal.UserName = line.Split(':')[1].Trim();
                    else if (line.StartsWith("Password:")) goal.Password = line.Split(':')[1].Trim();
                    else if (line.StartsWith("Goal Title:")) goal.GoalTitle = line.Split(':')[1].Trim();
                    else if (line.StartsWith("Goal Amount:"))
                        goal.GoalAmount = double.Parse(Regex.Replace(line.Split(':')[1], @"\s", "").Replace("R", ""));
                    else if (line.StartsWith("Start Amount:"))
                        goal.StartAmount = long.Parse(Regex.Replace(line.Split(':')[1], @"\s+", "").Replace("R", ""));
                    else if (line.StartsWith("Current Amount:"))
                        goal.CurrentAmount = long.Parse(Regex.Replace(line.Split(':')[1], @"\s+", "").Replace("R", "")
);
                    else if (line.StartsWith("Recent Earnings:"))
                        readingEarnings = true;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading file: " + ex.Message);
        }

        return goal;
    }

    public static bool VerifyAccount(string username, string password, Goal goal)
    {
        return goal.UserName.Equals(username, StringComparison.OrdinalIgnoreCase) && goal.Password == password;
    }

    public static void UpdateCurrentAmount(Goal goal)
    {
        Console.Write("Enter new income amount to add: ");
        if (long.TryParse(Console.ReadLine(), out long newIncome))
        {
            goal.CurrentAmount += newIncome;
            Console.WriteLine($"Updated Current Amount: R{goal.CurrentAmount}");
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid number.");
        }
    }
    public static void SaveToTxtFile(Goal goal, string path)
    {
        try
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine($"Username: {goal.UserName}");
                sw.WriteLine($"Password: {goal.Password}");
                sw.WriteLine();
                sw.WriteLine($"Goal Title: {goal.GoalTitle}");
                sw.WriteLine($"Goal Amount: R{goal.GoalAmount:N0}");
                sw.WriteLine($"Start Amount: R{goal.StartAmount:N0}");
                sw.WriteLine($"Current Amount: R{goal.CurrentAmount:N0}");
                sw.WriteLine();
                sw.WriteLine("Recent Earnings:");
                foreach (var earning in goal.RecentEarnings)
                {
                    sw.WriteLine($" > {earning.Amount} ({earning.Source})");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error saving file: " + ex.Message);
        }
    }

    public static void Main()
    {
        string path = @"C:\Visual Studio IDE\Side Projects\MoneySaveDraft1\GoalDat.txt.txt";
        DisplayWelcomeMenu();

        Goal userGoal = ReadFromTxtFile(path);

        Console.WriteLine("Choose the following option(1/2)\n 1.Log In\n 2.Create Account\nNOTE Enter 'X' to return to main menu");
        int loginOpt = Convert.ToInt32(Console.ReadLine());

        switch (loginOpt)
        {
            case 1: LogIn(userGoal);
                break;
            case 2: CreateAccount();
                break;
            default: Console.WriteLine("Invalid Option. Select 1 or 2");
                break;
        }
        
    }
    
}
