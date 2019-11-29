using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UltrahapticsCoreAsset
{

public class EmitterDataSource : MonoBehaviour, IDataSource
{
    private Dictionary<string, UnityEngine.Vector3> data_ = new Dictionary<string, UnityEngine.Vector3>();

    public string[] GetAvailableDataItemsForType<T>()
    {
        if (typeof(T) == typeof(UnityEngine.Vector3))
        {
            return data_.Keys.ToArray();
        }
        return null;
    }

    public T GetDataItemByName<T>(string name)
    {
        if (!data_.ContainsKey(name))
        {
            throw new NullReferenceException();
        }
        return (T)(object)data_[name]; // Force conversion from Vector3 to T (which must be Vector3)
    }

    UnityEngine.Vector3 GetPosition()
    {
        return this.gameObject.transform.position;
    }

    UnityEngine.Vector3 GetXAxis()
    {
        var rotation = this.gameObject.transform.rotation;
        var direction = new UnityEngine.Vector3(1, 0, 0);
        return rotation * direction;
    }

    UnityEngine.Vector3 GetYAxis()
    {
        var rotation = this.gameObject.transform.rotation;
        var direction = new UnityEngine.Vector3(0, 1, 0);
        return rotation * direction;
    }

    UnityEngine.Vector3 GetZAxis()
    {
        var rotation = this.gameObject.transform.rotation;
        var direction = new UnityEngine.Vector3(0, 0, 1);
        return rotation * direction;
    }

    int NumberOfDataSourceInScene()
    {
        return GameObject.FindObjectsOfType<EmitterDataSource>().Length;
    }

    void UpdateDataItems()
    {
        data_["virtualEmitterOriginInVirtualSpace"] = GetPosition();
        data_["virtualEmitterXInVirtualSpace"] = GetXAxis();
        data_["virtualEmitterYInVirtualSpace"] = GetYAxis();
        data_["virtualEmitterZInVirtualSpace"] = GetZAxis();
    }

    void Start()
    {
        UpdateDataItems();
        this.RegisterToAutoMapper();
        if (NumberOfDataSourceInScene() > 1)
        {
            this.LogWarningForDuplicateDataSources("Emitter Data Source");
        }
    }

    void Update()
    {
        UpdateDataItems();
    }
}

}
