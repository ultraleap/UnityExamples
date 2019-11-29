#pragma once

#include "uhsclErrorCode.h"
#include "uhsclHandle.h"
#include "uhsclVector.h"

#include "sensationcore_export.h"
#include "sensationcore_version.h"

#include <stddef.h>

#ifdef __cplusplus
extern "C" {
#endif

typedef struct uhsclInstance_t * uhsclInstance;
typedef uhsclHandle_t uhsclInputPortHandle;
typedef uhsclHandle_t uhsclOutputPortHandle;
typedef uhsclHandle_t * uhsclChannelHandle;
typedef struct uhsclBlockInstance * uhsclBlockInstanceHandle;
typedef struct uhsclInputInstance * uhsclInputInstanceHandle;
typedef struct uhsclOutputInstance * uhsclOutputInstanceHandle;
typedef const struct uhsclEvaluationContext * uhsclEvaluationContextHandle;

typedef uhsclVector3_t (*uhsclBehaviour)(void *, uhsclEvaluationContextHandle);

SENSATIONCORE_EXPORT uhsclInputInstanceHandle uhsclInputInstance(uhsclBlockInstanceHandle,
                                                                 uhsclInputPortHandle);

SENSATIONCORE_EXPORT uhsclOutputInstanceHandle uhsclOutputInstance(uhsclBlockInstanceHandle,
                                                                   uhsclOutputPortHandle);

SENSATIONCORE_EXPORT uhsclVector3_t uhsclPortValueFromContext(uhsclEvaluationContextHandle, uhsclHandle_t port);
SENSATIONCORE_EXPORT extern uhsclChannelHandle uhsclUChannel;
SENSATIONCORE_EXPORT double uhsclChannelValueFromContext(uhsclEvaluationContextHandle, uhsclChannelHandle channel);

SENSATIONCORE_EXPORT const char *uhsclVersion(void);

SENSATIONCORE_EXPORT uhsclInstance uhsclCreate(void);
SENSATIONCORE_EXPORT void uhsclRelease(uhsclInstance);

SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclImportPythonModule(uhsclInstance instance, const char *modulename);

SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclGetNameLength(uhsclInstance, uhsclHandle_t handle, size_t *length);

SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclGetName(uhsclInstance, uhsclHandle_t handle, size_t maxLength, char *nameBuffer);

SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclCreateBlock(uhsclInstance, const char *identifier, uhsclHandle_t *handle);
SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclCreateBlockInstance(uhsclInstance, uhsclHandle_t block, const char *instanceName, uhsclBlockInstanceHandle *handle);

SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclInputCount(uhsclInstance, uhsclHandle_t handle, size_t *count);
SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclGetInputAtIndex(uhsclInstance, uhsclHandle_t blockHandle, int32_t index, uhsclHandle_t *inputHandle);

SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclOutputCount(uhsclInstance, uhsclHandle_t handle, size_t *count);
SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclGetOutputAtIndex(uhsclInstance, uhsclHandle_t blockHandle, int32_t index, uhsclHandle_t *outputHandle);

SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclSetMetaDataBool(uhsclInstance, uhsclHandle_t handle, const char* identifier, int32_t boolValue);
SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclGetMetaDataBool(uhsclInstance, uhsclHandle_t handle, const char* identifier, int32_t *boolValue);

SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclSetMetaDataString(uhsclInstance, uhsclHandle_t handle, const char* identifier, const char* stringValue);
SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclGetMetaDataStringLength(uhsclInstance, uhsclHandle_t handle, const char* identifier, size_t *length);
SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclGetMetaDataString(uhsclInstance, uhsclHandle_t handle, const char* identifier, size_t maxLength, char* stringValue);

SENSATIONCORE_EXPORT int32_t uhsclIsSensationProducing(uhsclInstance, uhsclHandle_t block, uhsclErrorCode_t *err);

// TODO: Move to Playback API

SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclCreateInputSource(uhsclInstance, uhsclHandle_t blockHandle, uhsclHandle_t *inputSource);

SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclSetInputToUhsclVector3ByIndex(uhsclInstance instance, uhsclHandle_t inputSource, int32_t idx, uhsclVector3_t inputVector);
SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclSetInputToUhsclVector3(uhsclInstance instance, uhsclHandle_t inputSource, uhsclHandle_t port, uhsclVector3_t inputVector);

SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclGetInputAsUhsclVector3ByIndex(uhsclInstance instance, uhsclHandle_t inputSource, int32_t idx, uhsclVector3_t *value);
// TODO : GetInput

SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclStart(uhsclInstance, uhsclHandle_t outputHandle, uhsclHandle_t inputSource, uhsclHandle_t *playbackInstanceHandle);
SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclStop(uhsclInstance, uhsclHandle_t playbackInstanceHandle);
SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclMute(uhsclInstance, uhsclHandle_t playbackInstanceHandle);
SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclUnmute(uhsclInstance, uhsclHandle_t playbackInstanceHandle);
SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclSetPriority(uhsclInstance, uhsclHandle_t playbackInstanceHandle, uint32_t priority);

SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclIsCurrentlyPlaying(uhsclInstance, int32_t *playing);
SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclGetCurrentlyPlayingInstance(uhsclInstance, uhsclHandle_t *playbackInstanceHandle);

SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclUpdate(uhsclInstance);

SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclAcquireEmitter(uhsclInstance);
SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclAcquireMockEmitter(uhsclInstance, const char *deviceModel, const char* logFileRelativePath);
SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclAddDevice(uhsclInstance instance, const char* deviceId, const float* transform);
SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclSetDeviceTransform(uhsclInstance instance, const char* deviceId, const float* transform);
SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclGetConnectedDevicesStringLength(uhsclInstance instance, size_t *length);
SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclGetConnectedDevices(uhsclInstance instance, size_t allocatedLength, char* buffer);

SENSATIONCORE_EXPORT int32_t uhsclIsEmitterConnected(uhsclInstance);
SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclReleaseEmitter(uhsclInstance);

SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclEmitterModelDescriptionStringLength(uhsclInstance, size_t *length);
SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclEmitterModelDescription(uhsclInstance,
                                                                   size_t allocatedLength,
                                                                   char *buffer);
SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclEmitterSerialNumberStringLength(uhsclInstance, size_t *length);
SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclEmitterSerialNumber(uhsclInstance, size_t allocatedLength, char *buffer);
SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclEmitterFirmwareVersionStringLength(uhsclInstance, size_t *length);
SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclEmitterFirmwareVersion(uhsclInstance, size_t allocatedLength, char *buffer);

SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclSetEvaluationHistorySizeOnCurrentPlaybackDevice(uhsclInstance, uint32_t size);
SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclGetEvaluationHistoryOnCurrentPlaybackDevice(uhsclInstance, uhsclVector4_t *evaluationHistory, uint32_t *size);
// End Playback API

SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclAddSearchPath(uhsclInstance, const char *path);

SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclDefineBlock(uhsclInstance, const char *name, uhsclHandle_t *blockDefinitionHandle);
SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclDefineBlockInput(uhsclInstance, uhsclHandle_t blockDefinitionHandle, const char *inputName, uhsclHandle_t *blockInputHandle);
SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclDefineBlockOutput(uhsclInstance, uhsclHandle_t blockDefinitionHandle, const char *outputName, uhsclHandle_t *blockOutputHandle);

SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclDefineBlockOutputBehaviour(uhsclInstance, uhsclHandle_t blockOutputHandle, void *callbackData, uhsclBehaviour behaviour);

SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclGetOutputByName(uhsclInstance, uhsclHandle_t blockDefinitionHandle, const char *outputName, uhsclHandle_t *blockOutputHandle);
SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclGetInputByName(uhsclInstance, uhsclHandle_t blockDefinitionHandle, const char *inputName, uhsclHandle_t *blockInputHandle);

SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclEvaluateBlock(uhsclInstance, uhsclHandle_t blockHandle, uhsclVector3_t *value, const uhsclVector3_t *inputAsVectors);

SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclConnectChildOutputToParentOutput(uhsclInstance,
                                                                            uhsclOutputInstanceHandle child,
                                                                            uhsclHandle_t parent);
SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclConnectParentInputToChildInput(uhsclInstance,
                                                                          uhsclHandle_t parent,
                                                                          uhsclInputInstanceHandle child);
SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclConnectChildren(uhsclInstance,
                                                           uhsclOutputInstanceHandle source,
                                                           uhsclInputInstanceHandle destination);

//TO DO: This could be removed from the public API, as a constant block could already be created using the other functions
SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclDefineConstant(uhsclInstance, uhsclVector3_t value, uhsclHandle_t *outputHandle);

SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclGetBlockCount(uhsclInstance, size_t*);
SENSATIONCORE_EXPORT uhsclErrorCode_t uhsclGetBlockHandleAtIndex(uhsclInstance, size_t, uhsclHandle_t *);

SENSATIONCORE_EXPORT size_t uhsclGetLogInfoMessageBufferSize(uhsclInstance);
SENSATIONCORE_EXPORT void uhsclGetLogInfoMessageBufferAndClear(uhsclInstance, size_t, char *);

SENSATIONCORE_EXPORT size_t uhsclGetLogWarningMessageBufferSize(uhsclInstance);
SENSATIONCORE_EXPORT void uhsclGetLogWarningMessageBufferAndClear(uhsclInstance, size_t, char *);

SENSATIONCORE_EXPORT size_t uhsclGetLogErrorMessageBufferSize(uhsclInstance);
SENSATIONCORE_EXPORT void uhsclGetLogErrorMessageBufferAndClear(uhsclInstance, size_t, char *);

#ifdef __cplusplus
}
#endif
