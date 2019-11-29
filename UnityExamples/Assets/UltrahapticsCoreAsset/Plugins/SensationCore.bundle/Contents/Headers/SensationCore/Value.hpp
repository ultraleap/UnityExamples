#pragma once

#include "sensationcore_export.h"

#include <boost/any.hpp>

namespace SensationCore {

using uhsclValue_t = boost::any;
extern const uhsclValue_t uhsclNoneValue;

SENSATIONCORE_EXPORT bool isNoneValue(uhsclValue_t const &v);

}
