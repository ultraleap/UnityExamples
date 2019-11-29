#pragma once

#include <stdint.h>

typedef enum
{
    NoError = 0,
    UninitialisedSensationCore = 1,
    NullOutputPointer = 2,
    InvalidHandle = 3,
    InvalidArgument = 4,
    InvalidOperation = 5,
    HardwareError = 6,
    UnknownError = 7
} uhsclErrorCode_e;
typedef int32_t uhsclErrorCode_t;

