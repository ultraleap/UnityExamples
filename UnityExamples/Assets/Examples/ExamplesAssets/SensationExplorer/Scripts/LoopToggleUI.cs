using UnityEngine;
using UnityEngine.UI;

public class LoopToggleUI : MonoBehaviour
{
    public Toggle toggleButton;
    public Text label;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetLoopActive(bool active )
    {
        toggleButton.interactable = active;
        if (active)
        {
            label.CrossFadeAlpha(1.0f, 0.25f, false);
        }
        else
        {
            label.CrossFadeAlpha(0.5f, 0.25f, false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
