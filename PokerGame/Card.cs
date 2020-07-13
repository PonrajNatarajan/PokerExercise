using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Class is used to define Card's Suit and Value as Enumeration
/// So that sorting and grouping can be done for different combinations
/// </summary>
namespace PokerGame
{
    class Card
    {
        public enum CARDSUIT
        {
            S,
            H,
            D,
            C
        }

       public enum CARDVALUE
        {
            Two = 2,
            three,
            four,
            five,
            six,
            seven,
            eight,
            nine,
            T,
            J,
            Q,
            K,
            A
        }
                
        public CARDSUIT Suit { get; set; }
        public CARDVALUE Value { get; set; }
    }
}

