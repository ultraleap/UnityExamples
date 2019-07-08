using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace UltrahapticsCoreAsset.UnityExamples
{
    public class SensationPlaybackManager : MonoBehaviour
    {

        public SensationSource activeSensation;
        public PlayableDirector playableDirector;

        public Toggle playToggleButton;

        //public PlayableDirector playableDirector;
        public bool handPresenceEnablesPlayback = true;

        // Use this for initialization
        void Start()
        {
            activeSensation.inputsCache.Clear();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown("space"))
            {
                TogglePlayback();
            }
        }
        
        public void TogglePlayback()
        {
            playToggleButton.isOn = !playToggleButton.isOn;
        }

        public void EnablePlayback(bool playing)
        {
            Debug.Log("Enable Playback: " + playing);
            activeSensation.enabled = playing;
            //playableDirector.enabled = playing;
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