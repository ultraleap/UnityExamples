using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardShortcuts : MonoBehaviour
{
    public GameObject controlPointModel;
    public GameObject kitModel;
    public GameObject trackerModel;
    public CameraController cameraController;

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

        // Show/Hide the Ultrahaptics Control Points
        if (Input.GetKeyDown(KeyCode.C))
        {
            controlPointModel.SetActive(!controlPointModel.active);
        }

        // Show/Hide the Ultrahaptics Kit
        if (Input.GetKeyDown(KeyCode.K))
        {
            kitModel.SetActive(!kitModel.activeSelf);
            trackerModel.SetActive(!trackerModel.activeSelf);
        }

        // Camera View Shortcuts
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            cameraController.SetCameraLeft();
        }

        if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.F))
        {
            cameraController.SetCameraDefault();
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            cameraController.SetCameraRight();
        }

        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            cameraController.SetCameraTop();
        }
    }
}
