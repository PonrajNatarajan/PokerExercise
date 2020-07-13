using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;

/// <summary>
/// EvalutionBase class which comprises of method to evaluate each combination
/// </summary>
namespace PokerGame
{
    /// <summary>
    /// Enum define type of combinations from low to high
    /// </summary>
    public enum combination
    {
        Highcard,
        Pair,
        TwoPair,
        Threeofkind,
        Straight,
        Flush,
        Fullhouse,
        FourofKind,
        StraightFlush,
        RoyalFlush

    }

    /// <summary>
    /// Structure defines total and highcard value for the Combinations
    /// </summary>
    public struct HandValue
    {
        public int Total { get; set; }
        public int HighCard { get; set; }
    }

    /// <summary>
    /// Evaluation is base class,inherits from card
    /// </summary>
    class EvaluationBase:Card
    {
        private int totalHeartSuit;
        private int totalDiamondSuit;
        private int totalClubSuit;
        private int totalSpadeSuit;
        private int totalRoyalSuit;
        private Card[] cards;
        private HandValue handValue;
        
        /// <summary>
        /// Constructor initialize attributes of the class
        /// </summary>
        /// <param name="sortedCardFromPlayer">Sorter array of cards for palyer</param>
        public EvaluationBase(Card[] sortedCardFromPlayer)
        {
            totalHeartSuit = 0;
            totalDiamondSuit = 0;
            totalClubSuit = 0;
            totalSpadeSuit = 0;
            totalRoyalSuit = 0;
            cards = new Card[5];
            Cards = sortedCardFromPlayer;
            handValue = new HandValue();
        }

        /// <summary>
        /// Defines the handValue for the combination
        /// </summary>
        public HandValue HandValues
        {
            get { return handValue; }
            set { handValue = value; }
        }


        /// <summary>
        /// Get/Set Card Values
        /// </summary>
        public Card[] Cards
        {
            get { return cards; }
            set
            {
                cards[0] = value[0];
                cards[1] = value[1];
                cards[2] = value[2];
                cards[3] = value[3];
                cards[4] = value[4];
                cards[0] = value[0];
            }
        }

        /// <summary>
        ///Method evaluate PlayerHand to identify type of combination
        /// </summary>
        /// <returns></returns>
        public combination EvaluatePlayerHand()
        {
            EvaluateNumberofSuit();
            if(RoyalFlush())
                return combination.RoyalFlush;
            else if (StraightFlush())
                return combination.StraightFlush;
            else if (FourOfKind())
                return combination.FourofKind;
            else if (Fullhouse())
                return combination.Fullhouse;
            else if (Flush())
                return combination.Flush;
            else if (Straight())
                return combination.Straight;
            else if (ThreeOfKind())
                return combination.Threeofkind;
            else if (TwoPairs())
                return combination.TwoPair;
            else if (Pair())
                return combination.Pair;

            handValue.HighCard = (int)cards[4].Value;

            return combination.Highcard;

       }

        /// <summary>
        /// Used to calculate Suit with in the Player's combination
        /// Count of 5, indicates all in same suit
        /// </summary>
        public void EvaluateNumberofSuit()
        {

            foreach(var element in Cards)
            {
                if (element.Suit == Card.CARDSUIT.C)
                    totalClubSuit++;
                else if (element.Suit == Card.CARDSUIT.D)
                    totalDiamondSuit++;
                else if (element.Suit == Card.CARDSUIT.H)
                    totalHeartSuit++;
                else if (element.Suit == Card.CARDSUIT.S)
                    totalSpadeSuit++;
                
            } 
       }


        /// <summary>
        /// Ten, Jack, Queen, King and Ace in the same suit
        /// </summary>
        /// <returns></returns>
        private bool RoyalFlush()
        {
            if (totalClubSuit == 5 || totalDiamondSuit == 5 || totalSpadeSuit == 5 || totalHeartSuit == 5)
            {
                foreach (var element in Cards)
                {
                    if (element.Value == Card.CARDVALUE.T)
                        totalRoyalSuit++;
                    else if (element.Value == Card.CARDVALUE.J)
                        totalRoyalSuit++;
                    else if (element.Value == Card.CARDVALUE.Q)
                        totalRoyalSuit++;
                    else if (element.Value == Card.CARDVALUE.K)
                        totalRoyalSuit++;
                    else if (element.Value == Card.CARDVALUE.A)
                        totalRoyalSuit++;

                }

                if (totalRoyalSuit == 5)
                    return true;
                else
                    return false;
            }

            return false;

        }

        /// <summary>
        /// All five cards in consecutive value order, with the same suit
        /// </summary>
        /// <returns></returns>
        private bool StraightFlush()
        {
            if (Flush() && Straight())
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Four cards of the same value
        /// </summary>
        /// <returns></returns>
        private bool FourOfKind()
        { 
            if(cards[0].Value== cards[1].Value && cards[0].Value == cards[2].Value && cards[0].Value == cards[3].Value && cards[0].Value == cards[4].Value)
            {
                handValue.Total = (int)cards[1].Value * 4;
                handValue.HighCard = (int)cards[4].Value;
                return true;
            }
            else if (cards[1].Value == cards[2].Value && cards[1].Value == cards[3].Value && cards[1].Value == cards[4].Value )
            {
                handValue.Total = (int)cards[1].Value * 4;
                handValue.HighCard = (int)cards[0].Value;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Three of a kind and a Pair
        /// </summary>
        /// <returns></returns>
        private bool Fullhouse()
        {

            if (cards[0].Value == cards[1].Value && cards[0].Value == cards[2].Value && cards[3].Value==cards[4].Value)
            {
                handValue.Total = (int)cards[0].Value*3;

                return true;
            }
            else if(cards[0].Value == cards[1].Value && cards[2].Value == cards[3].Value && cards[2].Value == cards[4].Value)
            {
                handValue.Total = (int)cards[2].Value *3 ;
                return true;
            }
            return false;
        }

        /// <summary>
        /// All five cards having the same suit
        /// </summary>
        /// <returns></returns>
        private bool Flush()
        {
            if (totalClubSuit == 5 || totalHeartSuit == 5 || totalDiamondSuit == 5 || totalSpadeSuit == 5)
            {
                handValue.Total = (int)cards[4].Value;
                return true;
            }
            return false;
        }

        /// <summary>
        /// All five cards in consecutive value order
        /// </summary>
        /// <returns></returns>
        private bool Straight()
        {
            if(cards[4].Value-1==Cards[3].Value&& cards[3].Value - 1 == Cards[2].Value&& cards[2].Value - 1 == Cards[1].Value&& cards[1].Value - 1 == Cards[0].Value)
            {
                handValue.Total = (int)cards[4].Value;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Three cards of the same value
        /// </summary>
        /// <returns></returns>
        private bool ThreeOfKind()
        {
            if((cards[0].Value == Cards[1].Value && cards[0].Value == Cards[2].Value)||(cards[1].Value==cards[2].Value && cards[1].Value==cards[3].Value))
            {
                handValue.Total = (int)cards[2].Value * 3;
                handValue.HighCard = (int)cards[4].Value;
                return true;
            }
            else if(cards[2].Value==cards[3].Value&&cards[2].Value==cards[4].Value)
            {
                handValue.Total = (int)cards[2].Value * 3;
                handValue.HighCard = (int)cards[4].Value;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Two different pairs
        /// </summary>
        /// <returns></returns>
        private bool TwoPairs()
        {
            if(cards[0].Value==cards[1].Value && cards[2].Value == cards[3].Value)
            {
                handValue.Total = ((int)cards[1].Value * 2) + ((int)cards[3].Value * 2);
                handValue.HighCard = (int)cards[4].Value;
                return true;

            }
            else if(cards[1].Value == cards[2].Value && cards[3].Value == cards[4].Value)
            {
                handValue.Total = ((int)cards[1].Value * 2) + ((int)cards[3].Value * 2);
                handValue.HighCard= (int)cards[0].Value;
                return true;
            }
            else if(cards[0].Value == cards[1].Value && cards[3].Value == cards[4].Value)
            {
                handValue.Total = ((int)cards[1].Value * 2) + ((int)cards[3].Value * 2);
                handValue.HighCard = (int)cards[2].Value;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Two cards of same value
        /// </summary>
        /// <returns></returns>
        private bool Pair()
        {
            if (cards[0].Value == cards[1].Value)
            {
                handValue.Total = (int)cards[0].Value * 2;
                handValue.HighCard = (int)cards[4].Value;
                return true;
            }
            else if (cards[1].Value == cards[2].Value)
            {
                handValue.Total = (int)cards[1].Value * 2;
                handValue.HighCard = (int)cards[4].Value;
                    return true;
            }
            else if (cards[2].Value == cards[3].Value)
            {
                handValue.Total = (int)cards[2].Value * 2;
                handValue.HighCard = (int)cards[4].Value;
                    return true;
            }
            else if (cards[3].Value == cards[4].Value)
            {
                handValue.Total = (int)cards[3].Value * 2;
                handValue.HighCard = (int)cards[4].Value;
                return true;
            }
            return false;
        }    

    }
}
