using System.Collections.Generic;
using DataObjects;
using UnityEngine;

public class Player
{
    private int _score = 0;
    private List<Card> _hand = new();
    private Vector3 _location;
    public Player(Vector3 location) => this._location = location;
    private bool _idle = false;

    public void DealCard(Card card)
    {
        _hand.Add(card);
        _score += card.Value;
    }

    public int Score => _score;
    public Vector3 Location => Location;
    public bool Idle
    {
        get;
        set;
    }
}