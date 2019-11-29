#pragma once

namespace Ultrahaptics
{
  /** Log level for a message */
  enum class Message
  {
    /** A fatal error indicating that further operation, if possible, will be unpredictable */
    FATAL_ERROR = 0,
    /** An error which may impact operation of the system */
    ERROR = 1,
    /** A warning which might indicate suboptimal operation of the system */
    WARNING = 2,
    /** Informational output */
    VERBOSE = 3,
    /** Extremely verbose informational output */
    EXTRA_VERBOSE = 4
  };
  
  /** Error codes for specific error conditions */
  enum class ErrorCode
  {
    /** The attached log message does not indicate an error */
    NO_ERROR                                  = 0x00000000,
    /** No additional information is available about the error */
    UNKNOWN_ERROR                             = 0x00000001,
    /** The error was related to internal operation of the system */
    INTERNAL_ERROR                            = 0x00000002,
    /** The SDK failed to access a file, likely due to non-existence or permission issues */
    FILE_ACCESS_ERROR                         = 0x00000003,
    /** There is a mismatch between the version of the header and the linked library */
    VERSION_MISMATCH                          = 0x00000004,
    /** The versions in the header and the linked library are known to be incompatible */
    VERSION_INCOMPATIBLE                      = 0x00000005,
    /** An operation involving a device was requested, but was unable to find a device to use */
    DEVICE_NOT_DETECTED                       = 0x00000100,
    /** The requested device is in DFU mode, and as such cannot be used by the SDK until restarted */
    DEVICE_IN_DFU_MODE                        = 0x00000101,
    /** The requested device is using a firmware version not supported by this SDK */
    DEVICE_INVALID_FIRMWARE_VERSION           = 0x00000102,
    /** The requested device could not be initialised */
    DEVICE_FAILED_TO_INITIALISE               = 0x00000103,
    /** The requested device could not be claimed, possibly indicating it is in use by another process */
    DEVICE_FAILED_TO_CLAIM                    = 0x00000104,
    /** The available device does not support the capabilities requested */
    DEVICE_CAPABILITIES_MISMATCH              = 0x00000105,
    /** A device was not able to estimate the host-to-device or device-to-host latency */
    DEVICE_FAILED_TO_ESTIMATE_LATENCY         = 0x00000106,
    /** The device may not be used yet as it is in the process of initialising */
    DEVICE_INITIALISATION_IN_PROGRESS         = 0x00000107,
    /** This emitter is in an invalid state for the requested operation (may be disconnected) */
    EMITTER_INVALID                           = 0x00000200,
    /** This emitter could not initialise properly */
    EMITTER_INITIALISATION_FAILED             = 0x00000201,
    /** Emitter initialisation was attempted but has already been performed */
    EMITTER_ALREADY_INITIALISED               = 0x00000202,
    /** Emitter failed to send an update to the device; frequent occurrences of this message may result in dropouts */
    EMITTER_FAILED_TO_SEND_TIMEPOINT_UPDATE   = 0x00000203,
    /** The emitter was requested to stop but was unable to do so */
    EMITTER_FAILED_TO_STOP                    = 0x00000204,
    /** An attempt was made to modify a property of an emitter which may only be changed while the emitter is stopped */
    EMITTER_CANNOT_MODIFY_WHILE_RUNNING       = 0x00000205,
    /** Failed to load alignment data from a file or string */
    ALIGNMENT_DATA_FAILED_TO_LOAD             = 0x00000300,
    /** Failed to save alignment data to a file or string */
    ALIGNMENT_DATA_FAILED_TO_SAVE             = 0x00000301,
    /** Failed to load a device's hardware configuration file (system.xml) */
    FAILED_TO_LOAD_DEVICE_CONFIGURATION       = 0x00000302,
    /** An error was encountered when communicating with the device over USB */
    USB_COMMUNICATION_FAILURE                 = 0x00000400,
    /** The requested control point configuration included points closer together than permitted */
    FOCAL_POINTS_TOO_CLOSE_TOGETHER           = 0x00000500,
    /** The requested control point configuration included more control points than permitted */
    CANNOT_EXCEED_CONTROL_POINT_COUNT_LIMIT   = 0x00000501,
    /** An attempt was made to access a control point index beyond the currently available set */
    CANNOT_ACCESS_CONTROL_POINT_OUTSIDE_RANGE = 0x00000502,
    /** The emitter's user callback took too long to run, resulting in data being unavailable to send to the device */
    EMITTER_TIMEPOINT_CALLBACK_TOO_SLOW       = 0x00000503,
    /** The requested control point configuration was outside the allowed bounding box */
    CONTROL_POINTS_OUTSIDE_ALLOWED_AREA       = 0x00000504,
    /** The requested update rate is not valid for the current configuration */
    INVALID_DEVICE_UPDATE_RATE                = 0x00000505,
    /** The control point data provided to the device included floating-point NaNs */
    CONTROL_POINT_DATA_IS_NAN                 = 0x00000506
  };
}

