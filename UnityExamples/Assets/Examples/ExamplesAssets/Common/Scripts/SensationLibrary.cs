using UnityEngine;
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

        // This JSON file will allow users to set the white/black list of sensations
        private string libraryListStreamingAssetFilename = "/SensationExplorerLibraryList.json";

        [Serializable]
        public class SensationIncludeExcludeList
        {
            public List<string> includeList;
            public List<string> excludeList;
        }

        private BlockLibrary blockLibrary;
        public SensationIncludeExcludeList includeExcludeList;

        public void BuildBlockLibrary()
        {
            if (blockLibrary == null)
            {
                blockLibrary = new BlockLibrary();
            }

            string includeExcludeJSON = File.ReadAllText(Application.streamingAssetsPath + libraryListStreamingAssetFilename);
            includeExcludeList = JsonUtility.FromJson<SensationIncludeExcludeList>(includeExcludeJSON);

            // Build the full sorted list
            List<string> sortedBlockList = blockLibrary.SortedBlockList();

            // if the include list is empty, assume all is required, except for excludelisted.
            if (includeExcludeList.includeList.Count == 0)
            {
                sensationList = sortedBlockList.Except(includeExcludeList.excludeList).ToList();
            }
            // Otherwise, only include includeListed items.
            else
            {
                sensationList = includeExcludeList.includeList.ToList();
                // A final filter just in case include list contains non-available Sensations
                foreach (string sensationName in sensationList)
                {
                    if (!sortedBlockList.Contains(sensationName))
                    {
                        Debug.LogWarning("Unable to locate Sensation named: " + sensationName);
                        includeExcludeList.includeList.Remove(sensationName);
                    }
                }
                sensationList = includeExcludeList.includeList;
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