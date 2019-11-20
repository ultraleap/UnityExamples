using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltrahapticsCoreAsset;

public class HandRowSelector : MonoBehaviour {

    public GameObject RowContentView;
    private IAutoMapper _autoMapper;
    public bool selectionIsActive = false;
    public float handEntryHeight = 0.2f;
	public int numRows = 0;
	public RoomListDataSource data;

	// Use this for initialization
	void Start()
    {
        _autoMapper = FindObjectOfType<IAutoMapper>();
    }
	
    public void SelectRowAtIndex(int index)
    {
        ClearRowSelection();
        if (index >= 0 && index < data.rowCells.Count)
        {
            var rowCell = data.rowCells[index];
            //SelectorIndicator.transform.position = rowCell.transform.position;
            rowCell.SetSelectedState();
            data.roomIcons[index].SetActive(true);
            data.routes[index].SetActive(true);
        }
    }

    public void ClearRowSelection()
    {
        var index = 0;
        foreach (var rowCell in data.rowCells)
        {
            rowCell.SetDeselectedState();
            data.roomIcons[index].SetActive(false);
            data.routes[index].SetActive(false);
            index += 1;
        }
    }

    public void SetRoomListDataSource(RoomListDataSource dataSource)
    {
        data = dataSource;
    }
}
