﻿using System;

namespace UltrahapticsCoreAsset
{
    // This is required to be kept in sync with the enum defined in the C++ library
    public enum uhsclErrorCode_t : Int32
    {
        NoError = 0,
        UninitialisedSensationCore = 1,
        NullOutputPointer = 2,
        InvalidHandle = 3,
        InvalidArgument = 4,
        InvalidOperation = 5,
        HardwareError = 6,
        UnknownError = 7,
    }
}
