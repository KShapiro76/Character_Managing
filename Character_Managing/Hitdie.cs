public class HitDie : BasicForm
{
    private int _type;
    private int _quantityMax;
    private int _quantityCurrent;
    public HitDie(int type, int quantityMax, int quantityCurrent)
    {
        _type = type;
        _quantityMax = quantityMax;
        _quantityCurrent = quantityCurrent;
    }
    public HitDie()
    {
        _type = 0;
        _quantityMax = 0;
        _quantityCurrent = 0;
    }
    public override void LoadFromFile(string[] lines = null)
    {
        
        string[] hitDieParts = lines[0].Substring(9).Split(new[] { " out of ", "d" }, StringSplitOptions.RemoveEmptyEntries);
        _quantityCurrent = int.Parse(hitDieParts[0]);
        _quantityMax = int.Parse(hitDieParts[1]);
        _type = int.Parse(hitDieParts[2]);
    }
    public string DisplayHitDieInfo(bool SavetoFile = false)
    {
        string HitDieInfo;
        if (SavetoFile)
        {
            HitDieInfo = $"Hit Die: {_quantityCurrent} out of {_quantityMax}d{_type}";
        }
        else
        {
            HitDieInfo = $"Hit Die: \u001b[31m{_quantityCurrent}\u001b[0m out of \u001b[31m{_quantityMax}d{_type}\u001b[0m";
        }
        //Hit Die: 2 out of 3d6
        //Hit Die: 1 out of 2d8
        return HitDieInfo;
    }
    public int UseHitDie()
    {
        Console.WriteLine(DisplayHitDieInfo());
        int healthToRestore;
        if (_quantityCurrent == 0)
        {
            Console.WriteLine("These hit dice are depleted and can't be used to restore HP");
            healthToRestore = 0;
        }
        else
        {
            Console.Write("How many hit dice would you like to use of this type? ");
            int hitDiceUsed = GetIntInput();
            _quantityCurrent -= hitDiceUsed;
            if (_quantityCurrent < 0)
            {
                _quantityCurrent = 0;
            }
            if (hitDiceUsed == 0)
            {
                healthToRestore = 0;
            }
            else
            {
                Console.Write("\nAfter rolling, how much health was restored? ");
                healthToRestore = GetIntInput();;
            }
        }
        return healthToRestore;

        
    }
    public void RechargeHitDie()
    {
        _quantityCurrent = _quantityMax;
    }
    public void CreateHitDie()
    {
        Console.WriteLine($"Enter the type of hit die (e.g., 6 for d6):");
        _type = GetIntInput();
        Console.WriteLine($"Enter the maximum quantity of this hit die:");
        _quantityMax = GetIntInput();;
        Console.WriteLine($"Enter the current quantity of this hit die:");
        _quantityCurrent = GetIntInput();
    }
    public void UpdateHitDie()
    {
        DisplayHitDieInfo();
        Console.Write("\nEnter the new type of hit die (e.g., 6 for d6): ");
        _type = GetIntInput();
        Console.Write("\nEnter the new maximum quantity of this hit die: ");
        _quantityMax = GetIntInput();
        Console.Write("\nEnter the new current quantity of this hit die: ");
        _quantityCurrent = GetIntInput();
        Console.WriteLine();
    }
}