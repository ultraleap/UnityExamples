using UnityEngine;

namespace UltrahapticsCoreAsset.Examples.Polyline
{
    public class PolylineRenderer : MonoBehaviour
    {

        public RectTransform Point0;
        public RectTransform Point1;
        public RectTransform Point2;
        public RectTransform Point3;
        public RectTransform Point4;
        public RectTransform Point5;
        public LineRenderer LineRenderer;
        public SensationSource Sensation;
        private float scaling_ = 3000.0f;


        // Use this for initialization
        void Start()
        {
            LineRenderer.positionCount = 6;
            Point0.localPosition = Sensation.Inputs["point0"].Value * scaling_;
            Point1.localPosition = Sensation.Inputs["point1"].Value * scaling_;
            Point2.localPosition = Sensation.Inputs["point2"].Value * scaling_;
            Point3.localPosition = Sensation.Inputs["point3"].Value * scaling_;
            Point4.localPosition = Sensation.Inputs["point4"].Value * scaling_;
            Point5.localPosition = Sensation.Inputs["point5"].Value * scaling_;
        }

        private UnityEngine.Vector3[] PolylinePoints()
        {
            UnityEngine.Vector3[] points = new[] {
                Point0.position,
                Point1.position,
                Point2.position,
                Point3.position,
                Point4.position,
                Point5.position
            };
            return points;
        }

        // Update is called once per frame
        void Update()
        {
            LineRenderer.SetPositions(PolylinePoints());
            Sensation.Inputs["point0"].Value = Point0.localPosition / scaling_;
            Sensation.Inputs["point1"].Value = Point1.localPosition / scaling_;
            Sensation.Inputs["point2"].Value = Point2.localPosition / scaling_;
            Sensation.Inputs["point3"].Value = Point3.localPosition / scaling_;
            Sensation.Inputs["point4"].Value = Point4.localPosition / scaling_;
            Sensation.Inputs["point5"].Value = Point5.localPosition / scaling_;
        }

        public void SetLine()
        {
            Point0.localPosition = new UnityEngine.Vector3(-0.04f, 0.0f, 0.0f);
            Point1.localPosition = new UnityEngine.Vector3(0.04f, 0.0f, 0.0f);
            Point2.localPosition = new UnityEngine.Vector3(0.04f, 0.0f, 0.0f);
            Point3.localPosition = new UnityEngine.Vector3(0.04f, 0.0f, 0.0f);
            Point4.localPosition = new UnityEngine.Vector3(0.04f, 0.0f, 0.0f);
            Point5.localPosition = new UnityEngine.Vector3(0.04f, 0.0f, 0.0f);

            ScalePoints();
        }

        public void SetTriangle()
        {
            Point0.localPosition = new UnityEngine.Vector3(0.0f, 0.025f, 0.0f);
            Point1.localPosition = new UnityEngine.Vector3(0.025f, -0.02f, 0.0f);
            Point2.localPosition = new UnityEngine.Vector3(-0.025f, -0.02f, 0.0f);
            Point3.localPosition = new UnityEngine.Vector3(0.0f, 0.025f, 0.0f);
            Point4.localPosition = new UnityEngine.Vector3(0.0f, 0.025f, 0.0f);
            Point5.localPosition = new UnityEngine.Vector3(0.0f, 0.025f, 0.0f);

            ScalePoints();
        }

        public void SetSquare()
        {
            Point0.localPosition = new UnityEngine.Vector3(-0.025f, 0.025f, 0.0f);
            Point1.localPosition = new UnityEngine.Vector3(0.025f, 0.025f, 0.0f);
            Point2.localPosition = new UnityEngine.Vector3(0.025f, -0.025f, 0.0f);
            Point3.localPosition = new UnityEngine.Vector3(-0.025f, -0.025f, 0.0f);
            Point4.localPosition = new UnityEngine.Vector3(-0.025f, 0.025f, 0.0f);
            Point5.localPosition = new UnityEngine.Vector3(-0.025f, 0.025f, 0.0f);

            ScalePoints();
        }

        public void SetPentagon()
        {
            Point0.localPosition = new UnityEngine.Vector3(0.0f, 0.025f, 0.0f);
            Point1.localPosition = new UnityEngine.Vector3(0.024f, 0.008f, 0.0f);
            Point2.localPosition = new UnityEngine.Vector3(0.015f, -0.02f, 0.0f);
            Point3.localPosition = new UnityEngine.Vector3(-0.015f, -0.02f, 0.0f);
            Point4.localPosition = new UnityEngine.Vector3(-0.024f, 0.008f, 0.0f);
            Point5.localPosition = new UnityEngine.Vector3(0.0f, 0.025f, 0.0f);

            ScalePoints();
        }

        private void ScalePoints()
        {
            Point0.localPosition *= scaling_;
            Point1.localPosition *= scaling_;
            Point2.localPosition *= scaling_;
            Point3.localPosition *= scaling_;
            Point4.localPosition *= scaling_;
            Point5.localPosition *= scaling_;
        }

    }
}
