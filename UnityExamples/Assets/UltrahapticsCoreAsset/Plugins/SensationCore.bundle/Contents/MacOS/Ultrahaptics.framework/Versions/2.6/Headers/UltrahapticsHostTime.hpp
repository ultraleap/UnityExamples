#pragma once

#include <cmath>
#include <stdint.h>
#include "UltrahapticsDecoration.hpp"

// Internally we have some concenience functions that convert HostDuration
// and HostTimePoint to/from std::chrono types. These are not exposed when
// importing the library from another program because STL types don't have
// a stable ABI.
#include <chrono>

namespace Ultrahaptics
{
  class HostTimePoint;

  /** \brief A period of time. The difference between two HostTimePoint%s.
   * 
   * Represents a duration of time as A + B/2^32 seconds where A is an int64 and B is a uint32.
   * 
   * Use seconds() to convert the fixed point representation to floating point seconds.
   */
  class UH_API_CLASS_DECORATION HostDuration
  {
    friend class HostTimePoint;
    public:
      /** Constructor for Duration. */
      UH_API_DECORATION HostDuration();
      
      /** Constructor for Duration. A Duration is the difference
       * between two points in time, and is always positive.
       *
       * @param iseconds Seconds in duration
       *
       * @param ifractional Fractional part of the duration in seconds as a double precision floating point value */
      UH_API_DECORATION HostDuration(const int64_t iseconds, const double ifractional);
      
      /** Constructor for Duration. A Duration is the difference
       * between two points in time, and is always positive.
       *
       * @param iseconds Seconds in duration
       *
       * @param ifractional Fractional part of the duration in seconds as a 32-bit unsigned value */
      UH_API_DECORATION HostDuration(const int64_t iseconds, const int64_t ifractional);
      
      /** Constructor for Duration. A Duration is the difference
       * between two points in time, and is always positive.
       *
       * @param iseconds Seconds as a double precision floating-point value */
      UH_API_DECORATION HostDuration(const double iseconds);

      /** Constructor from std::chrono::duration
       *
       * @param itime Duration in std::chrono::duration */
      template <typename Rep_, typename Period_>
      inline HostDuration(const std::chrono::duration<Rep_, Period_> &itime)
      {
        const std::chrono::seconds iseconds =
          std::chrono::duration_cast<std::chrono::seconds>(itime);
        const std::chrono::nanoseconds inanoseconds =
          std::chrono::duration_cast<std::chrono::nanoseconds>(itime - iseconds);
        *this = HostDuration(iseconds.count(), ((double)inanoseconds.count()) / ((double)1e9));
      }
      
      /** Conversion to std::chrono::duration type */
      template <typename Rep_, typename Period_>
      inline operator std::chrono::duration<Rep_, Period_>() const
      {
        return std::chrono::duration_cast<std::chrono::duration<Rep_, Period_> >(
          std::chrono::seconds(sec) +
            std::chrono::nanoseconds((size_t)std::round((((double)fractional) / exp2(32)) * 1000000000.)));
      }

      /** Conversion to double-precision float of seconds -
       * should be adequate precision for these uses. */
      UH_API_DECORATION double seconds() const;
      
      /** Overload of addition operator.
       *
       * @param rhs The right hand side duration in the addition
       *
       * @return The sum of the two durations */
      UH_API_DECORATION HostDuration operator+(const HostDuration &rhs) const;
      
      /** Overload of add-accumulate operator.
       *
       * @param rhs The right hand side duration to add to this
       *
       * @return The updated sum */
      UH_API_DECORATION HostDuration &operator+=(const HostDuration &rhs);
      
      /** Overload of subtraction operator.
       *
       * @param rhs The right hand side duration in the addition
       *
       * @return The sum of the two durations */
      UH_API_DECORATION HostDuration operator-(const HostDuration &rhs) const;
      
      /** Overload of subtract-accumulate operator.
       *
       * @param rhs The right hand side duration to subtract from this
       *
       * @return The updated subtraction */
      UH_API_DECORATION HostDuration &operator-=(const HostDuration &rhs);
      
      /** Overload of multiply accumulate.
       *
       * @param rhs The scaling factor for the duration
       *
       * @return The new scaled duration */
      UH_API_DECORATION HostDuration &operator*=(const uint32_t rhs);
      
      /** Overload of multiplication.
       *
       * @param rhs The scaling factor for the duration
       *
       * @return The new scaled duration */
      UH_API_DECORATION HostDuration operator*(const uint32_t rhs) const;
      
      /** Overload of divide accumulate.
       *
       * @param rhs The divisor factor for the duration
       *
       * @return The new divided duration */
      UH_API_DECORATION HostDuration &operator/=(const uint32_t rhs);
      
      /** Overload of division.
       *
       * @param rhs The divisor factor for the duration
       *
       * @return The new divided duration */
      UH_API_DECORATION HostDuration operator/(const uint32_t rhs) const;
      
      /** Overload of less than operator for duration comparisons.
       *
       * @param rhs The right hand of the comparison
       *
       * @return True if this value is less than the other */
      UH_API_DECORATION bool operator<(const HostDuration &rhs) const;
      
      /** Overload of less than or equal to operator for duration comparisons.
       *
       * @param rhs The right hand of the comparison
       *
       * @return True if this value is less than or equal to the other */
      UH_API_DECORATION bool operator<=(const HostDuration &rhs) const;
      
      /** Overload of greater than operator for duration comparisons.
       *
       * @param rhs The right hand of the comparison
       *
       * @return True if this value is greater than the other */
      UH_API_DECORATION bool operator>(const HostDuration &rhs) const;
      
      /** Overload of greater than or equal to operator for duration comparisons.
       *
       * @param rhs The right hand of the comparison
       *
       * @return True if this value is greater than or equal to the other */
      UH_API_DECORATION bool operator>=(const HostDuration &rhs) const;
      
      /** Overload of equal to operator for duration comparisons.
       *
       * @param rhs The right hand of the comparison
       *
       * @return True if this value is equal to the other */
      UH_API_DECORATION bool operator==(const HostDuration &rhs) const;
      
      /** Overload of not equal to operator for duration comparisons.
       *
       * @param rhs The right hand of the comparison
       *
       * @return True if this value is not equal to the other */
      UH_API_DECORATION bool operator!=(const HostDuration &rhs) const;
      

      /** Construction from std::chrono::steady_clock::duration
       *
       * @param iduration The duration to construct the HostDuration from. */
      HostDuration(const std::chrono::steady_clock::duration &iduration)
      {
          const std::chrono::seconds iseconds = std::chrono::duration_cast<std::chrono::seconds>(iduration);
          const std::chrono::nanoseconds inanoseconds = std::chrono::duration_cast<std::chrono::nanoseconds>(iduration - iseconds);
          *this = HostDuration(iseconds.count(), ((double)inanoseconds.count()) / ((double)1e9));
      }
      
      /** Conversion to std::chrono::steady_clock::duration */
      operator std::chrono::steady_clock::duration() const
      {
          return std::chrono::seconds(sec) + std::chrono::nanoseconds((size_t)std::round((((double)fractional) / std::exp2(32)) * 1000000000.));
      }

    private:
      /** This is the number of seconds in the duration. */
      int64_t sec;
      
      /** Fractional part of the number of seconds. All bits are used, for example
       * 0x80000000 is 0.5 seconds. */
      uint32_t fractional;
  };

  /** \brief A point in time.
   * 
   * Represents a point in time as A + B/2^32 seconds where A is an int64 and B is a uint32.
   *
   * Use seconds() to conver the fixed point representation to floating point seconds. Use now()
   * to get the current HostPointTime. Internally now() uses std::chrono::steady_clock
   * which is not related to wall time and increases monotonically (e.g. it might be the time since the
   * last reboot).
   */
  class UH_API_CLASS_DECORATION HostTimePoint
  {
    public:
      /** Create a zero-valued time point. The real time of '0' is implementation-defined. */
      UH_API_DECORATION HostTimePoint();
      
      /** Constructor for a time point.
       *
       * @param iseconds Seconds since epoch
       *
       * @param ifractional Fractional part of the seconds as a double precision floating point value */
      UH_API_DECORATION HostTimePoint(const int64_t iseconds, const double ifractional);
      
      /** Constructor for a time point.
       *
       * @param iseconds Seconds since epoch
       *
       * @param ifractional Fractional part of the seconds as a 32-bit unsigned value */
      UH_API_DECORATION HostTimePoint(const int64_t iseconds, const int64_t ifractional);
      
      /** Get the current time. Internally this uses std::chrono::steady_clock. */
      UH_API_DECORATION static HostTimePoint now();
      
      /** Get the time as a double-precision float. */
      UH_API_DECORATION double seconds() const;
      
      /** Overload of addition operator.
       *
       * @param rhs The right hand side duration in the addition
       *
       * @return The point in time with the duration added */
      UH_API_DECORATION HostTimePoint operator+(const HostDuration &rhs) const;
      
      /** Overload of add-accumulate operator.
       *
       * @param rhs The right hand side duration to add to this
       *
       * @return This point in time with the duration added */
      UH_API_DECORATION HostTimePoint &operator+=(const HostDuration &rhs);
      
      /** Overload of subtraction operator.
       *
       * @param rhs The right hand side point in time to subtract
       *
       * @return The duration between this point in time or the other point in time */
      UH_API_DECORATION HostDuration operator-(const HostTimePoint &rhs) const;
      
      /** Overload of subtraction operator.
       *
       * @param rhs The right hand side duration in the subtraction
       *
       * @return The point in time with the duration subtracted */
      UH_API_DECORATION HostTimePoint operator-(const HostDuration &rhs) const;
      
      /** Overload of subtract-accumulate operator.
       *
       * @param rhs The right hand side duration to subtract from this
       *
       * @return This point in time with the duration subtracted */
      UH_API_DECORATION HostTimePoint &operator-=(const HostDuration &rhs);
      
      /** Overload of less than operator for timeline comparisons.
       *
       * @param rhs The right hand of the comparison
       *
       * @return True if this point in time is earlier than the other */
      UH_API_DECORATION bool operator<(const HostTimePoint &rhs) const;
      
      /** Overload of less than or equal to operator for timeline comparisons.
       *
       * @param rhs The right hand of the comparison
       *
       * @return True if this point in time occurs earlier than or at the same time as the other */
      UH_API_DECORATION bool operator<=(const HostTimePoint &rhs) const;
      
      /** Overload of greater than operator for timeline comparisons.
       *
       * @param rhs The right hand of the comparison
       *
       * @return True if this point in time comes later than the other */
      UH_API_DECORATION bool operator>(const HostTimePoint &rhs) const;
      
      /** Overload of greater than or equal to operator for timeline comparisons.
       *
       * @param rhs The right hand of the comparison
       *
       * @return True if this point in time is the same or comes later than the other */
      UH_API_DECORATION bool operator>=(const HostTimePoint &rhs) const;
      
      /** Overload of equal to operator for timeline comparisons.
       *
       * @param rhs The right hand of the comparison
       *
       * @return True if the two points in time are the same */
      UH_API_DECORATION bool operator==(const HostTimePoint &rhs) const;
      
      /** Overload of not equal to operator for timeline comparisons.
       *
       * @param rhs The right hand of the comparison
       *
       * @return True if the two points in time are not the same */
      UH_API_DECORATION bool operator!=(const HostTimePoint &rhs) const;

      /** Construction from std::chrono::steady_clock::time_point
       *
       * @param itime The time_point to construct the TimePoint from. */
      HostTimePoint(const std::chrono::steady_clock::time_point &itime)
      {
          const std::chrono::seconds iseconds = std::chrono::duration_cast<std::chrono::seconds>(itime.time_since_epoch());
          const std::chrono::nanoseconds inanoseconds = std::chrono::duration_cast<std::chrono::nanoseconds>(itime.time_since_epoch() - iseconds);
          *this = HostTimePoint(iseconds.count(), ((double)inanoseconds.count()) / ((double)1e9));
      }
      
      /** Conversion to std::chrono::steady_clock::time_point */
      operator std::chrono::steady_clock::time_point() const
      {
          return std::chrono::steady_clock::time_point() + std::chrono::seconds(sec) + std::chrono::nanoseconds((size_t)std::round((((double)fractional) / std::exp2(32)) * 1000000000.));
      }

    private:
      /** This value is the number of seconds since the '0' time. That time is
       * implementation-defined. For example it may be 1/1/1970, or it might be the time this
       * machine was turned on. */
      int64_t sec;
      
      /** Fractional part of the number of seconds. All bits are used, for example
       * 0x80000000 is 0.5 seconds. */
      uint32_t fractional;
  };
  
  typedef HostTimePoint TimePoint;
  typedef HostDuration Duration;

  UH_API_DECORATION inline HostDuration operator*(const uint32_t lhs, const HostDuration &rhs)
  {
      return rhs * lhs;
  }
}

