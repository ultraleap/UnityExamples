using System;

namespace UltrahapticsCoreAsset
{
    class ForwardLogMessagesDecorator : ISensationCoreInterop
    {
        private ISensationCoreInterop interop_;
        private ILogStreamReader logStreamReader_;

        public void ForwardLogMessages(IntPtr sensationCoreInstancePtr)
        {
            logStreamReader_.GetInfoMessages(sensationCoreInstancePtr).ForEach(x => UCA.Logger.LogInfo(x));
            logStreamReader_.GetWarningMessages(sensationCoreInstancePtr).ForEach(x => UCA.Logger.LogWarning(x));
            logStreamReader_.GetErrorMessages(sensationCoreInstancePtr).ForEach(x => UCA.Logger.LogError(x));
        }

        public ForwardLogMessagesDecorator(ISensationCoreInterop interop, ILogStreamReader logStreamReader)
        {
            interop_ = interop;
            logStreamReader_ = logStreamReader;
        }

        public IntPtr uhsclCreate()
        {
            return interop_.uhsclCreate();
        }

        public void uhsclRelease(IntPtr sensationCoreInstancePtr)
        {
            interop_.uhsclRelease(sensationCoreInstancePtr);
            // Deliberately ignoring logging
        }

        public uhsclErrorCode_t uhsclImportPythonModule(IntPtr sensationCoreInstancePtr, string modulename)
        {
            var error = interop_.uhsclImportPythonModule(sensationCoreInstancePtr, modulename);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclGetNameLength(IntPtr sensationCoreInstancePtr, uhsclHandle handle, out ulong count)
        {
            var error = interop_.uhsclGetNameLength(sensationCoreInstancePtr, handle, out count);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclGetName(IntPtr sensationCoreInstancePtr, uhsclHandle handle, ulong nameBufferLength, byte[] nameBuffer)
        {
            var error = interop_.uhsclGetName(sensationCoreInstancePtr, handle, nameBufferLength, nameBuffer);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclCreateBlock(IntPtr sensationCoreInstancePtr, string identifier, out uhsclHandle handle)
        {
            var error = interop_.uhsclCreateBlock(sensationCoreInstancePtr, identifier, out handle);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclCreateInputSource(IntPtr sensationCoreInstancePtr, uhsclHandle blockHandle,
            out uhsclHandle inputSourceHandle)
        {
            var error = interop_.uhsclCreateInputSource(sensationCoreInstancePtr, blockHandle, out inputSourceHandle);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclStart(IntPtr sensationCoreInstancePtr, uhsclHandle outputHandle, uhsclHandle inputSourceHandle, out uhsclHandle playbackInstanceHandle)
        {
            var error = interop_.uhsclStart(sensationCoreInstancePtr, outputHandle, inputSourceHandle, out playbackInstanceHandle);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclStop(IntPtr sensationCoreInstancePtr, uhsclHandle handle)
        {
            var error = interop_.uhsclStop(sensationCoreInstancePtr, handle);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclMute(IntPtr sensationCoreInstancePtr, uhsclHandle handle)
        {
            var error = interop_.uhsclMute(sensationCoreInstancePtr, handle);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclUnmute(IntPtr sensationCoreInstancePtr, uhsclHandle handle)
        {
            var error = interop_.uhsclUnmute(sensationCoreInstancePtr, handle);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclIsCurrentlyPlaying(IntPtr sensationCoreInstancePtr, out bool isPlaying)
        {
            var error = interop_.uhsclIsCurrentlyPlaying(sensationCoreInstancePtr, out isPlaying);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclGetCurrentlyPlayingInstance(IntPtr sensationCoreInstancePtr,out uhsclHandle currentlyPlayingInstance)
        {
            var error = interop_.uhsclGetCurrentlyPlayingInstance(sensationCoreInstancePtr,out currentlyPlayingInstance);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclSetPriority(IntPtr sensationCoreInstancePtr, uhsclHandle handle, uint priority)
        {
            var error = interop_.uhsclSetPriority(sensationCoreInstancePtr, handle, priority);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclAcquireEmitter(IntPtr sensationCoreInstancePtr)
        {
            var error = interop_.uhsclAcquireEmitter(sensationCoreInstancePtr);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclAcquireMockEmitter(IntPtr sensationCoreInstancePtr, string deviceModel, string logFilePath)
        {
            var error = interop_.uhsclAcquireMockEmitter(sensationCoreInstancePtr, deviceModel, logFilePath);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclAddDevice(IntPtr sensationCoreInstancePtr, string deviceId, float[] deviceTransform)
        {
            var error = interop_.uhsclAddDevice(sensationCoreInstancePtr, deviceId, deviceTransform);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclSetDeviceTransform(IntPtr sensationCoreInstancePtr, string deviceId, float[] deviceTransform)
        {
            var error = interop_.uhsclSetDeviceTransform(sensationCoreInstancePtr, deviceId, deviceTransform);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclGetConnectedDevicesStringLength(IntPtr sensationCoreInstancePtr, out ulong count)
        {
            var error = interop_.uhsclGetConnectedDevicesStringLength(sensationCoreInstancePtr, out count);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclGetConnectedDevices(IntPtr sensationCoreInstancePtr, ulong deviceIdsBufferLength, byte[] deviceIdsBuffer)
        {
            var error = interop_.uhsclGetConnectedDevices(sensationCoreInstancePtr, deviceIdsBufferLength, deviceIdsBuffer);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public int uhsclIsEmitterConnected(IntPtr sensationCoreInstancePtr)
        {
            var error = interop_.uhsclIsEmitterConnected(sensationCoreInstancePtr);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclReleaseEmitter(IntPtr sensationCoreInstancePtr)
        {
            var error = interop_.uhsclReleaseEmitter(sensationCoreInstancePtr);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclEmitterModelDescriptionStringLength(IntPtr sensationCoreInstancePtr, out ulong length)
        {
            var error = interop_.uhsclEmitterModelDescriptionStringLength(sensationCoreInstancePtr, out length);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclEmitterSerialNumberStringLength(IntPtr sensationCoreInstancePtr, out ulong length)
        {
            var error = interop_.uhsclEmitterSerialNumberStringLength(sensationCoreInstancePtr, out length);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclEmitterFirmwareVersionStringLength(IntPtr sensationCoreInstancePtr, out ulong length)
        {
            var error = interop_.uhsclEmitterFirmwareVersionStringLength(sensationCoreInstancePtr, out length);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclEmitterModelDescription(IntPtr sensationCoreInstancePtr, ulong allocatedLength, byte[] buffer)
        {
            var error = interop_.uhsclEmitterModelDescription(sensationCoreInstancePtr, allocatedLength, buffer);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclEmitterSerialNumber(IntPtr sensationCoreInstancePtr, ulong allocatedLength, byte[] buffer)
        {
            var error = interop_.uhsclEmitterSerialNumber(sensationCoreInstancePtr, allocatedLength, buffer);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclEmitterFirmwareVersion(IntPtr sensationCoreInstancePtr, ulong allocatedLength, byte[] buffer)
        {
            var error = interop_.uhsclEmitterFirmwareVersion(sensationCoreInstancePtr, allocatedLength, buffer);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclInputCount(IntPtr sensationCoreInstancePtr, uhsclHandle handle, out ulong count)
        {
            var error = interop_.uhsclInputCount(sensationCoreInstancePtr, handle, out count);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclGetInputAtIndex(IntPtr sensationCoreInstancePtr, uhsclHandle handle, int idx, out uhsclHandle inputHandle)
        {
            var error = interop_.uhsclGetInputAtIndex(sensationCoreInstancePtr, handle, idx, out inputHandle);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclOutputCount(IntPtr sensationCoreInstancePtr, uhsclHandle handle, out ulong count)
        {
            var error = interop_.uhsclOutputCount(sensationCoreInstancePtr, handle, out count);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclGetOutputAtIndex(IntPtr sensationCoreInstancePtr, uhsclHandle handle, int idx, out uhsclHandle outputHandle)
        {
            var error = interop_.uhsclGetOutputAtIndex(sensationCoreInstancePtr, handle, idx, out outputHandle);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclSetInputToUhsclVector3(IntPtr sensationCoreInstancePtr, uhsclHandle inputSourceHandle, uhsclHandle inputHandle, uhsclVector3_t inputVector)
        {
            return interop_.uhsclSetInputToUhsclVector3(sensationCoreInstancePtr, inputSourceHandle, inputHandle, inputVector);
            // TODO : Deliberately ignoring logging
        }

        public uhsclErrorCode_t uhsclGetInputAsUhsclVector3ByIndex(IntPtr sensationCoreInstancePtr, uhsclHandle handle, int idx, out uhsclVector3_t value)
        {
            var error = interop_.uhsclGetInputAsUhsclVector3ByIndex(sensationCoreInstancePtr, handle, idx, out value);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclUpdate(IntPtr sensationCoreInstancePtr)
        {
            var error = interop_.uhsclUpdate(sensationCoreInstancePtr);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclSetEvaluationHistorySizeOnCurrentPlaybackDevice(IntPtr sensationCoreInstancePtr, uint size)
        {
            var error = interop_.uhsclSetEvaluationHistorySizeOnCurrentPlaybackDevice(sensationCoreInstancePtr, size);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclGetEvaluationHistoryOnCurrentPlaybackDevice(IntPtr sensationCoreInstancePtr, uhsclVector4_t[] evaluationHistory, out uint size)
        {
            var error = interop_.uhsclGetEvaluationHistoryOnCurrentPlaybackDevice(sensationCoreInstancePtr, evaluationHistory, out size);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclAddSearchPath(IntPtr sensationCoreInstancePtr, string path)
        {
            var error = interop_.uhsclAddSearchPath(sensationCoreInstancePtr, path);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclGetOutputByName(IntPtr sensationCoreInstancePtr, uhsclHandle blockDefinitionHandle, string outputName, out uhsclHandle blockOutputHandle)
        {
            var error = interop_.uhsclGetOutputByName(sensationCoreInstancePtr, blockDefinitionHandle, outputName, out blockOutputHandle);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclGetInputByName(IntPtr sensationCoreInstancePtr, uhsclHandle blockDefinitionHandle, string inputName, out uhsclHandle blockInputHandle)
        {
            var error = interop_.uhsclGetInputByName(sensationCoreInstancePtr, blockDefinitionHandle, inputName, out blockInputHandle);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclGetMetaDataBool(IntPtr sensationCoreInstancePtr, uhsclHandle handle, string identifier, out bool metadataValue)
        {
            var error = interop_.uhsclGetMetaDataBool(sensationCoreInstancePtr, handle, identifier, out metadataValue);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclGetMetaDataString(IntPtr sensationCoreInstancePtr, uhsclHandle handle, string identifier, ulong metadataBufferLength, byte[] metadataValue)
        {
            var error = interop_.uhsclGetMetaDataString(sensationCoreInstancePtr, handle, identifier, metadataBufferLength, metadataValue);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclGetMetaDataStringLength(IntPtr sensationCoreInstancePtr, uhsclHandle handle, string identifier, out ulong count)
        {
            var error = interop_.uhsclGetMetaDataStringLength(sensationCoreInstancePtr, handle, identifier, out count);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
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
            var error = interop_.uhsclGetBlockCount(sensationCoreInstancePtr, out blockCount);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }

        public uhsclErrorCode_t uhsclGetBlockHandleAtIndex(IntPtr sensationCoreInstancePtr, ulong index, out uhsclHandle handle)
        {
            var error = interop_.uhsclGetBlockHandleAtIndex(sensationCoreInstancePtr, index, out handle);
            ForwardLogMessages(sensationCoreInstancePtr);
            return error;
        }
    }
}
