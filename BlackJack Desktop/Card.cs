using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_Desktop
{
    //internal class Card
    class Card
    {
        public string name { get; set; }
        public Suit suit { get; set; }
        public Rank rank { get; set; }
        
        public enum Suit
        {
            Clubs,
            Diamonds,
            Hearts,
            Spades
        }
        public enum Rank
        {
            Ace = 11,
            Two = 2,
            Three = 3,
            Four = 4,
            Five = 5,
            Six = 6,
            Seven = 7,
            Eight = 8,
            Nine = 9,
            Ten = 10,
            Jack = 10,
            Queen = 10,
            King = 10
        }

        public Card(Suit suit, Rank rank)
        {
            this.suit = suit;
            this.rank = rank;
            this.name = setName();
        }
        public string setName()
        {
            return rank + " of " + suit;
        }

        public void printName()
        {
            Console.Write(name);
        }
    }
}
