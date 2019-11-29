using System;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor.Callbacks;
using UnityEditor;
#endif

namespace UltrahapticsCoreAsset
{

    using size_t = System.UInt64; // We should be using UIntPtr here, https://github.com/Moq/moq4/issues/42

    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    public class SensationCore : MonoBehaviour, ISensationCore
    {
        private static CustomPythonSearchPaths pythonSearchPaths_ = new CustomPythonSearchPaths();

        private ISensationCoreInterop sensationCoreInterop_;
        private IntPtr sensationCoreInstancePtr_;
        private uint evaluationHistorySize_ = 0;

        public static ISensationCore Instance = null;

        private static IEnumerable<MethodInfo> GetAllMethodsWithAttribute<T>()
            where T : Attribute
        {
            // Performance consideration : This LINQ query takes ~2 seconds
            return ReflectionUtilities.GetExportedTypesFromAssemblies()
                .SelectMany(x => x.GetMethods())
                .Where(x => Attribute.IsDefined(x, typeof(T)))
                .ToArray();
        }

        private static IEnumerable<MethodInfo> onSensationCoreInitializeUserMethods_;
        static SensationCore()
        {
            var allOnSensationCoreInitializeMethods = GetAllMethodsWithAttribute<OnSensationCoreInitializeAttribute>();
            var methodsGroupedByValidity = allOnSensationCoreInitializeMethods.ToLookup(x => x.IsPublic && x.IsStatic && x.ReturnType == typeof(void) && !x.GetParameters().Any());
            onSensationCoreInitializeUserMethods_ = methodsGroupedByValidity[true].ToArray();
            var invalidMethods = methodsGroupedByValidity[false];
            foreach (var invalidMethod in invalidMethods)
            {
                UCA.Logger.LogWarning(LogMessages.SensationCore.InvalidMethodOnSensationCoreInitialize(invalidMethod.Name));
            }
        }
        private SensationCore(){}

        public void Start()
        {
            CreateSensationCore();
#if UNITY_EDITOR
            AssemblyReloadEvents.beforeAssemblyReload += OnDestroy;
#endif
        }

#if UNITY_EDITOR
        [DidReloadScripts(1)]
        public static void ScriptReload()
        {
            if (!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }
            if (Instance == null)
            {
                SensationCore core = GameObject.FindObjectOfType<SensationCore>();
                if (core != null)
                {
                    core.Start();
                }
            }
        }
#endif

        public static void AddSearchPath(string path)
        {
            if (SensationCore.Instance != null)
            {
                UCA.Logger.LogError(LogMessages.SensationCore.InvalidAddSearchPathsUsage);
            }
            else
            {
                pythonSearchPaths_.AddSearchPath(path);
            }
        }

        public void CreateSensationCore(ISensationCoreInterop SensationCoreInterop = null)
        {
            try
            {
                if (SensationCore.Instance == null)
                {
                    if (SensationCoreInterop == null)
                    {
                        sensationCoreInterop_ = new SensationCoreInterop();
                        sensationCoreInterop_ = new LockDecorator(sensationCoreInterop_);
                        sensationCoreInterop_ = new ThrowOnErrorDecorator(sensationCoreInterop_);
                        sensationCoreInterop_ = new ForwardLogMessagesDecorator(sensationCoreInterop_, new LogStreamReader(sensationCoreInterop_));
                    }
                    else
                    {
                        sensationCoreInterop_ = SensationCoreInterop;
                    }
                    sensationCoreInstancePtr_ = sensationCoreInterop_.uhsclCreate();
                    if (sensationCoreInstancePtr_ == default(IntPtr))
                    {
                        throw new Exception("Failed to initialise SensationCore library");
                    }

                    foreach (var method in onSensationCoreInitializeUserMethods_)
                    {
                        try
                        {
                            method.Invoke(null, null);
                        }
                        catch (Exception ex)
                        {
                            UCA.Logger.LogWarning("Failed executing OnSensationCoreInitialize Method : " + method.Name + "\n" + ex.InnerException ?? ex.Message);
                        }
                    }

                    pythonSearchPaths_.ResetApplied();
                    pythonSearchPaths_.Apply(sensationCoreInterop_, sensationCoreInstancePtr_);
                    pythonSearchPaths_.DisplayWarningIfNoSearchPathsAreValid();

                    Instance = this;
                    if (Application.isPlaying)
                        UCA.Logger.LogInfo("SensationCore Successfully Created");
                }
            }
            catch (Exception e)
            {
                UCA.Logger.LogInfo("Exception creating SensationCore: " + e);
            }
        }

#if UNITY_EDITOR
        public void OnDisable()
        {
            if (!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
            {
                OnDestroy();
            }
        }
#endif

        public void OnDestroy()
        {
            try
            {
                try
                {
                    if (sensationCoreInterop_ != null)
                    {
                        sensationCoreInterop_.uhsclRelease(sensationCoreInstancePtr_);
                        sensationCoreInterop_ = null;
                        SensationCore.Instance = null;
                        sensationCoreInstancePtr_ = default(IntPtr);
                    }
                }
                catch
                {
                    ReleaseEmitter();
                    sensationCoreInterop_.uhsclRelease(sensationCoreInstancePtr_);
                    sensationCoreInterop_ = null;
                    SensationCore.Instance = null;
                    sensationCoreInstancePtr_ = default(IntPtr);
                }
            }
            catch (Exception e)
            {
                UCA.Logger.LogInfo("Exception when destroying SensationCore: " + e);
            }
        }

        public uhsclHandle CreateBlock(string name)
        {
            uhsclHandle handle = uhsclHandle.INVALID_HANDLE;
            if (sensationCoreInterop_ != null)
            {
                sensationCoreInterop_.uhsclCreateBlock(sensationCoreInstancePtr_, name, out handle);
                return handle;
            }
            return uhsclHandle.INVALID_HANDLE;
        }

        public uhsclHandle CallStart(uhsclHandle blockHandle, uhsclHandle inputSourceHandle)
        {
            uhsclHandle playbackInstanceHandle = uhsclHandle.INVALID_HANDLE;
            if (sensationCoreInterop_ != null)
            {
                // TODO: Instead of taking the first output for a block, we should customize the output!
                uhsclHandle outputHandle;
                sensationCoreInterop_.uhsclGetOutputAtIndex(sensationCoreInstancePtr_, blockHandle, 0, out outputHandle);

                sensationCoreInterop_.uhsclStart(sensationCoreInstancePtr_, outputHandle, inputSourceHandle, out playbackInstanceHandle);
            }
            return playbackInstanceHandle;
        }

        public void Stop(uhsclHandle playbackInstanceHandle)
        {
            if (sensationCoreInterop_ != null)
            {
                sensationCoreInterop_.uhsclStop(sensationCoreInstancePtr_, playbackInstanceHandle);
            }
        }

        public uhsclHandle CreateInputSource(uhsclHandle blockHandle)
        {
            uhsclHandle inputSourceHandle = uhsclHandle.INVALID_HANDLE;
            if (sensationCoreInterop_ != null)
            {
                sensationCoreInterop_.uhsclCreateInputSource(sensationCoreInstancePtr_, blockHandle, out inputSourceHandle);
            }
            return inputSourceHandle;
        }

        public void AcquireEmitter()
        {
            if (sensationCoreInterop_ != null)
            {
                sensationCoreInterop_.uhsclAcquireEmitter(sensationCoreInstancePtr_);
            }
        }

        public void AcquireMockEmitter()
        {
            if (UCA.MockEmitterLoggingEnabled)
            {
                sensationCoreInterop_.uhsclAcquireMockEmitter(sensationCoreInstancePtr_, UCA.MockEmitterModel, UCA.MockEmitterLogFile);
            }
            else
            {
                sensationCoreInterop_.uhsclAcquireMockEmitter(sensationCoreInstancePtr_, UCA.MockEmitterModel);
            }
        }

        float[] ToEmitterSpaceTransformArray(UnityEngine.Transform initialTransform)
        {
            // Convert from Unity Emitter Space to SDK space by
            // swapping Y and Z, before converting to a float array
            var tmpTransform = new GameObject().transform;
            UnityEngine.Vector3 rotationVector = new UnityEngine.Vector3(
                initialTransform.eulerAngles.x,
                initialTransform.eulerAngles.z,
                initialTransform.eulerAngles.y);
            UnityEngine.Vector3 positionVector = new UnityEngine.Vector3(
                initialTransform.localPosition.x,
                initialTransform.localPosition.z,
                initialTransform.localPosition.y);

            Quaternion rotation = Quaternion.Euler(rotationVector);
            tmpTransform.SetPositionAndRotation(positionVector, rotation);

            float[] result = new float[16]{tmpTransform.right.x, tmpTransform.up.x, tmpTransform.forward.x, tmpTransform.position.x,
                                           tmpTransform.right.y, tmpTransform.up.y, tmpTransform.forward.y, tmpTransform.position.y,
                                           tmpTransform.right.z, tmpTransform.up.z, tmpTransform.forward.z, tmpTransform.position.z,
                                                              0,                 0,                      0,                       1};

            DestroyImmediate(tmpTransform.gameObject);
            return result;
        }

        public void AddDevice(string deviceId, UnityEngine.Transform transform)
        {
            // Transform in emitter space
            var transf = ToEmitterSpaceTransformArray(transform);
            sensationCoreInterop_.uhsclAddDevice(sensationCoreInstancePtr_, deviceId, transf);
        }

        public void SetDeviceTransform(string deviceId, UnityEngine.Transform transform)
        {
            // Transform in emitter space
            var transf = ToEmitterSpaceTransformArray(transform);
            sensationCoreInterop_.uhsclSetDeviceTransform(sensationCoreInstancePtr_, deviceId, transf);
        }

        public List<string> GetConnectedDevices()
        {
            if (!Application.isPlaying)
            {
                try
                {
                    SensationCore.Instance.AcquireEmitter();
                }
                catch(Exception)
                {
                    return new List<string>();
                }
            }

            size_t length = 0; //In C# 7 we will be able to use inline out parameters, so we could remove this
            sensationCoreInterop_.uhsclGetConnectedDevicesStringLength(sensationCoreInstancePtr_, out length);
            if (length == 0)
            {
                return new List<string>();
            }

            byte[] buffer = new byte[(uint)length + 1];
            sensationCoreInterop_.uhsclGetConnectedDevices(sensationCoreInstancePtr_, Convert.ToUInt32(buffer.Length), buffer);
            var list = CreateListOfIdsFromBuffer(buffer);

            if (!Application.isPlaying)
            {
                SensationCore.Instance.ReleaseEmitter();
            }

            return list;
        }

        private static List<string> CreateListOfIdsFromBuffer(byte[] buffer)
        {
            var connectedDevicesIds = new List<string>();
            var tempBuffer = new List<byte>();

            foreach (var character in buffer)
            {
                if (character != '\0')
                {
                    tempBuffer.Add(character);
                }
                else
                {
                    if (tempBuffer.Count > 0)
                    {
                        connectedDevicesIds.Add(System.Text.Encoding.ASCII.GetString(tempBuffer.ToArray()));
                    }
                    tempBuffer.Clear();
                }
            }

            return connectedDevicesIds;
        }

        public bool IsEmitterConnected()
        {
            if (sensationCoreInterop_ != null)
            {
                if (sensationCoreInterop_.uhsclIsEmitterConnected(sensationCoreInstancePtr_) != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public void ReleaseEmitter()
        {
            if (sensationCoreInterop_ != null)
            {
                sensationCoreInterop_.uhsclReleaseEmitter(sensationCoreInstancePtr_);
            }
        }

        public string EmitterModelDescription()
        {
            size_t length = (size_t)0;
            sensationCoreInterop_.uhsclEmitterModelDescriptionStringLength(sensationCoreInstancePtr_, out length);
            byte[] buffer = new byte[(uint)length + 1];

            sensationCoreInterop_.uhsclEmitterModelDescription(sensationCoreInstancePtr_, length, buffer);
            // Drop the null terminating character when creating the C# string
            return System.Text.Encoding.ASCII.GetString(buffer.Take(buffer.Length - 1).ToArray());
        }

        public string EmitterSerialNumber()
        {
            size_t length = (size_t)0;
            sensationCoreInterop_.uhsclEmitterSerialNumberStringLength(sensationCoreInstancePtr_, out length);
            byte[] buffer = new byte[(uint)length + 1];

            sensationCoreInterop_.uhsclEmitterSerialNumber(sensationCoreInstancePtr_, length, buffer);
            // Drop the null terminating character when creating the C# string
            return System.Text.Encoding.ASCII.GetString(buffer.Take(buffer.Length - 1).ToArray());
        }

        public string EmitterFirmwareVersion()
        {
            size_t length = (size_t)0;
            sensationCoreInterop_.uhsclEmitterFirmwareVersionStringLength(sensationCoreInstancePtr_, out length);
            byte[] buffer = new byte[(uint)length + 1];

            sensationCoreInterop_.uhsclEmitterFirmwareVersion(sensationCoreInstancePtr_, length, buffer);
            // Drop the null terminating character when creating the C# string
            return System.Text.Encoding.ASCII.GetString(buffer.Take(buffer.Length - 1).ToArray());
        }

        public void Mute(uhsclHandle playbackInstanceHandle)
        {
            if (sensationCoreInterop_ != null)
            {
                sensationCoreInterop_.uhsclMute(sensationCoreInstancePtr_, playbackInstanceHandle);
            }
        }
        public void Unmute(uhsclHandle playbackInstanceHandle)
        {
            if (sensationCoreInterop_ != null)
            {
                sensationCoreInterop_.uhsclUnmute(sensationCoreInstancePtr_, playbackInstanceHandle);
            }
        }
        public void SetPriority(uhsclHandle playbackInstanceHandle, uint priority)
        {
            if (sensationCoreInterop_ != null)
            {
                sensationCoreInterop_.uhsclSetPriority(sensationCoreInstancePtr_, playbackInstanceHandle, priority);
            }
        }

        public bool IsCurrentlyPlaying()
        {
            bool playing = false;
            if (sensationCoreInterop_ != null)
            {
                sensationCoreInterop_.uhsclIsCurrentlyPlaying(sensationCoreInstancePtr_, out playing);
            }
            return playing;
        }

        public uhsclHandle GetCurrentlyPlayingInstance()
        {
            uhsclHandle currentlyPlaying = uhsclHandle.INVALID_HANDLE;
            if (sensationCoreInterop_ != null)
            {
                sensationCoreInterop_.uhsclGetCurrentlyPlayingInstance(sensationCoreInstancePtr_, out currentlyPlaying);
            }
            return currentlyPlaying;
        }

        public void SetEvaluationHistorySize(uint size)
        {
            if (sensationCoreInterop_ != null)
            {
                evaluationHistorySize_ = size;
                sensationCoreInterop_.uhsclSetEvaluationHistorySizeOnCurrentPlaybackDevice(sensationCoreInstancePtr_, size);
            }
        }

        public List<Vector4> GetEvaluationHistory()
        {
            List<Vector4> evaluationHistoryUnityVector4 = new List<Vector4>();
            if (sensationCoreInterop_ != null)
            {

                uhsclVector4_t[] evaluationHistoryUhsclVector4 = new uhsclVector4_t[evaluationHistorySize_];
                uint size = 0;
                sensationCoreInterop_.uhsclGetEvaluationHistoryOnCurrentPlaybackDevice(sensationCoreInstancePtr_, evaluationHistoryUhsclVector4, out size);
                for(uint i = 0; i < size; i++)
                {
                    evaluationHistoryUnityVector4.Add(evaluationHistoryUhsclVector4[i].toVector4());
                }
            }
            return evaluationHistoryUnityVector4;
        }

        public int InputCount(uhsclHandle handle)
        {
            size_t count = 0;
            if (sensationCoreInterop_ != null)
            {
                sensationCoreInterop_.uhsclInputCount(sensationCoreInstancePtr_, handle, out count);
            }
            return Convert.ToInt32(count);
        }

        public int OutputCount(uhsclHandle blockHandle)
        {
            size_t count = 0;
            if (sensationCoreInterop_ != null)
            {
                sensationCoreInterop_.uhsclOutputCount(sensationCoreInstancePtr_, blockHandle, out count);
            }
            return Convert.ToInt32(count);
        }

        public uhsclHandle OutputHandleToBlockAtIndex(uhsclHandle blockHandle, int idx)
        {
            uhsclHandle outputHandle = uhsclHandle.INVALID_HANDLE;
            if (sensationCoreInterop_ != null)
            {
                sensationCoreInterop_.uhsclGetOutputAtIndex(sensationCoreInstancePtr_, blockHandle, idx, out outputHandle);
            }
            return outputHandle;
        }

        public string HandleName(uhsclHandle handle)
        {
            if (sensationCoreInterop_ != null)
            {
                size_t nameLength = 0; // TODO : use size_t uniformly, get name below expects length to have converted to int32...
                sensationCoreInterop_.uhsclGetNameLength(sensationCoreInstancePtr_, handle, out nameLength);
                byte[] nameBuffer = new byte[nameLength + 1];
                sensationCoreInterop_.uhsclGetName(sensationCoreInstancePtr_, handle, Convert.ToUInt64(nameBuffer.Length), nameBuffer);
                // Drop the null terminating character when creating the C# string
                return System.Text.Encoding.ASCII.GetString(nameBuffer.Take(nameBuffer.Length - 1).ToArray());
            }
            return null;
        }

        public uhsclHandle GetInputAtIndex(uhsclHandle blockHandle, int inputIdx)
        {
            if (sensationCoreInterop_ != null)
            {
                uhsclHandle inputHandle = uhsclHandle.INVALID_HANDLE;
                sensationCoreInterop_.uhsclGetInputAtIndex(sensationCoreInstancePtr_, blockHandle, inputIdx, out inputHandle);
                return inputHandle;
            }
            else
            {
                return uhsclHandle.INVALID_HANDLE;
            }
        }

        public UnityEngine.Vector3 GetInputAsVector3ByIndex(uhsclHandle handle, int inputIdx)
        {
            uhsclVector3_t value = new uhsclVector3_t(0, 0, 0);
            if (sensationCoreInterop_ != null)
            {
                sensationCoreInterop_.uhsclGetInputAsUhsclVector3ByIndex(sensationCoreInstancePtr_, handle, inputIdx, out value);
            }
            return value.toVector3();
        }

        public void SetInputToVector3(uhsclHandle blockHandle, uhsclHandle inputHandle, UnityEngine.Vector3 inputValue)
        {
            if (sensationCoreInterop_ != null)
            {
                sensationCoreInterop_.uhsclSetInputToUhsclVector3(sensationCoreInstancePtr_, blockHandle, inputHandle, new uhsclVector3_t(inputValue.x, inputValue.y, inputValue.z));
            }
        }

        public T GetMetaData<T>(uhsclHandle handle, string identifier)
        {
            if (typeof(T) == typeof(bool))
            {
                bool value;
                sensationCoreInterop_.uhsclGetMetaDataBool(sensationCoreInstancePtr_, handle, identifier, out value);
                return (T)(object)value;
            }
            else if (typeof(T) == typeof(string))
            {
                size_t valueLength;
                sensationCoreInterop_.uhsclGetMetaDataStringLength(sensationCoreInstancePtr_, handle, identifier, out valueLength);

                var valueBuffer = new byte[valueLength + 1];
                sensationCoreInterop_.uhsclGetMetaDataString(sensationCoreInstancePtr_, handle, identifier, Convert.ToUInt64(valueBuffer.Length), valueBuffer);

                // Drop the null terminating character when creating the C# string
                return (T)(object)System.Text.Encoding.ASCII.GetString(valueBuffer.Take(valueBuffer.Length - 1).ToArray());
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public List<string> GetSensationProducingBlockNames()
        {
            var numBlocks = default(size_t);
            var blockList = new List<string>();

            if (sensationCoreInterop_ == null)
            {
                throw new Exception ("SensationCoreInterop not initialised");
            }

            sensationCoreInterop_.uhsclGetBlockCount(sensationCoreInstancePtr_, out numBlocks);

            for(size_t i = 0; i < numBlocks; i++)
            {
                uhsclHandle blockHandle = uhsclHandle.INVALID_HANDLE;
                sensationCoreInterop_.uhsclGetBlockHandleAtIndex(sensationCoreInstancePtr_, i, out blockHandle);
                size_t outputCount = 0;
                sensationCoreInterop_.uhsclOutputCount(sensationCoreInstancePtr_, blockHandle, out outputCount);

                var count = Convert.ToInt64(outputCount);
                if (count == 0)
                {
                    continue;
                }

                uhsclHandle outputHandle = uhsclHandle.INVALID_HANDLE;
                sensationCoreInterop_.uhsclGetOutputAtIndex(sensationCoreInstancePtr_, blockHandle, 0, out outputHandle);

                bool sensationProducing = false;
                try
                {
                    sensationProducing = GetMetaData<bool>(outputHandle, "Sensation-Producing");
                }
                catch (ArgumentException)
                {
                    sensationProducing = true;
                }

                if (sensationProducing)
                {
                    var blockName = HandleName(blockHandle);
                    blockList.Add(blockName);
                }
            }

            return blockList;
        }

        public bool BlockSupportsTransformField(uhsclHandle blockHandle)
        {
            if (sensationCoreInterop_ == null)
            {
                throw new Exception ("SensationCoreInterop not initialised");
            }

            bool supportsTransform;
            try
            {
                supportsTransform = GetMetaData<bool>(blockHandle, "Allow-Transform");
            }
            catch (ArgumentException)
            {
                return false;
            }

            return supportsTransform;
        }

        public void CallUpdate()
        {
            if (sensationCoreInterop_ != null)
            {
                if (IsEmitterConnected())
                {
                    sensationCoreInterop_.uhsclUpdate(sensationCoreInstancePtr_);
                }
            }
        }

        public void Dispose()
        {
            OnDestroy();
        }
    }
}
