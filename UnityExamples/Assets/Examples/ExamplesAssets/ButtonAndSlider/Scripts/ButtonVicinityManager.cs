using UnityEngine;
using UltrahapticsCoreAsset;
using UnityEngine.Playables;

namespace ButtonSliderExample
{
    public class ButtonVicinityManager : MonoBehaviour
    {
        [SerializeField]
        public AudioSource _audioClickDown;
        public AudioSource _audioClickUp;
        [SerializeField]
        private GameObject _hoverState;
        [SerializeField]
        private GameObject _sunOn;
        [SerializeField]
        private GameObject _sunOff;
        [SerializeField]
        private GameObject _buttonPresser;
        private GameObject _tempObject = null;
        public SensationSource _pressHaptic;
        public float _minPressIntensity;
        public float _maxPressIntensity;
        public SensationSource _clickHaptic;
        public float _clickHapticDuration = 0.2f;
        public enum SWITCH_POSITION_E {DISABLED, READY, ACTUATING, CLICKED};
        public SWITCH_POSITION_E _switchPos =  SWITCH_POSITION_E.READY;

        public enum SWITCH_STATE_E {OFF=0, ON=1, UNSET};
        public SWITCH_STATE_E _switchState = SWITCH_STATE_E.OFF; 
        public float _buttonTravelDistance; // Distance between button top and bottom
        private float _buttonMaxYValue;
        private float _buttonMinYValue;
        private float _buttonPositionY;
        private float _buttonClickUpPosition; 
        public float _offset = 0.026f;
        [SerializeField]

        void Start()
        {
            _sunOn.SetActive(false);
            _sunOff.SetActive(true);

            _buttonMaxYValue = _buttonPresser.transform.position.y;
            _buttonMinYValue = _buttonMaxYValue - _buttonTravelDistance;
            _buttonClickUpPosition = _buttonMinYValue + _buttonTravelDistance * 0.2f;
            _buttonPositionY = _buttonMaxYValue;

            _pressHaptic.Inputs["intensity"].Value = new Vector3(_minPressIntensity, 0, 0);
        }

        void Update()
        {        
            // if the hand is in the vicinity of the collider/buttopTop 
            // set the button top position to the colliding object else reset the position back to default
            if (_tempObject != null)
            {
                _buttonPositionY = _tempObject.transform.position.y - _offset;		
                setPressPosition(_buttonPositionY);	
                
                if (_switchPos == SWITCH_POSITION_E.READY)
                {
                    if (_buttonPositionY > _buttonMaxYValue)
                    {
                        _hoverState.SetActive(true);
                        _pressHaptic.Running = true;
                        _switchPos = SWITCH_POSITION_E.ACTUATING;
                    }
                }
                else if (_switchPos == SWITCH_POSITION_E.ACTUATING)
                {                    
                    // Update pressing haptic                    
                    setPressHapticIntensity((_buttonPresser.transform.position.y - _buttonMinYValue) / _buttonTravelDistance);

                    if (_buttonPositionY < _buttonMinYValue){
                        _switchPos = SWITCH_POSITION_E.CLICKED;
                        pressComplete();
                    }			
                }
                else if (_switchPos == SWITCH_POSITION_E.CLICKED)
                {				
                    if (_buttonPositionY >= _buttonClickUpPosition)
                    {
                        // Hand is moving back up out off the button. 
                        _switchPos = SWITCH_POSITION_E.ACTUATING;
                        clickComplete();
                    }
                }
            }
            else
            {
                // Catch button state if leaving vicinity
                if (_buttonPositionY < _buttonMaxYValue)
                {
                    // Return the button position back to initial value.
                    float damping = 0.1f;
                    _buttonPositionY = _buttonPositionY + damping * (_buttonMaxYValue - _buttonPositionY);
                    _buttonPositionY = Mathf.Min(_buttonPositionY, _buttonMaxYValue);
                    _buttonPresser.transform.position = new UnityEngine.Vector3(_buttonPresser.transform.position.x, _buttonPositionY, _buttonPresser.transform.position.z);
                }
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {	
            if (_switchPos == SWITCH_POSITION_E.DISABLED)
                return;

            // Debug.Log("ButtonVicinityManager OnTriggerExit");
            if (other.name == "HandTrackedBlock")
            {
                // Use a proxy object, attached to the hand, to actuate the button.
                _tempObject = other.gameObject;     
            }       
        }

        private void OnTriggerStay(Collider other)
        {
            if (_switchPos == SWITCH_POSITION_E.DISABLED)
                return;
                                        
            // Keep the press haptic tracking the hand's Y position.
            if (other.name == "HandTrackedBlock")
            {
                _pressHaptic.transform.position = new UnityEngine.Vector3(
                    _buttonPresser.transform.position.x, 
                    _buttonPositionY, 					
                    _buttonPresser.transform.position.z);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_switchPos == SWITCH_POSITION_E.DISABLED)
                return;                

            if (other.name == "HandTrackedBlock")
            {
                // Debug.Log("ButtonVicinityManager OnTriggerExit");
                // Leaving the vicinity, go back to ready state
                _switchPos = SWITCH_POSITION_E.READY; 

                _tempObject = null;
                _hoverState.SetActive(false);
                _pressHaptic.Running = false;
            }		
        }
        
        private void setPressPosition(float _buttonPositionY)
        {
            float newY = Mathf.Clamp(_buttonPositionY, _buttonMinYValue, _buttonMaxYValue);
            _buttonPresser.transform.position = new UnityEngine.Vector3(_buttonPresser.transform.position.x, newY, _buttonPresser.transform.position.z);
        }

        private void setPressHapticIntensity(float normalisedTravel)
        {
            float intensity = Mathf.Lerp(_maxPressIntensity, _minPressIntensity, normalisedTravel);
            _pressHaptic.Inputs["intensity"].Value = new UnityEngine.Vector3(intensity, 0, 0);		
        }

        private void toggleState()
        {
            _switchState = (_switchState == SWITCH_STATE_E.OFF) ? SWITCH_STATE_E.ON : SWITCH_STATE_E.OFF;
            _sunOn.SetActive(_switchState == SWITCH_STATE_E.ON);
            _sunOff.SetActive(_switchState == SWITCH_STATE_E.OFF);
        }

        private void clickComplete()
        {
            // Hand is returning out to complete the toggle.
            toggleState();
            _clickHaptic.RunForDuration(_clickHapticDuration); // Play haptic
            _audioClickUp.Play();
        }

        private void pressComplete()
        {
            // Hand has travelled fully to start first part of switching.
            _clickHaptic.RunForDuration(_clickHapticDuration); // Play haptic
            _pressHaptic.Inputs["intensity"].Value = new UnityEngine.Vector3(1.0f, 0, 0);	
            _audioClickDown.Play();
        }
    }
}