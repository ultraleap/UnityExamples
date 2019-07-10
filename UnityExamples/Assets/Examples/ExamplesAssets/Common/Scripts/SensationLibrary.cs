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
        [NonSerialized]
        public List<string> sensationList;

        // List of Sensation Block names to ignore
        public List<string> blackList;

        public BlockLibrary blockLibrary = new BlockLibrary();

        void Start()
        {
            BuildBlockLibrary();
        }

        void BuildBlockLibrary()
        {
            if (blockLibrary == null)
            {
                blockLibrary = new BlockLibrary();
            }
            sensationList = blockLibrary.SortedBlockList();
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