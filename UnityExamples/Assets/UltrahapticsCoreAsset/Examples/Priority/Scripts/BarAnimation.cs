using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using UnityEngine.UI;

namespace UltrahapticsCoreAsset.Examples
{
    public class BarAnimation : MonoBehaviour
    {

        public SensationSource HandScanSensation;
        public SensationSource PalmTrackedCircleSensation;
        public PlayableDirector Timeline;
        public RectTransform CursorRect;
        public Image CursorImage;
        public Sprite DefaultSprite;
        public Sprite HoverSprite;
        public float LoopGapTime = 3.0f;
        public bool Looping = true;

        // 'interruptible' state is used because we want to make the cursor
        // be unaffected by other objects, as if it were a 'regnerative forcefield'
        public bool Interruptible = true;

        // Use this for initialization
        void Start()
        {
            StartCoroutine("LoopBar");
        }

        IEnumerator LoopBar()
        {
            WaitForSeconds waitTime = new WaitForSeconds(LoopGapTime);
            while (Looping)
            {
                Timeline.Play();
                yield return waitTime;
            }
        }

        // This is called when the Bar enters the region of the Cursor.
        public void SetCursorHoverStateForDuration(float seconds)
        {
            if (seconds < 0)
            {
                Debug.LogWarning("Playback Duration cannot be negative");
                return;
            }
            CursorRect.sizeDelta = new Vector2(480.0f, 480.0f);

            // Make the Cursor Blue when bar Sensation is active
            CursorImage.sprite = HoverSprite;


            // When the Bar collides with the cursor, it acts like a forcefield - the particle does not affect it!
            Interruptible = false;

            // The HandScan Sensation must be started from 0, so toggle the enabled state...
            HandScanSensation.enabled = false;
            HandScanSensation.enabled = true;

            // And ensure that the HandScan Sensation is running
            HandScanSensation.Running = true;

            StartCoroutine(DefaultCursorAfterSeconds(seconds));
        }

        private IEnumerator DefaultCursorAfterSeconds(float seconds)
        {
            yield return new WaitForSeconds(seconds);

            // The bar has finished its handscan cycle - cursor can now be affected by the particle.
            Interruptible = true;
            CursorImage.sprite = DefaultSprite;
            HandScanSensation.enabled = false;
            HandScanSensation.Running = false;
            PalmTrackedCircleSensation.enabled = false;
        }
    }
}
