using UnityEngine;
using UnityEngine.Playables;

namespace UltrahapticsCoreAsset.Examples
{
    public class ParticleMotion : MonoBehaviour
    {
        public float Speed = 100.0f;
        public int Direction = 0;
        public Collider2D BarCollider;
        public PlayableDirector CursorTimeline;
        public BarAnimation BarAnimator;

        void Start()
        {
            switch (Direction)
            {
                case 0:
                    GetComponent<Rigidbody2D>().velocity = (Vector2.left + Vector2.up) * Speed;
                    break;
                case 1:
                    GetComponent<Rigidbody2D>().velocity = (Vector2.right + Vector2.up) * Speed;
                    break;
                case 2:
                    GetComponent<Rigidbody2D>().velocity = (Vector2.left + Vector2.down) * Speed;
                    break;
                default:
                    GetComponent<Rigidbody2D>().velocity = (Vector2.right + Vector2.down) * Speed;
                    break;
            }
        }

        // We don't trigger the animation of the Cursor on Particle collision IF the Bar is
        // currently in its HandScan cycle
        public void PlayAnimationIfBarInterruptible()
        {
            if (BarAnimator.Interruptible)
            {
                CursorTimeline.Play();
            }
        }

        private void FixedUpdate()
        {
            // Ensure that the Bar Object does not affect this Particle object
            Physics2D.IgnoreCollision(BarCollider, GetComponent<Collider2D>());
        }
    }
}
