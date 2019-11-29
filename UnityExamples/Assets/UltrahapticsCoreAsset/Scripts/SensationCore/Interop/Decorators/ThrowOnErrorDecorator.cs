using System;
using System.Diagnostics;
using System.IO;

namespace UltrahapticsCoreAsset
{
    class ThrowOnErrorDecorator : ISensationCoreInterop
    {
        private ISensationCoreInterop interop_;

        private StackTrace GetOriginalStackTrace()
        {
            var parentFrameIdx = 1;
            var trace = new StackTrace();
            while (true)
            {
                var frame = trace.GetFrame(parentFrameIdx);
                var method = frame.GetMethod();
                if (method.DeclaringType.FullName.Contains("Ultrahaptics"))
                {
                    return new StackTrace(parentFrameIdx, true);
                }
                parentFrameIdx++;
            }
        }

        public uhsclErrorCode_t ThrowOnError(uhsclErrorCode_t errorCode)
        {
            if (errorCode == uhsclErrorCode_t.NoError)
            {
                return errorCode;
            }
            else
            {
                var originalStackTraceMsg = "\n" + GetOriginalStackTrace();
                switch (errorCode)
                {
                    case uhsclErrorCode_t.UninitialisedSensationCore:
                        throw new Exception("SensationCore not initialised" + originalStackTraceMsg);
                    case uhsclErrorCode_t.InvalidHandle:
                        throw new ArgumentException("Invalid handle" + originalStackTraceMsg);
                    case uhsclErrorCode_t.InvalidArgument:
                        throw new ArgumentException("Invalid argument" + originalStackTraceMsg);
                    case uhsclErrorCode_t.InvalidOperation:
                        throw new InvalidOperationException("Invalid operation" + originalStackTraceMsg);
                    case uhsclErrorCode_t.HardwareError:
                        throw new IOException("Hardware Error" + originalStackTraceMsg);
                    default:
                        throw new Exception("Unknown error: " + (int) errorCode + originalStackTraceMsg);
                }
            }
        }

        public ThrowOnErrorDecorator(ISensationCoreInterop interop)
        {
            interop_ = interop;
        }

        public IntPtr uhsclCreate()
        {
            return interop_.uhsclCreate();
        }

        public void uhsclRelease(IntPtr sensationCoreInstancePtr)
        {
            interop_.uhsclRelease(sensationCoreInstancePtr);
        }

        public uhsclErrorCode_t uhsclImportPythonModule(IntPtr instance, string modulename)
        {
            return ThrowOnError(interop_.uhsclImportPythonModule(instance, modulename));
        }

        public uhsclErrorCode_t uhsclGetNameLength(IntPtr sensationCoreInstancePtr, uhsclHandle handle, out ulong count)
        {
            return ThrowOnError(interop_.uhsclGetNameLength(sensationCoreInstancePtr, handle, out count));
        }

        public uhsclErrorCode_t uhsclGetName(IntPtr sensationCoreInstancePtr, uhsclHandle handle, ulong nameBufferLength, byte[] nameBuffer)
        {
            return ThrowOnError(interop_.uhsclGetName(sensationCoreInstancePtr, handle, nameBufferLength, nameBuffer));
        }

        public uhsclErrorCode_t uhsclCreateBlock(IntPtr sensationCoreInstancePtr, string identifier, out uhsclHandle handle)
        {
            return ThrowOnError(interop_.uhsclCreateBlock(sensationCoreInstancePtr, identifier, out handle));
        }

        public uhsclErrorCode_t uhsclCreateInputSource(IntPtr sensationCoreInstancePtr, uhsclHandle blockHandle,
            out uhsclHandle inputSourceHandle)
        {
            return ThrowOnError(interop_.uhsclCreateInputSource(sensationCoreInstancePtr, blockHandle, out inputSourceHandle));
        }

        public uhsclErrorCode_t uhsclStart(IntPtr sensationCoreInstancePtr, uhsclHandle outputHandle, uhsclHandle inputSourceHandle, out uhsclHandle playbackInstanceHandle)
        {
            return ThrowOnError(interop_.uhsclStart(sensationCoreInstancePtr, outputHandle, inputSourceHandle, out playbackInstanceHandle));
        }

        public uhsclErrorCode_t uhsclStop(IntPtr sensationCoreInstancePtr, uhsclHandle handle)
        {
            return ThrowOnError(interop_.uhsclStop(sensationCoreInstancePtr, handle));
        }

        public uhsclErrorCode_t uhsclMute(IntPtr sensationCoreInstancePtr, uhsclHandle handle)
        {
            return ThrowOnError(interop_.uhsclMute(sensationCoreInstancePtr, handle));
        }

        public uhsclErrorCode_t uhsclUnmute(IntPtr sensationCoreInstancePtr, uhsclHandle handle)
        {
            return ThrowOnError(interop_.uhsclUnmute(sensationCoreInstancePtr, handle));
        }

        public uhsclErrorCode_t uhsclIsCurrentlyPlaying(IntPtr sensationCoreInstancePtr, out bool isPlaying)
        {
            return ThrowOnError(interop_.uhsclIsCurrentlyPlaying(sensationCoreInstancePtr, out isPlaying));
        }

        public uhsclErrorCode_t uhsclGetCurrentlyPlayingInstance(IntPtr sensationCoreInstancePtr,out uhsclHandle currentlyPlayingInstance)
        {
            return ThrowOnError(interop_.uhsclGetCurrentlyPlayingInstance(sensationCoreInstancePtr,out currentlyPlayingInstance));
        }

        public uhsclErrorCode_t uhsclSetPriority(IntPtr sensationCoreInstancePtr, uhsclHandle handle, uint priority)
        {
            return ThrowOnError(interop_.uhsclSetPriority(sensationCoreInstancePtr, handle, priority));
        }

        public uhsclErrorCode_t uhsclAcquireEmitter(IntPtr sensationCoreInstancePtr)
        {
            return ThrowOnError(interop_.uhsclAcquireEmitter(sensationCoreInstancePtr));
        }

        public uhsclErrorCode_t uhsclAcquireMockEmitter(IntPtr sensationCoreInstancePtr, string deviceModel, string logFilePath)
        {
            return ThrowOnError(interop_.uhsclAcquireMockEmitter(sensationCoreInstancePtr, deviceModel, logFilePath));
        }

        public uhsclErrorCode_t uhsclAddDevice(IntPtr sensationCoreInstancePtr, string deviceId, float[] deviceTransform)
        {
            return ThrowOnError(interop_.uhsclAddDevice(sensationCoreInstancePtr, deviceId, deviceTransform));
        }

        public uhsclErrorCode_t uhsclSetDeviceTransform(IntPtr sensationCoreInstancePtr, string deviceId, float[] deviceTransform)
        {
            return ThrowOnError(interop_.uhsclSetDeviceTransform(sensationCoreInstancePtr, deviceId, deviceTransform));
        }

        public uhsclErrorCode_t uhsclGetConnectedDevicesStringLength(IntPtr sensationCoreInstancePtr, out ulong count)
        {
            return ThrowOnError(interop_.uhsclGetConnectedDevicesStringLength(sensationCoreInstancePtr, out count));
        }

        public uhsclErrorCode_t uhsclGetConnectedDevices(IntPtr sensationCoreInstancePtr, ulong deviceIdsBufferLength, byte[] deviceIdsBuffer)
        {
            return ThrowOnError(interop_.uhsclGetConnectedDevices(sensationCoreInstancePtr, deviceIdsBufferLength, deviceIdsBuffer));
        }

        public int uhsclIsEmitterConnected(IntPtr sensationCoreInstancePtr)
        {
            return interop_.uhsclIsEmitterConnected(sensationCoreInstancePtr);
        }

        public uhsclErrorCode_t uhsclReleaseEmitter(IntPtr sensationCoreInstancePtr)
        {
            return ThrowOnError(interop_.uhsclReleaseEmitter(sensationCoreInstancePtr));
        }

        public uhsclErrorCode_t uhsclEmitterModelDescriptionStringLength(IntPtr sensationCoreInstancePtr, out ulong length)
        {
            return ThrowOnError(interop_.uhsclEmitterModelDescriptionStringLength(sensationCoreInstancePtr, out length));
        }

        public uhsclErrorCode_t uhsclEmitterSerialNumberStringLength(IntPtr sensationCoreInstancePtr, out ulong length)
        {
            return ThrowOnError(interop_.uhsclEmitterSerialNumberStringLength(sensationCoreInstancePtr, out length));
        }

        public uhsclErrorCode_t uhsclEmitterFirmwareVersionStringLength(IntPtr sensationCoreInstancePtr, out ulong length)
        {
            return ThrowOnError(interop_.uhsclEmitterFirmwareVersionStringLength(sensationCoreInstancePtr, out length));
        }

        public uhsclErrorCode_t uhsclEmitterModelDescription(IntPtr sensationCoreInstancePtr, ulong allocatedLength, byte[] buffer)
        {
            return ThrowOnError(interop_.uhsclEmitterModelDescription(sensationCoreInstancePtr, allocatedLength, buffer));
        }

        public uhsclErrorCode_t uhsclEmitterSerialNumber(IntPtr sensationCoreInstancePtr, ulong allocatedLength, byte[] buffer)
        {
            return ThrowOnError(interop_.uhsclEmitterSerialNumber(sensationCoreInstancePtr, allocatedLength, buffer));
        }

        public uhsclErrorCode_t uhsclEmitterFirmwareVersion(IntPtr sensationCoreInstancePtr, ulong allocatedLength, byte[] buffer)
        {
            return ThrowOnError(interop_.uhsclEmitterFirmwareVersion(sensationCoreInstancePtr, allocatedLength, buffer));
        }

        public uhsclErrorCode_t uhsclInputCount(IntPtr sensationCoreInstancePtr, uhsclHandle handle, out ulong count)
        {
            return ThrowOnError(interop_.uhsclInputCount(sensationCoreInstancePtr, handle, out count));
        }

        public uhsclErrorCode_t uhsclGetInputAtIndex(IntPtr sensationCoreInstancePtr, uhsclHandle handle, int idx, out uhsclHandle inputHandle)
        {
            return ThrowOnError(interop_.uhsclGetInputAtIndex(sensationCoreInstancePtr, handle, idx, out inputHandle));
        }

        public uhsclErrorCode_t uhsclOutputCount(IntPtr sensationCoreInstancePtr, uhsclHandle handle, out ulong count)
        {
            return ThrowOnError(interop_.uhsclOutputCount(sensationCoreInstancePtr, handle, out count));
        }

        public uhsclErrorCode_t uhsclGetOutputAtIndex(IntPtr sensationCoreInstancePtr, uhsclHandle handle, int idx, out uhsclHandle outputHandle)
        {
            return ThrowOnError(interop_.uhsclGetOutputAtIndex(sensationCoreInstancePtr, handle, idx, out outputHandle));
        }

        public uhsclErrorCode_t uhsclSetInputToUhsclVector3(IntPtr sensationCoreInstancePtr, uhsclHandle inputSourceHandle, uhsclHandle inputHandle, uhsclVector3_t inputVector)
        {
            return ThrowOnError(interop_.uhsclSetInputToUhsclVector3(sensationCoreInstancePtr, inputSourceHandle, inputHandle, inputVector));
        }

        public uhsclErrorCode_t uhsclGetInputAsUhsclVector3ByIndex(IntPtr sensationCoreInstancePtr, uhsclHandle handle, int idx, out uhsclVector3_t value)
        {
            return ThrowOnError(interop_.uhsclGetInputAsUhsclVector3ByIndex(sensationCoreInstancePtr, handle, idx, out value));
        }

        public uhsclErrorCode_t uhsclUpdate(IntPtr sensationCoreInstancePtr)
        {
            return ThrowOnError(interop_.uhsclUpdate(sensationCoreInstancePtr));
        }

        public uhsclErrorCode_t uhsclSetEvaluationHistorySizeOnCurrentPlaybackDevice(IntPtr sensationCoreInstancePtr, uint size)
        {
            return ThrowOnError(interop_.uhsclSetEvaluationHistorySizeOnCurrentPlaybackDevice(sensationCoreInstancePtr, size));
        }

        public uhsclErrorCode_t uhsclGetEvaluationHistoryOnCurrentPlaybackDevice(IntPtr sensationCoreInstancePtr, uhsclVector4_t[] evaluationHistory, out uint size)
        {
            return ThrowOnError(interop_.uhsclGetEvaluationHistoryOnCurrentPlaybackDevice(sensationCoreInstancePtr, evaluationHistory, out size));
        }

        public uhsclErrorCode_t uhsclAddSearchPath(IntPtr sensationCoreInstancePtr, string path)
        {
            return ThrowOnError(interop_.uhsclAddSearchPath(sensationCoreInstancePtr, path));
        }

        public uhsclErrorCode_t uhsclGetOutputByName(IntPtr sensationCoreInstancePtr, uhsclHandle blockDefinitionHandle, string outputName, out uhsclHandle blockOutputHandle)
        {
            return ThrowOnError(interop_.uhsclGetOutputByName(sensationCoreInstancePtr, blockDefinitionHandle, outputName, out blockOutputHandle));
        }

        public uhsclErrorCode_t uhsclGetInputByName(IntPtr sensationCoreInstancePtr, uhsclHandle blockDefinitionHandle, string inputName, out uhsclHandle blockInputHandle)
        {
            return ThrowOnError(interop_.uhsclGetInputByName(sensationCoreInstancePtr, blockDefinitionHandle, inputName, out blockInputHandle));
        }

        public uhsclErrorCode_t uhsclGetMetaDataBool(IntPtr sensationCoreInstancePtr, uhsclHandle handle, string identifier, out bool metadataValue)
        {
            return ThrowOnError(interop_.uhsclGetMetaDataBool(sensationCoreInstancePtr, handle, identifier, out metadataValue));
        }

        public uhsclErrorCode_t uhsclGetMetaDataString(IntPtr sensationCoreInstancePtr, uhsclHandle handle, string identifier, ulong metadataBufferLength, byte[] metadataValue)
        {
            return ThrowOnError(interop_.uhsclGetMetaDataString(sensationCoreInstancePtr, handle, identifier, metadataBufferLength, metadataValue));
        }

        public uhsclErrorCode_t uhsclGetMetaDataStringLength(IntPtr sensationCoreInstancePtr, uhsclHandle handle, string identifier, out ulong count)
        {
            return ThrowOnError(interop_.uhsclGetMetaDataStringLength(sensationCoreInstancePtr, handle, identifier, out count));
        }

        public ulong uhsclGetLogInfoMessageBufferSize(IntPtr sensationCoreInstancePtr)
        {
            return interop_.uhsclGetLogInfoMessageBufferSize(sensationCoreInstancePtr);
        }

        public ulong uhsclGetLogWarningMessageBufferSize(IntPtr sensationCoreInstancePtr)
        {
            return interop_.uhsclGetLogWarningMessageBufferSize(sensationCoreInstancePtr);
        }

        public ulong uhsclGetLogErrorMessageBufferSize(IntPtr sensationCoreInstancePtr)
        {
            return interop_.uhsclGetLogErrorMessageBufferSize(sensationCoreInstancePtr);
        }

        public void uhsclGetLogInfoMessageBufferAndClear(IntPtr sensationCoreInstancePtr, ulong logBufferLength, byte[] logBuffer)
        {
            interop_.uhsclGetLogInfoMessageBufferAndClear(sensationCoreInstancePtr, logBufferLength, logBuffer);
        }

        public void uhsclGetLogWarningMessageBufferAndClear(IntPtr sensationCoreInstancePtr, ulong logBufferLength, byte[] logBuffer)
        {
            interop_.uhsclGetLogWarningMessageBufferAndClear(sensationCoreInstancePtr, logBufferLength, logBuffer);
        }

        public void uhsclGetLogErrorMessageBufferAndClear(IntPtr sensationCoreInstancePtr, ulong logBufferLength, byte[] logBuffer)
        {
            interop_.uhsclGetLogErrorMessageBufferAndClear(sensationCoreInstancePtr, logBufferLength, logBuffer);
        }

        public uhsclErrorCode_t uhsclGetBlockCount(IntPtr sensationCoreInstancePtr, out ulong blockCount)
        {
            return ThrowOnError(interop_.uhsclGetBlockCount(sensationCoreInstancePtr, out blockCount));
        }

        public uhsclErrorCode_t uhsclGetBlockHandleAtIndex(IntPtr sensationCoreInstancePtr, ulong index, out uhsclHandle handle)
        {
            return ThrowOnError(interop_.uhsclGetBlockHandleAtIndex(sensationCoreInstancePtr, index, out handle));
        }
    }
}
