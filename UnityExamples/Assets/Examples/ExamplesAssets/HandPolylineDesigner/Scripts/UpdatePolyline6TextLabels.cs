using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UltrahapticsCoreAsset.Examples.PolylineHandPathDesigner
{
    public class UpdatePolyline6TextLabels : MonoBehaviour
    {

        public Polyline6DataSource polyline6DataSource;
        public List<Text> pointLabelList = new List<Text>(6);

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i <= pointLabelList.Count-1; i++)
            {
                pointLabelList[i].text = "feature" + i + ": " + polyline6DataSource.gameObjectFeatureList[i].name;
            }
        }
    }
}
