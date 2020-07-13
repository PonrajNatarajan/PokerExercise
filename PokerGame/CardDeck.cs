using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// Class define deck of card for each player and evaluates the combination
/// </summary>
namespace PokerGame
{
    class CardDeck:Card
    {
        //private Card[] cards;
        public string[] deckFromTextFile;
        private Card[] player1;
        private Card[] player2;
        public int player1WinningHands;
        public int player2WinningHands;
        public int tieHands;
        public bool log;

    public CardDeck()
    {
        player1WinningHands=0;
        player2WinningHands=0;
            log = false;

    }
        /// <summary>
        /// If two ranks tie, for example, if both players have a pair of Jacks, then highest cards in each hand are compared; if the highest cards tie then the next highest cards are compared, and so on.
        /// </summary>
        /// <param name="compareCardIndex"></param>
    private void EvaluateHighCard(int compareCardIndex)
    {
            try
            {
                if (((Card)player1[compareCardIndex]).Value == ((Card)player2[compareCardIndex]).Value)
                {
                    if (((Card)player1[compareCardIndex - 1]).Value > ((Card)player2[compareCardIndex - 1]).Value)
                    {
                        player1WinningHands++;
                        return;
                    }
                    else if (((Card)player1[compareCardIndex - 1]).Value < ((Card)player2[compareCardIndex - 1]).Value)
                    {
                        player2WinningHands++;
                        return;
                    }
                    else
                    {
                        if (compareCardIndex > 0)
                            EvaluateHighCard(compareCardIndex - 1);
                        else
                            tieHands++;
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error occured in EvaluateHighCard"+ ex.Message);
            }
        }

        /// <summary>
        /// Evaluate Player card to identify type of combination
        /// reads the line of card values from array,spilt player1 and player2 cards and assign to card object
        /// </summary>
        public void EvaluatePlayerCards()
        {
            try
            {
                player1 = new Card[5];
                player2 = new Card[5];
                int cardIndex = 0;
                string[] tempCardCombination;
                string cardSuit;
                string cardValue;
                foreach (string lineInput in deckFromTextFile)
                {
                    tempCardCombination = lineInput.Split(" ");
                    foreach (string card in tempCardCombination)
                    {
                        cardSuit = card.Substring(1);
                        cardValue = card.Substring(0, 1);

                        if (cardIndex < 5)
                        {
                            player1[cardIndex] = new Card { Suit = (CARDSUIT)Enum.Parse(typeof(CARDSUIT), cardSuit, true), Value = (CARDVALUE)Enum.Parse(typeof(CARDVALUE), cardValue, true) };
                        }
                        else
                        {
                            player2[cardIndex - 5] = new Card { Suit = (CARDSUIT)Enum.Parse(typeof(CARDSUIT), cardSuit, true), Value = (CARDVALUE)Enum.Parse(typeof(CARDVALUE), cardValue, true) };

                        }
                        cardIndex++;
                    }
                    cardIndex = 0;

                    // Sort the card of each player by value
                    var sortPlayer1 = from hand in player1.ToList()
                                      orderby hand.Value
                                      select hand;

                    var sortPlayer2 = from hand in player2.ToList()
                                      orderby hand.Value
                                      select hand;
                    int sortCardIndex = 0;

                    foreach (var sortedCard in sortPlayer1)
                    {
                        player1[sortCardIndex] = sortedCard;
                        sortCardIndex++;
                    }
                    sortCardIndex = 0;
                    foreach (var sortedCard in sortPlayer2)
                    {
                        player2[sortCardIndex] = sortedCard;
                        sortCardIndex++;
                    }

                    //Evaluate 
                    EvaluationBase player1Evaluation = new EvaluationBase(player1);
                    EvaluationBase player2Evaluation = new EvaluationBase(player2);
                    combination player1Combination = player1Evaluation.EvaluatePlayerHand();
                    combination player2Combination = player2Evaluation.EvaluatePlayerHand();
                    string player1Value = "";
                    foreach (var element in player1)
                    {
                        player1Value += ((int)element.Value).ToString() + element.Suit + " ";
                    }
                    string player2Value = "";
                    foreach (var element in player2)
                    {
                        player2Value += ((int)element.Value).ToString() + element.Suit + " ";
                    }


                    if (player1Combination > player2Combination)
                    {
                        player1WinningHands++;
                    }
                    else if (player1Combination < player2Combination)
                    {
                        player2WinningHands++;
                    }
                    else
                    {

                        if (player1Evaluation.HandValues.Total > player2Evaluation.HandValues.Total)
                        {
                            player1WinningHands++;
                        }
                        else if (player1Evaluation.HandValues.Total < player2Evaluation.HandValues.Total)
                        {
                            player2WinningHands++;
                        }
                        else if (player1Evaluation.HandValues.HighCard > player2Evaluation.HandValues.HighCard)
                        {
                            player1WinningHands++;
                        }
                        else if (player1Evaluation.HandValues.HighCard < player2Evaluation.HandValues.HighCard)
                        {
                            player2WinningHands++;
                        }
                        else
                        {
                            EvaluateHighCard(4);// Evaluate From last to determine winner.
                        }
                    }
                    if(log)
                    {
                        //Console.WriteLine(player1Value + " " + player2Value + " " + player1Combination + " " + player2Combination + " " + player1Evaluation.HandValues.Total + " " + player2Evaluation.HandValues.Total + " " + player1Evaluation.HandValues.HighCard + " " + player2Evaluation.HandValues.HighCard + " " + player1WinningHands + " " + player2WinningHands);
                        WriteLog("-----------------------------------------------------------------------------------------------");
                        WriteLog("Player1 Value: " + player1Value + " Player2 Value: " + player2Value);
                        WriteLog("Combination: " + player1Combination + "            Combination: " + player2Combination);
                    }
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine("Error occured in EvaluatePlayerCards " + ex.Message);
            }
       }   
        
        private void WriteLog(string logText)
        {

            var logFilePath = ConfigurationManager.AppSettings["LogFilePath"];
            File.AppendAllLines(logFilePath,new string[] { logText });
                
                        
        }
    }
}
