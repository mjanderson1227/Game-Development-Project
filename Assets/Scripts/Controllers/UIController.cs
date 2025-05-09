using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Assets.Scripts.DataObjects;
using TMPro;

namespace Assets.Scripts.Controllers
{
    class UIController : MonoBehaviour
    {
        private UnityEvent<PlayerOption> onUserAction;

        public void UpdateScore(int newScore)
        {
            var scoreUI = GameObject.Find("ScoreDisplay").GetComponent<TextMeshProUGUI>();
            scoreUI.text = $"Score: {newScore}";
        }

        public void UpdateChips(int newChips)
        {
            var chipsUI = GameObject.Find("ChipsDisplay").GetComponent<TextMeshProUGUI>();
            chipsUI.text = $"Chips: {newChips}";
        }

        void Start()
        {
            onUserAction ??= new UnityEvent<PlayerOption>();

            var gameController = GameObject.Find("GameController").GetComponent<GameController>();
            onUserAction.AddListener(gameController.DoChoice);

            var hitButton = GameObject.Find("HitButton").GetComponent<Button>();
            hitButton.onClick.AddListener(() => onUserAction.Invoke(PlayerOption.Hit));

            var standButton = GameObject.Find("StandButton").GetComponent<Button>();
            standButton.onClick.AddListener(() => onUserAction.Invoke(PlayerOption.Stand));
        }
    }
}