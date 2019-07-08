using UnityEngine;

namespace UltrahapticsCoreAsset.Examples.PolylineHandPathDesigner
{
    public class PolylineHandPathRenderer : MonoBehaviour
    {

        public GameObject point0;
        public GameObject point1;
        public GameObject point2;
        public GameObject point3;
        public GameObject point4;
        public GameObject point5;
        public LineRenderer lineRenderer;

        // Use this for initialization
        void Start()
        {
            lineRenderer.positionCount = 6;
        }

        private UnityEngine.Vector3[] PolylinePoints()
        {
            UnityEngine.Vector3[] points = new[] {
            point0.transform.position,
            point1.transform.position,
            point2.transform.position,
            point3.transform.position,
            point4.transform.position,
            point5.transform.position
        };
            return points;
        }

        // Update is called once per frame
        void Update()
        {
            lineRenderer.SetPositions(PolylinePoints());
        }
    }
}