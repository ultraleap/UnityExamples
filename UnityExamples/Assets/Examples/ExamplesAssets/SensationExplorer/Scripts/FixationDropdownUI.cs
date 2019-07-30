using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace UltrahapticsCoreAsset.UnityExamples
{
    public class FixationDropdownUI : MonoBehaviour
    {

        public Dropdown fixationDrodown;

        /// <summary>
        /// HAND: Transform tracks the palm orientation
        /// FIXED: Transform based on freeform transform in space
        /// </summary>
        /// 
        public enum Fixation
        {
            HAND,
            FIXED
        };

        public static Dictionary<Fixation, string> Fixations =
        new Dictionary<Fixation, string>
        {
                {Fixation.HAND, "Palm"},
                {Fixation.FIXED, "Freeform"}
        };

        // Start is called before the first frame update
        void Start()
        {
            fixationDrodown.ClearOptions();

            var fixations = new List<string>();
            foreach (KeyValuePair<Fixation, string> fixation in Fixations)
            {
                fixations.Add(fixation.Value);
            }
            fixationDrodown.AddOptions(fixations);
        }
    }
}
