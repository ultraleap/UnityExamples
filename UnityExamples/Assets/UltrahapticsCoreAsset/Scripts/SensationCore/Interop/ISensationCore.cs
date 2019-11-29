using System;
using System.Collections.Generic;
using UnityEngine;

namespace UltrahapticsCoreAsset
{

public interface ISensationCore : IDisposable
{
    uhsclHandle CreateBlock(string name);

    uhsclHandle CallStart(uhsclHandle blockHandle, uhsclHandle inputSourceHandle);
    void Stop(uhsclHandle playbackInstance);
    void Mute(uhsclHandle playbackInstance);
    void Unmute(uhsclHandle playbackInstance);
    void SetPriority(uhsclHandle playbackInstance, uint priority);

    bool IsCurrentlyPlaying();
    uhsclHandle GetCurrentlyPlayingInstance();

    uhsclHandle CreateInputSource(uhsclHandle blockHandle);

    void AcquireEmitter();
    void AcquireMockEmitter();
    bool IsEmitterConnected();
    void AddDevice(string deviceId, UnityEngine.Transform transform);
    void SetDeviceTransform(string deviceId, UnityEngine.Transform transform);
    List<string> GetConnectedDevices();

    void ReleaseEmitter();
    string EmitterModelDescription();
    string EmitterSerialNumber();
    string EmitterFirmwareVersion();
    void CallUpdate();
    void SetEvaluationHistorySize(uint size);
    List<UnityEngine.Vector4> GetEvaluationHistory();

    int InputCount(uhsclHandle blockHandle);
    string HandleName(uhsclHandle handle);
    uhsclHandle GetInputAtIndex(uhsclHandle blockHandle, int inputIdx);

    UnityEngine.Vector3 GetInputAsVector3ByIndex(uhsclHandle blockHandle, int inputIdx);
    void SetInputToVector3(uhsclHandle blockHandle, uhsclHandle inputHandle, UnityEngine.Vector3 inputValue);

    void CreateSensationCore(ISensationCoreInterop sensationCoreInterop = null);
    List<string> GetSensationProducingBlockNames();
    bool BlockSupportsTransformField(uhsclHandle blockHandle);

    T GetMetaData<T>(uhsclHandle handle, string identifier);
}

}
