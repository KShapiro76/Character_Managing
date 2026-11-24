public class RechargeAbility : BasicForm
{
    private string _rechargeRestType;
    private bool _rechargeAtDawn;
    private int _usesRechargeDie;
    private int _quantityOfDice;
    private int _usesRechargeBase;
    public RechargeAbility()
    {
        _rechargeRestType = "None";
        _rechargeAtDawn = false;
        _usesRechargeDie = 0;
        _quantityOfDice = 0;
        _usesRechargeBase = 0;
    }
    public bool HasQuantity(string quantity)
    {
        
        bool hasQuantity;
        if (quantity.Contains('d') && quantity.Contains('+'))
        {
            int plusIndex = quantity.IndexOf('+');
            int dieIndex = quantity.IndexOf('d');
            if (int.TryParse(quantity.Substring(0, dieIndex), out int number1) && int.TryParse(quantity.Substring(dieIndex + 1, plusIndex - dieIndex - 1), out int number2) && int.TryParse(quantity.Substring(plusIndex + 1), out int number3))
            {
                hasQuantity = true;
            }
            else
            {
                hasQuantity = false;
            }
        }
        else if (quantity.Contains('d') && !quantity.Contains('+'))
        {
            int dieIndex = quantity.IndexOf('d');
            if (int.TryParse(quantity.Substring(0, dieIndex), out int number1) && int.TryParse(quantity.Substring(dieIndex + 1), out int number2))
            {
                hasQuantity = true;
            }
            else
            {
                hasQuantity = false;
            }
        }
        else if (!quantity.Contains('d') && !quantity.Contains('+'))
        {
            if (int.TryParse(quantity, out int number1))
            {
                hasQuantity = true;
            }
            else
            {
                hasQuantity = false;
            }
        }
        else
        {
            hasQuantity = false;
        }
        return hasQuantity;
    }
    public override void LoadFromFile(string[] lines = null)
    {
        int dashIndex = lines[0].Trim().IndexOf("-");
        int everyIndex = lines[0].Trim().IndexOf(" every ");
        int quantityStartIndex = dashIndex + 14;
        if (lines[0].Trim().Substring(dashIndex).Contains("(At Dawn)"))
        {
            _rechargeRestType = "None";
            _rechargeAtDawn = true;
        }
        else if (lines[0].Contains("Short"))
        {
            _rechargeRestType = "Short or Long Rest";
            _rechargeAtDawn = false;
        }
        else
        {
            _rechargeRestType = "Long Rest";
            _rechargeAtDawn = false;
        }
        SetQuantity(lines[0].Trim().Substring(quantityStartIndex, everyIndex - quantityStartIndex).Trim());
    }
    public void SetQuantity(string quantity)
    {
        if (quantity.Contains('d') && quantity.Contains('+'))
        {
            int plusIndex = quantity.IndexOf('+');
            int dieIndex = quantity.IndexOf('d');
            // 10d6 + 3
            _quantityOfDice = int.Parse(quantity.Substring(0, dieIndex));
            _usesRechargeDie = int.Parse(quantity.Substring(dieIndex + 1, plusIndex - dieIndex - 1));
            _usesRechargeBase = int.Parse(quantity.Substring(plusIndex + 1));
        }
        else if (quantity.Contains('d') && !quantity.Contains('+'))
        {
            int dieIndex = quantity.IndexOf('d');
            // 10d6
            _quantityOfDice = int.Parse(quantity.Substring(0, dieIndex));
            _usesRechargeDie = int.Parse(quantity.Substring(dieIndex + 1));
            _usesRechargeBase = 0;
        }
        else if (!quantity.Contains('d') && !quantity.Contains('+'))
        {
            // 3
            _quantityOfDice = 0;
            _usesRechargeDie = 0;
            _usesRechargeBase = int.Parse(quantity);
        }
    }
    public void SetRechargeType(string rechargeType)
    {
        _rechargeRestType = rechargeType;
    }
    public void SetRechargeAtDawn(bool rechargeAtDawn)
    {
        _rechargeAtDawn = rechargeAtDawn;
    }
    public string GetRechargeRestType()
    {
        return _rechargeRestType;
    }
    public int GetRechargeBase()
    {
        return _usesRechargeBase;
    }
    public bool GetRechargeAtDawn()
    {
        return _rechargeAtDawn;
    }
    public string GetRechargeInfo(bool SavetoFile = false)
    {
        string result;
        if (SavetoFile)
        {
            if (_rechargeRestType != "None")
            {
                result = $"Recharges: {GetQuantityRecharged()} every {_rechargeRestType}";
            }
            else
            {
                result = $"Recharges: {GetQuantityRecharged()} every Day (At Dawn)";
            }
        }
        else
        {
            if (_rechargeRestType != "None")
            {
                result = $"Recharges: \u001b[31m{GetQuantityRecharged()}\u001b[0m every {_rechargeRestType}";
            }
            else
            {
                result = $"Recharges: \u001b[31m{GetQuantityRecharged()}\u001b[0m every Day (At Dawn)";
            }
        }
        
        
        return result;
    }
    public void UpdateRechargeAbilty()
    {
        Console.WriteLine(GetRechargeInfo());
        Console.WriteLine("Which would you like to change?");
        Console.WriteLine($"1. Base amount: {_usesRechargeBase}");
        Console.WriteLine($"2. Dice Type: d{_usesRechargeDie}");
        Console.WriteLine($"3. Quantity of Dice: {_quantityOfDice}");
        Console.WriteLine($"4. Recharges at Dawn: {(_rechargeAtDawn ? "Yes":"No")}");
        Console.WriteLine($"5. Rest Type: {_rechargeRestType}");
        Console.WriteLine("6. Exit");
        string input = Console.ReadLine();

        switch (input)
        {
            case "1":
                Console.Write("Enter the new base amount: ");
                _usesRechargeBase = GetIntInput();
                Console.WriteLine();
                break;
            case "2":
                Console.Write("Enter the new dice type: d");
                _usesRechargeDie = GetIntInput();
                Console.WriteLine();
                break;
            case "3":
                Console.Write("Enter the new quantity: ");
                _quantityOfDice = GetIntInput();
                Console.WriteLine();
                break;
            case "4":
                Console.Write("Does the ability recharge at dawn (yes/no)? ");
                _rechargeAtDawn = Console.ReadLine().Trim().ToLower() == "yes" ? true : false;
                break;
            case "5":
                break;
            default:
                Console.WriteLine("Invalid option. Please Try Again");
                break;
        }
    }
    public bool NeedsRoll()
    {
        if (_usesRechargeDie == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public string GetQuantityRecharged()
    {
        string result = "";
        if (_usesRechargeBase == 0)
        {
            result = $"{_quantityOfDice}d{_usesRechargeDie}";
        }
        else if (_usesRechargeDie == 0)
        {
            result = _usesRechargeBase.ToString();
        }
        else
        {
            result = $"{_quantityOfDice}d{_usesRechargeDie} + {_usesRechargeBase}";
        }
        
        
        return result;
    }
}