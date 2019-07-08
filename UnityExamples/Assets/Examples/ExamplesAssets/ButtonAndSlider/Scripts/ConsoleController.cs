using System.Collections;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using UltrahapticsCoreAsset;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace ButtonSliderExample
{
	public class ConsoleController : MonoBehaviour
	{
		private ButtonVicinityManager _buttonVicinityManager;
		public AudioSource _musicSource;
		private ButtonVicinityManager.SWITCH_STATE_E _lastSwitchState;
		private SliderManager _sliderManager;
		private float _lastSliderVal;
		[SerializeField]
		public Image backgroundImage;
		private Color onColor = new Color(1.0f,1.0f,1.0f,1.0f);
		private Color offColor = new Color(0.75f, 0.33f, 0.0f, 1.0f);

		void Start()
		{
			_buttonVicinityManager = FindObjectOfType<ButtonVicinityManager>();
			_sliderManager = FindObjectOfType<SliderManager>();

			_lastSwitchState = ButtonVicinityManager.SWITCH_STATE_E.UNSET;	

			_lastSliderVal = _sliderManager._sliderValue;
			_musicSource.volume = _lastSliderVal;

			backgroundImage.color = offColor;
		}

		void Update()
		{
			UpdateControlStatus();

			// Slider updates audio volume
			if (_lastSliderVal != _sliderManager._sliderValue)
			{
				_musicSource.volume = _sliderManager._sliderValue;
				_lastSliderVal = _sliderManager._sliderValue;
			}

			// Button updates switch status
			if (_lastSwitchState != _buttonVicinityManager._switchState)
			{
				// Button has clicked
				if (_buttonVicinityManager._switchState == ButtonVicinityManager.SWITCH_STATE_E.ON)
				{
					backgroundImage.color = onColor;
				}
				else
				{
					backgroundImage.color = offColor;

				}
				_lastSwitchState =  _buttonVicinityManager._switchState;
			}		
		}

		// Update all the controls to avoid any being used simultaneously.
		void UpdateControlStatus()
		{
			if (_sliderManager._sliderState >= SliderManager.SLIDER_STATE_E.READY)
			{
				// If the slider is being used, disable the button
				_buttonVicinityManager._switchPos = ButtonVicinityManager.SWITCH_POSITION_E.DISABLED;
			}
			else
			{
				// Re-enable the button
				if (_buttonVicinityManager._switchPos == ButtonVicinityManager.SWITCH_POSITION_E.DISABLED)
					_buttonVicinityManager._switchPos = ButtonVicinityManager.SWITCH_POSITION_E.READY;
			}	

			// and vice versa
			if (_buttonVicinityManager._switchPos >= ButtonVicinityManager.SWITCH_POSITION_E.ACTUATING)
			{
				_sliderManager._sliderState = SliderManager.SLIDER_STATE_E.DISABLED;
			}
			else
			{
				if (_sliderManager._sliderState == SliderManager.SLIDER_STATE_E.DISABLED)
					_sliderManager._sliderState = SliderManager.SLIDER_STATE_E.ENABLED;
			}
		}
	}
}
