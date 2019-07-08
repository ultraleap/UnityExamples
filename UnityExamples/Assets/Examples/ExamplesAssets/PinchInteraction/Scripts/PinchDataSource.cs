using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace UltrahapticsCoreAsset.Examples
{
    public class PinchDataSource : MonoBehaviour, IDataSource
    {
        private Dictionary<string, Vector3> _data = new Dictionary<string, Vector3>();
        public bool leftPinchActive = false;
        public bool rightPinchActive = false;
        public float THUMB_TRIGGER_DISTANCE = 0.04f;
        private IAutoMapper autoMapper_;

		[SerializeField] protected UnityEvent LeftHandBeganPinching = new UnityEvent();
		[SerializeField] protected UnityEvent LeftHandIsPinching = new UnityEvent();
		[SerializeField] protected UnityEvent LeftHandStoppedPinching = new UnityEvent();

		[SerializeField] protected UnityEvent RightHandBeganPinching = new UnityEvent();
		[SerializeField] protected UnityEvent RightHandIsPinching = new UnityEvent();
		[SerializeField] protected UnityEvent RightHandStoppedPinching = new UnityEvent();

		void Start()
        {
            autoMapper_ = FindObjectOfType<IAutoMapper>();
            updateDataItems();
            this.RegisterToAutoMapper();
        }

		public event UnityAction LeftPinchEngaged
		{
			add { LeftHandBeganPinching.AddListener(value); }
			remove { LeftHandBeganPinching.RemoveListener(value); }
		}

		public event UnityAction LeftIsPinching
		{
			add { LeftHandIsPinching.AddListener(value); }
			remove { LeftHandIsPinching.RemoveListener(value); }
		}

		public event UnityAction LeftPinchDisengaged
		{
			add { LeftHandStoppedPinching.AddListener(value); }
			remove { LeftHandStoppedPinching.RemoveListener(value); }
		}

		public event UnityAction RightPinchEngaged
		{
			add { RightHandBeganPinching.AddListener(value); }
			remove { RightHandBeganPinching.RemoveListener(value); }
		}

		public event UnityAction RightIsPinching
		{
			add { RightHandIsPinching.AddListener(value); }
			remove { RightHandIsPinching.RemoveListener(value); }
		}

		public event UnityAction RightPinchDisengaged
		{
			add { RightHandStoppedPinching.AddListener(value); }
			remove { RightHandStoppedPinching.RemoveListener(value); }
		}

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
            return (T)(object)_data[name];
        }

		void updateDataItems()
		{
			if (autoMapper_.HasValueForInputName("leftHand_thumb_distal_position"))
			{

				_data["leftPinching"] = new Vector3(0, 0, 0);
				_data["rightPinching"] = new Vector3(0, 0, 0);
				var left_thumb_pos = autoMapper_.GetValueForInputName("leftHand_thumb_distal_position");
				var right_thumb_pos = autoMapper_.GetValueForInputName("rightHand_thumb_distal_position");
				var left_index_pos = autoMapper_.GetValueForInputName("leftHand_indexFinger_distal_position");
				var right_index_pos = autoMapper_.GetValueForInputName("rightHand_indexFinger_distal_position");

				Vector3 left_difference = left_index_pos - left_thumb_pos;
				Vector3 right_difference = right_index_pos - right_thumb_pos;

				if (left_thumb_pos != Vector3.zero)
				{
					if (left_difference.magnitude < THUMB_TRIGGER_DISTANCE)
					{
						if (!leftPinchActive)
						{
							LeftHandBeganPinching.Invoke();
						}
						leftPinchActive = true;
						LeftHandIsPinching.Invoke();
					}
					else
					{
						if (leftPinchActive)
						{
							LeftHandStoppedPinching.Invoke();
						}
						leftPinchActive = false;

					}
				}
				else
				{
					leftPinchActive = false;
				}

				if (right_thumb_pos != Vector3.zero)
				{
					if (right_difference.magnitude < THUMB_TRIGGER_DISTANCE)
					{
						if (!rightPinchActive)
						{
							RightHandBeganPinching.Invoke();
						}
						rightPinchActive = true;
						RightHandIsPinching.Invoke();
					}
					else
					{
						if (rightPinchActive)
						{
                            RightHandStoppedPinching.Invoke();
						}
						rightPinchActive = false;
					}
				}
				else
				{
					rightPinchActive = false;
				}
			}
		}

        void Update()
        {
            updateDataItems();
        }
    }
}
