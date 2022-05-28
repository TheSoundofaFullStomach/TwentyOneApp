using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_Desktop
{
    class Deck
    {
        public List<Card> cards { get; set; }

        public Deck()
        {
            cards = new List<Card>();

            foreach (Card.Suit suit in (Card.Suit[]) Enum.GetValues(typeof(Card.Suit)))
            {
                foreach (Card.Rank rank in (Card.Rank[]) Enum.GetValues(typeof(Card.Rank)))
                {
                    Card newCard = new Card(suit, rank);
                    cards.Add(newCard);
                }
            }
        }
    }
}
