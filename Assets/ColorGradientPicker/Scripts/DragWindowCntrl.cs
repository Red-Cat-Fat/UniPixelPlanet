using UnityEngine;

namespace ColorGradientPicker.Scripts
{
    public class DragWindowCntrl : MonoBehaviour
    {
        private RectTransform _window;
        //delta drag
        private Vector2 _delta;

        private void Awake()
        {
            _window = (RectTransform)transform;
        }

        public void BeginDrag()
        {
            _delta = Input.mousePosition - _window.position;
        }
        public void Drag()
        {
            Vector2 newPos = (Vector2)Input.mousePosition - _delta;
            Vector2 transform = new Vector2(_window.rect.width * ((Component)this).transform.root.lossyScale.x, _window.rect.height * ((Component)this).transform.root.lossyScale.y);
            Vector2 offsetMin, offsetMax;
            offsetMin.x = newPos.x - _window.pivot.x * transform.x;
            offsetMin.y = newPos.y - _window.pivot.y * transform.y;
            offsetMax.x = newPos.x + (1 - _window.pivot.x) * transform.x;
            offsetMax.y = newPos.y + (1 - _window.pivot.y) * transform.y;
            if (offsetMin.x < 0)
            {
                newPos.x = _window.pivot.x * transform.x;
            }
            else if (offsetMax.x > Screen.width)
            {
                newPos.x = Screen.width - (1 - _window.pivot.x) * transform.x;
            }
            if (offsetMin.y < 0)
            {
                newPos.y = _window.pivot.y * transform.y;
            }
            else if (offsetMax.y > Screen.height)
            {
                newPos.y = Screen.height - (1 - _window.pivot.y) * transform.y;
            }
            _window.position = newPos;
        }
    }
}
