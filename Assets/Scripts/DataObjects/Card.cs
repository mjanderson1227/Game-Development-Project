using UnityEngine;

namespace Assets.Scripts.DataObjects
{
    public enum Suit
    {
        Hearts,
        Diamonds,
        Clubs,
        Spades
    }

    public class Card
    {
        public Suit Suit { get; private set; }
        public string Rank { get; private set; }
        private readonly int _value;

        public Card(Suit suit, string rank)
        {
            Suit = suit;
            Rank = rank;
            _value = CalculateValue();
        }

        public bool IsAce()
        {
            return Rank == "A";
        }

        private int CalculateValue()
        {
            // Calculate the card's value according to blackjack rules
            if (Rank == "J" || Rank == "Q" || Rank == "K")
            {
                return 10;
            }
            else if (Rank == "A")
            {
                return 11; // Note: In blackjack, Ace can be 1 or 11
            }
            else
            {
                return int.Parse(Rank);
            }
        }

        public int Value => _value;

        public override string ToString()
        {
            return $"{Rank} of {Suit}";
        }

        public string ToDetailedString()
        {
            return $"Card(suit={Suit}, rank='{Rank}', value={_value})";
        }

        public GameObject LoadModel()
        {
            var parsedSuit = Suit.ToString().Substring(0, Suit.ToString().Length - 1);
            string modelPath = $"Cards/{Suit}/{parsedSuit}_{Rank}";
            GameObject cardModel = Resources.Load<GameObject>(modelPath);

            if (cardModel == null)
            {
                Debug.LogError($"Failed to load card model at path: {modelPath}. Make sure the model exists in the Resources folder.");
                return null;
            }

            return cardModel;
        }
    }
}