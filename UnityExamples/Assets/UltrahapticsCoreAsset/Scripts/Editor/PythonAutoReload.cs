using UnityEditor;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace UltrahapticsCoreAsset.Editor
{
    public class PythonAutoReload : AssetPostprocessor
    {

        public static void OnPostprocessAllAssets(string[] importedAssets,
            string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (importedAssets
                .Concat(deletedAssets)
                .Concat(movedAssets)
                .Concat(movedFromAssetPaths)
                .Any(f => Path.GetExtension(f).ToLower() == ".py")
            )
            {
                // We need to completely reset SCL to clear the python module cache
                if (SensationCore.Instance != null)
                {
                    var activeSensationSources = Resources.FindObjectsOfTypeAll<SensationSource>().ToList();
                    InvalidateSources(activeSensationSources);

                    SensationCore.Instance.Dispose();
                    SensationCore.ScriptReload();

                    var changedSensationSources = GetChangedSources(importedAssets, activeSensationSources);
                    var unchangedSensationSources = GetUnchangedSources(activeSensationSources, changedSensationSources);

                    RecreateBlocksWithDefaultInputs(changedSensationSources);
                    RecreateBlocksWithCurrentInputs(unchangedSensationSources);
                }
            }
        }

        private static List<SensationSource> GetChangedSources(string[] modifiedAssets,
                                                              List<SensationSource> activeSensationSources)
        {
            var pythonFiles = modifiedAssets.Where(x => x.EndsWith(".py")).ToList();
            var changedSensationSources = new List<SensationSource>();

            foreach (var file in pythonFiles)
            {
                foreach (var source in activeSensationSources)
                {
                    if (file.Contains(source.SensationBlock))
                    {
                        changedSensationSources.Add(source);
                    }
                }
            }
            return changedSensationSources;
        }

        private static List<SensationSource> GetUnchangedSources(List<SensationSource> activeSensationSources, List<SensationSource> changedSources)
        {
            var unchangedSensationSources = new List<SensationSource>();

            foreach (var source in activeSensationSources)
            {
                if (!changedSources.Contains(source))
                {
                    unchangedSensationSources.Add(source);
                }
            }

            return unchangedSensationSources;
        }

        private static void InvalidateSources(List<SensationSource> sensationSources)
        {
            foreach (var sensationSource in sensationSources)
            {
                sensationSource.ReleaseCurrentBlock();
                sensationSource.InvalidateListOfSensationBlocks();
            }
        }

        private static void RecreateBlocksWithCurrentInputs(List<SensationSource> sensationSources)
        {
            foreach (var sensationSource in sensationSources)
            {
                sensationSource.RecreateBlockWithCurrentInputs();
                sensationSource.setBlockValidAfterBlockLibraryReload();
                if(sensationSource.isActiveAndEnabled)
                {
                    sensationSource.Start();
                }
            }
        }

        private static void RecreateBlocksWithDefaultInputs(List<SensationSource> sensationSources)
        {
            foreach (var sensationSource in sensationSources)
            {
                sensationSource.RecreateBlockWithDefaultInputs();
                if(sensationSource.isActiveAndEnabled)
                {
                    sensationSource.Start();
                }
            }
        }
    }
}
