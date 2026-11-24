using System.ComponentModel;
public class Session : BasicForm
{
    private Character _currentCharacter;
    public Session()
    {
        _currentCharacter = new Character();
    }
    public void SelectCharacter()//Finds all character files in the Characters folder and allows the user to select one by typing in the name.
    {
        string projectFolder = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../.."));
        string FilePath = Path.Combine(projectFolder, "Character_Managing" ,"Characters");
        Directory.CreateDirectory(FilePath); // Ensure the directory exists
        foreach (string file in Directory.GetFiles(FilePath, "*.txt"))
        {
            string characterName = Path.GetFileNameWithoutExtension(file);
            Console.WriteLine($"Found character file: {characterName}");
        }
        bool fileExists;
        do
        {
        Console.WriteLine("\nEnter the name of the character you want to load:");
        string[] characterNameInput = [Console.ReadLine()];
        if (File.Exists(FilePath + "/" + characterNameInput[0] + ".txt"))
        {
            Character loadedCharacter = new Character();
            loadedCharacter.LoadFromFile(characterNameInput);
            _currentCharacter = loadedCharacter;
            fileExists = true;
        }
        else
        {
            Console.WriteLine("Invalid filename. Please try again.");
            fileExists = false;
        }
        } while (!fileExists); //The loop will continue trying to get a file name input until a file with that name is found
    }
    private void MakeCharacterAdustment() //Uses a menu to allow the user to select an option to change manually about the current character
    {
        Console.Clear();
        Console.WriteLine(_currentCharacter.GetSummary() + "\n");
        Console.WriteLine("Select the attribute you'd like to adjust:");
        Console.WriteLine("1.   Character Name");
        Console.WriteLine("2.   Character Description");
        Console.WriteLine("3.   Base Max HP");
        Console.WriteLine("4.   Actual Max HP");
        Console.WriteLine("5.   Temporary HP");
        Console.WriteLine("6.   Current HP");
        Console.WriteLine("7.   Spell Slots");
        Console.WriteLine("8.   Hit Dice");
        Console.WriteLine("9.   Abilities");
        Console.WriteLine("10.  Exit");

        string input = Console.ReadLine();
        switch (input)
        {
            case "1":
                _currentCharacter.UpdateName();
                break;
            case "2":
                _currentCharacter.UpdateDescription();
                break;
            case "3":
                _currentCharacter.UpdateBaseMaxHP();
                break;
            case "4":
                _currentCharacter.UpdateActualMaxHP();
                break;
            case "5":
                _currentCharacter.UpdateTempHP();
                break;
            case "6":
                _currentCharacter.UpdateCurrentHP();
                break;
            case "7":
                _currentCharacter.UpdateSpellList();
                break;
            case "8":
                _currentCharacter.UpdateHitDice();
                break;
            case "9":
                _currentCharacter.UpdateAbilitiesList();
                break;
            case "10":
                break;
            default:
                Console.WriteLine("Invalid Option. Please Try Again");
                return;
        }
    }
//
    public void Start()
    {
        Console.Clear();
        Console.WriteLine($"Starting session for character:");
        StartAnimation();
        do
        {
            Console.Clear();
            Console.WriteLine( _currentCharacter.GetSummary());
            Console.WriteLine("\nSession Menu:");
            Console.WriteLine("1.   Apply Damage Taken");
            Console.WriteLine("2.   Apply Healing");
            Console.WriteLine("3.   Apply Temporary HP");
            Console.WriteLine("4.   Use Spell Slot");
            Console.WriteLine("5.   Use Ability");
            Console.WriteLine("6.   Perform Long Rest");
            Console.WriteLine("7.   Perform Short Rest");
            Console.WriteLine("8.   Pass Dawn (Recharge Abilities)");
            Console.WriteLine("9.   Make Character Adustment");
            Console.WriteLine("10.  Save");
            Console.WriteLine("11.  Save and Exit Session");
            Console.WriteLine("12.  Exit without Saving");



            string input = Console.ReadLine();
            Console.Clear();

            switch (input)
            {
                case "1":
                    Console.WriteLine(_currentCharacter.GetSummary());
                    _currentCharacter.ApplyDamage();
                    break;
                case "2":
                    Console.WriteLine(_currentCharacter.GetSummary());
                    _currentCharacter.ApplyHealing();
                    break;
                case "3":
                    Console.WriteLine(_currentCharacter.GetSummary());
                    _currentCharacter.ApplyTemporaryHP();
                    break;
                case "4":
                    _currentCharacter.UseSpellSlot();
                    break;
                case "5":
                    _currentCharacter.UseAbility();
                    break;
                case "6":
                    Console.WriteLine(_currentCharacter.GetSummary());
                    _currentCharacter.PerformRest("Long Rest");
                    break;
                case "7":
                    Console.WriteLine(_currentCharacter.GetSummary());
                    _currentCharacter.PerformRest("Short or Long Rest");
                    break;
                case "8":
                    Console.WriteLine(_currentCharacter.GetSummary());
                    _currentCharacter.PassDawn();
                    break;
                case "9":
                    Console.WriteLine(_currentCharacter.GetSummary());
                    MakeCharacterAdustment();
                    break;
                case "10":
                    Console.WriteLine("Saving...");
                    StartAnimation();
                    _currentCharacter.CharactertoFile();
                    break;
                case "11":
                    Console.WriteLine("Saving and Exiting session.");
                    StartAnimation();
                    _currentCharacter.CharactertoFile();
                    return;
                case "12":
                    Console.WriteLine("Exiting session");
                    StartAnimation();
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        } while (true);
    }
}