using System.Collections.Generic;
using DataObjects;
using UnityEngine;

public class Player
{
    private List<Card> _hand = new();
    public Player(Vector3 location)
    {
        Location = location;
        Idle = false;
    }

    public void DealCard(Card card)
    {
        _hand.Add(card);
    }

    // Get the score of the player.  Use the threshold in order to determine what the value of ace is going to be.
    public int CalculateScore(int threshold)
    {
        int totalScore = 0;
        int aceCount = 0;

        // First, calculate score for non-ace cards
        foreach (var card in _hand)
        {
            if (!card.IsAce())
            {
                totalScore += card.Value;
            }
            else
            {
                aceCount++;
            }
        }

        // Then handle aces optimally
        for (int i = 0; i < aceCount; i++)
        {
            // If adding 11 would exceed threshold, use 1 instead
            if (totalScore + 11 <= threshold)
            {
                totalScore += 11;
            }
            else
            {
                totalScore += 1;
            }
        }

        return totalScore;
    }
    public Vector3 Location
    {
        get;
        set;
    }
    public bool Idle
    {
        get;
        set;
    }
}