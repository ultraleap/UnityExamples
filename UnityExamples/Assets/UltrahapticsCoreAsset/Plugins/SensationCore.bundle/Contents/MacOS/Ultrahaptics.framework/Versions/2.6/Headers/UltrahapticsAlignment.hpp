
#pragma once

#include "UltrahapticsDecoration.hpp"
#include "UltrahapticsVector3.hpp"

namespace Ultrahaptics
{
   class AlignmentImplementation;
   
   /** \brief Transformation matrix to convert between hand-tracking coordinates and %Ultrahaptics array coordinates.
    * 
    * This class provides functions to convert between the hand-tracking coordinate system (e.g.
    * Leap Motion Controller) and the %Ultrahaptics array coordinate system. The transformation matrix is loaded
    * from an XML file, or the matrix coefficients can be explicitly set.
    * 
    * @note This class is provided as a convenience only. In production systems we expect that you will use your
    *       own alignment matrix configuration system, or simply hard-code the matrix.
    * 
    * See \ref Coordinates.md for more information.
    */
   class UH_API_CLASS_DECORATION Alignment
   {
      public:
         /** Default initialiser. Initialises the alignment to the default
          * setting for the Dragonfly Evaluation Kit. If you are not using
          * Dragonfly (e.g. UHDK5) use Alignment(const char *alignment_load) instead.
          * 
          * @deprecated We do not recommend using this constructor, and it will be removed in a future
          *             version of the SDK. Consider getting an appropriate default alignment file via
          *             DeviceInfo::getDefaultAlignment, or using your own alignment matrix configuration system.
          */
         UH_API_DECORATION Alignment();
         
         /** Initialise the alignment helper. Basic initialisation of the device
          * to tracking coordinates transform matrix. The matrix is constructed
          * as:
          *
          * \f$ \left[ \begin{array}{cccc} x_{00} & x_{01} & x_{02} & x_{03}
          * \\ x_{04} & x_{05} & x_{06} & x_{07} \\ x_{08} & x_{09} & x_{10} &
          * x_{11} \\ 0 & 0 & 0 & 1 \end{array} \right] \left[ \begin{array}{c}
          * x_d \\ y_d \\ z_d \\ 1 \end{array} \right] = \left[ \begin{array}{c}
          * x_t \\ y_t \\ z_t \\ 1 \end{array} \right] \f$
          *
          * where \f$x_d\f$, \f$y_d\f$, \f$z_d\f$ are intended to be the
          * \f$x\f$, \f$y\f$ and \f$z\f$
          * coordinates of a point position in the Ultrahaptics device space, and
          * \f$x_t\f$, \f$y_t\f$ and
          * \f$z_t\f$ are the corresponding coordinates in the tracking space.
          */
         UH_API_DECORATION Alignment(
            float x00, float x01, float x02, float x03,
            float x04, float x05, float x06, float x07,
            float x08, float x09, float x10, float x11);
         
         /** Load an alignment from an XML file. When loading an alignment file
          * it first looks on disk (you can pass a full or relative path), and
          * if no such file is found it looks for files stored in the library.
          * Currently the following files are available in the library:
          * 
          * * `dragonfly.alignment.xml`
          * * `U5_square.alignment.xml`
          * * `U5_rectangle.alignment.xml`
          * * `U5_rectangle_side.alignment.xml`
          * * `USX_square.alignment.xml`
          * 
          * @param alignment_load The file to load the alignment configuration
          * from */
         UH_API_DECORATION Alignment(const char *alignment_load);
         
         /** Copy constructor for alignment.
          *
          * @param other The other instance to copy */
         UH_API_DECORATION Alignment(const Alignment &other);
         
         /** Copy assignment operator for alignment.
          *
          * @param other The other instance to copy
          *
          * @return Itself */
         UH_API_DECORATION Alignment &operator=(const Alignment &other);
         
         /** Destuctor */
         UH_API_DECORATION ~Alignment();
         
         /** Save the alignment to an XML file.
          *
          * @param alignment_save The file to save the alignment configuration
          * to
          *
          * @return True if the save succeeded, false if it did not */
         UH_API_DECORATION bool saveAlignment(const char *alignment_save) const;
         
         /** Load the alignment from an XML file. The format of the file is as follows.
          * 
          * ```
          * <?xml version="1.0" encoding="UTF-8"?>
          * <AlignmentTransforms>
          *    <DeviceToTrackingSpace>
          *       <Row x="1000.0" y="0.0"     z="0.0"    w="0.0"  />
          *       <Row x="0.0"    y="0.0"     z="1000.0" w="0.0"  />
          *       <Row x="0.0"    y="-1000.0" z="0.0"    w="110.5" />
          *       <Row x="0.0"    y="0.0"     z="0.0"    w="1.0"  />
          *    </DeviceToTrackingSpace>
          *    <TrackingToDeviceSpace>
          *       <Row x="0.001" y="0.0"   z="0.0"    w="0.0"    />
          *       <Row x="0.0"   y="0.0"   z="-0.001" w="0.1105" />
          *       <Row x="0.0"   y="0.001" z="0.0"    w="0.0"    />
          *       <Row x="0.0"   y="0.0"   z="0.0"    w="1.0"    />
          *    </TrackingToDeviceSpace>
          * </AlignmentTransforms>
          * ```
          * 
          * The two matrices are the inverse of each other.
          *
          * @param alignment_load The file to load the alignment configuration
          * from
          *
          * @return True if the load succeeded, false if it did not */
         UH_API_DECORATION bool loadAlignment(const char *alignment_load);
         
         /** Transform a direction vector from tracking space to device space.
          *
          * @param tracking_direction The direction in tracking space
          *
          * @return The direction in device space */
         UH_API_DECORATION Vector3 fromTrackingDirectionToDeviceDirection(const Vector3 &tracking_direction) const;
         
         /** Transform a position vector from tracking space to device space.
          *
          * @param tracking_position The position in tracking space
          *
          * @return The position in device space */
         UH_API_DECORATION Vector3 fromTrackingPositionToDevicePosition(const Vector3 &tracking_position) const;
         
         /** Transform a direction vector from device space to tracking space.
          *
          * @param device_direction The direction in device space
          *
          * @return The direction in tracking space */
         UH_API_DECORATION Vector3 fromDeviceDirectionToTrackingDirection(const Vector3 &device_direction) const;
         
         /** Transform a position vector from device space to tracking space.
          *
          * @param device_position The position in device space
          *
          * @return The position in tracking space */
         UH_API_DECORATION Vector3 fromDevicePositionToTrackingPosition(const Vector3 &device_position) const;
         
         /** Put the 4x4 matrix that describes the transformation from tracking
          * space to device space into the array, in row major order.
          *
          * @param output_array The array to put the matrix into. It must have space for 16 elements. */
         UH_API_DECORATION void fromTrackingToDeviceTransformToArray4x4(float *output_array) const;
         
         /** Put the 4x4 matrix that describes the transformation from device
          * space to tracking space into the array, in row major order.
          *
          * @param output_array The array to put the matrix into. It must have space for 16 elements. */
         UH_API_DECORATION void fromDeviceToTrackingTransformToArray4x4(float *output_array) const;
         
         /** Put the 4x4 matrix that describes the transformation from tracking
          * space to device space into the array, in row major order.
          *
          * @param output_array The array to put the matrix into. It must have space for 16 elements. */
         UH_API_DECORATION void fromTrackingToDeviceTransformToArray4x4(double *output_array) const;
         
         /** Put the 4x4 matrix that describes the transformation from device
          * space to tracking space into the array, in row major order.
          *
          * @param output_array The array to put the matrix into. It must have space for 16 elements. */
         UH_API_DECORATION void fromDeviceToTrackingTransformToArray4x4(double *output_array) const;
         
      protected:
         /** Internal implmentation pointer. */
         AlignmentImplementation *alignment;
   };
}

