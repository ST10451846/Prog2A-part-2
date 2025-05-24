using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
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

        // Method to play the welcome audio 
        static void PlayWelcomeAudio(string filePath)
        {
            try
            {
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                if (File.Exists(fullPath))
                {
                    SoundPlayer player = new SoundPlayer(fullPath);
                    player.PlaySync();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: The file '{filePath}' was not found.");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($" Error playing audio: {ex.Message}");

            }
        }

        static string favoriteTopic = null;
        static string currentTopic = null; // Make sure this is declared at class level

        // Method that provides an output to the user's question 
        static void HandleUserQuery(string input, string userName)
        {
            if (string.IsNullOrWhiteSpace(input))
                return;

            var sentimentResponses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        { "worried", "It's completely understandable to feel that way. Let me share some tips to help you stay safe." },
        { "nervous", "No worries! Cybersecurity can seem tricky at first, but I’m here to help." },
        { "scared", "You’re not alone. Many people feel this way. Let's look at how to protect yourself." },
        { "curious", "Curiosity is great! Let’s dive into some interesting facts about cybersecurity." },
        { "frustrated", "I get it. These things can be overwhelming. Let's take it step by step." }
    };

            var responses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        { "password", "Create strong passwords by using a mix of letters, numbers, and symbols. Avoid using personal info!" },
        { "phishing", "Be cautious of emails that ask for personal info urgently. Always verify the sender!" },
        { "data protection", "Protect personal data by keeping your software updated and using two-factor authentication." },
        { "cybersecurity", "Cybersecurity refers to the practice of protecting systems, networks, and data from digital attacks." },
        { "how are you", "I'm doing great! How can I assist you with cybersecurity today?" },
        { "what's your purpose", "I'm here to help you learn how to stay safe online by giving cybersecurity advice." },
        { "what is phishing", "Phishing is a type of cyber attack where attackers impersonate legitimate entities to steal sensitive information." },
        { "what is data protection", "Data protection involves safeguarding important information from corruption, compromise, or loss." },
        { "what is cybersecurity", "Cybersecurity refers to the practice of protecting systems, networks, and data from digital attacks." },
        { "what can i ask you about", "You can ask me about password safety, phishing, protecting personal data, and general questions." },
        { "help", "You can ask about passwords, phishing, data protection, or general cybersecurity guidance." }
    };

            var detailedResponses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        { "password", "A strong password should be at least 12 characters long and combine uppercase, lowercase, numbers, and symbols. Avoid common words or patterns." },
        { "phishing", "Phishing attacks often use urgent or threatening language to trick you into clicking a malicious link or revealing sensitive info. Always verify sender details carefully." },
        { "data protection", "Data protection means securing your personal info using encryption, backups, secure passwords, and limiting who can access your data." },
        { "cybersecurity", "Cybersecurity involves multiple layers of protection across computers, networks, programs, or data to prevent unauthorized access or attacks." }
    };

            var keywordGroups = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
    {
        { "password", new List<string> { "password", "strong password", "secure password" } },
        { "phishing", new List<string> { "phishing", "phishing emails", "fake emails", "what is phishing", "explain phishing" } },
        { "data protection", new List<string> { "data protection", "protect personal data", "secure data", "what is data protection", "explain data protection" } },
        { "cybersecurity", new List<string> { "cybersecurity", "what is cybersecurity", "cyber security", "explain cybersecurity" } }
    };

            var tips = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        { "phishing tip", "Always hover over links in emails to preview the URL. Don’t click unless you’re sure it's legit!" },
        { "password tip", "Use a passphrase instead of a password – it's longer and easier to remember, yet secure." },
        { "data protection tip", "Regularly back up your data and avoid storing sensitive information on shared devices." }
    };

            Random random = new Random();

            // Clean input (remove punctuation and make lowercase)
            string cleanedInput = new string(input.Where(c => !char.IsPunctuation(c)).ToArray()).ToLower();

            // Improved follow-up detection

            // Multi-word phrases to check first
            string[] multiWordFollowUps = new string[]
            {
        "tell me more about",
        "what do you mean"
            };

            // Single word keywords to check
            string[] singleWordFollowUps = new string[]
            {
        "more", "explain", "explanation", "clarify", "confused", "details"
            };

            bool isFollowUp = false;

            // Check multi-word phrases first
            foreach (var phrase in multiWordFollowUps)
            {
                if (cleanedInput.Contains(phrase))
                {
                    isFollowUp = true;
                    break;
                }
            }

            if (!isFollowUp)
            {

                // Split input into words, ignoring empty entries
                string[] inputWords = input
                    .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);


                foreach (var keyword in singleWordFollowUps)
                {
                    if (inputWords.Contains(keyword))
                    {
                        isFollowUp = true;
                        break;
                    }
                }
            }

            // Check sentiment first
            string matchedSentiment = sentimentResponses.Keys
                .FirstOrDefault(sentiment => cleanedInput.Contains(sentiment.ToLower()));

            if (matchedSentiment != null)
            {
                RespondWithSpeech(sentimentResponses[matchedSentiment]);
                return;
            }

            //Check exact match in responses
            string exactMatch = responses.Keys
                .FirstOrDefault(k => k.Equals(cleanedInput, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(exactMatch))
            {
                RespondWithSpeech(responses[exactMatch]);
                currentTopic = exactMatch.ToLower();
                return;
            }

            // If follow-up, and currentTopic has a detailed response, return that
            if (isFollowUp && currentTopic != null && detailedResponses.ContainsKey(currentTopic))
            {
                RespondWithSpeech(detailedResponses[currentTopic]);
                return;
            }

            // Check if input contains any key exactly, respond accordingly
            foreach (var key in responses.Keys)
            {
                if (cleanedInput.Contains(key.ToLower()))
                {
                    string baseResponse = responses[key];
                    string personalizedResponse = (!string.IsNullOrEmpty(favoriteTopic) && favoriteTopic.ToLower() == key.ToLower())
                        ? $"Since you're interested in {favoriteTopic}, here's a tip: {baseResponse}"
                        : baseResponse;

                    RespondWithSpeech(personalizedResponse);
                    currentTopic = key.ToLower();
                    return;
                }
            }

            //  Check sentiment again 
            foreach (var sentiment in sentimentResponses)
            {
                if (cleanedInput.Contains(sentiment.Key.ToLower()))
                {
                    RespondWithSpeech(sentiment.Value);
                    return;
                }
            }

            //  Check tips dictionary
            foreach (var tip in tips)
            {
                if (cleanedInput.Contains(tip.Key.ToLower()))
                {
                    RespondWithSpeech(tip.Value);
                    return;
                }
            }

            // Keyword group fallback
            foreach (var group in keywordGroups)
            {
                foreach (var keyword in group.Value)
                {
                    if (cleanedInput.Contains(keyword.ToLower()))
                    {
                        RespondWithSpeech(responses[group.Key]);
                        currentTopic = group.Key;
                        return;
                    }
                }
            }

            //  Fallback tips if nothing matched
            string[] fallbackTips = new string[]
            {
               "Use multi-factor authentication (MFA) wherever possible to enhance security!",
               "Always verify the sender's email address before clicking on any link!",
               "Avoid using public Wi-Fi for accessing sensitive information like banking."
            };

            string randomTip = fallbackTips[random.Next(fallbackTips.Length)];
            RespondWithSpeech($"Hmm... I’m not sure about that. Here's a tip instead: {randomTip}");
        }

        // Method for the typing effect of the chatbot
        static void TypingEffect(string message, int minDelay = 20, int maxDelay = 80)
        {
            foreach (char c in message)
            {
                Console.Write(c);
                Thread.Sleep(random.Next(minDelay, maxDelay));
            }
        }

        //Method for the loading of the chatbot's output
        static void LoadingEffect()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("ChatBot");
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(200);
                Console.Write(".");
            }
            Console.WriteLine();
        }

        // method saves the chat history of conversation
        static void SaveChatHistory()
        {
            string path = "chat_history.txt";
            File.WriteAllLines(path, chatHistory);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"Chat History saved to {path}");
        }

        //Chatbot is able to read out the output it provides
        static void RespondWithSpeech(string response)
        {
            LoadingEffect();
            Console.ForegroundColor = ConsoleColor.Yellow;
            TypingEffect($"ChatBot: {response}\n");

            try
            {
                synth.Speak(response); //Reliable speech output 
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"TTS Error: {ex.Message} ");
            }

            chatHistory.Add($"ChatBot: {response}");
        }


    }
}
