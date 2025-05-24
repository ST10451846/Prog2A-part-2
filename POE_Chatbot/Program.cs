using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace POE_Chatbot
{
    internal class Program
    {
        static List<string> chatHistory = new List<string>();
        static SpeechSynthesizer synth = new SpeechSynthesizer
        {
            Volume = 100,
            Rate = 0
        };

        static Random random = new Random();
        static void Main(string[] args)
        {
            // Play audio greeting
            PlayWelcomeAudio("Welcome_greeting.wav");

            // Creating a console window
            Console.Title = "Cybersecurity Awareness Chatbot";
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(new string('*', Console.WindowWidth));


            //ASCII ART
            String[] AsciiArtLines = new String[]
{
    "  _______     ______  ______ _____  _____  ______ ______ ______ _   _ _____  ______ _____   _____ ",
    "  / ____\\ \\   / /  _ \\|  ____|  __ \\|  __ \\|  ____|  ____|  ____| \\ | |  __ \\|  ____|  __ \\ / ____|",
    " | |     \\ \\_/ /| |_) | |__  | |__) | |  | | |__  | |__  | |__  |  \\| | |  | | |__  | |__) | (___  ",
    " | |      \\   / |  _ <|  __| |  _  /| |  | |  __| |  __| |  __| | . ` | |  | |  __| |  _  / \\___ \\ ",
    " | |____   | |  | |_) | |____| | \\ \\| |__| | |____| |    | |____| |\\  | |__| | |____| | \\ \\ ____) | ",
    "  \\_____|  |_|  |____/|______|_|  \\_\\_____/|______|_|    |______|_| \\_|_____/|______|_|  \\_\\_____/  ",
    "",
    "[CBS BOT - Your Cybersecurity Buddy!]"
};


            int consoleWidth = Console.WindowWidth;
            foreach (string line in AsciiArtLines)
            {
                int padding = (consoleWidth - line.Length) / 2;
                Console.WriteLine(new string(' ', Math.Max(0, padding)) + line);
            }

            Console.WriteLine(new string('*', Console.WindowWidth)); // Bottom border

            TypingEffect("Hello! Welcome to your Cybersecurity Awareness Assistant!\n");

            // Ask for the user's name

        }
    }
}
