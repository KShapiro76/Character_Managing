public class SpellSlots : BasicForm
{
    private bool _hasSpellSlots;
    private int[] _maxSpellSlots = new int[9];
    private int[] _currentSpellSlots = new int[9];
    public SpellSlots(bool hasSpellSlots = true)
    {
        _hasSpellSlots = hasSpellSlots;
    }
    public void CreateSpellSlots()
    {
        Console.WriteLine("Does your character have spell slots? (yes/no)");
        bool boolFound = false;
        do
        {
        string boolInput = Console.ReadLine();
        if (boolInput.Trim().ToLower() == "yes" || boolInput.Trim().ToLower() == "y")
        {
            _hasSpellSlots = true;
            boolFound = true;
        }
        else if (boolInput.Trim().ToLower() == "no" || boolInput.Trim().ToLower() == "n")
        {
            _hasSpellSlots = false;
            boolFound = true;
        }
        else
        {
            Console.WriteLine("Invalid input. Please try again.");
        }
        } while (!boolFound);
        if (_hasSpellSlots)
        {
            Console.WriteLine("Enter the maximum number of spell slots for each level (1-9).");
            
            for (int i = 0; i < 9; i++)
            {
                Console.Write($"Level {i + 1} (E.g. '4/4'): ");
                string input = Console.ReadLine();
                if (!input.Contains("/"))
                {
                    Console.WriteLine("Invalid format. Please try again.");
                    i--;
                }
                else
                {
                    string[] SpellSlots = input.Trim().Split("/");
                    if (int.TryParse(SpellSlots[0], out int number) && int.TryParse(SpellSlots[1], out int number2))
                    {
                    _maxSpellSlots[i] = int.Parse(SpellSlots[1]);
                    _currentSpellSlots[i] = int.Parse(SpellSlots[0]);
                    if (_maxSpellSlots[i] == 0)
                    {
                        for (int index = i; index < 9; index++)
                        {
                            _maxSpellSlots[index] = 0;
                            _currentSpellSlots[index] = 0;
                        }
                        return;
                    }
                    }
                    else
                    {
                        Console.WriteLine("Invalid format. Please try again.");
                        i--;
                    }
                }
            }
        }
    }
    public void UpdateSpellSlots()
    {
        Console.WriteLine("Enter the number of spell slots for each level (1-9).");
        
            for (int i = 0; i < 9; i++)
            {
                Console.Write($"Level {i + 1} (E.g. '4/4'): ");
                string input = Console.ReadLine();
                if (!input.Contains("/"))
                {
                    Console.WriteLine("Invalid format. Please try again.");
                    i--;
                }
                else
                {
                    string[] SpellSlots = input.Trim().Split("/");
                    if (int.TryParse(SpellSlots[0], out int number) || int.TryParse(SpellSlots[1], out int number2))
                    {
                    _maxSpellSlots[i] = int.Parse(SpellSlots[1]);
                    _currentSpellSlots[i] = int.Parse(SpellSlots[0]);
                    if (_maxSpellSlots[i] == 0)
                    {
                        for (int index = i; index < 9; index++)
                        {
                            _maxSpellSlots[index] = 0;
                            _currentSpellSlots[index] = 0;
                        }
                        return;
                    }
                    }
                    else
                    {
                        Console.WriteLine("Invalid format. Please try again.");
                        i--;
                    }
                }
            }
    }
    public override void LoadFromFile(string[] lines)
    {
        int spellSlotStartIndex;
        if (lines[0].Trim() == "no spell slots available.")
        {
            _hasSpellSlots = false;
            return;
        }
        else
        {
            _hasSpellSlots = true;
            spellSlotStartIndex = Array.FindIndex(lines, line => line.StartsWith("Spell Slots:"));
            if (spellSlotStartIndex != -1)
            {
                int spellSlotEndIndex = Array.FindIndex(lines, spellSlotStartIndex + 1, line => string.IsNullOrWhiteSpace(line));
                if (spellSlotEndIndex == -1)
                {
                    spellSlotEndIndex = lines.Length;
                }
                for (int i = spellSlotStartIndex + 1; i < spellSlotEndIndex; i++)
                {
                    if (lines[i].StartsWith("Level"))
                    {
                        string[] slotValues = lines[i].Substring(9).Split('/');
                        if (slotValues.Length == 2)
                        {
                            int level = i - spellSlotStartIndex; // Level is determined by the index
                            if (level >= 1 && level <= 9)
                            {
                                _currentSpellSlots[level - 1] = int.Parse(slotValues[0]);
                                _maxSpellSlots[level - 1] = int.Parse(slotValues[1]);
                            }
                        }
                    }
                }
            }

        }
        if (spellSlotStartIndex != -1)
        {
            int spellSlotEndIndex = Array.FindIndex(lines, spellSlotStartIndex + 1, line => string.IsNullOrWhiteSpace(line));
            if (spellSlotEndIndex == -1)
            {
                spellSlotEndIndex = lines.Length;
            }
            for (int i = spellSlotStartIndex + 1; i < spellSlotEndIndex; i++)
            {
                if (lines[i].StartsWith("Level"))
                {
                    string[] slotValues = lines[i].Substring(9).Split('/');
                    if (slotValues.Length == 2)
                    {
                        int level = i - spellSlotStartIndex; // Level is determined by the index
                        if (level >= 1 && level <= 9)
                        {
                            _currentSpellSlots[level - 1] = int.Parse(slotValues[0]);
                            _maxSpellSlots[level - 1] = int.Parse(slotValues[1]);
                        }
                    }
                }
            }
        }
    }
    public string GetSpellSlotsInfo(bool SavetoFile = false)
    {
        string result;
        if (!_hasSpellSlots)
        {
            result = "\nNo spell slots available.";
        }
        else
        {
            result = "\nSpell Slots:\n";
            if (SavetoFile)
            {
                for (int i = 0; i < 9; i++)
                {
                    if (_maxSpellSlots[i] > 0)
                    {
                        result += $"Level {i + 1}: {_currentSpellSlots[i]}/{_maxSpellSlots[i]}\n";
                    }
                }
            }
            else
            {
                for (int i = 0; i < 9; i++)
                {
                    if (_maxSpellSlots[i] > 0)
                    {
                        result += $"Level {i + 1}: \u001b[31m{_currentSpellSlots[i]}/{_maxSpellSlots[i]}\u001b[0m\n";
                    }
                }
            }
        }
        
        return result;
    }
    public void UseSpellSlot()
    {
        if (_maxSpellSlots[0] != 0)
        {
            Console.WriteLine("Enter the spell slot level you want to use (1-9):");
            int level = GetIntInput();
            if (level < 1 || level > 9)
                throw new ArgumentOutOfRangeException(nameof(level), "Level must be between 1 and 9.");
            if (_maxSpellSlots[level - 1] > 0)
            {
            if (_currentSpellSlots[level - 1] > 0)
            {
                _currentSpellSlots[level - 1]--;
            }
            else
            {
                Console.WriteLine($"No remaining spell slots for level {level}.");
                StartAnimation();
            }
            }
            else
            {
                Console.WriteLine("No spell slots available for that level.");
                StartAnimation();
            }
        }
        else
        {
            Console.WriteLine("No spell slots available.");
            StartAnimation();
        }
    }
    public void RechargeSpellSlots()
    {
        for (int i = 0; i < 9; i++)
        {
            _currentSpellSlots[i] = _maxSpellSlots[i];
        }
    }
}