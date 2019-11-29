using System;
using System.Runtime.InteropServices;

namespace UltrahapticsCoreAsset
{

    using size_t = System.UInt64; // We should be using UIntPtr here, https://github.com/Moq/moq4/issues/42

    public interface ISensationCoreInterop
    {
        IntPtr uhsclCreate();

        void uhsclRelease(IntPtr sensationCoreInstancePtr);

        uhsclErrorCode_t uhsclImportPythonModule(
            IntPtr instance,
            string modulename
        );

        uhsclErrorCode_t uhsclGetNameLength(
            IntPtr sensationCoreInstancePtr,
            uhsclHandle handle,
            out size_t count
        );

        uhsclErrorCode_t uhsclGetName(
            IntPtr sensationCoreInstancePtr,
            uhsclHandle handle,
            size_t nameBufferLength,
            [In, Out] byte[] nameBuffer
        );

        uhsclErrorCode_t uhsclCreateBlock(
            IntPtr sensationCoreInstancePtr,
            string identifier,
            out uhsclHandle handle
        );

        uhsclErrorCode_t uhsclCreateInputSource(
            IntPtr sensationCoreInstancePtr,
            uhsclHandle blockHandle,
            out uhsclHandle inputSourceHandle
        );

        uhsclErrorCode_t uhsclStart(
            IntPtr sensationCoreInstancePtr,
            uhsclHandle outputHandle,
            uhsclHandle inputSourceHandle,
            out uhsclHandle playbackInstanceHandle
        );

        uhsclErrorCode_t uhsclStop(
            IntPtr sensationCoreInstancePtr,
            uhsclHandle handle
        );

        uhsclErrorCode_t uhsclMute(
            IntPtr sensationCoreInstancePtr,
            uhsclHandle handle
        );

        uhsclErrorCode_t uhsclUnmute(
            IntPtr sensationCoreInstancePtr,
            uhsclHandle handle
        );

        uhsclErrorCode_t uhsclIsCurrentlyPlaying(
            IntPtr sensationCoreInstancePtr,
            out bool isPlaying
        );

        uhsclErrorCode_t uhsclGetCurrentlyPlayingInstance(
            IntPtr sensationCoreInstancePtr,
            out uhsclHandle currentlyPlayingInstance
        );

        uhsclErrorCode_t uhsclSetPriority(
            IntPtr sensationCoreInstancePtr,
            uhsclHandle handle,
            UInt32 priority
        );

        uhsclErrorCode_t uhsclAcquireEmitter(
            IntPtr sensationCoreInstancePtr
        );

        uhsclErrorCode_t uhsclAcquireMockEmitter(
            IntPtr sensationCoreInstancePtr,
            string deviceModel,
            string logFilePath = null
        );

        uhsclErrorCode_t uhsclAddDevice(
            IntPtr sensationCoreInstancePtr,
            string deviceId,
            float[] deviceTransform
        );

        uhsclErrorCode_t uhsclSetDeviceTransform(
            IntPtr sensationCoreInstancePtr,
            string deviceId,
            float[] deviceTransform
        );

        uhsclErrorCode_t uhsclGetConnectedDevicesStringLength(
            IntPtr sensationCoreInstancePtr,
            out size_t count
        );

        uhsclErrorCode_t uhsclGetConnectedDevices(
            IntPtr sensationCoreInstancePtr,
            size_t deviceIdsBufferLength,
            [In, Out] byte[] deviceIdsBuffer
        );

        Int32 uhsclIsEmitterConnected(
            IntPtr sensationCoreInstancePtr
        );

        uhsclErrorCode_t uhsclReleaseEmitter(
            IntPtr sensationCoreInstancePtr
        );

        uhsclErrorCode_t uhsclEmitterModelDescriptionStringLength(
            IntPtr sensationCoreInstancePtr,
            out size_t length
        );
        uhsclErrorCode_t uhsclEmitterSerialNumberStringLength(
            IntPtr sensationCoreInstancePtr,
            out size_t length
        );
        uhsclErrorCode_t uhsclEmitterFirmwareVersionStringLength(
            IntPtr sensationCoreInstancePtr,
            out size_t length
        );

        uhsclErrorCode_t uhsclEmitterModelDescription(
            IntPtr sensationCoreInstancePtr,
            size_t allocatedLength,
            [In, Out] byte[] buffer
        );
        uhsclErrorCode_t uhsclEmitterSerialNumber(
            IntPtr sensationCoreInstancePtr,
            size_t allocatedLength,
            [In, Out] byte[] buffer
        );
        uhsclErrorCode_t uhsclEmitterFirmwareVersion(
            IntPtr sensationCoreInstancePtr,
            size_t allocatedLength,
            [In, Out] byte[] buffer
        );

        uhsclErrorCode_t uhsclInputCount(
            IntPtr sensationCoreInstancePtr,
            uhsclHandle handle,
            out size_t count
        );

        uhsclErrorCode_t uhsclGetInputAtIndex(
            IntPtr sensationCoreInstancePtr,
            uhsclHandle handle,
            Int32 idx,
            out uhsclHandle inputHandle
        );

        uhsclErrorCode_t uhsclOutputCount(
            IntPtr sensationCoreInstancePtr,
            uhsclHandle handle,
            out size_t count
        );

        uhsclErrorCode_t uhsclGetOutputAtIndex(
            IntPtr sensationCoreInstancePtr,
            uhsclHandle handle,
            Int32 idx,
            out uhsclHandle outputHandle
        );

        uhsclErrorCode_t uhsclSetInputToUhsclVector3(
            IntPtr sensationCoreInstancePtr,
            uhsclHandle inputSourceHandle,
            uhsclHandle inputHandle,
            uhsclVector3_t inputVector
        );

        uhsclErrorCode_t uhsclGetInputAsUhsclVector3ByIndex(
            IntPtr sensationCoreInstancePtr,
            uhsclHandle handle,
            Int32 idx,
            out uhsclVector3_t value
        );

        uhsclErrorCode_t uhsclUpdate(
            IntPtr sensationCoreInstancePtr
        );

        uhsclErrorCode_t uhsclSetEvaluationHistorySizeOnCurrentPlaybackDevice(
            IntPtr sensationCoreInstancePtr,
            UInt32 size
        );

        uhsclErrorCode_t uhsclGetEvaluationHistoryOnCurrentPlaybackDevice(
            IntPtr sensationCoreInstancePtr,
            [In, Out] uhsclVector4_t[] evaluationHistory,
            out UInt32 size
        );

        uhsclErrorCode_t uhsclAddSearchPath(
            IntPtr sensationCoreInstancePtr,
            string path
        );

        uhsclErrorCode_t uhsclGetOutputByName(
            IntPtr sensationCoreInstancePtr,
            uhsclHandle blockDefinitionHandle,
            string outputName,
            out uhsclHandle blockOutputHandle
        );

        uhsclErrorCode_t uhsclGetInputByName(
            IntPtr sensationCoreInstancePtr,
            uhsclHandle blockDefinitionHandle,
            string inputName,
            out uhsclHandle blockInputHandle
        );

        uhsclErrorCode_t uhsclGetMetaDataBool(
            IntPtr sensationCoreInstancePtr,
            uhsclHandle handle,
            string identifier,
            out bool metadataValue
        );

        uhsclErrorCode_t uhsclGetMetaDataString(
            IntPtr sensationCoreInstancePtr,
            uhsclHandle handle,
            string identifier,
            size_t metadataBufferLength,
            [In, Out] byte[] metadataValue
        );

        uhsclErrorCode_t uhsclGetMetaDataStringLength(
            IntPtr sensationCoreInstancePtr,
            uhsclHandle handle,
            string identifier,
            out size_t count
        );

        size_t uhsclGetLogInfoMessageBufferSize(IntPtr sensationCoreInstancePtr);
        size_t uhsclGetLogWarningMessageBufferSize(IntPtr sensationCoreInstancePtr);
        size_t uhsclGetLogErrorMessageBufferSize(IntPtr sensationCoreInstancePtr);

        void uhsclGetLogInfoMessageBufferAndClear(
            IntPtr sensationCoreInstancePtr,
            size_t logBufferLength,
            [In, Out] byte[] logBuffer
        );

        void uhsclGetLogWarningMessageBufferAndClear(
            IntPtr sensationCoreInstancePtr,
            size_t logBufferLength,
            [In, Out] byte[] logBuffer
        );

        void uhsclGetLogErrorMessageBufferAndClear(
            IntPtr sensationCoreInstancePtr,
            size_t logBufferLength,
            [In, Out] byte[] logBuffer
        );

        uhsclErrorCode_t uhsclGetBlockCount(
            IntPtr sensationCoreInstancePtr,
            out size_t blockCount
        );

        uhsclErrorCode_t uhsclGetBlockHandleAtIndex(
            IntPtr sensationCoreInstancePtr,
            size_t index,
            out uhsclHandle handle
        );

    }
}
