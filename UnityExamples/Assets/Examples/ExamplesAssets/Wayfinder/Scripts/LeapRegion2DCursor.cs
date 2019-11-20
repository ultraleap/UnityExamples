using UnityEngine;
using UnityEngine.UI;

namespace UltrahapticsCoreAsset.Examples
{
    public class LeapRegion2DCursor : MonoBehaviour
    {
        // The 2D region in which the Cursor should move
        public RectTransform MainCanvas;
        public RectTransform Region2D;
        public RectTransform CursorRect;
        public Vector2 Scaling = new Vector2(4000, 7000);
        //public Vector2 CursorPosition;
        public Image CursorImage;
        public Vector2 pos;
        private IAutoMapper autoMapper_;

        public Vector2 worldPoint;

        private bool visible_ = true;

        void Start()
        {
            autoMapper_ = GameObject.FindObjectOfType<IAutoMapper>();
            CursorImage = CursorRect.GetComponent<Image>();
            //Debug.Log("Rect xMin:" + Region2D.rect.xMin);
            //Debug.Log("Rect xMax:" + Region2D.rect.xMax);
            //Debug.Log("Rect yMin:" + Region2D.rect.yMin);
            //Debug.Log("Rect yMax:" + Region2D.rect.yMax);

            Debug.Log("MainCanvas.rect.width:" + MainCanvas.rect.width);
            Debug.Log("MainCanvas.rect.height:" + MainCanvas.rect.height);

            Debug.Log("Region2D.anchoredPosition.x:" + Region2D.anchoredPosition.x);
            Debug.Log("Region2D.anchoredPosition.rect.size:" + Region2D.rect.size);
            Debug.Log("Region2D.anchoredPosition.y:" + Region2D.anchoredPosition.y);
        }

        protected void Update()
        {
            if (autoMapper_.HasValueForInputName("palm_position"))
            {
                var palmPos = autoMapper_.GetValueForInputName("palm_position");
                pos = new Vector2(palmPos.x, palmPos.y);
                pos.Scale(Scaling);

                pos.x = Mathf.Clamp(pos.x, -1032.0f, 360.0f);
                pos.y = Mathf.Clamp(pos.y, -300.0f, 500.0f);
                // Now map the Screen Space to the Rectangle

                RectTransformUtility.ScreenPointToLocalPointInRectangle(Region2D, pos, Camera.main, out worldPoint);

                //Debug.Log(worldPoint);

                CursorRect.anchoredPosition = pos;
                //CursorPosition = Camera.main.WorldToScreenPoint(CursorRect.position);
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
