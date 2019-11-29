using System;
using System.Collections.Generic;

namespace UltrahapticsCoreAsset
{
    public interface ILogStreamReader
    {
        List<string> GetInfoMessages(IntPtr sensationCoreInstancePtr);
        List<string> GetWarningMessages(IntPtr sensationCoreInstancePtr);
        List<string> GetErrorMessages(IntPtr sensationCoreInstancePtr);
    }

}
