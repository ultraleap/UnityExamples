using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace UltrahapticsCoreAsset.Examples.PolylineHandPathDesigner
{
    public class Polyline6DataSource : MonoBehaviour, IDataSource
    {
        private Dictionary<string, UnityEngine.Vector3> _data = new Dictionary<string, UnityEngine.Vector3>();
        public AutoMapper AutoMapper;
        public List<GameObject> pointList = new List<GameObject>(6);
        public List<GameObject> gameObjectFeatureList = new List<GameObject>(6);

        public string[] GetAvailableDataItemsForType<T>()
        {
            if (typeof(T) == typeof(UnityEngine.Vector3))
            {
                return _data.Keys.ToArray();
            }
            return null;
        }

        public T GetDataItemByName<T>(string name)
        {
            if (!_data.ContainsKey(name))
            {
                throw new NullReferenceException();
            }
            return (T)(object)_data[name]; // Force conversion from Vector3 to T (which must be Vector3)
        }

        void updateDataItems()
        {
            if (AutoMapper.HasValueForInputName("palm_position"))
            {

                for (int i = 0; i < pointList.Count; i++)
                {
                    GameObject pointGO = pointList[i];
                    DragSnapToNearestObject2D draggable = pointGO.GetComponent<DragSnapToNearestObject2D>();
                    if (draggable.nearestObject != null)
                    {
                        gameObjectFeatureList[i] = draggable.nearestObject;
                        var inputName = draggable.nearestObject.name;
                        _data["feature" + i] = AutoMapper.GetValueForInputName(inputName);
                    }
                }
            }
        }

        void Start()
        {
            updateDataItems();
            this.RegisterToAutoMapper();
        }

        void Update()
        {
            updateDataItems();
        }
    }
}
