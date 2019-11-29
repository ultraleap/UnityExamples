#pragma once

#include "sensationcore_export.h"
#include "uhsclErrorCode.h"
#include "uhsclHandle.h"
#include "Value.hpp"

namespace SensationCore {

class SENSATIONCORE_EXPORT ISensationInputSource
{
public:
    virtual ~ISensationInputSource() = default;

    virtual uhsclErrorCode_t createInputSource(uhsclHandle_t blockHandle, uhsclHandle_t *inputSourceHandle) = 0;
    virtual uhsclErrorCode_t blockHandleForInputSource(uhsclHandle_t inputSourceHandle, uhsclHandle_t *blockHandle) const = 0;

    template <typename T>
    static uhsclValue_t convertToValue(const T &value)
    {
        return value;
    }

    template <typename T>
    static bool matchesType(uhsclValue_t const &value)
    {
        return typeid(T) == value.type();
    }

    virtual uhsclErrorCode_t setInputToValue(uhsclHandle_t inputSourceHandle, uhsclHandle_t inputHandle, const uhsclValue_t &value) = 0;
    virtual uhsclErrorCode_t getValueForInput(uhsclHandle_t inputSourceHandle, uhsclHandle_t inputHandle, uhsclValue_t *result) const = 0;
    virtual uhsclErrorCode_t hasValueForInput(uhsclHandle_t inputSourceHandle, uhsclHandle_t inputHandle, bool *result) const = 0;

    template <typename T>
    static T const getValueAs(uhsclValue_t const &value)
    {
        if (matchesType<T>(value))
        {
            return boost::any_cast<T>(value);
        }
        else
        {
            throw std::bad_cast();
        }
    }
};

} // namespace SensationCore
