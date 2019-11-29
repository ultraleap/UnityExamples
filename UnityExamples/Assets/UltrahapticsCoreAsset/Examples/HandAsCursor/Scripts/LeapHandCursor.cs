using UnityEngine;
using UnityEngine.UI;

namespace UltrahapticsCoreAsset.Examples
{
    public class LeapHandCursor : MonoBehaviour
    {
        public RectTransform CursorRect;
        public Vector2 Scaling = new Vector2(4000, 7000);
        public Vector2 CursorPosition;
        public Image CursorImage;
        private IAutoMapper autoMapper_;

        private bool visible_ = true;

        void Start()
        {
            autoMapper_ = GameObject.FindObjectOfType<IAutoMapper>();
            CursorImage = CursorRect.GetComponent<Image>();
        }

        protected void Update()
        {
            if (autoMapper_.HasValueForInputName("palm_position"))
            {
                var palmPos = autoMapper_.GetValueForInputName("palm_position");
                var pos = new Vector2(palmPos.x, palmPos.z);
                pos.Scale(Scaling);
                CursorRect.anchoredPosition = pos;
                CursorPosition = Camera.main.WorldToScreenPoint(CursorRect.position);
            }
            CursorImage.enabled = visible_;
        }

        public bool Visible
        {
            get { return visible_; }
            set { visible_ = value; }
        }
    }
}
