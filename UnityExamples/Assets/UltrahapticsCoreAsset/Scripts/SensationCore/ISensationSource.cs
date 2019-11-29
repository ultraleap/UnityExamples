using UnityEngine;

namespace UltrahapticsCoreAsset
{

    public abstract class ISensationSource : MonoBehaviour
    {
        public abstract string SensationBlock { get; set; }
        public abstract bool Running { get; set; }
        public abstract uint Priority { get; set; }

        public abstract uhsclHandle PlaybackInstanceHandle { get; }
    }
}
