#pragma once

#include "UltrahapticsMatrix3x3.hpp"
#include "UltrahapticsQuaternion.hpp"
#include "UltrahapticsVector3.hpp"

#include <cmath>
#include <sstream>

namespace Ultrahaptics {

/** \brief A basis transformation. */
class Transform {
public:
    Transform(Matrix3x3 basis = Matrix3x3::identity(),
              Vector3 origin = {})
        : m_basis(std::move(basis))
        , m_origin(std::move(origin))
    {
    }

    Transform(const Quaternion &q, Vector3 origin = {})
        : m_basis(q)
        , m_origin(std::move(origin))
    {
    }

    /** Create a new translation transform
     *
     * @param v Translation vector
     *
     * @return New transform for translation
     */
    static Transform translation(Vector3 v)
    {
        Transform t;
        t.setOrigin(std::move(v));
        return t;
    }

    /** Create a new translation transform
     *
     * @param tx Units to translate in x-axis direction
     *
     * @param ty Units to translate in y-axis direction
     *
     * @param tz Units to translate in z-axis direction
     *
     * @return New transform for translation
     */
    static Transform translation(double tx, double ty, double tz)
    {
        return Transform::translation(Vector3(tx, ty, tz));
    }

    /** @return the basis */
    Matrix3x3 &basis() { return m_basis; }
    /** @return the basis */
    const Matrix3x3 &basis() const { return m_basis; }
    /** @param basis the basis */
    void setBasis(Matrix3x3 basis) { m_basis = std::move(basis); }

    /** @return the origin */
    Vector3 &origin() { return m_origin; }
    /** @return the origin */
    const Vector3 &origin() const { return m_origin; }
    /** @param origin the origin */
    void setOrigin(Vector3 origin) { m_origin = std::move(origin); }

    /** Product-accumulate operator
     *
     * @param rhs The right hand side to multiply and accumulate with the transform
     *
     * @return The product of the transforms
     */
    Transform &operator*=(const Transform &rhs)
    {
        m_origin = transformPosition(rhs.m_origin);
        m_basis *= rhs.m_basis;
        return *this;
    }

    /** Right vector product as a position.
     *
     * @param v Position vector to transform
     *
     * @return Transformed resulting position vector
     */
    Vector3 transformPosition(const Vector3 &v) const
    {
        return {
            m_basis[0].dot(v) + m_origin.x,
            m_basis[1].dot(v) + m_origin.y,
            m_basis[2].dot(v) + m_origin.z};
    }

    /** Rotate a vector.
     *
     * This assumes this transform is a valid rotation transform, e.g. one created by
     * Transform::rotate().
     *
     * @param v The vector to rotate.
     *
     * @return Transformed resulting direction vector
     */
    Vector3 transformDirection(const Vector3 &v) const
    {
        return {
            m_basis[0].dot(v),
            m_basis[1].dot(v),
            m_basis[2].dot(v)};
    }

    /** Inverse transform.
     *
     * @return the inverse transform
     */
    Transform inverse() const
    {
        auto inv = m_basis.transpose();
        return Transform(inv, inv * -m_origin);
    }

#ifndef UH_NOSTL
    inline std::string toString() const;
#endif

private:
    /// \cond EXCLUDE_FROM_DOCS
    Matrix3x3 m_basis = Matrix3x3::identity();
    Vector3 m_origin;
    /// \endcond
};

/** Transform position
 *
 * @param lhs The transform
 * @param rhs The vector to transform
 *
 * @return The transformed position
 */
static inline Vector3 operator*(const Transform &lhs, const Vector3 &rhs)
{
    return lhs.transformPosition(rhs);
}

/** Multiplication operator
 *
 * @param lhs The left hand side to multiply with the transform
 * @param rhs The right hand side to multiply with the transform
 *
 * @return The multiplication of the transforms
 */
static inline Transform operator*(Transform lhs, const Transform &rhs)
{
    lhs *= rhs;
    return lhs;
}

}

#ifndef UH_NOSTL
#include <iostream>
#include <sstream>

namespace Ultrahaptics {
inline std::ostream &operator<<(std::ostream &os, const Transform &t)
{
    os << t.origin() << '\n'
        << t.basis();
    return os;
}

std::string Transform::toString() const
{
    std::stringstream ss;
    ss << *this;
    return ss.str();
}
} // namespace Ultrahaptics
#endif // ifndef UH_NOSTL
