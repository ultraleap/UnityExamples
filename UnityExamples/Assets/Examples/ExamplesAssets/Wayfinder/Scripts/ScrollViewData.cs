using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room
{
    public string Name;
    public string Building;
    public string Floor;
}

public class Hospital
{
    public List<Room> rooms;
}

public class ScrollViewData : MonoBehaviour
{

    public TextAsset jsonText;
    public Hospital hospital;

    // Use this for initialization
    void Awake()
    {
        var data = MiniJSON.Json.Deserialize(jsonText.text) as Dictionary<string, object>;
        hospital = JsonUtility.FromJson<Hospital>(jsonText.text);
        //Debug.Log(data.Keys.Count);

        foreach (KeyValuePair<string, object> keyValue in data)
        {
            string key = keyValue.Key;
            Room newRoom = new Room();
            newRoom.Name = key;
            var roomDict = keyValue.Value as Dictionary<string, object>;
            //Debug.Log("Room:" + key);
            foreach (var kvpRoom in roomDict)
            {
                //Debug.Log("Building:" + roomDict["BUILDING"]);
                //Debug.Log("Floor:" + roomDict["FLOOR"]);
                newRoom.Building = (string)roomDict["BUILDING"];
                newRoom.Floor = (string)roomDict["FLOOR"];
            }
            hospital.rooms.Add(newRoom);
        }
    }
}