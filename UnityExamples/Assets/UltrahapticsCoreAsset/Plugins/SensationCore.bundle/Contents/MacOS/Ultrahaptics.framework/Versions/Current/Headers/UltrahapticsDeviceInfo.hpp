#pragma once

#include "UltrahapticsDecoration.hpp"
#include "UltrahapticsAlignment.hpp"
#include "UltrahapticsTransducers.hpp"


namespace Ultrahaptics
{
  /** \cond EXCLUDE_FROM_DOCS */
  class DeviceInfoImplementation;
  class UltrahapticsLibraryImplementation;
  class BaseEmitterBackend;
  /** \endcond */

  /** Flags indicating features which devices may support */
  enum class DeviceFeatures : unsigned int
  {
    /** The device supports fire-and-forget amplitude modulation operation */
    AMPLITUDE_MODULATION = (1 << 0),
    /** The device supports streaming data constantly to maintain operation */
    TIMEPOINT_STREAMING  = (1 << 1)
  };

  /** Bitwise "or" operator for device features enum */
  UH_API_DECORATION DeviceFeatures operator|(DeviceFeatures lhs, DeviceFeatures rhs);
  /** Bitwise "and" operator for device features enum */
  UH_API_DECORATION DeviceFeatures operator&(DeviceFeatures lhs, DeviceFeatures rhs);
  /** Bitwise "xor" operator for device features enum */
  UH_API_DECORATION DeviceFeatures operator^(DeviceFeatures lhs, DeviceFeatures rhs);
  /** Bitwise "not" operator for device features enum */
  UH_API_DECORATION DeviceFeatures operator~(DeviceFeatures lhs);
  /** Bitwise "or" assignment operator for device features enum */
  UH_API_DECORATION DeviceFeatures& operator|=(DeviceFeatures& lhs, DeviceFeatures rhs);
  /** Bitwise "and" assignment operator for device features enum */
  UH_API_DECORATION DeviceFeatures& operator&=(DeviceFeatures& lhs, DeviceFeatures rhs);
  /** Bitwise "xor" assignment operator for device features enum */
  UH_API_DECORATION DeviceFeatures& operator^=(DeviceFeatures& lhs, DeviceFeatures rhs);


  /** This represents information about a device at the time it was queried.
   *
   * No persistent link to the device is maintained via this class, so there is no guarantee
   * that the device has not been disconnected or altered since the information was gathered. */
  class UH_API_CLASS_DECORATION DeviceInfo
  {
  friend class UltrahapticsLibraryImplementation;
  friend class BaseEmitterBackend;
  public:
    /** Copy-construct a DeviceInfo object from another */
    UH_API_DECORATION DeviceInfo(const DeviceInfo& other);
    /** Copy-assign a DeviceInfo object from another */
    UH_API_DECORATION DeviceInfo& operator=(const DeviceInfo& other);
    /** Move-construct a DeviceInfo object from another */
    UH_API_DECORATION DeviceInfo(DeviceInfo&& other);
    /** Move-assign a DeviceInfo object from another */
    UH_API_DECORATION DeviceInfo& operator=(DeviceInfo&& other);
    /** Destroy the object */
    UH_API_DECORATION ~DeviceInfo();

    /** Get the full device identifier for this device. This identifier is often used
     * in other parts of the SDK to specify a device to use.
     *
     * Please note that this returns a pointer to a string held within the DeviceInfo object.
     *
     * @return The device identifier */
    UH_API_DECORATION const char* getDeviceIdentifier() const;

    /** Gets the model identifier used by the SDK for this model of device.
     *
     * Please note that this returns a pointer to a string held within the DeviceInfo object.
     *
     * @return The model identifier */
    UH_API_DECORATION const char* getModelIdentifier() const;
    /** Gets a text description of the model of this device.
     *
     * Please note that this returns a pointer to a string held within the DeviceInfo object.
     *
     * @return The model description */
    UH_API_DECORATION const char* getModelDescription() const;
    /** Gets this device's serial number.
     *
     * Please note that this returns a pointer to a string held within the DeviceInfo object.
     *
     * @return The serial number */
    UH_API_DECORATION const char* getSerialNumber() const;

    /** Gets the name used to identify the transducer layout used by this board.
     * If no layouts are defined for the model of device in use, "default" will be used.
     *
     * Please note that this returns a pointer to a string held within the DeviceInfo object.
     *
     * @return The layout name */
    UH_API_DECORATION const char* getTransducerLayoutName() const;

    /** Gets the version of the firmware currently installed on this device.
     *
     * Please note that this returns a pointer to a string held within the DeviceInfo object.
     *
     * @return The firmware version string */
    UH_API_DECORATION const char* getFirmwareVersion() const;
    /** Gets the build date of the firmware currently installed on this device.
     *
     * Please note that this returns a pointer to a string held within the DeviceInfo object.
     *
     * @return The firmware build date string */
    UH_API_DECORATION const char* getFirmwareBuildDate() const;

    /** @return The default Alignment object appropriate for this device and transducer layout */
    UH_API_DECORATION Alignment getDefaultAlignment() const;

    /** @return The current transducer configuration of this device, as used by the SDK */
    UH_API_DECORATION TransducerContainer getTransducerConfiguration() const;
    /** @return Width of the device. */
    UH_API_DECORATION float arrayWidth() const;
    /** @return Depth of the device. */
    UH_API_DECORATION float arrayDepth() const;

    /** Get a bitfield representing the set of features supported by this device.
     *
     * Note that the value returned is a set of flags (a bitfield), so it
     * may not match any individual value of the DeviceFeatures enumeration.
     *
     * @return The set of features supported by this device */
    UH_API_DECORATION DeviceFeatures getSupportedFeatures() const;

    /** Checks whether this device supports a specific feature set
     *
     * The argument may be a single feature, or a combination
     * of features specified by using the bitwise OR operator.
     *
     * @param features The feature(s) to check for support for
     * @return True if the device supports this feature set, false if not */
    UH_API_DECORATION bool hasSupportFor(DeviceFeatures features) const;

  private:
    /** \cond EXCLUDE_FROM_DOCS */
    DeviceInfo(DeviceInfoImplementation* impl);
    DeviceInfoImplementation* impl;
    /** \endcond */
  };
}

