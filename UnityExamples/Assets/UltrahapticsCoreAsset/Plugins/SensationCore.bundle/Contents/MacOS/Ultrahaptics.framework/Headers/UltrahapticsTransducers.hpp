#pragma once

#include "UltrahapticsDecoration.hpp"
#include "UltrahapticsMatrix4x4.hpp"

#if defined(UH_LIBRARY_EXPORT)
#include <vector>
#endif

namespace Ultrahaptics
{

/** \brief Data on individual transducers.
 *
 * Individual transducer data: position, direction and phase multiplier.
 *
 * The phase multiplier is used to account for the transducer wiring polarity and also
 * cases where no transducer is present in a transducer space. So normally it is either
 * 1 for normal wiring ("A"), -1 for transducers with inverted polarity ("B") or 0 for missing
 * transducers ("X"). isEnabled() can be used to check for X.
 */
struct Transducer
{
    /** Default constructor for a disabled transducer at the origin. */
    Transducer()
        : x_position(0.f), y_position(0.f), z_position(0.f), x_upvector(0.f), y_upvector(0.f), z_upvector(0.f), multiplier(0.f)
    {}

    /** Create a Transducer with the given position, direction and phase multiplier.
     *
     * @param xp x position of this transducer in metres
     * @param yp y position of this transducer in metres
     * @param zp z position of this transducer in metres
     * @param xu x component of the normalised emission direction vector of this transducer
     * @param yu y component of the normalised emission direction vector of this transducer
     * @param zu z component of the normalised emission direction vector of this transducer
     * @param m Phase multiplier, to account for inverted or missing transducers. See the documentation for this class for more information. */
    Transducer(float xp, float yp, float zp, float xu, float yu, float zu, float m)
        : x_position(xp), y_position(yp), z_position(zp), x_upvector(xu), y_upvector(yu), z_upvector(zu), multiplier(m)
    {}

    /** X position of this transducer in metres */
    float x_position;
    /** Y position of this transducer in metres */
    float y_position;
    /** Z position of this transducer in metres */
    float z_position;
    /** X component of the normalised emission direction vector of this transducer */
    float x_upvector;
    /** Y component of the normalised emission direction vector of this transducer */
    float y_upvector;
    /** Z component of the normalised emission direction vector of this transducer */
    float z_upvector;
    /** Phase multiplier, to account for inverted or missing transducers. See the documentation for this class for more information. */
    float multiplier;

    /** Determine if this transducer is enabled */
    bool isEnabled() const
    {
        return multiplier != 0.0f;
    }
};

/** \cond EXCLUDE_FROM_DOCS */
#if defined(UH_LIBRARY_EXPORT)
typedef std::vector<Transducer> TransducerContainerImplementation;
#else
class TransducerContainerImplementation;
#endif
/** \endcond */

/** \brief Container for holding physical transducer data.
 *
 * Container for the set of Transducer%s. It provides the same functionality as
 * a std::vector<Transducer> but also allows a callback to be set up that is called
 * whenever the TransducerContainer is modified.
 */
class UH_API_CLASS_DECORATION TransducerContainer
{
public:
    /** Construct a transducer container containing zero transducers */
    UH_API_DECORATION TransducerContainer();

    /** Construct a transducer container containing a set of transducers
     *
     * @param n The number of transducers to copy from the array
     *
     * @param iphysical_transducer_data The array of physical transducer data */
    UH_API_DECORATION TransducerContainer(size_t n, const Transducer *iphysical_transducer_data);

    /** Copy constructor
     *
     * @param other The other transducer container to copy from */
    UH_API_DECORATION TransducerContainer(const TransducerContainer &other);

    /** Copy assignment operator
     *
     * @param other The other transducer container to copy from
     *
     * @return Itself */
    UH_API_DECORATION TransducerContainer &operator=(const TransducerContainer &other);

    /** Destructor. */
    UH_API_DECORATION ~TransducerContainer();

    /** @return Size of container */
    UH_API_DECORATION size_t size() const;

    /** Get a constant reference to a transducers physical attributes
     *
     * @param idx Index of the transducer whose data is to be referenced
     *
     * @return Constant reference to the physical data about a transducer */
    UH_API_DECORATION const Transducer &operator[](size_t idx) const;

    /** Get a constant reference to a transducers physical attributes
     *
     * @param idx Index of the transducer whose data is to be referenced
     *
     * @return Constant reference to the physical data about a transducer */
    UH_API_DECORATION const Transducer &at(size_t idx) const;

    /** @return A constant pointer to the whole array of transducers */
    UH_API_DECORATION const Transducer *data() const;

    /** Remove a range of transducers from the container
     *
     * @param i The place in the indexing to remove the range of transducers from
     *
     * @param n The number of transducers to remove from the array
     *
     * @return True if the update succeeded */
    UH_API_DECORATION bool erase(size_t i, size_t n);

    /** Insert a range of transducers inside the container
     *
     * @param i The place in the indexing to insert the newly updated transducer data
     *
     * @param n The number of transducers to copy from the array
     *
     * @param iphysical_transducer_data The array of physical transducer data
     *
     * @return True if the insert succeeded */
    UH_API_DECORATION bool insert(size_t i, size_t n, const Transducer *iphysical_transducer_data);

    /** Create a new container with the given transformation
     * applied to the physical transducer data
     *
     * @param transform The transformation to apply
     *
     * @return New physical transducer data with transform applied */
    UH_API_DECORATION TransducerContainer operator*(const Matrix4x4 &transform) const;

    /** Apply transformation to physical transducer data in this container
     *
     * @param transform The transformation to apply
     *
     * @return Itself */
    UH_API_DECORATION TransducerContainer &operator*=(const Matrix4x4 &transform);

private:
    /** Array of physical transducer data */
    TransducerContainerImplementation *impl;
};

}
