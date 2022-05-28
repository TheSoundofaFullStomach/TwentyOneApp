using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_Desktop
{
    class Shoe
    {
        public List<Card> cards { get; set; }
        public int decks { get; set; }
        public double targetPenetration { get; set; }
        public int cardsLeftInShoe { get; set; }

        private Deck referenceDeck;
        private int currentIndex;
        private int cutCardIndex;
        private int numCardsInDeck = 52;


        public Shoe(int decks, double targetPenetration)
        {
            this.decks = decks;
            this.targetPenetration = targetPenetration;
            this.cards = new List<Card>();
            this.currentIndex = 0;
            this.cardsLeftInShoe = numCardsInDeck - currentIndex;
            this.referenceDeck = new Deck();

            markCutCard();
            fillShoe();
            shuffleShoe();
        }

        public void markCutCard()
        {
            this.cutCardIndex = (int)Math.Round(52.0 * (double)decks * targetPenetration) + (new Random().Next(-5, 6));
            cutCardIndex = cutCardIndex > 0 ? cutCardIndex : 0;
            cutCardIndex = cutCardIndex < (numCardsInDeck * decks) ? cutCardIndex : numCardsInDeck * decks;

            if ((numCardsInDeck * decks) - cutCardIndex < 20)
            {
                UI.TargetPenetrationWarning();
            }
        }

        public void fillShoe()
        {
            foreach(Card card in referenceDeck.cards)
            {
                for (int i = 0; i < decks; i++)
                {
                    Card newCard = new Card(card.suit, card.rank);
                    cards.Add(newCard);
                }
            }
        }

        public void shuffleShoe()
        {
            currentIndex = 0;

            Random random = new Random();
            cards = cards.OrderBy(card => random.Next()).ToList();
        }

        public Card playCard()
        {
            if (currentIndex == numCardsInDeck * decks)
            {
                shuffleShoe();
                UI.EndOfShoe();
            }

            return cards[currentIndex++];
        }

        public bool pastCutCard()
        {
            return currentIndex > cutCardIndex;
        }
    }
}
