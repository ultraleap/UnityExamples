using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UltrahapticsCoreAsset.UnityExamples
{
    public class SensationLibrary : MonoBehaviour
    {

        // The full list that SensationCore knows about
        public List<string> sensationList;

        // List of Sensation Block names to ignore
        [SerializeField] public List<string> whiteList;

        // List of Sensation Block names to ignore
        [SerializeField] public List<string> blackList;

        private BlockLibrary blockLibrary;

        public void BuildBlockLibrary()
        {
            if (blockLibrary == null)
            {
                blockLibrary = new BlockLibrary();
            }

            // Build the full sorted list
            List<string> sortedBlockList = blockLibrary.SortedBlockList();

            // if the White list is empty, assume all is required, except for blacklisted.
            if (whiteList.Count == 0)
            {
                sensationList = sortedBlockList.Except(blackList).ToList();
            }
            // Otherwise, only include WhiteListed items.
            else
            {
                sensationList = whiteList.ToList();
                // A final filter just in case White list contains non-available Sensations
                foreach (string sensationName in sensationList)
                {
                    if (!sortedBlockList.Contains(sensationName))
                    {
                        Debug.LogWarning("Unable to locate Sensation named: " + sensationName);
                        whiteList.Remove(sensationName);
                    }
                }
                sensationList = whiteList;
            }
        }
    }

    public class BlockLibrary
    {
        private List<string> _sensationsList;

        [OnSensationCoreInitialize]
        public static void AddSearchPath()
        {
            var searchPath = Path.Combine(Application.streamingAssetsPath, "Python/BlockLibraries/UnityExamples");
            Debug.Log("Adding Search Path for UnityExamples:" + searchPath);
            SensationCore.AddSearchPath(searchPath);
        }

        public List<string> SortedBlockList()
        {
            var sc = SensationCore.Instance ?? FindObjectOfType<SensationCore>();
            if (sc == null)
            {
                throw new Exception("No SensationCore found");
            }

            _sensationsList = _sensationsList ?? sc.GetSensationProducingBlockNames();
            var sortedSensations = _sensationsList.OrderBy(s => s);
            return sortedSensations.ToList();
        }

        private ISensationCore FindObjectOfType<T>()
        {
            throw new NotImplementedException();
        }
    }
}