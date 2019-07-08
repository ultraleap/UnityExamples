using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UltrahapticsCoreAsset.UnityExamples
{
    public class SensationListManager : MonoBehaviour
    {

        public RectTransform contentRect;
        public GameObject SensationRowPrefab;
        public List<string> sensationList = null;
        public List<GameObject> sensationRows = null;
        public SensationInputPropertyFactory inputPropertyFactory;
        public SensationSource activeSensation;
        public string selectedSensationName;

        // Use this for initialization
        void Start()
        {
            RefreshSensationList();
        }

        // Used to remove all of the Child Game Objects in Content GameObject.
        public void ClearInputGameObjects()
        {
            foreach (Transform child in contentRect.gameObject.transform)
            {
                Destroy(child.gameObject);
            }
        }

        public void RefreshSensationList()
        {
            BuildSensationList();
            PopulateSensationRows();
        }

        void BuildSensationList()
        {
            sensationList.Clear();
            List<string> sensationNames = SensationCore.Instance.GetSensationProducingBlockNames();
            var sortedSensations = sensationNames.OrderBy(s => s);
            sensationList.AddRange(sortedSensations);
        }

        void PopulateSensationRows()
        {
            GameObject row;
            for (int i=0; i < sensationList.Count; i++)
            {
                row = (GameObject)Instantiate(SensationRowPrefab, contentRect.transform);
                var rowUI = row.GetComponent<SensationRowUI>();
                var sensationName = sensationList[i];
                rowUI.SetSensationName(sensationName);
                sensationRows.Add(row);
                rowUI.button.onClick.AddListener(delegate {SensationSelected(sensationName);});
            }
        }

        // This should get called whenever the Button of the SensationRow is clicked.
        public void SensationSelected(string sensationName)
        {

            activeSensation.SensationBlock = sensationName;
            activeSensation.OnValidate();
            activeSensation.enabled = false;
            activeSensation.Running = true;

            // Now pass the Sensation Source to the Sensation Input Property Factory
            inputPropertyFactory.SetSensationInputsFromSensation(activeSensation);
        }
    }
}
