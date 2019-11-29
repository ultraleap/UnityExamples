using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UltrahapticsCoreAsset.Examples
{
    public class Collider2DEventHelper : MonoBehaviour
    {
        [SerializeField] protected UnityEvent onEnter_ = new UnityEvent();
        [SerializeField] protected UnityEvent onLeave_ = new UnityEvent();
        private readonly HashSet<Collider2D> entered_ = new HashSet<Collider2D>();

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

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (entered_.Count == 0)
            {
                onEnter_.Invoke();
            }
            entered_.Add(other);
        }

        protected virtual void OnTriggerExit2D(Collider2D other)
        {
            var removeSucceeded = entered_.Remove(other);

            if (removeSucceeded == false)
            {
                Debug.LogWarning("OnTriggerExit2D: Collider2D object left without first entering it?");
            }

            if (entered_.Count == 0)
            {
                onLeave_.Invoke();
            }
        }

        private readonly System.Predicate<Collider2D> IsNull = c => c == null;

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
