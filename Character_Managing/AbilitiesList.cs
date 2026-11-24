public class AbilitiesList : BasicForm
{
    private List<Ability> _abilitiesList;

    public AbilitiesList()
    {
        _abilitiesList = new List<Ability>();
    }
    public void CreateAbilitiesList()
    {
        Console.WriteLine("Does your character have any abilities with expendible uses? (yes/no)");
        bool hasAbilities = Console.ReadLine().Trim().ToLower() == "yes"? true : false;
        while (hasAbilities)
        {
            Ability ability = new Ability("", "", 0, 0, null);
            ability.CreateAbility();
            _abilitiesList.Add(ability);
            Console.WriteLine("Does your character have any more abilities with expendible uses? (yes/no)");
            hasAbilities = Console.ReadLine().Trim().ToLower() == "yes"? true : false;
        }
    }
    public override void LoadFromFile(string[] lines)
    {
        int abilitiesStartIndex = Array.FindIndex(lines, line => line.StartsWith("Ability Name:"));
        if (abilitiesStartIndex != -1)
        {
            int abilitiesEndIndex = Array.FindIndex(lines, abilitiesStartIndex + 1, line => string.IsNullOrWhiteSpace(line));
            if (abilitiesEndIndex == -1)
            {
                abilitiesEndIndex = lines.Length;
            }
            for (int i = abilitiesStartIndex; i < lines.Count(); i++)
            {
                if (lines[i].StartsWith("Ability Name:"))
                {
                    /*
                    Ability Name: Name
                         Description: Ability description
                         Uses: 3/4 -> Recharges: 10d12 + 9 every Day (At Dawn)
                                               (10d12)     every Long Rest
                                                        9 every Short or Long Rest
                    */
                    Ability ability = new Ability();
                    ability.LoadFromFile(lines[i..(i+3)]);
                    _abilitiesList.Add(ability);
                }
            }
        }
    }
    public void UseAbility(Ability Ability)
    {
        Ability.UseAbility();
    }
    public void UpdateAbility(Ability Ability)
    {
        Ability.UpdateAbility();
    }
    public void RemoveAbility(Ability Ability)
    {
        _abilitiesList.Remove(Ability);
    }
    public List<Ability> GetAbilitiesList()
    {
        return _abilitiesList;
    }
    public void DisplayAllAbilities()
    {
        if (_abilitiesList.Count == 0)
        {
            Console.WriteLine("No abilities available.");
            return;
        }
        int i=1;
        foreach (Ability ability in _abilitiesList)
        {
            Console.WriteLine(ability.GetAbilityInfo());
            i++;
        }
    }
    public string GetAbilitiesInfo(bool SavetoFile = false)
    {
        string result = "";
        foreach (Ability ability in _abilitiesList)
        {
            result += $"\n{ability.GetAbilityInfo(SavetoFile)}";
        }
        return result;
    }
    public Ability GetAbilityByName(string name)
    {
        return _abilitiesList.FirstOrDefault(a => a.GetName().Equals(name, StringComparison.OrdinalIgnoreCase));
    }
    public void RechargeAbilities(string restType)
    {
        Console.Clear();
        foreach (Ability ability in _abilitiesList)
        {
            ability.RechargeAbility(restType);
        }
    }
    public void AddAbility(Ability ability)
    {
        _abilitiesList.Add(ability);
    }
    public Ability FindAbilityTo(string UpdateorRemoveorUse)
    {
        DisplayAllAbilities();
        do
        {
            Console.WriteLine($"Enter the name of the ability you want to {UpdateorRemoveorUse}:");
            string abilityName = Console.ReadLine();
            if (_abilitiesList.Contains(GetAbilityByName(abilityName)))
            {
            return GetAbilityByName(abilityName);
            }
            else
            {
                Console.WriteLine("Invalid Name. Please try again.");
            }
        } while (true);
    }
    public void UpdateAbilityList()
    {
        do
        {
        Console.WriteLine("Please select an option:");
        Console.WriteLine("1. Add new ability");
        Console.WriteLine("2. Update ability");
        Console.WriteLine("3. Remove Ability");
        Console.WriteLine("4. Exit");
        string input = Console.ReadLine();
        Console.Clear();
        switch (input)
        {
            case "1":
                Ability ability = new Ability("", "", 0, 0, null);
                ability.CreateAbility();
                AddAbility(ability);
                return;
            case "2":
                if (GetAbilitiesList().Count == 0)
                {
                    Console.WriteLine($"No abilities available to update.");

                }
                else
                {
                UpdateAbility(FindAbilityTo("update"));
                }
                return;
            case "3":
                if (GetAbilitiesList().Count == 0)
                {
                    Console.WriteLine($"No abilities available to remove.");
                }
                else
                {
                RemoveAbility(FindAbilityTo("remove"));
                }
                return;
            case "4":
                return;
            default:
                Console.WriteLine("Invalid Option. Please Try Again");
                break;;
        }
        } while (true);
    }
}