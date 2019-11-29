#pragma once

#ifndef M_PI
#define M_PI 3.141592653589793238462643383279502
#endif

#include "UltrahapticsVector3.hpp"

#include <cmath>

namespace Ultrahaptics {
class Quaternion;
inline Quaternion operator*(Quaternion lhs, const Quaternion &rhs);
inline Quaternion operator/(Quaternion lhs, float s);

class Quaternion {
public:
    Quaternion() = default;
    Quaternion(float x, float y, float z, float w)
        : m_x(x)
        , m_y(y)
        , m_z(z)
        , m_w(w)
    {
    }

    float x() const { return m_x; }
    float y() const { return m_y; }
    float z() const { return m_z; }
    float w() const { return m_w; }

    float &x() { return m_x; }
    float &y() { return m_y; }
    float &z() { return m_z; }
    float &w() { return m_w; }

    void setRotation(const Ultrahaptics::Vector3 &n, float theta)
    {
        auto f = std::sin(theta / 2.0f) / n.length();
        m_x = n.x * f;
        m_y = n.y * f;
        m_z = n.z * f;
        m_w = std::cos(theta / 2.0f);
    }

    enum class Order {
        XYZ,
        XZY,
        YXZ,
        YZX,
        ZXY,
        ZYX
    };

    /** Set values based on Euler angles (intrinsic, active)
     * @param theta_x rotation around x-axis
     * @param theta_y rotation around y-axis
     * @param theta_z rotation around z-axis
     * @param order the \c Order in which the rotations are applied
     */
    void setEuler(float theta_x, float theta_y, float theta_z, Order order)
    {
        auto cx = std::cos(theta_x / 2.f);
        auto cy = std::cos(theta_y / 2.f);
        auto cz = std::cos(theta_z / 2.f);
        auto sx = std::sin(theta_x / 2.f);
        auto sy = std::sin(theta_y / 2.f);
        auto sz = std::sin(theta_z / 2.f);

        Quaternion qx{sx,  0,  0, cx};
        Quaternion qy{ 0, sy,  0, cy};
        Quaternion qz{ 0,  0, sz, cz};

        switch (order) {
        case Order::XYZ: *this = qx * qy * qz; return;
        case Order::XZY: *this = qx * qz * qy; return;
        case Order::YXZ: *this = qy * qx * qz; return;
        case Order::YZX: *this = qy * qz * qx; return;
        case Order::ZXY: *this = qz * qx * qy; return;
        case Order::ZYX: *this = qz * qy * qx; return;
        }
    }

    static Quaternion fromRotation(const Ultrahaptics::Vector3 &n, float theta)
    {
        Quaternion q;
        q.setRotation(n, theta);
        return q;
    }

    static Quaternion fromEuler(float theta_x, float theta_y, float theta_z, Order order)
    {
        Quaternion q;
        q.setEuler(theta_x, theta_y, theta_z, order);
        return q;
    }

    Quaternion &operator+=(const Quaternion &rhs)
    {
        m_x += rhs.m_x;
        m_y += rhs.m_y;
        m_z += rhs.m_z;
        m_w += rhs.m_w;
        return *this;
    }

    Quaternion operator-() const
    {
        return {-m_x, -m_y, -m_z, -m_w};
    }

    Quaternion &operator-=(const Quaternion &rhs)
    {
        m_x -= rhs.m_x;
        m_y -= rhs.m_y;
        m_z -= rhs.m_z;
        m_w -= rhs.m_w;
        return *this;
    }

    Quaternion &operator*=(const Quaternion &rhs)
    {
        Vector3 l{m_x, m_y, m_z};
        Vector3 r{rhs.m_x, rhs.m_y, rhs.m_z};

        auto cross = l.cross(r);
        auto p = m_w * r + rhs.m_w * l + cross;
        m_x = p.x;
        m_y = p.y;
        m_z = p.z;
        m_w = m_w * rhs.m_w - l.dot(r);
        return *this;
    }

    Quaternion &operator*=(float s)
    {
        m_x *= s;
        m_y *= s;
        m_z *= s;
        m_w *= s;
        return *this;
    }

    Quaternion &operator/=(float s)
    {
        *this *= 1 / s;
        return *this;
    }

    float dot(const Quaternion &rhs) const
    {
        return m_x * rhs.m_x
             + m_y * rhs.m_y
             + m_z * rhs.m_z
             + m_w * rhs.m_w;
    }

    float length2() const
    {
        return dot(*this);
    }

    float length() const
    {
        return std::sqrt(length2());
    }

    Quaternion inverse() const
    {
        return Quaternion{-m_x, -m_y, -m_z, m_w} / length2();
    }

private:
    float m_x = 0.0f;
    float m_y = 0.0f;
    float m_z = 0.0f;
    float m_w = 1.0f;
};

inline Quaternion operator+(Quaternion lhs, const Quaternion &rhs)
{
    lhs += rhs;
    return rhs;
}

inline Quaternion operator-(Quaternion lhs, const Quaternion &rhs)
{
    lhs -= rhs;
    return lhs;
}

inline Quaternion operator*(Quaternion lhs, const Quaternion &rhs)
{
    lhs *= rhs;
    return lhs;
}

inline Quaternion operator*(Quaternion lhs, float s)
{
    lhs *= s;
    return lhs;
}

inline Quaternion operator*(float s, Quaternion rhs)
{
    rhs *= s;
    return rhs;
}

inline Quaternion operator/(Quaternion lhs, float s)
{
    lhs /= s;
    return lhs;
}
} // namespace Ultrahaptics

#ifndef UH_NOSTL
#include <iostream>

namespace Ultrahaptics {
inline std::ostream &operator<<(std::ostream &os, const Quaternion &q)
{
    os << "{" << q.x() << ", " << q.y() << ", " << q.z() << ", " << q.w() << "}";
    return os;
}
} // namespace Ultrahaptics
#endif
