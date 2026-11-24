using System.IO.Pipelines;

public class HitDieList : BasicForm
{
    private List<HitDie> _hitDieList;

    public HitDieList()
    {
        _hitDieList = new List<HitDie>();
    }
    public void CreateHitDieList()
    {
        Console.WriteLine("Enter the number of different hit die types your character has:");
        int numHitDieTypes = GetIntInput();
        for (int i = 0; i < numHitDieTypes; i++)
        {
            HitDie hitDie = new HitDie(0,0,0);
            hitDie.CreateHitDie();
            _hitDieList.Add(hitDie);
        }
    }
    public override void LoadFromFile(string[] lines)
    {
        int hitDieStartIndex = Array.FindIndex(lines, line => line.StartsWith("Hit Die:"));
        if (hitDieStartIndex != -1)
        {
            int hitDieEndIndex = Array.FindIndex(lines, hitDieStartIndex + 1, line => string.IsNullOrWhiteSpace(line));
            if (hitDieEndIndex == -1)
            {
                hitDieEndIndex = lines.Length;
            }
            for (int i = hitDieStartIndex; i < hitDieEndIndex; i++)
            {
                if (lines[i].StartsWith("Hit Die:"))
                {
                    string[] hitDieLine = {lines[i]};
                    HitDie hitdie = new HitDie();
                    hitdie.LoadFromFile(hitDieLine);
                    _hitDieList.Add(hitdie);
                }
            }
        }
    }
    public void RemoveHitDie(HitDie hitDie)
    {
        _hitDieList.Remove(hitDie);
    }
    public int UseHitdice()
    {
        int healthtoRestore = 0;
        foreach (HitDie hitdie in _hitDieList)
        {
            healthtoRestore += hitdie.UseHitDie();
        }
        return healthtoRestore;
    }
    public void RechargeAllHitDie()
    {
        foreach (HitDie hitDie in _hitDieList)
        {
            hitDie.RechargeHitDie();
        }
    }
    public string GetHitDiceInfo(bool SavetoFile = false)
    {
        string result = "";
        foreach (HitDie hitDie in _hitDieList)
        {
            result += $"\n{hitDie.DisplayHitDieInfo(SavetoFile)}";
        }
        return result;
    }
    private void DisplayHitDice()
    {
        int i = 1;
        foreach (HitDie hitDie in _hitDieList)
        {
            Console.WriteLine($"{i}. {hitDie.DisplayHitDieInfo()}");
            i++;
        }
    }
    public void UpdateHitDiceList()
    {
        do
        {
        Console.WriteLine("Choose an option:");
        Console.WriteLine("1. Add new Hit Die");
        Console.WriteLine("2. Update Hit Die");
        Console.WriteLine("3. Remove Hit Die");
        Console.WriteLine("4. Exit");
        string input = Console.ReadLine();
        Console.Clear();

        switch (input)
        {
            case "1":
                HitDie hitdie = new HitDie(0,0,0);
                hitdie.CreateHitDie();
                _hitDieList.Add(hitdie);
                return;
            case "2":
                DisplayHitDice();
                Console.WriteLine("Select a Hit Die to Update");
                int HitDietoUpdate = GetIntInput();
                _hitDieList[HitDietoUpdate - 1].UpdateHitDie();
                return;
            case "3":
                DisplayHitDice();
                Console.WriteLine("Select a Hit Die to Remove");
                int HitDietoRemove = GetIntInput();
                RemoveHitDie(_hitDieList[HitDietoRemove - 1]);
                return;
            case "4":
                return;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
        }
        } while (true);
    }
}