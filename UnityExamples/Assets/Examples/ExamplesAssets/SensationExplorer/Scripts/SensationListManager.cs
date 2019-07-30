using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


namespace UltrahapticsCoreAsset.UnityExamples
{
    public class SensationListManager : MonoBehaviour
    {
        public List<GameObject> sensationRows;
        public RectTransform contentRect;

        public GameObject SensationRowPrefab;
        public SensationLibrary sensationLibrary;
        public SensationInputPropertyFactory inputPropertyFactory;
        public SensationSource activeSensation;

        public SensationPlaybackManager playbackManager;

        public Text activeSensationTopText;

        public string selectedSensationName;

        // Use this for initialization
        void Start()
        {
            if (sensationLibrary.sensationList.Count == 0)
            {
                sensationLibrary.BuildBlockLibrary();
            }
            RefreshSensationList();

            // TODO On Start, select the startup Sensation
            //ActivateSensationByName(startupSensationName);
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
            sensationRows.Clear();
            BuildSensationRows();
        }

        void BuildSensationRows()
        {

            GameObject row;
            foreach (string sensationName in sensationLibrary.sensationList)
            {
                row = (GameObject)Instantiate(SensationRowPrefab, contentRect.transform);
                var rowUI = row.GetComponent<SensationRowUI>();
                rowUI.SetSensationName(sensationName);
                sensationRows.Add(row);
                rowUI.button.onClick.AddListener(delegate { SensationSelected(sensationName); });
            }
        }

        // This should get called whenever the Button of the SensationRow is clicked.
        public void SensationSelected(string sensationName)
        {
            selectedSensationName = sensationName;
            activeSensationTopText.text = sensationName;
            activeSensation.SensationBlock = selectedSensationName;
            activeSensation.OnValidate();

            // Now pass the Sensation Source to the Sensation Input Property Factory
            inputPropertyFactory.SetSensationInputsFromSensation(activeSensation);

            playbackManager.ResetSensationToDefault();

            // If a Sensation is selected, set the playback state to true
            playbackManager.EnablePlayback(true);

        }

        // TODO: This method needs to activate the row item with a given 
        // Sensation name...
        public void ActivateSensationByName(string sensationName)
        {
            // Get index of sensationName in list
            int index = sensationLibrary.sensationList.ToList().IndexOf(sensationName);
            if (index > sensationRows.Count-1)
            {
                Debug.LogWarning("Unable to select sensation named: " + sensationName);
                return;
            }
            var activeRow = sensationRows[index];
            var rowUI = activeRow.GetComponent<SensationRowUI>();

            rowUI.button.onClick.Invoke();
            SensationSelected(sensationName);
        }
    }
}
