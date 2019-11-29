using System;
using System.Linq;
using System.Collections.Generic;

namespace UltrahapticsCoreAsset
{

    using size_t = System.UInt64; // We should be using UIntPtr here, https://github.com/Moq/moq4/issues/42

    public class LogStreamReader : ILogStreamReader
    {


        const char LogDelimiter = '\0';

        Func<IntPtr, size_t> uhsclGetLogInfoMessageBufferSize_;
        Action<IntPtr, size_t, byte[]> uhsclGetLogInfoMessageBufferAndClear_;

        Func<IntPtr, size_t> uhsclGetLogWarningMessageBufferSize_;
        Action<IntPtr, size_t, byte[]> uhsclGetLogWarningMessageBufferAndClear_;

        Func<IntPtr, size_t> uhsclGetLogErrorMessageBufferSize_;
        Action<IntPtr, size_t, byte[]> uhsclGetLogErrorMessageBufferAndClear_;

        public LogStreamReader(ISensationCoreInterop sensationCoreInterop)
        {
            uhsclGetLogInfoMessageBufferSize_ = sensationCoreInterop.uhsclGetLogInfoMessageBufferSize;
            uhsclGetLogInfoMessageBufferAndClear_ = sensationCoreInterop.uhsclGetLogInfoMessageBufferAndClear;

            uhsclGetLogWarningMessageBufferSize_ = sensationCoreInterop.uhsclGetLogWarningMessageBufferSize;
            uhsclGetLogWarningMessageBufferAndClear_ = sensationCoreInterop.uhsclGetLogWarningMessageBufferAndClear;

            uhsclGetLogErrorMessageBufferSize_ = sensationCoreInterop.uhsclGetLogErrorMessageBufferSize;
            uhsclGetLogErrorMessageBufferAndClear_ = sensationCoreInterop.uhsclGetLogErrorMessageBufferAndClear;
        }

        private List<string> GetLogMessagesFromBuffer(byte[] buffer)
        {
            var logMessagesAsOneString = System.Text.Encoding.ASCII.GetString(buffer.Take(buffer.Length).ToArray());
            var logMessages = new List<string>(logMessagesAsOneString.Split(LogDelimiter));
            logMessages.RemoveAt(logMessages.Count - 1);
            return logMessages;
        }

        private readonly List<string> emptyList_ = new List<string>();
        private List<string> GetLogMessages(IntPtr sensationCoreInstancePtr,
                                            Func<IntPtr, size_t> getBufferSize,
                                            Action<IntPtr, size_t, byte[]> getLogMessagesInBufferAndClear)
        {
            var size = getBufferSize(sensationCoreInstancePtr);
            if (size > 0)
            {
                byte[] buffer = new byte[(int)size];
                getLogMessagesInBufferAndClear(sensationCoreInstancePtr, size, buffer);
                return GetLogMessagesFromBuffer(buffer);
            }
            else
            {
                return emptyList_;
            }
        }

        public List<string> GetInfoMessages(IntPtr sensationCoreInstancePtr)
        {
            return GetLogMessages(
                sensationCoreInstancePtr,
                uhsclGetLogInfoMessageBufferSize_,
                uhsclGetLogInfoMessageBufferAndClear_);
        }

        public List<string> GetWarningMessages(IntPtr sensationCoreInstancePtr)
        {
            return GetLogMessages(
                sensationCoreInstancePtr,
                uhsclGetLogWarningMessageBufferSize_,
                uhsclGetLogWarningMessageBufferAndClear_);
        }

        public List<string> GetErrorMessages(IntPtr sensationCoreInstancePtr)
        {
            return GetLogMessages(
                sensationCoreInstancePtr,
                uhsclGetLogErrorMessageBufferSize_,
                uhsclGetLogErrorMessageBufferAndClear_);
        }
    }

}
