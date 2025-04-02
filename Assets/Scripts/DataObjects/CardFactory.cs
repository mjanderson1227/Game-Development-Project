using System;

namespace DataObjects
{
    public class CardFactory
    {
        private static readonly Array _suits = Enum.GetValues(typeof(Suit));
        private static readonly Random _random = new Random();
        private static readonly string[] _ranks = { "Ace", "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King" };

        public static Card CreateCard(Suit suit, string rank)
        {
            return new Card(suit, rank);
        }

        public static Card[] CreateDeck()
        {
            var deck = new Card[52];
            int index = 0;

            foreach (Suit suit in _suits)
            {
                foreach (string rank in _ranks)
                {
                    deck[index++] = CreateCard(suit, rank);
                }
            }

            return deck;
        }

        public static Card[] CreateShuffledDeck()
        {
            var deck = CreateDeck();
            for (int i = deck.Length - 1; i > 0; i--)
            {
                int j = _random.Next(i + 1);
                var temp = deck[i];
                deck[i] = deck[j];
                deck[j] = temp;
            }
            return deck;
        }
    }
}