#pragma once

#ifndef M_PI
#define M_PI 3.141592653589793238462643383279502
#endif

#include "UltrahapticsQuaternion.hpp"
#include "UltrahapticsVector3.hpp"

#include <cmath>
#include <cstring>

namespace Ultrahaptics {
/** \brief A 3x3 transformation matrix. */
struct Matrix3x3 {
    /** Create a zero matrix
      *
      * @return A zero matrix
      */
    static Matrix3x3 zero()
    {
        return Matrix3x3();
    }

    /** Create an identity matrix
      *
      * @return An identity matrix
      */
    static Matrix3x3 identity()
    {
        Matrix3x3 m;

        m.element[0] = 1.0;
        m.element[1] = 0.0;
        m.element[2] = 0.0;

        m.element[3] = 0.0;
        m.element[4] = 1.0;
        m.element[5] = 0.0;

        m.element[6] = 0.0;
        m.element[7] = 0.0;
        m.element[8] = 1.0;

        return m;
    }

    /** Create a new scaling matrix
     *
     * @param sx Scaling factor to apply in the x-axis direction
     *
     * @param sy Scaling factor to apply in the y-axis direction
     *
     * @param sz Scaling factor to apply in the z-axis direction
     *
     * @return New matrix for scaling
     */
    static Matrix3x3 scale(double sx, double sy, double sz)
    {
        return {
            sx, 0.0, 0.0,
            0.0, sy, 0.0,
            0.0, 0.0, sz
        };
    }

    /** Create a rotation matrix that rotates one vector onto another
     *
     * @param p Unit direction vector to rotate from
     *
     * @param q Unit direction vector to rotate onto
     *
     * @return Rotation matrix that rotates p onto q
     */
    static Matrix3x3 rotateOntoVector(Vector3 p, Vector3 q)
    {
        Vector3 cross_pq = p.cross(q);
        float dot_pq = p.dot(q);

        if (dot_pq <= -1.f)
            return rotate(Vector3(1.0f, 0.0f, 0.0f), M_PI);

        float rcp_1p_dot_pq = 1.f / (1.f + dot_pq);

        Matrix3x3 m;

        m.element[0] = cross_pq.x * cross_pq.x * rcp_1p_dot_pq + dot_pq;
        m.element[1] = cross_pq.x * cross_pq.y * rcp_1p_dot_pq - cross_pq.z;
        m.element[2] = cross_pq.x * cross_pq.z * rcp_1p_dot_pq + cross_pq.y;

        m.element[3] = cross_pq.y * cross_pq.x * rcp_1p_dot_pq + cross_pq.z;
        m.element[4] = cross_pq.y * cross_pq.y * rcp_1p_dot_pq + dot_pq;
        m.element[5] = cross_pq.y * cross_pq.z * rcp_1p_dot_pq - cross_pq.x;

        m.element[6] = cross_pq.z * cross_pq.x * rcp_1p_dot_pq - cross_pq.y;
        m.element[7] = cross_pq.z * cross_pq.y * rcp_1p_dot_pq + cross_pq.x;
        m.element[8] = cross_pq.z * cross_pq.z * rcp_1p_dot_pq + dot_pq;

        return m;
    }

    /** Create a new rotation matrix
     *
     * @param n Unit vector describing the axis to rotate around
     *
     * @param theta Angle through which to rotate around the axis
     *
     * @return New matrix for rotation
     */
    static Matrix3x3 rotate(Vector3 n, double theta)
    {
        double c_theta = cos(theta);

        // Return the identity matrix
        if (c_theta >= 1.)
            return identity();

        double s_theta = sin(theta);
        double one_m_c = 1. - c_theta;

        Matrix3x3 m;

        m.element[0] = n.x * n.x * one_m_c + c_theta;
        m.element[1] = n.x * n.y * one_m_c - n.z * s_theta;
        m.element[2] = n.x * n.z * one_m_c + n.y * s_theta;

        m.element[3] = n.y * n.x * one_m_c + n.z * s_theta;
        m.element[4] = n.y * n.y * one_m_c + c_theta;
        m.element[5] = n.y * n.z * one_m_c - n.x * s_theta;

        m.element[6] = n.z * n.x * one_m_c - n.y * s_theta;
        m.element[7] = n.z * n.y * one_m_c + n.x * s_theta;
        m.element[8] = n.z * n.z * one_m_c + c_theta;

        return m;
    }

    /** Default constructor */
    Matrix3x3()
        : element{}
    {
    }

    /** Element constructor */
    Matrix3x3(double a00, double a01, double a02,
              double a10, double a11, double a12,
              double a20, double a21, double a22)
        : element{a00, a01, a02,
                  a10, a11, a12,
                  a20, a21, a22}
    {
    }

    explicit Matrix3x3(const Quaternion &quat)
    {
        setRotation(quat);
    }

    void setRotation(const Quaternion &q)
    {
        double s = q.length2();
        auto wx = s * q.w() * q.x(), wy = s * q.w() * q.y(), wz = s * q.w() * q.z();
        auto xx = s * q.x() * q.x(), xy = s * q.x() * q.y(), xz = s * q.x() * q.z();
        auto yy = s * q.y() * q.y(), yz = s * q.y() * q.z(), zz = s * q.z() * q.z();

        auto &A = element; // short alias
        A[0] = 1 - (yy + zz);
        A[1] = xy - wz;
        A[2] = xz + wy;
        A[3] = xy + wz;
        A[4] = 1 - (xx + zz);
        A[5] = yz - wx;
        A[6] = xz - wy;
        A[7] = yz + wx;
        A[8] = 1 - (xx + yy);
    }

    /** Determinant.
     * @return the determinant.
     */
    double determinant() const
    {
        const auto &A = element; // short alias
        return
            A[0] * A[4] * A[8]
          + A[1] * A[5] * A[6]
          + A[2] * A[3] * A[7]
          - A[2] * A[4] * A[6]
          - A[0] * A[5] * A[7]
          - A[1] * A[3] * A[8];
    }

    /** Inverse matrix.
     * @return the inverse matrix.
     */
    Matrix3x3 inverse() const
    {
        auto const &A = element; // short alias

        auto const det = determinant();
        if (std::abs(det) < 1e-19)
            return Matrix3x3();

        Matrix3x3 m{
            (A[4] * A[8] - A[5] * A[7]),
          - (A[1] * A[8] - A[2] * A[7]),
            (A[1] * A[5] - A[2] * A[4]),

          - (A[3] * A[8] - A[5] * A[6]),
            (A[0] * A[8] - A[2] * A[6]),
          - (A[0] * A[5] - A[2] * A[3]),

            (A[3] * A[7] - A[4] * A[6]),
          - (A[0] * A[7] - A[1] * A[6]),
            (A[0] * A[4] - A[1] * A[3])
        };

        return m / det;
    }

    /** Transpose matrix.
     * @return the transpose matrix.
     */
    Matrix3x3 transpose() const
    {
        auto const &A = element; // short alias
        return {
            A[0], A[3], A[6],
            A[1], A[4], A[7],
            A[2], A[5], A[8]
        };
    }

    /** No-sign-change operator
     *
     * @return A new copy of the matrix without changing the sign of the matrix
     */
    Matrix3x3 operator+() const
    {
        return *this;
    }

    /** Addition operator
     *
     * @param lhs The left hand side of the addition operation
     * @param rhs The right hand side of the addition operation
     *
     * @return The addition of the two operands
     */
    friend Matrix3x3 operator+(Matrix3x3 lhs, const Matrix3x3 &rhs)
    {
        lhs += rhs;
        return lhs;
    }

    /** Summation operator
     *
     * @param rhs The right hand side to add to the existing matrix
     *
     * @return The summation of this with the right hand side
     */
    Matrix3x3 &operator+=(const Matrix3x3 &rhs)
    {
        element[0] += rhs.element[0];
        element[1] += rhs.element[1];
        element[2] += rhs.element[2];

        element[3] += rhs.element[3];
        element[4] += rhs.element[4];
        element[5] += rhs.element[5];

        element[6] += rhs.element[6];
        element[7] += rhs.element[7];
        element[8] += rhs.element[8];

        return *this;
    }

    /** Sign change operator, negate
     *
     * @return A new negated copy of the matrix
     */
    Matrix3x3 operator-() const
    {
        Matrix3x3 m;

        m.element[0] = -element[0];
        m.element[1] = -element[1];
        m.element[2] = -element[2];

        m.element[3] = -element[3];
        m.element[4] = -element[4];
        m.element[5] = -element[5];

        m.element[6] = -element[6];
        m.element[7] = -element[7];
        m.element[8] = -element[8];

        return m;
    }

    /** Subtract-accumulate operator
     *
     * @param rhs The right hand side to subtract from the existing matrix
     *
     * @return The subtraction with accumulation of this with the right hand side
     */
    Matrix3x3 &operator-=(const Matrix3x3 &rhs)
    {
        *this += -rhs;
        return *this;
    }

    /** Multiplication operator
     *
     * @param rhs The right hand side to multiply with the matrix
     *
     * @return The multiplication of the matrices
     */
    Matrix3x3 operator*(const Matrix3x3 &rhs) const
    {
        Matrix3x3 m;

        for (int j = 0; j < 3; j++)
            for (int i = 0; i < 3; i++)
                m.element[j * 3 + i] =
                    element[j * 3 + 0] * rhs.element[0 * 3 + i] +
                    element[j * 3 + 1] * rhs.element[1 * 3 + i] +
                    element[j * 3 + 2] * rhs.element[2 * 3 + i];

        return m;
    }

    /** Product-accumulate operator
     *
     * @param rhs The right hand side to multiply and accumulate with the matrix
     *
     * @return The product of the matrices
     */
    Matrix3x3 &operator*=(const Matrix3x3 &rhs)
    {
        Matrix3x3 m((*this) * rhs);
        *this = m;
        return *this;
    }

    /** Scale-accumulate operator
     *
     * @param scale The scaling factor to apply
     *
     * @return The existing matrix after scaling
     */
    Matrix3x3 &operator*=(double scale)
    {
        element[0] *= scale;
        element[1] *= scale;
        element[2] *= scale;

        element[3] *= scale;
        element[4] *= scale;
        element[5] *= scale;

        element[6] *= scale;
        element[7] *= scale;
        element[8] *= scale;

        return *this;
    }

    /** Scale operator
     * @param scale The scaling factor
     * @return A new scaled matrix
     */
    Matrix3x3 operator*(double scale) const
    {
        Matrix3x3 m = *this;
        m *= scale;
        return m;
    }

    /** Inverse Scale operator
     * @param scale The inverse scaling factor
     * @return A new scaled matrix
     */
    Matrix3x3 operator/(double scale) const
    {
        Matrix3x3 m = *this;
        m *= 1. / scale;
        return m;
    }

    /** Get matrix row
     * @param r Index of the row
     * @return A copy of the requested row
     */
    Vector3 getRow(int r) const
    {
        return {
            static_cast<float>(element[3 * r + 0]),
            static_cast<float>(element[3 * r + 1]),
            static_cast<float>(element[3 * r + 2])};
    }

    /** Get matrix column
     * @param c Index of the column
     * @return A copy of the requested column
     */
    Vector3 getCol(int c) const
    {
        return {
            static_cast<float>(element[3 * 0 + c]),
            static_cast<float>(element[3 * 1 + c]),
            static_cast<float>(element[3 * 2 + c])};
    }

    /** Get matrix row
     * @param r Index of the row
     * @return A copy of the requested row
     */
    Vector3 operator[](int r) const
    {
        return getRow(r);
    }

    /** Get matrix element
     * @param r Index of the row
     * @param c Index of the col
     * @return Element at index
     */
    double at(size_t r, size_t c) const
    {
        return element[r * 3 + c];
    }

    /** Get matrix element
     * @param r Index of the row
     * @param c Index of the col
     * @return Element at index
     */
    double &at(size_t r, size_t c)
    {
        return element[r * 3 + c];
    }

    /** Elements of the matrix, stored in row major format.
      * These can be freely specified as this is a struct.
      */
    double element[9];
};


/** Subtraction operator
 *
 * @param lhs The left hand side of the subtraction operation
 * @param rhs The right hand side of the subtraction operation
 *
 * @return The subtraction of the right hand side
 */
static inline Matrix3x3 operator-(Matrix3x3 lhs, const Matrix3x3 &rhs)
{
    lhs -= rhs;
    return lhs;
}

/** Scaling multiplication operator
 *
 * @param lhs The left hand side of the multiplication operation (scale factor)
 * @param rhs The right hand side of the multiplication operation
 *
 * @return The scaled matrix
 */
static inline Matrix3x3 operator*(double lhs, const Matrix3x3 &rhs)
{
    return rhs * lhs;
}

/** Subtraction operator
 *
 * @param lhs The left hand side of the multiplication operation
 * @param rhs The right hand side of the multiplication operation
 *
 * @return The multiplication of two matrices
 */
static inline Vector3 operator*(Matrix3x3 lhs, Vector3 rhs)
{
    return {
        lhs.getRow(0).dot(rhs),
        lhs.getRow(1).dot(rhs),
        lhs.getRow(2).dot(rhs)
    };
}
}

#ifndef UH_NOSTL
#include <iostream>

namespace Ultrahaptics {
inline std::ostream &operator<<(std::ostream &os, const Matrix3x3 &m)
{
    const auto &md = m.element;
    os << md[0] << " " << md[1] << " " << md[2] << '\n'
       << md[3] << " " << md[4] << " " << md[5] << '\n'
       << md[6] << " " << md[7] << " " << md[8];
    return os;
}
} // namespace Ultrahaptics
#endif // ifndef UH_NOSTL
