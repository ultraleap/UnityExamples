using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace UltrahapticsCoreAsset
{
    [Serializable]
    public class InputGroups : IEnumerable<InputGroup>
    {
        public static string UncategorizedGroupId = "uncategorized";
        public static string HiddenGroupId = "Hidden Inputs";
        [SerializeField] private List<InputGroup> groups_ = new List<InputGroup>();
        public InputGroup this[string id]
        {
            get
            {
                foreach (var inputGroup in groups_)
                {
                    if (inputGroup.Name == id)
                    {
                        return inputGroup;
                    }
                }

                return null;
            }
        }

        public IEnumerator<InputGroup> GetEnumerator()
        {
            return groups_.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static void SortInputGroupsForRenderingOrder(List<InputGroup> inputGroups)
        {
            inputGroups.Sort(
                delegate(InputGroup lhs, InputGroup rhs)
                {
                    if (lhs.Name == HiddenGroupId || rhs.Name == UncategorizedGroupId)
                    {
                        return 1;
                    }
                    else if (rhs.Name == HiddenGroupId || lhs.Name == UncategorizedGroupId)
                    {
                        return -1;
                    }
                    else
                    {
                        return String.Compare(lhs.Name, rhs.Name, StringComparison.Ordinal);
                    }
                });
        }

        public void Add(string inputGroupName)
        {
            groups_.Add(new InputGroup(inputGroupName));
            SortInputGroupsForRenderingOrder(groups_);
        }

        public void Clear()
        {
            groups_.Clear();
        }
    }
}
