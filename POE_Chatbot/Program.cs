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
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("What's your name? ");
            Console.ForegroundColor = ConsoleColor.White;
            string userName = Console.ReadLine();


            LoadingEffect();
            Console.ForegroundColor = ConsoleColor.Magenta;
            RespondWithSpeech($"Hi, {userName}! I'm here to help you stay safe online.\n");

            // Ask about interests
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\nIs there a specific topic you're interested in? ");
            Console.ForegroundColor = ConsoleColor.Green;
            string interestInput = Console.ReadLine()?.ToLower().Trim();



            if (!string.IsNullOrWhiteSpace(interestInput))
            {
                string inputLower = interestInput.ToLower();
                string topic = null;
                string phrase = "interested in";

                int startIndex = inputLower.IndexOf(phrase);
                if (startIndex >= 0)
                {
                    // Extract after "interested in"
                    startIndex += phrase.Length;
                    if (startIndex < interestInput.Length)
                    {
                        topic = interestInput.Substring(startIndex).Trim(' ', '.', '!', '?');
                    }
                }
                else
                {
                    // If phrase not found, assume entire input is the topic
                    topic = interestInput.Trim(' ', '.', '!', '?');
                }

                if (!string.IsNullOrEmpty(topic))
                {
                    favoriteTopic = topic;
                    currentTopic = topic.ToLower();

                    RespondWithSpeech($"Great! I'll remember that you're interested in {favoriteTopic}. It's a crucial part of staying safe online.");
                }
            }

            // Display available topics the user can ask about
            Console.WriteLine(new string('-', 50));
            Console.WriteLine(" You can ask about:");
            Console.WriteLine("- What is phishing?");
            Console.WriteLine("- What is cybersecurity?");
            Console.WriteLine("- What is data protection?");
            Console.WriteLine(" - How to create strong passwords");
            Console.WriteLine(" - How to recognize phishing emails");
            Console.WriteLine(" - How to protect personal data");
            Console.WriteLine(" - General questions like \"How are you?\", \"What's your purpose?\", " +
            "and \"What can I ask you about?\"");
            Console.WriteLine(" - Or type 'exit' to quit.");
            Console.WriteLine(new string('-', 50));

            // Start the chat loop
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"\n{userName}: ");
                string userInput = Console.ReadLine()?.ToLower().Trim();

                // checks for an empty input
                if (string.IsNullOrEmpty(userInput))
                {
                    LoadingEffect();
                    Console.ForegroundColor = ConsoleColor.Red;
                    RespondWithSpeech("Please enter a valid question.");
                    continue;
                }

                //Check to see if the user enters 'exit' command
                if (userInput == "exit")
                {
                    LoadingEffect();
                    Console.ForegroundColor = ConsoleColor.Green;
                    RespondWithSpeech($"Goodbye, {userName}! Stay sharp and secure out there.");
                    break;
                }

                HandleUserQuery(userInput, userName);

            }

            // Save chat history when exiting
            SaveChatHistory();
        }


    }
}
