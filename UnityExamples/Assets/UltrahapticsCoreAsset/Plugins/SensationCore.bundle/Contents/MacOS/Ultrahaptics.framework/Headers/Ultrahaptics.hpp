
#pragma once

#include "UltrahapticsDecoration.hpp"
#include "UltrahapticsVersion.hpp"
#include "UltrahapticsVector3.hpp"
#include "UltrahapticsUnits.hpp"
#include "UltrahapticsAlignment.hpp"
#include "UltrahapticsDeviceInfo.hpp"

#ifdef ULTRAHAPTICS_AMPLITUDE_MODULATION_API_HPP__
namespace Ultrahaptics
{
  namespace AmplitudeModulation
  {
    class ControlPoint;
    class Emitter;
  }
}
#else
#include "UltrahapticsAmplitudeModulation.hpp"
#endif

#define ENABLE_LEGACY_V1_ULTRAHAPTICS
#ifdef ENABLE_LEGACY_V1_ULTRAHAPTICS
namespace Ultrahaptics
{
  typedef Ultrahaptics::AmplitudeModulation::ControlPoint ControlPoint;
  typedef Ultrahaptics::AmplitudeModulation::Emitter Emitter;
}
#endif

/// \brief The Ultrahaptics namespace contains everything in this library.
///
/// If typing `Ultrahaptics::` everywhere gets too tedious we recommend using a shorter
/// synonym like this:
///
///     namespace uh = Ultrahaptics;
///
namespace Ultrahaptics
{
   class LoggingExternalInterface;
   class UltrahapticsLibraryImplementationSingleton;
   class DriverLibrary;

   /// \brief Represents the %Ultrahaptics library.
   ///
   /// An internal reference-counted singleton library class
   /// is created the first time an instance of either this class, TimePointStreaming::Emitter, or
   /// AmplitudeModulation::Emitter is created. An instance of this class can be created at any
   /// time to provide access to its functions, e.g. to query the device count or set the logging level.
   ///
   /// Although this class cannot be copied, additional instances of it can be created - they will all
   /// point to the same internal library instance.
   ///
   /// The main action performed by library initialisation is to load all of the `*.system.xml` files.
   ///
   /// Since the internal library singleton is reference counted, when the last instance of UltrahapticsLibrary,
   /// TimePointStreaming::Emitter or AmplitudeModulation::Emitter is destroyed, the underlying
   /// singleton is also destroyed.
   class UH_API_CLASS_DECORATION UltrahapticsLibrary
   {
      public:
         /** Constructs all of the resources needed by the %Ultrahaptics library */
        UH_API_DECORATION UltrahapticsLibrary()
        {
            init();
            VersionInfo::checkVersion();
        }

         /** Move constructor */
         UH_API_DECORATION UltrahapticsLibrary(UltrahapticsLibrary &&other);

         /** Move assignment operator */
         UH_API_DECORATION UltrahapticsLibrary &operator=(UltrahapticsLibrary &&other);

         /** Deleted copy constructor */
         UltrahapticsLibrary(const UltrahapticsLibrary &other) = delete;

         /** Deleted Copy assignment operator */
         UltrahapticsLibrary &operator=(const UltrahapticsLibrary &other) = delete;

         /** Destroys the library and all its resources */
         UH_API_DECORATION ~UltrahapticsLibrary();

         /** @return The number of devices currently connected */
         UH_API_DECORATION unsigned int getDeviceCount() const;

         /** Get the identifying name of a currently connected device. The
          * buffer will be null terminated but only if it contains enough space
          * for the null character, so you should not rely on that.
          *
          * Simple use using a large buffer is:
          *
          * ```
          * char buffer[256] = {};
          * unsigned int sz = lib.getDeviceIdentifier(i, buffer, sizeof(buffer));
          * std::string identifier(buffer, sz);
          * ```
          *
          * See [Board Identification](BoardIdentification.md).
          *
          * @param device_index The zero-indexed index of the device to get the
          * name of.
          *
          * @param name_buffer The caller-owned buffer to write the
          * identifier of the device into. This can be null.
          *
          * @param buffer_size The size of the buffer that has been given
          * to write the name of the device into. This is premitted to be
          * insufficient.
          *
          * @return The number of bytes which would have been written to the
          * buffer, regardless of whether or not the buffer exists or was big enough. */
         UH_API_DECORATION unsigned int getDeviceIdentifier(
           unsigned int device_index, char *name_buffer, unsigned int buffer_size) const;

         /** Get detailed information about a connected device
          *
          * @param device_identifier The device identifier to access information about
          * @return A DeviceInfo object with information about the device, or
          *         a blank object if the requested device is not connected. */
         UH_API_DECORATION DeviceInfo getDeviceInfoByIdentifier(const char* device_identifier) const;
         /** Get detailed information about a connected device
          *
          * @param device_index The index of the device to retrieve information about
          * @return A DeviceInfo object with information about the device, or
          *         a blank object if the requested device is not connected. */
         UH_API_DECORATION DeviceInfo getDeviceInfoByIndex(unsigned int device_index) const;

         /** Determine whether a specific device is connected
          *
          * @param device_identifier The device identifier to query
          * @return true if the device is detected as being connected, false if it is not */
         UH_API_DECORATION bool isDeviceConnected(const char* device_identifier) const;

         /** Determine whether a specific device is both connected and claimed as being in use by this process
          *
          * Note that this will only identify devices which are in use by this process. Devices
          * in use by other processes will not appear as connected and will not be available for use.
          *
          * @param device_identifier The device identifier to query
          * @return true if the device is connected and in use, false if this is not the case */
         UH_API_DECORATION bool isDeviceClaimed(const char* device_identifier) const;

         /** @return Pointer to driver library */
         UH_API_DECORATION DriverLibrary &getDriverLibrary();
         /** @return Pointer to driver library */
         UH_API_DECORATION const DriverLibrary &getDriverLibrary() const;

         /** @return A logging interface to modify the library's logging behaviour */
         UH_API_DECORATION LoggingExternalInterface &getLoggingInterface() const;

         /** Informs the caller whether or not the SDK is currently configured to
          * allow changing the priority of the current process to improve performance
          *
          * Please note that management of process priorities is not supported on all platforms;
          * this value being true does not necessarily mean that it is in effect, merely that it is permitted.
          *
          * @return True if the SDK is currently allowed to modify the OS priority of the current process; false if not */
         UH_API_DECORATION bool isProcessPriorityManagementAllowed() const;
         /** Informs the caller whether the SDK is currently configured to
          * allow changing the priority of SDK threads to improve performance
          *
          * Please note that management of thread priorities is not supported on all platforms;
          * this value being true does not necessarily mean that it is in effect, merely that it is permitted.
          *
          * @return True if the SDK is currently allowed to modify the OS priority of SDK-controlled threads; false if not */
         UH_API_DECORATION bool isThreadPriorityManagementAllowed() const;
         /** Controls whether or not the SDK is allowed to reconfigure
          * the OS priority of the current process to improve performance.
          *
          * Please note that management of process priorities is not supported on all platforms;
          * setting this value to true does not necessarily mean that it is in effect, merely that it is permitted.
          *
          * @param allowed If true, allows the SDK to change the current process's priority */
         UH_API_DECORATION void setProcessPriorityManagementAllowed(bool allowed);
         /** Controls whether or not the SDK is allowed to reconfigure the OS priority
          * of SDK-controlled threads within the current process to improve performance.
          *
          * Please note that management of process priorities is not supported on all platforms;
          * setting this value to true does not necessarily mean that it is in effect, merely that it is permitted.
          *
          * @param allowed If true, allows the SDK to change the priority of SDK-controlled threads */
         UH_API_DECORATION void setThreadPriorityManagementAllowed(bool allowed);

         /** Recheck all hardware configurations (*.system.xml files)
          *
          * @return True if the reload was performed, false if not */
         bool reloadHardwareConfigurations();

         /** Recheck all hardware configurations (*.system.xml files)
          *
          * @param directory A path to an additional directory to search for configurations
          *
          * @return True if the reload was performed, false if not */
         bool reloadHardwareConfigurations(const char* directory);

      private:
         /** Pointer for holding the driver library implementation */
         UltrahapticsLibraryImplementationSingleton *ultrahaptics_library_singleton;
         UH_API_DECORATION void init();
   };
}


