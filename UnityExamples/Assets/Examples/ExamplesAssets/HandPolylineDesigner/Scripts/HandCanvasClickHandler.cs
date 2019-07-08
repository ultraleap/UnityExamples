using System.Linq;
using UnityEngine;
using UnityEngine.UI;
namespace UltrahapticsCoreAsset.Examples.PolylineHandPathDesigner
{
    public class HandCanvasClickHandler : MonoBehaviour
    {
        public Canvas myCanvas;
        public Polyline6DataSource polyline6DataSource;
        public Image dummyCursorImage;

        // When the mouse is clicked in the Canvas, we will optionally move the closest
        // Polyline6  Point List object to the Polyline6 Game Object closes to the click position
        void OnMouseDown()
        {

            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);

            var pointPos = myCanvas.transform.TransformPoint(pos);

            dummyCursorImage.transform.position = pointPos;

            var closestFeatureObject = FindClosestPolylineFeatureObjectToCursor();
            Debug.Log("Closet Object to click was:" + closestFeatureObject.name);
            if (polyline6DataSource.gameObjectFeatureList.Contains(closestFeatureObject))
            {
                Debug.Log(closestFeatureObject.name + " was found in the object feature list");
            }
            else
            {
                Debug.Log(closestFeatureObject.name + " NOT found in the object feature list");
                // Now find the closest Polyline6 Point to the Feature point!...
                var closestPolylinePoint = FindClosestPolylinePointToObject(closestFeatureObject);
                Debug.Log("Should now set the transform to be: " + closestFeatureObject.name);
                closestPolylinePoint.transform.position = closestFeatureObject.transform.position;
            }
        }

        // A method to find the closet GameObject to the dummy Mouse cursor
        private GameObject FindClosestPolylineFeatureObjectToCursor(string trgt = "Player")
        {
            UnityEngine.Vector3 position = dummyCursorImage.transform.position;
            return GameObject.FindGameObjectsWithTag(trgt)
                .OrderBy(o => (o.transform.position - position).sqrMagnitude)
                .FirstOrDefault();
        }

        // A method to find the closet GameObject to the dummy Mouse cursor
        private GameObject FindClosestPolylinePointToObject(GameObject featureObject)
        {
            UnityEngine.Vector3 position = featureObject.transform.position;
            return GameObject.FindGameObjectsWithTag("Finish")
                .OrderBy(o => (o.transform.position - position).sqrMagnitude)
                .FirstOrDefault();
        }
    }
}