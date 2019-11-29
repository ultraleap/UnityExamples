using UnityEngine;

namespace UltrahapticsCoreAsset
{
    public abstract class IAutoMapper : MonoBehaviour
    {
        public abstract void RegisterDataSource(IDataSource dataSource);
        public abstract void RegisterBlockInput(SensationBlockInput blockInput);

        public abstract void DeregisterDataSource(IDataSource dataSource);
        public abstract void DeregisterBlockInput(SensationBlockInput blockInput);

        public abstract bool HasValueForInputName(string input);
        public abstract UnityEngine.Vector3 GetValueForInputName(string input);
    }
}
