using UnityEngine;
using UnityEngine.UI;

public class RoomCell : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] public Text roomName;
    [SerializeField] public Image colorIndicator;
    [SerializeField] protected Button button;

    [SerializeField] public GameObject routeObject;
    [SerializeField] public GameObject roomObject;

    [SerializeField] public Image backgroundImage;
    //float currentPosition = 0;

    void Start()
    {
        //button.onClick.AddListener(() => Context.OnCellClicked?.Invoke(Index));
    }

    public void SetSelectedState()
    {
        backgroundImage.color = colorIndicator.color;
        roomName.color = Color.white;
    }

    public void SetDeselectedState()
    {
        backgroundImage.color = Color.white;
        roomName.color = Color.black;
    }

}

