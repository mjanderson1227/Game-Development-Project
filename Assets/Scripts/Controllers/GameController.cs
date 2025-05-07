using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DataObjects;
using UnityEditor.Rendering;
using UnityEngine;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
        private Stack<Card> deck;
        [SerializeField] private Vector3 p1Location;
        [SerializeField] private Vector3 p2Location;
        [SerializeField] private GameOverScreen gameOverScreen;
        private Player p1; // player
        private Player p2; // dealer
        private int round = 0;
        private int threshold = 21; // if both players bust then the threshold will increase
        private UIController uiController;
        private TableController tableController;
        private Task CurrentDecision = Task.CompletedTask;

        private PlayerOption AiMakeDecision()
        {
            var scoreDiff = threshold - p2.CalculateScore(threshold);

            if (scoreDiff > 6)
            {
                return PlayerOption.Hit;
            }

            return PlayerOption.Stand;
        }

        private async void WinGame()
        {
            await tableController.ShowFaceDownCards();
            await Task.Delay(1000);
            gameOverScreen.Setup("You win!");
        }

        private async void LoseGame()
        {
            await tableController.ShowFaceDownCards();
            await Task.Delay(1000);
            gameOverScreen.Setup("You lose.");
        }

        private bool ValidateRound()
        {
            var p1Score = p1.CalculateScore(threshold);
            var p2Score = p2.CalculateScore(threshold);

            // Check for explicit loss
            if (p1Score > threshold && p2Score <= threshold)
            {
                LoseGame();
                return false;
            }
            else if (p1Score <= threshold && p2Score > threshold)
            {
                WinGame();
                return false;
            }
            else if (p1Score > threshold && p2Score > threshold)
            {
                threshold *= 2;
                return true;
            }

            // Find who has the higher score at the end
            if (p1.Idle && p2.Idle)
            {
                if (p1Score > p2Score)
                {
                    WinGame();
                    return false;
                }
                else if (p2Score > p1Score)
                {
                    LoseGame();
                    return false;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }


        public async Task<bool> PlayRound(PlayerOption playerOption)
        {
            if (!p1.Idle)
            {
                switch (playerOption)
                {
                    case PlayerOption.Hit:
                        var card = deck.Pop();
                        p1.DealCard(card);
                        await tableController.DealCard(card, p1);
                        uiController.UpdateScore(p1.CalculateScore(threshold));
                        break;
                    case PlayerOption.Stand:
                        p1.Idle = true;
                        break;
                }
            }

            if (!p2.Idle)
            {
                var decision = AiMakeDecision();
                switch (decision)
                {
                    case PlayerOption.Hit:
                        Debug.Log("HIT");
                        var card = deck.Pop();
                        p2.DealCard(card);
                        if (round == 0)
                        {
                            await tableController.DealCard(card, p2);
                        }
                        else
                        {
                            await tableController.DealCardFaceDown(card, p2);
                        }
                        break;
                    case PlayerOption.Stand:
                        Debug.Log("STAND");
                        p2.Idle = true;
                        break;
                }
            }

            round++;

            return ValidateRound();
        }

        public void DoChoice(PlayerOption playerOption)
        {
            if (CurrentDecision.IsCompleted)
            {
                CurrentDecision = HandlePlayerChoice(playerOption);
            }
        }

        public async Task HandlePlayerChoice(PlayerOption playerOption)
        {
            var shouldContinue = await PlayRound(playerOption);
            if (shouldContinue && !p1.Idle)
            {
                return;
            }

            while (shouldContinue && !p2.Idle)
            {
                shouldContinue = await PlayRound(PlayerOption.Stand);
                await Task.Delay(300);
            }
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            uiController = GameObject.Find("PlayerUI").GetComponent<UIController>();
            p1 = new(p1Location);
            p2 = new(p2Location);
            deck = new(CardFactory.CreateShuffledDeck());
            tableController = GameObject.Find("Table").GetComponent<TableController>();

            _ = PlayRound(PlayerOption.Hit);
        }
    }
}