
#pragma once

#include "UltrahapticsControlPointBase.hpp"
#include "UltrahapticsHostTime.hpp"
#include "UltrahapticsDeviceSelectionMode.hpp"

#include "UltrahapticsTransform.hpp"

namespace Ultrahaptics {
/** \cond EXCLUDE_FROM_DOCS */
class DriverDevice;
/** \endcond */

/// \brief TimePointStreaming provides explicit control of the haptic modulation signal.
///
/// Unlike the AmplitudeModulation API, where a default sine wave modulation is always used, Time
/// Point Streaming allows an arbitrary modulation signal to be specified. This provides more
/// advanced control over the haptic sensation and even allows you to use the array to play music.
///
/// See \ref TimePointStreaming.md for an introduction to the Time Point Streaming API, and
/// TimePointStreaming::Emitter for API details.
namespace TimePointStreaming {
/** \brief Point at which the air pressure is controlled, with position and intensity.
 *
 * A control point is simply a point in space at which you can specify
 * the desired SPL (Sound Pressure Level) modulation. In almost all cases
 * the control point will coincide with the focal point of the ultrasound.
 *
 * This class differs from Ultrahaptics::AmplitudeModulation::ControlPoint in that the
 * it does not include a frequency since the modulation is not necessarily sinusoidal -
 * modulation is acheived by calling setIntensity() with different values over time.
 * Unlike Ultrahaptics::AmplitudeModulation::ControlPoint, this class represents
 * a control point at a specific point in time.
 *
 * This class is essentially an import of Ultrahaptics::ControlPointBase into this namespace.
 * It can be assigned to ControlPointPersistent in the emission callback. See ControlPointPersistent
 * for more details.
 */
struct ControlPoint : public Ultrahaptics::ControlPointBase {
    /** Control point constructor. */
    ControlPoint() : Ultrahaptics::ControlPointBase() {}

    /** Control point constructor with x, y, z and amplitude.
     *
     * @param x The x position of the control point
     *
     * @param y The y position of the control point
     *
     * @param z The z position of the control point
     *
     * @param iintensity The amplitude of the control point
     */
    ControlPoint(const float x, const float y, const float z, const float iintensity)
        : Ultrahaptics::ControlPointBase(x, y, z, iintensity)
    {
    }

    /** Control point implementation constructor with position and intensity.
     *
     * @param iposition The position of the control point
     *
     * @param iintensity The amplitude of the control point
     */
    ControlPoint(const Ultrahaptics::Vector3 &iposition, const float iintensity)
        : Ultrahaptics::ControlPointBase(iposition, iintensity)
    {
    }

    /** Control point implementation constructor with position, direction and intensity.
     *
     * @param iposition The position of the control point
     * @param idirection The direction of the control point
     * @param iintensity The amplitude of the control point
     */
    ControlPoint(const Ultrahaptics::Vector3 &iposition,
                const Ultrahaptics::Vector3 &idirection,
                const float iintensity)
        : Ultrahaptics::ControlPointBase(iposition, idirection, iintensity)
    {
    }

    /** @deprecated The phase angle has never actually been used. This function will be removed in a future release. */
    ControlPoint(const float x, const float y, const float z, const float iintensity, const float iphase_angle)
        : Ultrahaptics::ControlPointBase(x, y, z, iintensity, iphase_angle)
    {
    }

    /** @deprecated The phase angle has never actually been used. This function will be removed in a future release. */
    ControlPoint(const Ultrahaptics::Vector3 &iposition, const float iintensity, const float iphase_angle)
        : Ultrahaptics::ControlPointBase(iposition, iintensity, iphase_angle)
    {
    }
};

/** \brief A control point that can be enabled or disabled and can belong to a control point group.
 *
 * This class is the same as a ControlPoint but adds an "enabled" flag and control point group membership.
 *
 * The control point can be disabled by calling disable(). In this state it will not be emitted.
 *
 * You can set the control point group using setGroup(). See \ref Introduction.md for more
 * information about control point groups, and why they need to be specified here.
 */
class ControlPointPersistent : public Ultrahaptics::ControlPointPersistentBase {
    friend class Emitter;

public:
    /** Control point copy assignment operator. */
    inline ControlPointPersistent &operator=(const ControlPoint &rhs)
    {
        Ultrahaptics::ControlPointPersistentBase::operator=(rhs);
        enabled = true;
        return *this;
    }
    /** Disable this control point. */
    inline void disable()
    {
        enabled = false;
    }

    /** Enabled this control point. */
    inline void enable()
    {
        enabled = true;
    }

    /** @return True if this control point is enabled. */
    inline bool isEnabled() const
    {
        return enabled;
    }

    /** Set position of the raw control point.
     *
     * @param iposition The position of the raw control point.
     */
    inline void setPosition(const Ultrahaptics::Vector3 &iposition)
    {
        Ultrahaptics::ControlPointPersistentBase::setPosition(iposition);
        enabled = true;
    }

    /** Set direction of the raw control point.
     *
     * @param idirection The direction of the raw control point.
     */
    inline void setDirection(const Ultrahaptics::Vector3 &idirection)
    {
        Ultrahaptics::ControlPointPersistentBase::setDirection(idirection);
    }

    /** Set amplitude of the raw control point.
     *
     * @param iintensity The amplitude of the raw control point.
     */
    inline void setIntensity(const float iintensity)
    {
        Ultrahaptics::ControlPointPersistentBase::setIntensity(iintensity);
        enabled = true;
    }

    /** @deprecated The phase angle has never actually been used. This function will be removed in a future release. */
    inline void setPhaseAngle(const float iphase_angle)
    {
        enabled = true;
    }

    /** @deprecated The phase angle has never actually been used. This function will be removed in a future release. */
    inline void autoPhaseAngle()
    {
        enabled = true;
    }

    /** Set group ID on this control point.
     *
     * @param new_group The new group ID for this point
     */
    void setGroup(const unsigned new_group)
    {
        group_id = new_group;
    }

    /** @return Group ID of this control point. */
    unsigned getGroup() const
    {
        return group_id;
    }

    // protected:
    /** Protected default constructor */
    ControlPointPersistent()
        : Ultrahaptics::ControlPointPersistentBase(0)
        , group_id(0)
        , enabled(false)
    {
    }
    /** Control point constructor.
     *
     * @param iid The unique id of this channel.
     */
    ControlPointPersistent(const unsigned iid)
        : Ultrahaptics::ControlPointPersistentBase(iid)
        , group_id(0)
        , enabled(false)
    {
    }
    /** Copy constructor.
     *
     * @param other The other object to copy
     */
    ControlPointPersistent(const ControlPointPersistent &other)
        : Ultrahaptics::ControlPointPersistentBase(other)
        , group_id(other.group_id)
        , enabled(other.enabled)
    {
    }
    /** Disabled copy assignment operator. */
    ControlPointPersistent &operator=(const ControlPointPersistent &other)
    {
        if (this != &other) {
            Ultrahaptics::ControlPointPersistentBase::operator=(other);
            group_id = other.group_id;
            enabled = other.enabled;
        }
        return *this;
    }

private:
    /** Grouping number */
    unsigned group_id = 0;
    /** Is the control point channel enabled? */
    bool enabled = false;
};

class OutputInterval;

/** \brief A point in time on which control points can be set
 *
 * Class that describes a point in time on which control
 * points can be set.
 */
class UH_API_CLASS_DECORATION TimePointOnOutputInterval : public Ultrahaptics::HostTimePoint {
    friend class OutputIntervalIterator;

public:
    /** Get a reference to a persistent control point at a given index
     *
     * @param idx The index of the control point to get
     *
     * @return A reference to the persistent control point at the given index
     */
    UH_API_DECORATION ControlPointPersistent &persistentControlPoint(const unsigned idx);
    /** Get a constant reference to a persistent control point at a given index
     *
     * @param idx The index of the control point to get
     *
     * @return A constant reference to the persistent control point at the given index
     */
    UH_API_DECORATION const ControlPointPersistent &persistentControlPoint(const unsigned idx) const;

protected:
    /** Protected constructor for output interval of time point.
     *
     * @param itime The time which this timepoint corresponds to
     *
     * @param iinterval The interval that this time point refers back to.
     */
    TimePointOnOutputInterval(const Ultrahaptics::HostTimePoint &itime, OutputInterval &iinterval);
    /** Reference to interval */
    OutputInterval &interval;
};

/** \brief Iterator for the output interval
 *
 * Class that describes a point in time on which control
 * points can be set, which is also an iterator for the output interval.
 */
class UH_API_CLASS_DECORATION OutputIntervalIterator {
    friend class OutputInterval;

public:
    /** Constant reference to the time point on the output interval in question. */
    typedef const TimePointOnOutputInterval &const_reference;
    /** Constant pointer to the time point on the output interval in question. */
    typedef const TimePointOnOutputInterval *const_pointer;
    /** Reference to the time point on the output interval in question. */
    typedef TimePointOnOutputInterval &reference;
    /** Pointer to the time point on the output interval in question. */
    typedef TimePointOnOutputInterval *pointer;
    /** Operator overload for pre-increment
     *
     * @return This with sample interval increment.
     */
    UH_API_DECORATION OutputIntervalIterator &operator++();
    /** Operator overload for less than compare
     *
     * @param other The other iterator to compare this to.
     */
    UH_API_DECORATION bool operator<(const OutputIntervalIterator &other) const;
    /** Operator overload for unequal comparison
     *
     * @param other The other iterator to compare this to.
     */
    UH_API_DECORATION bool operator!=(const OutputIntervalIterator &other) const;
    /** Operator overload of dereference
     *
     * @return Reference to the dereferenced time point that is held by this iterator.
     */
    UH_API_DECORATION reference operator*();
    /** Operator overload of pointer member access.
     *
     * @return Pointer to time point that is held by this iterator.
     */
    UH_API_DECORATION pointer operator->();
    /** Operator overload of constant dereference
     *
     * @return Reference to the dereferenced time point that is held by this iterator.
     */
    UH_API_DECORATION const_reference operator*() const;
    /** Operator overload of constant pointer member access.
     *
     * @return Pointer to time point that is held by this iterator.
     */
    UH_API_DECORATION const_pointer operator->() const;
    /** Get the iterator time step.
     *
     * @return The duration corresponding to the iterator time step.
     */
    UH_API_DECORATION const Ultrahaptics::HostDuration &timeStep() const;
    /** Get the iterator time.
     *
     * @return The time point corresponding to the iterator.
     */
    UH_API_DECORATION const Ultrahaptics::HostTimePoint &time() const;

protected:
    /** Protected constructor. */
    OutputIntervalIterator(const Ultrahaptics::HostTimePoint &itime, OutputInterval &iinterval);
    /** Protected time point corresponding to the current time. */
    TimePointOnOutputInterval current_time_point;
    /** Reference to interval */
    OutputInterval &interval;
};

class OutputIntervalImplementation;

/** Class that describes and escapsulates an output interval in time.
     *
     * \brief Representation of an output time interval */
class UH_API_CLASS_DECORATION OutputInterval {
    friend class OutputIntervalIterator;
    friend class TimePointOnOutputInterval;

public:
    /** Public typedef for iterator */
    typedef OutputIntervalIterator iterator;
    /** Get the start of the interval as a timepoint.
     *
     * @return Time point pointing to the beginning of the interval.
     */
    UH_API_DECORATION virtual const Ultrahaptics::HostTimePoint &interval_begin() const = 0;
    /** Get a time point pointing to the end of the interval.
     *
     * @return Time point pointing to the end of the interval.
     */
    UH_API_DECORATION virtual const Ultrahaptics::HostTimePoint &interval_end() const = 0;
    /** Get a time point pointing to the first sample of the interval.
     *
     * @return Time point pointing to the first sample of the interval.
     */
    UH_API_DECORATION virtual const Ultrahaptics::HostTimePoint &first_sample() const = 0;
    /** Get the first output position of the interval,
     * according to the sampling rate.
     *
     * @return Iterator pointing to the first output position.
     */
    UH_API_DECORATION iterator begin();
    /** Get an iterator that points to a time point after the
     * last valid output time.
     *
     * @return Iterator pointing after the last valid output time.
     */
    UH_API_DECORATION iterator end();
    /** Get the iterator time step.
     *
     * @return The duration corresponding to the iterator time step.
     */
    UH_API_DECORATION virtual const Ultrahaptics::HostDuration &iteratorTimeStep() const = 0;

protected:
    /** Protected constructor. */
    OutputInterval() = default;
    /** Copy constructor.
     *
     * @param other Other interval to copy.
     */
    OutputInterval(const OutputInterval &other) = delete;
    /** Copy assignment operator.
     *
     * @param other Other interval to copy and assign.
     *
     * @return This which is now a copy of the other class.
     */
    OutputInterval &operator=(const OutputInterval &other) = delete;
    /** Destructor. */
    virtual ~OutputInterval() = default;

private:
    virtual ControlPointPersistent &persistentControlPoint(const unsigned idx) = 0;
    virtual const ControlPointPersistent &persistentControlPoint(const unsigned idx) const = 0;
    virtual bool commit(const HostTimePoint &icurrent_time) = 0;
};

/** \cond EXCLUDE_FROM_DOCS */
/** Forward declaration of implementation class */
class EmitterImplementation;
/** Forward declaration of emitter class */
class Emitter;
/** \endcond */

/** Callback function for a TimePointStreaming Emitter.
 * @param timepoint_emitter The TimePointStreaming::Emitter this callback was called on.
 * @param interval The time interval that control point updates are being requested for.
 * @param submission_deadline The time by which the callback must complete to ensure continuous output.
 * @param user_pointer A user data pointer for storing arbitrary information.
 */
typedef void (*TimePointEmissionCallback)(
        const Ultrahaptics::TimePointStreaming::Emitter &timepoint_emitter,
        OutputInterval &interval,
        const Ultrahaptics::HostTimePoint &submission_deadline,
        void *user_pointer);

/** \brief A timepoint streaming emitter to which control points can be sent.
 *
 * Class defining a timepoint streaming emitter.
 */
class UH_API_CLASS_DECORATION Emitter {
public:
    /** Constructor for the timepoint emitter.
     */
    UH_API_DECORATION Emitter();
    /** Constructor for the emitter with a specific device.
     *
     * See [Board Identification](BoardIdentification.md).
     *
     * @param idevice_identifier `model:serial` or just `model`. E.g. "Dragonfly:0123".
     */
    UH_API_DECORATION Emitter(const char *idevice_identifier);
    /** Construct a new emitter with transform.
     *
     * If it has not been done already this will initialise the %Ultrahaptics library and try
     * to connect to the specified device.
     *
     * See [Board Identification](BoardIdentification.md).
     *
     * @param idevice_identifier `model:serial` or just `model`. E.g. "Dragonfly:0123".
     * @param transform          affine transform from global space to device space.
     */
    UH_API_DECORATION Emitter(const char *idevice_identifier,
                              Ultrahaptics::Transform transform)
    {
        init(idevice_identifier, transform);
        VersionInfo::checkVersion();
    }
    UH_API_DECORATION Emitter(Emitter &&other);

    /** Deleted copy constructor. */
    Emitter(const Emitter &other) = delete;

    /** Deleted copy assignment operator. */
    Emitter &operator=(const Emitter &other) = delete;

    /** Destructor for the timepoint emitter. */
    UH_API_DECORATION virtual ~Emitter();

    /** Add a device with its transform.
     *
     * Adds a device identified by \c device_identifier to this \c Emitter. \c transform is an
     * affine transformation matrix describing the translation and rotation of the emitter in the
     * frame of this \c Emitter.
     * @param device_identifier `model:serial` or just `model`. E.g. "Dragonfly:0123".
     * @param transform the affine transformation matrix
     */
    UH_API_DECORATION bool addDevice(const char *device_identifier,
                                     Ultrahaptics::Transform transform = Ultrahaptics::Transform());

    /** Set device transform.
     *
     * Sets the affine transformation matrix for the device identified by \c device_identifier.
     *
     * @param device_identifier `model:serial` or just `model`. E.g. "Dragonfly:0123".
     * @param transform the affine transformation matrix
     */
    UH_API_DECORATION bool setDeviceTransform(const char *device_identifier,
                                              Ultrahaptics::Transform transform);

    /** Get device transform.
     *
     * Gets the affine transformation matrix for the device identified by \c device_identifier.
     *
     * @param device_identifier `model:serial` or just `model`. E.g. "Dragonfly:0123".
     * @return the affine transformation matrix
     */
    UH_API_DECORATION Ultrahaptics::Transform getDeviceTransform(const char *device_identifier) const;

    /** Set device selection mode.
     *
     * @see Ultrahaptics::DeviceSelectionMode
     * @param mode The selection mode this emitter should use.
     * @return \c true if succeeded.
     */
    UH_API_DECORATION bool setDeviceSelectionMode(Ultrahaptics::DeviceSelectionMode mode);

    /** Get device selection mode.
     *
     * @see Ultrahaptics::DeviceSelectionMode
     * @return the selection mode the emitter is using.
     */
    UH_API_DECORATION Ultrahaptics::DeviceSelectionMode getDeviceSelectionMode() const;

    /** Remove a device.
     *
     * Remove the device identified by \c device_identifier from this \c Emitter.
     * @param device_identifier `model:serial` or just `model`. E.g. "Dragonfly:0123".
     */
    UH_API_DECORATION bool removeDevice(const char *device_identifier);

    /** Set emission callback.
     *
     * @param callback_function The callback function for updating the timepoint emissions.
     * If the array sample rate is 16000Hz and the emission callback requests data for 8 intervals,
     * then you'd expect the callback to be called every 500 microseconds.
     *
     * @param set_user_pointer Set user pointer for the user of the callback to access their data from inside the callback function.
     *
     * @return True if the callback was set successfully, false if an error occurred and it was not set.
     */
    UH_API_DECORATION bool setEmissionCallback(
        TimePointEmissionCallback callback_function,
        void *set_user_pointer);
    /** Set the maximum number of control points that will be used. This will
     * dynamically select a suitable sample rate for you.
     *
     * @param maximum_number_of_control_points Maximum number of control points
     * that it is expected you will use.
     *
     * @return New sample rate in Hz.
     */
    UH_API_DECORATION unsigned setMaximumControlPointCount(const unsigned maximum_number_of_control_points);
    /** @return The maximum number of control points that can be used. */
    UH_API_DECORATION unsigned getMaximumControlPointCount() const;
    /** @return The duration of the sampling interval between device updates, i.e. the duration of one sample or the time between control point updates */
    UH_API_DECORATION const Ultrahaptics::HostDuration &getSamplingInterval() const;
    /** @return The time point that the last update to the device was committed to. */
    UH_API_DECORATION const Ultrahaptics::HostTimePoint &getLastSampleTime() const;
    /** Start the callback emitter.
     *
     * @return True if the emitter starts at this point, false when the start doesn't work.
     */
    UH_API_DECORATION bool start();
    /** Stop the callback emitter.
     *
     * @return True if the emitter stops at this point, false when the stop doesn't work.
     */
    UH_API_DECORATION bool stop();
    /** @deprecated Not required; all relevant functionality is performed by constructor and Emitter::isConnected
     *
     * We do not recommend calling initialize directly and it should not be necessary to call it yourself. When an emitter is constructed, initialize will be called automatically if an %Ultrahaptics device is connected.
     *
     * If you need to check for device connection after emitter construction, we recommend using Emitter::isConnected, which will check for the presence of an %Ultrahaptics device, connect to it if it is present, and then will call initialize for you once it has successfully connected to an %Ultrahaptics device.
     */
    UH_API_DECORATION virtual void initialize();
    /** Checks for the presence of a suitable %Ultrahaptics device.
     *
     * If one is found, will connect to it and initialize it.
     *
     * @return True if a suitable device is connected after checks
     */
    UH_API_DECORATION bool isConnected();
    /** @deprecated Use DeviceInfo::arrayWidth instead.
     * @return Width of the device.
     */
    UH_API_DECORATION float arrayWidth() const;
    /** @deprecated Use DeviceInfo::arrayDepth instead.
     * @return Depth of the device.
     */
    UH_API_DECORATION float arrayDepth() const;
    /** @return Addressable width. */
    UH_API_DECORATION float interactionSpaceWidth() const;
    /** @return Addressable height. */
    UH_API_DECORATION float interactionSpaceHeight() const;
    /** @return Addressable depth. */
    UH_API_DECORATION float interactionSpaceDepth() const;

    /** @return The maximum number of control points the connected device supports emitting, or 0 if no device is connected. */
    UH_API_DECORATION unsigned int getMaximumControlPointLimit() const;

    /** Sets the rate at which control points are sent to the emitter's device.
     *
     * Please note that available values for this setting may depend on the number of control points the emitter is configured for.
     * Calling setMaximumControlPointCount will also cause the sample rate to be reset to its default for that control point count.
     *
     * Please note that changing this value will not affect how often the emission callback is called.
     * Also note that setting this value too low may result in degradation in the haptic effects generated.
     *
     * If the currently connected device does not support operating at the requested rate, the rate will be unchanged
     * (and thus the return value of this method will be the previous rate, not the requested one).
     *
     * If the currently connected device does not support this operation at all, the method will return 0.
     *
     * @param rate_hz the update rate for the device to operate at, in Hz
     *
     * @return The update rate of the device after attempting to set the rate, or 0 if the device does not support this operation or is not connected.
     */
    UH_API_DECORATION unsigned int setSampleRate(unsigned int rate_hz);

    /** @return The update rate in Hz currently used by this emitter to send control points to the device, or 0 if no device is connected. */
    UH_API_DECORATION unsigned int getSampleRate() const;

    /** Gets the maximum update rate supported by this emitter's device in Hz, or 0 if no device is connected.
     *
     * Please note that this limit may be affected by the number of control points this emitter is configured to output.
     *
     * @return The current update rate limit
     */
    UH_API_DECORATION unsigned int getSampleRateLimit() const;

    /** Extensions interface to set extended attributes on the emitter.
     *
     * @param configuration_option The configuration option to set
     *
     * @param configuration_value The configuration setting value
     *
     * @return True if this was recognised as a valid configuration option and setting it succeeded
     */
    UH_API_DECORATION bool setExtendedOption(const char *configuration_option, const char *configuration_value);
    /** Extensions interface to get extended attributes on the emitter.
     *
     * @param configuration_option The configuration option to get
     *
     * @param buffer The buffer to place the result into
     *
     * @param length The length of the buffer
     *
     * @return The size of the result, regardless of the buffer state
     */
    UH_API_DECORATION size_t getExtendedOption(const char *configuration_option, char *buffer, const size_t length) const;
    /** Resets the device this emitter is using.
     *
     * @warning Do not attempt to reconnect to a device which has been reset for two seconds.
     * Attempting to reconnect too quickly may result in the device being incorrectly detected.
     */
    UH_API_DECORATION bool reset();

    /** @return An object containing information about the device backing this emitter, if it is currently connected */
    UH_API_DECORATION DeviceInfo getDeviceInfo() const;
    /** \overload
     * @param device_identifier `model:serial` or just `model`. E.g. "Dragonfly:0123"
     */
    UH_API_DECORATION DeviceInfo getDeviceInfo(const std::string &device_identifier) const;

    /** \cond EXCLUDE_FROM_DOCS */
    /** @return The internal device object backing this emitter, if it is currently connected */
    UH_API_DECORATION const DriverDevice *getDriverDevice() const;
    /** \overload
     * @param device_identifier `model:serial` or just `model`. E.g. "Dragonfly:0123"
     */
    UH_API_DECORATION const DriverDevice *getDriverDevice(const std::string &device_identifier) const;

    /** @return The internal device object backing this emitter, if it is currently connected */
    UH_API_DECORATION DriverDevice *getDriverDevice();
    /** \overload
     * @param device_identifier `model:serial` or just `model`. E.g. "Dragonfly:0123"
     */
    UH_API_DECORATION DriverDevice *getDriverDevice(const std::string &device_identifier);

    /** @return The internal emitter object backing this emitter, if it is currently connected */
    UH_API_DECORATION void *getStateEmitter();
    /** \overload
     * @param device_identifier `model:serial` or just `model`. E.g. "Dragonfly:0123"
     */
    UH_API_DECORATION void *getStateEmitter(const std::string &device_identifier);
    /** \endcond */
private:
    /** \cond EXCLUDE_FROM_DOCS */
    /** Internal implementation pointer */
    EmitterImplementation *emitter_impl;
    UH_API_DECORATION void init(const char*, Transform);
    /** \endcond */
};
}
}
