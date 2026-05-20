using System;
using System.Threading;
using AgainstTheStormTrainer.Core;

namespace AgainstTheStormTrainer
{
    /// <summary>
    /// Entry point for the Against the Storm Trainer application.
    /// Provides a console interface for activating various cheats.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Against the Storm Trainer v1.0 ===");
            Console.WriteLine("Ensure the game is running before using features.\n");

            using (var memoryManager = new GameMemoryManager("AgainstTheStorm"))
            using (var resourceHack = new ResourceHack(memoryManager))
            using (var speedHack = new SpeedHack(memoryManager))
            {
                if (!memoryManager.IsProcessOpen)
                {
                    Console.WriteLine("Game process not found. Please start Against the Storm.");
                    return;
                }

                Console.WriteLine("Game process detected.\n");
                Console.WriteLine("Commands:");
                Console.WriteLine("  resources - Set all resources to 9999");
                Console.WriteLine("  speed     - Toggle game speed (2x)");
                Console.WriteLine("  exit      - Quit the trainer\n");

                string input;
                while ((input = Console.ReadLine()?.ToLower()) != "exit")
                {
                    switch (input)
                    {
                        case "resources":
                            if (resourceHack.SetAllResources(9999))
                                Console.WriteLine("Resources set to 9999.");
                            else
                                Console.WriteLine("Failed to modify resources.");
                            break;
                        case "speed":
                            speedHack.ToggleSpeed();
                            Console.WriteLine($"Speed hack toggled. Current multiplier: {speedHack.CurrentMultiplier}x");
                            break;
                        default:
                            Console.WriteLine("Unknown command.");
                            break;
                    }
                }
            }

            Console.WriteLine("Trainer closed.");
        }
    }
}
