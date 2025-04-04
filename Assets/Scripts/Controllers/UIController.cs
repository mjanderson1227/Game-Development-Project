using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace Controllers
{
    class UIController : MonoBehaviour
    {
        private UnityEvent<PlayerOption> onUserAction;

        public void UpdateScore(int newScore)
        {
            var scoreUI = GameObject.Find("ScoreDisplay").GetComponent<TextMeshProUGUI>();
            scoreUI.text = $"Score: {newScore}";
        }

        void Start()
        {
            if (onUserAction == null)
                onUserAction = new UnityEvent<PlayerOption>();

            var gameController = GameObject.Find("GameController").GetComponent<GameController>();

            onUserAction.AddListener(gameController.HandlePlayerChoice);

            var hitButton = GameObject.Find("HitButton").GetComponent<Button>();
            hitButton.onClick.AddListener(() => onUserAction.Invoke(PlayerOption.Hit));

            var standButton = GameObject.Find("StandButton").GetComponent<Button>();
            standButton.onClick.AddListener(() => onUserAction.Invoke(PlayerOption.Stand));
        }
    }
}