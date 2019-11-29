#pragma once

namespace Ultrahaptics {
/** Device Selection Mode.
 *
 * This determines how an Emitter with multiple devices decides which device to emit to. It
 * currently supports the following modes:
 *
 * * **Distance**
 *
 *   This mode will select the device whose center is closest to the point being emitted.
 *
 * * **Direction**
 *
 *   This mode will select the device where the angle between the device's normal and the point's
 *   direction is the smallest.

 * * **Hybrid**
 *
 *   This mode will select the closest device. If the angle between the device's normal and the
 *   point's direction is larger than 60°, that device is ignored in the selection process.
 */
enum class DeviceSelectionMode
{
    /** Device is selected based on distance between the point and the device. */
    Distance,
    /** Device is selected based on angle between the points direction and the normal of the device. */
    Direction,
    /** Device is selected based on a mix between Direction and Distance. */
    Hybrid
};
} // namespace Ultrahaptics
