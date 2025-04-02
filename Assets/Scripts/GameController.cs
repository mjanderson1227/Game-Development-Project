using System;
using System.Collections.Generic;
using DataObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
        private Stack<Card> deck;
        [SerializeField] private Vector3 p1Location;
        [SerializeField] private Vector3 p2Location;
        private Player p1; // player
        private Player p2; // dealer
        private int round = 0;
        private int threshold = 21; // if both players bust then the threshold will increase
        private UIController uiController;

        private Vector3 CalculateOffset(int xOffset)
        {
            var center = this.transform.position;
            var translateUp = Vector3.up * (this.transform.localScale.y / 2);
            var topFaceCenter = center + translateUp;

            return topFaceCenter + Vector3.left * xOffset;
        }

        private PlayerOption AiMakeDecision()
        {
            var scoreDiff = threshold - p2.Score;

            // Simple scoring algorithm
            if (scoreDiff < 11)
            {
                return PlayerOption.Hit;
            }

            return PlayerOption.Stand;
        }

        private bool ValidateRound()
        {
            // Both players chose to stand check who wins based off of score
            if (p1.Idle && p2.Idle)
            {
                if (p1.Score > p2.Score)
                {
                    SceneManager.LoadScene("WinScene");
                    return false;
                }
                else if (p2.Score > p1.Score)
                {
                    SceneManager.LoadScene("LoseScene");
                    return false;
                }
            }

            if (p1.Score > threshold && p2.Score <= threshold)
            {
                SceneManager.LoadScene("LoseScreen");
                return false;
            }
            else if (p1.Score <= threshold && p2.Score > threshold)
            {
                SceneManager.LoadScene("WinScreen");
                return false;
            }
            else if (p1.Score > threshold && p2.Score > threshold)
            {
                threshold *= 2;
            }

            return true;
        }


        public bool PlayRound(PlayerOption playerOption)
        {
            if (!p1.Idle)
            {
                switch (playerOption)
                {
                    case PlayerOption.Hit:
                        var card = deck.Pop();
                        p1.DealCard(card);
                        uiController.UpdateScore(p1.Score);
                        break;
                    case PlayerOption.Stand:
                        p1.Idle = true;
                        break;
                }
            }

            if (!p2.Idle)
            {
                switch (AiMakeDecision())
                {
                    case PlayerOption.Hit:
                        var card = deck.Pop();
                        p2.DealCard(card);
                        break;
                    case PlayerOption.Stand:
                        p2.Idle = true;
                        break;
                }
            }

            round++;

            return ValidateRound();
        }

        public void HandlePlayerChoice(PlayerOption playerOption)
        {
            var shouldContinue = PlayRound(playerOption);
            while (shouldContinue)
            {
                PlayRound(PlayerOption.Stand);
            }
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            uiController = GameObject.Find("PlayerUI")?.GetComponent<UIController>();
            p1 = new(p1Location);
            p2 = new(p2Location);
            deck = new(CardFactory.CreateShuffledDeck());
        }
    }
}