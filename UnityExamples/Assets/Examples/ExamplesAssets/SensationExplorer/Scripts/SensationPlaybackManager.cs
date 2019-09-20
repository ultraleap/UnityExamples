using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace UltrahapticsCoreAsset.UnityExamples
{
    public class SensationPlaybackManager : MonoBehaviour
    {

        public SensationSource activeSensation;
        public PlayableDirector playableDirector;
        public SensationListManager sensationListManager;

        public Image playbackButtonImage;

        public Sprite playSprite;
        public Sprite stopSprite;

        public Transform sensationTransform;

        public Toggle loopToggle;

        public ResetSensationButton resetButton;

        public bool handPresenceActivatesPlayback { get; set; } = true;
        public bool handPresent { get; set; } = false;

        // Use this for initialization
        void Start()
        {
            activeSensation.inputsCache.Clear();

            // Start Playback in Stopped State
            EnablePlayback(false);

            Application.targetFrameRate = 200;
        }

        // Returns true if Timeline is actively playing back.
        public bool SensationTimelineIsRunning()
        {
            return (playableDirector.state == PlayState.Playing);
        }


        // Reset a Sensation to defaults
        public void ResetSensationToDefault()
        {
            resetButton.ResetSensationInputs();
            resetButton.ResetSensationTransform();
        }

        public void HandEntered()
        {
            handPresent = true;
            if (handPresenceActivatesPlayback)
            {
                RestartPlayback();
            }
        }

        public void HandExited()
        {
            handPresent = false;
            if (handPresenceActivatesPlayback)
            {
                EnablePlayback(false);
            }
        }

        public void SetFixation(int fixationInt)
        {
            FixationDropdownUI.Fixation fixationMode = (FixationDropdownUI.Fixation)fixationInt;

            if (fixationMode == FixationDropdownUI.Fixation.HAND)
            {
                activeSensation.Inputs.TrackingObject = null;
                sensationTransform.gameObject.SetActive(false);
            }
            else if (fixationMode == FixationDropdownUI.Fixation.FIXED)
            {
                activeSensation.Inputs.TrackingObject = sensationTransform;
                sensationTransform.gameObject.SetActive(true);
            }
        }

        public void TogglePlayback()
        {
            if (!SensationTimelineIsRunning() && !activeSensation.enabled)
            {
                EnablePlayback(true);
            }
            else
            {
                EnablePlayback(false);
            }
        }

        // Call this to start time at 0
        public void RestartPlayback()
        {
            EnablePlayback(false);
            EnablePlayback(true);
        }

        public void SetLooping(bool looping)
        {
            if (looping)
            {
                // TODO: Programmatically set the timeline via Playables library
                playableDirector.time = 120;
                playableDirector.Play();
            }
            else
            {
                playableDirector.Stop();
                playableDirector.time = 0;
            }
            loopToggle.isOn = looping;
        }

        public void EnablePlayback(bool playing)
        {
            activeSensation.enabled = playing;
            playbackButtonImage.sprite = playing ? stopSprite : playSprite;          
        }
    }
}