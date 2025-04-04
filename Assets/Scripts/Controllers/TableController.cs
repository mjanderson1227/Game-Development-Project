using System.Collections;
using System.Threading.Tasks;
using DataObjects;
using UnityEngine;

namespace Controllers
{
    class TableController : MonoBehaviour
    {
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
        }

        public async Task DealCardFaceDown(Card card, Player player)
        {
            var pos = CalculateOffset();
            var cardModel = card.LoadModel();
            var instantiatedCard = Instantiate(cardModel, pos, Quaternion.Euler(0, 0, 180));
            instantiatedCard.transform.localScale = new Vector3(5f, 1f, 5f);

            await ObjMoveFromTo(instantiatedCard, pos, player.Location, 0.5f);
            player.Location += Vector3.right * 0.4f;
        }
    }
}