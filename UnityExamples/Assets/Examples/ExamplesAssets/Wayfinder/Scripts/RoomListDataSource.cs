using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListDataSource : MonoBehaviour
{
    public ScrollViewData data;
    public GameObject ContentView;
    public GameObject CellPrefab;
    public List<RoomCell> rowCells;

    public List<GameObject> roomIcons;
    public List<GameObject> routes;

    public Image buildColorA;
    public Image buildColorB;
    public Image buildColorC;
    public Image buildColorD;
    public Image buildColorE;

    [SerializeField]
    public Dictionary<string, Color> buildingColors = new Dictionary<string, Color>();

    void Start()
    {
        buildingColors.Add("A", buildColorA.color);
        buildingColors.Add("B", buildColorB.color);
        buildingColors.Add("C", buildColorC.color);
        buildingColors.Add("D", buildColorD.color);
        buildingColors.Add("E", buildColorE.color);

        var rooms = data.hospital.rooms;

        foreach (Room room in rooms)
        {
            var roomRow = (GameObject)Instantiate(CellPrefab, ContentView.transform);

            var rowCell = roomRow.GetComponent<RoomCell>();
            rowCell.roomName.text = room.Name;
            rowCell.colorIndicator.color = buildingColors[room.Building];

            rowCells.Add(rowCell);
        }
    }
}