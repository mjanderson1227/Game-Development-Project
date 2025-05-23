using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.DataObjects;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    class TableController : MonoBehaviour
    {
        [SerializeField] private GameObject smokePrefab;
        private List<GameObject> faceDownCards = new List<GameObject>();
        private List<GameObject> cards = new List<GameObject>();

        private async Task ObjMoveFromTo(GameObject obj, Vector3 start, Vector3 end, float duration)
        {
            float elapsed = 0;
            while (elapsed < duration)
            {
                obj.transform.position = Vector3.Lerp(start, end, elapsed / duration);
                elapsed += Time.deltaTime;
                await Task.Yield();
            }
            obj.transform.position = end;
        }
        private Vector3 CalculateOffset()
        {
            var center = transform.position;
            var translateUp = Vector3.up * (transform.localScale.y / 2);
            var topFaceCenter = center + translateUp;

            return topFaceCenter;
        }

        public async Task DealCard(Card card, Player player)
        {
            var pos = CalculateOffset(); // Calculate the position of the center of the table
            var cardModel = card.LoadModel();
            var instantiatedCard = Instantiate(cardModel, pos, Quaternion.identity);
            instantiatedCard.transform.localScale = new Vector3(5f, 1f, 5f);

            await ObjMoveFromTo(instantiatedCard, pos, player.Location, 0.5f);
            player.Location += Vector3.right * 0.4f;
            cards.Add(instantiatedCard);
        }

        public async Task DealCardFaceDown(Card card, Player player)
        {
            var pos = CalculateOffset();
            var cardModel = card.LoadModel();
            var instantiatedCard = Instantiate(cardModel, pos, Quaternion.Euler(0, 0, 180));
            instantiatedCard.transform.localScale = new Vector3(5f, 1f, 5f);

            await ObjMoveFromTo(instantiatedCard, pos, player.Location, 0.5f);
            player.Location += Vector3.right * 0.4f;
            faceDownCards.Add(instantiatedCard);
            cards.Add(instantiatedCard);
        }

        public async Task RotateObjectAsync(GameObject targetObject, float duration = 1.0f)
        {
            Quaternion startRotation = targetObject.transform.rotation;
            Quaternion targetRotation = startRotation * Quaternion.Euler(0f, 0f, 180f);

            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;

                float smoothT = Mathf.SmoothStep(0f, 1f, t);

                targetObject.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, smoothT);

                await Task.Yield();

                elapsedTime += Time.deltaTime;
            }

            targetObject.transform.rotation = targetRotation;
        }

        public void ClearTable()
        {
            foreach (var card in cards)
            {
                Destroy(card);
            }
        }

        public async Task ShowFaceDownCards()
        {
            foreach (var card in faceDownCards)
            {
                await RotateObjectAsync(card);
            }
        }

        public async Task EmitSmoke(Player p)
        {
            var location = p.Location;
            GameObject smoke = Instantiate(smokePrefab, location, Quaternion.Euler(-90, 0, 0));

            if (smoke.TryGetComponent<ParticleSystem>(out var particleSystem))
            {
                particleSystem.Play();

                var duration = particleSystem.main.duration + particleSystem.main.startLifetime.constantMax;

                await Task.Delay(Mathf.CeilToInt(duration * 1000));
                Destroy(smoke);
            }
        }
    }
}