using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UltrahapticsCoreAsset
{
    public class HapticTriggerRegion : MonoBehaviour
    {
        public class EditorHelpBoxAttribute : PropertyAttribute { }

        [EditorHelpBox, SerializeField] private bool dummyField_;
        [SerializeField] protected UnityEvent onEnter_ = new UnityEvent();
        [SerializeField] protected UnityEvent onLeave_ = new UnityEvent();
        private readonly HashSet<Collider> entered_ = new HashSet<Collider>();

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

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (entered_.Count == 0)
            {
                onEnter_.Invoke();
            }
            entered_.Add(other);
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (!entered_.Remove(other))
            {
                Debug.LogWarningFormat("{0} left {1} without first entering it");
            } else if (entered_.Count == 0)
            {
                onLeave_.Invoke();
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
