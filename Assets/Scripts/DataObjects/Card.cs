using System;
using UnityEngine;

namespace DataObjects
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

        private int CalculateValue()
        {
            // Calculate the card's value according to blackjack rules
            if (Rank == "Jack" || Rank == "Queen" || Rank == "King")
            {
                return 10;
            }
            else if (Rank == "Ace")
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
            return Resources.Load<GameObject>($"{Suit.ToString()}/{Rank}");
        }
    }
}