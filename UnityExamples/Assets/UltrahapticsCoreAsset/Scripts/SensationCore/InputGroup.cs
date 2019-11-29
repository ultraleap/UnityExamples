using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace UltrahapticsCoreAsset
{
    [Serializable]
    public class InputGroup : IEnumerable<SensationBlockInput>
    {
        public InputGroup(string name)
        {
            name_ = name;
        }

        [SerializeField] private string name_;
        public string Name
        {
            get { return name_; }
        }

        [SerializeField] private List<SensationBlockInput> members_ = new List<SensationBlockInput>();
        public SensationBlockInput this[int i]
        {
            get
            {
                if (i < members_.Count)
                {
                    return members_[i];
                }
                else
                {
                    return null;
                }
            }
        }
        public SensationBlockInput this[string id]
        {
            get
            {
                foreach (var input in members_)
                {
                    if (input.Name == id)
                    {
                        return input;
                    }
                }

                return null;
            }
        }

        public IEnumerator<SensationBlockInput> GetEnumerator()
        {
            return members_.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal void AddMember(SensationBlockInput input)
        {
            members_.Add(input);
        }

    }
}
