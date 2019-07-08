using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltrahapticsCoreAsset;

namespace ButtonSliderExample
	{
	public class SliderManager : MonoBehaviour 
	{
		public enum SLIDER_STATE_E {DISABLED, ENABLED, READY, TRACKING};
		public SLIDER_STATE_E _sliderState;
		public float _sliderValue;
		public float _sliderMaxValue = 1.0f;
		private IAutoMapper _autoMapper;
		private float _sliderMinPosition;
		private float _sliderTravel;
		[SerializeField]
		public GameObject _hoverState;
		public Transform _markerTransform;
		public Transform _sensationTransform;
		public SensationSource _sliderSensation;
		public SensationSource _notchSensation;
		private Vector3 _lastFingerPoint;
		private Vector3 _smoothedDisplacement;
		public AudioSource _notchClick;
		private int _lastNotch = 0;
		public int _notchGradations = 5;

		private Collider _collider;
		public string _trackedFingerId = "middleFinger_intermediate_position";
		private float _handAlignmentAngleThreshold = 30.0f;

		// Use this for initialization
		void Start () {
			_sliderState = SLIDER_STATE_E.ENABLED;			
			// This provides the hand tracking data
			_autoMapper = FindObjectOfType<IAutoMapper>();
			_sliderMinPosition = _markerTransform.position.z; 
			_sliderTravel = 0.0606f - _sliderMinPosition; // This is the length of the slider travel 
			_lastFingerPoint = _markerTransform.position;
			_smoothedDisplacement = new Vector3(0, 0, 0);	
			_collider = GetComponent<Collider>();

			UpdateSliderValue(0);
		}
		
		// Update is called once per frame
		void Update () {
		
			if (_sliderState == SLIDER_STATE_E.READY)
			{
				_hoverState.SetActive(true);
				if (isHandAligned(_handAlignmentAngleThreshold))
				{
					_sliderState = SLIDER_STATE_E.TRACKING;					
				}
			}
			else if (_sliderState == SLIDER_STATE_E.TRACKING)
			{
				if(isTrackedFingerInCollider(_trackedFingerId))
				{
					// Track the slider haptic to the middle finger
					Vector3 fingerPos = _autoMapper.GetValueForInputName(_trackedFingerId);
					_sensationTransform.position = new Vector3(fingerPos.x, fingerPos.y, _sensationTransform.position.z);

					if(HandAlongSliderDirection(fingerPos)) 
					{
						// Only update the slider position if the hand is moving along its motion of travel.
						float x = UpdateSliderPosition(fingerPos);

						UpdateSliderValue(x);
					}
					else
					{
						// Return to a ready state.
						_sliderState = SLIDER_STATE_E.READY;
					}
				}
			}
			else 
			{
				_hoverState.SetActive(false);
				_sliderSensation.Running = false;
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (_sliderState == SLIDER_STATE_E.DISABLED)
				return;

			//Debug.Log("Slider Manager - OnTriggerEnter ctr: [" + _handComponentCtr + "]");
			if (other.name == "HandTrackedBlock")
            {		
				if (_sliderState == SLIDER_STATE_E.ENABLED)
				{
					_sliderState = SLIDER_STATE_E.READY;
					_sliderSensation.Running = true;
				}
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (_sliderState == SLIDER_STATE_E.DISABLED)
				return;
			
			if (other.name == "HandTrackedBlock")
            {
				_sliderState = SLIDER_STATE_E.ENABLED;
				_sliderSensation.Running = false;
			}
		}

		private bool isTrackedFingerInCollider(string fingerId)
		{
			if (_autoMapper.HasValueForInputName(fingerId))
			{
				// Get the postion of the index finger
				Vector3 pt = _autoMapper.GetValueForInputName(fingerId);
				if(_collider.bounds.Contains(pt))
				{
					return true;
				}
			}

			return false;
		}

		private bool isNotch(float x, int gradations, ref int lastNotch)
		{
			// Scale up by nGradations and find absolute fractional component
			float nearestNotch = Mathf.Round(x * gradations);
			float rem = Mathf.Abs((x * gradations) - nearestNotch);

			if (nearestNotch != lastNotch)
			{
				if (rem <= 0.1)
				{
					// Debug.Log("Notch: " + nearestNotch);
					lastNotch = (int)nearestNotch;

					return true;
				}
			}

			return false;
		}

		private void UpdateSliderValue(float x)
		{
			// Clamp between 0 and 1 and round to hundredths 
			x = Mathf.Clamp(x, 0, 1.0f);
			x = Mathf.Round(x * 100.0f)/100.0f;
			
			_sliderValue = x * _sliderMaxValue;

			if (isNotch(x, _notchGradations, ref _lastNotch))
			{
				// play notch haptic and sound cue
				_notchSensation.RunForDuration(0.1f);
				_notchClick.Play();
			}
		}

		// Update the slider position for the given vector3 value and return a normalised scaling value.
		private float UpdateSliderPosition(Vector3 fingerPos)
		{
			// Move teh slider marker to the new finger position.
			float p = Mathf.Clamp(fingerPos.z, _sliderMinPosition, _sliderMinPosition+_sliderTravel);
			_markerTransform.position = new Vector3(_markerTransform.position.x, _markerTransform.position.y, p);

			// Keep the slider haptic in the correct position
			_sensationTransform.position = new Vector3(_sensationTransform.position.x, _sensationTransform.position.y, p);

			return (p - _sliderMinPosition) / _sliderTravel;
		}

		private bool angleWithinDegrees(float angle, float toleranceDegrees)
		{
			return ((angle < toleranceDegrees) | (angle > (180.0f - toleranceDegrees)));
		}

		// Is the hand moving along the direction of the slider travel?
		private bool HandAlongSliderDirection(Vector3 fingerPos)
		{
			float toleranceDegrees = 20.0f;
			float beta = 0.5f; // Smoothing coefficient	

			// Change in position since last call
			Vector3 sliderHandDisplacement = fingerPos-_lastFingerPoint;

			_lastFingerPoint = fingerPos;
			// Smooth using an autorecursive average
			_smoothedDisplacement = (1-beta) * sliderHandDisplacement + beta * _smoothedDisplacement;

			// Measure the angle between the slider and the direction of travel.
			float angle = Vector3.Angle(_smoothedDisplacement, transform.forward);

			// +/- 20 degrees?
			return angleWithinDegrees(angle, toleranceDegrees);
		}

		private bool isHandAligned(float toleranceDegrees)
		{
			Vector3 palm_direction = _autoMapper.GetValueForInputName("palm_direction");
			float angle = Vector3.Angle(transform.forward, palm_direction);
			
			return angleWithinDegrees(angle, toleranceDegrees);
		}
	}
}
