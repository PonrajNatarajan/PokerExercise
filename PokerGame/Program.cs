using System;
using System.Configuration;
using System.IO;

/// <summary>
/// 
/// </summary>
namespace PokerGame
{
    class Program
    {
        
    /// <summary>
    /// Reads deckfile path from AppSettings
    /// </summary>
    /// <param name="args"></param>
        static void Main(string[] args)
        {
            string deckFilePath=ReadSetting("DeckFilePath");
            string deckFileFormat = ReadSetting("FileFormat");
            string logFilePath = ReadSetting("LogFilePath");

            bool enableLog=Convert.ToBoolean(ReadSetting("EnableLog"));
            if (enableLog)
            {
                FileStream fs=File.Create(logFilePath);
                fs.Close();
                
            }

            if (File.Exists(deckFilePath))
            {
                string extension = Path.GetExtension(deckFilePath).Substring(1);
                if(deckFileFormat.Contains(extension.ToLower()))
                {
                    CardDeck deck = new CardDeck();
                    deck.deckFromTextFile = System.IO.File.ReadAllLines(deckFilePath);
                    deck.log = enableLog;
                    deck.EvaluatePlayerCards();

                    Console.WriteLine("Player 1 : " + deck.player1WinningHands.ToString() + " hands");
                    Console.WriteLine("Player 2 : " + deck.player2WinningHands.ToString() + " hands");

                    Console.WriteLine("Press any key to exit.");
                    System.Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Please make sure valid Deck file configured !!! ");
                }
            }
            else
            {
                Console.WriteLine("Deck file not found on specified path");
            }
        }


        static string ReadSetting(string key)
        {
            string result = "";
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                result = appSettings[key] ?? "Not Found";
                
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading default Deck file path");
            }
            return result;
        }
    }
}
