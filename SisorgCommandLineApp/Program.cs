﻿using SisorgCommandLineApp;
using SisorgCommandLineApp.CommandStrategy;
using System;

internal class Program
{
    private static void Main(string[] args)
    {
        string currentPath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
        CommandLine cmd = new CommandLine(currentPath);
        
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Press Control + C to stop the application.");
            currentPath = cmd.currentPath;
            Console.WriteLine(currentPath);
            string userInput = GetUserInput();
            string[] userInputCommandArgs = userInput.Split();
            switch (userInputCommandArgs[0])
            {
                case "tch":
                    cmd.SetCommandStrategy(new CreateFileCommandStrategy(currentPath));
                    break;
                case "mv":
                    cmd.SetCommandStrategy(new MoveFileCommandStrategy(currentPath));
                    break;
                case "ls":
                    cmd.SetCommandStrategy(new ListCommandStrategy(currentPath));
                    break;
                case "cd":
                    cmd.SetCommandStrategy(new ChangeDirectoryCommandStrategy(cmd));
                    break;
                case "help":
                    cmd.SetCommandStrategy(new HelpCommandStrategy());
                    break;
                default:
                    Console.WriteLine($"\"{userInputCommandArgs[0]}\" is not a valid or known command.\nPress any key to continue.");
                    Console.ReadKey();
                    continue;
            }

            string[] optionsArray = new string[userInputCommandArgs.Length-1];
            Array.Copy(userInputCommandArgs, 1, optionsArray, 0, userInputCommandArgs.Length - 1);

            try 
            { 
                cmd.ExecuteCommand(optionsArray);
            } 
            catch(Exception ex)
            {
                Console.WriteLine($"\nThe following error ocurred: \"{ex.Message}\", So the command was not executed.");
            }

            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
        }
    }

    private static string GetUserInput()
    {
        string userInput;

        try
        {
            userInput = Console.ReadLine();
            if (userInput == null)
            {
                throw new Exception("User input has a null value.");
            }
        }
        catch (Exception ex)
        {
            userInput = "";
            Console.WriteLine($"An error ocurred: {ex.Message}\nPress any key to continue.");
            Console.ReadKey();
        }

        return userInput;
    }
}