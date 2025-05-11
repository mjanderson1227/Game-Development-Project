using System.Collections.Generic;
using Assets.Scripts.DataObjects;
using Unity.VisualScripting;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Controllers
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
        private readonly int threshold = 21; // if both players bust then the threshold will increase
        private UIController uiController;
        private TableController tableController;
        private AudioController audioController;
        private Task CurrentDecision = Task.CompletedTask;
        private bool isBettingPhase = false; // TODO: If time permits, add a betting mechanic here.
        private int currentBet = 0;

        private PlayerOption AiMakeDecision()
        {
            var scoreDiff = threshold - p2.CalculateScore(threshold);

            if (scoreDiff > 6)
            {
                return PlayerOption.Hit;
            }

            return PlayerOption.Stand;
        }

        private void UpdateChips(int newChips)
        {
            p1.Chips = newChips;
            uiController.UpdateChips(newChips);
        }

        private async Task EndRound(RoundStatus roundStatus)
        {
            await tableController.ShowFaceDownCards();
            await Task.Delay(1000);

            switch (roundStatus)
            {
                case RoundStatus.Win:
                    audioController.WinSound();
                    gameOverScreen.Setup("You win!");
                    UpdateChips(p1.Chips + currentBet);
                    break;
                case RoundStatus.Lose:
                    audioController.LoseSound();
                    gameOverScreen.Setup("You lose.");
                    UpdateChips(p1.Chips - currentBet);
                    break;
                case RoundStatus.Tie:
                    audioController.WinSound();
                    gameOverScreen.Setup("Tie Game.");
                    break;
            }

            // if (p1.Chips <= 0)
            // {
            //     gameOverScreen.Setup("You lose.");
            // }
            // else if (p1.Chips >= 400)
            // {
            //     gameOverScreen.Setup("You win!");
            // }
        }

        private async Task<bool> ValidateRound()
        {
            var p1Score = p1.CalculateScore(threshold);
            var p2Score = p2.CalculateScore(threshold);

            // Check for explicit loss
            if (p1Score > threshold && p2Score <= threshold)
            {
                await tableController.EmitSmoke(p1);
                await EndRound(RoundStatus.Lose);
                return false;
            }
            else if (p1Score <= threshold && p2Score > threshold)
            {
                await tableController.EmitSmoke(p2);
                await EndRound(RoundStatus.Win);
                return false;
            }
            else if (p1Score > threshold && p2Score > threshold)
            {
                await tableController.EmitSmoke(p1);
                await tableController.EmitSmoke(p2);
                await EndRound(RoundStatus.Tie);
                return true;
            }

            // Find who has the higher score at the end
            if (p1.Idle && p2.Idle)
            {
                if (p1Score > p2Score)
                {
                    await EndRound(RoundStatus.Win);
                    return false;
                }
                else if (p2Score > p1Score)
                {
                    await EndRound(RoundStatus.Lose);
                    return false;
                }
                else
                {
                    await EndRound(RoundStatus.Tie);
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
                        audioController.CardFlipSound();
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
                        var card = deck.Pop();
                        p2.DealCard(card);
                        if (round == 0)
                        {
                            await tableController.DealCard(card, p2);
                            audioController.CardFlipSound();
                        }
                        else
                        {
                            await tableController.DealCardFaceDown(card, p2);
                            audioController.CardFlipSound();
                        }
                        break;
                    case PlayerOption.Stand:
                        p2.Idle = true;
                        break;
                }
            }

            round++;

            return await ValidateRound();
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
            if (isBettingPhase)
            {
                return;
            }

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
            audioController = GameObject.Find("SoundEffects").GetComponent<AudioController>();

            DoChoice(PlayerOption.Hit);
            audioController.HitOrStandSound();
        }
    }
}