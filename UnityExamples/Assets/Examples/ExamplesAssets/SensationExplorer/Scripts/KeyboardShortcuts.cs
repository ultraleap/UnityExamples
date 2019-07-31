using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardShortcuts : MonoBehaviour
{
    public GameObject focalPointModel;
    public GameObject kitModel;
    public GameObject trackerModel;

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

        if (Input.GetKeyDown(KeyCode.F))
        {
            focalPointModel.SetActive(!focalPointModel.active);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            kitModel.SetActive(!kitModel.active);
            trackerModel.SetActive(!trackerModel.active);
        }
    }
}
