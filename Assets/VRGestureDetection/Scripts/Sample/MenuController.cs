using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRGestureDetection.Core;
using UnityEngine.UI;

namespace VRGestureDetection.Sample
{
    public class MenuController : MonoBehaviour
    {
        public GridLayoutGroup gridLayoutGroup;
        public RectTransform cursor;
        private GestureDetection gestureDetection;
        private Vector2Int currentCursorPosition;
        private bool canMove = true;
        public float moveCooldown = 0.5f;

        void Start()
        {
            gestureDetection ??= GestureDetection.Instance;
            LoadControlGestures();
            currentCursorPosition = new Vector2Int(2, 2);
            UpdateCursorPosition();
        }

        void LoadControlGestures()
        {
            // swiping right
            AddGestureEvent("left hand swipe R", () => MoveMenu(new Vector2Int(1, 0)));
            AddGestureEvent("right hand swipe R", () => MoveMenu(new Vector2Int(1, 0)));

            // swiping left
            AddGestureEvent("left hand swipe L", () => MoveMenu(new Vector2Int(-1, 0)));
            AddGestureEvent("right hand swipe L", () => MoveMenu(new Vector2Int(-1, 0)));

            // swiping up
            AddGestureEvent("left hand swipe up", () => MoveMenu(new Vector2Int(0, -1)));
            AddGestureEvent("right hand swipe up", () => MoveMenu(new Vector2Int(0, -1)));

            // swiping down
            AddGestureEvent("left hand swipe down", () => MoveMenu(new Vector2Int(0, 1)));
            AddGestureEvent("right hand swipe down", () => MoveMenu(new Vector2Int(0, 1)));
        }

        void AddGestureEvent(string gestureName, UnityEngine.Events.UnityAction action)
        {
            var gestureEvent = new GestureEvent { gestureName = gestureName };
            gestureEvent.onGestureDetected = new UnityEngine.Events.UnityEvent();
            gestureEvent.onGestureDetected.AddListener(action);
            gestureDetection.AddGestureEvent(gestureEvent);
        }

        void MoveMenu(Vector2Int direction)
        {
            if (!canMove) return;

            Vector2Int newPosition = currentCursorPosition + direction;

            // Ensure the new position is within the bounds of the grid
            newPosition.x = Mathf.Clamp(newPosition.x, 0, 4);
            newPosition.y = Mathf.Clamp(newPosition.y, 0, 4);

            if (newPosition != currentCursorPosition)
            {
                currentCursorPosition = newPosition;
                UpdateCursorPosition();
                StartCoroutine(CooldownCoroutine());
            }
            cursor.gameObject.SetActive(true);
        }

        void UpdateCursorPosition()
        {
            // Calculate the new anchored position for the cursor
            int index = currentCursorPosition.y * 5 + currentCursorPosition.x;
            RectTransform cellRect = gridLayoutGroup.transform.GetChild(index).GetComponent<RectTransform>();

            // Adjust the cursor position to the top-left corner
            RectTransform gridRect = gridLayoutGroup.GetComponent<RectTransform>();
            float offsetX = gridRect.rect.width / 2f;
            float offsetY = gridRect.rect.height / 2f;

            cursor.anchoredPosition = cellRect.anchoredPosition - new Vector2(offsetX, offsetY * -1f);
        }

        IEnumerator CooldownCoroutine()
        {
            canMove = false;
            yield return new WaitForSeconds(moveCooldown);
            canMove = true;
        }
    }
}
