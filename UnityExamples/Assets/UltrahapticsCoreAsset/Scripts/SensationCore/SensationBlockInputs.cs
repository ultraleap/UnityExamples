using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace UltrahapticsCoreAsset
{
    [Serializable]
    public class SensationBlockInputs : IEnumerable<SensationBlockInput>
    {
        [SerializeField] private List<SensationBlockInput> inputs_ = new List<SensationBlockInput>();

        [SerializeField] private bool supportsTransform_ = false;

        public bool SupportsTransform
        {
            get { return supportsTransform_; }
        }
        [SerializeField] public UnityEngine.Transform TrackingObject = null;
        [NonSerialized] private IAutoMapper autoMapper_;

        public SensationBlockInputs(bool supportsTransform)
        {
            supportsTransform_ = supportsTransform;
        }

        public void ApplySaved(uhsclHandle inputSourceHandle)
        {
            if (TrackingObject != null)
            {
                foreach (var input in inputs_)
                {
                    if (input.Name == "virtualObjectXInVirtualSpace")
                    {
                        input.Value = TrackingObject.TransformVector(1, 0, 0);
                    }
                    else if (input.Name == "virtualObjectYInVirtualSpace")
                    {
                        input.Value = TrackingObject.TransformVector(0, 1, 0);
                    }
                    else if (input.Name == "virtualObjectZInVirtualSpace")
                    {
                        input.Value = TrackingObject.TransformVector(0, 0, 1);
                    }
                    else if (input.Name == "virtualObjectOriginInVirtualSpace")
                    {
                        input.Value = TrackingObject.position;
                    }
                }
            }

            for (int i = 0; i < inputs_.Count; i++)
            {
                var input = inputs_[i];

                if (input.Type == "Point")
                {
                    var transform = input.Object as UnityEngine.Transform;
                    if (transform != null)
                    {
                        input.Value = transform.position;
                    }
                    else
                    {
                        continue;
                    }
                }

                input.Apply(inputSourceHandle);
            }
        }

        public SensationBlockInput this[string name]
        {
            get
            {
                foreach (var input in inputs_)
                {
                    if (input.Name == name)
                    {
                        return input;
                    }
                }
                return null;
            }
        }

        public List<SensationBlockInput> ToList()
        {
            return inputs_;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<SensationBlockInput> GetEnumerator()
        {
            return inputs_.GetEnumerator();
        }

        public int Count
        {
            get { return inputs_.Count; }
        }

        public bool HasInput(string inputName)
        {
            return inputs_.Find( x => x.Name == inputName ) != null;
        }

        public static SensationBlockInputs DefaultInputsForBlock(uhsclHandle blockHandle, uhsclHandle inputSourceHandle)
        {
            SensationBlockInputs inputs = new SensationBlockInputs(SensationCore.Instance.BlockSupportsTransformField(blockHandle));

            int inputSize = SensationCore.Instance.InputCount(blockHandle);
            for (int i = 0; i < inputSize; i++)
            {
                var handle = SensationCore.Instance.GetInputAtIndex(blockHandle, i);
                var defaultValue = SensationCore.Instance.GetInputAsVector3ByIndex(inputSourceHandle, i);
                SensationBlockInput input = new SensationBlockInput{Handle = handle, Value = defaultValue};
                inputs.AddInput(input);
            }
            return inputs;
        }

        public InputGroups Groups()
        {
            var inputGroups = new InputGroups();

            for (var i = 0; i < inputs_.Count; i++)
            {
                var input = inputs_[i];
                if (input.Name != "t")
                {
                    if (input.InputGroup == null)
                    {
                        if (inputGroups[InputGroups.UncategorizedGroupId] == null)
                        {
                            inputGroups.Add(InputGroups.UncategorizedGroupId);
                        }
                        input.InputGroup = InputGroups.UncategorizedGroupId;
                        inputGroups[InputGroups.UncategorizedGroupId].AddMember(input);
                    }
                    else
                    {
                        var inputGroupName = input.InputGroup;
                        if (inputGroups[inputGroupName] == null)
                        {
                            inputGroups.Add(inputGroupName);
                        }
                        inputGroups[inputGroupName].AddMember(input);
                    }
                }
            }
            return inputGroups;
        }

        public void Clear()
        {
            DeregisterInputs();
            inputs_.Clear();
        }

        internal void AddInput(SensationBlockInput input)
        {
            inputs_.Add(input);
            if (autoMapper_)
            {
                autoMapper_.RegisterBlockInput(input);
            }
        }

        public void DeregisterInputs()
        {
            if (autoMapper_)
            {
                foreach (var input in inputs_)
                {
                    autoMapper_.DeregisterBlockInput(input);
                }
            }
        }

        public void RegisterAutoMapper()
        {
            if (!autoMapper_)
            {
                autoMapper_ = GameObject.FindObjectOfType<IAutoMapper>();
            }
        }

        public void RegisterInputs()
        {
            if (autoMapper_)
            {
                foreach (var input in inputs_)
                {
                    autoMapper_.RegisterBlockInput(input);
                }
            }
        }

        internal void ReassignInputsHandles(uhsclHandle blockHandle)
        {
            for (int i = 0; i < inputs_.Count; i++)
            {
                var handle = SensationCore.Instance.GetInputAtIndex(blockHandle, i);
                inputs_[i].Handle = handle;
            }
        }
    }
}
