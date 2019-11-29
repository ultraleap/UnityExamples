using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UltrahapticsCoreAsset
{

public class SensationSpaceToVirtualSpaceDataSource : MonoBehaviour, IDataSource
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

    int NumberOfDataSourceInScene()
    {
        return GameObject.FindObjectsOfType<SensationSpaceToVirtualSpaceDataSource>().Length;
    }

    void Start()
    {
        data_.Add("sensationOriginInVirtualSpace", new UnityEngine.Vector3(0, 0, 0));
        data_.Add("sensationXInVirtualSpace", new UnityEngine.Vector3(1, 0, 0));
        data_.Add("sensationYInVirtualSpace", new UnityEngine.Vector3(0, 0, 1));
        data_.Add("sensationZInVirtualSpace", new UnityEngine.Vector3(0, 1, 0));

        this.RegisterToAutoMapper();
        if (NumberOfDataSourceInScene() > 1)
        {
            this.LogWarningForDuplicateDataSources("Sensation Space to Virtual Space Data Source");
        }
    }

}

}
