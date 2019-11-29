using System;
using UnityEngine;

namespace UltrahapticsCoreAsset
{
    [Serializable]
    public class SensationBlockInput
    {
        public string Name;
        public UnityEngine.Vector3 Value;
        public UnityEngine.Object Object;
        public bool IsVisible;
        public string Type;
        public string InputGroup;

        [SerializeField] private uhsclHandle handle_;
        [NonSerialized] private UnityEngine.Vector3 previousValue_;
        [NonSerialized] private bool isDirty_ = true;

        private bool IsDirty
        {
            get { return isDirty_; }
            set { isDirty_ = value; }
        }

        internal uhsclHandle Handle
        {
            set
            {
                handle_ = value;
                IsDirty = true;
                Name = SensationCore.Instance.HandleName(handle_);
                try
                {
                    IsVisible = SensationCore.Instance.GetMetaData<bool>(handle_, "Input-Visibility");
                }
                catch (ArgumentException)
                {
                    IsVisible = true;
                }

                try
                {
                    Type = SensationCore.Instance.GetMetaData<string>(handle_, "Type");
                }
                catch (ArgumentException)
                {
                    Type = "";
                }

                if (!IsVisible)
                {
                    InputGroup = InputGroups.HiddenGroupId;
                }
                else
                {
                    try
                    {
                        InputGroup = SensationCore.Instance.GetMetaData<string>(handle_, "Input-Group");
                    }
                    catch (ArgumentException)
                    {
                        InputGroup = null;
                    }
                }

            }
        }

        private bool HasChangedSinceLastApply()
        {
            return previousValue_ != Value || IsDirty;
        }

        internal void Apply(uhsclHandle inputSource)
        {
            if (HasChangedSinceLastApply())
            {
                SensationCore.Instance.SetInputToVector3(inputSource, handle_, Value);
                IsDirty = false;
                previousValue_ = Value;
            }
        }

        public T GetMetaData<T>(string identifier)
        {
            return SensationCore.Instance.GetMetaData<T>(handle_, identifier);
        }
    }
}
