using System.Formats.Asn1;

public class Ability : BasicForm
{
    private string _name;
    private string _description;
    private int _usesPerDay;
    private int _remainingUses;
    private RechargeAbility _recharge = new RechargeAbility();
    public Ability()
    {
        _name = "";
        _description = "";
        _usesPerDay = 0;
        _remainingUses = 0;
        _recharge = new RechargeAbility();
    }
    public Ability(string name, string description, int usesPerDay, int remainingUses, RechargeAbility recharge)
    {
        _name = name;
        _description = description;
        _usesPerDay = usesPerDay;
        _remainingUses = remainingUses;
        _recharge = recharge;
    }
    public override void LoadFromFile(string[] lines = null)
    {
        int slashIndex = lines[2].Trim().IndexOf("/");
        int dashIndex = lines[2].Trim().IndexOf("-");
        _name = lines[0].Trim().Substring(14);
        _description = lines[1].Trim().Substring(13);
        _usesPerDay = int.Parse(lines[2].Trim().Substring(slashIndex + 1, dashIndex - slashIndex - 1));
        _remainingUses = int.Parse(lines[2].Trim().Substring(5, slashIndex - 5));
        //RechargeAbility _recharge = new RechargeAbility();
        string[] rechargeLine = {lines[2]};
        _recharge.LoadFromFile(rechargeLine);
    }
    public void CreateAbility()
    {
        Console.WriteLine("Enter ability name:");
        _name = Console.ReadLine();
        Console.WriteLine("Enter ability description:");
        _description = Console.ReadLine();
        Console.WriteLine("Enter maximum uses per day or rest:");
        _usesPerDay = GetIntInput();
        Console.WriteLine("Enter remaining uses:");
        _remainingUses = GetIntInput();
        _recharge = new RechargeAbility();
        string rechargeRestType;
        bool notDoneWithRestType = true;
        do
        {
            Console.WriteLine("Enter recharge rest type (e.g., Long Rest, Short or Long Rest, None):");
            rechargeRestType = Console.ReadLine();
        
            string noSpaces = rechargeRestType.Trim().Replace(" ", "").ToLower();
            switch (noSpaces)
            {
                case "longrest":
                    rechargeRestType = "Long Rest";
                    notDoneWithRestType = false;
                    break;
                case "shortorlongrest":
                    rechargeRestType = "Short or Long Rest";
                    notDoneWithRestType = false;
                    break;
                case "none":
                    rechargeRestType = "None";
                    notDoneWithRestType = false;
                    break;
                default:
                    Console.WriteLine("Invalid Input. Please enter either 'Long Rest', 'Short or Long Rest', or 'None'");
                    break;

            }
        } while (notDoneWithRestType);
        Console.WriteLine("Does this ability recharge at dawn? (yes/no)");
        bool rechargeAtDawn = Console.ReadLine().Trim().ToLower() == "yes" ? true : false;
        bool DoneWithQuantity = false;
        string quantity;
        do
        {
        Console.WriteLine("Enter quantity recharged (e.g., 2d6 + 3, 1d4, 5):");
        quantity = Console.ReadLine();
        DoneWithQuantity = !_recharge.HasQuantity(quantity);
        if (DoneWithQuantity)
        {
            Console.WriteLine("Invalid format. Please try again.");
        }
        } while (DoneWithQuantity);
        _recharge.SetRechargeType(rechargeRestType);
        _recharge.SetRechargeAtDawn(rechargeAtDawn);
        _recharge.SetQuantity(quantity);
    }
    public string GetName()
    {
        return _name;
    }
    public string GetRecharge(bool SavetoFile = false)
    {
        return _recharge.GetRechargeInfo(SavetoFile);
    }
    public string GetAbilityInfo(bool SavetoFile = false)
    {
        string abilityInfo;
        if (SavetoFile)
        {
            abilityInfo = 
            $"Ability Name: {_name}" + 
            $"\n     Description: {_description}" + 
            $"\n     Uses: {_remainingUses}/{_usesPerDay} -> {GetRecharge(SavetoFile)}";
        }
        else
        {
            abilityInfo = 
            $"Ability Name: \u001b[34m{_name}\u001b[0m" + 
            $"\n     Description: \u001b[32m{_description}\u001b[0m" + 
            $"\n     Uses: \u001b[31m{_remainingUses}/{_usesPerDay}\u001b[0m -> {GetRecharge()}";
        }
        return abilityInfo;
    }
    public void UseAbility()
    {
        Console.WriteLine("How many uses of the ability would you like to use?");
        int usesToUse;
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out usesToUse))
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a positive integer.");
            }
        }
        if (usesToUse <= _remainingUses && _remainingUses > 0)
        {
            _remainingUses -= usesToUse;
            Console.WriteLine($"Used {_name} {usesToUse} time(s). Remaining uses: {_remainingUses}");
        }
        else if (_remainingUses == 0)
        {
            Console.WriteLine($"No remaining uses for ability: {_name}");
        }
        else
        {
            Console.WriteLine($"Not enough remaining uses for {_name}. You have {_remainingUses} remaining.");
        }
        
        StartAnimation();
    }
    public void RechargeAbility(string RestType)
    {
        
        if (((RestType == "Dawn" && _recharge.GetRechargeAtDawn()) || (RestType == _recharge.GetRechargeRestType()) || (RestType == "Long Rest" && _recharge.GetRechargeRestType() == "Short or Long Rest")) && _remainingUses != _usesPerDay)
        {    
            Console.WriteLine($" Ability: \u001b[31m{(_name)}\u001b[0m");
            int usesToRecharge;
            if (_recharge.NeedsRoll())
            {
                Console.WriteLine($"This ability recharges {_recharge.GetQuantityRecharged()}");
                Console.WriteLine("After rolling, enter the total number of uses to recharge:");
                while (true)
                {
                    if (int.TryParse(Console.ReadLine(), out usesToRecharge) && usesToRecharge > 0)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a positive integer.");
                    }
                }
            }
            else
            {
                usesToRecharge = _recharge.GetRechargeBase();
            }
            if (usesToRecharge + _remainingUses > _usesPerDay && _remainingUses != _usesPerDay)
            {
                Console.Write($"{_usesPerDay - _remainingUses} uses recharged.");
                _remainingUses = _usesPerDay;
                Console.Write($" \u001b[31m{_remainingUses}/{_usesPerDay}\u001b[0m");
                Console.WriteLine();
            }
            
            else if (usesToRecharge < 0 && _remainingUses != _usesPerDay)
            {
                Console.WriteLine("Cannot recharge a negative number of uses.");
            }
            else
            {
                _remainingUses = usesToRecharge + _remainingUses;
                Console.WriteLine($"{usesToRecharge} uses recharged. \u001b[31m{_remainingUses}/{_usesPerDay}\u001b[0m");
            }
        }
        else
        {
            return;
        }
        StartAnimation();
    }
    public void UpdateAbility()
    {
        do
        {
        GetAbilityInfo();
        Console.WriteLine("Which detail would you like to update?");
        Console.WriteLine("1. Ability name");
        Console.WriteLine("2. Ability description");
        Console.WriteLine("3. Number of uses per day or rest");
        Console.WriteLine("4. Number of remaining uses");
        Console.WriteLine("5. Recharge conditions");
        Console.WriteLine("6. Exit");
        string input = Console.ReadLine();
        switch (input)
        {
            case "1":
                Console.Write("Enter the new ability name: ");
                _name = Console.ReadLine();
                Console.WriteLine();
                return;
            case "2":
                Console.Write("Enter the new ability description: ");
                _description = Console.ReadLine();
                Console.WriteLine();
                return;
            case "3":
                Console.Write("Enter the new total for uses per day or rest: ");
                _usesPerDay = GetIntInput();
                Console.WriteLine();
                return;
            case "4":
                Console.Write("Enter the new total for remaining uses: ");
                _remainingUses = GetIntInput();
                Console.WriteLine();
                return;
            case "5":
                _recharge.UpdateRechargeAbilty();
                return;
            case "6":
                return;
            default:
                Console.WriteLine("Invalid opiton. Please try again.");
                break;
            
        }
        } while (true);



        
    }
}