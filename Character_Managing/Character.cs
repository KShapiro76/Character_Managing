using System.Runtime.CompilerServices;
public class Character : BasicForm
{
    private string _name;
    private string _description;
    private int _baseMaxHP;
    private int _actualMaxHP;
    private int _temporaryHP;
    private int _currentHP;
    private SpellSlots _spellSlots;
    private HitDieList _hitDies;
    private AbilitiesList _abilities;
    public Character()
    {
        // Default constructor
        _name = "Character Name";
        _description = "Character Description";
        _baseMaxHP = 0;
        _actualMaxHP = 0;
        _temporaryHP = 0;
        _currentHP = 0;
        _spellSlots = new SpellSlots(false); // Default to no spell slots
        _hitDies = new HitDieList();
        _abilities = new AbilitiesList();
        
    }
    /*
    CharactertoFile() - Saves the character's information to a text file.
    LoadCharacterFromFile(string CharacterName) - Loads character information from a text file based on the provided character name.
    CreateCharacter() - Prompts the user to input character information and creates a new character object

    */
    public void CharactertoFile()
    {
        // Implement logic to convert character data to a string representation and save it to a text file

        string projectFolder = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../.."));
        string filePath = Path.Combine(projectFolder, "Character_Managing" ,"Characters", $"{_name}.txt");
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)); // Ensure the directory exists
        File.WriteAllText(filePath, GetSummary(true));
    }
    public override void LoadFromFile(string[] CharacterName)
    {
        string projectFolder = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../.."));
        string filePath = Path.Combine(projectFolder, "Character_Managing" ,"Characters", $"{CharacterName[0]}.txt");
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Character file '{CharacterName[0]}.txt' not found.");
            StartAnimation();
        }
        string[] lines = File.ReadAllLines(filePath);
        if (lines.Length < 1)
        {
            Console.WriteLine($"Character file '{CharacterName[0]}.txt' is empty or invalid.");
            StartAnimation();
        }
        else
        {
            
            _name = lines[0].Trim()[6..];
            _description = lines[1].Trim()[13..];
            _baseMaxHP = int.Parse(lines[2].Trim()[13..]);
            _actualMaxHP = int.Parse(lines[3].Trim()[15..]);
            _temporaryHP = int.Parse(lines[4].Trim()[13..]);
            _currentHP = int.Parse(lines[5].Trim()[11..]);

            _spellSlots.LoadFromFile(lines[6..]);
            _hitDies.LoadFromFile(lines[6..]);
            _abilities.LoadFromFile(lines[6..]);

        }
    }
    public void CreateCharacter()
    {
        Console.WriteLine("Creating a new character");
        Console.WriteLine("Enter character name:");
        _name = Console.ReadLine();
        Console.WriteLine("Enter character description (Species, Subspecies, Class, and Subclass):");
        _description = Console.ReadLine();
        Console.WriteLine("Enter base maximum hit points:");
        _baseMaxHP = GetIntInput();
        Console.WriteLine("Enter current maximum hit points:");
        _actualMaxHP = GetIntInput();
        Console.WriteLine("Enter temporary hit points (or 0 if none):");
        _temporaryHP = GetIntInput();
        Console.WriteLine("Enter current hit points:");
        _currentHP = GetIntInput();
        _spellSlots.CreateSpellSlots();
        _hitDies.CreateHitDieList();
        _abilities.CreateAbilitiesList();
        CharactertoFile(); // Save the character to a file after creation

    }
    public void AddHealing(int healing)
    {
        _currentHP += healing;

        // Ensure current HP does not exceed actual max HP
        if (_currentHP > _actualMaxHP)
        {
            _currentHP = _actualMaxHP;
        }
    }
    public string GetSummary(bool SavetoFile = false)
    {
        string CharacterBaseInfo;
        if (SavetoFile)
        {
            CharacterBaseInfo =
            $"Name: {_name}" + 
            $"\nDescription: {_description}" +
            $"\nBase Max HP: {_baseMaxHP}" +
            $"\nActual Max HP: {_actualMaxHP}" +
            $"\nTemporary HP: {_temporaryHP}" +
            $"\nCurrent HP: {_currentHP}";
        }
        else
        {
            CharacterBaseInfo = 
            $"Name: \u001b[34m{_name}\u001b[0m" + 
            $"\nDescription: \u001b[32m{_description}\u001b[0m" +
            $"\nBase Max HP: {_baseMaxHP}" +
            $"\nActual Max HP: \u001b[31m{_actualMaxHP}\u001b[0m" +
            $"\nTemporary HP: \u001b[31m{_temporaryHP}\u001b[0m" +
            $"\nCurrent HP: \u001b[31m{_currentHP}\u001b[0m";
        }
        
        
        CharacterBaseInfo += _spellSlots.GetSpellSlotsInfo(SavetoFile);
        CharacterBaseInfo += _hitDies.GetHitDiceInfo(SavetoFile);
        CharacterBaseInfo += _abilities.GetAbilitiesInfo(SavetoFile);
        return CharacterBaseInfo;
    }
    public void ApplyDamage()
    {
        Console.WriteLine("Enter the amount of damage taken:");
        int damage = GetIntInput();
        if (damage <= 0)
        {
            Console.WriteLine("Damage must be a positive value.");
            return;
        }

        // Apply damage to temporary HP first
        if (_temporaryHP > 0)
        {
            if (damage <= _temporaryHP)
            {
                _temporaryHP -= damage;
                damage = 0;
            }
            else
            {
                damage -= _temporaryHP;
                _temporaryHP = 0;
            }
        }

        // Apply remaining damage to current HP
        if (damage > 0)
        {
            _currentHP -= damage;
            if (_currentHP < 0)
            {
                _currentHP = 0; // Ensure current HP doesn't go below zero
            }
        }
    }
    public void ApplyHealing()
    {
        Console.WriteLine("Enter the amount of healing:");
        int healing = GetIntInput();
        if (healing <= 0)
        {
            Console.WriteLine("Healing must be a positive value.");
            return;
        }

        _currentHP += healing;

        // Ensure current HP does not exceed actual max HP
        if (_currentHP > _actualMaxHP)
        {
            _currentHP = _actualMaxHP;
        }
    }
    public void ApplyTemporaryHP()
    {
        Console.WriteLine("Enter the amount of temporary HP to apply:");
        int tempHPInput = GetIntInput();
        if (tempHPInput <= 0)
        {
            Console.WriteLine("Temporary HP must be a positive value.");
            return;
        }

        // Only apply temporary HP if it's greater than the current temporary HP
        if (tempHPInput > _temporaryHP)
        {
            _temporaryHP = tempHPInput;
        }
    }
    public void UseSpellSlot()
    {
        _spellSlots.UseSpellSlot();
    }
    public void UseAbility()
    {
        if (_abilities.GetAbilitiesList().Count == 0)
        {
            Console.WriteLine("No abilities available to use.");
            StartAnimation();
            return;
        }
        else
        {
            _abilities.UseAbility(_abilities.FindAbilityTo("use"));
        }
    }
    public void PerformRest(string restType)
    {
        if (restType == "Long Rest")
        {
            _currentHP = _actualMaxHP; // Restore current HP to actual max HP
            _temporaryHP = 0; // Clear temporary HP
            _spellSlots.RechargeSpellSlots(); // Recharge spell slots
            _hitDies.RechargeAllHitDie(); // Recharge hit dies
            Console.WriteLine("Are you passing dawn while resting (yes/no)?");
            bool passDawn = Console.ReadLine().Trim().ToLower() == "yes" ? true : false;
            _abilities.RechargeAbilities(restType); // Recharge abilities
            if (passDawn)
            {
                _abilities.RechargeAbilities("Dawn");
            }
        }
        else if (restType == "Short or Long Rest")
        {
            Console.WriteLine("Are you passing dawn while resting?(recharge abilities)?");
            bool passDawn = Console.ReadLine().Trim().ToLower() == "yes" ? true : false;

            bool usingHitDice = true;
            while (usingHitDice)
            {
                int healthtoRestore = _hitDies.UseHitdice();
                if (healthtoRestore > 0)
                {
                    AddHealing(healthtoRestore);
                }
                Console.WriteLine($"{healthtoRestore} HP restored. Current HP: \u001b[31m{_currentHP}\u001b[0m");
                StartAnimation();
                Console.WriteLine("Would you like to use more hit dice? (yes/no)");
                usingHitDice = Console.ReadLine().Trim().ToLower() == "yes" ? true : false;
            }
            _abilities.RechargeAbilities(restType); // Recharge abilities
            if (passDawn)
            {
                _abilities.RechargeAbilities("Dawn");
            }
        }
    }
    public void PassDawn()
    {
        _abilities.RechargeAbilities("Dawn");
    }
    public void UpdateName()
    {
        Console.Write("Enter the new character name: ");
        _name = Console.ReadLine();
        Console.WriteLine();
    }
    public void UpdateDescription()
    {
        Console.WriteLine("Enter the new character description: ");
        _description = Console.ReadLine();
    }
    public void UpdateBaseMaxHP()
    {
        Console.Write("Enter the new base max HP total: ");
        _baseMaxHP = GetIntInput();
        if (_actualMaxHP > _baseMaxHP)
        {
            _actualMaxHP = _baseMaxHP;
        }
        if (_currentHP > _baseMaxHP)
        {
            _currentHP = _baseMaxHP;
        }
        Console.WriteLine();
    }
    public void UpdateActualMaxHP()
    {
        Console.Write("Enter the new actual max HP total: ");
        _actualMaxHP = GetIntInput();
        if (_currentHP > _actualMaxHP)
        {
            _currentHP = _actualMaxHP;
        }
        Console.WriteLine();
    }
    public void UpdateTempHP()
    {
        Console.Write("Enter the new temporary HP total: ");
        _temporaryHP = GetIntInput();
        Console.WriteLine();
    }
    public void UpdateCurrentHP()
    {
        Console.Write("Enter the new current HP total: ");
        _currentHP = GetIntInput();
        Console.WriteLine();
    }
    public void UpdateSpellList()
    {
        _spellSlots.UpdateSpellSlots();
    }
    public void UpdateHitDice()
    {
        _hitDies.UpdateHitDiceList();
    }
    public void UpdateAbilitiesList()
    {
        _abilities.UpdateAbilityList();
    }
}