using System;

namespace UltrahapticsCoreAsset
{
    class LockDecorator : ISensationCoreInterop
    {
        private object lock_ = new object();
        private ISensationCoreInterop interop_;
        public LockDecorator(ISensationCoreInterop interop)
        {
            interop_ = interop;
        }

        public IntPtr uhsclCreate()
        {
            lock (lock_)
            {
                return interop_.uhsclCreate();
            }
        }

        public void uhsclRelease(IntPtr sensationCoreInstancePtr)
        {
            lock (lock_)
            {
                interop_.uhsclRelease(sensationCoreInstancePtr);
            }
        }

        public uhsclErrorCode_t uhsclImportPythonModule(IntPtr instance, string modulename)
        {
            lock (lock_)
            {
                return interop_.uhsclImportPythonModule(instance, modulename);
            }
        }

        public uhsclErrorCode_t uhsclGetNameLength(IntPtr sensationCoreInstancePtr, uhsclHandle handle, out ulong count)
        {
            lock (lock_)
            {
                return interop_.uhsclGetNameLength(sensationCoreInstancePtr, handle, out count);
            }
        }

        public uhsclErrorCode_t uhsclGetName(IntPtr sensationCoreInstancePtr, uhsclHandle handle, ulong nameBufferLength, byte[] nameBuffer)
        {
            lock (lock_)
            {
                return interop_.uhsclGetName(sensationCoreInstancePtr, handle, nameBufferLength, nameBuffer);
            }
        }

        public uhsclErrorCode_t uhsclCreateBlock(IntPtr sensationCoreInstancePtr, string identifier, out uhsclHandle handle)
        {
            lock (lock_)
            {
                return interop_.uhsclCreateBlock(sensationCoreInstancePtr, identifier, out handle);
            }
        }

        public uhsclErrorCode_t uhsclCreateInputSource(IntPtr sensationCoreInstancePtr, uhsclHandle blockHandle,
            out uhsclHandle inputSourceHandle)
        {
            lock (lock_)
            {
                return interop_.uhsclCreateInputSource(sensationCoreInstancePtr, blockHandle, out inputSourceHandle);
            }
        }

        public uhsclErrorCode_t uhsclStart(IntPtr sensationCoreInstancePtr, uhsclHandle outputHandle, uhsclHandle inputSourceHandle, out uhsclHandle playbackInstanceHandle)
        {
            lock (lock_)
            {
                return interop_.uhsclStart(sensationCoreInstancePtr, outputHandle, inputSourceHandle, out playbackInstanceHandle);
            }
        }

        public uhsclErrorCode_t uhsclStop(IntPtr sensationCoreInstancePtr, uhsclHandle handle)
        {
            lock (lock_)
            {
                return interop_.uhsclStop(sensationCoreInstancePtr, handle);
            }
        }

        public uhsclErrorCode_t uhsclMute(IntPtr sensationCoreInstancePtr, uhsclHandle handle)
        {
            lock (lock_)
            {
                return interop_.uhsclMute(sensationCoreInstancePtr, handle);
            }
        }

        public uhsclErrorCode_t uhsclUnmute(IntPtr sensationCoreInstancePtr, uhsclHandle handle)
        {
            lock (lock_)
            {
                return interop_.uhsclUnmute(sensationCoreInstancePtr, handle);
            }
        }

        public uhsclErrorCode_t uhsclIsCurrentlyPlaying(IntPtr sensationCoreInstancePtr, out bool isPlaying)
        {
            lock (lock_)
            {
                return interop_.uhsclIsCurrentlyPlaying(sensationCoreInstancePtr, out isPlaying);
            }
        }

        public uhsclErrorCode_t uhsclGetCurrentlyPlayingInstance(IntPtr sensationCoreInstancePtr,out uhsclHandle currentlyPlayingInstance)
        {
            lock (lock_)
            {
                return interop_.uhsclGetCurrentlyPlayingInstance(sensationCoreInstancePtr,out currentlyPlayingInstance);
            }
        }

        public uhsclErrorCode_t uhsclSetPriority(IntPtr sensationCoreInstancePtr, uhsclHandle handle, uint priority)
        {
            lock (lock_)
            {
                return interop_.uhsclSetPriority(sensationCoreInstancePtr, handle, priority);
            }
        }

        public uhsclErrorCode_t uhsclAcquireEmitter(IntPtr sensationCoreInstancePtr)
        {
            lock (lock_)
            {
                return interop_.uhsclAcquireEmitter(sensationCoreInstancePtr);
            }
        }

        public uhsclErrorCode_t uhsclAcquireMockEmitter(IntPtr sensationCoreInstancePtr, string deviceModel, string logFilePath)
        {
            lock (lock_)
            {
                return interop_.uhsclAcquireMockEmitter(sensationCoreInstancePtr, deviceModel, logFilePath);
            }
        }

        public uhsclErrorCode_t uhsclAddDevice(IntPtr sensationCoreInstancePtr, string deviceId, float[] deviceTransform)
        {
            lock (lock_)
            {
                return interop_.uhsclAddDevice(sensationCoreInstancePtr, deviceId, deviceTransform);
            }
        }

        public uhsclErrorCode_t uhsclSetDeviceTransform(IntPtr sensationCoreInstancePtr, string deviceId, float[] deviceTransform)
        {
            lock (lock_)
            {
                return interop_.uhsclSetDeviceTransform(sensationCoreInstancePtr, deviceId, deviceTransform);
            }
        }

        public uhsclErrorCode_t uhsclGetConnectedDevicesStringLength(IntPtr sensationCoreInstancePtr, out ulong count)
        {
            lock (lock_)
            {
                return interop_.uhsclGetConnectedDevicesStringLength(sensationCoreInstancePtr, out count);
            }
        }

        public uhsclErrorCode_t uhsclGetConnectedDevices(IntPtr sensationCoreInstancePtr, ulong deviceIdsBufferLength, byte[] deviceIdsBuffer)
        {
            lock (lock_)
            {
                return interop_.uhsclGetConnectedDevices(sensationCoreInstancePtr, deviceIdsBufferLength, deviceIdsBuffer);
            }
        }

        public int uhsclIsEmitterConnected(IntPtr sensationCoreInstancePtr)
        {
            lock (lock_)
            {
                return interop_.uhsclIsEmitterConnected(sensationCoreInstancePtr);
            }
        }

        public uhsclErrorCode_t uhsclReleaseEmitter(IntPtr sensationCoreInstancePtr)
        {
            lock (lock_)
            {
                return interop_.uhsclReleaseEmitter(sensationCoreInstancePtr);
            }
        }

        public uhsclErrorCode_t uhsclEmitterModelDescriptionStringLength(IntPtr sensationCoreInstancePtr, out ulong length)
        {
            lock (lock_)
            {
                return interop_.uhsclEmitterModelDescriptionStringLength(sensationCoreInstancePtr, out length);
            }
        }

        public uhsclErrorCode_t uhsclEmitterSerialNumberStringLength(IntPtr sensationCoreInstancePtr, out ulong length)
        {
            lock (lock_)
            {
                return interop_.uhsclEmitterSerialNumberStringLength(sensationCoreInstancePtr, out length);
            }
        }

        public uhsclErrorCode_t uhsclEmitterFirmwareVersionStringLength(IntPtr sensationCoreInstancePtr, out ulong length)
        {
            lock (lock_)
            {
                return interop_.uhsclEmitterFirmwareVersionStringLength(sensationCoreInstancePtr, out length);
            }
        }

        public uhsclErrorCode_t uhsclEmitterModelDescription(IntPtr sensationCoreInstancePtr, ulong allocatedLength, byte[] buffer)
        {
            lock (lock_)
            {
                return interop_.uhsclEmitterModelDescription(sensationCoreInstancePtr, allocatedLength, buffer);
            }
        }

        public uhsclErrorCode_t uhsclEmitterSerialNumber(IntPtr sensationCoreInstancePtr, ulong allocatedLength, byte[] buffer)
        {
            lock (lock_)
            {
                return interop_.uhsclEmitterSerialNumber(sensationCoreInstancePtr, allocatedLength, buffer);
            }
        }

        public uhsclErrorCode_t uhsclEmitterFirmwareVersion(IntPtr sensationCoreInstancePtr, ulong allocatedLength, byte[] buffer)
        {
            lock (lock_)
            {
                return interop_.uhsclEmitterFirmwareVersion(sensationCoreInstancePtr, allocatedLength, buffer);
            }
        }

        public uhsclErrorCode_t uhsclInputCount(IntPtr sensationCoreInstancePtr, uhsclHandle handle, out ulong count)
        {
            lock (lock_)
            {
                return interop_.uhsclInputCount(sensationCoreInstancePtr, handle, out count);
            }
        }

        public uhsclErrorCode_t uhsclGetInputAtIndex(IntPtr sensationCoreInstancePtr, uhsclHandle handle, int idx, out uhsclHandle inputHandle)
        {
            lock (lock_)
            {
                return interop_.uhsclGetInputAtIndex(sensationCoreInstancePtr, handle, idx, out inputHandle);
            }
        }

        public uhsclErrorCode_t uhsclOutputCount(IntPtr sensationCoreInstancePtr, uhsclHandle handle, out ulong count)
        {
            lock (lock_)
            {
                return interop_.uhsclOutputCount(sensationCoreInstancePtr, handle, out count);
            }
        }

        public uhsclErrorCode_t uhsclGetOutputAtIndex(IntPtr sensationCoreInstancePtr, uhsclHandle handle, int idx, out uhsclHandle outputHandle)
        {
            lock (lock_)
            {
                return interop_.uhsclGetOutputAtIndex(sensationCoreInstancePtr, handle, idx, out outputHandle);
            }
        }

        public uhsclErrorCode_t uhsclSetInputToUhsclVector3(IntPtr sensationCoreInstancePtr, uhsclHandle inputSourceHandle, uhsclHandle inputHandle, uhsclVector3_t inputVector)
        {
            lock (lock_)
            {
                return interop_.uhsclSetInputToUhsclVector3(sensationCoreInstancePtr, inputSourceHandle, inputHandle, inputVector);
            }
        }

        public uhsclErrorCode_t uhsclGetInputAsUhsclVector3ByIndex(IntPtr sensationCoreInstancePtr, uhsclHandle handle, int idx, out uhsclVector3_t value)
        {
            lock (lock_)
            {
                return interop_.uhsclGetInputAsUhsclVector3ByIndex(sensationCoreInstancePtr, handle, idx, out value);
            }
        }

        public uhsclErrorCode_t uhsclUpdate(IntPtr sensationCoreInstancePtr)
        {
            lock (lock_)
            {
                return interop_.uhsclUpdate(sensationCoreInstancePtr);
            }
        }

        public uhsclErrorCode_t uhsclSetEvaluationHistorySizeOnCurrentPlaybackDevice(IntPtr sensationCoreInstancePtr, uint size)
        {
            lock (lock_)
            {
                return interop_.uhsclSetEvaluationHistorySizeOnCurrentPlaybackDevice(sensationCoreInstancePtr, size);
            }
        }

        public uhsclErrorCode_t uhsclGetEvaluationHistoryOnCurrentPlaybackDevice(IntPtr sensationCoreInstancePtr, uhsclVector4_t[] evaluationHistory, out uint size)
        {
            lock (lock_)
            {
                return interop_.uhsclGetEvaluationHistoryOnCurrentPlaybackDevice(sensationCoreInstancePtr, evaluationHistory, out size);
            }
        }

        public uhsclErrorCode_t uhsclAddSearchPath(IntPtr sensationCoreInstancePtr, string path)
        {
            lock (lock_)
            {
                return interop_.uhsclAddSearchPath(sensationCoreInstancePtr, path);
            }
        }

        public uhsclErrorCode_t uhsclGetOutputByName(IntPtr sensationCoreInstancePtr, uhsclHandle blockDefinitionHandle, string outputName, out uhsclHandle blockOutputHandle)
        {
            lock (lock_)
            {
                return interop_.uhsclGetOutputByName(sensationCoreInstancePtr, blockDefinitionHandle, outputName, out blockOutputHandle);
            }
        }

        public uhsclErrorCode_t uhsclGetInputByName(IntPtr sensationCoreInstancePtr, uhsclHandle blockDefinitionHandle, string inputName, out uhsclHandle blockInputHandle)
        {
            lock (lock_)
            {
                return interop_.uhsclGetInputByName(sensationCoreInstancePtr, blockDefinitionHandle, inputName, out blockInputHandle);
            }
        }

        public uhsclErrorCode_t uhsclGetMetaDataBool(IntPtr sensationCoreInstancePtr, uhsclHandle handle, string identifier, out bool metadataValue)
        {
            lock (lock_)
            {
                return interop_.uhsclGetMetaDataBool(sensationCoreInstancePtr, handle, identifier, out metadataValue);
            }
        }

        public uhsclErrorCode_t uhsclGetMetaDataString(IntPtr sensationCoreInstancePtr, uhsclHandle handle, string identifier, ulong metadataBufferLength, byte[] metadataValue)
        {
            lock (lock_)
            {
                return interop_.uhsclGetMetaDataString(sensationCoreInstancePtr, handle, identifier, metadataBufferLength, metadataValue);
            }
        }

        public uhsclErrorCode_t uhsclGetMetaDataStringLength(IntPtr sensationCoreInstancePtr, uhsclHandle handle, string identifier, out ulong count)
        {
            lock (lock_)
            {
                return interop_.uhsclGetMetaDataStringLength(sensationCoreInstancePtr, handle, identifier, out count);
            }
        }

        public ulong uhsclGetLogInfoMessageBufferSize(IntPtr sensationCoreInstancePtr)
        {
            lock (lock_)
            {
                return interop_.uhsclGetLogInfoMessageBufferSize(sensationCoreInstancePtr);
            }
        }

        public ulong uhsclGetLogWarningMessageBufferSize(IntPtr sensationCoreInstancePtr)
        {
            lock (lock_)
            {
                return interop_.uhsclGetLogWarningMessageBufferSize(sensationCoreInstancePtr);
            }
        }

        public ulong uhsclGetLogErrorMessageBufferSize(IntPtr sensationCoreInstancePtr)
        {
            lock (lock_)
            {
                return interop_.uhsclGetLogErrorMessageBufferSize(sensationCoreInstancePtr);
            }
        }

        public void uhsclGetLogInfoMessageBufferAndClear(IntPtr sensationCoreInstancePtr, ulong logBufferLength, byte[] logBuffer)
        {
            lock (lock_)
            {
                interop_.uhsclGetLogInfoMessageBufferAndClear(sensationCoreInstancePtr, logBufferLength, logBuffer);
            }
        }

        public void uhsclGetLogWarningMessageBufferAndClear(IntPtr sensationCoreInstancePtr, ulong logBufferLength, byte[] logBuffer)
        {
            lock (lock_)
            {
                interop_.uhsclGetLogWarningMessageBufferAndClear(sensationCoreInstancePtr, logBufferLength, logBuffer);
            }
        }

        public void uhsclGetLogErrorMessageBufferAndClear(IntPtr sensationCoreInstancePtr, ulong logBufferLength, byte[] logBuffer)
        {
            lock (lock_)
            {
                interop_.uhsclGetLogErrorMessageBufferAndClear(sensationCoreInstancePtr, logBufferLength, logBuffer);
            }
        }

        public uhsclErrorCode_t uhsclGetBlockCount(IntPtr sensationCoreInstancePtr, out ulong blockCount)
        {
            lock (lock_)
            {
                return interop_.uhsclGetBlockCount(sensationCoreInstancePtr, out blockCount);
            }
        }

        public uhsclErrorCode_t uhsclGetBlockHandleAtIndex(IntPtr sensationCoreInstancePtr, ulong index, out uhsclHandle handle)
        {
            lock (lock_)
            {
                return interop_.uhsclGetBlockHandleAtIndex(sensationCoreInstancePtr, index, out handle);
            }
        }
    }
}
