using UnityEngine;
using UnityEngine.UI;

namespace UltrahapticsCoreAsset.Examples.Polyline
{
    public class DragConstrained2D : MonoBehaviour
    {

        public Canvas MyCanvas;
        public RectTransform ConstrainRectangle;
        public Image Image;
        public Sprite DefaultSprite;
        public Sprite DraggingSprite;

        void OnMouseDrag()
        {
            // When the Drag starts, update the image to draggingSprite
            Image.sprite = DraggingSprite;

            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(MyCanvas.transform as RectTransform, Input.mousePosition, MyCanvas.worldCamera, out pos);

            var pointPos = MyCanvas.transform.TransformPoint(pos);

            // We constrain the object to only move within a defined rect
            if (RectTransformUtility.RectangleContainsScreenPoint(ConstrainRectangle, pointPos) == false)
            {
                return;
            }
            transform.position = pointPos;
        }

        void OnMouseUp()
        {
            // On Mouse up, return the image to the default sprite
            Image.sprite = DefaultSprite;
        }
    }
}
