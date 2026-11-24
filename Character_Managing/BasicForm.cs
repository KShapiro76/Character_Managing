using System;
using System.Text;
using System.Threading;
/*I couldn't find a good name for the parent class
Most all classes in this program follow the "has a" rule. 
This gives all child classes a way to safely handle expetions
with grabbing integer inputs, playing an animation during wait times,
and Loading data from a file.
*/
public abstract class BasicForm
{
    public int GetIntInput()
    {
        do
        {
            string input = Console.ReadLine();
            if (input != null)
            {
                if (int.TryParse(input, out int number))
                {
                    return number;
                }
                else
                {
                    Console.WriteLine("Invalid entry. Please Try again.");
                }
            }

        } while (true);
        
    }
    public void StartAnimation(int cycles = 1)
    {
        Console.OutputEncoding = Encoding.UTF8;
        for (int i = 0; i < cycles; i++)
        {

        char[] frames = ['⠋', '⠙', '⠹', '⠸', '⠼', '⠴', '⠦', '⠧', '⠇', '⠏'];
        int durationMs = 2000;
        int frameDelayMs = 100;
        int totalSteps = durationMs / frameDelayMs;

        for (int ind = 0; ind < totalSteps; ind++)
        {
            Console.Write($"\r[{frames[ind % frames.Length]}]");
            Thread.Sleep(frameDelayMs);
        }

        Console.WriteLine("\r[✓]");
        }
    }
public virtual void LoadFromFile(string[] lines = null) {} 
/*LoadFromFile is virtual to allow each class to change it.
It's virtual instead of abstract so that it doesn't require
each child class to do so if it doesn't need to
*/
}