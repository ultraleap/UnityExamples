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

        if (Input.GetKeyDown(KeyCode.C))
        {
            controlPointModel.SetActive(!controlPointModel.active);
        }

        // Reset the Camera to Default and Fit
        if (Input.GetKeyDown(KeyCode.F))
        {
            cameraController.SetCameraDefault();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            kitModel.SetActive(!kitModel.activeSelf);
            trackerModel.SetActive(!trackerModel.activeSelf);
        }
    }
}
