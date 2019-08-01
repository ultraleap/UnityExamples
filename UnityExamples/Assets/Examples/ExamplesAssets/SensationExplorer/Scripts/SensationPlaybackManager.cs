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

        public ResetSensationButton resetButton;

        public string startupSensationName = "CircleSensation";

        public bool handPresenceActivatesPlayback { get; set; } = true;
        public bool handPresent { get; set; } = false;

        // Use this for initialization
        void Start()
        {
            activeSensation.inputsCache.Clear();
            //sensationListManager.ActivateSensationByName(startupSensationName);

            // Start Playback in Stopped State
            EnablePlayback(false);

            Application.targetFrameRate = 200;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown("space"))
            {
                TogglePlayback();
            }
        }

        // For simplicity the state of the playable director will be the single
        // point of truth for the playback engine.
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
                if (!SensationTimelineIsRunning())
                {
                    EnablePlayback(true);
                }
            }
        }

        public void HandExited()
        {
            handPresent = false;
            if (handPresenceActivatesPlayback)
            {
                if (SensationTimelineIsRunning())
                {
                    EnablePlayback(false);
                }
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
            if (!SensationTimelineIsRunning())
            {
                EnablePlayback(true);
            }
            else
            {
                EnablePlayback(false);
            }
        }

        public void EnablePlayback(bool playing)
        {
            activeSensation.enabled = playing;
            playbackButtonImage.sprite = playing ? stopSprite : playSprite;

            if (playing)
            {
                playableDirector.Play();
            }
            else
            {
                playableDirector.Stop();
            }
            
        }
    }
}