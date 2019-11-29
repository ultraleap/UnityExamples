
#pragma once

#include <stddef.h>
#include <math.h>

namespace Ultrahaptics
{
   /** \brief A simple vector class.
    */
   class Vector3
   {
      public:
         /** Default constructor. */
         Vector3() : x(0.0f), y(0.0f), z(0.0f) {}
         
         /** Construct a 3D vector with x, y and z components.
          *
          * @param ix The x-coordinate for the new vector
          *
          * @param iy The y-coordinate for the new vector
          *
          * @param iz The z-coordinate for the new vector */
         Vector3(float ix, float iy, float iz) : x(ix), y(iy), z(iz) {}
         
         /** Unary operation of not-negation for vectors.
          *
          * @return The same vector, as not-negation doesn't change the
          * vector */
         Vector3 operator+() const
            { return *this; }
         
         /** Binary operation of addition for vectors.
          *
          * @param rhs The right-hand part of the addition
          *
          * @return Sum of the vectors */
         Vector3 operator+(const Vector3 &rhs) const
            { return Vector3(x + rhs.x, y + rhs.y, z + rhs.z); }
         
         /** Binary sum-accumlate operation for vectors.
          *
          * @param rhs The vector to accumlate in this vector
          *
          * @return The sum this vector */
         Vector3 &operator+=(const Vector3 &rhs)
            { x += rhs.x; y += rhs.y; z += rhs.z; return *this; }
         
         /** Negation operation for vectors.
          *
          * @return The negated vector */
         Vector3 operator-() const
            { return Vector3(-x, -y, -z); }
         
         /** Binary operation of subtraction for vectors.
          *
          * @param rhs The right-hand part of the subtraction
          *
          * @return Difference of the vectors */
         Vector3 operator-(const Vector3 &rhs) const
            { return Vector3(x - rhs.x, y - rhs.y, z - rhs.z); }
         
         /** Binary subtract-accumlate operation for vectors.
          *
          * @param rhs The vector to accumlate in this vector
          *
          * @return The subtracted this vector */
         Vector3 &operator-=(const Vector3 &rhs)
            { x -= rhs.x; y -= rhs.y; z -= rhs.z; return *this; }
         
         /** Binary scaling operation for vectors.
          *
          * @param scale Scaling factor for the vector
          *
          * @return Scaled vector */
         Vector3 operator*(float scale) const
            { return Vector3(scale * x, scale * y, scale * z); }
         
         /** Binary scale-accumlate operation for vectors.
          *
          * @param scale The scale factor to apply to this vector
          *
          * @return The scaled version of this vector */
         Vector3 &operator*=(float scale)
            { x *= scale; y *= scale; z *= scale; return *this; }
         
         /** Vector scaling by a reciprocal, division by a scaling factor.
          *
          * @param denominator The division to apply to each vector component
          *
          * @return The scaled version of this vector */
         Vector3 operator/(float denominator) const
         {
            float rcpdenom = 1.0f / denominator;
            return (*this) * rcpdenom;
         }
         
         /** Vector scale-accumulate by a reciprocal, division by a scaling
          * factor.
          *
          * @param denominator The division factor to apply to each component of
          * this vector
          *
          * @return The scaled version of this vector */
         Vector3 &operator/=(float denominator)
         {
            float rcpdenom = 1.0f / denominator;
            (*this) *= rcpdenom;
            return *this;
         }
         
         /** Vector (cross) product of two vectors.
          *
          * @param rhs The right-hand vector in the cross product calculation
          *
          * @return The vector (cross) product vector */
         Vector3 cross(const Vector3 &rhs) const
            { return Vector3(y*rhs.z - z*rhs.y, z*rhs.x - x*rhs.z, x*rhs.y - y*rhs.x); }
         
         /** Scalar (dot) product of two vectors.
          *
          * @param rhs The right-hand vector in the dot-product calculation
          *
          * @return The scalar (dot) product scalar value */
         float dot(const Vector3 &rhs) const
            { return x*rhs.x + y*rhs.y + z*rhs.z; }
         
         /** @return The length of this vector */
         float length() const
            { return sqrtf(x*x + y*y + z*z); }
         
         /** @return A normalised version of this vector, does not modify the
          * zero vector. */
         Vector3 normalize() const
         {
            float tlength = sqrtf(x*x + y*y + z*z);
            float reciprocal_of_length = 1.0f / tlength;
            return (tlength > 0.0f) ? ((*this) * reciprocal_of_length) : Vector3();
         }
         
         /** The x-component of the vector */
         float x;
         /** The y-component of the vector */
         float y;
         /** The z-component of the vector */
         float z;
   };

   /** Binary scaling operation for vectors.
    *
    * @param scale Scaling factor for the vector
    *
    * @param rhs The vector to scale
    *
    * @return Scaled vector */
   inline Vector3 operator*(float scale, const Vector3 &rhs)
      { return rhs * scale; }

   /** Dot product of two vectors. */
   inline float operator*(const Vector3 &lhs, const Vector3 &rhs)
   {
      return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
   }

   /** Equality operator. */
   inline bool operator==(const Vector3 &lhs, const Vector3 &rhs)
   {
       return lhs.x == rhs.x
              && lhs.y == rhs.y
              && lhs.z == rhs.z;
   }

   /** Inequality operator. */
   inline bool operator!=(const Vector3 &lhs, const Vector3 &rhs)
   {
       return !(lhs == rhs);
   }
}

#ifndef UH_NOSTL
#include <iomanip>

namespace Ultrahaptics {
inline std::ostream &operator<<(std::ostream &os, const Vector3 &v)
{
    auto p = os.precision();
    auto w = os.width();
    auto f = os.flags();
    os.precision(5);
    os << '{' << std::fixed
       << std::setw(9) << v.x << ','
       << std::setw(9) << v.y << ','
       << std::setw(9) << v.z << '}';
    os.precision(p);
    os.width(w);
    os.setf(f);
    return os;
}
} // namespace Ultrahaptics
#endif  // ifndef UH_NOSTL
