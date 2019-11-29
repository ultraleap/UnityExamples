using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace UltrahapticsCoreAsset.Examples
{
    public class DotImageProgress : MonoBehaviour
    {
        public Image ProgressImage;

        private bool progressing_ = false;
        public bool Progress
        {
            get { return progressing_; }
            set { progressing_ = value; }
        }

        [SerializeField] protected UnityEvent progressCompleted_ = new UnityEvent();

        private void Start()
        {
            progressing_ = false;
        }

        public event UnityAction ProgressCompleted
        {
            add { progressCompleted_.AddListener(value); }
            remove { progressCompleted_.RemoveListener(value); }
        }

        public void FadeInProgressDotWithDuration(float duration)
        {
            progressing_ = true;
            ProgressImage.canvasRenderer.SetAlpha(0.0f);
            ProgressImage.CrossFadeAlpha(1.0f, duration, true);
        }

        public void FadeOutProgressDot()
        {
            progressing_ = false;
            ProgressImage.CrossFadeAlpha(0.0f, 0.3f, true);
        }

        void Update()
        {
            // If we have reached full alpha, fire progress Completed Event
            if (progressing_ == true && ProgressImage.canvasRenderer.GetAlpha() >= 1.0f)
            {
                progressCompleted_.Invoke();
            }
        }
    }
}