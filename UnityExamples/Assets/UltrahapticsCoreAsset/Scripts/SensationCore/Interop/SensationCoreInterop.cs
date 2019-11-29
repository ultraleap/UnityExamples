using System;
using System.Runtime.InteropServices;

namespace UltrahapticsCoreAsset
{
    using NativeUhsclHandle_t = Int32;
    using size_t = System.UInt64; // We should be using UIntPtr here, https://github.com/Moq/moq4/issues/42

    internal class StringMarshaler : ICustomMarshaler
    {
        private static readonly System.Collections.Generic.HashSet<IntPtr> owned_ = new System.Collections.Generic.HashSet<IntPtr>();

        public void CleanUpManagedData(object ManagedObj)
        {
        }

        public void CleanUpNativeData(IntPtr pNativeData)
        {
            if (owned_.Remove(pNativeData)) Marshal.FreeCoTaskMem(pNativeData);
        }

        public int GetNativeDataSize()
        {
            throw new NotImplementedException();
        }

        public IntPtr MarshalManagedToNative(object ManagedObj)
        {
            var ptr = Marshal.StringToCoTaskMemAnsi((string)ManagedObj);
            owned_.Add(ptr);
            return ptr;
        }

        public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            return Marshal.PtrToStringAnsi(pNativeData);
        }

        public static ICustomMarshaler GetInstance(string cookie)
        {
            return new StringMarshaler();
        }
    }

    public class SensationCoreInterop : ISensationCoreInterop
    {
        private class Native
        {
            [DllImport("SensationCore")]
            internal static extern IntPtr uhsclCreate();

            [DllImport("SensationCore")]
            internal static extern void uhsclRelease(IntPtr sensationCoreInstancePtr);

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclImportPythonModule(
                IntPtr sensationCoreInstancePtr,
                [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler))] string modulename
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclGetNameLength(
                IntPtr sensationCoreInstancePtr,
                NativeUhsclHandle_t handle,
                ref size_t count
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclGetName(
                IntPtr sensationCoreInstancePtr,
                NativeUhsclHandle_t handle,
                size_t allocatedLength,
                [In, Out] byte[] nameBuffer
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclCreateBlock(
                IntPtr sensationCoreInstancePtr,
                [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler))] string identifier,
                ref NativeUhsclHandle_t handle
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclCreateInputSource(
                IntPtr sensationCoreInstancePtr,
                NativeUhsclHandle_t blockHandle,
                ref NativeUhsclHandle_t inputSourceHandle
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclStart(
                IntPtr sensationCoreInstancePtr,
                NativeUhsclHandle_t outputHandle,
                NativeUhsclHandle_t inputSourceHandle,
                ref NativeUhsclHandle_t playbackInstanceHandle
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclStop(
                IntPtr sensationCoreInstancePtr,
                NativeUhsclHandle_t playbackInstanceHandle
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclMute(
                IntPtr sensationCoreInstancePtr,
                NativeUhsclHandle_t playbackInstanceHandle
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclUnmute(
                IntPtr sensationCoreInstancePtr,
                NativeUhsclHandle_t playbackInstanceHandle
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclIsCurrentlyPlaying(
                IntPtr sensationCoreInstancePtr,
                ref Int32 isPlaying
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclGetCurrentlyPlayingInstance(
                IntPtr sensationCoreInstancePtr,
                ref NativeUhsclHandle_t currentlyPlayingInstance
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclSetPriority(
                IntPtr sensationCoreInstancePtr,
                uhsclHandle handle,
                UInt32 priority
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclAcquireEmitter(
                IntPtr sensationCoreInstancePtr
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclAcquireMockEmitter(
                IntPtr sensationCoreInstancePtr,
                [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler))] string deviceModel,
                [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler))] string logFilePath
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclAddDevice(
                IntPtr sensationCoreInstancePtr,
                [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler))] string deviceId,
                float[] deviceTransform
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclSetDeviceTransform(
                IntPtr sensationCoreInstancePtr,
                [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler))] string deviceId,
                float[] deviceTransform
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclGetConnectedDevicesStringLength(
                IntPtr sensationCoreInstancePtr,
                ref size_t count
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclGetConnectedDevices(
                IntPtr sensationCoreInstancePtr,
                size_t deviceIdsBufferLength,
                [In, Out] byte[] deviceIdsBuffer
            );

            [DllImport("SensationCore")]
            internal static extern Int32 uhsclIsEmitterConnected(
                IntPtr sensationCoreInstancePtr
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclReleaseEmitter(
                IntPtr sensationCoreInstancePtr
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclEmitterModelDescriptionStringLength(
                IntPtr sensationCoreInstancePtr,
                ref size_t length
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclEmitterSerialNumberStringLength(
                IntPtr sensationCoreInstancePtr,
                ref size_t length
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclEmitterFirmwareVersionStringLength(
                IntPtr sensationCoreInstancePtr,
                ref size_t length
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclEmitterModelDescription(
                IntPtr sensationCoreInstancePtr,
                size_t allocatedLength,
                [In, Out] byte[] buffer
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclEmitterSerialNumber(
                IntPtr sensationCoreInstancePtr,
                size_t allocatedLength,
                [In, Out] byte[] buffer
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclEmitterFirmwareVersion(
                IntPtr sensationCoreInstancePtr,
                size_t allocatedLength,
                [In, Out] byte[] buffer
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclInputCount(
                IntPtr sensationCoreInstancePtr,
                NativeUhsclHandle_t handle,
                ref size_t count
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclGetInputAtIndex(
                IntPtr sensationCoreInstancePtr,
                NativeUhsclHandle_t handle,
                Int32 idx,
                ref NativeUhsclHandle_t inputHandle
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclOutputCount(
                IntPtr sensationCoreInstancePtr,
                NativeUhsclHandle_t handle,
                ref size_t count
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclGetOutputAtIndex(
                IntPtr sensationCoreInstancePtr,
                NativeUhsclHandle_t handle,
                Int32 idx,
                ref NativeUhsclHandle_t outputHandle
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclSetInputToUhsclVector3(
                IntPtr sensationCoreInstancePtr,
                NativeUhsclHandle_t inputSourceHandle,
                NativeUhsclHandle_t inputHandle,
                uhsclVector3_t inputVector
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclGetInputAsUhsclVector3ByIndex(
                IntPtr sensationCoreInstancePtr,
                NativeUhsclHandle_t handle,
                Int32 idx,
                ref uhsclVector3_t value
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclUpdate(
                IntPtr sensationCoreInstancePtr
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclSetEvaluationHistorySizeOnCurrentPlaybackDevice(
                IntPtr sensationCoreInstancePtr,
                UInt32 size
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclGetEvaluationHistoryOnCurrentPlaybackDevice(
                IntPtr sensationCoreInstancePtr,
                [In, Out] uhsclVector4_t[] evaluationHistory,
                ref UInt32 size
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclAddSearchPath(
                IntPtr sensationCoreInstancePtr,
                [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler))] string path
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclGetOutputByName(
                IntPtr sensationCoreInstancePtr,
                NativeUhsclHandle_t blockDefinitionHandle,
                [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler))] string outputName,
                ref NativeUhsclHandle_t blockOutputHandle
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclGetInputByName(
                IntPtr sensationCoreInstancePtr,
                NativeUhsclHandle_t blockDefinitionHandle,
                [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler))] string inputName,
                ref NativeUhsclHandle_t blockInputHandle
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclGetMetaDataBool(
                IntPtr sensationCoreInstancePtr,
                NativeUhsclHandle_t handle,
                [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler))] string identifier,
                ref Int32 metadataValue
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclGetMetaDataString(
                IntPtr sensationCoreInstancePtr,
                NativeUhsclHandle_t handle,
                [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler))] string identifier,
                size_t allocatedLength,
                [In, Out] byte[] metadataValue
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclGetMetaDataStringLength(
                IntPtr sensationCoreInstancePtr,
                NativeUhsclHandle_t handle,
                [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler))] string identifier,
                ref size_t count
            );

            [DllImport("SensationCore")]
            internal static extern size_t uhsclGetLogInfoMessageBufferSize(IntPtr sensationCoreInstancePtr);

            [DllImport("SensationCore")]
            internal static extern size_t uhsclGetLogWarningMessageBufferSize(IntPtr sensationCoreInstancePtr);

            [DllImport("SensationCore")]
            internal static extern size_t uhsclGetLogErrorMessageBufferSize(IntPtr sensationCoreInstancePtr);

            [DllImport("SensationCore")]
            internal static extern void uhsclGetLogInfoMessageBufferAndClear(
                IntPtr sensationCoreInstancePtr,
                size_t logBufferLength,
                [In, Out] byte[] logBuffer
            );

            [DllImport("SensationCore")]
            internal static extern void uhsclGetLogWarningMessageBufferAndClear(
                IntPtr sensationCoreInstancePtr,
                size_t logBufferLength,
                [In, Out] byte[] logBuffer
            );

            [DllImport("SensationCore")]
            internal static extern void uhsclGetLogErrorMessageBufferAndClear(
                IntPtr sensationCoreInstancePtr,
                size_t logBufferLength,
                [In, Out] byte[] logBuffer
            );

            [DllImport("SensationCore")]

            internal static extern uhsclErrorCode_t uhsclGetBlockCount(
                IntPtr sensationCoreInstancePtr,
                ref size_t blockCount
            );

            [DllImport("SensationCore")]
            internal static extern uhsclErrorCode_t uhsclGetBlockHandleAtIndex(
                IntPtr sensationCoreInstancePtr,
                size_t index,
                ref NativeUhsclHandle_t handle
            );
        }

        public IntPtr uhsclCreate()
        {
            return Native.uhsclCreate();
        }

        public void uhsclRelease(IntPtr sensationCoreInstancePtr)
        {
            Native.uhsclRelease(sensationCoreInstancePtr);
        }

        public uhsclErrorCode_t uhsclImportPythonModule(
            IntPtr sensationCoreInstancePtr,
            string modulename
        )
        {
            return Native.uhsclImportPythonModule(sensationCoreInstancePtr, modulename);
        }

        public uhsclErrorCode_t uhsclGetNameLength(
            IntPtr sensationCoreInstancePtr,
            uhsclHandle handle,
            out size_t count
        )
        {
            size_t returnedCount = 0;
            uhsclErrorCode_t error = Native.uhsclGetNameLength(sensationCoreInstancePtr, handle.Value, ref returnedCount);
            count = returnedCount;
            return error;
        }

        public uhsclErrorCode_t uhsclGetName(
            IntPtr sensationCoreInstancePtr,
            uhsclHandle handle,
            size_t nameBufferLength,
            [In, Out] byte[] nameBuffer
        )
        {
            return Native.uhsclGetName(sensationCoreInstancePtr, handle.Value, nameBufferLength, nameBuffer);
        }

        public uhsclErrorCode_t uhsclCreateBlock(
            IntPtr sensationCoreInstancePtr,
            string identifier,
            out uhsclHandle handle
        )
        {
            NativeUhsclHandle_t returnedHandle = 0;
            uhsclErrorCode_t error = Native.uhsclCreateBlock(sensationCoreInstancePtr, identifier, ref returnedHandle);
            handle = new uhsclHandle(returnedHandle);
            return error;
        }

        public uhsclErrorCode_t uhsclCreateInputSource(
            IntPtr sensationCoreInstancePtr,
            uhsclHandle blockHandle,
            out uhsclHandle inputSourceHandle
        )
        {
            NativeUhsclHandle_t returnedInputSourceHandle = 0;
            uhsclErrorCode_t error = Native.uhsclCreateInputSource(sensationCoreInstancePtr, blockHandle.Value, ref returnedInputSourceHandle);
            inputSourceHandle = new uhsclHandle(returnedInputSourceHandle);
            return error;
        }

        public uhsclErrorCode_t uhsclStart(
            IntPtr sensationCoreInstancePtr,
            uhsclHandle outputHandle,
            uhsclHandle inputSourceHandle,
            out uhsclHandle playbackInstanceHandle
        )
        {
            NativeUhsclHandle_t returnedPlaybackInstanceHandle = 0;
            uhsclErrorCode_t error = Native.uhsclStart(sensationCoreInstancePtr, outputHandle.Value, inputSourceHandle.Value, ref returnedPlaybackInstanceHandle);
            playbackInstanceHandle = new uhsclHandle(returnedPlaybackInstanceHandle);
            return error;
        }

        public uhsclErrorCode_t uhsclStop(
            IntPtr sensationCoreInstancePtr,
            uhsclHandle playbackInstanceHandle
        )
        {
            return Native.uhsclStop(sensationCoreInstancePtr, playbackInstanceHandle.Value);
        }

        public uhsclErrorCode_t uhsclMute(
            IntPtr sensationCoreInstancePtr,
            uhsclHandle playbackInstanceHandle
        )
        {
            return Native.uhsclMute(sensationCoreInstancePtr, playbackInstanceHandle.Value);
        }

        public uhsclErrorCode_t uhsclUnmute(
            IntPtr sensationCoreInstancePtr,
            uhsclHandle playbackInstanceHandle
        )
        {
            return Native.uhsclUnmute(sensationCoreInstancePtr, playbackInstanceHandle.Value);
        }

        public uhsclErrorCode_t uhsclIsCurrentlyPlaying(
            IntPtr sensationCoreInstancePtr,
            out bool isPlaying
        )
        {
            Int32 isPlayingReturned = 0;
            uhsclErrorCode_t error = Native.uhsclIsCurrentlyPlaying(sensationCoreInstancePtr, ref isPlayingReturned);
            isPlaying = Convert.ToBoolean(isPlayingReturned);
            return error;
        }

        public uhsclErrorCode_t uhsclGetCurrentlyPlayingInstance(
            IntPtr sensationCoreInstancePtr,
            out uhsclHandle currentlyPlayingInstance
        )
        {
            NativeUhsclHandle_t currentlyPlayingInstanceReturned = 0;
            uhsclErrorCode_t error = Native.uhsclGetCurrentlyPlayingInstance(sensationCoreInstancePtr, ref currentlyPlayingInstanceReturned);
            currentlyPlayingInstance = new uhsclHandle(currentlyPlayingInstanceReturned);
            return error;
        }

        public uhsclErrorCode_t uhsclSetPriority(
            IntPtr sensationCoreInstancePtr,
            uhsclHandle playbackInstanceHandle,
            UInt32 priority
        )
        {
            return Native.uhsclSetPriority(sensationCoreInstancePtr, playbackInstanceHandle, priority);
        }

        public uhsclErrorCode_t uhsclAcquireEmitter(
            IntPtr sensationCoreInstancePtr
        )
        {
            return Native.uhsclAcquireEmitter(sensationCoreInstancePtr);
        }

        public uhsclErrorCode_t uhsclAcquireMockEmitter(
            IntPtr sensationCoreInstancePtr,
            string deviceModel,
            string logFilePath
        )
        {
            return Native.uhsclAcquireMockEmitter(sensationCoreInstancePtr, deviceModel, logFilePath);
        }

        public uhsclErrorCode_t uhsclAddDevice(
                IntPtr sensationCoreInstancePtr,
                string deviceId,
                float[] deviceTransform
        )
        {
            return Native.uhsclAddDevice(sensationCoreInstancePtr, deviceId, deviceTransform);
        }

        public uhsclErrorCode_t uhsclSetDeviceTransform(IntPtr sensationCoreInstancePtr, string deviceId, float[] deviceTransform)
        {
            return Native.uhsclSetDeviceTransform(sensationCoreInstancePtr, deviceId, deviceTransform);
        }

        public uhsclErrorCode_t uhsclGetConnectedDevicesStringLength(
                IntPtr sensationCoreInstancePtr,
                out size_t count
        )
        {
            size_t returnedCount = 0;
            uhsclErrorCode_t error = Native.uhsclGetConnectedDevicesStringLength(sensationCoreInstancePtr, ref returnedCount);
            count = returnedCount;
            return error;
        }
        public uhsclErrorCode_t uhsclGetConnectedDevices(
                IntPtr sensationCoreInstancePtr,
                size_t deviceIdsBufferLength,
                [In, Out] byte[] deviceIdsBuffer
        )
        {
            return Native.uhsclGetConnectedDevices(sensationCoreInstancePtr, deviceIdsBufferLength, deviceIdsBuffer);
        }

        public Int32 uhsclIsEmitterConnected(
            IntPtr sensationCoreInstancePtr
        )
        {
            return Native.uhsclIsEmitterConnected(sensationCoreInstancePtr);
        }

        public uhsclErrorCode_t uhsclReleaseEmitter(
            IntPtr sensationCoreInstancePtr
        )
        {
            return Native.uhsclReleaseEmitter(sensationCoreInstancePtr);
        }

        public uhsclErrorCode_t uhsclEmitterModelDescriptionStringLength(
            IntPtr sensationCoreInstancePtr,
            out size_t length
        )
        {
            size_t len = 0;
            uhsclErrorCode_t error = Native.uhsclEmitterModelDescriptionStringLength(sensationCoreInstancePtr, ref len);
            length = len;
            return error;
        }

        public uhsclErrorCode_t uhsclEmitterSerialNumberStringLength(
            IntPtr sensationCoreInstancePtr,
            out size_t length
        )
        {
            size_t len = 0;
            uhsclErrorCode_t error = Native.uhsclEmitterSerialNumberStringLength(sensationCoreInstancePtr, ref len);
            length = len;
            return error;
        }

        public uhsclErrorCode_t uhsclEmitterFirmwareVersionStringLength(
            IntPtr sensationCoreInstancePtr,
            out size_t length
        )
        {
            size_t len = 0;
            uhsclErrorCode_t error = Native.uhsclEmitterFirmwareVersionStringLength(sensationCoreInstancePtr, ref len);
            length = len;
            return error;
        }

        public uhsclErrorCode_t uhsclEmitterModelDescription(
            IntPtr sensationCoreInstancePtr,
            size_t allocatedLength,
            [In, Out] byte[] buffer
        )
        {
            return Native.uhsclEmitterModelDescription(sensationCoreInstancePtr, allocatedLength, buffer);
        }

        public uhsclErrorCode_t uhsclEmitterSerialNumber(
            IntPtr sensationCoreInstancePtr,
            size_t allocatedLength,
            [In, Out] byte[] buffer
        )
        {
            return Native.uhsclEmitterSerialNumber(sensationCoreInstancePtr, allocatedLength, buffer);
        }

        public uhsclErrorCode_t uhsclEmitterFirmwareVersion(
            IntPtr sensationCoreInstancePtr,
            size_t allocatedLength,
            [In, Out] byte[] buffer
        )
        {
            return Native.uhsclEmitterFirmwareVersion(sensationCoreInstancePtr, allocatedLength, buffer);
        }

        public uhsclErrorCode_t uhsclInputCount(
            IntPtr sensationCoreInstancePtr,
            uhsclHandle handle,
            out size_t count
        )
        {
            size_t returnedCount = 0;
            uhsclErrorCode_t error = Native.uhsclInputCount(sensationCoreInstancePtr, handle.Value, ref returnedCount);
            count = returnedCount;
            return error;
        }

        public uhsclErrorCode_t uhsclGetInputAtIndex(
            IntPtr sensationCoreInstancePtr,
            uhsclHandle handle,
            Int32 idx,
            out uhsclHandle inputHandle
        )
        {
            NativeUhsclHandle_t returnedInputHandle = 0;
            uhsclErrorCode_t error = Native.uhsclGetInputAtIndex(sensationCoreInstancePtr, handle.Value, idx, ref returnedInputHandle);
            inputHandle = new uhsclHandle(returnedInputHandle);
            return error;
        }

        public uhsclErrorCode_t uhsclOutputCount(
            IntPtr sensationCoreInstancePtr,
            uhsclHandle handle,
            out size_t count
        )
        {
            size_t returnedCount = 0;
            uhsclErrorCode_t error = Native.uhsclOutputCount(sensationCoreInstancePtr, handle.Value, ref returnedCount);
            count = returnedCount;
            return error;
        }

        public uhsclErrorCode_t uhsclGetOutputAtIndex(
            IntPtr sensationCoreInstancePtr,
            uhsclHandle handle,
            Int32 idx,
            out uhsclHandle outputHandle
        )
        {
            NativeUhsclHandle_t returnedOutputHandle = 0;
            uhsclErrorCode_t error = Native.uhsclGetOutputAtIndex(sensationCoreInstancePtr, handle.Value, idx, ref returnedOutputHandle);
            outputHandle = new uhsclHandle(returnedOutputHandle);
            return error;
        }

        public uhsclErrorCode_t uhsclSetInputToUhsclVector3(
            IntPtr sensationCoreInstancePtr,
            uhsclHandle inputSourceHandle,
            uhsclHandle inputHandle,
            uhsclVector3_t inputVector
        )
        {
            return Native.uhsclSetInputToUhsclVector3(sensationCoreInstancePtr, inputSourceHandle.Value, inputHandle.Value, inputVector);
        }

        public uhsclErrorCode_t uhsclGetInputAsUhsclVector3ByIndex(
            IntPtr sensationCoreInstancePtr,
            uhsclHandle handle,
            Int32 idx,
            out uhsclVector3_t value
        )
        {
            var returnedValue = new uhsclVector3_t();
            uhsclErrorCode_t error = Native.uhsclGetInputAsUhsclVector3ByIndex(sensationCoreInstancePtr, handle.Value, idx, ref returnedValue);
            value = returnedValue;
            return error;
        }

        public uhsclErrorCode_t uhsclUpdate(
            IntPtr sensationCoreInstancePtr
        )
        {
            return Native.uhsclUpdate(sensationCoreInstancePtr);
        }

        public uhsclErrorCode_t uhsclSetEvaluationHistorySizeOnCurrentPlaybackDevice(
            IntPtr sensationCoreInstancePtr,
            UInt32 size
        )
        {
            uhsclErrorCode_t error = Native.uhsclSetEvaluationHistorySizeOnCurrentPlaybackDevice(sensationCoreInstancePtr, size);
            return error;
        }

        public uhsclErrorCode_t uhsclGetEvaluationHistoryOnCurrentPlaybackDevice(
            IntPtr sensationCoreInstancePtr,
            [In, Out] uhsclVector4_t[] evaluationHistory,
            out UInt32 size
        )
        {
            uint returnedSize = 0;
            uhsclErrorCode_t error = Native.uhsclGetEvaluationHistoryOnCurrentPlaybackDevice(sensationCoreInstancePtr, evaluationHistory, ref returnedSize);
            size = returnedSize;
            return error;
        }

        public uhsclErrorCode_t uhsclAddSearchPath(
            IntPtr sensationCoreInstancePtr,
            string path
        )
        {
            return Native.uhsclAddSearchPath(sensationCoreInstancePtr, path);
        }

        public uhsclErrorCode_t uhsclGetOutputByName(
            IntPtr sensationCoreInstancePtr,
            uhsclHandle blockDefinitionHandle,
            string outputName,
            out uhsclHandle blockOutputHandle
        )
        {
            NativeUhsclHandle_t returnedOutputHandle = 0;
            uhsclErrorCode_t error = Native.uhsclGetOutputByName(sensationCoreInstancePtr, blockDefinitionHandle.Value, outputName, ref returnedOutputHandle);
            blockOutputHandle = new uhsclHandle(returnedOutputHandle);
            return error;
        }

        public uhsclErrorCode_t uhsclGetInputByName(
            IntPtr sensationCoreInstancePtr,
            uhsclHandle blockDefinitionHandle,
            string inputName,
            out uhsclHandle blockInputHandle
        )
        {
            NativeUhsclHandle_t returnedInputHandle = 0;
            uhsclErrorCode_t error = Native.uhsclGetInputByName(sensationCoreInstancePtr, blockDefinitionHandle.Value, inputName, ref returnedInputHandle);
            blockInputHandle = new uhsclHandle(returnedInputHandle);
            return error;
        }

        public uhsclErrorCode_t uhsclGetMetaDataBool(
            IntPtr sensationCoreInstancePtr,
            uhsclHandle handle,
            string identifier,
            out bool metadataValue
        )
        {
            Int32 returnedMetadataValue = 0;
            uhsclErrorCode_t error = Native.uhsclGetMetaDataBool(sensationCoreInstancePtr, handle.Value, identifier, ref returnedMetadataValue);
            metadataValue = Convert.ToBoolean(returnedMetadataValue);
            return error;
        }

        public uhsclErrorCode_t uhsclGetMetaDataString(
            IntPtr sensationCoreInstancePtr,
            uhsclHandle handle,
            string identifier,
            size_t metadataBufferLength,
            [In, Out] byte[] metadataValue
        )
        {
            return Native.uhsclGetMetaDataString(sensationCoreInstancePtr, handle.Value, identifier, metadataBufferLength, metadataValue);
        }

        public uhsclErrorCode_t uhsclGetMetaDataStringLength(
            IntPtr sensationCoreInstancePtr,
            uhsclHandle handle,
            string identifier,
            out size_t count
        )
        {
            size_t returnedCount = 0;
            uhsclErrorCode_t error = Native.uhsclGetMetaDataStringLength(sensationCoreInstancePtr, handle.Value, identifier, ref returnedCount);
            count = returnedCount;
            return error;
        }

        public size_t uhsclGetLogInfoMessageBufferSize(IntPtr sensationCoreInstancePtr)
        {
            return Native.uhsclGetLogInfoMessageBufferSize(sensationCoreInstancePtr);
        }

        public size_t uhsclGetLogWarningMessageBufferSize(IntPtr sensationCoreInstancePtr)
        {
            return Native.uhsclGetLogWarningMessageBufferSize(sensationCoreInstancePtr);
        }

        public size_t uhsclGetLogErrorMessageBufferSize(IntPtr sensationCoreInstancePtr)
        {
            return Native.uhsclGetLogErrorMessageBufferSize(sensationCoreInstancePtr);
        }

        public void uhsclGetLogInfoMessageBufferAndClear(
            IntPtr sensationCoreInstancePtr,
            size_t logBufferLength,
            [In, Out] byte[] logBuffer
        )
        {
            Native.uhsclGetLogInfoMessageBufferAndClear(sensationCoreInstancePtr, logBufferLength, logBuffer);
        }

        public void uhsclGetLogWarningMessageBufferAndClear(
            IntPtr sensationCoreInstancePtr,
            size_t logBufferLength,
            [In, Out] byte[] logBuffer
        )
        {
            Native.uhsclGetLogWarningMessageBufferAndClear(sensationCoreInstancePtr, logBufferLength, logBuffer);
        }

        public void uhsclGetLogErrorMessageBufferAndClear(
            IntPtr sensationCoreInstancePtr,
            size_t logBufferLength,
            [In, Out] byte[] logBuffer
        )
        {
            Native.uhsclGetLogErrorMessageBufferAndClear(sensationCoreInstancePtr, logBufferLength, logBuffer);
        }

        public uhsclErrorCode_t uhsclGetBlockCount(
            IntPtr sensationCoreInstancePtr,
            out size_t blockCount
        )
        {
            size_t count = 0;
            uhsclErrorCode_t error = Native.uhsclGetBlockCount(sensationCoreInstancePtr, ref count);
            blockCount = count;
            return error;
        }

        public uhsclErrorCode_t uhsclGetBlockHandleAtIndex(
            IntPtr sensationCoreInstancePtr,
            size_t index,
            out uhsclHandle handle
        )
        {
            NativeUhsclHandle_t returnedHandle = 0;
            uhsclErrorCode_t error = Native.uhsclGetBlockHandleAtIndex(sensationCoreInstancePtr, index, ref returnedHandle);
            handle = new uhsclHandle(returnedHandle);
            return error;
        }

    }
}
