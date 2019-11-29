
#pragma once

#include <string.h>
#include <math.h>
#include "Ultrahaptics.hpp"

#ifndef M_PI
#define M_PI 3.141592653589793238462643383279502
#endif

namespace Ultrahaptics
{
  /** \brief Raw control point (unmodulated state).
   *
   * This represents the state of a control point at a single point in time - its
   * position and modulation intensity.
   */
  struct ControlPointBase
  {
    /** Raw control point constructor. */
    ControlPointBase()
      : ControlPointBase({0.f, 0.f, 0.f}, {0.f, 0.f, 0.f}, 0)
      {}
  
    /** Raw control point constructor with x, y, z, nx, ny, nz and intensity.
     * 
     * `nx`, `ny` and `nz` should represent a normalized vector.
     *
     * @param x The x position of the control point
     *
     * @param y The y position of the control point
     *
     * @param z The z position of the control point
     *
     * @param nx The x direction of the control point
     *
     * @param ny The y direction of the control point
     *
     * @param nz The z direction of the control point
     *
     * @param iintensity The intensity of the control point */
      ControlPointBase(const float x, const float y, const float z,
                       const float nx, const float ny, const float nz,
                       const float iintensity)
        : ControlPointBase({x, y, z}, {nx, ny, nz}, iintensity)
      {}

    /** Raw control point constructor with x, y, z and intensity.
     *
     * @param x The x position of the control point
     *
     * @param y The y position of the control point
     *
     * @param z The z position of the control point
     *
     * @param iintensity The intensity of the control point
     */
    ControlPointBase(const float x, const float y, const float z, const float iintensity)
      : ControlPointBase({x, y, z}, iintensity)
      {}

    /** Raw control point constructor with position and intensity.
     *
     * @param iposition The position of the control point
     *
     * @param iintensity The intensity of the control point
     */
      ControlPointBase(const Ultrahaptics::Vector3 &iposition,
                       const float iintensity)
          : ControlPointBase(iposition, {0.f, 0.f, 0.f}, iintensity)
      {}

    /** Raw control point constructor with position, direction and intensity.
     *
     * @param iposition The position of the control point
     *
     * @param idirection The direction unit vector of the control point
     *
     * @param iintensity The intensity of the control point
     */
      ControlPointBase(const Ultrahaptics::Vector3 &iposition,
                       const Ultrahaptics::Vector3 &idirection,
                       const float iintensity)
      : phase_angle(-1.f)
      {
          setPosition(iposition);
          setDirection(idirection);
          setIntensity(iintensity);
      }

    /** @deprecated The phase angle has never actually been used. This function will be removed in a future release. */
    ControlPointBase(const float x, const float y, const float z, const float iintensity, const float iphase_angle)
      : ControlPointBase({x, y, z}, {0.f, 0.f, 0.f}, iintensity)
      {}

      /** @deprecated The phase angle has never actually been used. This function will be removed in a future release. */
    ControlPointBase(const Ultrahaptics::Vector3 &iposition, const float iintensity, const float iphase_angle)
      : ControlPointBase(iposition, {0.f, 0.f, 0.f}, iintensity)
      {}

    /** @return Position of the control point. */
    Ultrahaptics::Vector3 getPosition() const
    {
      return position;
    }

    /** Set position of the raw control point.
     *
     * @param iposition The position of the raw control point. */
    void setPosition(const Ultrahaptics::Vector3 &iposition)
    {
      position = iposition;
    }

    /** @return Control point direction. */
    UH_API_DECORATION Ultrahaptics::Vector3 getDirection() const
    {
        return direction;
    }

    /** Set direction of the raw control point.
     *
     * @param idirection The direction of the raw control point. */
    void setDirection(const Ultrahaptics::Vector3 &idirection)
    {
      direction = idirection;
    }

    /** @return Intensity of the control point. */
    float getIntensity() const
    {
      return intensity;
    }

    /** Set intensity of the raw control point.
     *
     * @param iintensity The intensity of the raw control point. */
    void setIntensity(const float iintensity)
    {
      intensity = fmin(fmax(-1.0f, iintensity), 1.0f);
    }

    /** @deprecated The phase angle has never actually been used. This function will be removed in a future release. */
    void setPhaseAngle(const float /*iphase_angle*/)
    {
    }

    /** @deprecated The phase angle has never actually been used. This function will be removed in a future release. */
    void autoPhaseAngle()
    {
    }

    /** @deprecated The phase angle has never actually been used. This function will be removed in a future release. */
    float getPhaseAngle() const
    {
      return -1.0f;
    }

    /** Position of the raw control point */
    Ultrahaptics::Vector3 position;
    /** Normalized direction of the raw control point */
    Ultrahaptics::Vector3 direction;
    /** Intensity of the raw control point */
    float intensity;
    /** @deprecated The phase angle has never actually been used. This function will be removed in a future release. */
    float phase_angle;
  };

  /** \brief A ControlPointBase that has a persistent ID.
   */
  class ControlPointPersistentBase : public ControlPointBase
  {
    public:
      ControlPointPersistentBase(const ControlPointBase &rhs)
      {
        *this = rhs;
      }

      /** Control point copy assignment operator. */
      inline ControlPointPersistentBase &operator=(const ControlPointBase &rhs)
      {
        if (this != &rhs)
        {
          ControlPointBase::operator=(rhs);
        }
        return *this;
      }
    
      /** Raw control point constructor.
       *
       * @param iid The ID for this basic persistent control point */
      ControlPointPersistentBase(const unsigned iid)
        : ControlPointBase(), id(iid)
        {}

      /** Raw control point constructor.
       *
       * @param iid The ID for this basic persistent control point
       *
       * @param cp_base A basic control point to copy. */
      ControlPointPersistentBase(const unsigned iid, const ControlPointBase &cp_base)
        : ControlPointBase(cp_base), id(iid)
        {}
    
      /** Raw control point constructor with x, y, z and intensity.
       *
       * @param x The x position of the control point
       *
       * @param y The y position of the control point
       *
       * @param z The z position of the control point
       *
       * @param iintensity The intensity of the control point
       *
       * @param iid The ID for this basic persistent control point */
      ControlPointPersistentBase(const float x, const float y, const float z, const float iintensity, const unsigned iid)
        : ControlPointBase(x, y, z, iintensity), id(iid)
        {}

      /** Control point implementation constructor with position, intensity and modulation frequency.
       *
       * @param iposition The position of the control point
       *
       * @param iintensity The intensity of the control point
       *
       * @param iid The ID for this basic persistent control point */
      ControlPointPersistentBase(const Ultrahaptics::Vector3 &iposition, const float iintensity, const unsigned iid)
        : ControlPointBase(iposition, iintensity), id(iid)
        {}

      /** @deprecated The phase angle has never actually been used. This function will be removed in a future release. */
      ControlPointPersistentBase(const float x, const float y, const float z, const float iintensity, const float iphase_angle, const unsigned iid)
        : ControlPointBase(x, y, z, iintensity, iphase_angle), id(iid)
        {}

      /** @deprecated The phase angle has never actually been used. This function will be removed in a future release. */
      ControlPointPersistentBase(const Ultrahaptics::Vector3 &iposition, const float iintensity, const float iphase_angle, const unsigned iid)
        : ControlPointBase(iposition, iintensity, iphase_angle), id(iid)
        {}
    
      /** Get ID of control point */
      unsigned getID() const { return id; }
    
    protected:
      /** ID value for persistence */
      unsigned id = 0;
  };
}
