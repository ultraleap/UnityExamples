using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RuntimeGizmos;

namespace UltrahapticsCoreAsset.UnityExamples
{
    public class KeyboardShortcuts : MonoBehaviour
    {
        public GameObject controlPointModel;
        public GameObject kitModel;
        public GameObject trackerModel;
        public CameraController cameraController;
        public TransformGizmo transformOverlays;
        public SensationPlaybackManager playbackManager;
        public SensationListManager listManager;
        public SensationSource activeSensation;
        
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("SPACE");
                playbackManager.TogglePlayback();
            }

            // Show/Hide the Ultrahaptics Control Points
            if (Input.GetKeyDown(KeyCode.C))
            {
                controlPointModel.SetActive(!controlPointModel.activeSelf);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                listManager.SelectNextSensation();
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                listManager.SelectPreviousSensation();
            }

            // Show/Hide the Ultrahaptics Control Points
            if (Input.GetKeyDown(KeyCode.O))
            {
                transformOverlays.enabled = !transformOverlays.enabled;
            }

            // Show/Hide the Ultrahaptics Kit
            if (Input.GetKeyDown(KeyCode.K))
            {
                kitModel.SetActive(!kitModel.activeSelf);
                trackerModel.SetActive(!trackerModel.activeSelf);
            }

            // Camera View Shortcuts
            if (Input.GetKeyDown(KeyCode.L))
            {
                cameraController.SetCameraLeft();
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                cameraController.SetCameraFront();
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                cameraController.SetCameraDefault();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                cameraController.SetCameraRight();
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                cameraController.SetCameraTop();
            }
        }
    }
}