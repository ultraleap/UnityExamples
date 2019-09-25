using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


namespace UltrahapticsCoreAsset.UnityExamples
{
    public class SensationListManager : MonoBehaviour
    {
        
        private SensationRowUI selectedSensationRowUI;

        public List<GameObject> sensationRows;
        public RectTransform contentRect;
        public GameObject SensationRowPrefab;
        public SensationLibrary sensationLibrary;
        public SensationInputPropertyFactory inputPropertyFactory;
        public SensationSource activeSensation;

        public SensationPlaybackManager playbackManager;

        public Text activeSensationTopText;

        public string selectedSensationName;
        public string startupSensationName = "CircleSensation";


        // Use this for initialization
        void Start()
        {
            if (sensationLibrary.sensationList.Count == 0)
            {
                sensationLibrary.BuildBlockLibrary();
            }
            RefreshSensationList();

            // Optionally, on Start, select the startup Sensation
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

        public void SelectNextSensation()
        {
            var sensationList = sensationLibrary.sensationList;
            if (selectedSensationRowUI != null)
            {           
                var selectedGameObject = selectedSensationRowUI.gameObject;
                var index = sensationRows.IndexOf(selectedGameObject);

                if (index < 0)
                {
                    index = 0;
                }
                else if (index > sensationList.Count - 2)
                {
                    index = sensationList.Count - 1;
                }
                else
                {
                    index += 1; 
                }
                ActivateSensationByName(sensationList[index]);
            }
            else
            {
                ActivateSensationByName(sensationList[0]);
            }
        }

        public void SelectPreviousSensation()
        {
            var sensationList = sensationLibrary.sensationList;
            if (selectedSensationRowUI != null)
            {
                var selectedGameObject = selectedSensationRowUI.gameObject;
                var index = sensationRows.IndexOf(selectedGameObject);

                if (index <= 0)
                {
                    index = 0;
                }
                else if (index > sensationList.Count - 1)
                {
                    index = sensationList.Count - 2;
                }
                else
                {
                    index -= 1;
                }
                ActivateSensationByName(sensationList[index]);
            }
            else
            {
                ActivateSensationByName(sensationList[0]);
            }
        }


        // Used to remove all of the Child Game Objects in Content GameObject.
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
                rowUI.button.onClick.AddListener(delegate { SensationSelected(rowUI); });
                row.name = sensationName;
            }
        }

        // This should get called whenever the Button of the SensationRow is clicked.
        public void SensationSelected(SensationRowUI sensationRowUI)
        {
            if (selectedSensationRowUI != null)
            {
                selectedSensationRowUI.SetSelectedState(false);
            }

            selectedSensationRowUI = sensationRowUI;
            sensationRowUI.SetSelectedState(true);
            
            activeSensationTopText.text = selectedSensationRowUI.sensationName;
            activeSensation.SensationBlock = selectedSensationRowUI.sensationName;
            activeSensation.OnValidate();

            // Now pass the Sensation Source to the Sensation Input Property Factory
            inputPropertyFactory.SetSensationInputsFromSensation(activeSensation);

            // We might optionally want to keep the values, and not reset each time...
            //playbackManager.ResetSensationToDefault();

            // If a Sensation is selected, set the playback state to true
            playbackManager.EnablePlayback(true);

        }

        // This method activates row item with a given Sensation name...
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

            rowUI.button.Select();
            SensationSelected(rowUI);
        }
    }
}
