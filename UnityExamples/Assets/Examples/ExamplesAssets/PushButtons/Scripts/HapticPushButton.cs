using UnityEngine;
using UltrahapticsCoreAsset;
using UnityEngine.Events;

namespace HapticButtonExamples
{
    public class HapticPushButton : MonoBehaviour
    {

        // ButtonHoverStateObject - enabled when the hand is hovering/engaged with the Push button
        [SerializeField]
        private GameObject HandInteractionObject;

        // ButtonHoverStateObject - enabled when the hand is hovering/engaged with the Push button
        [SerializeField]
        private GameObject ButtonHoverStateObject;

        [SerializeField]
        private GameObject ButtonMeshObject;
        private GameObject _tempObject = null;

        // The Press Haptic is enabled when the button plunger is being pressed (ENGAGED)
        public SensationSource _pressHaptic;

        [Range(0.0f, 1.0f)]
        public float _minPressIntensity = 0.7f;

        [Range(0.0f, 1.0f)]
        public float _maxPressIntensity = 0.9f;

        public float maxRadius = .04f;
        public float minRadius = .01f;

        // The Click Haptic is triggered when the CONFIRM state is triggered
        public SensationSource _clickHaptic;
        public float _clickHapticDuration = 0.1f;


        //public enum CONTROL_STATE_E {DISABLED, READY, ACTUATING, CONFIRM};
        public enum CONTROL_STATE_E { IDLE, HOVER, ENGAGED, CONFIRM };
        public CONTROL_STATE_E switchPos = CONTROL_STATE_E.IDLE;

        // Distance between button top and bottom of Button Plunger Travel
        public float _buttonTravelDistance = 0.04f;

        private float _buttonMaxYValue;
        private float _buttonMinYValue;
        private float _buttonPositionY;
        private float _buttonClickUpPosition;

        // Events for Button States

        [SerializeField] protected UnityEvent ButtonPushed = new UnityEvent();

        public event UnityAction ButtonPushedAction
        {
            add { ButtonPushed.AddListener(value); }
            remove { ButtonPushed.RemoveListener(value); }
        }

        void Start()
        {
            // Ensure scene contains a Hand Interaction Cube Object
            bool sceneIncludesHandInteractionObject = SceneContainsHandInteractionObject();
            if (!sceneIncludesHandInteractionObject)
            {
                Debug.LogError("Please ensure that a HandInteractionCube is present in the Scene!");
            }
            else
            {
                // If the HandInteractionObject has not been set on the Button, ensure we grab one from the scene
                if (HandInteractionObject == null)
                {
                    HandInteractionObject = GetHandInteractionObjectFromScene();
                }
            }

            _buttonMaxYValue = ButtonMeshObject.transform.position.y;
            _buttonMinYValue = _buttonMaxYValue - _buttonTravelDistance;
            _buttonClickUpPosition = _buttonMinYValue + _buttonTravelDistance * 0.2f;
            _buttonPositionY = _buttonMaxYValue;

            _pressHaptic.Inputs["intensity"].Value = new Vector3(_minPressIntensity, 0, 0);
        }

        private GameObject GetHandInteractionObjectFromScene()
        {
            var handInteractionObject = FindObjectOfType<ButtonSliderExample.HandTrackingCuboid>();
            return handInteractionObject.gameObject;
        }

        private bool SceneContainsHandInteractionObject()
        {
            var handInteractionObject = GetHandInteractionObjectFromScene();
            if (handInteractionObject != null)
            {
                return true;
            }
            return false;
        }

        void Update()
        {
            if(switchPos == CONTROL_STATE_E.IDLE)
                _pressHaptic.Running = false;
            // if the hand is in the vicinity of the collider/buttopTop 
            // set the button top position to the colliding object else reset the position back to default
            if (_tempObject != null)
            {
                _buttonPositionY = _tempObject.transform.position.y;
                setPressPosition(_buttonPositionY);

                if (switchPos == CONTROL_STATE_E.IDLE)
                {
                    if (_buttonPositionY > _buttonMaxYValue)
                    {
                        ButtonHoverStateObject.SetActive(true);
                        _pressHaptic.Running = true;
                        switchPos = CONTROL_STATE_E.ENGAGED;
                    }
                }
                else if (switchPos == CONTROL_STATE_E.ENGAGED)
                {
                    // Update pressing haptic                    
                    setPressHapticRadius((ButtonMeshObject.transform.position.y - _buttonMinYValue) / _buttonTravelDistance);

                    if (_buttonPositionY < _buttonMinYValue)
                    {
                        switchPos = CONTROL_STATE_E.CONFIRM;
                        pressComplete();
                    }
                }
                else if (switchPos == CONTROL_STATE_E.CONFIRM)
                {
                    if (_buttonPositionY >= _buttonClickUpPosition)
                    {
                        // Hand is moving back up out off the button. 
                        switchPos = CONTROL_STATE_E.ENGAGED;
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
                    ButtonMeshObject.transform.position = new UnityEngine.Vector3(ButtonMeshObject.transform.position.x, _buttonPositionY, ButtonMeshObject.transform.position.z);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            _pressHaptic.Running = true;

            if (other.gameObject == HandInteractionObject.gameObject)
            {
                // Avoid an entry from BELOW the Top of the Button
                if (other.transform.position.y < _buttonMinYValue)
                {
                    return;
                }

                // Use a proxy object, attached to the hand, to actuate the button.
                _tempObject = other.gameObject;
            }
        }

        public GameObject ButtonTop;

        private void OnTriggerStay(Collider other)
        {
            _pressHaptic.Running = true;
            // Keep the press haptic tracking the hand's Y position.
            if (other.gameObject == HandInteractionObject.gameObject)
            {
                _pressHaptic.transform.position = ButtonTop.transform.position;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == HandInteractionObject.gameObject)
            {
                switchPos = CONTROL_STATE_E.IDLE;

                _tempObject = null;
                ButtonHoverStateObject.SetActive(false);
                _pressHaptic.Running = false;
            }
        }

        private void setPressPosition(float posY)
        {
            float newY = Mathf.Clamp(posY, _buttonMinYValue, _buttonMaxYValue);
            ButtonMeshObject.transform.position = new UnityEngine.Vector3(ButtonMeshObject.transform.position.x, newY, ButtonMeshObject.transform.position.z);
        }

        private void setPressHapticRadius(float normalizedTravel)
        {
            float intensity = Mathf.Lerp(minRadius, maxRadius, normalizedTravel);
            _pressHaptic.Inputs["radius"].Value = new UnityEngine.Vector3(intensity, 0, 0);
        }

        private void setPressHapticIntensity(float normalisedTravel)
        {
            float intensity = Mathf.Lerp(_maxPressIntensity, _minPressIntensity, normalisedTravel);
            _pressHaptic.Inputs["intensity"].Value = new UnityEngine.Vector3(intensity, 0, 0);
        }


        private void clickComplete()
        {
            // Hand is returning out to complete the toggle.
            _clickHaptic.RunForDuration(_clickHapticDuration); // Play haptic for short duration
        }

        private void pressComplete()
        {
            // Hand has travelled fully to start first part of switching.
            ButtonPushed.Invoke();
            _clickHaptic.RunForDuration(_clickHapticDuration); // Play haptic for short duration
            _pressHaptic.Inputs["intensity"].Value = new UnityEngine.Vector3(1.0f, 0, 0);
        }
    }
}