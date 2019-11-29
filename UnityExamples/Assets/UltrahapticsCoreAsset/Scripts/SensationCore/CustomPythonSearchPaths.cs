using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UltrahapticsCoreAsset
{
    internal class CustomPythonSearchPaths
    {
        // All search paths are stored as absolute paths
        private readonly HashSet<string> setSearchPaths_ = new HashSet<string>();
        public readonly HashSet<string> AdditionalSearchPaths = new HashSet<string>();

        public string DefaultSearchPath { get { return Path.Combine(Application.streamingAssetsPath, "Python"); } } // Application.streamingAssetsPath cannot be called in a ctor

        public void AddSearchPath(string path)
        {
            if (!Directory.Exists(path))
            {
                UCA.Logger.LogWarning(string.Format("The custom search path directory \"{0}\" could not be found", path));
            }
            AdditionalSearchPaths.Add(path);
        }

        public void ResetApplied()
        {
            setSearchPaths_.Clear();
        }

        private void LoadPythonModulesInPath(ISensationCoreInterop interop, IntPtr sclInstance, string path)
        {
            if (Directory.Exists(path))
            {
                foreach (string file in System.IO.Directory.GetFiles(path))
                {
                    if (Path.GetExtension(file) == ".py")
                    {
                        var module = Path.GetFileNameWithoutExtension(file);
                        Console.Error.Write("Loading " + module + " ... ");
                        try
                        {
                            interop.uhsclImportPythonModule(sclInstance, module);
                            Console.Error.WriteLine("success");
                        }
                        catch
                        {
                            Console.Error.WriteLine("failed");
                        }
                    }
                }
            }
        }

        private void ApplySearchPath(ISensationCoreInterop interop, IntPtr sclInstance, string path)
        {
            if (!setSearchPaths_.Contains(path))
            {
                interop.uhsclAddSearchPath(sclInstance, path);
                setSearchPaths_.Add(path);
            }
        }

        public void Apply(ISensationCoreInterop interop, IntPtr sclInstance)
        {
            ApplySearchPath(interop, sclInstance, DefaultSearchPath);
            foreach (var path in AdditionalSearchPaths)
            {
                ApplySearchPath(interop, sclInstance, path);
            }

            LoadPythonModulesInPath(interop, sclInstance, DefaultSearchPath);
            foreach (var path in AdditionalSearchPaths)
            {
                LoadPythonModulesInPath(interop, sclInstance, path);
            }
        }

        private const string NoSearchPathsValidMessage = "No valid search paths for Sensation Blocks could be found.\n" +
                                                         "Please check that 'Assets/StreamingAssets/Python' directory exists, " +
                                                         "or your custom search paths exist in the project.";

        public void DisplayWarningIfNoSearchPathsAreValid()
        {
            if (!Directory.Exists(DefaultSearchPath) && !AdditionalSearchPaths.Any(path => Directory.Exists(path)))
            {
                UCA.Logger.LogWarning(NoSearchPathsValidMessage);
            }
        }

    }
}
