using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class ExtensionMethods
{
    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}

namespace UltrahapticsCoreAsset
{
    public class UpDownRowSelectorRegion : MonoBehaviour
    {
        public class EditorHelpBoxAttribute : PropertyAttribute { }

        [SerializeField] public GameObject ExclusiveCollider;
        [SerializeField] protected UnityEvent onEnter_ = new UnityEvent();
        [SerializeField] protected UnityEvent onLeave_ = new UnityEvent();
        [SerializeField] protected UnityEvent rowChanged_ = new UnityEvent();

        public bool leftHandPresent = false;
        public bool rightHandPresent = false;

        private readonly HashSet<Collider> entered_ = new HashSet<Collider>();

        public float initialHandEntryHeight = 0.2f;
        public float handHeightDelta = 0.0f;

        public GameObject handEntryHeightBar;
        public GameObject minRangeHeightBar;
        public GameObject maxRangeHeightBar;

        // When hand enters the collider, how much ±height to consider the list min/max range.
        // By default, the full travel of the listview is available with ±8cm
        public float listMinMax = 0.05f;

        public int numberOfSteps = 8;
        public int currentIndex = 4;

        public HandRowSelector handRowSelector;

        public event UnityAction OnEnter
        {
            add { onEnter_.AddListener(value); }
            remove { onEnter_.RemoveListener(value); }
        }

        public event UnityAction OnLeave
        {
            add { onLeave_.AddListener(value); }
            remove { onLeave_.RemoveListener(value); }
        }

        public event UnityAction RowChanged
        {
            add { rowChanged_.AddListener(value); }
            remove { rowChanged_.RemoveListener(value); }
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.name != ExclusiveCollider.name)
            {
                return;
            }

            if (entered_.Count == 0)
            {
                onEnter_.Invoke();
            }

            // When the hand enters the collider, store its initial height
            initialHandEntryHeight = other.gameObject.transform.position.y;
            entered_.Add(other);
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (other.gameObject.name != ExclusiveCollider.name) {
                return;
            }
            
            if (!entered_.Remove(other))
            {
                Debug.LogWarningFormat("{0} left {1} without first entering it");
            } else if (entered_.Count == 0)
            {
                onLeave_.Invoke();
            }
            // Clear the hand entry height on hand leave
            initialHandEntryHeight = -1;
            currentIndex = -1;
        }

        protected virtual void OnTriggerStay(Collider other)
        {
            if (other.gameObject.name != ExclusiveCollider.name || initialHandEntryHeight < 0)
            {
                return;
            }

            handEntryHeightBar.transform.position = new Vector3(transform.position.x, initialHandEntryHeight, -0.05f);
            minRangeHeightBar.transform.position = new Vector3(transform.position.x, initialHandEntryHeight - listMinMax, -0.05f);
            maxRangeHeightBar.transform.position = new Vector3(transform.position.x, initialHandEntryHeight + listMinMax, -0.05f);

            // Get closest height value to colour list
            var remapped = ExclusiveCollider.transform.position.y.Remap(minRangeHeightBar.transform.position.y, maxRangeHeightBar.transform.position.y, 8, 0);
            int remappedIndex = Mathf.RoundToInt(remapped);

            if (remappedIndex < 0 || remappedIndex > numberOfSteps)
            {
                return;
            }

            if (remappedIndex > numberOfSteps-1)
            {
                remappedIndex = numberOfSteps-1;
            }

            // Do nothing if the current index is the same as the last one.
            if (currentIndex != remappedIndex)
            {
                currentIndex = remappedIndex;

                handRowSelector.SelectRowAtIndex(currentIndex);
                rowChanged_.Invoke();
            }
            else
            {
                return;
            }
        }

        private readonly System.Predicate<Collider> IsNull = c => c == null || !c.enabled || !c.gameObject.activeInHierarchy;

        protected virtual void Update()
        {
            if (entered_.Count > 0)
            {
                if (entered_.Count == entered_.RemoveWhere(IsNull))
                {
                    onLeave_.Invoke();
                }
            }
        }
    }
}
