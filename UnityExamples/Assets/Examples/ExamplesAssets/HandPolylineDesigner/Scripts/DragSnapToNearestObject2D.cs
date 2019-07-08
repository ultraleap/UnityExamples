using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UltrahapticsCoreAsset.Examples.PolylineHandPathDesigner
{
    public class DragSnapToNearestObject2D : MonoBehaviour
    {
        public Canvas myCanvas;
        public RectTransform constrainRectangle;
        public Image image;
        public Sprite defaultSprite;
        public Sprite draggingSprite;
        public GameObject nearestObject;
        public Polyline6DataSource polylineFeatureUpdater;
        public int pointIndex;

        void OnMouseDrag()
        {
            // When the Drag starts, update the image to draggingSprite
            image.sprite = draggingSprite;

            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);

            var pointPos = myCanvas.transform.TransformPoint(pos);

            // We constrain the object to only move within a defined rect
            if (RectTransformUtility.RectangleContainsScreenPoint(constrainRectangle, pointPos) == false)
            {
                return;
            }
            transform.position = pointPos;
        }

        void OnMouseUp()
        {
            // On Mouse up, return the image to the default sprite
            image.sprite = defaultSprite;

            // Find which is the closet object and then snap to it
            var nearest = FindClosestGameObject();
            nearestObject = nearest;
            transform.position = nearestObject.transform.position;

            polylineFeatureUpdater.gameObjectFeatureList[pointIndex] = nearestObject;
        }

        // A method to find the closet GameObject to this transfrom, with a given tag.
        private GameObject FindClosestGameObject(string trgt = "Player")
        {
            UnityEngine.Vector3 position = transform.position;
            return GameObject.FindGameObjectsWithTag(trgt)
                .OrderBy(o => (o.transform.position - position).sqrMagnitude)
                .FirstOrDefault();
        }
    }
}