#pragma once

#include <cmath>
#include "UltrahapticsVector3.hpp"

namespace Ultrahaptics
{
  /** A generic bounding volume of any shape. */
  class BoundingVolume
  {
  public:
    virtual ~BoundingVolume() {}
      
    /** Return true if the bounding volume contains the given point. */
    virtual bool contains(float x, float y, float z) const = 0;
      
    /** Return true if the bounding volume contains the given point. */
    bool contains(const Vector3 &v) const
    {
      return contains(v.x, v.y, v.z);
    }
  };

  /** A bounding box aligned to the axes of its coordinate space.
   * 
   * This class does not check that the minimum coordinates are less than the maximum.
   * If they are not, then the width(), height(), depth() and volume() calculations
   * may be negative.
   */
  class AxisAlignedBoundingBox : public BoundingVolume
  {
    public:
      /** Create an axis-aligned bounding box from the given positions
       *
       * @param p_min_x The minimum X coordinate of the bounding box
       * @param p_min_y The minimum Y coordinate of the bounding box
       * @param p_min_z The minimum Z coordinate of the bounding box
       * @param p_max_x The maximum X coordinate of the bounding box
       * @param p_max_y The maximum Y coordinate of the bounding box
       * @param p_max_z The maximum Z coordinate of the bounding box */
      AxisAlignedBoundingBox(float p_min_x, float p_min_y, float p_min_z, float p_max_x, float p_max_y, float p_max_z)
        : min_x(p_min_x), min_y(p_min_y), min_z(p_min_z), max_x(p_max_x), max_y(p_max_y), max_z(p_max_z)
      {
      }

      /** Create an axis-aligned bounding box from the given vectors
       *
       * @param min_v The vector representing the minimum positions of the bounding box
       * @param max_v The vector representing the maximum positions of the bounding box */
      AxisAlignedBoundingBox(const Vector3 &min_v, const Vector3 &max_v)
        : min_x(min_v.x), min_y(min_v.y), min_z(min_v.z), max_x(max_v.x), max_y(max_v.y), max_z(max_v.z)
      {
      }

      /** Create a default axis-aligned bounding box of zero size */
      AxisAlignedBoundingBox()
        : min_x(0.0f), min_y(0.0f), min_z(0.0f), max_x(0.0f), max_y(0.0f), max_z(0.0f)
      {
      }

      /** Determine whether or not the given point is contained within the bounding box
       *
       * @param x the X coordinate of the position to check
       * @param y the Y coordinate of the position to check
       * @param z the Z coordinate of the position to check
       *
       * @return True if the point is contained within this box, or false if not */
      bool contains(float x, float y, float z) const
      {
        return (x >= min_x && x <= max_x
             && y >= min_y && y <= max_y
             && z >= min_z && z <= max_z);
      }

      /** @return The width (in the X axis) of the bounding box. This may be negative. */
      inline float width()  const { return (max_x - min_x); }
      /** @return The depth (in the Y axis) of the bounding box. This may be negative. */
      inline float depth()  const { return (max_y - min_y); }
      /** @return The height (in the Z axis) of the bounding box. This may be negative. */
      inline float height() const { return (max_z - min_z); }
      /** @return The volume of the bounding box. This may be negative. */
      inline float volume() const { return (width() * depth() * height()); }


      /** The minimum X value contained by the bounding box */
      float min_x;
      /** The minimum Y value contained by the bounding box */
      float min_y;
      /** The minimum Z value contained by the bounding box */
      float min_z;
      /** The maximum X value contained by the bounding box */
      float max_x;
      /** The maximum Y value contained by the bounding box */
      float max_y;
      /** The maximum Z value contained by the bounding box */
      float max_z;
  };

  /** OrientedBoundingBox is a parallelepiped in any orientation.
   * 
   * A parallelepiped could be specified by four vectors - a corner, and vectors for the three edges
   * connected to that corner. However this class actually uses the centre instead of a corner, and
   * the edge vectors are split into normalised vectors and lengths scalars.
   */
  class OrientedBoundingBox : public BoundingVolume
  {
    public:
      /** Create an oriented bounding box from the given positions. The axis vectors
       * are normalised - their length does not matter.
       *
       * @param p_centre  the position of the centre of the box
       * @param p_axis1   the direction of the first axis of the box (width)
       * @param p_axis2   the direction of the second axis of the box (depth)
       * @param p_axis3   the direction third axis of the box (height)
       * @param p_slen1   the side length of axis 1
       * @param p_slen2   the side length of axis 2
       * @param p_slen3   the side length of axis 3 */
      OrientedBoundingBox(const Vector3 &p_centre, const Vector3 &p_axis1, const Vector3 &p_axis2, const Vector3 &p_axis3, float p_slen1, float p_slen2, float p_slen3)
        : centre(p_centre), axis1(p_axis1.normalize()), axis2(p_axis2.normalize()), axis3(p_axis3.normalize()), extent1(p_slen1/2.0f), extent2(p_slen2/2.0f), extent3(p_slen3/2.0f)
      {
      }

      /** Create a default oriented bounding box of zero size */
      OrientedBoundingBox()
        : centre(0.0f, 0.0f, 0.0f), axis1(1.0f, 0.0f, 0.0f), axis2(0.0f, 1.0f, 0.0f), axis3(0.0f, 0.0f, 1.0f), extent1(0.0f), extent2(0.0f), extent3(0.0f)
      {
      }

      /** Determine whether or not the given point is contained within the bounding box
       *
       * @param x the X coordinate of the position to check
       * @param y the Y coordinate of the position to check
       * @param z the Z coordinate of the position to check
       *
       * @return True if the point is contained within this box, or false if not */
      bool contains(float x, float y, float z) const
      {
        // This could also be done by calculating the inverse of the 3x3 matrix that transforms
        // the unit cube to this parallelepiped, multiplying [x y z] by it and checking if the
        // result is in the unit cube. However then we would need to handle degenerate cases.
        //
        // Simpler is just to project the point onto each axis.
          
        Vector3 diff(x - centre.x, y - centre.y, z - centre.z);
        
        // Distance in the direction of each principal axis.
        double dist1 = diff.dot(axis1);
        double dist2 = diff.dot(axis2);
        double dist3 = diff.dot(axis3);
        
        // The above as vectors components.
        Vector3 comp1 = dist1 * axis1;
        Vector3 comp2 = dist2 * axis2;
        Vector3 comp3 = dist3 * axis3;
        
        // Now subtract out each of the unimportant components before checking.
        if ((diff - comp2 - comp3).length() > extent1)
            return false;
        if ((diff - comp1 - comp3).length() > extent2)
            return false;
        if ((diff - comp1 - comp2).length() > extent3)
            return false;
        return true;
      }

      /** @return The width (in the first axis) of the bounding box */
      inline float width()  const { return (extent1 * 2.0f); }
      /** @return The depth (in the second axis) of the bounding box */
      inline float depth()  const { return (extent2 * 2.0f); }
      /** @return The height (in the third axis) of the bounding box */
      inline float height() const { return (extent3 * 2.0f); }
      /** @return The volume of the bounding box */
      inline float volume() const { return (width() * depth() * height() * axis1.cross(axis2).dot(axis3)); }

      Vector3 centre;
      Vector3 axis1;
      Vector3 axis2;
      Vector3 axis3;
      float extent1;
      float extent2;
      float extent3;
  };
}

