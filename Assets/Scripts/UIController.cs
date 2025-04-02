using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using TMPro;

namespace Controllers
{
    class UIController : MonoBehaviour
    {
        private UnityEvent<PlayerOption> onUserAction;

        public void UpdateScore(int newScore)
        {
            var scoreUI = GameObject.Find("UI")?.GetComponent<TextMeshProUGUI>();
            scoreUI.text = newScore.ToString();
        }

        void Awake()
        {
            if (onUserAction == null)
                onUserAction = new UnityEvent<PlayerOption>();

            var gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

            onUserAction.AddListener(gameController.HandlePlayerChoice);

            var hitButton = GameObject.Find("HitButton")?.GetComponent<Button>();
            hitButton.clicked += () => onUserAction.Invoke(PlayerOption.Hit);

            var standButton = GameObject.Find("HitButton")?.GetComponent<Button>();
            standButton.clicked += () => onUserAction.Invoke(PlayerOption.Stand);
        }
    }
}