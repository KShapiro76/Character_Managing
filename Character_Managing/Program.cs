using System;
using System.Reflection.PortableExecutable;

class Program
{
    
    
    
    static void Main(string [] args)
    {
        
        do
        {
            Console.Clear();
            string intro = 
            "   Welcome to the Character Manager." +
            "\nThe purpose of this program is to keep" + 
            "\ntrack of the HP, abilites, and magic items" +  
            "\nof a D&D character that can be changed over" + 
            "\nthe course of a session. You can start by " + 
            "\neither choosing a character to manage " + 
            "\nduring the session, or by adding a new" + 
            "\ncharacter to use." + 
            "\n   The purpose of this program is not " +
            "\nto create a new character from scratch" + 
            "\nor to keep track of all character " +
            "\ndetails. Only the details that can be" + 
            "\naltered over the course of a session" + 
            "\n   Choose a character to use or add" + 
            "\na new one and get started!";

            Console.WriteLine(intro + "\n");
            Console.WriteLine("Please select an option:");
            Console.WriteLine("1. Add a new character");
            Console.WriteLine("2. Choose character for a the current session");
            Console.WriteLine("3. Exit");

            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Console.Clear();
                    Character newCharacter = new Character();
                    newCharacter.CreateCharacter();
                    break;
                case "2":
                    Console.Clear();
                    Session session = new Session();
                    session.SelectCharacter();
                    session.Start();
                    break;
                case "3":
                    Console.WriteLine("Exiting the Character Manager.");
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        } while (true);
    }
}